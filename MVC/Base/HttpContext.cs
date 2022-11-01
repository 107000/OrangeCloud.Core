using Microsoft.AspNetCore.Http;
using System.Text;

namespace OrangeCloud.Core
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _accessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public static string Url(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }

        /*
         
            接着就可以在Startup 类中进行调用。

            默认情况下如果在MVC项目中直接调用  UseStaticHttpContext() 即可。

            public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
            {
                app.UseStaticHttpContext();
            }
            
            在没有注入 HttpContextAccessor的项目中，还需在ConfigureServices 方法中调用

            services.AddHttpContextAccessor();
         
            然后就可以在其他地方使用HttpContext.Current。

         */
    }
}
