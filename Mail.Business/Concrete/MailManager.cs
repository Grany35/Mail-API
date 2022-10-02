using Mail.Business.Abstract;
using Mail.Business.Entities;
using Mail.Business.Entities.Dtos;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail.Business.Concrete
{
    public class MailManager : IMailService
    {
        private readonly IMongoCollection<MailParameter> _parameterCollection;
        private readonly IMongoCollection<MailTemplate> _templateCollection;
        private IConfiguration _config;

        public MailManager(IConfiguration config)
        {
            _config = config;

            var client = new MongoClient(_config.GetSection("DbSettings:ConnectionUri").Value);
            var database = client.GetDatabase(_config.GetSection("DbSettings:DbName").Value);

            _parameterCollection = database.GetCollection<MailParameter>("Parameters");
            _templateCollection = database.GetCollection<MailTemplate>("Templates");

        }

        public async Task AddMailParameterAsync(ParameterAddDto parameterAddDto)
        {
            var mailParameter = new MailParameter
            {
                Host = parameterAddDto.Host,
                Password = parameterAddDto.Password,
                Port = parameterAddDto.Port,
                UserName = parameterAddDto.UserName
            };

            await _parameterCollection.InsertOneAsync(mailParameter);
        }

        public async Task AddMailTemplateAsync(TemplateAddDto templateAddDto)
        {
            var mailTemplate = new MailTemplate
            {
                Body = templateAddDto.Body,
                Sender = templateAddDto.Sender,
                Subject = templateAddDto.Subject
            };

            await _templateCollection.InsertOneAsync(mailTemplate);
        }

        public async Task SendMail(string receiverMail)
        {
            var mailTemplate = await _templateCollection.Find(x => true).FirstOrDefaultAsync();

            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(mailTemplate.Sender));
            mail.To.Add(MailboxAddress.Parse(receiverMail));
            mail.Subject = mailTemplate.Subject;
            mail.Body = new TextPart(TextFormat.Html)
            {
                Text = mailTemplate.Body,
            };
            
            var mailParameter=await _parameterCollection.Find(x=> true).FirstOrDefaultAsync();

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(mailParameter.Host, mailParameter.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(mailParameter.UserName, mailParameter.Password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
