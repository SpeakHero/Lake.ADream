using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Identity
{
    /// <summary>
    /// Options for user tokens.
    /// </summary>
    public class TokenOptions
    {
        /// <summary>
        ///默认令牌提供程序名称，用于电子邮件确认、密码重置和更改电子邮件。
        /// </summary>
        public static readonly string DefaultProvider = "Default";

        /// <summary>
        /// 默认的令牌提供程序名称由电子邮件提供程序使用。/&gt;.
        /// </summary>
        public static readonly string DefaultEmailProvider = "Email";

        /// <summary>
        /// 电话提供商使用的默认令牌提供程序名称。/&gt;.
        /// </summary>
        public static readonly string DefaultPhoneProvider = "Phone";

        /// <summary>
        /// 默认令牌提供程序名称<see cref="P:Microsoft.AspNetCore.Identity.TokenOptions.AuthenticatorTokenProvider" />.
        /// </summary>
        public static readonly string DefaultAuthenticatorProvider = "Authenticator";

        /// <summary>
        ///将用于使用提供程序名称的密钥构造用户令牌提供程序。
        /// </summary>
        public Dictionary<string, TokenProviderDescriptor> ProviderMap
        {
            get;
            set;
        } = new Dictionary<string, TokenProviderDescriptor>();


        /// <summary>
        ///获取或设置用于在帐户确认电子邮件中生成令牌的令牌提供程序。
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.IUserTwoFactorTokenProvider`1" /> 用于生成用于帐户确认电子邮件的令牌。
        /// </value>
        public string EmailConfirmationTokenProvider
        {
            get;
            set;
        } = DefaultProvider;


        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Identity.IUserTwoFactorTokenProvider`1" /> 用于生成密码重置电子邮件中使用的令牌。
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.IUserTwoFactorTokenProvider`1" /> 用于生成密码重置电子邮件中使用的令牌。
        /// </value>
        public string PasswordResetTokenProvider
        {
            get;
            set;
        } = DefaultProvider;


        /// <summary>
        /// Gets or sets the <see cref="P:Microsoft.AspNetCore.Identity.TokenOptions.ChangeEmailTokenProvider" /> 用于生成电子邮件更改确认电子邮件中使用的令牌。
        /// </summary>
        /// <value>
        /// The <see cref="P:Microsoft.AspNetCore.Identity.TokenOptions.ChangeEmailTokenProvider" /> 用于生成电子邮件更改确认电子邮件中使用的令牌。
        /// </value>
        public string ChangeEmailTokenProvider
        {
            get;
            set;
        } = DefaultProvider;


        /// <summary>
        /// Gets or sets the <see cref="P:Microsoft.AspNetCore.Identity.TokenOptions.ChangePhoneNumberTokenProvider" /> 用于生成更改电话号码时使用的令牌。
        /// </summary>
        /// <value>
        /// The <see cref="P:Microsoft.AspNetCore.Identity.TokenOptions.ChangePhoneNumberTokenProvider" />用于生成更改电话号码时使用的令牌。
        /// </value>
        public string ChangePhoneNumberTokenProvider
        {
            get;
            set;
        } = GetDefaultChangePhoneNumberTokenProvider();


        /// <summary>
        /// Gets or sets the <see cref="P:Microsoft.AspNetCore.Identity.TokenOptions.AuthenticatorTokenProvider" /> 使用验证器验证两因子签到。
        /// </summary>
        /// <value>
        /// The <see cref="P:Microsoft.AspNetCore.Identity.TokenOptions.AuthenticatorTokenProvider" /> 使用验证器验证两因子签到。
        /// </value>
        public string AuthenticatorTokenProvider
        {
            get;
            set;
        } = DefaultAuthenticatorProvider;


        private static string GetDefaultChangePhoneNumberTokenProvider()
        {
            if (!(AppContext.TryGetSwitch("Microsoft.AspNetCore.Identity.ChangePhoneNumberTokenProvider1483", out bool flag) & flag))
            {
                return DefaultPhoneProvider;
            }
            return DefaultProvider;
        }
    }

}
