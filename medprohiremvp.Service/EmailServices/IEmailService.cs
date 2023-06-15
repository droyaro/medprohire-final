using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace medprohiremvp.Service.EmailServices
{
    public interface IEmailService
    {
        Task<string> SendEmailAsync(string to, string subject, string message, bool ishtmlmessage);
        Task SendEmailAsync(string to, string subject, string message, bool ishtmlmessage, List<string> files);
    }
}
