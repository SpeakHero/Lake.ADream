using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Identity
{

    /// <summary>
    /// 配置用户锁定的选项。
    /// </summary>
    public class ADreamLockoutOptions
    {
        /// <value>
        /// 如果新创建的用户可以被锁定，则为true，否则为false。
        /// </value>
        /// <remarks>
        /// Defaults to true.
        /// </remarks>
        public bool AllowedForNewUsers
        {
            get;
            set;
        } = true;


        /// <summary>
        ///获取或设置在用户被锁定之前允许的失败访问尝试的数量，假设已启用锁定。
        /// </summary>
        /// <value>
        ///如果启用锁定，则在用户被锁定之前允许失败访问尝试的次数。
        /// </value>
        /// <remarks>默认为在帐户被锁定之前失败的5次尝试。</remarks>
        public int MaxFailedAccessAttempts
        {
            get;
            set;
        } = 5;


        /// <summary>
        ///获取或设置 <see cref="T:System.DateTime" /> 当锁定发生时，用户被锁定。
        /// </summary>
        /// <value>The <see cref="T:System.DateTime" />当锁定发生时，用户被锁定。</value>
        /// <remarks>默认为5分钟。</remarks>
        public DateTime DefaultLockoutTimeSpan
        {
            get;
            set;
        } = DateTime.Now.AddMinutes(5);

    }
}
