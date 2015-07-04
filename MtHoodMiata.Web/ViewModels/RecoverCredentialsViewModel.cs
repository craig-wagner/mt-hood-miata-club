#region using
using System.ComponentModel.DataAnnotations;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public enum RecoverCredentialsResult
    {
        None,
        Success,
        EmailNotFound
    }

    public class RecoverCredentialsViewModel
    {
        [Required( ErrorMessage = "Email is required." )]
        [StringLength( 50, MinimumLength = 5 )]
        public string Email { get; set; }

        public RecoverCredentialsResult Result { get; set; }
    }
}