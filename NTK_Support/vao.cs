using Common;
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
using Aspose.Pdf;

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

        string myFile = @"F:\vanitha - vao\achunthavayal\v-3\chitta-achunthavayal.txt";
        string empty = "";
        private void ProcessChittaFile()
        {

            var content = File.ReadAllText(myFile);
            var pattaas = content.Replace("பட்டா எண்", "$");

            var data = pattaas.Split('$').ToList();

            data.RemoveAt(0); // empty data
            List<ChittaData> cds = new List<ChittaData>();

            data.ForEach(fe =>
            {
                var item = fe.Replace("வ.எண்", empty).Replace("உ எண்", empty).Replace("உறவினர் பெயர்", empty)
                             .Replace("உறவு", empty).Replace("உரிமையாளர் பெயர்", empty).Replace("புல எண்", empty)
                             .Replace("உட்பிரிவு எண்", empty).Replace("நன்செய்", empty).Replace("புன்செய்", empty)
                             .Replace("மற்றவை", empty).Replace("குறிப்பு", empty).Replace("பரப்பு", empty)
                             .Replace("தீர்வை", empty).Replace("மொத்தம்", "TOTAL").Replace('\t', '$');

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
                    string oName = "";
                    int pattaNo = 0;
                    pattaNo = Convert.ToInt32(d3[0].Replace(":", "").Trim());

                    //if (pattaNo == 1235)
                    //{

                    //}

                    var names = d3[1].Split('$');

                    if (isVagai)
                        oName = $"{pattaNo} - {names[4]} வகை ";
                    else
                    {
                        string initial = "";

                        if (Convert.ToInt32(names[2][1]).ToString()[0] == '3')
                            initial = $"{names[2][0]}{names[2][1]}";
                        else
                            initial = $"{names[2][0]}";

                        if (names[2].Trim() == "இல்லை" || names[2].Trim() == "அச்சுந்தன்வயல்")
                            oName = $"{pattaNo} - {names[4]} ";
                        else
                            oName = $"{pattaNo} - {initial}.{names[4]} ";

                        // if already have initial then ignore.
                    }

                    for (int i = 2; i <= d3.Count - 1; i++)
                    {
                        ChittaData d = new ChittaData();
                        d.OwnerName = oName;
                        d.PattaNo = pattaNo;

                        var nums = d3[i].Split('$');

                        d.SurveyNo = Convert.ToInt32(nums[1].Split('-')[0].Trim());
                        d.SubDivNo = nums[1].Split('-')[1].Trim();

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

                        //index = (index + 1);
                        //d.index = index;

                        cds.Add(d);
                    }
                }
            }
            );


            string Nansaipath = @"F:\1N-10.csv";
            string Punsaipath = @"F:\2M-10.csv";
            string Maanaavaaripath = @"F:\3P-11.csv";

            //WriteData(cds, "1N", Nansaipath);
            //WriteData(cds, "2P", Punsaipath);
            //WriteData(cds, "3M", Maanaavaaripath);

        }

        bool forPageList = true;
        public void WriteData(List<ChittaData> data, string filter, string filePath)
        {
            string landType = "";

            if (filter == "1N") landType = "( நன்செய் )";
            else if (filter == "2P") landType = "( புன்செய் )";
            else if (filter == "3M") landType = "( மானாவாரி )";
            else if (filter == "4P") landType = "( புறம்போக்கு )";



            var filteredList = data.Where(w => w.LandType == filter).OrderBy(o => o.LandType).ThenBy(t => t.SurveyNo).ThenBy(t => t.SubDivNo, new AlphanumericComparer()).ToList();
            var finalList = new List<ChittaData>();
            int index = 0;

            var MyPagesList = new List<ChittaData>();


            var fList = new List<ChittaData>();

            var pageCount = filteredList.Count / 7;

            if (filteredList.Count % 7 > 0)
            {
                pageCount = pageCount + 1;
            }

            for (int i = 0; i <= pageCount - 1; i++)
            {
                var html = FileContentReader.DataPageHtml;

                StringBuilder sb = new StringBuilder();

                var temData = filteredList.Skip(i * 7).Take(7).ToList();


                var totalData = new ChittaData()
                {
                    Parappu = GetSumThreeDotNo(temData.Select(s => s.Parappu).ToList(), filter, i + 1),
                    Theervai = temData.Sum(s => s.Theervai),
                    PageNumber = i + 1
                };

                string dataRows = "";

                temData.ForEach(ff => {
                    
                    dataRows += $@"<tr><td class='datahgt' style='min-width:35px;font-weight: bold;'>{ff.SurveyNoStr}</td><td style='min-width:50px;font-weight: bold;'>{ff.SubDivNo}</td>
 <td style='min-width:50px;font-weight: bold;'>{ff.Parappu}</td><td style='min-width:20px;font-weight: bold;'>{ff.TheervaiStr}</td>
 <td style='min-width:20px;'></td><td style='min-width:200px;word-break:break-word;font-weight: bold;'>{ff.OwnerName}</td><td></td><td></td><td></td><td></td><td></td><td></td>
  </tr>
  <tr>
  <td class='datahgt'></td><td></td><td></td><td>
  </td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>
  </tr>";
                });

                dataRows += $@"<td class='datahgt'></td><td></td><td class='footer'>{totalData.Parappu}</td><td class='footer'>{totalData.TheervaiStr}</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>
  </ tr > ";

                html = html.Replace("[data]", dataRows).Replace("[landtype]", landType);

                

                File.WriteAllText($@"F:\vanitha - vao\achunthavayal\v-3\DataPages\DP-{filter}{totalData.PageNumber + 108}.htm", html);

                //HtmlLoadOptions htmloptions = new HtmlLoadOptions();
                // Load HTML file
                //Document doc = new Document(@"F:\vanitha - vao\achunthavayal\v-3\1.htm", htmloptions);
                // Convert HTML file to PDF
                //doc.Save(@"F:\vanitha - vao\achunthavayal\v-3\HTML-to-PDF.pdf");

            }




            int pageListRowNo = 22;

            var pageListCount = MyPagesList.Count / pageListRowNo;

            if (MyPagesList.Count % pageListRowNo > 0)
            {
                pageListCount = pageListCount + 1;
            }


            

            for (int i = 0; i <= pageListCount - 1; i++)
            {
                StringBuilder sb2 = new StringBuilder();
                var PageCountFinal = new List<ChittaData>();

                var pageListtemData = MyPagesList.Skip(i * pageListRowNo).Take(pageListRowNo).ToList();

                // String.Format("{0}\t{1}\t{2}{3}", PageNumberStr, Parappu, TheervaiStr, Environment.NewLine);
                //PageCountFinal.Add(new ChittaData() { PageNumberStr = "பக்கம் எண்." });
                for (int jj = 0; jj < pageListtemData.Count; jj++)
                {
                    PageCountFinal.Add(pageListtemData[jj]);
                    //PageCountFinal.Add(new ChittaData());

                }

                PageCountFinal.ForEach(td => { sb2.Append(td); });
                Clipboard.SetText(sb2.ToString());


                if (MessageBox.Show($"Copied PageList-Data {filter} for page : {i + 1} out of {pageListCount}") == DialogResult.OK)
                {

                }
            }

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
            });

            var addedData = decimalList.Sum();
            var intAddedData = intList.Sum();

            var finalData = addedData + (intAddedData * 100);

            var firstPart = Convert.ToDecimal(Convert.ToInt32(finalData.ToString().Split('.')[0])) / Convert.ToDecimal(100);

            var result = $"{firstPart}.{finalData.ToString().Split('.')[1]}";

            textBox2.Text = result;

        }

        private string GetSumThreeDotNo(List<string> nos, string filter, int pn)
        {

            var decimalList = new List<decimal>();
            var intList = new List<int>();

            nos.ForEach(fe =>
            {
                decimalList.Add(Convert.ToDecimal(fe.Substring(fe.IndexOf(".") + 1).Trim()));
                intList.Add(Convert.ToInt32(fe.Split('.')[0]));
            });

            var addedData = decimalList.Sum();
            var intAddedData = intList.Sum();

            var finalData = addedData + (intAddedData * 100);

            if (finalData >= 100)
            {

                var firstPart = Convert.ToDecimal(Convert.ToInt32(finalData.ToString().Split('.')[0])) / Convert.ToDecimal(100);
                var secPart = $"{finalData.ToString().Split('.')[1]}";

                return $"{firstPart}.{secPart}";
            }
            else
            {
                var f = filter;
                var p = pn;
                return $"0.{finalData}";
            }

        }
    }


    public class ChittaData
    {

        //public ChittaData()
        //{

        //}

        //public ChittaData(string pageNo, string parappu, string theervai)
        //{
        //    PageNumberStr = pageNo;

        //}
        public int index { get; set; }

        public int SurveyNo { get; set; }

        public string SurveyNoStr { get { return SurveyNo == 0 ? "" : SurveyNo.ToString(); } }

        public string SubDivNo { get; set; }

        public string Parappu { get; set; }

        public decimal Theervai { get; set; }
        public string TheervaiStr { get { return Theervai == 0 ? "" : Theervai.ToString(); } }

        public int PattaNo { get; set; }

        public string OwnerName { get; set; }

        public string LandType { get; set; }    // N-Nanjai, P- Punjai, M-matravai

        public int PageNumber { get; set; }



        public string PageNumberStr
        {
            get
            {
                return PageNumber == 0 ? "" : PageNumber.ToString();
            }
        }

        //string pn;
        //public string PageNumberStr {
            
        //   get 
        //    {
        //        return PageNumber == 0 ? "" : PageNumber.ToString(); 
        //    }

        //    set
        //    {
        //        this.pn = value;
        //    }
        //}

        public string PageIndex { get; set; }
       

        public override string ToString()
        {
            // main data 
            //return String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}{6}", SurveyNoStr, SubDivNo, Parappu, TheervaiStr, "", OwnerName, Environment.NewLine);

            return String.Format("{0}\t{1}\t{2}{3}", PageNumberStr, Parappu, TheervaiStr, Environment.NewLine);

        }


    }
}
