#region using
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
using MtHoodMiata.Web.Properties;
using MtHoodMiata.Web.ViewModels;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class MembershipController : BaseController
    {
        #region Fields
        private IEmailHelper _emailHelper;
        #endregion Fields

        #region Properties
        private IEmailHelper EmailHelper
        {
            get
            {
                _emailHelper = _emailHelper ?? new EmailHelper();
                return _emailHelper;
            }
        }
        #endregion Properties

        #region Constructors
        public MembershipController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        internal MembershipController( IMtHoodMiataRepository repository, IEmailHelper emailHelper )
            : base( repository )
        {
            _emailHelper = emailHelper;
        }
        #endregion Constructors

        #region Public Methods
        [OutputCache( CacheProfile = "Static" )]
        public ActionResult Information()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Application()
        {
            MembershipApplicationViewModel model = new MembershipApplicationViewModel();
            model.ModelYears = GetModelYears();

            return View( model );
        }

        [HttpPost]
        public ActionResult Application( MembershipApplicationViewModel viewModel )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                switch( Request.Form["btnSubmit"] )
                {
                    case "paypal":
                        using( MailMessage msg = new MailMessage() )
                        {
                            msg.From = new MailAddress( Settings.Default.EmailFrom );
                            msg.To.Add( Settings.Default.AppEmailTo );
                            msg.CC.Add( Settings.Default.AppEmailCC );
                            msg.Subject = "Membership Application";

                            msg.IsBodyHtml = true;

                            msg.Body = GetApplicationBody( viewModel );

                            EmailHelper.Send( msg );
                        }

                        // Save this in case we need it on the cancellation page
                        Session["ApplicationName"] =
                            viewModel.Member1FirstName + " " +
                            viewModel.Member1LastName;

                        result = Redirect( "~/Content/Membership/Payment.html" );
                        break;

                    case "print":
                        result = View( "PrintApplication", viewModel );
                        break;
                }
            }
            else
            {
                viewModel.ModelYears = GetModelYears();
                result = View( viewModel );
            }

            return result;
        }

        public ActionResult Confirmation()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DiscardApplication()
        {
            ViewBag.ApplicationName = Session["ApplicationName"];

            return View();
        }

        [HttpPost]
        public ActionResult DiscardApplication( FormCollection form )
        {
            const string cancellationMessage =
                "{0} has chosen to cancel their membership application request. Please " +
                "disregard the application {0} submitted.";

            using( MailMessage msg = new MailMessage() )
            {
                msg.From = new MailAddress( "noreply@mthoodmiata.org" );
                msg.To.Add( Settings.Default.AppEmailTo );
                msg.CC.Add( Settings.Default.AppEmailCC );
                msg.Subject = "Membership Application Cancellation - " + form["name"];

                msg.IsBodyHtml = false;

                msg.Body = String.Format( cancellationMessage, form["name"] );

                EmailHelper.Send( msg );
            }

            return Redirect( "/Home/Index" );
        }

        [HttpGet]
        [Authorize( MustBeBoardMember = true )]
        public ActionResult Create()
        {
            Membership membership = new Membership()
            {
                MembershipDueDate = DateTime.Now.AddYears( 1 ),
                ShowInRoster = true
            };

            return View( membership );
        }

        [HttpPost]
        [Authorize( MustBeBoardMember = true )]
        public ActionResult Create( Membership model )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                Repository.Add( model );
                Repository.SaveChanges();

                result = RedirectToAction( GetString.Of( () => AdminEdit( 0 ) ), new { id = model.MembershipId } );
            }
            else
            {
                result = View( model );
            }

            return result;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit()
        {
            Membership membership = Repository.GetMembershipById( this.MembershipId, "CarDetails.Color" );

            return View( membership );
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit( Membership model )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                Membership membership = Repository.GetMembershipById( this.MembershipId );

                CopyMembershipValues( model, membership );

                Repository.SaveChanges();

                return Redirect( "/MembersOnly/Index" );
            }
            else
            {
                result = View( model );
            }

            return result;
        }

        [HttpGet]
        [Authorize( MustBeBoardMember = true )]
        public ActionResult AdminEdit( int id )
        {
            Membership membership = Repository.GetMembershipById( id, "CarDetails.Color" );

            return View( membership );
        }

        [HttpPost]
        [Authorize( MustBeBoardMember = true )]
        public ActionResult AdminEdit( int id, Membership model )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                Membership membership = Repository.GetMembershipById( id );

                CopyMembershipValues( model, membership );
                membership.IsMember1BoardMember = model.IsMember1BoardMember;
                membership.IsMember2BoardMember = model.IsMember2BoardMember;
                membership.Member1Password = model.Member1Password;
                membership.Member1Username = model.Member1Username;
                membership.Member2Password = model.Member2Password;
                membership.Member2Username = model.Member2Username;
                membership.MembershipDueDate = model.MembershipDueDate;

                Repository.SaveChanges();

                result = Redirect( "/Membership/List" );
            }
            else
            {
                result = View( model );
            }

            return result;
        }

        [Authorize( MustBeBoardMember = true )]
        public ActionResult Delete( int id )
        {
            Membership membership = Repository.GetMembershipById( id, "CarDetails" );
            Repository.Remove( membership );
            Repository.SaveChanges();

            return RedirectToAction( GetString.Of( () => List() ) );
        }

        [HttpGet]
        [Authorize( MustBeBoardMember = true )]
        public ActionResult List()
        {
            IEnumerable<Membership> memberships = Repository.GetMemberships();

            ViewBag.InSearchMode = false;

            return View( memberships );
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
                IEnumerable<Membership> memberships = Repository.QueryMemberships( m =>
                    m.Member1FirstName.Contains( searchString ) ||
                    m.Member1LastName.Contains( searchString ) ||
                    m.Member1Email.Contains( searchString ) ||
                    m.Member2FirstName.Contains( searchString ) ||
                    m.Member2LastName.Contains( searchString ) ||
                    m.Member2Email.Contains( searchString ) );

                ViewBag.InSearchMode = true;

                result = View( memberships );
            }
            return result;
        }
        #endregion Public Methods

        #region Private Methods
        private void CopyMembershipValues( Membership source, Membership target )
        {
            target.AddressLine1 = source.AddressLine1;
            target.AddressLine2 = source.AddressLine2;
            target.AnniversaryDate = source.AnniversaryDate;
            target.City = source.City;
            target.HomePhone = source.HomePhone;
            target.Member1CellPhone = source.Member1CellPhone;
            target.Member1Dob = source.Member1Dob;
            target.Member1Email = source.Member1Email;
            target.Member1FirstName = source.Member1FirstName;
            target.Member1LastName = source.Member1LastName;
            target.Member1WorkPhone = source.Member1WorkPhone;
            target.Member2CellPhone = source.Member2CellPhone;
            target.Member2Dob = source.Member2Dob;
            target.Member2Email = source.Member2Email;
            target.Member2FirstName = source.Member2FirstName;
            target.Member2LastName = source.Member2LastName;
            target.Member2WorkPhone = source.Member2WorkPhone;
            target.NewsletterPreference = source.NewsletterPreference;
            target.ShowInRoster = source.ShowInRoster;
            target.State = source.State;
            target.Zip = source.Zip;
        }

        private string GetApplicationBody( MembershipApplicationViewModel viewModel )
        {
            StringBuilder msgBody;

            using( StreamReader reader = new StreamReader(
                Path.Combine( Server.MapPath( "/Content/Membership" ), "ApplicationEmailTemplate.html" ) ) )
            {
                msgBody = new StringBuilder( reader.ReadToEnd() );
            }

            msgBody.Replace( "txtFirstName", viewModel.Member1FirstName );
            msgBody.Replace( "txtLastName", viewModel.Member1LastName );
            msgBody.Replace( "txtEmail", viewModel.Member1Email );
            msgBody.Replace( "txtBirthDate", viewModel.Member1Dob );
            msgBody.Replace( "txtCellPhone", viewModel.Member1CellPhone );
            msgBody.Replace( "txtWorkPhone", viewModel.Member1WorkPhone );

            msgBody.Replace( "txtSOFirstName", viewModel.Member2FirstName );
            msgBody.Replace( "txtSOLastName", viewModel.Member2LastName );
            msgBody.Replace( "txtSOEmail", viewModel.Member2Email );
            msgBody.Replace( "txtSOBirthDate", viewModel.Member2Dob );
            msgBody.Replace( "txtSOCellPhone", viewModel.Member2CellPhone );
            msgBody.Replace( "txtSOWorkPhone", viewModel.Member2WorkPhone );

            msgBody.Replace( "txtAnniversary", viewModel.WeddingAnniversaryDate );

            string address =
                viewModel.AddressLine1 +
                (String.IsNullOrWhiteSpace( viewModel.AddressLine2 ) ? String.Empty : "<br/>") +
                viewModel.AddressLine2;

            msgBody.Replace( "txtAddr", address );
            msgBody.Replace( "txtCity", viewModel.City );
            msgBody.Replace( "txtState", viewModel.State );
            msgBody.Replace( "txtZip", viewModel.Zip );

            msgBody.Replace( "txtHomePhone", viewModel.HomePhone );

            msgBody.Replace( "txtYear1", viewModel.Car1.Year.ToString() );
            msgBody.Replace( "txtColor1", viewModel.Car1.Color );
            msgBody.Replace( "txtPlate1", viewModel.Car1.LicensePlate );
            msgBody.Replace( "txtNickname1", viewModel.Car1.Nickname );
            msgBody.Replace( "txtMods1", viewModel.Car1.Modifications );

            msgBody.Replace( "txtYear2", viewModel.Car2.Year.ToString() );
            msgBody.Replace( "txtColor2", viewModel.Car2.Color );
            msgBody.Replace( "txtPlate2", viewModel.Car2.LicensePlate );
            msgBody.Replace( "txtNickname2", viewModel.Car2.Nickname );
            msgBody.Replace( "txtMods2", viewModel.Car2.Modifications );

            msgBody.Replace( "txtYear3", viewModel.Car3.Year.ToString() );
            msgBody.Replace( "txtColor3", viewModel.Car3.Color );
            msgBody.Replace( "txtPlate3", viewModel.Car3.LicensePlate );
            msgBody.Replace( "txtNickname3", viewModel.Car3.Nickname );
            msgBody.Replace( "txtMods3", viewModel.Car3.Modifications );

            msgBody.Replace( "txtComments", viewModel.Comments );

            return msgBody.ToString();
        }
        #endregion Private Methods
    }
}
