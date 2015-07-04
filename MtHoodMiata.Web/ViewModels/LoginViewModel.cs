#region using
using System.ComponentModel.DataAnnotations;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public enum LoginStatus
    {
        Success,
        LapsedMembership,
        InvalidCredentials
    }

    public class LoginViewModel
    {
        [Required]
        [StringLength( 50, MinimumLength = 2 )]
        public string Username { get; set; }

        [Required]
        [StringLength( 50, MinimumLength = 2 )]
        public string Password { get; set; }

        public bool IsLoginEnabled { get; set; }

        public LoginStatus LoginStatus { get; set; }
    }
}