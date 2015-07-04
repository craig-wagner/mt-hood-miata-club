#region using
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        #region Fields
        private IMtHoodMiataRepository _repository;
        #endregion Fields

        #region Properties
        protected IMtHoodMiataRepository Repository
        {
            get
            {
                if( _repository == null )
                {
                    _repository = new MtHoodMiataRepository();
                }

                return _repository;
            }
        }

        public int MembershipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsBoardMember { get; set; }
        public int MemberNumber { get; set; }
        public bool ShowInRoster { get; set; }
        #endregion Properties

        #region Constructors
        public BaseController() { }

        /// <summary>
        /// Create and initialize an instance of the controller with injected dependency
        /// objects. This constructor will typically be restricted to use by unit tests.
        /// </summary>
        /// <param name="repository">
        /// An instance of a class that implements IMtHoodMiataRepository. This will
        /// typically be a mock for unit testing.
        /// </param>
        protected BaseController( IMtHoodMiataRepository repository )
        {
            _repository = repository;
        }
        #endregion Constructors

        #region Protected Methods
        protected IEnumerable<int> GetModelYears()
        {
            List<int> modelYears = new List<int>();

            for( int year = 1989; year <= DateTime.Now.Year + 1; year++ )
            {
                if( year != 1998 )
                {
                    modelYears.Add( year );
                }
            }

            return modelYears;
        }
        #endregion Protected Methods

        #region Disposable
        protected override void Dispose( bool disposing )
        {
            try
            {
                if( disposing )
                {
                    if( _repository != null )
                    {
                        _repository.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose( disposing );
            }
        }
        #endregion Disposable
    }
}
