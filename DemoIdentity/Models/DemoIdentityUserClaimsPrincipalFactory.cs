using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DemoIdentity.Models
{
    public class DemoIdentityUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<DemoIdentityUser>
    {
        public DemoIdentityUserClaimsPrincipalFactory(UserManager<DemoIdentityUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(DemoIdentityUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("locale", user.Locale));
            return identity;
        }
    }
}
