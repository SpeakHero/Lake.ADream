using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Identity
{

    /// <summary>
    /// 表示可用于配置标识系统的所有选项。
    /// </summary>
    public class IdentityOptions
    {

        /// <summary>
        ///获取或设置 <see cref="T:Microsoft.AspNetCore.Identity.UserOptions" /> 身份系统。
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.UserOptions" /> 身份系统。
        /// </value>
        public UserOptions User
        {
            get;
            set;
        } = new UserOptions();


        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.PasswordOptions" /> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.PasswordOptions" /> for the identity system.
        /// </value>
        public PasswordOptions Password
        {
            get;
            set;
        } = new PasswordOptions();


        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.LockoutOptions" /> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.LockoutOptions" /> for the identity system.
        /// </value>
        public LockoutOptions Lockout
        {
            get;
            set;
        } = new LockoutOptions();


        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.SignInOptions" /> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.SignInOptions" /> for the identity system.
        /// </value>
        public SignInOptions SignIn
        {
            get;
            set;
        } = new SignInOptions();


        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.TokenOptions" /> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.TokenOptions" /> for the identity system.
        /// </value>
        public TokenOptions Tokens
        {
            get;
            set;
        } = new TokenOptions();

    }

}
