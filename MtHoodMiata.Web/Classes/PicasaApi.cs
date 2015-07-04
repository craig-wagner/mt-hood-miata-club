#region using
using Google.GData.Client;
using Google.GData.Photos;
using Google.Picasa;
using MtHoodMiata.Web.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#endregion using

namespace MtHoodMiata.Web
{
    public class PicasaApi : IPicasaApi
    {
        #region Fields
        private const string _user = "mthoodmiata";
        private const string _password = "mymiata99";
        private PicasaService _service;
        #endregion Fields

        #region Constructors
        public PicasaApi()
        {
            _service = new PicasaService( "MtHoodMiata-WebSite-1" );
            _service.setUserCredentials( _user, _password );
        }
        #endregion Constructors

        #region Public Methods
        public Album GetAlbum( string albumId )
        {
            AlbumQuery query = new AlbumQuery( PicasaQuery.CreatePicasaUri( _user ) );
            PicasaFeed albumFeed = _service.Query( query );
            Album albumToReturn = null;

            if( albumFeed != null )
            {
                foreach( PicasaEntry entry in albumFeed.Entries )
                {
                    Album temp = new Album() { AtomEntry = entry };

                    if( temp.Id == albumId )
                    {
                        albumToReturn = temp;
                        break;
                    }
                }
            }

            return albumToReturn;
        }

        public Album FindAlbum( string albumName )
        {
            AlbumQuery query = new AlbumQuery( PicasaQuery.CreatePicasaUri( _user ) );
            PicasaFeed albumFeed = _service.Query( query );
            Album albumToReturn = null;

            if( albumFeed != null )
            {
                AtomEntry entry = albumFeed.Entries.Where( e => e.Title.Text == albumName ).SingleOrDefault();

                if( entry != null )
                {
                    albumToReturn = new Album() { AtomEntry = entry };
                }
            }

            return albumToReturn;
        }

        public IEnumerable<Album> GetAlbums()
        {
            AlbumQuery query = new AlbumQuery( PicasaQuery.CreatePicasaUri( _user ) );
            PicasaFeed picasaFeed = _service.Query( query );

            List<Album> albums = new List<Album>();

            if( picasaFeed != null )
            {
                foreach( PicasaEntry entry in picasaFeed.Entries )
                {
                    albums.Add( new Album() { AtomEntry = entry } );
                }
            }

            return albums;
        }

        public IEnumerable<Photo> GetPhotos( string albumId )
        {
            string uri = PicasaQuery.CreatePicasaUri( _user, albumId ) + "?imgmax=" + Settings.Default.MaxImageSize;

            PhotoQuery photoQuery = new PhotoQuery( uri );
            PicasaFeed photoFeed = _service.Query( photoQuery );

            List<Photo> photos = new List<Photo>();

            if( photoFeed != null )
            {
                foreach( PicasaEntry entry in photoFeed.Entries )
                {
                    photos.Add( new Photo() { AtomEntry = entry } );
                }
            }

            return photos;
        }

        public Album AddAlbum( string albumName )
        {
            AlbumEntry newAlbum = new AlbumEntry();
            newAlbum.Title.Text = albumName;

            AlbumAccessor accessor = new AlbumAccessor( newAlbum );
            accessor.Access = "public";

            Uri uri = new Uri( PicasaQuery.CreatePicasaUri( _user ) );

            PicasaEntry entry = (PicasaEntry)_service.Insert( uri, newAlbum );

            Album album = new Album() { AtomEntry = entry };

            return album;
        }

        public Photo AddPhoto( string albumId, Stream fileStream )
        {
            Uri uri = new Uri( PicasaQuery.CreatePicasaUri( _user, albumId ) );

            PicasaEntry entry = (PicasaEntry)_service.Insert(
                uri, fileStream, "image/jpeg", Guid.NewGuid().ToString() );

            Photo photo = new Photo() { AtomEntry = entry };

            return photo;
        }
        #endregion Public Methods
    }
}