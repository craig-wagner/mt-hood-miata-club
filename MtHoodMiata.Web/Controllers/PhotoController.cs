#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Google.Picasa;
using MtHoodMiata.Web.Models;
using MtHoodMiata.Web.ViewModels;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class PhotoController : BaseController
    {
        #region Fields
        private IPicasaApi _picasa;
        #endregion Fields

        #region Properties
        private IPicasaApi Picasa
        {
            get
            {
                if( _picasa == null )
                {
                    _picasa = new PicasaApi();
                }

                return _picasa;
            }
        }
        #endregion Properties

        #region Constructors
        public PhotoController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        /// <param name="picasa">
        /// An instance of a class that implements the IPicasaApi interface. This will
        /// typically be a mock for unit testing.
        /// </param>
        internal PhotoController( IMtHoodMiataRepository repository, IPicasaApi picasa )
            : base( repository )
        {
            _picasa = picasa;
        }
        #endregion Constructors

        public ActionResult Index()
        {
            IEnumerable<Album> albums = Picasa.GetAlbums()
                .OrderByDescending( a => a.Updated )
                .ThenBy( a => a.Title );

            return View( albums );
        }

        public ActionResult Album( string id )
        {
            Album album = Picasa.GetAlbum( id );
            IEnumerable<Google.Picasa.Photo> photos = Picasa.GetPhotos( id );

            PhotoAlbumViewModel viewModel = new PhotoAlbumViewModel()
            {
                AlbumTitle = album.Title,
                AlbumUri = album.PicasaEntry.AlternateUri.Content
            };

            foreach( Google.Picasa.Photo photo in photos )
            {
                viewModel.Photos.Add( new MtHoodMiata.Web.ViewModels.Photo()
                {
                    Caption = photo.PicasaEntry.Summary.Text,
                    PhotoUri = photo.PhotoUri.AbsoluteUri,
                    ThumbnailUri = photo.PicasaEntry.Media.Thumbnails[1].Url
                } );
            }

            return Json( viewModel, JsonRequestBehavior.AllowGet );
        }

        [Authorize]
        public ActionResult Upload()
        {
            List<Album> albums = Picasa.GetAlbums().OrderBy( a => a.Title ).ToList();
            albums.Add( new Album()
            {
                Title = "A new album...",
                Updated = DateTime.MinValue
            } );

            IEnumerable<EventInfo> events = Repository.QueryEventInfos( e => e.StartDate <= DateTime.Now );
            List<SelectListItem> items = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "-- Select an Event --" }
            };

            foreach( EventInfo theEvent in events.OrderByDescending( e => e.StartDate ) )
            {
                items.Add( new SelectListItem()
                {
                    Text = theEvent.EventName + theEvent.StartDate.ToString( " (yyyy/MM/dd)" ),
                    Value = theEvent.EventId.ToString()
                } );
            }

            UploadPhotosViewModel viewModel = new UploadPhotosViewModel()
            {
                Albums = albums,
                Events = new SelectList( items, "Value", "Text" ),
                MemberName = this.FirstName + " " + this.LastName
            };

            return View( viewModel );
        }
    }
}
