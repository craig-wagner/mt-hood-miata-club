#region using
using System.Collections.Generic;
using System.Web.Mvc;
using Google.Picasa;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public class UploadPhotosViewModel
    {
        public IEnumerable<Album> Albums { get; set; }
        public SelectList Events { get; set; }

        public string AlbumName { get; set; }
        public string AlbumId { get; set; }
        public string EventId { get; set; }

        public string MemberName { get; set; }
    }
}