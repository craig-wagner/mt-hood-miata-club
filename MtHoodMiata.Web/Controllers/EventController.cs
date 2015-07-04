#region using
using MtHoodMiata.Web.Models;
using MtHoodMiata.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class EventController : BaseController
    {
        #region Constructors
        public EventController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        internal EventController( MtHoodMiataRepository repository )
            : base( repository ) { }
        #endregion Constructors

        #region Public Methods
        [OutputCache( CacheProfile = "Static" )]
        public ActionResult Planning()
        {
            return View();
        }

        [OutputCache( Duration = 600, VaryByParam = "year" )]
        public ActionResult Index( int? year )
        {
            EventIndexViewModel model = new EventIndexViewModel();
            model.ModelContainsPastEvents = year.HasValue;
            model.EventsYear = year ?? DateTime.Now.Year;
            model.EventYears = Repository.GetEventYears();

            DateTime today = DateTime.Today;

            Expression<Func<EventInfo, bool>> condition = null;

            if( year.HasValue )
            {
                condition = e => e.StartDate < today && e.StartDate.Year == year;
            }
            else
            {
                condition = e => e.StartDate >= today;
            }

            model.Events = Repository.QueryEventInfos( condition )
                .OrderBy( e => e.StartDate );

            return View( model );
        }

        public ActionResult Detail( int id )
        {
            ActionResult result = null;

            EventInfo theEvent = Repository.GetEventInfoById( id );

            if( theEvent != null )
            {
                result = View( theEvent );
            }
            else
            {
                result = RedirectToAction( "NotFound", "Error" );
            }

            return result;
        }

        [OutputCache( CacheProfile = "Static" )]
        public ActionResult Checklist()
        {
            string filePath = Server.MapPath( "/Content/Event" );
            filePath = Path.Combine( filePath, "MiataClubRunChecklist.pdf" );
            return new FilePathResult( filePath, "application/pdf" );
        }

        [HttpGet]
        [Authorize( MustBeBoardMember = true )]
        public ActionResult List()
        {
            IEnumerable<EventInfo> events = Repository.GetEventInfos();

            ViewBag.InSearchMode = false;

            return View( events );
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
                IEnumerable<EventInfo> events = Repository.QueryEventInfos( ei =>
                    ei.EventDetails.Contains( searchString ) ||
                    ei.EventName.Contains( searchString ) ||
                    ei.Planner.Contains( searchString ) );

                ViewBag.InSearchMode = true;

                result = View( events );
            }
            return result;
        }

        [Authorize( MustBeBoardMember = true )]
        public ActionResult Delete( int id )
        {
            EventInfo eventInfo = Repository.GetEventInfoById( id );
            Repository.Remove( eventInfo );
            Repository.SaveChanges();

            return Redirect( "/Event/List?sortdir=DESC" );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpGet]
        public ActionResult Copy( int id )
        {
            EventInfo eventInfo = Repository.GetEventInfoById( id );
            eventInfo.StartDate = DateTime.Now;
            eventInfo.EndDate = DateTime.Now;

            return View( eventInfo );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpPost]
        public ActionResult Copy( EventInfo model )
        {
            return CreateNewEvent( model );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpGet]
        public ActionResult Create()
        {
            EventInfo eventInfo = new EventInfo()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            return View( eventInfo );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpPost]
        public ActionResult Create( EventInfo model )
        {
            return CreateNewEvent( model );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpGet]
        public ActionResult Edit( int id )
        {
            EventInfo eventInfo = Repository.GetEventInfoById( id );

            return View( eventInfo );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpPost]
        public ActionResult Edit( int id, EventInfo model )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                EventInfo eventInfo = Repository.GetEventInfoById( id );

                eventInfo.EndDate = model.EndDate;
                eventInfo.EventDetails = model.EventDetails;
                eventInfo.EventName = model.EventName;
                eventInfo.Planner = model.Planner;
                eventInfo.StartDate = model.StartDate;
                eventInfo.UpdatedDttm = DateTime.Now;

                Repository.SaveChanges();

                result = Redirect( "/Event/List?sortdir=DESC" );
            }
            else
            {
                result = View( model );
            }

            return result;
        }
        #endregion Public Methods

        #region Private Methods
        private ActionResult CreateNewEvent( EventInfo model )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                model.UpdatedDttm = DateTime.Now;

                Repository.Add( model );
                Repository.SaveChanges();

                result = Redirect( "/Event/List?sortdir=DESC" );
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
