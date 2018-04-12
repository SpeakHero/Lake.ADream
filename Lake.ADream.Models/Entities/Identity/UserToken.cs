using Lake.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Models.Entities.Identity
{

    /// <summary>
    /// 表示用户的身份验证令牌。
    /// </summary>
    public class UserToken : EntityBase
    {
        /// <summary>
        /// 获取或设置令牌所属的用户的主键。
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置loginprovider这个令牌是从。
        /// </summary>
        public virtual string LoginProvider
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置令牌的名称。
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        ///获取或设置令牌值。
        /// </summary>
        public virtual string Value
        {
            get;
            set;
        }
    }
}
