using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Identity
{

    /// <summary>
    /// 表示登录操作的结果。
    /// </summary>
    public class SignInResult
    {
        private readonly string notAllowed = "不允许登陆";
        private readonly string lockedout = "账号被锁定";
        private readonly string requiresTwoFactor = "需要二次登陆验证";
        private readonly string failed = "登陆失败,账号或者密码错误！";
        private readonly string succeded = "登陆成功";
        private static readonly SignInResult _success = new SignInResult
        {
            Succeeded = true
        };

        private static readonly SignInResult _failed = new SignInResult();

        private static readonly SignInResult _lockedOut = new SignInResult
        {
            IsLockedOut = true
        };

        private static readonly SignInResult _notAllowed = new SignInResult
        {
            IsNotAllowed = true
        };

        private static readonly SignInResult _twoFactorRequired = new SignInResult
        {
            RequiresTwoFactor = true
        };
        private static readonly SignInResult _emailConfirmed = new SignInResult { IsEmailConfirmed = false };
        private static SignInResult _phoneNumberConfirmed = new SignInResult { IsPhoneNumberConfirmed = false };

        private bool _succeeded;
        private bool _isLockedOut;
        private bool _isNotAllowed;
        private bool _requiresTwoFactor;
        private bool _isemailConfirmed;
        private bool _isPhoneNumberConfirmed;
        /// <summary>
        /// 返回标志指示登录是否成功。
        /// </summary>
        /// <value>如果登录成功，则为true，否则为false。</value>
        public bool Succeeded
        {
            get => _succeeded;
            protected set => _succeeded = value;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEmailConfirmed
        {
            get => _isemailConfirmed;
            protected set => _isemailConfirmed = value;
        }
        /// <summary>
        /// 返回一个标志指示是否试图登录的用户被锁定。
        /// </summary>
        /// <value>如果试图登录的用户被锁定，则为true，否则为false。</value>
        public bool IsLockedOut
        {
            get => _isLockedOut;
            protected set => _isLockedOut = value;
        }

        /// <summary>
        ///返回标志指示是否试图登录的用户不允许登录。
        /// </summary>
        /// <value>如果试图登录的用户不允许登录，则为true，否则为false。</value>
        public bool IsNotAllowed
        {
            get => _isNotAllowed;
            protected set => _isNotAllowed = value;
        }

        /// <summary>
        /// 返回标志表示用户是否试图登录，需要双因素身份验证。
        /// </summary>
        /// <value>如果试图登录的用户需要双因素身份验证，则为true，否则为false。</value>
        public bool RequiresTwoFactor
        {
            get => _requiresTwoFactor;
            protected set => _requiresTwoFactor = value;
        }

        /// <summary>
        /// Returns a <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> that represents a successful sign-in.
        /// </summary>
        /// <returns>A <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> that represents a successful sign-in.</returns>
        public static SignInResult Success => _success;

        /// <summary>
        /// Returns a <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> that represents a failed sign-in.
        /// </summary>
        /// <returns>A <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> that represents a failed sign-in.</returns>
        public static SignInResult Failed => _failed;

        /// <summary>
        /// Returns a <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> that represents a sign-in attempt that failed because 
        /// the user was logged out.
        /// </summary>
        /// <returns>A <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> that represents sign-in attempt that failed due to the
        /// user being locked out.</returns>
        public static SignInResult LockedOut => _lockedOut;

        /// <summary>
        /// Returns a <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> that represents a sign-in attempt that failed because 
        /// the user is not allowed to sign-in.
        /// </summary>
        /// <returns>A <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> that represents sign-in attempt that failed due to the
        /// user is not allowed to sign-in.</returns>
        public static SignInResult NotAllowed => _notAllowed;

        /// <summary>
        /// Returns a <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> that represents a sign-in attempt that needs two-factor 
        /// authentication.
        /// </summary>
        /// <returns>A <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> that represents sign-in attempt that needs two-factor
        /// authentication.</returns>
        public static SignInResult TwoFactorRequired => _twoFactorRequired;
        /// <summary>
        /// 
        /// </summary>
        public static SignInResult EmailConfirmed => _emailConfirmed;
        /// <summary>
        /// 
        /// </summary>
        public static SignInResult PhoneNumberConfirmed => _phoneNumberConfirmed;

        /// <summary>
        /// 
        /// </summary>
        public bool IsPhoneNumberConfirmed { get => _isPhoneNumberConfirmed; private set => _isPhoneNumberConfirmed = value; }

        /// <summary>
        /// Converts the value of the current <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of value of the current <see cref="T:Microsoft.AspNetCore.Identity.SignInResult" /> object.</returns>
        public override string ToString()
        {
            if (!IsLockedOut)
            {
                if (!IsNotAllowed)
                {
                    if (!IsPhoneNumberConfirmed)
                    {
                        if (!IsEmailConfirmed)
                        {
                            if (!RequiresTwoFactor)
                            {
                                if (!Succeeded)
                                {
                                    return failed;
                                }
                                return succeded;
                            }
                            return requiresTwoFactor;
                        }
                        return "需要激活邮箱";
                    }
                    return "需要激活移动电话";
                }
                return notAllowed;
            }
            return lockedout;
        }
    }
}
