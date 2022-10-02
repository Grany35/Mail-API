using Mail.Business.Abstract;
using Mail.Business.Entities.Dtos;
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
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(string receiverMail)
        {
            await _mailService.SendMail(receiverMail);
            return NoContent();
        }

        [HttpPost("addmailparameter")]
        public async Task<IActionResult> AddMailParameter(ParameterAddDto parameterAddDto)
        {
            await _mailService.AddMailParameterAsync(parameterAddDto);

            return NoContent();
        }

        [HttpPost("addmailtemplate")]
        public async Task<IActionResult> AddMailTemplate(TemplateAddDto templateAddDto)
        {
            await _mailService.AddMailTemplateAsync(templateAddDto);
            return NoContent();
        }
    }
}
