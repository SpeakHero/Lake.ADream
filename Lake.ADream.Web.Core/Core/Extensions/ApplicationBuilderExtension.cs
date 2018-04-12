using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Web.Core.Core.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder ADreamUsed(this IApplicationBuilder builder)
        {
            builder.UseSession();
            builder.UseStaticFiles();
            return builder;
        }
    }
}
