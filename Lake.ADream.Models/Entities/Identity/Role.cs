using Lake.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lake.ADream.Models.Entities.Identity
{
    public class Role: EntityBase
    {
        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "角色的名称")]
        /// <summary>
        /// 获取或设置此角色的名称。
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置此角色的规范化名称。
        /// </summary>
        public virtual string NormalizedName
        {
            get;
            set;
        }

        /// <summary>
        ///一个随机值，每当某个角色被保存到存储区时，该值将发生变化。
        /// </summary>
        public virtual string ConcurrencyStamp
        {
            get;
            set;
        } = Guid.NewGuid().ToString();


        /// <summary>
        /// Initializes a new instance of <see cref="T:Microsoft.AspNetCore.Identity.IdentityRole`1" />.
        /// </summary>
        public Role()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:Microsoft.AspNetCore.Identity.IdentityRole`1" />.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        public Role(string roleName)
            : this()
        {
            Name = roleName;
        }

        /// <summary>
        /// 返回角色的名称。
        /// </summary>
        /// <returns>The name of the role.</returns>
        public override string ToString()
        {
            return Name;
        }
        public ICollection<RolePermission> Permissions { get; set; }
    }
}
