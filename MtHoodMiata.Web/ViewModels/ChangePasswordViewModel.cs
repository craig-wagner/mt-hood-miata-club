#region using
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required( ErrorMessage = "Old password is required." )]
        public string OldPassword { get; set; }

        [Required( ErrorMessage = "New password is required." )]
        public string NewPassword { get; set; }

        [Required( ErrorMessage = "New password confirmation is required." )]
        [Compare( "NewPassword", ErrorMessage = "New Password and Confirm New Password do not match." )]
        public string NewPasswordConfirm { get; set; }

        public bool IsChangeForced { get; set; }
    }
}