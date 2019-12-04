using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace JSStudyGameWebApp.Helpers
{
    public static class EmailWorker
    {
        public static bool SendEmail(ViewModels.PlayerVM player, string message)
        {
            try
            {
                var SmtpServer = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("semenplujara@gmail.com", "Qwerty1-"),
                    EnableSsl = true
                };

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("semenplujara@gmail.com");
                mail.To.Add(player.Email);
                mail.Subject = "Registration information!";
                mail.IsBodyHtml = true;
                string url = "https://itproger.com/img/courses/1476977754.jpg";
                string body = "<html> " +
                    "<head> " +
                    "<title></title> " +
                    "</head> " +
                    "<body> " +
                    "<h3>Hello dear friend!</h2> " +
                    $"<h3>{message}</h3> " +
                    $"<p>LOGIN: {player.Login}</p> " +
                    $"<p>PASSWORD: {player.Password}</p> " +
                    "<div style=\"box - shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2); transition: 0.3s; width: 40 %; \"> " +
                    $"<img src=\"{url}\" alt=\"JS Study Game\" style=\"width:100%\"> " +
                    "<div style=\"padding: 2px 16px; \"> " +
                    "<h3>Enjoy the game!</h3> " +
                    "<p>our site: ...</p> " +
                    "</div> " +
                    "</div> " +
                    "</body> " +
                    "</html>";
                mail.Body = body;
                SmtpServer.Send(mail);
            }
            catch (Exception) { return false; }
            return true;
        }
    }
}
