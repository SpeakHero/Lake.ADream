using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Lake.ADream.Infrastructure.Identity;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Lake.ADream.Web.Core.Controllers
{
    public class ADreamControllerBase : Controller
    {
      
        protected ADreamResult aDreamResult = new ADreamResult();
        protected IServiceCollection _service;

        protected ILogger Logger => GetService<ILogger>();

        protected  IHostingEnvironment Env => GetService<IHostingEnvironment>();

        protected ADreamControllerBase(IServiceCollection services)
        {
            _service = services;
        }

        protected IActionResult ValidFailed()
        {
            var err = GetModelError().Append(new ADreamError { Description = "请检测你的输入!" });
            aDreamResult = ADreamResult.Failed(err.ToArray());
            return JsonOrView(ADreamResult.Failed(err.ToArray()));
        }
        protected TResult GetService<TResult>()
        {
            return _service.BuildServiceProvider().GetService<TResult>();
        }
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

        internal IEnumerable<ADreamError> GetModelError()
        {
            var errinfo = new ADreamError();
            foreach (var s in ModelState.Values)
            {
                foreach (var p in s.Errors)
                {
                    errinfo.Description = p.ErrorMessage;
                    yield return errinfo;
                }
            }
        }
    }
}