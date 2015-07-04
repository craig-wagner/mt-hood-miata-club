#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
using MtHoodMiata.Web.Properties;
using MtHoodMiata.Web.ViewModels;
using Security = System.Web.Security;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class MembersOnlyController : BaseController
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
        public MembersOnlyController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        /// <param name="emailHelper">
        /// An instance of a class that implements IEmailHelper. This will typically be a mock
        /// for unit testing.
        /// </param>
        internal MembersOnlyController( IMtHoodMiataRepository repository, IEmailHelper emailHelper )
            : base( repository )
        {
            _emailHelper = emailHelper;
        }
        #endregion Constructors

        #region Public Methods
        [HttpGet]
        public ActionResult Login()
        {
            LoginViewModel viewModel = new LoginViewModel();
            viewModel.IsLoginEnabled = Settings.Default.EnableMemberLogin;

            return View( viewModel );
        }

        [HttpPost]
        public ActionResult Login( LoginViewModel viewModel )
        {
            if( ModelState.IsValid )
            {
                string enteredUsername = viewModel.Username.ToLower();
                string enteredPassword = viewModel.Password.ToLower();
                string memberPassword = String.Empty;

                IEnumerable<Membership> memberships = null;

                using( MtHoodMiataRepository repository = new MtHoodMiataRepository() )
                {
                    memberships = repository.QueryMemberships( m =>
                        m.Member1Username == enteredUsername ||
                        m.Member2Username == enteredUsername );
                }

                if( memberships.Count() > 0 )
                {
                    Membership membership = memberships.First();

                    if( !membership.IsActive )
                    {
                        viewModel.LoginStatus = LoginStatus.LapsedMembership;
                    }
                    else
                    {
                        int memberNumber = 0;

                        if( enteredUsername == membership.Member1Username.ToLower() )
                        {
                            memberPassword = membership.Member1Password;
                            memberNumber = 1;
                        }
                        else
                        {
                            memberPassword = membership.Member2Password;
                            memberNumber = 2;
                        }

                        if( enteredPassword == memberPassword.ToLower() )
                        {
                            Session[Identifiers.MembershipId] = membership.MembershipId;
                            Session[Identifiers.MemberNumber] = memberNumber;

                            Security.FormsAuthentication.SetAuthCookie( enteredUsername, false );

                            if( String.IsNullOrWhiteSpace( Request.QueryString["ReturnUrl"] ) )
                            {
                                return Redirect( Security.FormsAuthentication.DefaultUrl );
                            }
                            else
                            {
                                return Redirect( Request.QueryString["ReturnUrl"] );
                            }
                        }
                        else
                        {
                            viewModel.LoginStatus = LoginStatus.InvalidCredentials;
                        }
                    }
                }
                else
                {
                    viewModel.LoginStatus = LoginStatus.InvalidCredentials;
                }
            }

            viewModel.IsLoginEnabled = Settings.Default.EnableMemberLogin;
            return View( viewModel );
        }

        [HttpGet]
        public ActionResult RecoverCredentials()
        {
            RecoverCredentialsViewModel viewModel = new RecoverCredentialsViewModel();
            viewModel.Result = RecoverCredentialsResult.None;

            return View( viewModel );
        }

        [HttpPost]
        public ActionResult RecoverCredentials( RecoverCredentialsViewModel viewModel )
        {
            if( ModelState.IsValid )
            {
                List<Membership> memberships = null;

                using( MtHoodMiataRepository repository = new MtHoodMiataRepository() )
                {
                    memberships = repository.QueryMemberships(
                        m => m.Member1Email == viewModel.Email ||
                            m.Member2Email == viewModel.Email ).ToList();
                }

                if( memberships.Count == 0 )
                {
                    viewModel.Result = RecoverCredentialsResult.EmailNotFound;
                }
                else if( memberships.Count > 1 )
                {
                    throw new MtHoodMiataException( "Multiple memberships found for email address: " + viewModel.Email );
                }
                else
                {
                    Membership membership = memberships.Single();

                    string username = String.Empty;
                    string password = String.Empty;

                    if( membership.Member1Email.ToLower() == viewModel.Email.ToLower() )
                    {
                        username = membership.Member1Username;
                        password = membership.Member1Password;
                    }
                    else
                    {
                        username = membership.Member2Username;
                        password = membership.Member2Password;
                    }

                    StringBuilder message = new StringBuilder( 5000 );

                    message.Append( "Your username and password for the Mt. Hood Miata Club web site are below:<br /><br />" );
                    message.AppendFormat( "&nbsp;&nbsp;&nbsp;&nbsp;Username:&nbsp;&nbsp;{0}<br />", username );
                    message.AppendFormat( "&nbsp;&nbsp;&nbsp;&nbsp;Password:&nbsp;&nbsp;{0}", password );
                    message.Append( "<p>You may want to consider using a password safe to store " );
                    message.Append( "all your usernames and passwords. A couple of good (and " );
                    message.Append( "<strong>free</strong>) ones are <a href=\"http://keepass." );
                    message.Append( "info\">KeePass</a> and <a href=\"http://lastpass.com\">" );
                    message.Append( "LastPass</a>.</p>" );

                    using( MailMessage msg = new MailMessage() )
                    {
                        msg.From = new MailAddress( "membership@mthoodmiata.org" );
                        msg.To.Add( viewModel.Email );
                        msg.Subject = "Your Mt. Hood Miata Username & Password";

                        msg.IsBodyHtml = true;
                        msg.Body = message.ToString();

                        EmailHelper.Send( msg );
                    }

                    viewModel.Result = RecoverCredentialsResult.Success;
                }
            }

            return View( viewModel );
        }

        public ActionResult Logout()
        {
            Security.FormsAuthentication.SignOut();

            return Redirect( "/Home/Index" );
        }

        [Authorize]
        public ActionResult Index()
        {
            return View( (object)FirstName );
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel viewModel = new ChangePasswordViewModel();

            if( Password == Constants.DefaultPassword )
            {
                viewModel.IsChangeForced = true;
            }

            return View( viewModel );
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword( ChangePasswordViewModel viewModel )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                if( this.Password.ToLower() == viewModel.OldPassword.ToLower() )
                {
                    Repository.UpdateMemberPassword(
                        this.MembershipId, this.MemberNumber, viewModel.NewPassword );

                    result = Redirect( "/MembersOnly/Index" );
                }
                else
                {
                    ModelState.AddModelError( "OldPassword", "Old password is incorrect" );
                    result = View( viewModel );
                }
            }
            else
            {
                result = View( viewModel );
            }

            return result;
        }

        [Authorize]
        public ActionResult Constitution()
        {
            return File( "/Content/MembersOnly/MtHoodMiataClubConstitution.pdf", "application/pdf" );
        }
        #endregion Public Methods
    }
}
