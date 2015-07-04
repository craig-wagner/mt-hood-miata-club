#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class BoardController : BaseController
    {
        #region Fields
        private const string _httpContentType = "application/octet-stream";
        #endregion Fields

        #region Constructors
        public BoardController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        internal BoardController( IMtHoodMiataRepository repository )
            : base( repository ) { }
        #endregion Constructors

        #region Public Methods
        [Authorize( MustBeBoardMember = true )]
        [OutputCache( CacheProfile = "Static" )]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize( MustBeBoardMember = true )]
        public FileResult Roster()
        {
            List<Membership> memberships = Repository.GetActiveMemberships()
                .OrderBy( m => m.Member1LastName )
                .ThenBy( m => m.Member1FirstName )
                .ToList();

            StringBuilder csv = new StringBuilder( 100000 );

            csv.Append( "Member 1 First Name,Member 1 Last Name, Member 1 Email, Member 2 First Name, Member 2 Last Name, Member 2 Email, Home Phone, Address 1, Address 2, City, State, Zip, Due Date" );
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
                csv.Append( "," );
                csv.Append( membership.MembershipDueDate.ToShortDateString() );
                csv.Append( Environment.NewLine );
            }

            return File( ASCIIEncoding.Default.GetBytes( csv.ToString() ), _httpContentType, "active_members.csv" );
        }

        [Authorize( MustBeBoardMember = true )]
        public FileResult NewsletterEmailList()
        {
            List<Membership> memberships = Repository.GetActiveMemberships()
                    .Where( m => m.NewsletterPreference == "E" || m.NewsletterPreference == "B" )
                    .ToList();

            string csv = CreateEmailList( memberships );

            return File( ASCIIEncoding.Default.GetBytes( csv.ToString() ), _httpContentType, "newsletter_email_list.txt" );
        }

        [Authorize( MustBeBoardMember = true )]
        public FileResult NewsletterMailingList()
        {
            List<Membership> memberships = Repository.GetActiveMemberships()
                    .Where( m => m.NewsletterPreference == "U" || m.NewsletterPreference == "B" )
                    .OrderBy( m => m.Member1LastName )
                    .ThenBy( m => m.Member1FirstName )
                    .ToList();

            StringBuilder csv = new StringBuilder( 10000 );

            foreach( Membership membership in memberships )
            {
                csv.Append( "\"" );
                csv.Append( membership.Member1FirstName );
                csv.Append( "\",\"" );
                csv.Append( membership.Member1LastName );
                csv.Append( "\",\"" );
                csv.Append( membership.Member2FirstName );
                csv.Append( "\",\"" );
                csv.Append( membership.Member2LastName );
                csv.Append( "\",\"" );
                csv.Append( membership.AddressLine1 );
                csv.Append( "\",\"" );
                csv.Append( membership.AddressLine2 );
                csv.Append( "\",\"" );
                csv.Append( membership.City );
                csv.Append( "\",\"" );
                csv.Append( membership.State );
                csv.Append( "\",\"" );
                csv.Append( membership.Zip );
                csv.Append( "\"" );
                csv.Append( Environment.NewLine );
            }

            return File( ASCIIEncoding.Default.GetBytes( csv.ToString() ), _httpContentType, "newsletter_mailing_list.csv" );
        }

        [Authorize( MustBeBoardMember = true )]
        public FileResult EmailList()
        {
            List<Membership> memberships = Repository.GetActiveMemberships().ToList();

            string csv = CreateEmailList( memberships );

            return File( ASCIIEncoding.Default.GetBytes( csv.ToString() ), _httpContentType, "email_list.txt" );
        }
        #endregion Public Methods

        #region Private Methods
        private string CreateEmailList( IEnumerable<Membership> memberships )
        {
            StringBuilder csv = new StringBuilder( 10000 );

            foreach( Membership membership in memberships )
            {
                if( !String.IsNullOrEmpty( membership.Member1Email ) )
                {
                    csv.Append( membership.Member1Email );
                    csv.Append( Environment.NewLine );
                }

                if( !String.IsNullOrEmpty( membership.Member2Email ) &&
                    membership.Member1Email != membership.Member2Email )
                {
                    csv.Append( membership.Member2Email );
                    csv.Append( Environment.NewLine );
                }
            }

            return csv.ToString();
        }
        #endregion Private Methods
    }
}
