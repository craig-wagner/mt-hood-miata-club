#region using
using System.Collections.Generic;
using System.Linq;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public class IndexViewModel
    {
        public int MemberCount { get; set; }
        public int MembershipCount { get; set; }
        public int MiataCount { get; set; }
        public int HitCount { get; set; }
        public IEnumerable<IGrouping<string, string>> Years { get; set; }
        public IEnumerable<IGrouping<string, string>> Colors { get; set; }

        public IEnumerable<EventInfo> UpcomingEvents { get; set; }
        public MeetingInfo UpcomingMeeting { get; set; }
        public IEnumerable<UpdatedItem> UpdatedItems { get; set; }
    }
}