using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project_Fin_Etude___Maalem_Bin_Idik
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /* Hide X-AspNetMvc-Version */
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        /* Hide Server */
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
        }
    }
}
