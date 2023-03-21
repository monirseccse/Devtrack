using DevTrack.Api.Model;
using DevTrack.Infrastructure;
using DevTrack.Infrastructure.Entities;
using DevTrack.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevTrack.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly ITimeService timeservice;
        public readonly SignInManager<ApplicationUser> signInManager;
        public AccountsController(SignInManager<ApplicationUser> _signInManager,
                                UserManager<ApplicationUser> _userManager,
                                ITokenService _tokenService, ITimeService _timeservice)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            tokenService = _tokenService;
            timeservice = _timeservice;
        }

        [HttpPost("Login")]
        //[ValidateAntiForgeryToken] It's doesn'twork. Because Devskill team doesn't implement
        //this feature to their tool
        public async Task<LoginResponse> Post(LoginModel loginModel)
        {
            var loginResponse = new LoginResponse();

            string email = loginModel.Email;
            string password = loginModel.Password;

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var person = await userManager.FindByEmailAsync(email);
                bool Correctpassword = false;

                if (person != null && !string.IsNullOrEmpty(password))
                {
                    var result = await signInManager.CheckPasswordSignInAsync(person, password, true);
                    if (result.Succeeded)
                    {
                        Correctpassword = true;
                    }
                    else
                        Correctpassword = false;
                }

                if (person != null && Correctpassword)
                {
                    var claims = (await userManager.GetClaimsAsync(person)).ToList();                   
                    var idClaim = new Claim("userId", person.Id.ToString());
                    claims.Add(idClaim);
                    var token = tokenService.GetToken(claims);

                    loginResponse.data = new Data()
                    {
                        token = token,
                        email = person.Email,
                        name = person.Name,
                        expireDate = timeservice.Now.AddDays(7),
                        userId = person.Id
                    };

                    loginResponse.statusCode = 200;
                    loginResponse.isSuccess = true;
                    loginResponse.errors = new List<string>() { "Success" };
                }
                else
                {
                    if (person == null)
                    {
                        loginResponse.statusCode = 404;
                        loginResponse.errors = new List<string>() { "Account not found" };
                        loginResponse.isSuccess = false;
                    }
                    else if (!Correctpassword)
                    {
                        loginResponse.statusCode = 401;
                        loginResponse.errors = new List<string>() { "Password does't match" };
                        loginResponse.isSuccess = false;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    loginResponse.statusCode = 404;
                    loginResponse.errors = new List<string>() { "Email is required" };
                    loginResponse.isSuccess = false;
                }
                else if (string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email))
                {
                    loginResponse.statusCode = 404;
                    loginResponse.errors = new List<string>() { "Password is required" };
                    loginResponse.isSuccess = false;
                }
                else
                {
                    loginResponse.statusCode = 404;
                    loginResponse.errors = new List<string>() { "Please insert Email and Password" };
                    loginResponse.isSuccess = false;
                }
            }

            return loginResponse;

        }
    }
}
