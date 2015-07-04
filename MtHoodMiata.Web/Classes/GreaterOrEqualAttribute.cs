#region using
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
#endregion using

namespace MtHoodMiata.Web
{
    public sealed class GreaterOrEqualAttribute : ValidationAttribute
    {
        #region Fields
        public string OtherProperty { get; private set; }
        #endregion Fields

        #region Constructors
        public GreaterOrEqualAttribute( string otherProperty )
        {
            OtherProperty = otherProperty;
            ErrorMessage = "{0} must be greater than or equal to {1}.";
        }
        #endregion Constructors

        #region Public Methods
        public override string FormatErrorMessage( string name )
        {
            return String.Format( ErrorMessageString, name, OtherProperty );
        }
        #endregion Public Methods

        #region Protected Methods
        protected override ValidationResult IsValid( object value, ValidationContext validationContext )
        {
            ValidationResult result = ValidationResult.Success;

            IComparable firstComparable = value as IComparable;
            IComparable secondComparable = GetSecondComparable( validationContext );

            if( firstComparable != null && secondComparable != null )
            {
                if( firstComparable.CompareTo( secondComparable ) < 0 )
                {
                    result = new ValidationResult( FormatErrorMessage(
                        validationContext.DisplayName, GetOtherDisplayName( validationContext ) ) );
                }
            }

            return result;
        }
        #endregion Protected Methods

        #region Private Methods
        private string FormatErrorMessage( string firstProperty, string secondProperty )
        {
            return String.Format( ErrorMessageString, firstProperty, secondProperty );
        }

        private IComparable GetSecondComparable( ValidationContext validationContext )
        {
            IComparable returnValue = null;

            PropertyInfo propertyInfo =
                validationContext.ObjectType.GetProperty( OtherProperty );

            if( propertyInfo != null )
            {
                object secondValue = propertyInfo.GetValue( validationContext.ObjectInstance, null );
                returnValue = secondValue as IComparable;
            }

            return returnValue;
        }

        private string GetOtherDisplayName( ValidationContext validationContext )
        {
            string displayName = OtherProperty;

            ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForProperty(
                null, validationContext.ObjectType, OtherProperty );

            if( metadata != null )
            {
                displayName = metadata.GetDisplayName();
            }

            return displayName;
        }
        #endregion Private Methods
    }
}