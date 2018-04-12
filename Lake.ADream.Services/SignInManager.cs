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

namespace Lake.ADream.Services
{
    public class SignInManager : ServicesBase<User>
    {
        private const string LoginProviderKey = "LoginProvider";
        private const string XsrfKey = "XsrfId";
        private IAuthenticationSchemeProvider _schemes;
        public SignInManager(ADreamDbContext aDreamDbContext, IHttpContextAccessor contextAccessor) : base(aDreamDbContext, contextAccessor)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public User GetUser
        {
            get; private set;
        }

        /// <summary>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.IdentityOptions" /> used.
        /// </summary>
        public IdentityOptions Options
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
            }
        }
        /// <summary>
        /// 如果主体具有与应用程序cookie标识相同的标识，则返回true。
        /// </summary>
        /// <returns>如果用户以身份登录，则为true.</returns>
        public virtual bool IsSignedIn()
        {
            return Context.User.Identity.IsAuthenticated;
        }
        /// <summary>
        /// 登陆方法
        /// </summary>
        /// <param name="account">账号，自动判断手机、邮箱、用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public virtual async Task<SignInResult> SignInAsync(string account, string password)
        {
            ThrowIfDisposed();
            SignInResult signInResult = new SignInResult();

            var predicate = SingInPredicate(account, password);
            GetUser = Dbset.Where(predicate).Where(d => d.PasswordHash.Equals(password)).SingleOrDefault();
            return await CanSignInAsync(GetUser);

        }
        private async Task<SignInResult> CanSignInAsync(User user)
        {
            if (user != null)
            {
                if (user.LockoutEnd > DateTime.Now)
                {
                    user.LockoutEnabled = false;
                }
                if (user.LockoutEnabled)
                {
                    return SignInResult.LockedOut;
                }
                var option = Options.SignIn;
                if (option.RequireConfirmedEmail)
                {
                    if (!user.EmailConfirmed)
                    {
                        return SignInResult.EmailConfirmed;
                    }
                }
                if (option.RequireConfirmedPhoneNumber)
                {
                    if (!user.PhoneNumberConfirmed)
                    {
                        return SignInResult.PhoneNumberConfirmed;
                    }
                }
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.SecurityAudit = new SecurityAudit().ToJson();
                await SaveChangesAsync();
                return SignInResult.Success;
            }
            ///登录失败
            return await Failed(user);
        }
        private async Task<SignInResult> Failed(User user)
        {
            user.AccessFailedCount = +1;
            if (Options.Lockout.MaxFailedAccessAttempts == user.AccessFailedCount)
            {
                user.LockoutEnabled = true;
                var d = Options.Lockout.DefaultLockoutTimeSpan;
                user.LockoutEnd = d;
            }
            await SaveChangesAsync();
            return SignInResult.Failed;
        }
        private Expression<Func<User, bool>> SingInPredicate(string account, string password)
        {
            account.CheakArgument();
            password.CheakArgument();
            Expression<Func<User, bool>> predicate = d => d.UserName.Equals(account);
            if (account.IsPhone())
            {
                predicate = d => d.PhoneNumber.Equals(account);
            }
            if (account.IsEmail())
            {
                predicate = d => d.Email.Equals(account);
            }
            return predicate;
        }
    }
}
