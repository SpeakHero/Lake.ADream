using Lake.ADream.Entities.Framework;
using Lake.ADream.EntityFrameworkCore;
using Lake.ADream.Infrastructure.Identity;
using Lake.ADream.Models.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Lake.ADream.Services
{
    public class UserManagerService : ServicesBase<User>
    {
        public UserManagerService(ADreamDbContext aDreamDbContext, IHttpContextAccessor contextAccessor, IOptions<ADreamOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IPasswordValidator<User>> passwordValidators, IServiceProvider services) : base(aDreamDbContext, contextAccessor, services)
        {
            optionsAccessor.Value.User.AllowedUserNameCharacters = ""; /// @"[a-zA-Z\u4e00-\u9fa5][a-zA-Z0-9\u4e00-\u9fa5]+"  //默认不能有中文 设置为空就可以了
            var pass = optionsAccessor.Value.Password;
            pass.RequireUppercase = false;
            pass.RequireNonAlphanumeric = false;
            pass.RequireLowercase = false;
            pass.RequireDigit = false;
            PasswordHasher = passwordHasher;

            PasswordValidators = passwordValidators.ToList();
            Options = optionsAccessor;
        }
        public virtual IOptions<ADreamOptions> Options
        {
            get;
            set;
        }
        private IQueryable<User> Users { get { return Dbset; } }
        /// <summary>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.IPasswordHasher`1" /> used to hash passwords.
        /// </summary>
        public virtual IPasswordHasher<User> PasswordHasher
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="T:Microsoft.AspNetCore.Identity.IPasswordValidator`1" /> used to validate passwords.
        /// </summary>
        public virtual IList<IPasswordValidator<User>> PasswordValidators
        {
            get;
        } = new List<IPasswordValidator<User>>();
        /// <summary>
        /// 支持用户安全标记
        /// </summary>
        public virtual bool SupportsUserSecurityStamp { get { ThrowIfDisposed(); return true; } }
        public virtual bool SupportsUserClaim { get { ThrowIfDisposed(); return true; } }

        public bool SupportsUserLockout
        {
            get => Options.Value.Lockout.AllowedForNewUsers;
        }
        public bool SupportsUserTwoFactor { get; internal set; }
        public User User { get; set; }

        public override async Task<ADreamResult> CreateAsync(User user)
        {
            ThrowIfDisposed();
            user.CheakArgument();
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                if (!user.PhoneNumber.IsPhone())
                {
                    return ADreamResult.Failed(new ADreamError { Description = "手机号码格式不正确", Code = nameof(user.PhoneNumber) });
                }
                if (await PhoneNumberExistsAsync(user.PhoneNumber))
                {
                    return ADreamResult.Failed(new ADreamError { Description = "手机号码已经存在", Code = nameof(user.PhoneNumber) });
                }
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                if (!user.Email.IsEmail())
                {
                    return ADreamResult.Failed(new ADreamError { Description = "电子邮件地址格式不正确", Code = nameof(user.Email) });

                }
                if (await EmailExistsAsync(user.Email))
                {
                    return ADreamResult.Failed(new ADreamError { Description = "电子邮件已经存在", Code = nameof(user.Email) });
                }
            }
            if (!string.IsNullOrEmpty(user.UserName))
            {
                if (await UserNameExistsAsync(user.UserName))
                {
                    return ADreamResult.Failed(new ADreamError { Description = "用户名称已经存在", Code = nameof(user.UserName) });
                }
            }
            user.PasswordHash = PasswordHasher.HashPassword(user, user.PasswordHash);
            return await base.CreateAsync(user);
        }
        public virtual async Task<User> FindByPhoneOrEmailOrNameAsync(User user)
        {
            return await FindByPhoneOrEmailOrNameAsync(user, s => s);
        }



        public virtual async Task<TResult> FindByPhoneOrEmailOrNameAsync<TResult>(User user, Expression<Func<User, TResult>> selector) where TResult : class
        {
            user.CheakArgument();
            Expression<Func<User, bool>> predicate = d => d.Email.Equals(user.Email);
            if (user.UserName.IsNotNullOrEmpty())
            {
                predicate = d => d.UserName.Equals(user.UserName);
            }
            else
            {
                if (user.PhoneNumber.IsNotNullOrEmpty())
                {
                    predicate = d => d.PhoneNumber.Equals(user.UserName);
                }
            }
            return await GetQueryable(predicate, selector).SingleOrDefaultAsync();
        }

        public virtual async Task<TResult> FindByPhoneAsync<TResult>(string phoneNumber, Expression<Func<User, TResult>> selector, CancellationToken cancellationToken = default) where TResult : class
        {
            return await FindAsync(queryable => queryable.PhoneNumber.Equals(phoneNumber), selector, cancellationToken);
        }
        public virtual async Task<TResult> FindByNameAsync<TResult>(string username, Expression<Func<User, TResult>> selector, CancellationToken cancellationToken = default) where TResult : class
        {
            return await FindAsync(queryable => queryable.UserName.Equals(username), selector, cancellationToken);
        }

        public virtual async Task<TResult> FindByEmailAsync<TResult>(string email, Expression<Func<User, TResult>> selector, CancellationToken cancellationToken = default) where TResult : class
        {
            return await FindAsync(queryable => queryable.Email.Equals(email), selector, cancellationToken);
        }
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey)
        {
            var val = await FirstOrDefaultAsync<UserLogin>(d => d.LoginProvider.Equals(loginProvider) && d.ProviderKey.Equals(providerKey));
            if (val != null)
            {
                return await FindUserAsync(val.UserId);
            }
            return null;
        }
        //public async Task<TResult> FindByLoginAsync<TResult>(string loginProvider, string providerKey, Expression<Func<UserLogin, TResult>> selector) where TResult : class
        //{
        //    var val = await FirstOrDefaultAsync<UserLogin>(d => d.LoginProvider.Equals(loginProvider) && d.ProviderKey.Equals(providerKey), selector);
        //    if (val != null)
        //    {
        //        return await FindUserAsync(val.UserId);
        //    }
        //    return null;
        //}
        public async Task<string> GetUserIdAsync(User user)
        {
            return await FindByPhoneOrEmailOrNameAsync(user, selector => selector.Id);
        }
        public virtual string GetUserId(ClaimsPrincipal principal)
        {
            principal.CheakArgument();
            return PrincipalExtensions.FindFirstValue(principal, Options.Value.ClaimsIdentity.UserIdClaimType);
        }
        public virtual async Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            principal.CheakArgument();
            string userId = GetUserId(principal);
            return await FindUserAsync(userId);
        }
        public virtual string GetUserName(ClaimsPrincipal principal)
        {
            principal.CheakArgument();
            return principal.FindFirstValue(Options.Value.ClaimsIdentity.UserNameClaimType);
        }
        public virtual async Task<User> GetUsersForClaimAsync(Claim claim)
        {
            ThrowIfDisposed();
            claim.CheakArgument();
            var userId = await FirstOrDefaultAsync<string, UserClaim>(predicate => predicate.ClaimType.Equals(claim.Type) && predicate.ClaimValue.Equals(claim.Value), selector => selector.UserId);
            return await FindUserAsync(userId);
        }
        /// <summary>
        /// 是否设置了密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<bool> HasPasswordAsync(User user)
        {
            ThrowIfDisposed();
            user.CheakArgument();
            return await AnyAsync(predicate => (predicate.Id.Equals(user.Id) || predicate.UserName.Equals(user.UserName) || predicate.PhoneNumber.Equals(user.PhoneNumber) || predicate.Email.Equals(user.Email)) && predicate.PasswordHash.IsNotNullOrEmpty());
        }
        public virtual async Task<User> FindUserAsync(string userId)
        {
            if (userId.IsNullOrEmpty())
            {
                return null;
            }
            return await FindAsync(userId);
        }

        public virtual async Task<ADreamResult> SetSecurityStampAsync(User user, string stamp = "", CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user != null)
            {
                user.SecurityStamp = stamp ?? Guid.NewGuid().ToString();
                return await UpdateAsync(user, nameof(user.SecurityStamp));
            }
            //"用户不存在"
            return ADreamResult.Failed();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">用户名,手机,邮箱</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var securityStamp = await FindByPhoneOrEmailOrNameAsync(user, s => s.SecurityStamp);
            return securityStamp;
        }

        public virtual async Task<bool> UserNameExistsAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            return await AnyAsync(d => d.UserName == username);
        }

        public virtual async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            return await AnyAsync(d => d.Email == email);
        }

        public virtual async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }
            return await AnyAsync(d => d.PhoneNumber == phoneNumber);
        }
        public virtual async Task<bool> PhoneNumberConfirmedAsync(string phoneNumber, bool phoneNumberConfirmed = true)
        {
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.IsPhone() == false)
            {
                return false;
            }
            return await AnyAsync(d => d.PhoneNumber == phoneNumber && d.PhoneNumberConfirmed == phoneNumberConfirmed);
        }
        public virtual async Task<bool> EmailConfirmedAsync(string email, bool emailConfirmed = true)
        {
            if (string.IsNullOrEmpty(email) || email.IsEmail() == false)
            {
                return false;
            }
            return await AnyAsync(d => d.Email == email && d.EmailConfirmed == emailConfirmed);
        }
        public virtual async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            user.CheakArgument();
            return await EntityFrameworkQueryableExtensions.ToListAsync(Queryable.Select(Queryable.Where(dbContext.UserClaims, (UserClaim uc) => !uc.IsDelete && uc.UserId.Equals(user.Id)), (UserClaim c) => c.ToClaim()), cancellationToken);
        }
        public virtual async Task<ADreamResult> SetUserNameAsync(User user, string userName)
        {
            ThrowIfDisposed();
            user.CheakArgument();
            var existname = await AnyAsync(predicate => predicate.UserName.Equals(userName) && !predicate.Id.Equals(user.Id));
            if (existname)
            {
                return ADreamResult.Failed("已经存在相同的用户名");
            }
            user = new User
            {
                // NormalizedUserName
                UserName = userName,
                Id = user.Id,
                TimeSpan = user.TimeSpan,
            };
            return await UpdateAsync(user, nameof(user.UserName));
        }
        /// <summary>
        /// 设置用户失败的次数
        /// </summary>
        /// <param name="user">查询user</param>
        /// <returns></returns>
        public virtual async Task AccessFailedAsync(User user)
        {
            user.AccessFailedCount += 1;
            await UpdateAsync(user, nameof(user.AccessFailedCount));
        }
        /// <summary>
        /// 是否被锁定
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> IsLockedOutAsync(User user)
        {
            return await AnyAsync(predicate => predicate.LockoutEnabled && predicate.Id.Equals(user.Id));
        }

        /// <summary>
        /// 请先从数据库中查询user passwordhash 
        /// 检测输入的密码和数据库中的密码是否一致
        /// </summary>
        /// <param name="user"></param>
        /// <param name="nohashpassword">没有加密的密码</param>
        /// <returns></returns>
        public virtual bool CheckPassword(User user, string nohashpassword)
        {
            if (user == null)
            {
                return false;
            }
            var result = PasswordHasher.HashPassword(user, nohashpassword).Equals(user.PasswordHash);

            return result;
        }
        /// <summary>
        /// 设置用户真实姓名
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedUserName"></param>
        /// <returns></returns>
        public virtual async Task<ADreamResult> SetNormalizedUserNameAsync(User user, string normalizedUserName)
        {
            user = new User
            {

                NormalizedUserName = normalizedUserName,
                Id = user.Id,
                TimeSpan = user.TimeSpan,
            };
            return await UpdateAsync(user, nameof(user.NormalizedUserName));
        }

        public async Task<ADreamResult> RedeemTwoFactorRecoveryCodeAsync(User user, string recoveryCode)
        {
            return await UpdateAsync(user, nameof(user.ConcurrencyStamp));
        }

        public async Task<bool> VerifyTwoFactorTokenAsync(User user, string authenticatorTokenProvider, string code)
        {
            throw new NotImplementedException();
        }

        public async Task<ADreamResult> SetAuthenticationTokenAsync(User user, string loginProvider, string name, string value)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return await AnyAsync(d => d.TwoFactorEnabled && d.Id.Equals(user.Id));
        }

        public async Task<ICollection<string>> GetValidTwoFactorProvidersAsync(User user)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 重置失败登陆次数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            await UpdateAsync(user, nameof(user.AccessFailedCount));
        }
        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task SetLockedOutAsync(User user)
        {
            user.LockoutEnabled = true;
            await UpdateAsync(user, nameof(user.LockoutEnabled));
        }
        /// <summary>
        /// 设置锁定结束时间
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task SetLockoutEndAsync(User user)
        {
            await SetLockoutEndAsync(user, Options.Value.Lockout.DefaultLockoutTimeSpan);
        }
        /// <summary>
        /// 结束锁定,并重置登录失败次数
        /// </summary>
        /// <param name="user">结束锁定,并重置登录失败次数</param>
        /// <returns></returns>
        public async Task<bool> UnLockoutEndAsync(User user)
        {
            if (user.LockoutEnd.HasValue)
            {
               if (user.LockoutEnd >= DateTime.Now)
                {
                    user.LockoutEnabled = false;
                    user.AccessFailedCount = 0;
                    await UpdateAsync(user, nameof(user.LockoutEnabled), nameof(user.AccessFailedCount));
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 设置锁定结束时间
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task SetLockoutEndAsync(User user, DateTime dateTime)
        {
            user.LockoutEnd = dateTime;
            await UpdateAsync(user, nameof(user.LockoutEnabled));
        }
        /// <summary>
        /// 解除锁定
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UnLockedOutAsync(User user)
        {
            user.LockoutEnabled = false;
            await UpdateAsync(user, nameof(user.LockoutEnabled));
        }
    }
}
