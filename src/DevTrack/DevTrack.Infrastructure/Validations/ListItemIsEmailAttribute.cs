using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace DevTrack.Infrastructure.Validations
{
    public class ListItemIsEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var list = value as IList;
            var invalidIndex = -1;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
                    var trimmedEmail = list[i].ToString().Trim();
                    if (trimmedEmail.EndsWith("."))
                    {
                        invalidIndex = i+1;
                    }
                    else
                    {
                        try
                        {
                            var emailAddress = new MailAddress(trimmedEmail);
                        }
                        catch
                        {
                            invalidIndex = i+1;
                        }
                    }
                }
            }
            if(invalidIndex == -1)
                return ValidationResult.Success;
            else
                return new ValidationResult($"Email is Invalid for Member {invalidIndex}");
        }
    }
}
