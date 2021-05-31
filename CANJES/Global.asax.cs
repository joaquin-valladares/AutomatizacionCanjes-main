using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CANJES
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            string sitiolocal = "http://localhost:63061/";
            string sitioQA = "http://172.16.2.127:8991/";
            string sitioPRD = "http://172.16.2.128:8991/";
            Application["sitio"]= sitioPRD;
        }
    }
}
