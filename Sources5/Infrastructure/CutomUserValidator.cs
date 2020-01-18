using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sources5.Models;
namespace Sources5.Infrastructure
{
    public class CustomUserValidator : UserValidator<ApplicationUser>
    {
        public override async Task<IdentityResult> ValidateAsync(
            UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(manager, user);

            List<IdentityError> errors = result.Succeeded ?
                new List<IdentityError>() : result.Errors.ToList();

            // For now we'll comment out this code...
            //if (!user.Email.ToLower().EndsWith("@example.com"))
            //{
            //    errors.Add(new IdentityError
            //    {
            //        Code = "EmailDomainError",
            //        Description = "Only example.com email addresses are allowed"
            //    });
            //}



            return errors.Count == 0 ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray());





            // in the real world we would switch this to disallow example.com
            // example.org and example.net
        }
    }
}
