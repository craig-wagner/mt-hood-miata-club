#region using
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web
{
    public class UsernameIsUniqueAttribute : ValidationAttribute
    {
        #region Constructors
        public UsernameIsUniqueAttribute()
        {
            ErrorMessage = "{0} must be unique in the system.";
        }
        #endregion Constructors

        protected override ValidationResult IsValid( object value, ValidationContext validationContext )
        {
            ValidationResult result = ValidationResult.Success;

            if( value != null )
            {
                string username = value.ToString();

                PropertyInfo propertyInfo =
                    validationContext.ObjectType.GetProperty( "MembershipId" );

                int membershipId = 0;

                if( propertyInfo != null )
                {
                    membershipId = (int)propertyInfo.GetValue( validationContext.ObjectInstance, null );
                }

                using( MtHoodMiataRepository repository = new MtHoodMiataRepository() )
                {
                    List<Membership> memberships =
                        repository.QueryMemberships( m => m.MembershipId != membershipId &&
                            (m.Member1Username == username || m.Member2Username == username) ).ToList();

                    if( memberships.Count > 0 )
                    {
                        result = new ValidationResult( FormatErrorMessage( validationContext.DisplayName ) );
                    }
                }
            }

            return result;
        }
    }
}