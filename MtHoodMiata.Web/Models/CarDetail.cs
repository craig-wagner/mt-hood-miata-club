#region using
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#endregion using

namespace MtHoodMiata.Web.Models
{
    [MetadataType( typeof( CarDetailMetadata ) )]
    public partial class CarDetail { }

    public class CarDetailMetadata
    {
        [DisplayName( "Color" )]
        public int ColorId { get; set; }

        [DisplayName( "Special Edition" )]
        public bool SpecialEdition { get; set; }
    }
}