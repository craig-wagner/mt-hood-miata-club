#region using
using MtHoodMiata.Web.Controllers;
using MtHoodMiata.Web.Models;
using System.Web;
using System.Web.Mvc;
#endregion using

namespace MtHoodMiata.Web
{
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        #region Fields
        private MtHoodMiataRepository _repository;
        #endregion Fields

        #region Properties
        public bool MustBeBoardMember { get; set; }

        private MtHoodMiataRepository Repository
        {
            get
            {
                _repository = _repository ?? new MtHoodMiataRepository();
                return _repository;
            }
        }
        #endregion Properties

        #region Constructors
        public AuthorizeAttribute() { }

        internal AuthorizeAttribute(MtHoodMiataRepository repository)
        {
            _repository = repository;
        }

        ~AuthorizeAttribute()
        {
            if (_repository != null)
            {
                _repository.Dispose();
            }
        }
        #endregion Constructors

        #region Public Methods
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result == null)
            {
                BaseController controller = (BaseController)filterContext.Controller;

                HttpSessionStateBase session = filterContext.HttpContext.Session;

                if (session[Identifiers.MemberNumber] != null)
                {
                    controller.MemberNumber = (int)session[Identifiers.MemberNumber];
                    controller.MembershipId = (int)session[Identifiers.MembershipId];
                }

                Membership membership = Repository.GetMembershipById(controller.MembershipId);

                if (membership != null)
                {
                    controller.ShowInRoster = membership.ShowInRoster;

                    switch (controller.MemberNumber)
                    {
                        case 1:
                            controller.FirstName = membership.Member1FirstName;
                            controller.LastName = membership.Member1LastName;
                            controller.Username = membership.Member1Username;
                            controller.Password = membership.Member1Password;
                            controller.IsBoardMember = membership.IsMember1BoardMember;
                            break;

                        case 2:
                            controller.FirstName = membership.Member2FirstName;
                            controller.LastName = membership.Member2LastName;
                            controller.Username = membership.Member2Username;
                            controller.Password = membership.Member2Password;
                            controller.IsBoardMember = membership.IsMember2BoardMember;
                            break;
                    }

                    if (MustBeBoardMember && !controller.IsBoardMember)
                    {
                        filterContext.Result = new RedirectResult("/MembersOnly/Index");
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("/MembersOnly/Login");
                }

                if (filterContext.Result == null)
                {
                    string controllerMethod = filterContext.RequestContext.RouteData.Values["action"].ToString();

                    if (controller.Password == Constants.DefaultPassword && controllerMethod != "ChangePassword")
                    {
                        filterContext.Result = new RedirectResult("/MembersOnly/ChangePassword");
                    }
                }
            }
        }
        #endregion Public Methods
    }
}