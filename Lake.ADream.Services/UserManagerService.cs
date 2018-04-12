using Lake.ADream.Entities.Framework;
using Lake.ADream.EntityFrameworkCore;
using Lake.ADream.Infrastructure.Identity;
using Lake.ADream.Models.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class UserManagerService : ServicesBase<User>, IDisposable
    {
        /// <summary>
        /// 用于重置密码相关方法的数据保护目的。
        /// </summary>
        protected const string ResetPasswordTokenPurpose = "ResetPassword";

        /// <summary>
        /// 用于更改电话号码方法的数据保护目的。
        /// </summary>
        protected const string ChangePhoneNumberTokenPurpose = "ChangePhoneNumber";

        /// <summary>
        /// 用于电子邮件确认相关方法的数据保护目的。
        /// </summary>
        protected const string ConfirmEmailTokenPurpose = "EmailConfirmation";
        private TimeSpan _defaultLockout = TimeSpan.Zero;
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public UserManagerService(ADreamDbContext aDreamDbContext, IHttpContextAccessor contextAccessor) : base(aDreamDbContext, contextAccessor)
        {
        }

        public override async Task<IdentityResult> CreateAsync(bool SaveRight, params User[] users)
        {
            var newusers = new List<User>();
            IList<IdentityError> identityErrors = new List<IdentityError>();
            foreach (var item in users)
            {
                var e = await Dbset.AnyAsync(a => a.UserName.Equals(item.UserName) || a.PhoneNumber.Equals(item.PhoneNumber) || a.Email.Equals(item.Email));
                if (e)
                {
                    var identityError = new IdentityError { Description = $"已经存在{item.UserName},{item.PhoneNumber},{item.Email}", Code = "500" };
                    identityErrors.Add(identityError);
                }
                else
                {
                    newusers.Add(item);
                }
            }
            if (identityErrors.Count == 0)
            {
                var newuserss = newusers.Distinct(d => d.UserName).Distinct(d => d.PhoneNumber).Distinct(d => d.Email).ToArray();
                await base.CreateAsync(SaveRight, newuserss);
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(identityErrors.ToArray()); ;
        }


        /// <summary>
        /// 确认指定密码后更改用户密码 <paramref name="currentPassword" /> 是正确的，
        /// 作为异步操作。
        /// </summary>
        /// <param name="userId">设置密码的用户(指定ID）</param>
        /// <param name="currentPassword">更改前要验证当前密码。</param>
        /// <param name="newPassword">为指定设置的新密码 <paramref name="user" />.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> 表示异步操作的，包含 <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
        /// of the operation.
        /// </returns>
        public virtual async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            ThrowIfDisposed();
            userId.CheakArgument();
            IdentityError identityError = new IdentityError { Description = "更改用户密码失败" };
            var exist = await AnyAsync(any => any.Id.Equals(userId) && any.PasswordHash.Equals(currentPassword));
            if (exist)
            {
                User user = new User()
                {
                    Id = userId,
                    PasswordHash = newPassword
                };
                DbContext.ChangeTracker.TrackGraph(user, u =>
                {
                    u.Entry.Property("PasswordHash").IsModified = true;
                });
                int save = await DbContext.SaveChangesAsync();
                if (save > 0)
                {
                    return IdentityResult.Success;
                }
                identityError.Code = "500";
            }
            else
            {
                identityError.Code = "404";
                LoggerExtensions.LogError(Logger, "修改失败，用户不存在");
            }
            var result = IdentityResult.Failed();
            result.Errors.Append(identityError);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public virtual async Task<string> GetUserNameById(string userId)
        {

            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            try
            {
                return await Task.FromResult(Dbset.Where(d => d.Id.Equals(userId)).Select(s => s.UserName).Single());
            }
            catch
            {
                return null;
            }
        }
        public virtual async Task<string> GetIdByName(string username)
        {
            return await GetUserAsync(username, s => s.Id);
        }
        /// <summary>
        /// 根据用户名\手机号\邮箱查找用户
        /// </summary>
        /// <typeparam name="TResult">TResult : class</typeparam>
        /// <param name="account">用户名\手机号\邮箱</param>
        /// <param name="selector">需要的字段</param>
        /// <returns></returns>
        public virtual async Task<TResult> GetUserAsync<TResult>(string account, Expression<Func<User, TResult>> selector) where TResult : class
        {
            return await GetQueryable(AccountPredicate(account), selector).SingleOrDefaultAsync();
        }
        /// <summary>
        /// 根据用户名\手机号\邮箱查找用户
        /// </summary>
        /// <param name="account">用户名\手机号\邮箱</param>
        /// <returns></returns>
        public virtual async Task<User> GetUserAsync(string account)
        {
            return await Dbset.SingleOrDefaultAsync(AccountPredicate(account));
        }

        /// <summary>
        /// 是否已存在用户名
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistUserNameAsync(string username)
        {
            return await AnyAsync(a => a.UserName.Equals(username));
        }

        private Expression<Func<User, bool>> AccountPredicate(string account)
        {
            account.CheakArgument();
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
