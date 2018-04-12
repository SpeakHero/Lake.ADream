//using Microsoft.AspNetCore.Http;
//using System.Security.Claims;
//using System.Security.Principal;

//namespace lake.adream.aspnetcore.runtime.session
//{
//    public class AspnetCorePrincipalaccessor:IPrincipal
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public override ClaimsPrincipal principal => _httpcontextaccessor.HttpContext?.User ?? base.principal;

//        private readonly IHttpContextAccessor _httpcontextaccessor;

//        public AspnetCorePrincipalaccessor(IHttpContextAccessor httpcontextaccessor)
//        {
//            _httpcontextaccessor = httpcontextaccessor;
//        }
//    }
//}
