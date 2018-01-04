using System;
using Trevo.Core.Model;

namespace Trevo.API.Helper
{
    /// <summary>
    /// Send Email
    /// </summary>
    public class SendMail
    {
        public static ReturnMsg SendEmail(string FromMail, string ToMail, string Subject, string MailBody)
        {
            ReturnMsg objReturnMsg = new ReturnMsg();


            string msg = null;
            try
            {

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Port = 587;
                //client.Host = "box669.bluehost.com";
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                //client.Timeout = 10000;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential("postmaster@codelaamaa.com", "CodeLaamaa99@");
                client.Credentials = new System.Net.NetworkCredential("info@natit.us", "Natit2016");

                System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress(FromMail);
                System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(ToMail);
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);

                message.Subject = Subject;
                message.Body = MailBody;
                message.IsBodyHtml = true;
                message.Priority = System.Net.Mail.MailPriority.High;
                client.Send(message);
                objReturnMsg.IsSuccess = true;
                objReturnMsg.Message = "success";


            }
            catch (Exception ex)
            {
                objReturnMsg.IsSuccess = false;
                objReturnMsg.Message = "Mail not Send";

                msg = ex.Message;
            }
            return objReturnMsg;

        }
    }
}