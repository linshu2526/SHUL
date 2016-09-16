using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace SHUL
{
    public class MailTools
    {
        private static readonly string __FromMailAddress = AppSetting.GetConfig("ServiceEmailAddress");
        private static readonly string __FromMailAddressUser = AppSetting.GetConfig("ServiceEmailUser");
        private static readonly string __FromMailAddressPwd = AppSetting.GetConfig("ServiceEmailPassword");
        private static readonly string __FromMailAddressSmtpHost = AppSetting.GetConfig("ServiceEmailSmtpHost");
        private static readonly int __FromMailAddressSmtpPort = Convert.ToInt32(AppSetting.GetConfig("ServiceEmailSmtpPort"));
        private static readonly bool __FromMailAddressEnableSsl = Convert.ToBoolean(AppSetting.GetConfig("ServiceEmailEnableSsl"));


        public static bool Send(string toEmail, string subject, string body)
        {
            MailMessage mail = CreateMailMessage(toEmail, subject, body);
            return Send(mail);
        }
        private static bool Send(MailMessage mail)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.EnableSsl = __FromMailAddressEnableSsl;
            smtpClient.Host = __FromMailAddressSmtpHost;
            smtpClient.Port = __FromMailAddressSmtpPort;
            smtpClient.Credentials = new NetworkCredential(__FromMailAddressUser, __FromMailAddressPwd);
            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception)
            {
                //System.Diagnostics.Debug.Assert(false, ex.Message);
                return false;
            }
        }
        private static MailMessage CreateMailMessage(string toEmail, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(__FromMailAddress);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            return mail;
        }
    }
}
