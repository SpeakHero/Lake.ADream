using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Identity
{
    /// <summary>
    /// 指定密码要求的选项。
    /// </summary>
    public class PasswordOptions
    {
        /// <summary>
        ///获取或设置密码必须为最小的长度。
        /// </summary>
        /// <remarks>
        /// This defaults to 6.
        /// </remarks>
        public int RequiredLength
        {
            get;
            set;
        } = 6;


        /// <summary>
        /// 获取或设置密码必须包含的唯一字符的最小数目。
        /// </summary>
        /// <remarks>
        /// 默认值为1。
        /// </remarks>
        public int RequiredUniqueChars
        {
            get;
            set;
        } = 1;


        /// <summary>
        /// 获取或设置一个标志，该标志指示密码是否必须包含非字母数字字符。
        /// </summary>
        /// <value>如果密码必须包含非字母数字字符，则为true，否则为false。</value>
        /// <remarks>
        /// 默认为true。
        /// </remarks>
        public bool RequireNonAlphanumeric
        {
            get;
            set;
        } = true;


        /// <summary>
        ///获取或设置一个标志，指示密码是否必须包含小写ASCII字符。
        /// </summary>
        /// <value>如果密码必须包含小写字母ASCII字符，则为true。</value>
        /// <remarks>
        /// 默认为true。
        /// </remarks>
        public bool RequireLowercase
        {
            get;
            set;
        } = true;


        /// <summary>
        /// 获取或设置一个标志，该标志指示密码是否必须包含大写字母ASCII字符。
        /// </summary>
        /// <value>如果密码必须包含大写字母ASCII字符，则为true。</value>
        /// <remarks>
        /// 默认为true。
        /// </remarks>
        public bool RequireUppercase
        {
            get;
            set;
        } = true;


        /// <summary>
        /// 获取或设置一个标志，指示密码是否必须包含数字。
        /// </summary>
        /// <value>如果密码必须包含数字，则为true。</value>
        /// <remarks>
        ///默认为true。
        /// </remarks>
        public bool RequireDigit
        {
            get;
            set;
        } = true;

    }

}
