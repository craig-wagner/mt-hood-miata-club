#region using
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
using MtHoodMiata.Web.ViewModels;
#endregion using

namespace MtHoodMiata.Web.Controllers
{
    public class CarController : BaseController
    {
        [Authorize]
        public ActionResult Delete( int id, string returnUrl )
        {
            CarDetail carDetail = Repository.GetCarById( id );
            Repository.Remove( carDetail );
            Repository.SaveChanges();

            return Redirect( returnUrl );
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create( int id, string returnUrl )
        {
            Membership membership = Repository.GetMembershipById( id );

            AddEditCarViewModel viewModel = new AddEditCarViewModel()
            {
                Colors = Repository.GetColors(),
                ModelYears = GetModelYears(),
                ReturnUrl = returnUrl,
                MemberName = membership.Member1FullName,
                MembershipId = id
            };

            return View( viewModel );
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create( AddEditCarViewModel viewModel )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                viewModel.Car.MembershipId = viewModel.MembershipId;
                Repository.Add( viewModel.Car );
                Repository.SaveChanges();

                result = Redirect( viewModel.ReturnUrl );
            }
            else
            {
                viewModel.ModelYears = GetModelYears();
                viewModel.Colors = Repository.GetColors();

                result = View( viewModel );
            }

            return result;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit( int id, string returnUrl )
        {
            CarDetail car = Repository.GetCarById( id, "Membership" );

            AddEditCarViewModel viewModel = new AddEditCarViewModel( car )
            {
                Colors = Repository.GetColors(),
                ModelYears = GetModelYears(),
                ReturnUrl = returnUrl,
                MemberName = car.Membership.Member1FullName,
                MembershipId = car.Membership.MembershipId
            };

            return View( viewModel );
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit( AddEditCarViewModel viewModel )
        {
            ActionResult result = null;

            if( ModelState.IsValid )
            {
                CarDetail car = Repository.GetCarById( viewModel.Car.CarDetailId );

                car.ColorId = viewModel.Car.ColorId;
                car.License = viewModel.Car.License;
                car.Mods = viewModel.Car.Mods;
                car.Nickname = viewModel.Car.Nickname;
                car.SpecialEdition = viewModel.Car.SpecialEdition;
                car.Year = viewModel.Car.Year;

                Repository.SaveChanges();

                result = Redirect( viewModel.ReturnUrl );
            }
            else
            {
                viewModel.ModelYears = GetModelYears();
                viewModel.Colors = Repository.GetColors();

                result = View( viewModel );
            }

            return result;
        }
    }
}
