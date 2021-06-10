using Common;
using Common.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTK_Support
{
    public partial class vao : Form
    {

        bool isProductionTest = true;

        string myFile = "";
        string content = "";

        string empty = "";
        int pageListRowNo = 24;
        bool forPageList = true;
        int rightPageNo = 6;
        int startPno = 284;

        public vao()
        {
            InitializeComponent();

            if (isProductionTest)
            {
                myFile = @"F:\TN GOV\VANITHA\Vaidehi-Vao\reg data\Chitta_Report-1.pdf";
                content = myFile.GetPdfContent();
            }
            else
            {
                myFile = @"F:\TN GOV\VANITHA\Vaidehi-Vao\reg data\Chitta_Report-1.txt";
                content = File.ReadAllText(myFile);
            }

            ProcessChittaFile();
        }

       
        private void ProcessChittaFile()
        {

            var pattaas = content.Replace("பட்டா எண்", "$");

            var data = pattaas.Split('$').ToList();

            data.RemoveAt(0); // empty data
            List<ChittaData> cds = new List<ChittaData>();

            var testFile = myFile.Replace(".txt", "wrongPattaCount.txt");

            int validCount = 0;

            int otherIssueCount = 0;

            int wrongPattaCount = 0;
            int invalidType3zeroonly = 0;
            int threeDigitIssueCount = 0;
            int continueNoIssueCount = 0;

            int previousPattaNo = 0;
            int currentPattaNo = 0;


            for (int i = 0; i <= data.Count - 1; i++)
            {

                //}
                //data.ForEach(fe =>
                //{
                //var item = fe.Replace("வ.எண்", empty).Replace("உ எண்", empty).Replace("உறவினர் பெயர்", empty)
                //             .Replace("உறவு", empty).Replace("உரிமையாளர் பெயர்", empty).Replace("புல எண்", empty)
                //             .Replace("உட்பிரிவு எண்", empty).Replace("நன்செய்", empty).Replace("புன்செய்", empty)
                //             .Replace("மற்றவை", empty).Replace("குறிப்பு", empty).Replace("பரப்பு", empty)
                //             .Replace("தீர்வை", empty).Replace("மொத்தம்", "TOTAL").Replace('\t', '$');

                //var item = data[i].Replace("வ.எண்.", empty).Replace("உ எண்", empty).Replace("உறவினர் ெபயர்", empty)
                //             .Replace("உறவ", empty).Replace("உரிமைமையாளர் ெபயர்", empty).Replace("புல எண்-", empty)
                //             .Replace(" புல எண் -    ", "")
                //             .Replace("உட்பிரிமவ எண்", empty).Replace("நனெசெய", empty).Replace("புனெசெய", empty)
                //             .Replace("மைற்றைவ", empty).Replace("குறிப்பு", empty).Replace("பரப்பு", empty)
                //             .Replace("தீர்ைவ", empty).Replace("ெமைாத்தம", "TOTAL"); //.Replace('\t', '$');

                var item = data[i].Replace("வ.எண்", empty).Replace("உ எண்", empty).Replace("உறவினர் ெபயர்", empty)
                             .Replace("உறவ", empty).Replace("உரிமைமையாளர் ெபயர்", empty).Replace("புல எண்-", empty)
                             .Replace(". புல எண் -    ", "")
                             .Replace("உட்பிரிமவ எண்", empty).Replace("நனெசெய", empty).Replace("புனெசெய", empty)
                             .Replace("மைற்றைவ", empty).Replace("குறிப்பு", empty).Replace("பரப்பு", empty)
                             .Replace("தீர்ைவ", empty).Replace("ெமைாத்தம", "TOTAL"); //.Replace('\t', '$');
                

                //var rrr = item.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList(); //1
                var rrr = item.Split('\n').Where(w => w.Trim() != empty && w.Contains("புல எண்") == false).ToList(); // 2

                var pattaNO = rrr.First().Replace(":", "").Trim();
                var isNo = pattaNO.isNumber();


                //var justData = rrr.Where(w => w.Contains('-')).ToList(); //1
                var justData = rrr.Where(ww => rrr.IndexOf(ww) >= rrr.FindIndex(w => w.Contains('-'))).ToList(); //2

                bool isValidData = false;

                if (justData.Count > 0)
                    isValidData = justData.Last().Contains("TOTAL");

                if (isNo == false)
                {
                    wrongPattaCount += 1;
                    File.AppendAllText(testFile, "|" + $"[{previousPattaNo}-{pattaNO}]");

                }
                // VALID DATA
                else if (isValidData)
                {
                    validCount += 1;
                    //File.AppendAllText(testFile, "|" + pattaNO);
                }
                // THREE DIGIT _ ISSUE
                else if (justData.Count == 0)
                {
                    threeDigitIssueCount += 1;
                    // File.AppendAllText(testFile, "|" + pattaNO);

                    //File.AppendAllText(testFile, "INVALID-Type-1:" + pattaNO);
                    //File.AppendAllLines(testFile, rrr);
                    //File.AppendAllText(testFile, "==========================================================");
                }
                // ZERO DATA
                else if (justData.All(a => a == "0 - 0.00 "))
                {
                    //invalidType3zeroonly += 1;
                    //File.AppendAllText(testFile, "|" + pattaNO);
                }
                // CONTINUE DATA ISSUE
                else if (currentPattaNo == (previousPattaNo + 1))
                {
                    continueNoIssueCount += 1;
                    //File.AppendAllText(testFile, "|" + $"[{previousPattaNo}-{currentPattaNo}]");
                }
                // OTHER ISSUES
                else
                {
                    otherIssueCount += 1;
                    //File.AppendAllText(testFile, "|" + pattaNO);
                    //File.AppendAllText(testFile, "INVALID-Type-2" + pattaNO);
                    //File.AppendAllLines(testFile, rrr);
                    //File.AppendAllText(testFile, "==========================================================");
                }

                if (isNo)
                    previousPattaNo = pattaNO.ToInt32();

                //File.AppendAllText(myFile.Replace(".txt", "-test-2.txt"), item);
                //File.AppendAllText(myFile.Replace(".txt", "-test-2.txt"), "-------------------------------------" + Environment.NewLine);

                //File.AppendAllLines(myFile.Replace(".txt", "-test-2.txt"), rrr);
                //File.AppendAllText(myFile.Replace(".txt", "-test-2.txt"),"==========================================================");


                //NEW CODE


                //NEW CODE
                //bool isVagai = false;

                //var dd2 = (from tt in rrr
                //               //where tt.Replace("$", "").Trim() != string.Empty
                //           where tt.Trim() != string.Empty
                //           select tt).ToList();

                //var dd = new List<string>(dd2);

                //for (int i = 2; i < dd.Count; i++)
                //{
                //    if (dd[i].Contains("-") == false && dd[i].Contains(":") == false)
                //    {
                //        isVagai = true;
                //        dd2[dd2.IndexOf(dd2[i])] = "*" + dd2[i];
                //    }
                //}


                //if (dd2.Count > 3)
                //{
                //    var d3 = dd2.Take(dd2.Count - 1).ToList().Where(w => w.StartsWith("*") == false).ToList();
                //    string oName = "";
                //    int pattaNo = 0;
                //    pattaNo = Convert.ToInt32(d3[0].Replace(":", "").Trim());

                //    var names = d3[1].Split('$');

                //    if (isVagai)
                //        oName = $"{pattaNo} - {names[4]} வகை ";
                //    else
                //    {
                //        string initial = "";


                //        if (names[2].Trim().StartsWith("இரா"))
                //            initial = "இரா";
                //        else if (Convert.ToInt32(names[2][1]).ToString()[0] == '3')
                //            initial = $"{names[2][0]}{names[2][1]}";
                //        else
                //            initial = $"{names[2][0]}";

                //        if (names[2].Trim() == "இல்லை" || names[2].Trim() == "அச்சுந்தன்வயல்")
                //            oName = $"{pattaNo} - {names[4]} ";
                //        else
                //            oName = $"{pattaNo} - {initial}.{names[4]} ";

                //        // if already have initial then ignore.
                //    }

                //    for (int i = 2; i <= d3.Count - 1; i++)
                //    {
                //        ChittaData d = new ChittaData();
                //        d.OwnerName = oName;
                //        d.PattaNo = pattaNo;

                //        var nums = d3[i].Split('$');

                //        d.SurveyNo = Convert.ToInt32(nums[1].Split('-')[0].Trim());
                //        d.SubDivNo = nums[1].Split('-')[1].Trim();

                //        if (nums[3].Trim() != "0.00")    // N - நன்செய் 
                //        {
                //            d.LandType = "1N";
                //            d.Parappu = nums[2].Replace("-", ".").Trim().Replace(" ", "");
                //            d.Theervai = Convert.ToDecimal(nums[3]);
                //        }
                //        else if (nums[5].Trim() != "0.00") // P - புன்செய் 
                //        {
                //            d.LandType = "2P";
                //            d.Parappu = nums[4].Replace("-", ".").Trim().Replace(" ", "");
                //            d.Theervai = Convert.ToDecimal(nums[5]);
                //        }
                //        else if (nums[7].Trim() != "0.00") // M - மானாவாரி  
                //        {
                //            d.LandType = "3M";
                //            d.Parappu = nums[6].Replace("-", ".").Trim().Replace(" ", "");
                //            d.Theervai = Convert.ToDecimal(nums[7]);
                //        }

                //        cds.Add(d);
                //    }
                //}
                //}
                //);
            }

            File.AppendAllText(testFile, $"total record: {data.Count}");
            File.AppendAllText(testFile, "VALID : " + validCount);
            File.AppendAllText(testFile, "wrongPattaCount" + wrongPattaCount);
            File.AppendAllText(testFile, "INVALID-Type-1" + threeDigitIssueCount);
            File.AppendAllText(testFile, "INVALID-Type-ZeroOnly" + invalidType3zeroonly);
            File.AppendAllText(testFile, "INVALID-Type-ZeroOnly" + invalidType3zeroonly);
            File.AppendAllText(testFile, "INVALID-Type-2" + otherIssueCount);


            CreateInitialPages(); // 10 pages
            WriteData(cds, "1N"); // from chitta
            WriteData(cds, "2P");  // from chitta
            WriteData(cds, "3M");  // from chitta
            WriteData(cds, "4P");  // from a-reg


        }

        private void CreateInitialPages()
        {


        }


        public void WriteData(List<ChittaData> data, string filter)
        {
            List<ChittaData> PageTotalList = new List<ChittaData>();
            string landType = "";

            if (filter == "1N") landType = "( நன்செய் )";
            else if (filter == "2P")
            {
                landType = "( புன்செய் )";
                rightPageNo = rightPageNo - 1;
            }
            else if (filter == "3M") landType = "( மானாவாரி )";
            else if (filter == "4P") landType = "( புறம்போக்கு )";

            var filteredList = new List<ChittaData>();

            if (filter == "4P")
            {
                var csvLines = File.ReadAllLines(@"F:\vanitha - vao\achunthavayal\v-3\DataPages/puram.txt");

                csvLines.ToList().ForEach(fe =>
                {
                    var dt = fe.Split(',').ToList();

                    filteredList.Add(new ChittaData
                    {
                        SurveyNo = Convert.ToInt32(dt[0].Trim()),
                        SubDivNo = dt[1].Trim(),
                        Parappu = dt[2].Trim(),
                        OwnerName = dt[3].Trim()
                    });
                });


            }
            else
            {
                filteredList = data.Where(w => w.LandType == filter).OrderBy(o => o.LandType).ThenBy(t => t.SurveyNo).ThenBy(t => t.SubDivNo, new AlphanumericComparer()).ToList();

            }

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
                string dataRows = "";

                var totalData = new ChittaData()
                {
                    Parappu = GetSumThreeDotNo(temData.Select(s => s.Parappu).ToList(), filter, i + 1),
                    Theervai = temData.Sum(s => s.Theervai),
                    PageNumber = i + 1
                };

                PageTotalList.Add(totalData);

                temData.ForEach(ff =>
                {

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
                // Data Pages html.
                File.WriteAllText($@"F:\vanitha - vao\achunthavayal\v-3\DataPages\DP-{filter}{totalData.PageNumber + 0}.htm", html);
            }

            var pageListCount = PageTotalList.Count / pageListRowNo;

            if (PageTotalList.Count % pageListRowNo > 0) pageListCount = pageListCount + 1;

            for (int i = 0; i <= pageListCount - 1; i++)
            {
                var html2 = FileContentReader.pageListHtml;
                string dataRows2 = "";
                StringBuilder sb2 = new StringBuilder();
                var PageCountFinal = new List<ChittaData>();

                var pageListtemData = PageTotalList.Skip(i * pageListRowNo).Take(pageListRowNo).ToList();

                pageListtemData.ForEach(ff =>
                {

                    rightPageNo = rightPageNo + 1;

                    dataRows2 += $@" <tr class='head'>
 <td>{rightPageNo}</td>
 <td>{ff.Parappu}</td>
 <td></td>
 <td></td>
 </tr>";
                });

                if (pageListtemData.Count < pageListRowNo)
                {
                    int diff = pageListRowNo - pageListtemData.Count;

                    for (int di = 1; di <= diff; di++)
                    {

                        dataRows2 += $@" <tr class='head'>
                                         <td></td>
                                         <td></td>
                                         <td></td>
                                         <td></td>
                                         </tr>";

                    }

                }

                startPno = startPno + 1;
                html2 = html2.Replace("[data]", dataRows2).Replace("[landtype]", landType.Replace("(", "").Replace(")", "")).Replace("[pn]", "");
                // Write to page total list.
                File.WriteAllText($@"F:\vanitha - vao\achunthavayal\v-3\DataPages\TP-{filter}{startPno}.htm", html2);

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

        public string PageIndex { get; set; }


        public override string ToString()
        {
            return String.Format("{0}\t{1}\t{2}{3}", PageNumberStr, Parappu, TheervaiStr, Environment.NewLine);
        }

    }
}