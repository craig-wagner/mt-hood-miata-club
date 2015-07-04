#region using
using System.Collections.Generic;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public class AddEditCarViewModel
    {
        #region Fields
        private CarDetail _car;
        #endregion Fields

        #region Properties
        public CarDetail Car
        {
            get
            {
                _car = _car ?? new CarDetail();
                return _car;
            }
        }

        public IEnumerable<int> ModelYears { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public string ReturnUrl { get; set; }
        public string MemberName { get; set; }
        public int MembershipId { get; set; }
        #endregion Properties

        #region Constructors
        public AddEditCarViewModel() { }

        public AddEditCarViewModel( CarDetail car )
        {
            _car = car;
        }
        #endregion Constructors
    }
}