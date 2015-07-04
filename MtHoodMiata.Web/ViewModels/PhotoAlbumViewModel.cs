#region using

#endregion using

using System.Collections.Generic;
namespace MtHoodMiata.Web.ViewModels
{
    public class PhotoAlbumViewModel
    {
        public PhotoAlbumViewModel()
        {
            Photos = new List<Photo>();
        }

        public string AlbumTitle { get; set; }
        public string AlbumUri { get; set; }
        public ICollection<Photo> Photos { get; private set; }
    }

    public class Photo
    {
        public string PhotoUri { get; set; }
        public string ThumbnailUri { get; set; }
        public string Caption { get; set; }
    }
}