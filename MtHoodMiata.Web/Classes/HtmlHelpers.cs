#region using
using System;
using System.Web.Mvc;
#endregion using

namespace MtHoodMiata.Web
{
    public static class HtmlHelpers
    {
        public static string PhoneNumber( this HtmlHelper helper, string phoneNumber )
        {
            return String.Format( "({0}) {1}-{2}",
                phoneNumber.Substring( 0, 3 ),
                phoneNumber.Substring( 3, 3 ),
                phoneNumber.Substring( 6, 4 ) );
        }
    }
}