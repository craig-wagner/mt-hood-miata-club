#region using
using System;
using System.IO;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    [OutputCache( CacheProfile = "Static" )]
    public class TechController : BaseController
    {
        #region Fields
        private string _contentFolder;
        #endregion Fields

        #region Properties
        private string ContentFolder
        {
            get
            {
                if( String.IsNullOrWhiteSpace( _contentFolder ) )
                {
                    _contentFolder = Server.MapPath( "/Content/Tech" );
                }

                return _contentFolder;
            }
        }
        #endregion Properties

        #region Constructors
        public TechController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        internal TechController( IMtHoodMiataRepository repository )
            : base( repository ) { }
        #endregion Constructors

        #region Public Methods
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BootFolding()
        {
            return View();
        }

        public ActionResult FuelFilter()
        {
            return View();
        }

        public ActionResult PcvValves()
        {
            return View();
        }

        public ActionResult TowHitch()
        {
            return View();
        }

        public ActionResult TransFluidChange()
        {
            return View();
        }

        public ActionResult WindowZippers()
        {
            return View();
        }

        public ActionResult CobraCBInstall()
        {
            return View();
        }

        public ActionResult NCPowerAntennaInstall()
        {
            string filePath = Path.Combine( ContentFolder, "NC_Aftermarket_Power_Antenna_Installation.pdf" );
            return new FilePathResult( filePath, "application/pdf" );
        }

        public ActionResult ShortenParkingBrakeLever()
        {
            string filePath = Path.Combine( ContentFolder, "Shorten_Your_Parking_Brake_Lever.pdf" );
            return new FilePathResult( filePath, "application/pdf" );
        }
        #endregion Public Methods
    }
}
