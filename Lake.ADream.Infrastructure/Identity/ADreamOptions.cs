using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Identity
{

    /// <summary>
    /// 表示可用于配置标识系统的所有选项。
    /// </summary>
    public class ADreamOptions
    {

        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.ClaimsIdentityOptions" /> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.ClaimsIdentityOptions" /> for the identity system.
        /// </value>
        public ADreamClaimsIdentityOptions ClaimsIdentity
        {
            get;
            set;
        } = new ADreamClaimsIdentityOptions();


        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.UserOptions" /> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.UserOptions" /> for the identity system.
        /// </value>
        public ADreamUserOptions User
        {
            get;
            set;
        } = new ADreamUserOptions();


        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.PasswordOptions" /> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.PasswordOptions" /> for the identity system.
        /// </value>
        public ADreamPasswordOptions Password
        {
            get;
            set;
        } = new ADreamPasswordOptions();


        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.LockoutOptions" /> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.LockoutOptions" /> for the identity system.
        /// </value>
        public ADreamLockoutOptions Lockout
        {
            get;
            set;
        } = new ADreamLockoutOptions();


        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.SignInOptions" /> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.SignInOptions" /> for the identity system.
        /// </value>
        public ADreamSignInOptions SignIn
        {
            get;
            set;
        } = new ADreamSignInOptions();


        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.TokenOptions" /> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.TokenOptions" /> for the identity system.
        /// </value>
        public ADreamTokenOptions Tokens
        {
            get;
            set;
        } = new ADreamTokenOptions();
    }

}
