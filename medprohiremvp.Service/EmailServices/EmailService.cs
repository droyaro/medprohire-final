
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace medprohiremvp.Service.EmailServices
{
    public class EmailService :IEmailService
    {

        private string host ="mail.dixonwalther.com";
        private string email = "info@dixonwalther.com";
        private string password = "K33P0ff!";
        private string port = "25";
        public async Task<string> SendEmailAsync(string to, string subject, string message, bool ishtmlmessage)
        {
            // if there will be certificate error in server
            //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            using (MailMessage mail = new MailMessage())
            {

                // for multiple send 
                //if(to.Contains(";"))
                //{
                //    string[] tos = to.Split(',');
                //    for ( int i=0; i<tos.Length;  i++)
                //    {
                //        if(tos[i].Trim()!="")
                //        mail.To.Add(tos[i].Trim());
                //    }
                //}
                
                mail.To.Add(to);
                mail.From= new MailAddress(email, "Medprohire");
                mail.Subject = subject;
               
         

                if (ishtmlmessage)
                {
                    mail.IsBodyHtml = true;
                  
                }
                else
                {
                    mail.IsBodyHtml = false;
               
                }
                mail.Body = message.Replace("{RecruiterEmail}", email);

                using (SmtpClient smtp = new System.Net.Mail.SmtpClient())
                {
                    smtp.Host = host;
                    smtp.Port = Convert.ToInt32(port);
                    smtp.EnableSsl = true;
                   // smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(email, password);
                    try
                    {
                       await smtp.SendMailAsync(mail);
                        return "ok";
                       
                    }
                    catch(Exception ex)
                    {
                        string es = ex.Message;
                        return es;
                    }
                }
            }

        }

        public async Task SendEmailAsync(string to, string subject, string message, bool ishtmlmessage, List<string> files)
        {
                  // if there will be certificate error in server
                //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                using (MailMessage mail = new MailMessage())
                {

                    // for multiple send 
                    //if(to.Contains(";"))
                    //{
                    //    string[] tos = to.Split(',');
                    //    for ( int i=0; i<tos.Length;  i++)
                    //    {
                    //        if(tos[i].Trim()!="")
                    //        mail.To.Add(tos[i].Trim());
                    //    }
                    //}
                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.Body = message;
                    if (ishtmlmessage)
                        mail.IsBodyHtml = true;
                    else
                        mail.IsBodyHtml = false;

                
                    if (files.Count > 0)
                    {
                        foreach (string file in files)
                        {
                        Attachment attachment = new Attachment(file);
                        mail.Attachments.Add(attachment);
                        }
                    }

                    using (SmtpClient smtp = new System.Net.Mail.SmtpClient())
                    {
                        smtp.Host = host;
                        smtp.Port = Convert.ToInt32(port);
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(email, password);
                        await smtp.SendMailAsync(mail);
                        mail.Attachments.Clear();
                        mail.Attachments.Dispose();
                    }
                }     

        }
    }
}
