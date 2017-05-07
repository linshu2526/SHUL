using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;

namespace SHUL
{
    public class MailHelper
    {

        public MailHelper()
        {
        }
        public static string SendMail(string mailadress, string subject, string mailbody, string usermail, string userpwd, string mailhost, int mailport = 0, string displayname = "", bool IsBodyHtml = false, bool EnableSsl = false, string mailadress2 = "")
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(mailadress);
            if (mailadress2 != "")
            {
                msg.CC.Add(mailadress2);
            }

            msg.From = new MailAddress(usermail, displayname == "" ? "linshu" : displayname, System.Text.Encoding.UTF8);
            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.Subject = subject;//邮件标题   
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码   
            msg.Body = mailbody;//邮件内容   
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
            msg.IsBodyHtml = IsBodyHtml;//是否是HTML邮件   
            //msg.Priority = MailPriority.High;//邮件优先级   
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(usermail, userpwd);
            //client.UseDefaultCredentials = false;
            //上述写你的GMail邮箱和密码   
            if (mailport != 0)
            {
                client.Port = mailport;//Gmail使用的端口   
            }

            client.Host = mailhost;
            if (EnableSsl)
            {
                client.EnableSsl = true;//经过ssl加密   
            }

            object userState = msg;
            try
            {
                //client.SendAsync(msg, userState);
                //简单一点儿可以
                client.Send(msg);
                return "ok";
            }
            catch (Exception ex)//System.Net.Mail.Smtp
            {
                if (ex.InnerException != null)
                {
                    return ex.Message + ":" + ex.InnerException.Message;
                }
                else
                {
                    return ex.Message;
                }

            }
        }


        public static string SendMailByQQ(string to, string subject,
            string mailbody, string usermail, string userpwd)
        {
            return SendMail(to, subject, mailbody, usermail, userpwd,
                mailhost: "smtp.qq.com",
                mailport: 587,
                IsBodyHtml: true,
                EnableSsl: true);
        }
    }


}
