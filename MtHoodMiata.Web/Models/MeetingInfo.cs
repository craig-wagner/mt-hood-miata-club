#region using
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
#endregion using

namespace MtHoodMiata.Web.Models
{
    [MetadataType( typeof( MeetingInfoMetadata ) )]
    public partial class MeetingInfo { }

    public class MeetingInfoMetadata
    {
        #region Properties
        [Required( ErrorMessage = "Meeting Date is required." )]
        [TodayOrFuture]
        [DisplayName( "Meeting Date" )]
        public object MeetingDate { get; set; }

        [Required( ErrorMessage = "Address is required." )]
        public string Address { get; set; }

        [Required( ErrorMessage = "City is required." )]
        public string City { get; set; }

        [Required( ErrorMessage = "Location is required." )]
        public string Location { get; set; }

        [Required( ErrorMessage = "Map URL is required." )]
        [DisplayName( "Map URL" )]
        public string MapUrl { get; set; }

        [AllowHtml]
        public string Notes { get; set; }

        [RegularExpression( "\\d{10}", ErrorMessage = "Phone number must be 10 digits with no formatting (e.g. 5035551212)." )]
        public string Phone { get; set; }

        [Required( ErrorMessage = "State is required." )]
        public string State { get; set; }

        [RegularExpression( "\\d{5}", ErrorMessage = "Zip code must be five digits with no formatting (e.g. 97224)." )]
        public string Zip { get; set; }
        #endregion Properties
    }

}