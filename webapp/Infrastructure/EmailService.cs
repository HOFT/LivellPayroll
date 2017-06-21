using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using System;
using SendGrid;
using System.Net.Http;
using System.Net;
using System.Configuration;
using LivellPayRoll.Configurations;
using System.Net.Mail;
using System.Text;

namespace LivellPayRoll.Infrastructure
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }
        private async Task configSendGridasync(IdentityMessage message)
        {
            MailConfig mailConfig = (MailConfig)ConfigurationManager.GetSection("application/mail");
            if (mailConfig.RequireValid)
            {
                // 设置邮件内容
                var mail = new MailMessage(
                    new MailAddress(mailConfig.EmailAddress, mailConfig.EmailUserName),
                    new MailAddress(message.Destination)
                    );
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                // 设置SMTP服务器
                var smtp = new SmtpClient(mailConfig.SmtpServer, mailConfig.SmtpPort);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(mailConfig.EmailAddress, mailConfig.EmailPwd);

                await smtp.SendMailAsync(mail);
            }
            await Task.FromResult(0);


            //var apiKey = Environment.GetEnvironmentVariable("ses-smtp-user.20170614-151511");
            //var client = new SendGridClient(apiKey);

            //var myMessagemsg = new SendGridMessage()
            //{
            //    From = new EmailAddress("hoya@payroll.com", "PayRoll."),
            //    Subject = message.Subject,
            //    PlainTextContent = message.Body,
            //    HtmlContent = message.Body
            //};
            //myMessagemsg.AddTo(message.Destination);

            //await client.SendEmailAsync(myMessagemsg);

        }
    }
}