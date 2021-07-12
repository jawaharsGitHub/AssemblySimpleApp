using Common.ExtensionMethod;
using System;
using System.Net;
using System.Net.Mail;

namespace Common
{
    public class AppCommunication
    {
        private static (MailMessage, SmtpClient) GetMailMessage(string subject, string mailBody, bool haveCC = false)
        {

            var myEmail = "jawahar.subramanian83@gmail.com";
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(myEmail);
            message.To.Add(new MailAddress(myEmail));

            if (haveCC) message.CC.Add(new MailAddress("jeyapriyagopal@gmail.com"));
            message.Subject = subject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = mailBody;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(myEmail, "nainamarbus");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            return (message, smtp);


        }

        

        public static void SendAdangalUpdate(string subject, string mailBody)
        {
            try
            {
                //var sub = subject;
                var smtp = GetMailMessage(subject, mailBody);
                //smtp.Item1.Attachments.Add(new Attachment(attachmentFilePath)); // attachments
                smtp.Item2.Send(smtp.Item1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
