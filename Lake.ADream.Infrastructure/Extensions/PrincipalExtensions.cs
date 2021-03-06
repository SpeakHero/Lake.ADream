﻿//namespace System.Security.Claims
//{

//    /// <summary>
//    /// Claims related extensions for <see cref="T:System.Security.Claims.ClaimsPrincipal" />.
//    /// </summary>
//    public static class PrincipalExtensions
//    {
//        /// <summary>
//        /// Returns the value for the first claim of the specified type otherwise null the claim is not present.
//        /// </summary>
//        /// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> instance this method extends.</param>
//        /// <param name="claimType">The claim type whose first value should be returned.</param>
//        /// <returns>The value of the first instance of the specified claim type, or null if the claim is not present.</returns>
//        public static string FindFirstValue(this ClaimsPrincipal principal, string claimType)
//        {
//            if (principal == null)
//            {
//                throw new ArgumentNullException("principal");
//            }
//            Claim claim = principal.FindFirst(claimType);
//            return claim?.Value;
//        }
//    }

//}
