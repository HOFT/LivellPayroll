using LivellPayRoll.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace LivellPayRoll.Infrastructure
{
    public class ApplicationSignInManager : SignInManager<AppUser, string>
    {
        public ApplicationSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager)
        : base(userManager, authenticationManager)
        {
        }
        public override Task<SignInStatus> PasswordSignInAsync(string userName,string password,bool isPersistent,bool shouldLockout)
        {

            return base.PasswordSignInAsync(userName,
                                            password,
                                            isPersistent,
                                            shouldLockout);
        }
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }
    }
}