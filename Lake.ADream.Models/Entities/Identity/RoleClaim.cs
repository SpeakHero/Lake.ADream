using Lake.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Lake.ADream.Models.Entities.Identity
{

    /// <summary>
    /// 表示在角色中授予所有用户的声明。
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key of the role associated with this claim.</typeparam>
    public class RoleClaim : EntityBase
    {


        /// <summary>
        /// 获取或设置与此请求相关联的角色的主键。
        /// </summary>
        public virtual string RoleId
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置此索赔的索赔类型。
        /// </summary>
        public virtual string ClaimType
        {
            get;
            set;
        }

        /// <summary>
        ///获取或设置此索赔的索赔值。
        /// </summary>
        public virtual string ClaimValue
        {
            get;
            set;
        }

        /// <summary>
        ///构建类型和值的新索赔。
        /// </summary>
        /// <returns></returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        /// <summary>
        ///初始化由其他索赔claimtype和claimvalue复制。
        /// </summary>
        /// <param name="other">The claim to initialize from.</param>
        public virtual void InitializeFromClaim(Claim other)
        {
            ClaimType = other?.Type;
            ClaimValue = other?.Value;
        }
    }
}
