#region using
using System.ComponentModel;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public class MembershipApplicationCarViewModel
    {
        public int Year { get; set; }
        public string Color { get; set; }

        [DisplayName( "License Plate" )]
        public string LicensePlate { get; set; }
        public string Nickname { get; set; }
        public string Modifications { get; set; }
    }
}