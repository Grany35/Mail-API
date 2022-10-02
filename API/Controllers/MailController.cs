using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendMail()
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse("admin@ogunergin.com"));
            mail.To.Add(MailboxAddress.Parse("ogun.ergin35@gmail.com"));
            mail.Subject = "Test";
            mail.Body = new TextPart(TextFormat.Html)
            {
                Text = "test body"
            };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.turkticaret.net", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("admin@ogunergin.com", "PASSWORD GOES HERE");
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);


            }
            return Ok();
        }
    }
}
