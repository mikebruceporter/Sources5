using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sources5.Models;
using System.Linq;
namespace Sources5.Infrastructure
{
    public class CustomPasswordValidator : PasswordValidator<ApplicationUser>
    {
        public override async Task<IdentityResult> ValidateAsync(
            UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            IdentityResult result = await base.ValidateAsync(manager, user, password);

            List<IdentityError> errors = result.Succeeded ?
                new List<IdentityError>() : result.Errors.ToList();

            if (password.Contains("1234") || password.Contains("pass"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsSequence",
                    Description = "Password cannot contain the numeric sequence 1234 or the characters pass"
                });
            }

            return errors.Count == 0 ? IdentityResult.Success
            : IdentityResult.Failed(errors.ToArray());
        }
    }
}
