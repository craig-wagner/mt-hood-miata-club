#region using
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Elmah;
#endregion using

namespace MtHoodMiata.Web
{
    public class Global : System.Web.HttpApplication
    {
        #region Event Handlers
        protected void Application_Start( object sender, EventArgs e )
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters( GlobalFilters.Filters );
            RegisterRoutes( RouteTable.Routes );
        }

        protected void Session_Start( object sender, EventArgs e )
        {
        }

        protected void Application_BeginRequest( object sender, EventArgs e )
        {
        }

        protected void Application_AuthenticateRequest( object sender, EventArgs e )
        {
        }

        protected void Application_Error( object sender, EventArgs e )
        {
        }

        protected void Session_End( object sender, EventArgs e )
        {
        }

        protected void Application_End( object sender, EventArgs e )
        {
        }

        public void ErrorMail_Filtering( object sender, ExceptionFilterEventArgs e )
        {
            HttpException httpException = e.Exception as HttpException;
            if( httpException != null && httpException.GetHttpCode() == 404 )
            {
                e.Dismiss();
            }
        }
        #endregion Event Handlers

        #region Private Methods
        private static void RegisterGlobalFilters( GlobalFilterCollection filters )
        {
            filters.Add( new HandleErrorAttribute() );
        }

        private static void RegisterRoutes( RouteCollection routes )
        {
            routes.IgnoreRoute( "{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" } );
            routes.IgnoreRoute( "{*allphp}", new { allaspx = @".*\.php(/.*)?" } );
            routes.IgnoreRoute( "{*robots}", new { robots = @"(.*/)?robots.txt(/.*)?" } );
            routes.IgnoreRoute( "{*favicon}", new { favicon = "(.*/)?favicon.ico(/.*)?" } );
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            routes.MapRoute(
                "EventList", "Event/Index/{year}",
                new { controller = "Event", action = "Index", year = UrlParameter.Optional } );

            routes.MapRoute(
                "Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } );
        }
        #endregion Private Methods
    }
}