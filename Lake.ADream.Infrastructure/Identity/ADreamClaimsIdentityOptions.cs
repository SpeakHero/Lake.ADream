using System;
using Microsoft.AspNetCore.Identity;

namespace Lake.ADream.Infrastructure.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class ADreamClaimsIdentityOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public ADreamClaimsIdentityOptions()
        {
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            UserNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            UserIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            SecurityStampClaimType = "AspNet.Identity.SecurityStamp";
        }


        /// <summary>
        /// 
        /// </summary>
        public string RoleClaimType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserNameClaimType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserIdClaimType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SecurityStampClaimType { get; set; }
    }
}