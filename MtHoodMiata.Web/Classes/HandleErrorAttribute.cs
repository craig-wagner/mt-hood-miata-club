#region using
using System;
using System.Web;
using System.Web.Mvc;
using Elmah;
#endregion using

namespace MtHoodMiata.Web
{
    public class HandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
    {
        public override void OnException( ExceptionContext context )
        {
            base.OnException( context );

            Exception ex = context.Exception;

            if( context.ExceptionHandled        // if unhandled, will be logged anyhow
                && !RaiseErrorSignal( ex )      // prefer signaling, if possible
                && !IsFiltered( context ) )     // filtered?
            {
                LogException( ex );
            }
        }

        private static bool RaiseErrorSignal( Exception ex )
        {
            bool raiseErrorSignal = false;
            HttpContext context = HttpContext.Current;

            if( context != null )
            {
                ErrorSignal signal = ErrorSignal.FromContext( context );

                if( signal != null )
                {
                    signal.Raise( ex, context );

                    raiseErrorSignal = true;
                }
            }

            return raiseErrorSignal;
        }

        private static bool IsFiltered( ExceptionContext context )
        {
            bool isFiltered = false;

            ErrorFilterConfiguration config =
                context.HttpContext.GetSection( "elmah/errorFilter" ) as ErrorFilterConfiguration;

            if( config != null )
            {
                ErrorFilterModule.AssertionHelperContext testContext =
                    new ErrorFilterModule.AssertionHelperContext( context.Exception, HttpContext.Current );

                isFiltered = config.Assertion.Test( testContext );
            }

            return isFiltered;
        }

        private static void LogException( Exception ex )
        {
            HttpContext context = HttpContext.Current;
            ErrorLog.GetDefault( context ).Log( new Error( ex, context ) );
        }
    }
}