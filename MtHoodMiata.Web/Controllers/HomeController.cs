#region using
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
using MtHoodMiata.Web.Properties;
using MtHoodMiata.Web.ViewModels;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Fields
        private IHitCounter _hitCounter;
        #endregion Fields

        #region Properties
        private IHitCounter HitCounter
        {
            get
            {
                if( _hitCounter == null )
                {
                    _hitCounter = new HitCounter();
                }

                return _hitCounter;
            }
        }
        #endregion Properties

        #region Constructors
        public HomeController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        internal HomeController( IMtHoodMiataRepository repository )
            : base( repository ) { }
        #endregion Constructors

        #region Public Methods
        public ActionResult Index()
        {
            IndexViewModel viewModel = new IndexViewModel();
            GetHitCount( viewModel );
            GetMembershipCounts( viewModel );
            GetCarCounts( viewModel );
            GetNextMeeting( viewModel );
            GetUpcomingEvents( viewModel );
            GetWhatsNew( viewModel );

            return View( viewModel );
        }

        [OutputCache( CacheProfile = "Static" )]
        public ActionResult OtherClubs()
        {
            return View();
        }

        [OutputCache( CacheProfile = "Static" )]
        public ActionResult Apparel()
        {
            return View();
        }

        [OutputCache( CacheProfile = "Static" )]
        public ActionResult AboutUs()
        {
            return View();
        }

        [OutputCache( CacheProfile = "Static" )]
        public ActionResult ContactUs()
        {
            return View();
        }

        [OutputCache( CacheProfile = "Static" )]
        public ActionResult Vendors()
        {
            return View();
        }
        #endregion Public Methods

        #region Private Methods
        private void GetHitCount( IndexViewModel viewModel )
        {
            int hitCount = 0;

            if( !String.IsNullOrWhiteSpace( Request.UserAgent ) && !Request.UserAgent.Contains( "bot" ) )
            {
                hitCount = HitCounter.GetHitCount(
                    Path.Combine( Server.MapPath( "/" ), "count.txt" ) );
            }

            viewModel.HitCount = hitCount;
        }

        private void GetMembershipCounts( IndexViewModel viewModel )
        {
            IEnumerable<Membership> memberships = Repository.GetActiveMemberships();

            int membershipCount = memberships.Count();
            int secondMemberCount = memberships
                .Where( m => !String.IsNullOrEmpty( m.Member2FirstName ) )
                .Count();

            viewModel.MembershipCount = membershipCount;
            viewModel.MemberCount = membershipCount + secondMemberCount;
        }

        private void GetCarCounts( IndexViewModel viewModel )
        {
            IEnumerable<CarDetail> carDetails = Repository.GetActiveMembershipCars();

            viewModel.MiataCount = carDetails.Count();

            viewModel.Years = carDetails
                .OrderBy( cd => cd.Year )
                .GroupBy( cd => cd.Year.Substring( 2, 2 ), cd => cd.Year );

            viewModel.Colors = carDetails
                .OrderBy( cd => cd.Color.ColorFamily.ColorFamilyName )
                .GroupBy( cd => cd.Color.ColorFamily.ColorFamilyName, cd => cd.Color.ColorFamily.ColorFamilyName );
        }

        private void GetWhatsNew( IndexViewModel viewModel )
        {
            viewModel.UpdatedItems =
                Repository.GetWhatsNew( Settings.Default.WhatsNewLimit );
        }

        private void GetUpcomingEvents( IndexViewModel viewModel )
        {
            IEnumerable<EventInfo> events = Repository.GetUpcomingEvents( 2 );

            viewModel.UpcomingEvents = events;
        }

        private void GetNextMeeting( IndexViewModel viewModel )
        {
            MeetingInfo meetingInfo = Repository.GetNextMeeting();

            viewModel.UpcomingMeeting = meetingInfo;
        }
        #endregion Private Methods
    }
}
