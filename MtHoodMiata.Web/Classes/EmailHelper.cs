#region using
using System.Net.Mail;
#endregion using

namespace MtHoodMiata.Web
{
    public class EmailHelper : IEmailHelper
    {
        public void Send( MailMessage msg )
        {
            using( SmtpClient smtp = new SmtpClient() )
            {
                smtp.Send( msg );
            }
        }
    }
}