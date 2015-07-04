#region using
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public class MembershipApplicationViewModel
    {
        public IEnumerable<int> ModelYears { get; set; }

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

        [RegularExpression( "^(0[1-9]|1[0-2])/(0[1-9]|(1|2)[0-9]|3[0-1])$", ErrorMessage = "Primary member birthday not in correct format." )]
        [DisplayName( "Birthday" )]
        public string Member1Dob { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Primary member's Cell Phone must be 10 digits with no formatting (e.g. 5035551212)." )]
        [DisplayName( "Cell Phone" )]
        public string Member1CellPhone { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Primary member's Work Phone must be 10 digits with no formatting (e.g. 5035551212)." )]
        [DisplayName( "Work Phone" )]
        public string Member1WorkPhone { get; set; }

        [DisplayName( "First Name" )]
        public string Member2FirstName { get; set; }

        [DisplayName( "Last Name" )]
        public string Member2LastName { get; set; }

        [RegularExpression( @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@(([\w-]+\.)+[a-zA-Z]{2,4})$", ErrorMessage = "Significant other's email address is not formatted correctly." )]
        [DisplayName( "Email Address" )]
        public string Member2Email { get; set; }

        [RegularExpression( "^(0[1-9]|1[0-2])/(0[1-9]|(1|2)[0-9]|3[0-1])$", ErrorMessage = "Significant other birthday not in correct format." )]
        [DisplayName( "Birthday" )]
        public string Member2Dob { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Member 2 Cell Phone must be 10 digits with no formatting (e.g. 5035551212)." )]
        [DisplayName( "Cell Phone" )]
        public string Member2CellPhone { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Member 2 Work Phone must be 10 digits with no formatting (e.g. 5035551212)." )]
        [DisplayName( "Work Phone" )]
        public string Member2WorkPhone { get; set; }

        [DisplayName( "Anniversary Date" )]
        public string WeddingAnniversaryDate { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Home Phone must be 10 digits with no formatting (e.g. 5035551212)." )]
        [DisplayName( "Home Phone" )]
        public string HomePhone { get; set; }

        [Required( ErrorMessage = "Street address is required." )]
        [DisplayName( "Street Address" )]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [Required( ErrorMessage = "City is required." )]
        public string City { get; set; }

        [Required( ErrorMessage = "State is required." )]
        public string State { get; set; }

        [Required( ErrorMessage = "Zip code is required." )]
        public string Zip { get; set; }

        public MembershipApplicationCarViewModel Car1 { get; set; }
        public MembershipApplicationCarViewModel Car2 { get; set; }
        public MembershipApplicationCarViewModel Car3 { get; set; }

        public string Comments { get; set; }
    }
}