using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Identity
{
    /// <summary>
    /// 用户验证选项。
    /// </summary>
    public class UserOptions
    {
        /// <summary>
        /// 获取或设置用于验证用户名的用户名中允许字符的列表。
        /// </summary>
        /// <value>
        /// 用于验证用户名的用户名中允许的字符列表。
        /// </value>
        public string AllowedUserNameCharacters
        {
            get;
            set;
        } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";


        /// <summary>
        /// 获取或设置一个标志，该标志指示应用程序是否需要为用户提供唯一的电子邮件。
        /// </summary>
        /// <value>
        ///如果应用程序要求每个用户都有自己独特的电子邮件，则为true，否则为false。
        /// </value>
        public bool RequireUniqueEmail
        {
            get;
            set;
        }
    }

}
