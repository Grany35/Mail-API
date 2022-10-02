using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail.Business.Entities.Dtos
{
    public class TemplateAddDto
    {
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
