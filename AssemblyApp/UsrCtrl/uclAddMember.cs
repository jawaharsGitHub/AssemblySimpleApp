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

            string pwd = Microsoft.VisualBasic.Interaction.InputBox("Password?", "password");

            nonVerified.ForEach(fe =>
            {
                SendBalanceEmail(fe, pwd);
            });

            MessageBox.Show("Done");


        }

        static List<MemberVerify> mvl = new List<MemberVerify>();
        public static void SendBalanceEmail(MemberVerify mv, string pwd)
        {
            try
            {
                var sub = $"நாம் தமிழர் கட்சி - இராமநாதபுரம் தொகுதி";
                var mBody = FileContentReader.EmailBodyHtml;
                mBody = mBody.Replace("[name]", mv.Name);
                mBody = mBody.Replace("[phone]", mv.ContactNo);

                var smtp = General.GetMailMessage(sub, mBody);

                smtp.Item2.Send(smtp.Item1);
                //Thread
            }
            catch (Exception ex)
            {
                mvl.Add(mv);
            }
        }

       
    }
}
