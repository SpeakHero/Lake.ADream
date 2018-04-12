using Lake.ADream.Models.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.EntityFrameworkCore
{
    public partial class ADreamDbContext : DbContext
    {
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }
        /// <summary>
        /// 角色权限
        /// </summary>
        public DbSet<RolePermission> RolesPermissions { get; set; }
        /// <summary>
        /// 功能
        /// </summary>
        public DbSet<Permission> Permissions { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// 用户声明
        /// </summary>
        public DbSet<UserClaim> UserClaims { get; set; }
        /// <summary>
        /// 令牌验证
        /// </summary>
        public DbSet<UserToken> UserTokens { get; set; }
        /// <summary>
        /// 外部登录
        /// </summary>
        public DbSet<UserLogin> UserLogins { get; set; }
    }
}
