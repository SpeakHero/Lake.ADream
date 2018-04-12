using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NotNullAttribute : ArgumentValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="argumentName"></param>
        public override void Validate(object value, object argumentName)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(argumentName));
        }
    }
}
