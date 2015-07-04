#region using
using System.Net.Mail;
#endregion using

namespace MtHoodMiata.Web
{
    public interface IEmailHelper
    {
        void Send( MailMessage msg );
    }
}
