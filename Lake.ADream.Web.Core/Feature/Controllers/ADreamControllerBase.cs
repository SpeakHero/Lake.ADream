using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lake.ADream.Web.Core.Controllers
{
    public class ADreamControllerBase : Controller
    {
        /// <summary>
        /// 是ajax返回json否则返回视图
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual IActionResult JsonOrView(object obj)
        {
            if (!Request.IsAjax())
            {
                return obj == null ? View() : View(obj);
            }
            return Json(obj);
        }
    }
}