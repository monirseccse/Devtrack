using Autofac;
using DevTrack.Infrastructure.Exceptions;
using DevTrack.Infrastructure.Services.Encryption;
using DevTrack.Web.Areas.App.Models.Projects;
using DevTrack.Web.Codes;
using DevTrack.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Text.Json;

namespace DevTrack.Web.Areas.App.Controllers
{
    [Area("App")]
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly ILifetimeScope _scope;

        public ProjectController(ILogger<ProjectController> logger, ILifetimeScope scope,
            ISymmetricEncryptionService symmetricEncryptionService)
        {
            _logger = logger;
            _scope = scope;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = _scope.Resolve<ProjectCreateModel>();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateModel model)
        {
            if (ModelState.IsValid)
            {
                model.ResolveDependency(_scope);
                try
                {
                    var projectGuid = await model.CreateAsync();
                    if (projectGuid == null)
                    {
                        return View(model);
                    }
                    else
                    {
                        TempData["ProjectID"] = projectGuid;
                        TempData["ProjectTitle"] = model.Title;
                        TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                        {
                            Message = "Successfully created project - " + model.Title,
                            Type = ResponseTypes.Success
                        });
                    }
                }
                catch (DuplicateException ioe)
                {
                    _logger.LogError(ioe, ioe.Message);
                    ModelState.AddModelError("", ioe.Message);
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = ioe.Message,
                        Type = ResponseTypes.Danger
                    });
                    return View(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "There was a problem in creating project.",
                        Type = ResponseTypes.Danger
                    });
                    return View(model);
                }
            }
            return RedirectToAction(nameof(Invite));
        }

        public IActionResult Invite()
        {
            var model = _scope.Resolve<InvitationCreateModel>();
            if (TempData.Peek("ProjectTitle") == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                model.ProjectName = (string)TempData.Peek("ProjectTitle");
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(InvitationCreateModel model)
        {
            if (ModelState.IsValid)
            {
                model.ResolveDependency(_scope);
                await model.CreateInvitationsAsync((Guid)TempData["ProjectID"]);
                TempData.Remove("ProjectTitle");
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Invitations Sent",
                    Type = ResponseTypes.Success
                });
            }
            else
            {
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> JoinProject(string invitationId, string projectId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var invitationModel = _scope.Resolve<InvitationModel>();
                invitationModel.ResolveDependency(_scope);

                var projectUserCreateModel = _scope.Resolve<ProjectUserCreateModel>();
                projectUserCreateModel.ResolveDependency(_scope);

                if (HttpContext.Session.GetString("invitationId") != null)
                {
                    invitationModel.InvitationId = HttpContext.Session.GetString("invitationId");
                    projectUserCreateModel.ProjectId = HttpContext.Session.GetString("invitationProjectId");
                }
                else
                {
                    invitationModel.InvitationId = invitationId;
                    projectUserCreateModel.ProjectId = projectId;
                }

                await invitationModel.LoadInvitation();
                if(invitationModel.Status != Infrastructure.Codes.InvitationStatus.Accepted)
                {
                    if(invitationModel.Email == User.Identity.Name)
                    {
                        projectUserCreateModel.ApplicationUserId = HttpContext.User.Claims.FirstOrDefault().Value;
                        await projectUserCreateModel.CreateProjectUserAsync();

                        await invitationModel.UpdateStatusToAccepted();

                        TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                        {
                            Message = "Successfully Joined project",
                            Type = ResponseTypes.Success
                        });
                    }
                }

                HttpContext.Session.Clear();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                HttpContext.Session.SetString("invitationId", invitationId);
                HttpContext.Session.SetString("invitationProjectId", projectId);
                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        public JsonResult GetProjectData(string status)
        {
            var dataTableModel =new DataTablesAjaxRequestModel(Request);
            var model =_scope.Resolve<ProjectListModel>();
            bool.TryParse(status, out bool stat);
            return Json(model.GetPagedProjects(dataTableModel, stat));
        }

        public IActionResult ManageInvitations(Guid projectId, string projectName)
        {
            var model = _scope.Resolve<InvitationCreateModel>();
            model.ProjectId = projectId;
            model.ProjectName = projectName;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> InviteAjax(InvitationCreateModel model)
        {
            if (ModelState.IsValid)
            {
                model.ResolveDependency(_scope);
                await model.CreateInvitationsAsync(model.ProjectId);
            }
            else
            {
                var errors = ModelState.Keys.Where(k => ModelState[k]?.Errors.Count > 0)
                        .Select(k => new { propertyName = k, errorMessage = ModelState[k]?.Errors[0].ErrorMessage });
                return BadRequest(errors?.FirstOrDefault()?.errorMessage);
            }
            return this.Ok($"Form Data received");
        }

        public async Task<object> GetTeamMembers(Guid projectId)
        {
            var model = _scope.Resolve<TeamMembersListModel>();
            var json = await model.GetTeamMembersAsync(projectId);
            return JsonSerializer.Serialize(json);
        }

        public async Task<IActionResult> Archive(Guid id)
        {
            var archiveModel = _scope.Resolve<ProjectArchiveModel>();
            var editModel = _scope.Resolve<ProjectEditModel>();

            var isOwner = await editModel.IsProjectOwner(id);
            if (isOwner)
            {
                await archiveModel.ProjectArchive(id, true);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Project Archived",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction(nameof(Index));
            }
            else
                return RedirectToAction(nameof(AccessDenied));
        }

        public async Task<IActionResult> Active(Guid id)
        {
            var archiveModel = _scope.Resolve<ProjectArchiveModel>();
            var editModel = _scope.Resolve<ProjectEditModel>();

            var isOwner = await editModel.IsProjectOwner(id);
            if (isOwner)
            {
                await archiveModel.ProjectArchive(id, false);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Project Activated",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction(nameof(Index));
            }
            else
                return RedirectToAction(nameof(AccessDenied));
        }

        public async Task<IActionResult> Edit(Guid projectId)
        {
            var model = _scope.Resolve<ProjectEditModel>();
            model.ResolveDependency(_scope);
            var isOwner = await model.IsProjectOwner(projectId);
            if (isOwner)
            {
                await model.LoadProject(projectId);
                return View(model);
            }
            else
                return RedirectToAction(nameof(AccessDenied));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectEditModel model)
        {
            var resolve = _scope.Resolve<ProjectEditModel>();
            try
            {
                await resolve.UpdateProject(model);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Project Updated Successfully",
                    Type = ResponseTypes.Success
                });
            }
            catch (DuplicateException ioe)
            {
                _logger.LogError(ioe, ioe.Message);
                ModelState.AddModelError("", ioe.Message);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = ioe.Message,
                    Type = ResponseTypes.Danger
                });
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in updating project.");
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "There was a problem in updating project.",
                    Type = ResponseTypes.Danger
                });
                return View(model);
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(Guid projectId, Guid userId)
        {
            var model = _scope.Resolve<TeamMembersListModel>();
            var projectName = await model.RemoveMember(projectId, userId);

            return RedirectToAction(nameof(ManageInvitations), new { projectId = projectId, projectName = projectName });
        }

        public async Task<object> GetOwnerProjectIdListAjax()
        {
            var model = _scope.Resolve<ProjectListModel>();
            var jsonData = await model.GetOwnerProjectIdListAsync();
            return JsonSerializer.Serialize(jsonData);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
