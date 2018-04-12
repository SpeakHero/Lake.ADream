using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Identity
{
    /// <summary>
    /// 用于表示令牌提供程序 <see cref="T:Microsoft.AspNetCore.Identity.TokenOptions" />'s TokenMap.
    /// </summary>
    public class TokenProviderDescriptor
    {
        /// <summary>
        /// 将用于此令牌提供程序的类型。
        /// </summary>
        public Type ProviderType
        {
            get;
        }

        /// <summary>
        /// 如果指定，将用于令牌提供程序的实例。
        /// </summary>
        public object ProviderInstance
        {
            get;
            set;
        }

        /// <summary>
        /// 初始化一个新实例 <see cref="T:Microsoft.AspNetCore.Identity.TokenProviderDescriptor" /> class.
        /// </summary>
        /// <param name="type">此令牌提供程序的具体类型。</param>
        public TokenProviderDescriptor(Type type)
        {
            ProviderType = type;
        }
    }

}
