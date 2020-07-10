using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using DataAccess.PrimaryTypes;
using System.Net.Mail;
using System.Net;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class uclAddMember : UserControl
    {
        public uclAddMember()
        {
            InitializeComponent();

            cmbDivision.DataSource = Zonal.GetAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var nonVerified = MemberVerify.GetAllNonVerified();

            nonVerified = nonVerified.Where(w => string.IsNullOrEmpty(w.Email) == false).ToList();
            var noemail = nonVerified.Where(w => string.IsNullOrEmpty(w.Email) == true).ToList();


            nonVerified.ForEach(fe => {
                SendBalanceEmail(fe);
            });

        }

        static List<MemberVerify> mvl = new List<MemberVerify>();
        public static void SendBalanceEmail(MemberVerify mv)
        {
            try
            {
                var sub = $"நாம் தமிழர் கட்சி - இராமநாதபுரம் தொகுதி";
                var mBody = FileContentReader.EmailBodyHtml;
                mBody = mBody.Replace("[name]", mv.Name);
                mBody = mBody.Replace("[phone]", mv.ContactNo);
                var smtp = GetMailMessage(sub, mBody, mv.Email);

                smtp.Item2.Send(smtp.Item1);
            }
            catch (Exception ex)
            {
                mvl.Add(mv);
            }
        }

        private static (MailMessage, SmtpClient) GetMailMessage(string subject, string mailBody, string email, string pwd = null)
        {

            //var myEmail = "ramnadntk211@gmail.com";
            var myEmail = "ntkramnad211@gmail.com";
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(myEmail);
            message.To.Add(new MailAddress(email));

            message.CC.Add(new MailAddress("jawahar.subramanian83@gmail.com"));
            message.Subject = subject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = mailBody;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(myEmail, "Ramnadntk@211");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            return (message, smtp);


        }
    }
}
