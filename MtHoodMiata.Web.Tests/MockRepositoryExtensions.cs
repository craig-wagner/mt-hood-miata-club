#region using
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;
#endregion using

namespace MtHoodMiata.Web.Tests
{
    #region Enumerations
    public enum FakeType
    {
        Dynamic,
        Strict,
        Partial,
        Stub
    }
    #endregion Enumerations

    public static class MockRepositoryExtensions
    {
        #region Public Methods
        public static HttpContextBase FakeHttpContext( this MockRepository mocks, FakeType fakeType )
        {
            HttpContextBase context = null;
            HttpRequestBase request = null;
            HttpResponseBase response = null;
            HttpSessionStateBase session = null;
            HttpServerUtilityBase server = null;

            switch( fakeType )
            {
                case FakeType.Dynamic:
                    DynamicHttpContext( mocks, out context, out request, out response, out session, out server );
                    break;

                case FakeType.Partial:
                    PartialHttpContext( mocks, out context, out request, out response, out session, out server );
                    break;

                case FakeType.Strict:
                    StrictHttpContext( mocks, out context, out request, out response, out session, out server );
                    break;

                case FakeType.Stub:
                    StubHttpContext( mocks, out context, out request, out response, out session, out server );
                    break;

                default:
                    throw new NotSupportedException( String.Format( "FakeType value of '{0}' is not supported.", fakeType ) );
            }

            SetupResult
                .For( context.Request )
                .Return( request );
            SetupResult
                .For( context.Response )
                .Return( response );
            SetupResult
                .For( context.Session )
                .Return( session );
            SetupResult
                .For( context.Server )
                .Return( server );

            mocks.Replay( context );

            return context;
        }

        /// <summary>
        /// Attach a fake HttpContext of the type specified to the controller's 
        /// ControllerContext property.
        /// </summary>
        /// <param name="mocks">
        /// An instance of the Rhino MockRepository class.
        /// </param>
        /// <param name="controller">
        /// An instance of an MVC controller to which the fake HttpContext
        /// should be attached.
        /// </param>
        /// <param name="fakeType">
        /// A member of the FakeType enumeration identifying what type of
        /// fake to create.
        /// </param>
        public static void SetFakeControllerContext( this MockRepository mocks, Controller controller, FakeType fakeType = FakeType.Dynamic )
        {
            var httpContext = mocks.FakeHttpContext( fakeType );
            ControllerContext context = new ControllerContext( new RequestContext( httpContext, new RouteData() ), controller );
            controller.ControllerContext = context;
        }
        #endregion Public Methods

        #region Private Methods
        private static void DynamicHttpContext( MockRepository mocks, out HttpContextBase context, out HttpRequestBase request, out HttpResponseBase response, out HttpSessionStateBase session, out HttpServerUtilityBase server )
        {
            context = mocks.DynamicMock<HttpContextBase>();
            request = mocks.DynamicMock<HttpRequestBase>();
            response = mocks.DynamicMock<HttpResponseBase>();
            session = mocks.DynamicMock<HttpSessionStateBase>();
            server = mocks.DynamicMock<HttpServerUtilityBase>();
        }

        private static void StrictHttpContext( MockRepository mocks, out HttpContextBase context, out HttpRequestBase request, out HttpResponseBase response, out HttpSessionStateBase session, out HttpServerUtilityBase server )
        {
            context = mocks.StrictMock<HttpContextBase>();
            request = mocks.StrictMock<HttpRequestBase>();
            response = mocks.StrictMock<HttpResponseBase>();
            session = mocks.StrictMock<HttpSessionStateBase>();
            server = mocks.StrictMock<HttpServerUtilityBase>();
        }

        private static void PartialHttpContext( MockRepository mocks, out HttpContextBase context, out HttpRequestBase request, out HttpResponseBase response, out HttpSessionStateBase session, out HttpServerUtilityBase server )
        {
            context = mocks.PartialMock<HttpContextBase>();
            request = mocks.PartialMock<HttpRequestBase>();
            response = mocks.PartialMock<HttpResponseBase>();
            session = mocks.PartialMock<HttpSessionStateBase>();
            server = mocks.PartialMock<HttpServerUtilityBase>();
        }

        private static void StubHttpContext( MockRepository mocks, out HttpContextBase context, out HttpRequestBase request, out HttpResponseBase response, out HttpSessionStateBase session, out HttpServerUtilityBase server )
        {
            context = mocks.Stub<HttpContextBase>();
            request = mocks.Stub<HttpRequestBase>();
            response = mocks.Stub<HttpResponseBase>();
            session = mocks.Stub<HttpSessionStateBase>();
            server = mocks.Stub<HttpServerUtilityBase>();
        }
        #endregion Private Methods
    }
}