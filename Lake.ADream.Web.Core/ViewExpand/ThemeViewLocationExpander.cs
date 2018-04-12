using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lake.ADream.Web.Core.ViewExpand
{
    /// <summary>
    /// 自定视图主题 实现IViewLocationExpander接口
    /// </summary>
    public class ThemeViewLocationExpander : IViewLocationExpander
    {
        public ThemeViewLocationExpander()
        {
        }
        /// <summary>
        /// 主题名称
        /// /Views/+ViewLocationExpanders
        /// </summary>
        public string ThemeName { get; set; } = "Default";

        public HttpContext Context { get; set; }


        public void PopulateValues([FromServices]ViewLocationExpanderContext context)
        {
            Context = Context ?? context.ActionContext.HttpContext;
            context.Values["theme"] = ThemeName;
        }

        /// <summary>
        /// 主题切换
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
                                                               IEnumerable<string> viewLocations)
        {
            string cookvalue = "";
            if (Context != null)
            {
                if (Context.Request.Cookies.TryGetValue("theme", out cookvalue))
                {
                    if (!string.IsNullOrEmpty(cookvalue))
                    {
                        ThemeName = cookvalue;
                    }
                }
            }
            var n = "";
            //return viewLocations.Where(d => !d.Equals("Views/Shared")).Select(f => f.Replace("Views", "Views/Theme/" + context.Values["theme"] + "/"));
            foreach (var item in viewLocations)
            {
                n = item;
                if (!item.Contains("/Views/Shared"))
                {
                    n = n.Replace("Views", "Views/Theme/" + context.Values["theme"] + "/"); ;
                }
                yield return n;
            }
        }
    }
}
