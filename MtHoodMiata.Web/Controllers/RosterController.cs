#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
using MtHoodMiata.Web.ViewModels;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class RosterController : BaseController
    {
        #region Constructors
        public RosterController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        internal RosterController( IMtHoodMiataRepository repository )
            : base( repository ) { }
        #endregion Constructors

        #region Public Methods
        [Authorize]
        public ActionResult List()
        {
            RosterViewModel viewModel = new RosterViewModel();

            if( this.ShowInRoster )
            {
                viewModel.Memberships = Repository.QueryActiveMemberships(
                    m => m.ShowInRoster, "CarDetails.Color" );
            }

            viewModel.ShowCurrentMemberInRoster = this.ShowInRoster;

            return View( viewModel );
        }

        [Authorize]
        public ActionResult Detail( int id )
        {
            Membership membership = Repository.GetMembershipById( id, "CarDetails.Color" );

            return View( membership );
        }

        [Authorize]
        public ActionResult Download()
        {
            List<Membership> memberships = Repository.GetActiveMemberships()
                .Where( m => m.ShowInRoster )
                .OrderBy( m => m.Member1LastName )
                .ThenBy( m => m.Member1FirstName )
                .ToList();

            StringBuilder csv = new StringBuilder( 100000 );

            csv.Append( "Member 1 First Name,Member 1 Last Name,Member 1 Email,Member 2 First Name,Member 2 Last Name,Member 2 Email,Home Phone,Address 1,Address 2,City,State,Zip" );
            csv.Append( Environment.NewLine );

            foreach( Membership membership in memberships )
            {
                csv.Append( "\"" );
                csv.Append( membership.Member1FirstName );
                csv.Append( "\",\"" );
                csv.Append( membership.Member1LastName );
                csv.Append( "\",\"" );
                csv.Append( membership.Member1Email );
                csv.Append( "\",\"" );
                csv.Append( membership.Member2FirstName );
                csv.Append( "\",\"" );
                csv.Append( membership.Member2LastName );
                csv.Append( "\",\"" );
                csv.Append( membership.Member2Email );
                csv.Append( "\"," );
                csv.Append( membership.HomePhone );
                csv.Append( ",\"" );
                csv.Append( membership.AddressLine1 );
                csv.Append( "\",\"" );
                csv.Append( membership.AddressLine2 );
                csv.Append( "\",\"" );
                csv.Append( membership.City );
                csv.Append( "\",\"" );
                csv.Append( membership.State );
                csv.Append( "\"," );
                csv.Append( membership.Zip );
                csv.Append( Environment.NewLine );
            }

            return File( ASCIIEncoding.Default.GetBytes( csv.ToString() ), "application/octet-stream", "MHMC_Roster.csv" );
        }
        #endregion Public Methods
    }
}
