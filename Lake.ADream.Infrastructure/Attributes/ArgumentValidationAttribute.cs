using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ArgumentValidationAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="argumentName"></param>
        public abstract void Validate(object value, object argumentName);
    }
}
