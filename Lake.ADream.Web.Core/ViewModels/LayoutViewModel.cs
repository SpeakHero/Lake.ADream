

using System;
using Microsoft.AspNetCore.Http;

namespace Lake.ADream.Web.Core.ViewModels
{
    public class LayoutViewModel
    {

        public LayoutViewModel(HttpContext context)
        {
            this.context = context;
        }
        public string Title { get; set; }
        public string Error { get; set; }

        public ISession GetSession()
        {
            return context.Session;
        }

        public bool HasError => !string.IsNullOrEmpty(this.Error);

        public HttpContext context { get; private set; }

        protected string GetUserId()
        {
            return  context.Request.Cookies["UserId"];
        }

        public void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Error = (ex is KnownException) ? ex.Message : ex.ToJson();
            }
        }
    }

    public class LayoutViewModel<T> : LayoutViewModel
    {
        public LayoutViewModel(HttpContext context) : base(context)
        {
        }

        public T Model { get; set; }
    }
}