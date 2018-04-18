using Lake.ADream.EntityFrameworkCore;
using Lake.ADream.Infrastructure.Identity;
using Lake.ADream.Models.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Lake.ADream.Services
{
    public class SignInManagerService
    {
        public SignInManagerService(UserManagerService userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<ADreamOptions> optionsAccessor, ILogger<SignInManagerService> logger, IAuthenticationSchemeProvider schemes)
        {
            UserManager.CheakArgument();
            contextAccessor.CheakArgument();
            optionsAccessor.CheakArgument();
            schemes.CheakArgument();
            UserManager = userManager;
            Context = contextAccessor.HttpContext;
            Options = optionsAccessor?.Value;
            Logger = logger;
            Schemes = schemes;
        }
        public User User { get => UserManager.User; }
        public UserManagerService UserManager { get; }
        public ILogger<SignInManagerService> Logger { get; }
        public IAuthenticationSchemeProvider Schemes { get; }
        public IUserClaimsPrincipalFactory<User> ClaimsFactory
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> used.
        /// </summary>
        public HttpContext Context
        {
            get;
            set;
        }
        internal class TwoFactorAuthenticationInfo
        {
            public string UserId
            {
                get;
                set;
            }

            public string LoginProvider
            {
                get;
                set;
            } = "LoginProvider";
        }

        private const string LoginProviderKey = "LoginProvider";

        private const string XsrfKey = "XsrfId";
        public ADreamOptions Options { get; }
        /// <summary>
        /// Creates a <see cref="T:System.Security.Claims.ClaimsPrincipal" /> for the specified <paramref name="user" />, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create a <see cref="T:System.Security.Claims.ClaimsPrincipal" /> for.</param>
        /// <returns>The task object representing the asynchronous operation, containing the ClaimsPrincipal for the specified user.</returns>
        public virtual async Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user)
        {
            return await ClaimsFactory.CreateAsync(user);
        }
        /// <summary>
        /// 如果主体与应用程序cookie身份具有身份，则返回true
        /// </summary>
        /// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> instance.</param>
        /// <returns>如果用户以身份登录，则为true。</returns>
        public virtual bool IsSignedIn(ClaimsPrincipal principal)
        {
            principal.CheakArgument();
            if (principal?.Identities != null)
            {
                return principal.Identities.Any(i => i.AuthenticationType == IdentityConstants.ApplicationScheme);
            }
            return false;
        }

        /// <summary>
        /// 返回指示指定用户是否可以登录的标志。
        /// </summary>
        /// <param name="user">应该返回登录状态的用户。</param>
        /// <returns>
        /// 表示异步操作的任务对象，其中包含一个为真的标志。
        /// 如果指定的用户可以登录，否则为false。
        /// </returns>
        public virtual async Task<bool> CanSignInAsync(User user)
        {
            bool flag = Options.SignIn.RequireConfirmedEmail;
            if (flag)
            {
                if (user == null && User != null)
                {
                    flag = User.EmailConfirmed;
                }
                else
                {
                    flag = !(await UserManager.EmailConfirmedAsync(user.Email));
                }
            }
            if (flag)
            {
                return false;
            }
            flag = Options.SignIn.RequireConfirmedPhoneNumber;
            if (flag)
            {
                if (user == null && User != null)
                {
                    flag = User.PhoneNumberConfirmed;
                }
                else
                {
                    flag = !(await UserManager.PhoneNumberConfirmedAsync(user.PhoneNumber));
                }
            }
            if (flag)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///刷新登录
        /// 认证性能一样记得，作为一个异步操作。
        /// </summary>
        /// <param name="user">应刷新登录cookie中的用户。</param>
        /// <returns>表示异步操作的任务对象。</returns>
        public virtual async Task RefreshSignInAsync(User user)
        {
            AuthenticateResult authenticateResult = await Context.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            string authenticationMethod = authenticateResult?.Principal?.FindFirstValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod");
            await SignInAsync(user, authenticateResult?.Properties, authenticationMethod);
        }

        /// <summary>
        /// 指定的标志 <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to sign-in.</param>
        /// <param name="isPersistent">记住用户</param>
        /// <param name="authenticationMethod">用于验证用户的方法的名称。</param>
        /// <returns>表示异步操作的任务对象。.</returns>
        public virtual Task SignInAsync(User user, bool isPersistent, string authenticationMethod = null)
        {
            return SignInAsync(user, new AuthenticationProperties
            {
                IsPersistent = isPersistent
            }, authenticationMethod);
        }

        /// <summary>
        /// Signs in the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to sign-in.</param>
        /// <param name="authenticationProperties">Properties applied to the login and authentication cookie.</param>
        /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task SignInAsync(User user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            ClaimsPrincipal claimsPrincipal = await CreateUserPrincipalAsync(user);
            if (authenticationMethod != null)
            {
                Enumerable.First(claimsPrincipal.Identities).AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", authenticationMethod));
            }
            await Context.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal, authenticationProperties ?? new AuthenticationProperties());
        }

        /// <summary>
        ///退出登录。
        /// </summary>
        public virtual async Task SignOutAsync()
        {
            await Context.SignOutAsync(IdentityConstants.ApplicationScheme);
            await Context.SignOutAsync(IdentityConstants.ExternalScheme);
            await Context.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
        }

        /// <summary>
        /// 验证安全标记 <paramref name="principal" /> 反对
        ///作为异步操作的当前用户的持久化标记。
        /// </summary>
        /// <param name="principal">印章应该被验证的校长。</param>
        /// <returns>The task object representing the asynchronous operation. The task will contain the <typeparamref name="User" />
        /// if the stamp matches the persisted value, otherwise it will return false.</returns>
        public virtual async Task<User> ValidateSecurityStampAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }
            User user = await UserManager.GetUserAsync(principal);
            if (user != null && UserManager.SupportsUserSecurityStamp)
            {
                string a = principal.FindFirstValue(Options.ClaimsIdentity.SecurityStampClaimType);
                if (a == await UserManager.GetSecurityStampAsync(user))
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// 使用密码登录应用<paramref name="user" /> and <paramref name="password" /> combination
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">用户模型.</param>
        /// <param name="password">密码</param>
        /// <param name="isPersistent">持久化,记住用户</param>
        /// <param name="lockoutOnFailure">如果真 将开启登录次数失败的锁定.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="ADreamSignInResult" />
        /// for the sign-in attempt.</returns>
        public virtual async Task<ADreamSignInResult> PasswordSignInAsync(User user, string password, bool isPersistent = true, bool lockoutOnFailure = true)
        {
            user.CheakArgument();
            ADreamSignInResult signin = await CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            return (!signin.Succeeded) ? signin : (await SignInOrTwoFactorAsync(user, isPersistent, null, false));
        }

        /// <summary>
        /// Attempts to sign in the specified <paramref name="userName" /> and <paramref name="password" /> combination
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="userName">The user name to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="lockoutOnFailure">登录失败是否被锁定.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="ADreamSignInResult" />
        /// for the sign-in attempt.</returns>
        public virtual async Task<ADreamSignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent = true, bool lockoutOnFailure = true)
        {
            User val = await UserManager.FindByNameAsync(userName, selector => new User { Id = selector.Id, UserName = selector.UserName, PhoneNumber = selector.PhoneNumber, Email = selector.Email, SecurityStamp = selector.SecurityStamp, TimeSpan = selector.TimeSpan, TwoFactorEnabled = selector.TwoFactorEnabled, EmailConfirmed = selector.EmailConfirmed, AccessFailedCount = selector.AccessFailedCount, PhoneNumberConfirmed = selector.PhoneNumberConfirmed, LockoutEnabled = selector.LockoutEnabled, LockoutEnd = selector.LockoutEnd, ConcurrencyStamp = selector.SecurityStamp, PasswordHash = selector.PasswordHash, Sex = selector.Sex });
            if (val == null)
            {
                return ADreamSignInResult.Failed;
            }
            return await PasswordSignInAsync(val, password, isPersistent, lockoutOnFailure);
        }

        /// <summary>
        /// 检测用户的密码
        /// </summary>
        /// <param name="user">先从数据库得到的user模型</param>
        /// <param name="password">登录时候检测用户密码</param>
        /// <param name="lockoutOnFailure">登录失败是否被锁定.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="ADreamSignInResult" />
        /// for the sign-in attempt.</returns>
        /// <returns></returns>
        public virtual async Task<ADreamSignInResult> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure = true)
        {
            user.CheakArgument();
            ADreamSignInResult ADreamSignInResult = await PreSignInCheck(user);
            if (ADreamSignInResult != null)
            {
                ///没有激活邮箱 手机 被锁定 就结束登录
                return ADreamSignInResult;
            }
            if (UserManager.CheckPassword(user, password))
            {
                //登录成功重置登录失败的次数和时间
                await ResetLockoutCount(user);
                return ADreamSignInResult.Success;
            }
            ///开启了登录失败锁定
            if (UserManager.SupportsUserLockout & lockoutOnFailure)
            {
                ///更新失败次数
                await UserManager.AccessFailedAsync(user);
                if (user.AccessFailedCount > Options.Lockout.MaxFailedAccessAttempts)
                {
                    await UserManager.SetLockedOutAsync(user);
                    await UserManager.SetLockoutEndAsync(user);
                    return ADreamSignInResult.LockedOut;
                }
            }
            return ADreamSignInResult.Failed;
        }

        /// <summary>
        /// Returns a flag indicating if the current client browser has been remembered by two factor authentication
        /// for the user attempting to login, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user attempting to login.</param>
        /// <returns>
        /// The task object representing the asynchronous operation containing true if the browser has been remembered
        /// for the current user.
        /// </returns>
        public virtual async Task<bool> IsTwoFactorClientRememberedAsync(User user)
        {
            string userId = await UserManager.GetUserIdAsync(user);
            AuthenticateResult authenticateResult = await Context.AuthenticateAsync(IdentityConstants.TwoFactorRememberMeScheme);
            return authenticateResult?.Principal != null && authenticateResult.Principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name") == userId;
        }

        /// <summary>
        /// Sets a flag on the browser to indicate the user has selected "Remember this browser" for two factor authentication purposes,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user who choose "remember this browser".</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task RememberTwoFactorClientAsync(User user)
        {
            string value = await UserManager.GetUserIdAsync(user);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(IdentityConstants.TwoFactorRememberMeScheme);
            claimsIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", value));
            await Context.SignInAsync(IdentityConstants.TwoFactorRememberMeScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
            {
                IsPersistent = true
            });
        }

        /// <summary>
        /// Clears the "Remember this browser flag" from the current browser, as an asynchronous operation.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task ForgetTwoFactorClientAsync()
        {
            return Context.SignOutAsync(IdentityConstants.TwoFactorRememberMeScheme);
        }

        /// <summary>
        /// Signs in the user without two factor authentication using a two factor recovery code.
        /// </summary>
        /// <param name="recoveryCode">The two factor recovery code.</param>
        /// <returns></returns>
        public virtual async Task<ADreamSignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode)
        {
            TwoFactorAuthenticationInfo twoFactorInfo = await RetrieveTwoFactorInfoAsync();
            if (twoFactorInfo != null && twoFactorInfo.UserId != null)
            {
                User user = await UserManager.FindByIdAsync(selector => selector, twoFactorInfo.UserId);
                if (user == null)
                {
                    return ADreamSignInResult.Failed;
                }
                if ((await UserManager.RedeemTwoFactorRecoveryCodeAsync(user, recoveryCode)).Succeeded)
                {
                    await DoTwoFactorSignInAsync(user, twoFactorInfo, false, false);
                    return ADreamSignInResult.Success;
                }
                return ADreamSignInResult.Failed;
            }
            return ADreamSignInResult.Failed;
        }

        private async Task DoTwoFactorSignInAsync(User user, TwoFactorAuthenticationInfo twoFactorInfo, bool isPersistent, bool rememberClient)
        {
            await ResetLockoutCount(user);
            if (twoFactorInfo.LoginProvider != null)
            {
                await Context.SignOutAsync(IdentityConstants.ExternalScheme);
            }
            await Context.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
            if (rememberClient)
            {
                await RememberTwoFactorClientAsync(user);
            }
            await SignInAsync(user, isPersistent, twoFactorInfo.LoginProvider);
        }

        /// <summary>
        /// Validates the sign in code from an authenticator app and creates and signs in the user, as an asynchronous operation.
        /// </summary>
        /// <param name="code">The two factor authentication code to validate.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="rememberClient">Flag indicating whether the current browser should be remember, suppressing all further 
        /// two factor authentication prompts.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="ADreamSignInResult" />
        /// for the sign-in attempt.</returns>
        public virtual async Task<ADreamSignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient)
        {
            TwoFactorAuthenticationInfo twoFactorInfo = await RetrieveTwoFactorInfoAsync();
            if (twoFactorInfo != null && twoFactorInfo.UserId != null)
            {
                User user = await UserManager.FindByIdAsync(twoFactorInfo.UserId);
                if (user == null)
                {
                    return ADreamSignInResult.Failed;
                }
                ADreamSignInResult aDreamSignInResult = await PreSignInCheck(user);
                if (aDreamSignInResult != null)
                {
                    return aDreamSignInResult;
                }
                if (await UserManager.VerifyTwoFactorTokenAsync(user, Options.Tokens.AuthenticatorTokenProvider, code))
                {
                    await DoTwoFactorSignInAsync(user, twoFactorInfo, isPersistent, rememberClient);
                    return ADreamSignInResult.Success;
                }
                await UserManager.AccessFailedAsync(user);
                return ADreamSignInResult.Failed;
            }
            return ADreamSignInResult.Failed;
        }

        /// <summary>
        /// Validates the two faction sign in code and creates and signs in the user, as an asynchronous operation.
        /// </summary>
        /// <param name="provider">The two factor authentication provider to validate the code against.</param>
        /// <param name="code">The two factor authentication code to validate.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="rememberClient">Flag indicating whether the current browser should be remember, suppressing all further 
        /// two factor authentication prompts.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="ADreamSignInResult" />
        /// for the sign-in attempt.</returns>
        public virtual async Task<ADreamSignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient)
        {
            TwoFactorAuthenticationInfo twoFactorInfo = await RetrieveTwoFactorInfoAsync();
            if (twoFactorInfo != null && twoFactorInfo.UserId != null)
            {
                User user = await UserManager.FindByIdAsync(twoFactorInfo.UserId);
                if (user == null)
                {
                    return ADreamSignInResult.Failed;
                }
                ADreamSignInResult aDreamSignInResult = await PreSignInCheck(user);
                if (aDreamSignInResult != null)
                {
                    return aDreamSignInResult;
                }
                if (await UserManager.VerifyTwoFactorTokenAsync(user, provider, code))
                {
                    await DoTwoFactorSignInAsync(user, twoFactorInfo, isPersistent, rememberClient);
                    return ADreamSignInResult.Success;
                }
                await UserManager.AccessFailedAsync(user);
                return ADreamSignInResult.Failed;
            }
            return ADreamSignInResult.Failed;
        }

        /// <summary>
        /// Gets the <typeparamref name="User" /> for the current two factor authentication login, as an asynchronous operation.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation containing the <typeparamref name="User" />
        /// for the sign-in attempt.</returns>
        public virtual async Task<User> GetTwoFactorAuthenticationUserAsync()
        {
            TwoFactorAuthenticationInfo twoFactorAuthenticationInfo = await RetrieveTwoFactorInfoAsync();
            if (twoFactorAuthenticationInfo == null)
            {
                return null;
            }
            return await UserManager.FindByIdAsync(twoFactorAuthenticationInfo.UserId);
        }

        /// <summary>
        /// Signs in a user via a previously registered third party login, as an asynchronous operation.
        /// </summary>
        /// <param name="loginProvider">The login provider to use.</param>
        /// <param name="providerKey">The unique provider identifier for the user.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="ADreamSignInResult" />
        /// for the sign-in attempt.</returns>
        public virtual Task<ADreamSignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            return ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, false);
        }

        /// <summary>
        /// Signs in a user via a previously registered third party login, as an asynchronous operation.
        /// </summary>
        /// <param name="loginProvider">The login provider to use.</param>
        /// <param name="providerKey">The unique provider identifier for the user.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="bypassTwoFactor">Flag indicating whether to bypass two factor authentication.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="ADreamSignInResult" />
        /// for the sign-in attempt.</returns>
        public virtual async Task<ADreamSignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            User user = await UserManager.FindByLoginAsync(loginProvider, providerKey);
            if (user == null)
            {
                return ADreamSignInResult.Failed;
            }
            ADreamSignInResult aDreamSignInResult = await PreSignInCheck(user);
            if (aDreamSignInResult != null)
            {
                return aDreamSignInResult;
            }
            return await SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
        }

        /// <summary>
        /// Gets a collection of <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationScheme" />s for the known external login providers.		
        /// </summary>		
        /// <returns>A collection of <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationScheme" />s for the known external login providers.</returns>		
        public virtual async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return Enumerable.Where<AuthenticationScheme>(await Schemes.GetAllSchemesAsync(), (Func<AuthenticationScheme, bool>)((AuthenticationScheme s) => !string.IsNullOrEmpty(s.DisplayName)));
        }

        /// <summary>
        /// Gets the external login information for the current login, as an asynchronous operation.
        /// </summary>
        /// <param name="expectedXsrf">Flag indication whether a Cross Site Request Forgery token was expected in the current request.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="ExternalLoginInfo" />
        /// for the sign-in attempt.</returns>
        public virtual async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
        {
            AuthenticateResult authenticateResult = await Context.AuthenticateAsync(IdentityConstants.ExternalScheme);
            IDictionary<string, string> dictionary = authenticateResult?.Properties?.Items;
            if (authenticateResult?.Principal != null && dictionary != null && dictionary.ContainsKey("LoginProvider"))
            {
                if (expectedXsrf != null)
                {
                    if (!dictionary.ContainsKey("XsrfId"))
                    {
                        return null;
                    }
                    if (dictionary["XsrfId"] != expectedXsrf)
                    {
                        return null;
                    }
                }
                string text = authenticateResult.Principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                string text2 = dictionary["LoginProvider"];
                if (text != null && text2 != null)
                {
                    return new ExternalLoginInfo(authenticateResult.Principal, text2, text, text2)
                    {
                        AuthenticationTokens = authenticateResult.Properties.GetTokens()
                    };
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// Stores any authentication tokens found in the external authentication cookie into the associated user.
        /// </summary>
        /// <param name="externalLogin">The information from the external login provider.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.ADreamResult" /> of the operation.</returns>
        public virtual async Task<ADreamResult> UpdateExternalAuthenticationTokensAsync(ExternalLoginInfo externalLogin)
        {
            externalLogin.CheakArgument();
            if (externalLogin.AuthenticationTokens != null && Enumerable.Any(externalLogin.AuthenticationTokens))
            {
                User user = await UserManager.FindByLoginAsync(externalLogin.LoginProvider, externalLogin.ProviderKey);
                if (user == null)
                {
                    return ADreamResult.Failed(Array.Empty<ADreamError>());
                }
                foreach (AuthenticationToken authenticationToken in externalLogin.AuthenticationTokens)
                {
                    ADreamResult ADreamResult = await UserManager.SetAuthenticationTokenAsync(user, externalLogin.LoginProvider, authenticationToken.Name, authenticationToken.Value);
                    if (!ADreamResult.Succeeded)
                    {
                        return ADreamResult;
                    }
                }
            }
            return ADreamResult.Success;
        }

        /// <summary>
        /// Configures the redirect URL and user identifier for the specified external login <paramref name="provider" />.
        /// </summary>
        /// <param name="provider">The provider to configure.</param>
        /// <param name="redirectUrl">The external login URL users should be redirected to during the login flow.</param>
        /// <param name="userId">The current user's identifier, which will be used to provide CSRF protection.</param>
        /// <returns>A configured <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationProperties" />.</returns>
        public virtual AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null)
        {
            AuthenticationProperties authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };
            authenticationProperties.Items["LoginProvider"] = provider;
            if (userId != null)
            {
                authenticationProperties.Items["XsrfId"] = userId;
            }
            return authenticationProperties;
        }

        /// <summary>
        /// Creates a claims principal for the specified 2fa information.
        /// </summary>
        /// <param name="userId">The user whose is logging in via 2fa.</param>
        /// <param name="loginProvider">The 2fa provider.</param>
        /// <returns>A <see cref="T:System.Security.Claims.ClaimsPrincipal" /> containing the user 2fa information.</returns>
        internal ClaimsPrincipal StoreTwoFactorInfo(string userId, string loginProvider)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);
            claimsIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userId));
            if (loginProvider != null)
            {
                claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", loginProvider));
            }
            return new ClaimsPrincipal(claimsIdentity);
        }

        private ClaimsIdentity CreateIdentity(TwoFactorAuthenticationInfo info)
        {
            if (info == null)
            {
                return null;
            }
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);
            claimsIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", info.UserId));
            if (info.LoginProvider != null)
            {
                claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", info.LoginProvider));
            }
            return claimsIdentity;
        }

        /// <summary>
        /// Signs in the specified <paramref name="user" /> if <paramref name="bypassTwoFactor" /> is set to false.
        /// Otherwise stores the <paramref name="user" /> for use after a two factor check.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="loginProvider">The login provider to use. Default is null</param>
        /// <param name="bypassTwoFactor">Flag indicating whether to bypass two factor authentication. Default is false</param>
        /// <returns>Returns a <see cref="T:Microsoft.AspNetCore.Identity.ADreamSignInResult" /></returns>
        protected virtual async Task<ADreamSignInResult> SignInOrTwoFactorAsync(User user, bool isPersistent, string loginProvider = null, bool bypassTwoFactor = false)
        {
            bool flag = !bypassTwoFactor && UserManager.SupportsUserTwoFactor;
            if (flag)
            {
                flag = await UserManager.GetTwoFactorEnabledAsync(user);
            }
            bool flag2 = flag;
            if (flag2)
            {
                flag2 = (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;
            }
            if (flag2 && !(await IsTwoFactorClientRememberedAsync(user)))
            {
                string userId = await UserManager.GetUserIdAsync(user);
                await Context.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, StoreTwoFactorInfo(userId, loginProvider));
                return ADreamSignInResult.TwoFactorRequired;
            }
            if (loginProvider != null)
            {
                await Context.SignOutAsync(IdentityConstants.ExternalScheme);
            }
            await SignInAsync(user, isPersistent, loginProvider);
            return ADreamSignInResult.Success;
        }

        private async Task<TwoFactorAuthenticationInfo> RetrieveTwoFactorInfoAsync()
        {
            AuthenticateResult authenticateResult = await Context.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
            if (authenticateResult?.Principal != null)
            {
                return new TwoFactorAuthenticationInfo
                {
                    UserId = authenticateResult.Principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"),
                    LoginProvider = authenticateResult.Principal.FindFirstValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod")
                };
            }
            return null;
        }
        #region
        /// <summary>
        /// 用于确定用户是否被认为已被锁定。
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>是否被锁定</returns>
        protected virtual async Task<bool> IsLockedOut(User user)
        {
            bool flag = UserManager.SupportsUserLockout;
            if (flag)
            {
                ///先尝试一下是否到结束锁定的时间,如果是就解锁
                flag = await UserManager.UnLockoutEndAsync(user);
                if (!flag)
                {
                    flag = (user == null && User != null) ? User.LockoutEnabled : await UserManager.IsLockedOutAsync(user);
                }
                
            }
            return flag;
        }
        protected virtual async Task<bool> IsEmailConfirmedAsync(User user)
        {
            return (user == null && User != null) ? User.EmailConfirmed : await UserManager.EmailConfirmedAsync(user.Email);
        }
        protected virtual async Task<bool> IsPhoneNumberConfirmedAsync(User user)
        {
            return (user == null && User != null) ? User.PhoneNumberConfirmed : await UserManager.PhoneNumberConfirmedAsync(user.PhoneNumber);
        }
        #endregion
        /// <summary>
        ///用于确保允许用户登录。
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>返回null是无效的</returns>
        protected virtual async Task<ADreamSignInResult> PreSignInCheck(User user)
        {
            if (!(await CanSignInAsync(user)))
            {
                return ADreamSignInResult.NotAllowed;
            }
            if (await IsLockedOut(user))
            {
                return ADreamSignInResult.LockedOut;
            }
            if (await IsPhoneNumberConfirmedAsync(user))
            {
                return ADreamSignInResult.PhoneNumberConfirmed;
            }
            if (await IsEmailConfirmedAsync(user))
            {
                return ADreamSignInResult.EmailConfirmed;
            }
            return null;
        }

        /// <summary>
        /// 用于重置用户的锁定计数。
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.ADreamResult" /> of the operation.</returns>
        protected virtual Task ResetLockoutCount(User user)
        {
            if (UserManager.SupportsUserLockout)
            {
                return UserManager.ResetAccessFailedCountAsync(user);
            }
            return Task.CompletedTask;
        }
    }
}
