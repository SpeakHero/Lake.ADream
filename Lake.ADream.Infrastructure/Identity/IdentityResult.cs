using System.Collections.Generic;
using System.Linq;

namespace Lake.ADream.Infrastructure.Identity
{

    /// <summary>
    /// 表示标识操作的结果。
    /// </summary>
    public class IdentityResult
    {
        private static readonly IdentityResult _success = new IdentityResult
        {
            Succeeded = true
        };

        private List<IdentityError> _errors = new List<IdentityError>();

        /// <summary>
        /// 标志，指示操作是否成功。
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded
        {
            get;
            protected set;
        }

        /// <summary>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" />s containing an errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" />s.</value>
        public IEnumerable<IdentityError> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> indicating a successful identity operation.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> indicating a successful operation.</returns>
        public static IdentityResult Success => _success;

        /// <summary>
        /// Creates an <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> indicating a failed identity operation, with a list of <paramref name="errors" /> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" />s which caused the operation to fail.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> indicating a failed identity operation, with a list of <paramref name="errors" /> if applicable.</returns>
        public static IdentityResult Failed(params IdentityError[] errors)
        {
            IdentityResult identityResult = new IdentityResult
            {
                Succeeded = false
            };
            if (errors != null)
            {
                identityResult._errors.AddRange(errors);
            }
            return identityResult;
        }

        /// <summary>
        /// Converts the value of the current <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned 
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="P:Microsoft.AspNetCore.Identity.IdentityResult.Errors" /> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            if (!Succeeded)
            {
                return string.Format("{0} : {1}", "Failed", string.Join(",", (from x in Errors
                                                                              select x.Code).ToList()));
            }
            return "Succeeded";
        }
    }
}
