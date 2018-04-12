using Lake.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lake.ADream.Models.Entities.Identity
{
    public class RolePermission: EntityBase
    {
        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "角色的主键")]
        public virtual string RoleId { get; set; }
        [Required]
        public virtual string ApplicationPermissionId { get; set; }
        [Display(Name = "是否允许")]
        public bool IsEnable { get; set; } = true;
        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "用户的主键")]
        public virtual string UserId { get; set; }
        [Display(Name = "参数正则表达式")]
        public virtual string Regular { get; set; }
    }
}
