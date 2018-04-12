namespace Lake.ADream.Infrastructure.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public  class IdentityError
    {   /// <summary>
        /// 获取或设置此错误的代码。
        /// </summary>
        /// <value>
        /// 此错误的代码。
        /// </value>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置此错误的说明。
        /// </summary>
        /// <value>
        /// 此错误的说明。
        /// </value>
        public string Description
        {
            get;
            set;
        }
    }
}