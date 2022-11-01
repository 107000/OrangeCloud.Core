using System;
using System.Collections.Generic;
using System.Text;

namespace OrangeCloud.Core
{
    public static class DI
    {
        public static IServiceProvider ServiceProvider
        {
            get;set;
        }

        /*

           接着就可以在Startup 类中进行调用。

           默认情况下如果在MVC项目中直接调用  UseServiceProvider() 即可。

           public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
           {
               app.UseServiceProvider();
           }

           在没有注入 HttpContextAccessor的项目中，AddServiceProviderAccessor 方法中调用

           services.AddServiceProviderAccessor();

           然后就可以在其他地方使用DI.***。

        */
    }
}
