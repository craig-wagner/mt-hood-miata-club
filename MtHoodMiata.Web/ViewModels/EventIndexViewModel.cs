#region using
using System.Collections.Generic;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public class EventIndexViewModel
    {
        public bool ModelContainsPastEvents { get; set; }
        public int? EventsYear { get; set; }
        public IEnumerable<EventInfo> Events { get; set; }
        public IEnumerable<int> EventYears { get; set; }
    }
}