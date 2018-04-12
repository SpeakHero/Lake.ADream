using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Identity
{
    /// <summary>
    /// 配置登录的选项。
    /// </summary>
    public class SignInOptions
    {
        /// <summary>
        ///获取或设置一个标志，该标志指示是否需要签名确认的电子邮件地址。
        /// </summary>
        /// <value>如果用户在登录前必须有确认的电子邮件地址，则为true，否则为false。</value>
        public bool RequireConfirmedEmail
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置标志，指示是否需要确认的电话号码登录。
        /// </summary>
        /// <value>如果用户在登录之前必须有一个确认的电话号码，否则为false。</value>
        public bool RequireConfirmedPhoneNumber
        {
            get;
            set;
        }
    }

}
