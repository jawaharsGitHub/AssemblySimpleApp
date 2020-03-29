using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{

    public class DuplicateByPhone
    {
        public string Name { get; set; }

        public int NameCount { get; set; }

        public string PhNo { get; set; }
    }
    public partial class ucMemberVerify : UserControl
    {

        List<MemberVerify> memberVerify;
        public ucMemberVerify()
        {
            InitializeComponent();
            memberVerify = MemberVerify.GetAll();
            dgvMember.DataSource = memberVerify;
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            // group by phone number and look for dulicate names

            /*
            string thoguthiName = "rmd";
            var fpath = @"E:/" + thoguthiName;
            var text = File.ReadAllLines(fpath);
            var rrr = new List<string>();

            text.ToList().ForEach(f =>
            {
                var lines = f.Split(' ').ToList();

                lines.ForEach(fe =>
                {
                    if (fe.Length == 10 && fe.ToInt64OrNull() != null)
                        rrr.Add(fe);
                });

            });
            */

            var duplicatePNo = (from p in memberVerify.Select(s => s.ContactNo)
                                group p by p into newPh
                                select new
                                {
                                    newPh.Key,
                                    Count = newPh.Count()
                                }).ToList();

            var resultPh = duplicatePNo.Where(w => w.Count > 1).OrderByDescending(o => o.Count).Select(s => s.Key).ToList();

            var phoneResult = new List<DuplicateByPhone>();

            resultPh.ForEach(fe =>
            {

                var data = (from m in memberVerify
                            where m.ContactNo == fe
                            select m).ToList();

                var dupPhone = (from d in data
                                group d by d.Name into nameGroup
                                select new DuplicateByPhone
                                {
                                    Name = nameGroup.Key,
                                    NameCount = nameGroup.Count(),
                                    PhNo = nameGroup.ToList().First().ContactNo
                                }).Where(w => w.NameCount > 1).ToList();

                phoneResult.AddRange(dupPhone);

            });


            // Write it in a file and open in notepad

            StringBuilder resultP = new StringBuilder();
            int index = 0;
            phoneResult.ForEach(rfe =>
            {
                index += 1;
                resultP.AppendLine($"{index}.{rfe.PhNo}({rfe.NameCount}) --> {rfe.Name}");
            });


            File.WriteAllText($"E:/DupByPhone-result.txt", resultP.ToString());

            Process.Start($"E:/DupByPhone-result.txt");
        }

        private void btnNameAddressVerify_Click(object sender, EventArgs e)
        {
            // group by name and look for duplicate or nearest address.

            var duplicateName = (from p in memberVerify.Select(s => s.Name)
                                 group p by p into newNameList
                                 select new
                                 {
                                     newNameList.Key,
                                     Count = newNameList.Count()
                                 }).ToList();

            var resultPh = duplicateName.Where(w => w.Count > 1).OrderByDescending(o => o.Count).Select(s => s.Key).ToList();

            var phoneResult = new List<DuplicateByPhone>();

            resultPh.ForEach(fe =>
            {

                var data = (from m in memberVerify
                            where m.Name == fe
                            select m).ToList();

                var dupName = (from d in data
                               group d by d.Address into nameGroup
                               select new DuplicateByPhone
                               {
                                   Name = nameGroup.Key,
                                   NameCount = nameGroup.Count(),
                                   PhNo = nameGroup.ToList().First().ContactNo
                               }).Where(w => w.NameCount > 1).ToList();

                phoneResult.AddRange(dupName);

            });


            // Write it in a file and open in notepad
            StringBuilder resultP = new StringBuilder();
            int index = 0;
            phoneResult.ForEach(rfe =>
            {
                index += 1;
                resultP.AppendLine($"{index}.{rfe.PhNo}({rfe.NameCount}) --> {rfe.Name}");
            });


            File.WriteAllText($"E:/DupByName-result.txt", resultP.ToString());

            Process.Start($"E:/DupByName-result.txt");



        }
    }
}
