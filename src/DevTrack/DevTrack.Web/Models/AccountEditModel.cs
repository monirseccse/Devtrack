using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;

namespace DevTrack.Web.Models
{
    public class AccountEditModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Image { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

        private IApplicationUserService _applicationUserService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor? _httpContextAccessor;
        private IImageService _imageService;

        public AccountEditModel()
        {

        }

        public AccountEditModel(IApplicationUserService applicationUserService, 
            IProjectService projectService, IMapper mapper, IHttpContextAccessor? httpContextAccessor,
            IImageService imageService)
        {
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _imageService = imageService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _applicationUserService = _scope.Resolve<IApplicationUserService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
            _imageService = _scope.Resolve<IImageService>();
        }

        internal async Task LoadAccount()
        {
            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.
                User.Claims.FirstOrDefault().Value);

            var account = await _applicationUserService.GetByUserIdAsync(currentUserId);
            if (account != null)
            {
                _mapper.Map(account, this);
            }
        }


        internal async Task EditAccount()
        {
            var user = _mapper.Map<ApplicationUser>(this);
            await _applicationUserService.EditAccountAsync(user);
        }

        public async Task SaveProfilePicture()
        {
            var imgExtension = Path.GetExtension(ImageFile.FileName);

            if (imgExtension == ".jpg" || imgExtension == ".png" || imgExtension == ".jpeg")
            {
                Image = await _imageService.UploadProfilePictureAsync(ImageFile);
            }
        }
    }
}
