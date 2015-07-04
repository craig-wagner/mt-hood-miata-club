#region using
using System;
using System.ComponentModel.DataAnnotations;
#endregion using

namespace MtHoodMiata.Web
{
    public sealed class TodayOrFutureAttribute : ValidationAttribute
    {
        #region Constructors
        public TodayOrFutureAttribute()
        {
            ErrorMessage = "{0} must be on or after today's date.";
        }
        #endregion Constructors

        #region Public Methods
        public override string FormatErrorMessage( string name )
        {
            return String.Format( ErrorMessage, name );
        }

        public override bool IsValid( object value )
        {
            DateTime dateStart = (DateTime)value;

            return dateStart.Date >= DateTime.Now.Date;
        }
        #endregion Public Methods
    }
}