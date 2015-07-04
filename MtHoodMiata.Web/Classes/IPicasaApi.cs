#region using
using System.Collections.Generic;
using System.IO;
using Google.Picasa;
#endregion using

namespace MtHoodMiata.Web
{
    interface IPicasaApi
    {
        Album FindAlbum( string albumName );
        Album GetAlbum( string albumId );
        IEnumerable<Album> GetAlbums();
        IEnumerable<Photo> GetPhotos( string albumId );
        Album AddAlbum( string albumName );
        Photo AddPhoto( string albumId, Stream fileStream );
    }
}
