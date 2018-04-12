using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class CheakArguments
    {
        /// <summary>
        /// 参数不能null检查
        /// </summary>
        public static void CheakArgument(this object obj)
        {
            if (obj.Equals(null))
            {
                throw new ArgumentException(nameof(obj));
            }
        }
    }
}
