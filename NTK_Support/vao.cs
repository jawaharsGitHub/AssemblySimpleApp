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
        string testFile = "";

        List<Patta> pattaList;
        Patta pattaSingle;
        List<string> relationTypes;

        public vao()
        {
            InitializeComponent();
            pattaList = new List<Patta>();
            relationTypes = new List<string>() {
                "தந்தைத",
                "கணவன",
                "காப்பாளர்",
                "மைகன",
                "மைைனவி"
            };



            if (isProductionTest)
            {
                myFile = @"F:\TN GOV\VANITHA\Vaidehi-Vao\reg data\Chitta_Report-1.pdf";
                content = myFile.GetPdfContent();
                //testFile = myFile.Replace(".pdf", "valid.txt");
            }
            else
            {
                myFile = @"F:\TN GOV\VANITHA\Vaidehi-Vao\reg data\Chitta_Report-1.txt";
                content = File.ReadAllText(myFile);
                //testFile = myFile.Replace(".txt", "valid.txt");
            }

            ProcessChittaFile();
        }

        private void updateTestFileName(string fn)
        {
            testFile = myFile.Replace("Chitta_Report-1.pdf", $"{fn}.txt");
        }

        private void ProcessChittaFile()
        {

            updateTestFileName("vagaiData");
            var pattaas = content.Replace("பட்டா எண்", "$");

            var data = pattaas.Split('$').ToList();

            data.RemoveAt(0); // empty data
            List<ChittaData> cds = new List<ChittaData>();
            bool isBreakingData = false;
            pattaList = new List<Patta>();

            for (int i = 0; i <= data.Count - 1; i++)
            {
                pattaSingle = new Patta();

                isBreakingData = false;

                var item = data[i].Replace("வ.எண்", empty).Replace("உ எண்", empty).Replace("உறவினர் ெபயர்", empty)
                             .Replace("உறவ", empty).Replace("உரிமைமையாளர் ெபயர்", empty).Replace("புல எண்-", empty)
                             .Replace(". புல எண் -    ", "")
                             .Replace("உட்பிரிமவ எண்", empty).Replace("நனெசெய", empty).Replace("புனெசெய", empty)
                             .Replace("மைற்றைவ", empty).Replace("குறிப்பு", empty).Replace("பரப்பு", empty)
                             .Replace("தீர்ைவ", empty).Replace("ெமைாத்தம", "TOTAL");

                var fullData = item.Split('\n').Where(w => w.Trim() != empty && w.Contains("புல எண்") == false).ToList();

                fullData = (from r in fullData
                            where r.Contains("digitally") == false &&
                                  r.Contains("_________________________________________________________________________________________________") == false
                            select r).ToList();

                var pattaNO = fullData.First().Replace(":", "").Trim();
                var isNo = pattaNO.isNumber();

                if (isNo == false)
                {
                    pattaSingle.PattaType = PattaType.InValidPatta;
                    pattaSingle.ErrorData = fullData.ListToString();
                    pattaList.Add(pattaSingle);
                    continue;
                }
                else
                {
                    pattaSingle.PattaEn = Convert.ToInt32(pattaNO);
                }

                var dataIndex = fullData.FindIndex(w => w.Contains('-'));

                var headerData = fullData.Take(dataIndex).ToList();
                var memberData = fullData.Where(ww => fullData.IndexOf(ww) >= dataIndex).ToList();
                var totalData = memberData.Last();
                memberData.RemoveAt(memberData.Count - 1);

                var totalRecord = (memberData.Count - memberData.ToList().Where(w => w.Contains("-") == false).Count());

                // zero record.
                if (memberData.Count == 0)
                {
                    pattaSingle.PattaType = PattaType.Zero;
                    pattaSingle.ErrorData = fullData.ListToString();
                    pattaList.Add(pattaSingle);
                    continue;
                }
                else if (totalRecord == memberData.Count)
                {
                    // perfect data
                    if ((memberData.All(a => a.Split('-').Count() == 5 &&
                        totalData.Split('-').Count() == 4)) == false)
                    {
                        if (memberData.All(a => a.Split('-').Count()) == 6 &&
                       (totalData.Split('-').Count() == 4) == false)
                        {
                            pattaSingle.PattaType = PattaType.Error;
                            pattaSingle.ErrorData = fullData.ListToString();
                            pattaList.Add(pattaSingle);
                            continue;

                        }
                        else
                        {
                            pattaSingle.PattaType = PattaType.ValidAndNoSubdivision;
                            pattaSingle.ErrorData = fullData.ListToString();
                            pattaList.Add(pattaSingle);
                            

                        }
                           
                    }

                }
                else if ((totalRecord * 2) == memberData.Count)
                {
                    isBreakingData = IsValidMemberBreakData(memberData);

                    if ((GetEvenIndexData(memberData).All(e => e.Split('-').Count() == 5) &&
                        GetOddIndexData(memberData).All(o => o.Split('-').Count() == 1) &&
                        totalData.Split('-').Count() == 4) == false)
                    {

                        if ((GetEvenIndexData(memberData).All(e => e.Split('-').Count() == 6) &&
                        GetOddIndexData(memberData).All(o => o.Split('-').Count() == 1) &&
                        totalData.Split('-').Count() == 4) == false)
                        {
                            pattaSingle.PattaType = PattaType.Error;
                            pattaSingle.ErrorData = fullData.ListToString();
                            pattaList.Add(pattaSingle);
                            continue;

                        }
                        else
                        {
                            pattaSingle.PattaType = PattaType.ValidAndNoSubdivision;
                            pattaSingle.ErrorData = fullData.ListToString();
                            pattaList.Add(pattaSingle);
                        }

                    }
                }
                else
                {
                    var isValidMemberData = GetEvenIndexData(memberData);

                    pattaSingle.PattaType = PattaType.Unknown;
                    pattaSingle.ErrorData = fullData.ListToString();
                    pattaList.Add(pattaSingle);
                    continue;
                }
                //else
                //{
                    
                //}

                // Gets the name
                pattaSingle.isVagai = (headerData.Count > 3);

                var nameRow = headerData[1];
                if (relationTypes.Any(a => nameRow.Split(' ').ToList().Contains(a)) == false)
                {
                    Debug.WriteLine($"{pattaNO} - {nameRow}");
                }

                //List<LandDetail> lds = new List<LandDetail>();

                // Gets the data.
                if (isBreakingData)
                {
                    var actualData = GetEvenIndexData(memberData);
                    var breakData = GetOddIndexData(memberData);

                    if (actualData.Count == breakData.Count)
                    {
                        pattaSingle.landDetails = ProcessLandType(actualData, breakData);
                        pattaList.Add(pattaSingle);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                }

                else
                {
                    var actualData = GetEvenIndexData(memberData);
                    pattaSingle.landDetails = ProcessLandType(actualData);
                    pattaList.Add(pattaSingle);
                }


                //pattaSingle.landDetails = lds;
                //pattaList.Add(pattaSingle);


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
            }

            // Full Report
            FinalReport fr = new FinalReport(pattaList);
            var result = fr.ToString();

            if (fr.IsFullProcessed == false)
            {
                MessageBox.Show($"{fr.NotProcessedData} not processes");
            }


            CreateInitialPages(); // 10 pages
            WriteData(cds, "1N"); // from chitta
            WriteData(cds, "2P");  // from chitta
            WriteData(cds, "3M");  // from chitta
            WriteData(cds, "4P");  // from a-reg


        }

        private List<LandDetail> ProcessLandType(List<string> actualData, List<string> breakData = null)
        {

            var landList = new List<LandDetail>();
            LandDetail land = null;

            for (int index = 0; index <= actualData.Count - 1; index++)
            {
                land = new LandDetail();

                var ad = actualData[index].Split('-').Where(w => w.Trim() != "").Select(s => s.Trim() + "-").ToList();

                if (breakData != null && breakData.Count > 0)
                {
                    land.PulaEn = ad[0].Split(' ')[1] + breakData[index];
                }
                else
                {
                    land.PulaEn = ad[0].Split(' ')[1];
                }

                land.nansaiParappu = ad[1] + ad[2].Split(' ')[0];
                land.nansaiTheervai = ad[2].Split(' ')[1];

                land.punsaiParappu = ad[2].Split(' ')[2] + ad[3].Split(' ')[0];
                land.punsaiTheervai = ad[3].Split(' ')[1];

                land.maanavariParappu = ad[3].Split(' ')[2] + ad[4].Split(' ')[0];
                land.maanavariTheervai = ad[4].Split(' ')[1].Replace("-", "");

                landList.Add(land);
            }

            return landList;

        }

        private void CreateInitialPages()
        {


        }

        private List<string> GetEvenIndexData(List<string> dataList)
        {
            return dataList.Where(w => dataList.IndexOf(w) % 2 == 0).ToList();

        }

        private List<string> GetOddIndexData(List<string> dataList)
        {
            return dataList.Where(w => dataList.IndexOf(w) % 2 != 0).ToList();

        }

        private bool IsValidMemberBreakData(List<string> memberData)
        {
            return GetEvenIndexData(memberData).All(e => e.Contains('-')) && 
                   GetOddIndexData(memberData).All(o => !o.Contains('-'));
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

    public class Patta
    {

        public int PattaEn { get; set; }

        public List<LandDetail> landDetails { get; set; }

        //public bool haveError { get; set; }

        public PattaType PattaType { get; set; }

        public bool isVagai { get; set; }

        public string ErrorData { get; set; }

    }

    public enum PattaType
    {
        Valid,
        ValidAndNoSubdivision,
        Zero,
        InValidPatta,
        Error,
        PartailBreak,
        Unknown
    }

    public class LandDetail
    {

        private string _pulaEn;
        // புல எண் - உட்பிரிவு எண் (survey no - subdidvision no)
        public string PulaEn {
            get { 
                    haveSubdivision = _pulaEn.Contains("-");
                return _pulaEn.Replace("-", "");

                }
            set { _pulaEn = value; }
        }

        // நன்செய் பரப்பு 
        public string nansaiParappu { get; set; }

        // நன்செய் தீர்வை  
        public string nansaiTheervai { get; set; }

        // புன்செய் பரப்பு 
        public string punsaiParappu { get; set; }

        // புன்செய் தீர்வை 
        public string punsaiTheervai { get; set; }

        // மானாவரி பரப்பு
        public string maanavariParappu { get; set; }

        // மானாவரி தீர்வை
        public string maanavariTheervai { get; set; }

        public LandType LandType { get; set; }

        public bool haveSubdivision { get; set; } = true;

    }

    public enum LandType
    {
        Nansai,
        Punsai,
        Maanaavari,
        Porambokku,
    }


    public class FinalReport
    {
        public FinalReport(List<Patta> PattaListCtr)
        {
            PattaList = PattaListCtr;
            KeyValue singleData = null;
            CountData = new List<KeyValue>();

            int processedCount = 0;
            foreach (PattaType rt in Enum.GetValues(typeof(PattaType)))
            {
                singleData = new KeyValue();
                singleData.Value = PattaList.Count(c => c.PattaType == rt);
                processedCount += singleData.Value;
                singleData.Caption = Enum.GetName(typeof(PattaType), rt);
                singleData.CaptionData = PattaList.Where(c => c.PattaType == rt).ToList().Select(s => s.ErrorData).ToList();
                CountData.Add(singleData);
            }

            IsFullProcessed = (PattaList.Count == processedCount);

            NotProcessedData = (PattaList.Count - processedCount);

            CountData.Add(new KeyValue("Total Record", PattaList.Count));
            CountData.Add(new KeyValue("Not Processed", NotProcessedData));


        }
        public List<KeyValue> CountData { get; set; }

        public List<Patta> PattaList { get; set; }

        public bool IsFullProcessed { get; set; }

        public int NotProcessedData { get; set; }
        public override string ToString()
        {

            var reportStr = "";

            if (PattaList == null || PattaList.Count == 0)
                return reportStr;

            return CountData.Select(s => s.ToString()).ToList().ListToString();
        }

    }


}