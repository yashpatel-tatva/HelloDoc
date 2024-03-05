using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface ISendEmailRepository
    {
        Task Sendemail(string email, string subject, string message);
        Task SendEmailwithAttachments(string email, string subject, string message, List<string> attachmentPaths);
    }
}
