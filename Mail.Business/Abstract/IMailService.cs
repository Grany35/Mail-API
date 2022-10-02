using Mail.Business.Entities;
using Mail.Business.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail.Business.Abstract
{
    public interface IMailService
    {
        Task AddMailParameterAsync(ParameterAddDto parameterAddDto);
        Task AddMailTemplateAsync(TemplateAddDto templateAddDto);
        Task SendMail(string receiverMail);
    }
}
