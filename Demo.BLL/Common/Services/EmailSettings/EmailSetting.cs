using Demo.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Common.Services.EmailSettings
{
    public class EmailSetting : IEmailSetting
    {
        public void SendEmail(Email email)
        {
            // mailServer => gmail.com

            var client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;

            // Sender, Receiver
            // User (Who tries to reset password)
            client.Credentials = new NetworkCredential("badrpotato@gmail.com", "wcmuermdjftgkvlh"); //generate application password 

            // Receiver
            client.Send("badrpotato@gmail.com", email.To, email.Subject, email.Body);

        }
    }
}
