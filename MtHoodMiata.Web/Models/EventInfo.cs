#region using
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
#endregion using

namespace MtHoodMiata.Web.Models
{
    [MetadataType( typeof( EventInfoMetadata ) )]
    public partial class EventInfo { }

    public class EventInfoMetadata
    {
        #region Properties
        [Required( ErrorMessage = "Start Date is required." )]
        [TodayOrFuture]
        [DisplayName( "Start Date" )]
        public object StartDate { get; set; }

        [Required( ErrorMessage = "End Date is required." )]
        [GreaterOrEqual( "StartDate" )]
        [DisplayName( "End Date" )]
        public object EndDate { get; set; }

        [Required( ErrorMessage = "Event Name is required." )]
        [DisplayName( "Event Name" )]
        public object EventName { get; set; }

        [Required( ErrorMessage = "Event Details are required." )]
        [DisplayName( "Event Details" )]
        [AllowHtml]
        public object EventDetails { get; set; }

        [Required( ErrorMessage = "Planner is required." )]
        public object Planner { get; set; }
        #endregion Properties
    }

}