#region using
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class MeetingController : BaseController
    {
        #region Constructors
        public MeetingController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        internal MeetingController( IMtHoodMiataRepository repository )
            : base( repository ) { }
        #endregion Constructors

        #region Public Methods
        [OutputCache( Duration = 86400 )]
        public ActionResult Index()
        {
            IEnumerable<MeetingInfo> meetings =
                Repository.QueryMeetingInfos( m => m.MeetingDate >= DateTime.Today );

            return View( meetings );
        }

        [HttpGet]
        [Authorize( MustBeBoardMember = true )]
        public ActionResult List()
        {
            IEnumerable<MeetingInfo> meetings = Repository.GetMeetingInfos();

            ViewBag.InSearchMode = false;

            return View( meetings );
        }

        [HttpPost]
        [Authorize( MustBeBoardMember = true )]
        public ActionResult List( string searchString )
        {
            ActionResult result = null;

            if( String.IsNullOrWhiteSpace( searchString ) )
            {
                result = RedirectToAction( GetString.Of( () => List() ) );
            }
            else
            {
                IEnumerable<MeetingInfo> meetings =
                    Repository.QueryMeetingInfos( mi => mi.Location.Contains( searchString ) ||
                        mi.Notes.Contains( searchString ) );

                ViewBag.InSearchMode = true;

                result = View( meetings );
            }
            return result;
        }

        [Authorize( MustBeBoardMember = true )]
        public ActionResult Delete( int id )
        {
            MeetingInfo meetingInfo = Repository.GetMeetingInfoById( id );
            Repository.Remove( meetingInfo );
            Repository.SaveChanges();

            //return RedirectToAction( GetString.Of( () => List() ) );
            return Redirect( "/Meeting/List?sortdir=DESC" );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpGet]
        public ActionResult Copy( int id )
        {
            MeetingInfo meetingInfo = Repository.GetMeetingInfoById( id );
            meetingInfo.MeetingDate = DateTime.Now;

            return View( meetingInfo );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpPost]
        public ActionResult Copy( MeetingInfo model )
        {
            return CreateNewMeeting( model );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpGet]
        public ActionResult Create()
        {
            MeetingInfo meetingInfo = new MeetingInfo()
            {
                MeetingDate = DateTime.Now
            };

            return View( meetingInfo );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpPost]
        public ActionResult Create( MeetingInfo model )
        {
            return CreateNewMeeting( model );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpGet]
        public ActionResult Edit( int id )
        {
            MeetingInfo meetingInfo = Repository.GetMeetingInfoById( id );

            return View( meetingInfo );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpPost]
        public ActionResult Edit( int id, MeetingInfo model )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                MeetingInfo meetingInfo = Repository.GetMeetingInfoById( id );

                meetingInfo.Address = model.Address;
                meetingInfo.City = model.City;
                meetingInfo.Location = model.Location;
                meetingInfo.MapUrl = model.MapUrl;
                meetingInfo.MeetingDate = model.MeetingDate;
                meetingInfo.Notes = model.Notes;
                meetingInfo.Phone = model.Phone;
                meetingInfo.State = model.State;
                meetingInfo.Zip = model.Zip;
                meetingInfo.UpdatedDttm = DateTime.Now;

                Repository.SaveChanges();

                result = Redirect( "/Meeting/List?sortdir=DESC" );
            }
            else
            {
                result = View( model );
            }

            return result;
        }
        #endregion Public Methods

        #region Private Methods
        private ActionResult CreateNewMeeting( MeetingInfo model )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                model.UpdatedDttm = DateTime.Now;

                Repository.Add( model );
                Repository.SaveChanges();

                //result = RedirectToAction( GetString.Of( () => List() ) );
                result = Redirect( "/Meeting/List?sortdir=DESC" );
            }
            else
            {
                result = View( model );
            }

            return result;
        }
        #endregion Private Methods
    }
}
