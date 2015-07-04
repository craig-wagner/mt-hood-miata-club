#region using
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#endregion using

namespace MtHoodMiata.Web.Models
{
    [MetadataType( typeof( MembershipMetadata ) )]
    public partial class Membership
    {
        public string Member1FullName
        {
            get { return Member1LastName + ", " + Member1FirstName; }
        }

        public string Member2FullName
        {
            get
            {
                string fullName = String.Empty;

                if( !String.IsNullOrEmpty( Member2FirstName ) )
                {
                    fullName = Member2LastName + ", " + Member2FirstName;
                }

                return fullName;
            }
        }

        public bool IsActive
        {
            get
            {
                DateTime cutoff = DateTime.Today.AddDays( -30 );

                return MembershipDueDate > cutoff;
            }
        }
    }

    public class MembershipMetadata
    {
        [Required( ErrorMessage = "Primary member's first name is required." )]
        [StringLength( 50, MinimumLength = 2 )]
        [DisplayName( "First Name" )]
        public string Member1FirstName { get; set; }

        [Required( ErrorMessage = "Primary member's last name is required." )]
        [StringLength( 50, MinimumLength = 2 )]
        [DisplayName( "Last Name" )]
        public string Member1LastName { get; set; }

        [Required( ErrorMessage = "Primary member's email address is required." )]
        [RegularExpression( @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@(([\w-]+\.)+[a-zA-Z]{2,4})$", ErrorMessage = "Primary member's email address is not formatted correctly." )]
        [DisplayName( "Email Address" )]
        public string Member1Email { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Primary member's Cell Phone must be 10 digits with no formatting (e.g. 5035551212)." )]
        [DisplayName( "Cell Phone" )]
        public string Member1CellPhone { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Primary member's Work Phone must be 10 digits with no formatting (e.g. 5035551212)." )]
        [DisplayName( "Work Phone" )]
        public string Member1WorkPhone { get; set; }

        [RegularExpression( "^(0[1-9]|1[0-2])(0[1-9]|(1|2)[0-9]|3[0-1])$", ErrorMessage = "Primary member birth date not in correct format." )]
        [DisplayName( "Birth Date" )]
        public string Member1Dob { get; set; }

        [Required]
        [UsernameIsUnique( ErrorMessage = "Primary member username must be unique in the system." )]
        [DisplayName( "Username" )]
        public string Member1Username { get; set; }

        [Required]
        [DisplayName( "Password" )]
        public string Member1Password { get; set; }

        [DisplayName( "Board Member" )]
        public bool IsMember1BoardMember { get; set; }

        [StringLength( 50, MinimumLength = 2 )]
        [DisplayName( "First Name" )]
        public string Member2FirstName { get; set; }

        [StringLength( 50, MinimumLength = 2 )]
        [DisplayName( "Last Name" )]
        public string Member2LastName { get; set; }

        [RegularExpression( @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@(([\w-]+\.)+[a-zA-Z]{2,4})$", ErrorMessage = "Significant other's email address is not formatted correctly." )]
        [DisplayName( "Email Address" )]
        public string Member2Email { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Significant other's Cell Phone must be 10 digits with no formatting (e.g. 5035551212)." )]
        [DisplayName( "Cell Phone" )]
        public string Member2CellPhone { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Significant other's Work Phone must be 10 digits with no formatting (e.g. 5035551212)." )]
        [DisplayName( "Work Phone" )]
        public string Member2WorkPhone { get; set; }

        [RegularExpression( "^(0[1-9]|1[0-2])(0[1-9]|(1|2)[0-9]|3[0-1])$", ErrorMessage = "Significant other birth date not in correct format." )]
        [DisplayName( "Birth Date" )]
        public string Member2Dob { get; set; }

        [UsernameIsUnique( ErrorMessage = "Significant other username must be unique in the system." )]
        [DisplayName( "Username" )]
        public string Member2Username { get; set; }

        [DisplayName( "Password" )]
        public string Member2Password { get; set; }

        [DisplayName( "Board Member" )]
        public bool IsMember2BoardMember { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Home Phone must be 10 digits with no formatting (e.g. 5035551212)." )]
        [DisplayName( "Home Phone" )]
        public string HomePhone { get; set; }

        [Required( ErrorMessage = "Street address is required." )]
        [DisplayName( "Address" )]
        public string AddressLine1 { get; set; }

        [Required( ErrorMessage = "City is required." )]
        public string City { get; set; }

        [Required( ErrorMessage = "State is required." )]
        public string State { get; set; }

        [Required( ErrorMessage = "Zip code is required." )]
        [RegularExpression( "\\d{5}(-\\d{4})?", ErrorMessage = "Zip code must be ##### or #####-####." )]
        public string Zip { get; set; }

        [RegularExpression( "^(0[1-9]|1[0-2])(0[1-9]|(1|2)[0-9]|3[0-1])$", ErrorMessage = "Anniversary date not in correct format." )]
        [DisplayName( "Anniversary Date" )]
        public string AnniversaryDate { get; set; }

        [DisplayName( "Newsletter Delivery" )]
        public string NewsletterPreference { get; set; }

        [DisplayName( "Show In Roster" )]
        public string ShowInRoster { get; set; }

        [Required( ErrorMessage = "Next renewal is required." )]
        [DisplayName( "Next Renewal" )]
        public DateTime MembershipDueDate { get; set; }
    }
}