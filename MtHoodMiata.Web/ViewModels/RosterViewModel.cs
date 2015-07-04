#region using
using System.Collections.Generic;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public class RosterViewModel
    {
        public IEnumerable<Membership> Memberships { get; set; }
        public bool ShowCurrentMemberInRoster { get; set; }
    }
}