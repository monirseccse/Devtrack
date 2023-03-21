using Autofac;
using DevTrack.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting.Internal;
using System.ComponentModel.DataAnnotations;

namespace DevTrack.Web.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; } 
        public string? Image { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password constrains missmatch", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password & confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string? ReturnUrl { get; set; }
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        private IImageService _imageService;

        public RegisterModel()
        {

        }

        public RegisterModel(ImageService imageService)
        {
            _imageService = imageService;
        }
        
        public void ResolveDependency(ILifetimeScope scope)
        {
            _imageService = scope.Resolve<IImageService>();
        }
        public async Task SaveProfilePictureAsync()
        {
            var imgExtension = Path.GetExtension(ImageFile.FileName);

            if (imgExtension == ".jpg" || imgExtension == ".png" || imgExtension == ".jpeg")
            {               
                Image = await _imageService.UploadProfilePictureAsync(ImageFile);
            }
        }        
    }
}
