using System.Collections.Generic;
using System.Linq;

namespace Lake.ADream.Infrastructure.Identity
{

    /// <summary>
    /// 表示标识操作的结果。
    /// </summary>
    public class ADreamResult
    {
        private static readonly ADreamResult _success = new ADreamResult
        {
            Succeeded = true
        };

        private List<ADreamError> _errors = new List<ADreamError>();

        /// <summary>
        /// 标志，指示操作是否成功。
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded
        {
            get;
            protected set;
        } = true;
        /// <summary>
        /// 
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" />s containing an errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" />s.</value>
        public IEnumerable<ADreamError> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> indicating a successful identity operation.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> indicating a successful operation.</returns>
        public static ADreamResult Success => _success;

        /// <summary>
        /// Creates an <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> indicating a failed identity operation, with a list of <paramref name="errors" /> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" />s which caused the operation to fail.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> indicating a failed identity operation, with a list of <paramref name="errors" /> if applicable.</returns>
        public static ADreamResult Failed(params ADreamError[] errors)
        {
            ADreamResult identityResult = new ADreamResult
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
        /// 
        /// </summary>
        /// <param name="errstr"></param>
        /// <returns></returns>
        public static ADreamResult Failed(params string[] errstr)
        {
            var errors = new List<ADreamError>();
            foreach (var item in errstr)
            {
                errors.Add(new ADreamError { Description = item });
            }
            return Failed(errors.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ADreamResult Failed()
        {
            return Failed("错误");
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
