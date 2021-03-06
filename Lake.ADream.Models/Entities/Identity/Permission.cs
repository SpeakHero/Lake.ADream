﻿using Lake.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lake.ADream.Models.Entities.Identity
{
    public class Permission : EntityBase
    {

        public Permission()

        {

            Role = new List<RolePermission>();

        }
        /// <summary>

        /// 控制器名

        /// </summary>

        [Required(AllowEmptyStrings = false)]

        [Display(Name = "控制器名")]

        public string Controller { get; set; }

        /// <summary>

        /// 方法名

        /// </summary>

        [Required(AllowEmptyStrings = false)]

        [Display(Name = "方法名")]

        public string Action { get; set; }

        /// <summary>

        /// 参数字符串

        /// </summary>

        [Display(Name = "参数字符串")]

        public string Params { get; set; }

        /// <summary>
        /// 是否允许匿名登录
        /// </summary>
        [Display(Name = "是否允许匿名访问")]
        public bool IsAllowAnonymous
        {
            get; set;
        } = false;
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public bool IsEnable { get; set; } = true;

        /// <summary>

        /// 角色列表

        /// </summary>

        public ICollection<RolePermission> Role { get; set; }
        [Display(Name = "区域名称")]

        public string AreaName { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Display(Name = "显示顺序")]

        public int ShowSort { get; set; } = 0;

        /// <summary>
        /// 级别 0为page 1为button
        /// </summary>
        public int Level { get; set; } = 0;
    }

    enum Levels
    {
        Page, Button
    }
}
