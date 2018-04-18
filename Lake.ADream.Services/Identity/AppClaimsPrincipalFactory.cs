using Lake.ADream.Infrastructure.Identity;
using Lake.ADream.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lake.ADream.Services.Identity
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    public class ADreamClaimsPrincipalFactory : IUserClaimsPrincipalFactory<User>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="options"></param>
        public ADreamClaimsPrincipalFactory(UserManagerService userManager)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Identity.UserClaimsPrincipalFactory`1" /> class.
        /// </summary>
        /// <param name="userManager">The <see cref="T:Microsoft.AspNetCore.Identity.UserManager`1" /> to retrieve user information from.</param>
        /// <param name="optionsAccessor">The configured <see cref="T:Microsoft.AspNetCore.Identity.IdentityOptions" />.</param>
        public ADreamClaimsPrincipalFactory(UserManagerService userManager, IOptions<ADreamOptions> optionsAccessor)
        {
            userManager.CheakArgument();
            optionsAccessor.CheakArgument();
            if (optionsAccessor.Value != null)
            {
                UserManager = userManager;
                Options = optionsAccessor.Value;
            }
        }
        /// <summary>
        /// Gets the <see cref="T:Microsoft.AspNetCore.Identity.UserManager`1" /> for this factory.
        /// </summary>
        /// <value>
        /// The current <see cref="T:Microsoft.AspNetCore.Identity.UserManager`1" /> for this factory instance.
        /// </value>
        public UserManagerService UserManager
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="T:Microsoft.AspNetCore.Identity.IdentityOptions" /> for this factory.
        /// </summary>
        /// <value>
        /// The current <see cref="T:Microsoft.AspNetCore.Identity.IdentityOptions" /> for this factory instance.
        /// </value>
        public ADreamOptions Options
        {
            get;
            private set; 
        }
        public virtual async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            user.CheakArgument();
            return new ClaimsPrincipal(await GenerateClaimsAsync(user));
        }

        /// <summary>
        /// Generate the claims for a user.
        /// </summary>
        /// <param name="user">The user to create a <see cref="T:System.Security.Claims.ClaimsIdentity" /> from.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous creation operation, containing the created <see cref="T:System.Security.Claims.ClaimsIdentity" />.</returns>
        protected virtual async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            string userId = await UserManager.GetUserIdAsync(user);
            var finduser = await UserManager.FindByPhoneOrEmailOrNameAsync(user);
            string value = finduser.UserName ?? finduser.Email ?? finduser.PhoneNumber;
            ClaimsIdentity id = new ClaimsIdentity("Identity.Application", Options.ClaimsIdentity.UserNameClaimType, Options.ClaimsIdentity.RoleClaimType);
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, value));
            if (UserManager.SupportsUserSecurityStamp)
            {
                ClaimsIdentity claimsIdentity = id;
                string securityStampClaimType = Options.ClaimsIdentity.SecurityStampClaimType;
                claimsIdentity.AddClaim(new Claim(type:securityStampClaimType, await UserManager.GetSecurityStampAsync(user)));
            }
            if (UserManager.SupportsUserClaim)
            {
                ClaimsIdentity claimsIdentity = id;
                claimsIdentity.AddClaims(await this.UserManager.GetClaimsAsync(user));
            }
            return id;
        }
    }
}
