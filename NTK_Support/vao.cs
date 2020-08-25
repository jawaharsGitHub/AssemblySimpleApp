using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTK_Support
{
    public partial class vao : Form
    {
        public vao()
        {
            InitializeComponent();
            ProcessChittaFile();
        }


        int index = 0;
        private void ProcessChittaFile()
        {
            ///var myFile = @"C:\Users\WelCome\Documents\Vaidehi-Vao\reg data\Chitta_Report-1.txt";

            var myFile = @"F:\chitta-copiedData.txt";


            var content = File.ReadAllText(myFile);

            //var pageCount = (from p in content.Split(' ').ToList()
            //                 where p.Contains("Taluk")
            //                 select p).ToList().Count;
            var pattaas = content.Replace("பட்டா எண்", "$");

            var data = pattaas.Split('$').ToList();

            data.RemoveAt(0); // empty data

            List<ChittaData> cds = new List<ChittaData>();

            string del1 = "";
            string path = @"F:\test-21.csv";

            data.ForEach(fe =>
            {

                var item = fe.Replace("வ.எண்", del1)
                             .Replace("உ எண்", del1)
                             .Replace("உறவினர் பெயர்", del1)
                             .Replace("உறவு", del1)
                             .Replace("உரிமையாளர் பெயர்", del1)
                             .Replace("புல எண்", del1)
                             .Replace("உட்பிரிவு எண்", del1)
                             .Replace("நன்செய்", del1)

                             .Replace("புன்செய்", del1)
                             .Replace("மற்றவை", del1)
                             .Replace("குறிப்பு", del1)
                             .Replace("பரப்பு", del1)
                             .Replace("தீர்வை", del1)
                             .Replace("மொத்தம்", "TOTAL")
                             .Replace('\t', '$');

                var rrr = item.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();


                bool isVagai = false;                

                var dd2 = (from tt in rrr
                           where tt.Replace("$", "").Trim() != string.Empty
                           select tt).ToList();

                var dd = new List<string>(dd2);


                for (int i = 2; i < dd.Count; i++)
                {

                    if (dd[i].Contains("-") == false && dd[i].Contains(":") == false)
                    {
                        isVagai = true;
                        dd2[dd2.IndexOf(dd2[i])] = "*" + dd2[i];
                    }


                }


                if (dd2.Count > 3)

                {
                    var d3 = dd2.Take(dd2.Count - 1).ToList().Where(w => w.StartsWith("*") == false).ToList();



                    //File.AppendAllLines(path, d3.ToArray());
                    //File.AppendAllText(path, "===============================================");

                    string oName = "";
                    int pattaNo = 0;

                    pattaNo = Convert.ToInt32(d3[0].Replace(":", "").Trim());

                    if (pattaNo == 1218)
                    {

                    }

                    var names = d3[1].Split('$');

                    

                    if (isVagai)
                    {
                        oName = $"{pattaNo} - {names[4]} வகை ";
                    }
                    else
                    {
                        oName = $"{pattaNo} - {names[2][0]}. {names[4]}";
                    }


                    for (int i = 2; i <= d3.Count - 1; i++)
                    {
                        ChittaData d = new ChittaData();
                        d.OwnerName = oName;
                        d.PattaNo = pattaNo;

                        var nums = d3[i].Split('$');

                        d.SurveyNo = Convert.ToInt32(nums[1].Split('-')[0].Trim());
                        d.SubDivNo = nums[1].Split('-')[1].Trim(); ;

                        if (nums[3].Trim() != "0.00")    // N - நன்செய் 
                        {
                            d.LandType = "1N";
                            d.Parappu = nums[2].Replace("-", ".").Trim().Replace(" ", "");
                            d.Theervai = Convert.ToDecimal(nums[3]);
                        }
                        else if (nums[5].Trim() != "0.00") // P - புன்செய் 
                        {
                            d.LandType = "2P";
                            d.Parappu = nums[4].Replace("-", ".").Trim().Replace(" ", "");
                            d.Theervai = Convert.ToDecimal(nums[5]);
                        }
                        else if (nums[7].Trim() != "0.00") // M - மானாவாரி  
                        {
                            d.LandType = "3M";
                            d.Parappu = nums[6].Replace("-", ".").Trim().Replace(" ", "");
                            d.Theervai = Convert.ToDecimal(nums[7]);
                        }

                        index = (index + 1);
                        d.index = index;

                        cds.Add(d);
                        

                    }

                    

                }

            }
            );

            //cds.OrderBy(o => o.index).ToList().ForEach(fe => {
            //    File.AppendAllText(path, fe.ToString());
            //});
            cds.OrderBy(o => o.LandType).ThenBy(t => t.SurveyNo).ThenBy(t => t.SubDivNo, new AlphanumericComparer()).ToList().ForEach(fe =>
            {
                File.AppendAllText(path, fe.ToString());
            });



        }

        private void button1_Click(object sender, EventArgs e)
        {
            var data = textBox1.Text;

            var d = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var decimalList = new List<decimal>();
            var intList = new List<int>();

            d.ForEach(fe =>
            {

                decimalList.Add(Convert.ToDecimal(fe.Substring(fe.IndexOf(".") + 1).Trim()));

                intList.Add(Convert.ToInt32(fe.Split('.')[0]));
                //var test = fe.Substring(fe.IndexOf("."));


            });

            var addedData = decimalList.Sum();
            var intAddedData = intList.Sum();

            var finalData = addedData + (intAddedData * 100);

            var firstPart = Convert.ToDecimal(Convert.ToInt32(finalData.ToString().Split('.')[0])) / Convert.ToDecimal(100);



            var result = $"{firstPart}.{finalData.ToString().Split('.')[1]}";

            textBox2.Text = result;





        }
    }


    public class ChittaData
    {

        public int index { get; set; }

        public int SurveyNo { get; set; }

        public string SubDivNo { get; set; }

        public string Parappu { get; set; }

        public decimal Theervai { get; set; }

        public int PattaNo { get; set; }

        public string OwnerName { get; set; }

        public string LandType { get; set; }    // N-Nanjai, P- Punjai, M-matravai

        public override string ToString()
        {

            //return String.Format("{7},{0},{1},{2},{3},{4},{5}{6}", SurveyNo, SubDivNo, Parappu, Theervai, OwnerName, LandType, Environment.NewLine, index);
            return String.Format("{0},{1},{2},{3},{4},{5}{6}", SurveyNo, SubDivNo, Parappu, Theervai, OwnerName, LandType, Environment.NewLine);

        }


    }
}
