#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
using MtHoodMiata.Web.ViewModels;

#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class AdController : BaseController
    {
        #region Constructors
        public AdController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        internal AdController( IMtHoodMiataRepository repository )
            : base( repository ) { }
        #endregion Constructors

        #region Public Methods
        [OutputCache( Duration = 600 )]
        public ActionResult Index()
        {
            DateTime cutOffDate = DateTime.Now.AddDays( -30 );

            IEnumerable<ClassifiedAd> ads = Repository.QueryClassifiedAds(
                ca => ca.AdPlacedDttm >= cutOffDate, "ClassifiedAdType" )
                .OrderBy( ca => ca.ClassifiedAdType.Sequence )
                .ThenBy( ca => ca.AdPlacedDttm );

            return View( ads );
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            ClassifiedAdViewModel viewModel = new ClassifiedAdViewModel();
            viewModel.AdTypes = Repository.GetClassifiedAdTypes();

            return View( viewModel );
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create( ClassifiedAdViewModel viewModel )
        {
            ActionResult result;

            if( ModelState.IsValid )
            {
                ClassifiedAd ad = new ClassifiedAd()
                {
                    MembershipId = this.MembershipId,
                    ClassifiedAdTypeCode = viewModel.ClassifiedAdTypeCode,
                    AdText = viewModel.AdText,
                    AdPlacedDttm = DateTime.Now
                };

                Repository.Add( ad );
                Repository.SaveChanges();

                TempData["AdsChanged"] = true;

                result = RedirectToAction( GetString.Of( () => List() ) );
            }
            else
            {
                viewModel.AdTypes = Repository.GetClassifiedAdTypes();
                result = View( viewModel );
            }

            return result;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit( int id )
        {
            ClassifiedAdViewModel viewModel = new ClassifiedAdViewModel();
            viewModel.AdTypes = Repository.GetClassifiedAdTypes();

            ClassifiedAd ad = Repository.GetClassifiedAdById( id );
            viewModel.ClassifiedAdTypeCode = ad.ClassifiedAdTypeCode;
            viewModel.AdText = Server.HtmlDecode( ad.AdText );

            return View( viewModel );
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit( int id, ClassifiedAdViewModel viewModel )
        {
            ActionResult result;

            if( ModelState.IsValid )
            {
                ClassifiedAd ad = Repository.GetClassifiedAdById( id );

                if( ad == null )
                {
                    ad = new ClassifiedAd() { MembershipId = this.MembershipId };
                    Repository.Add( ad );
                }

                ad.ClassifiedAdTypeCode = viewModel.ClassifiedAdTypeCode;
                ad.AdText = viewModel.AdText;
                ad.AdPlacedDttm = DateTime.Now;

                Repository.SaveChanges();

                TempData["AdsChanged"] = true;

                result = RedirectToAction( GetString.Of( () => List() ) );
            }
            else
            {
                viewModel.AdTypes = Repository.GetClassifiedAdTypes();
                result = View( viewModel );
            }

            return result;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete( int id )
        {
            ClassifiedAd ad = Repository.GetClassifiedAdById( id );
            Repository.Remove( ad );
            Repository.SaveChanges();

            TempData["AdsChanged"] = true;

            return RedirectToAction( GetString.Of( () => List() ) );
        }

        [HttpGet]
        [Authorize]
        public ActionResult Renew( int id )
        {
            ClassifiedAd ad = Repository.GetClassifiedAdById( id );
            ad.AdPlacedDttm = DateTime.Now;

            Repository.SaveChanges();

            return RedirectToAction( GetString.Of( () => List() ) );
        }

        [HttpGet]
        [Authorize]
        public ActionResult List()
        {
            ViewBag.AdTypes = Repository.GetClassifiedAdTypes();
            IEnumerable<ClassifiedAd> ads = Repository.QueryClassifiedAds(
                ca => ca.Membership.MembershipId == this.MembershipId, "ClassifiedAdType" );

            ViewBag.AdsChanged = TempData.ContainsKey( "AdsChanged" ) ? TempData["AdsChanged"] : false;

            return View( ads );
        }
        #endregion Public Methods
    }
}
