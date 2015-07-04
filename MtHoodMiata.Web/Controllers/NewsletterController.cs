#region using
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
using MtHoodMiata.Web.ViewModels;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class NewsletterController : BaseController
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
        public NewsletterController() { }

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
        internal NewsletterController( IMtHoodMiataRepository repository, IEmailHelper emailHelper )
            : base( repository )
        {
            _emailHelper = emailHelper;
        }
        #endregion Constructors

        #region Public Methods
        [Authorize]
        public ActionResult Index()
        {
            string folder = Server.MapPath( "/newsletters" );
            string[] files = Directory.GetFiles( folder, "*.pdf" ).OrderBy( s => s ).ToArray();

            return View( files );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpGet]
        public ActionResult Upload()
        {
            int currYear = DateTime.Now.Year;

            NewsletterUploadViewModel viewModel = new NewsletterUploadViewModel();

            ViewBag.UploadResult = TempData.ContainsKey( "UploadResult" ) ? TempData["UploadResult"] : UploadResult.None;

            return View( viewModel );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpPost]
        public ActionResult Upload( NewsletterUploadViewModel viewModel, HttpPostedFileBase file )
        {
            return HandleUpload( viewModel, file, false );
        }

        [Authorize( MustBeBoardMember = true )]
        [HttpPost]
        public ActionResult UploadAndEmail( NewsletterUploadViewModel viewModel, HttpPostedFileBase file )
        {
            return HandleUpload( viewModel, file, true );
        }
        #endregion Public Methods

        #region Private Methods
        private ActionResult HandleUpload( NewsletterUploadViewModel viewModel, HttpPostedFileBase file, bool sendEmail )
        {
            if( ModelState.IsValid )
            {
                string fullname = String.Empty;

                TempData["UploadResult"] = SaveNewsletter( viewModel, file, ref fullname );

                if( sendEmail )
                {
                    SendEmail( fullname );

                    TempData["UploadResult"] = UploadResult.UploadAndEmailSuccess;
                }
            }

            return RedirectToAction( GetString.Of( () => Upload() ) );
        }

        private UploadResult SaveNewsletter( NewsletterUploadViewModel viewModel, HttpPostedFileBase file, ref string fullname )
        {
            string pathname = Server.MapPath( "/newsletters" );
            string filename = String.Empty;
            UploadResult result = UploadResult.UploadSuccess;

            fullname = String.Empty;

            if( file != null && file.ContentLength > 0 )
            {
                if( file.ContentType == "application/pdf" )
                {
                    filename = String.Format( "{0}-{1}.pdf", viewModel.Year, viewModel.Month );
                    fullname = Path.Combine( pathname, filename );

                    if( System.IO.File.Exists( fullname ) )
                    {
                        result = UploadResult.FileExists;
                    }
                    else
                    {
                        file.SaveAs( fullname );
                    }
                }
                else
                {
                    result = UploadResult.OnlyPdf;
                }
            }
            else
            {
                result = UploadResult.NoFileUploaded;
            }

            return result;
        }

        private void SendEmail( string newsletter )
        {
            using( MailMessage msg = new MailMessage() )
            {
                msg.From = new MailAddress( "editor@mthoodmiata.org" );

                msg.To.Add( "mthoodmiataclub@googlegroups.com" );
                msg.Subject = "Mt. Hood Miata Club Newsletter";

                msg.IsBodyHtml = true;
                msg.Body = "Attached is the most recent newsletter from the Mt. Hood Miata Club.";
                msg.Attachments.Add( new Attachment( newsletter ) );

                EmailHelper.Send( msg );
            }
        }
        #endregion Private Methods
    }
}
