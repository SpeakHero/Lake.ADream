using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lake.ADream.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace Lake.ADream.Web.Core.Controllers
{
    public class AccoutController : ADreamControllerBase
    {
        public AccoutController()
        {
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ToJson()
        {
            var json = new IdentityOptions();
            json.User.RequireUniqueEmail = true;
            HttpContext.Session.SetObjectAsJson("json", json);
            HttpContext.Session.SetObjectAsJson("jso2n", json);
            var njson = HttpContext.Session.GetObjectFromJson<IdentityOptions>("jso2n");
            var id = HttpContext.Session.Id;
            njson.SignIn.RequireConfirmedEmail = true;
            return Json(njson);
        }
    }
}