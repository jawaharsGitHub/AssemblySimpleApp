using Common;
using Common.ExtensionMethod;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using NTK_Support.AdangalTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace NTK_Support
{
    public partial class vao : Form
    {
        int recordPerPage = 8;
        int pageTotalrecordPerPage = 25;

        string chittaFile = "";
        string aRegFile = "";
        string chittaContent = "";
        string aRegContent = "";

        string empty = "";
        string tamilMoththam = "மொத்தம்";
        int pageListRowNo = 24;
        int rightPageNo = 6;
        int startPno = 284;
        string villageName = "";

        PattaList pattaList;
        List<LandDetail> WholeLandList;
        List<Adangal> AdangalList;
        List<Adangal> PurambokkuAdangalList;
        List<Adangal> fullAdangalFromjson;
        Patta pattaSingle;
        List<string> relationTypes;
        List<string> relationTypesCorrect;


        string firstPage;
        string leftEmpty;
        string leftCertEmpty;
        string rightCertEmpty;

        int pageNumber = 0;
        List<PageTotal> pageTotalList = null;
        List<PageTotal> pageTotal2List = null;
        List<PageTotal> pageTotal3List = null;

        List<string> notInPdfToBeAdded;
        List<string> notInOnlineToBeDeleted;

        public vao()
        {
            InitializeComponent();
            BindDropdown(ddlDistrict, DataAccess.GetDistricts(), "Display", "Value");
        }

        private void BindDropdown(ComboBox cb, object dataSource, string DisplayMember, string ValueMember)
        {
            cb.DataSource = dataSource;
            cb.DisplayMember = DisplayMember;
            cb.ValueMember = ValueMember;
        }

        private void ProcessNames()
        {

            var namesContent = File.ReadAllText(@"F:\TN GOV\VANITHA\Vaidehi-Vao\reg data\22-Names.txt");
            //.Where(w => w.Contains("District :") == false).ToList();
            //namesContent

            //string sPattern = @"[a-zA-Z_\s]:[\s0-9]"; //$"[a-zA-Z]+:[0-9]";

            //var nm = namesContent.Split(Environment.NewLine.ToCharArray())
            //    .Where(w => 
            //    //w.Contains("Ramanathapuram") == false &&
            //    //w.Contains("Taluk") == false &&
            //    //w.Contains("Village") == false &&
            //    //w.Contains("ACHUTHANVAYAL") == false &&
            //    //w.Contains("District") == false &&
            //    w.Trim() != empty)
            //    .ToList(); 

            //for (int i = 0; i <= nm.Count - 1; i++)
            //{
            //    //if (nm[i].Contains("பட்டா எண்")) continue;

            //    //var matches = nm[i].Contains(":") && Regex.Matches(nm[i].Replace(" ",""), sPattern);

            //    //if(matches)
            //    //{

            //    //}
            //    //else
            //    //{

            //    //}

            //    //if(matches.Count == 1)
            //    //{
            //    //    Debug.WriteLine(matches[0]);
            //    //    namesContent = namesContent.Replace(
            //    //        matches[0].ToString().Split(':')[0], 
            //    //        $"பட்டா எண்");


            //    //    //nm[i] = nm[i].Replace(
            //    //    //    matches[0].ToString().Split(':')[0],
            //    //    //    $"பட்டா எண்");

            //    //}
            //    //else if (matches.Count > 1)
            //    //{


            //    //}
            //}

            namesContent = namesContent.Replace("பட்டா எண்", "$");
            var tn = namesContent.Split('$').Where(w => w.Trim().Trim() != empty).ToList();
            tn.RemoveAt(0);
            List<string> fullData;
            List<string> errorCount = new List<string>();
            for (int i = 0; i <= tn.Count - 1; i++)
            {
                fullData = tn[i].Split('\n').Where(w => w.Trim() != empty).ToList();

                fullData = tn[i].Split('\n').Where(w =>
                                                    //w.Trim() != empty &&
                                                    w.Contains("வ.எண்") == false &&
                                                    w.Contains("மொத்தம்") == false &&
                                                    w.Contains("000") == false &&
                                                    w.Contains("|")).ToList();

                if (fullData.Count == 1)
                    Debug.WriteLine($"{i + 1} : {fullData[0]}");
                else
                {
                    Debug.WriteLine($"{i} : ERROR: {tn[i]}");
                    errorCount.Add(i + ":" + tn[i].Split(Environment.NewLine.ToCharArray())[0].Replace(" ", ""));
                }
            }


        }
        private void ProcessAreg()
        {
            PurambokkuAdangalList = new List<Adangal>();

            var aregPatta = aRegContent.Split(Environment.NewLine.ToCharArray()).Where(w => w.Contains("புறமேபாககு")).ToList();

            foreach (var item in aregPatta)
            {
                var d = item.Split(' ').Where(w => w.Trim() != "").ToList();

                try
                {
                    PurambokkuAdangalList.Add(new Adangal()
                    {
                        NilaAlavaiEn = d[0].ToInt32(),
                        UtpirivuEn = d[1],
                        OwnerName = d[16],
                        Parappu = $"{d[11]}.{d[12]}",
                        //Theervai = $"{d[11]}.{d[12]}",
                        Anupathaarar = d[16],
                        LandType = LandType.Porambokku
                    });

                }
                catch (Exception)
                {
                    PurambokkuAdangalList.Add(new Adangal()
                    {
                        NilaAlavaiEn = d[0].ToInt32(),
                        UtpirivuEn = d[1],
                        LandType = LandType.PorambokkuError
                    });
                }


            }

            AdangalList.AddRange(PurambokkuAdangalList);

            fullAdangalFromjson = DataAccess.AdangalToJson(AdangalList, villageName);


        }

        private void ProcessChittaFile()
        {

            var pattaas = chittaContent.Replace("பட்டா எண்", "$");
            var data = pattaas.Split('$').ToList();
            villageName = data.First().Split(':')[3].Trim();

            if (DialogResult.No == MessageBox.Show($"{villageName} village?", "Confirm", MessageBoxButtons.YesNo))
                return;


            data.RemoveAt(0); // first is empty data

            List<ChittaData> cds = new List<ChittaData>();
            bool isFullBreakData, isPartialBreakData = false;
            List<string> brkData;
            List<string> nonBkData;
            pattaList = new PattaList();



            for (int i = 0; i <= data.Count - 1; i++)
            {
                List<string> fullData = null;

                try
                {
                    #region Initial Process...

                    pattaSingle = new Patta();

                    isFullBreakData = isPartialBreakData = false;
                    brkData = new List<string>();
                    nonBkData = new List<string>();

                    var item = data[i].Replace("வ.எண்", empty).Replace("உ எண்", empty).Replace("உறவினர் ெபயர்", empty)
                                 .Replace("உறவ", empty).Replace("உரிமைமையாளர் ெபயர்", empty).Replace("புல எண்-", empty)
                                 .Replace(". புல எண் -    ", "")
                                 .Replace("உட்பிரிமவ எண்", empty).Replace("நனெசெய", empty).Replace("புனெசெய", empty)
                                 .Replace("மைற்றைவ", empty).Replace("குறிப்பு", empty).Replace("பரப்பு", empty)
                                 .Replace("தீர்ைவ", empty).Replace("ெமைாத்தம", "TOTAL")
                                 .Replace("--", "உறவினர் இல்லை");

                    fullData = item.Split('\n').Where(w => w.Trim() != empty && w.Contains("புல எண்") == false).ToList();

                    fullData = (from r in fullData
                                where r.Contains("digitally") == false &&
                                      r.Contains("_________________________________________________________________________________________________") == false
                                select r).ToList();

                    var pattaNO = fullData.First().Replace(":", "").Trim();
                    var isNo = pattaNO.isNumber();

                    #endregion

                    #region "Identify PattaType"

                    if (isNo == false)
                    {
                        pattaList.AddAndUpdateList(pattaSingle, PattaType.InValidPatta, fullData);
                        continue;
                    }

                    pattaSingle.PattaEn = Convert.ToInt32(pattaNO);

                    var dataStartIndex = fullData.FindIndex(w => w.Contains('-'));
                    var headerData = fullData.Take(dataStartIndex).ToList();
                    var memberData = fullData.Where(ww => fullData.IndexOf(ww) >= dataStartIndex)
                                             .Where(ww => ww.Replace("ே", "").Replace("\0", "").Trim() != "")
                                            .ToList();



                    var totalData = memberData.Last();
                    memberData.RemoveAt(memberData.Count - 1);

                    var exactMemDat = memberData.ToList().Where(w => w.Contains("-") == false);
                    var totalRecord = memberData.Count - exactMemDat.Count(); // its for breaking record.

                    // zero record.
                    if (memberData.Count == 0 || headerData.Count == 0)
                    {
                        pattaList.AddAndUpdateList(pattaSingle, PattaType.Zero, fullData);
                        continue;
                    }

                    else if (totalRecord == memberData.Count)
                    {
                        var pt = isAllmemberDataValid(memberData, totalData);
                        if (pt == PattaType.KnownError || pt == PattaType.TotalRecordIssue)
                        {
                            pattaList.AddAndUpdateList(pattaSingle, pt, fullData);
                            continue;
                        }

                        pattaSingle.PattaType = pt;
                    }

                    else if ((totalRecord * 2) == memberData.Count)
                    {
                        var pt = isAllmemberBreakDataValid(memberData, totalData);
                        if (pt == PattaType.KnownError || pt == PattaType.TotalRecordIssue)
                        {
                            pattaList.AddAndUpdateList(pattaSingle, pt, fullData);
                            continue;
                        }

                        pattaSingle.PattaType = pt;
                        if (pt == PattaType.Valid) isFullBreakData = true;
                    }
                    else
                    {

                        if (IsSomeDots(memberData)) pattaSingle.PattaType = PattaType.SomeDots;

                        if (pattaSingle.PattaType == PattaType.SomeDots || pattaSingle.PattaType == PattaType.Valid)
                        {
                            var d = IsPartialBreak(memberData);

                            brkData = d.bk;
                            nonBkData = d.nobk;
                            isPartialBreakData = true;

                            if (pattaSingle.PattaType == PattaType.Valid)
                                pattaSingle.PattaType = PattaType.PartailBreak;
                        }
                        else
                        {
                            // UNKNOWN ERROR
                            pattaList.AddAndUpdateList(pattaSingle, PattaType.Unknown, fullData);
                            continue;
                        }
                    }

                    #endregion

                    #region "Process Owner Name"

                    // Gets the name
                    pattaSingle.isVagai = (headerData.Count > 3);

                    var nameRow = headerData[1];
                    if (relationTypes.Any(a => nameRow.Split(' ').ToList().Contains(a))) // have valid names.
                    {
                        var delitList = relationTypes.Intersect(nameRow.Split(' ').ToList()).ToList();

                        if (delitList.Count == 1)
                        {
                            var delimit = delitList[0];
                            pattaSingle.PattaTharar = nameRow.Replace(delimit, "$").Split('$')[1];
                            var d = nameRow.Replace(delimit, "$").Split('$');

                            //pattaSingle.PattaTharar = ApplyUnicode(d[1]);
                            //pattaSingle.PattaTharar = $"{d[1]} {delimit} {d[0]}";
                            pattaSingle.PattaTharar = $"{d[1]}";
                        }
                        else
                        {
                            // ERROR!
                            pattaList.AddAndUpdateList(pattaSingle, PattaType.TwoNameDelimit, fullData);
                            continue;
                        }
                    }
                    else
                    {
                        // ERROR!
                        pattaList.AddAndUpdateList(pattaSingle, PattaType.NameIssue, fullData);
                        continue;
                    }

                    #endregion

                    #region Process Land Data

                    // Gets the data.
                    if (isPartialBreakData)
                    {
                        var actualData = GetEvenIndexData(brkData);
                        var breakData = GetOddIndexData(brkData);

                        if (actualData.Count == breakData.Count) // Partial Break
                            pattaSingle.landDetails = ProcessLandType(actualData, breakData);
                        else
                            MessageBox.Show("isPartialBreakData Error!");

                        pattaSingle.landDetails.AddRange(ProcessLandType(nonBkData));  // partial Perfect 
                    }
                    else if (isFullBreakData)
                    {
                        var actualData = GetEvenIndexData(memberData);
                        var breakData = GetOddIndexData(memberData);

                        if (actualData.Count == breakData.Count) // full break
                            pattaSingle.landDetails = ProcessLandType(actualData, breakData);
                        else
                            MessageBox.Show("isFullBreakData Error!");

                    }
                    else
                    { // PERFECT DATA
                        pattaSingle.landDetails = ProcessLandType(memberData);
                    }

                    #endregion


                    pattaList.AddAndUpdateList(pattaSingle, pattaSingle.PattaType, fullData);

                }
                catch (Exception ex)
                {
                    if (pattaSingle.PattaType == PattaType.Valid)
                    {
                        pattaList.AddAndUpdateList(pattaSingle, PattaType.ValidException, fullData);
                    }
                    else
                    {
                        pattaList.AddAndUpdateList(pattaSingle, PattaType.Exception, fullData);
                    }

                    continue;
                }
            }

            WholeLandList = pattaList.SelectMany(x => x.landDetails.Select(y => y)).ToList();

            AdangalList = (from wl in WholeLandList
                               .Where(w => w.LandType != LandType.Other)
                                           .OrderBy(o => o.LandType)
                                           .ThenBy(o => o.SurveyNo)
                                           .ThenBy(t => t.Subdivision, new AlphanumericComparer()).ToList()
                           select new Adangal()
                           {
                               NilaAlavaiEn = wl.SurveyNo,
                               UtpirivuEn = wl.Subdivision,
                               OwnerName = wl.OwnerName,
                               Parappu = wl.Parappu,
                               Theervai = wl.Theervai,
                               Anupathaarar = wl.Anupathaarar,
                               LandType = wl.LandType
                           }).ToList();


        }

        private void ProcessFullReport()
        {
            // Full Report
            FinalReport fr = new FinalReport(pattaList);
            //var result = fr.ToString();

            BindDropdown(ddlPattaTypes, fr.CountData, "DisplayMember", "Id");
            BindDropdown(ddlListType, GetListTypes(), "Caption", "Id");
            ddlListType.SelectedIndex = 3;
            BindDropdown(ddlLandTypes, GetLandTypes(), "Caption", "Id");


            if (fr.IsFullProcessed == false)
            {
                MessageBox.Show($"{fr.NotProcessedData} not processes");
            }


        }

        private List<KeyValue> GetListTypes()
        {
            return new List<KeyValue> {
                new KeyValue() { Id = 1, Caption = "PATTA" },
                new KeyValue() { Id = 2, Caption = "LANDDETAIL" },
                new KeyValue() { Id = 3, Caption = "ADANGAL" },
                new KeyValue() { Id = 4, Caption = "JSON-ADANGAL" }
            };

        }

        private List<KeyValue> GetFullfilledOptions()
        {
            return new List<KeyValue> {

                new KeyValue() { Id = -1, Caption = "--select--" },
                new KeyValue() { Id = 1, Caption = "Fullfilled" },
                new KeyValue() { Id = 2, Caption = "Extend" },
                new KeyValue() { Id = 3, Caption = "SomeDots" }
            };

        }

        private List<KeyValue> GetLandTypes()
        {
            var landTypeSource = new List<KeyValue>();

            landTypeSource.Add(new KeyValue() { Caption = "ALL", Id = -1 });
            foreach (LandType rt in Enum.GetValues(typeof(LandType)))
            {

                landTypeSource.Add(new KeyValue()
                {
                    Caption = Enum.GetName(typeof(LandType), rt),
                    Id = (int)rt
                });
            }

            return landTypeSource;
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
                    land.nansaiParappu = ad[1] + ad[2].Split(' ')[0];
                }
                else
                {

                    if (ad[1].Split(' ').Count() == 2)
                    {
                        land.PulaEn = ad[0].Split(' ')[1] + ad[1].Split(' ')[0];
                        land.nansaiParappu = ad[1].Split(' ')[1] + ad[2].Split(' ')[0];
                    }
                    else if (ad[1].Split(' ').Count() == 1)
                    {
                        land.PulaEn = ad[0].Split(' ')[1];
                        land.nansaiParappu = ad[1].Split(' ')[0] + ad[2].Split(' ')[0];
                    }
                    else
                        throw new Exception();
                }
                land.nansaiTheervai = ad[2].Split(' ')[1];

                land.punsaiParappu = ad[2].Split(' ')[2] + ad[3].Split(' ')[0];
                land.punsaiTheervai = ad[3].Split(' ')[1];

                land.maanavariParappu = ad[3].Split(' ')[2] + ad[4].Split(' ')[0];
                land.maanavariTheervai = ad[4].Split(' ')[1].Replace("-", "");


                landList.Add(land);
            }

            return landList;

        }

        private List<string> GetEvenIndexData(List<string> dataList)
        {
            return dataList.Where(w => dataList.IndexOf(w) % 2 == 0).ToList();

        }

        private List<string> GetOddIndexData(List<string> dataList)
        {
            return dataList.Where(w => dataList.IndexOf(w) % 2 != 0).ToList();

        }

        private PattaType isAllmemberBreakDataValid(List<string> memberData, string totalData)
        {

            if (isValidTotalRecord(totalData) == false)
                return PattaType.TotalRecordIssue;

            var evenData = GetEvenIndexData(memberData);
            var oddData = GetOddIndexData(memberData);

            if (evenData.All(e => e.Contains('-') == true) &&
                    evenData.All(e => e.Split('-').Count() == 5) &&
                    oddData.All(o => o.Contains('-') == false) &&
                    oddData.All(o => o.Split('-').Count() == 1))
                return PattaType.Valid;
            else
                return PattaType.KnownError;
        }

        private PattaType isAllmemberDataValid(List<string> memberData, string totalData)
        {
            if (isValidTotalRecord(totalData) == false)
                return PattaType.TotalRecordIssue;
            if (memberData.All(a => a.Split('-').Count() == 5))
                return PattaType.Valid;
            else if (memberData.All(a => a.Split('-').Count() == 6))
                return PattaType.ValidAndNoSubdivision;
            if (memberData.All(a => a.Split('-').Count() == 5 || a.Split('-').Count() == 6))
                return PattaType.ValidAndPartialSubdivision;
            else
                return PattaType.KnownError;
        }

        private bool isValidTotalRecord(string total)
        {
            return total.Split('-').Count() == 4;
        }

        private (bool status, List<string> bk, List<string> nobk) IsPartialBreak(List<string> memberData)
        {
            var isPrevBkrec = false;
            var brkData = new List<string>();
            var nonBkData = new List<string>();
            var done = false;

            try
            {
                for (int mi = 0; mi <= memberData.Count; mi++)
                {
                    if (memberData.Count < (mi + 1)) break; // last rec is break record!. what about prev is break rec?

                    var isLastRecord = memberData.Count == (mi + 1); // last record

                    if (isLastRecord)
                    {
                        //if (!isPrevBkrec)
                        nonBkData.Add(memberData[mi]);

                        done = true;
                        break;
                    }

                    if (memberData[mi].Contains('-') && !memberData[mi + 1].Contains('-'))
                    {
                        brkData.Add(memberData[mi]);
                        brkData.Add(memberData[mi + 1]);
                        mi += 1; // very imporatant spot
                        isPrevBkrec = true;
                    }
                    else if (memberData[mi].Contains('-') && memberData[mi + 1].Contains('-'))
                    {
                        nonBkData.Add(memberData[mi]);
                        isPrevBkrec = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return (done, brkData, nonBkData); ;
            }

            return (done, brkData, nonBkData);

        }

        public bool IsSomeDots(List<string> memberData)
        {
            //bool done = false;
            if (memberData.Any(a => a.Contains("ே") || a.Contains("\0")))
            {
                for (int md = 0; md <= memberData.Count - 1; md++)
                {
                    memberData[md] = memberData[md].Replace("ே", "*").Replace("\0", "*").Trim();
                    if (memberData[md].Replace(" ", "").ToList().All(a => a == '*'))
                    {
                        memberData.RemoveAt(md);
                        md -= 1;
                    }
                    else if (memberData[md].Contains("*"))
                    {
                        memberData[md] = memberData[md].Replace("*", "").Trim();
                    }
                }
                return true;
            }

            return false;
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

        private string GetSumThreeDotNo(List<string> nos)
        {
            try
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

                return $"0.{finalData}";

            }
            catch (Exception)
            {
                return "Error";
            }

        }

        private void ddlListType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = ((KeyValue)ddlListType.SelectedItem).Id;

            if (selected == 1)
                dataGridView1.DataSource = pattaList;
            else if (selected == 2)
                dataGridView1.DataSource = WholeLandList;
            else if (selected == 3)
            {
                dataGridView1.DataSource = AdangalList;
                //EnableReady();

            }
            else if (selected == 4)
            {
                dataGridView1.DataSource = fullAdangalFromjson;
                EnableReady();
            }
        }

        private void ddlPattaTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = ddlListType.SelectedValue.ToInt32();

            if (selected == 1)
                dataGridView1.DataSource = pattaList.Where(w => (int)w.PattaType == ddlPattaTypes.SelectedValue.ToInt32()).ToList();
            else if (selected == 2)
                dataGridView1.DataSource = pattaList.Where(w => (int)w.PattaType == ddlPattaTypes.SelectedValue.ToInt32()).ToList()
                                            .SelectMany(x => x.landDetails.Select(y => y)).ToList();
        }

        private void ddlLandTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fullAdangalFromjson != null)
            {
                if (((KeyValue)ddlLandTypes.SelectedItem).Id == -1)
                    dataGridView1.DataSource = fullAdangalFromjson.OrderBy(o => o.NilaAlavaiEn).ToList();
                else
                    dataGridView1.DataSource = fullAdangalFromjson.Where(w => (int)w.LandType == ddlLandTypes.SelectedValue.ToInt32()).OrderBy(o => o.NilaAlavaiEn).ToList();
            }
        }

        private string GetLeftEmptyPage()
        {
            // LeftEmptyPage
            var leftPage = FileContentReader.LeftPageTableTemplate;
            var rowTemplate = FileContentReader.LeftPageRowTemplate;
            var totalRowTemplate = FileContentReader.LeftPageTotalTemplate;

            var sb = new StringBuilder();

            for (int i = 1; i <= recordPerPage; i++)
            {
                sb.Append(rowTemplate.Replace("[pulaen]", empty)
                                      .Replace("[utpirivu]", empty)
                                      .Replace("[parappu]", empty)
                                       .Replace("[theervai]", empty)
                                       .Replace("[pattaen-name]", empty));

            }

            var total = totalRowTemplate.Replace("[moththaparappu]", empty).Replace("[moththatheervai]", empty);

            leftPage = leftPage.Replace("[datarows]", sb.ToString());
            leftPage = leftPage.Replace("[totalrow]", total);
            leftPage = leftPage.Replace("( [landtype] )", empty);

            return leftPage;
        }

        private string GetLeftCertPage()
        {
            var sb = new StringBuilder();
            sb.Append(leftCertEmpty);
            sb.Append(GetRightEmptyPage());
            //pageNumber += 1;
            return sb.ToString();
        }

        private string GetRightEmptyPage()
        {
            var rightPage = FileContentReader.RightPageTableTemplate;
            var rowTemplate = FileContentReader.RightPageRowTemplate;
            var totalRowTemplate = FileContentReader.RightPageTotalTemplate;

            var sb = new StringBuilder();

            for (int i = 1; i <= recordPerPage; i++)
                sb.Append(rowTemplate);

            rightPage = rightPage.Replace("[datarows]", sb.ToString());
            rightPage = rightPage.Replace("[totalrow]", totalRowTemplate);

            pageNumber += 1;
            rightPage = rightPage.Replace("[pageNo]", pageNumber.ToString());

            return rightPage;
        }

        private string GetRightCertPage()
        {
            pageNumber += 1;
            var sb = new StringBuilder();
            sb.Append(leftEmpty);
            sb.Append(rightCertEmpty.Replace("[pageNo]", pageNumber.ToString()));
            return sb.ToString();
        }

        private string GetInitialPages()
        {
            // FIRST PAGE.
            var initialPages = new StringBuilder();
            initialPages.Append(firstPage);

            initialPages.Append(GetEmptyPages(1));
            initialPages.Append(GetLeftCertPage());
            initialPages.Append(GetRightCertPage());
            initialPages.Append(GetRightCertPage());
            initialPages.Append(GetEmptyPages(6));

            return initialPages.ToString();

        }

        private string GetEmptyPages(int pageCount)
        {
            var sb = new StringBuilder();

            for (int i = 1; i <= pageCount; i++)
            {
                sb.Append(leftEmpty);
                sb.Append(GetRightEmptyPage());
                //pageNumber += 1;
            }

            return sb.ToString();

        }

        private string GetPageTotal(List<PageTotal> source, List<PageTotal> destination)
        {
            StringBuilder totalContent = new StringBuilder();

            var landTypeGroup = (from wl in source
                                 where wl.LandType != LandType.Other
                                 group wl by wl.LandType into newGrp
                                 select newGrp).ToList();


            landTypeGroup.ToList().ForEach(fe =>
            {
                var dataToProcess = fe.ToList();

                var pageCount = dataToProcess.Count / pageTotalrecordPerPage;
                if (dataToProcess.Count % pageTotalrecordPerPage > 0) pageCount = pageCount + 1;

                var tableTemplate22 = FileContentReader.PageTotalTableTemplate;
                var rowTemplate22 = FileContentReader.PageTotalRowTemplate;
                var landType = fe.Key.ToName();

                for (int i = 0; i <= pageCount - 1; i++)
                {
                    pageNumber += 1;
                    var tbl = tableTemplate22;
                    var row = rowTemplate22;

                    string dataRows = "";
                    StringBuilder sb = new StringBuilder();

                    var temData = dataToProcess.Skip(i * pageTotalrecordPerPage).Take(pageTotalrecordPerPage).ToList();

                    temData.ForEach(ff =>
                    {
                        dataRows = row.Replace("[pageNo]", ff.PageNo.ToString())
                                              .Replace("[parappu]", ff.ParappuTotal)
                                              .Replace("[theervai]", (ff.TheervaiTotal == 0 ? "" : ff.TheervaiTotal.ToString()));
                        sb.Append(dataRows);
                    });

                    string totalTheervai = "";

                    if (fe.Key != LandType.Porambokku)
                        totalTheervai = temData.Sum(s => s.TheervaiTotal).ToString();

                    var totalRows = row.Replace("[pageNo]", tamilMoththam)
                                              .Replace("[parappu]", GetSumThreeDotNo(temData.Select(s => s.ParappuTotal).ToList()))
                                              .Replace("[theervai]", totalTheervai);

                    // Created a sub list item!
                    destination.Add(new PageTotal()
                    {
                        ParappuTotal = GetSumThreeDotNo(temData.Select(s => s.ParappuTotal).ToList()),
                        TheervaiTotal = temData.Sum(s => s.TheervaiTotal),
                        LandType = fe.Key,
                        PageNo = pageNumber
                    });


                    tbl = tbl.Replace("[datarows]", sb.ToString());
                    tbl = tbl.Replace("[totalrow]", totalRows);
                    tbl = tbl.Replace("[landtype]", landType);
                    tbl = tbl.Replace("[pageNo]", pageNumber.ToString());
                    totalContent.Append(tbl);
                }

            });

            return totalContent.ToString();
        }

        private string GetOverallTotal(List<PageTotal> source)
        {
            StringBuilder totalContent = new StringBuilder();

            var tbl = FileContentReader.PageOverallTotalTableTemplate;
            var row = FileContentReader.PageOverallTotalRowTemplate;
            pageNumber += 1;
            string dataRows = "";
            StringBuilder sb = new StringBuilder();

            source.ForEach(ff =>
            {
                dataRows = row.Replace("[vibaram]", ff.LandType.ToName())
                                      .Replace("[parappu]", ff.ParappuTotal)
                                      .Replace("[theervai]", ff.TheervaiTotal.ToString());
                sb.Append(dataRows);
            });

            var totalRows = row.Replace("[vibaram]", tamilMoththam)
                                      .Replace("[parappu]", GetSumThreeDotNo(source.Select(s => s.ParappuTotal).ToList()))
                                      .Replace("[theervai]", source.Sum(s => s.TheervaiTotal).ToString());

            tbl = tbl.Replace("[datarows]", sb.ToString());
            tbl = tbl.Replace("[totalrow]", totalRows);
            tbl = tbl.Replace("[pageNo]", pageNumber.ToString());
            totalContent.Append(tbl);
            return totalContent.ToString();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            pageNumber = 0;

            DirectoryInfo di = new DirectoryInfo(@"F:\AssemblySimpleApp\NTK_Support\AdangalHtmlTemplates");

            foreach (FileInfo file in di.GetFiles())
                file.Delete();

            StringBuilder allContent = new StringBuilder();
            pageTotalList = new List<PageTotal>();
            pageTotal2List = new List<PageTotal>();
            pageTotal3List = new List<PageTotal>();


            var mainHtml = FileContentReader.MainHtml;
            string initialPages = GetInitialPages();

            mainHtml = mainHtml.Replace("[initialPages]", initialPages);

            var landTypeGroup = (from wl in fullAdangalFromjson
                                 where wl.LandType != LandType.Other
                                 group wl by wl.LandType into newGrp
                                 select newGrp).ToList();

            var rowTemplate22 = FileContentReader.LeftPageRowTemplate;
            var totalTemplate22 = FileContentReader.LeftPageTotalTemplate;
            var tableTemplate22 = FileContentReader.LeftPageTableTemplate;

            landTypeGroup.ForEach(fe =>
            {
                var dataToProcess = fe.ToList();

                var pageCount = dataToProcess.Count / recordPerPage;
                if (dataToProcess.Count % recordPerPage > 0) pageCount = pageCount + 1;
                var landType = fe.Key.ToName();


                // FOR TETSING ONLY
                //int  testingPageNo = 6;
                //pageCount = pageCount >= testingPageNo ? testingPageNo : pageCount;
                // FOR TETSING ONLY

                for (int i = 0; i <= pageCount - 1; i++)
                {
                    var leftPage = tableTemplate22;
                    var rowTemplate = rowTemplate22;
                    var totalTemplate = totalTemplate22;

                    string dataRows = "";
                    StringBuilder sb = new StringBuilder();

                    var temData = dataToProcess.Skip(i * recordPerPage).Take(recordPerPage).ToList();

                    temData.ForEach(ff =>
                    {
                        dataRows = rowTemplate.Replace("[pulaen]", ff.NilaAlavaiEn.ToString())
                                              .Replace("[utpirivu]", ff.UtpirivuEn)
                                              .Replace("[parappu]", ff.Parappu)
                                               .Replace("[theervai]", ff.Theervai)
                                               .Replace("[pattaen-name]", ff.Anupathaarar);

                        sb.Append(dataRows);
                    });

                    // LEFT PAGE ROWS
                    leftPage = leftPage.Replace("[datarows]", sb.ToString());

                    //LEFT PAGE TOTAL
                    var totalparappu = GetSumThreeDotNo(temData.Select(s => s.Parappu).ToList());
                    //var totalTheervai = temData.Sum(s => Convert.ToDecimal(s.Theervai));

                    decimal totalTheervai = 0;
                    if (fe.Key != LandType.Porambokku)
                        totalTheervai = temData.Sum(s => Convert.ToDecimal(s.Theervai));

                    var total = totalTemplate.Replace("[moththaparappu]", totalparappu).Replace("[moththatheervai]", totalTheervai.ToString());


                    leftPage = leftPage.Replace("[totalrow]", total);
                    leftPage = leftPage.Replace("[landtype]", landType);

                    allContent.Append(leftPage);
                    allContent.Append(GetRightEmptyPage()); // right page

                    pageTotalList.Add(new PageTotal()
                    {
                        PageNo = pageNumber,
                        ParappuTotal = totalparappu,
                        TheervaiTotal = totalTheervai,
                        LandType = fe.Key
                    });
                }
            });

            allContent.Append(GetEmptyPages(4));  // add 4 empty pages.

            allContent.Append(GetPageTotal(pageTotalList, pageTotal2List));

            allContent.Append(GetPageTotal(pageTotal2List, pageTotal3List));

            allContent.Append(GetOverallTotal(pageTotal3List));

            mainHtml = mainHtml.Replace("[allPageData]", allContent.ToString());

            File.AppendAllText(@"F:\AssemblySimpleApp\NTK_Support\AdangalHtmlTemplates\All.htm", mainHtml);
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            lblMessage.Text = $"Record Count: {dataGridView1.Rows.Count} ";

        }

        private void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (General.CheckForInternetConnection() == false)
            {
                MessageBox.Show("No Internet Connection!");
                return;
            }

            if (ddlDistrict.SelectedItem != null)
            {
                var selValue = ((ComboData)ddlDistrict.SelectedItem).Value;


                var url = $"https://eservices.tn.gov.in/eservicesnew/land/ajax.html?page=taluk&districtCode={selValue}";

                var response = WebReader.CallHttpWebRequest(url);

                BindDropdown(cmbTaluk, WebReader.xmlToDynamic(response, "taluk"), "Display", "Value");
                cmbTaluk.SelectedIndexChanged += new System.EventHandler(this.cmbTaluk_SelectedIndexChanged);
            }

        }

        private void cmbTaluk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (General.CheckForInternetConnection() == false)
            {
                MessageBox.Show("No Internet Connection!");
                return;
            }
            if (ddlDistrict.SelectedItem != null)
            {
                var disValue = ((ComboData)ddlDistrict.SelectedItem).Value;
                var talValue = ((ComboData)cmbTaluk.SelectedItem).Value;

                var url = $"https://eservices.tn.gov.in/eservicesnew/land/ajax.html?page=village&districtCode={disValue}&&talukCode={talValue}";

                var response = WebReader.CallHttpWebRequest(url);

                BindDropdown(cmbVillages, WebReader.xmlToDynamic(response, "village"), "Display", "Value");
                //cmbVillages.SelectedIndexChanged += new System.EventHandler(this.cmbVillages_SelectedIndexChanged);
            }

        }

        private void vao_Load(object sender, EventArgs e)
        {
            ddlDistrict.SelectedIndexChanged += new System.EventHandler(this.ddlDistrict_SelectedIndexChanged);


        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Gets the expected land details count.

            if (General.CheckForInternetConnection() == false)
            {
                MessageBox.Show("No Internet Connection!");
                return;
            }

            var onlineData = GetLandCount();
            fullAdangalFromjson = DataAccess.GetActiveAdangal(villageName, true);

            var expLandDetails = (onlineData
                                .OrderBy(o => o.Value)
                                .ThenBy(o => o.Caption, new AlphanumericComparer())
                                .Select(s => (s.Value.ToString().Trim() + "~" + s.Caption.Trim()).Trim())).ToList();

            var actualLandDetails = fullAdangalFromjson
                                    .OrderBy(o => o.NilaAlavaiEn)
                                    .ThenBy(o => o.UtpirivuEn, new AlphanumericComparer())
                                    .Select(s =>
                                    (s.NilaAlavaiEn.ToString().Trim() + "~" + s.UtpirivuEn.Trim()).Trim())
                                    .ToList();

            notInPdfToBeAdded = expLandDetails.Except(actualLandDetails).ToList();
            notInOnlineToBeDeleted = actualLandDetails.Except(expLandDetails).ToList();

            if (notInPdfToBeAdded.Count == 0 && notInOnlineToBeDeleted.Count == 0)
            {
                btnStatusCheck.Text = "OK";
                btnStatusCheck.BackColor = Color.Green;
            }
            {
                btnStatusCheck.Text = $"ADD:{notInPdfToBeAdded.Count} {Environment.NewLine} DELETE:{notInOnlineToBeDeleted.Count}";
                btnStatusCheck.BackColor = Color.Red;
            }


            btnDelete.Enabled = (notInOnlineToBeDeleted.Count > 0);
            btnAdd.Enabled = (notInPdfToBeAdded.Count > 0);

            cmbItemToBeAdded.DataSource = notInPdfToBeAdded;


        }

        private List<KeyValue> GetLandCount()
        {
            var fileName = $"{villageName}-subdiv";

            if (DataAccess.IsAdangalExist(fileName) == true)
            {
                return DataAccess.GetSubdiv(fileName);
            }

            var totalLandList = new List<KeyValue>();

            if (cmbVillages.SelectedItem != null)
            {
                var disValue = ((ComboData)ddlDistrict.SelectedItem).Value;
                var talValue = ((ComboData)cmbTaluk.SelectedItem).Value;
                var villageValue = ((ComboData)cmbVillages.SelectedItem).Value.ToString().PadLeft(3, '0');

                var url = "";


                int continueCheckCount = 0;

                for (int i = 1; i <= 200; i++)
                {
                    if (continueCheckCount == 25)
                    {
                        break;
                    };

                    url = $"https://eservices.tn.gov.in/eservicesnew/land/ajax.html?page=getSubdivNo&districtCode={disValue}&talukCode={talValue}&villageCode={villageValue}&surveyno={i}";
                    var response = WebReader.CallHttpWebRequest(url);

                    var ubDivs = WebReader.GetSubdivisions(response, "subdiv");
                    if (ubDivs == null)
                    {
                        continueCheckCount += 1;
                    }
                    else
                    {
                        ubDivs.ForEach(fe =>
                        {
                            totalLandList.Add(new KeyValue(fe, i));
                            continueCheckCount = 0; // reset.
                        });
                    }
                }
            }

            return DataAccess.SubdivToJson(totalLandList, fileName);
            //return totalLandList;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"F:\AUTO-ADANGAL";

            string folderPath = "";
            if (DialogResult.OK == fbd.ShowDialog())
            {
                folderPath = fbd.SelectedPath;
            }

            if (haveValidFiles(folderPath))
            {
                pattaList = new PattaList();
                relationTypes = new List<string>() {
                "தந்தைத",
                "கணவன",
                "காப்பாளர்",
                "மைகன",
                "மைைனவி"
            };

                relationTypesCorrect = new List<string>() {
                "தந்தை",
                "கணவன்",
                "காப்பாளர்",
                "மகன்",
                "மனைவி"
            };


                chittaFile = Path.Combine(folderPath, "Chitta_Report-1.pdf");
                aRegFile = Path.Combine(folderPath, "Areg_Report-1.pdf");
                chittaContent = chittaFile.GetPdfContent();
                aRegContent = aRegFile.GetPdfContent();

                firstPage = FileContentReader.FirstPageTemplate;
                leftEmpty = GetLeftEmptyPage();
                leftCertEmpty = FileContentReader.LeftPageCertTableTemplate;
                rightCertEmpty = FileContentReader.RightPageTableCertTemplate;

                //ProcessNames();
                ProcessChittaFile();    // Nansai, Pun, Maa,
                ProcessAreg();  // Puram

                ProcessFullReport();

            }
            else
            {
                MessageBox.Show("Some file missing in the folder?");

            }


        }

        private bool haveValidFiles(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);

            var filesCount = files.Count();

            if (filesCount != 4)
            {
                MessageBox.Show("Some file missing!");
                return false;
            }

            var pdffilesCount = files.Where(w => w.EndsWith(".pdf")).Count();
            var txtfilesCount = files.Where(w => w.EndsWith(".txt")).Count();

            if (pdffilesCount != 2 || txtfilesCount != 2)
            {
                MessageBox.Show("Some txt or pdf file missing!");
                return false;
            }


            return true;
        }

        private void cmbVillages_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            EnableReady();
        }

        void EnableReady()
        {


            if ((((ComboData)cmbVillages.SelectedItem).Value != -1) && ddlListType.SelectedValue.ToInt32() == 4)
            {
                btnReady.Enabled = btnGenerate.Enabled = true;
                BindDropdown(cmbFulfilled, GetFullfilledOptions(), "Caption", "Id");
            }
            else
            {
                btnReady.Enabled = btnGenerate.Enabled = false;
            }

        }

        private void cmbFulfilled_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selValue = ((KeyValue)cmbFulfilled.SelectedItem).Id;

            if (selValue == -1) return;

            if (selValue == 1)
            {
                dataGridView1.DataSource = fullAdangalFromjson.Where(w => string.IsNullOrEmpty(w.UtpirivuEn) || w.UtpirivuEn == "-").ToList();
            }
            else if (selValue == 2)
            {
                dataGridView1.DataSource = fullAdangalFromjson.Where(w => !string.IsNullOrEmpty(w.UtpirivuEn) && w.UtpirivuEn != "-").ToList();
            }

            else if (selValue == 3)
            {
                dataGridView1.DataSource = fullAdangalFromjson.Where(w => w.UtpirivuEn.Contains("ே") || w.UtpirivuEn.Contains("\0")).ToList();
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            fullAdangalFromjson = DataAccess.SetDeleteFlag(villageName, notInOnlineToBeDeleted);
            dataGridView1.DataSource = fullAdangalFromjson;
        }

        private void EditCancel()
        {
            dataGridView1.CurrentCell.Style.BackColor = Color.Red;
            dataGridView1.CurrentCell.Style.ForeColor = Color.Yellow;
        }

        bool IsEnterKey = false;

        private void EditSuccess()
        {
            dataGridView1.CurrentCell.Style.BackColor = Color.LightGreen;
            dataGridView1.CurrentCell.Style.ForeColor = Color.White;
            this.dataGridView1.ClearSelection();
            IsEnterKey = false; // reset flag after ecery success edit.
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ddlListType.SelectedIndex != 3) return;

            if (IsEnterKey == false)
            {
                EditCancel();
                return;
            }

            int existingTxnId = 0; // to  keep existing txn id.
            DataGridView grid = (sender as DataGridView);
            int rowIndex = grid.CurrentCell.RowIndex;
            string owningColumnName = grid.CurrentCell.OwningColumn.Name;
            string cellValue = GetGridCellValue(grid, rowIndex, owningColumnName);
            Adangal cus = grid.Rows[grid.CurrentCell.RowIndex].DataBoundItem as Adangal;

            if (string.IsNullOrEmpty(cellValue))
            {
                EditCancel();
                return;
            }
        }

        public static string GetGridCellValue(DataGridView grid, int rowIndex, string columnName)
        {
            var cellValue = Convert.ToString(grid.Rows[grid.CurrentCell.RowIndex].Cells[columnName].Value);
            return (cellValue == string.Empty) ? null : cellValue;
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            IsEnterKey = (keyData == Keys.Enter);
            return false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtAddNewSurvey.Enabled = btnAddNewSurvey.Enabled = cmbItemToBeAdded.Enabled =  true;
            

        }

        private void btnAddNewSurvey_Click(object sender, EventArgs e)
        {
            var newData = txtAddNewSurvey.Text;

            var splitData = newData.Replace("உரிமையாளர்கள் பெயர்", "$").Split('$');
            var pattaEn = splitData[0].Replace("பட்டா எண்", "$").Split('$').ToList()[1].Replace(":", "").Trim();

            var first = splitData[1];

            var sec = first.Replace("குறிப்பு2", "$").Split('$')[0];
            var third = sec.Split(Environment.NewLine.ToCharArray()).Where(w => w.Trim() != "").ToList();

            var hecIndex = third.FindIndex(f => f.Contains("ஹெக்"));
            var totalIndex = third.Count - 1;

            var neededData = third.Skip(hecIndex + 1);
            neededData = neededData.Take(neededData.Count() - 1);

            var name = GetOwnerName(sec.Replace("புல எண்", "$").Split('$').ToList()[0]);

            var surveysubdiv = cmbItemToBeAdded.SelectedItem.ToString().Split('~').ToList();

            neededData.ToList().ForEach(fe => {

                var rowData = fe.Split('\t').ToList();

                if (rowData[0] == surveysubdiv[0] && rowData[1] == surveysubdiv[1])
                {
                    var adangal = GetAdangalFromCopiedData(rowData,  pattaEn, name);

                    if (MessageBox.Show(adangal.ToString(), "சரியா?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DataAccess.AddNewAdangal(villageName, adangal);
                        cmbItemToBeAdded.SelectedIndex += 1;
                    }

                }


            });

        }

        private string GetOwnerName(string nameRow)
        {
            nameRow = nameRow.Split(' ').ToList().Where(w => w.Trim() != "").ToList()[1];
            string name = "";
            if (relationTypesCorrect.Any(a => nameRow.Split('\t').ToList().Contains(a))) // have valid names.
            {
                var delitList = relationTypesCorrect.Intersect(nameRow.Split('\t').ToList()).ToList();

                if (delitList.Count == 1)
                {
                    var delimit = delitList[0];
                    name = nameRow.Replace(delimit, "$").Split('$')[1];
                    var d = nameRow.Replace(delimit, "$").Split('$');
                    name = $"{d[1]}";
                }
                else
                {
                    // ERROR!
                    MessageBox.Show("Error!");
                }
            }

            return name.Trim().Replace("\t", "").Replace("-", "");
        }


        private Adangal GetAdangalFromCopiedData(List<string> data, string pattaEn, string ownerName)
        {
            var adangal = new Adangal();

            // Test for both fullfilled and extend also
            adangal.NilaAlavaiEn = data[0].ToInt32();
            adangal.UtpirivuEn = data[1];
            var ld = GetLandDetails(data);
            adangal.Parappu = ld.par.Trim().Replace(" ", "").Replace("-", ".");
            adangal.Theervai = ld.thee;
            adangal.LandType = ld.lt;
            adangal.Anupathaarar = $"{pattaEn}-{ownerName}";
            adangal.LandStatus = LandStatus.Added;

            return adangal;
        }

        private (LandType lt, string par, string thee) GetLandDetails(List<string> data)
        {
            int i = 0;
            LandType ld = LandType.Other;
            var parappu = "";
            var theervai = "";

            if (data[2] != "--" && data[3] != "--")
            {
                ld = LandType.Punsai;
                parappu = data[2];
                theervai = data[3];
                i += 1;
            }
            if (data[4] != "--" && data[5] != "--")
            {
                ld = LandType.Nansai;
                parappu = data[4];
                theervai = data[5];
                i += 1;
            }
            if (data[6] != "--" && data[7] != "--")
            {
                ld = LandType.Maanaavari;
                parappu = data[6];
                theervai = data[7];
                i += 1;
            }

            if(i > 1)
            {
                ld = LandType.Other;
            }

            return (ld, parappu, theervai);
        }
        
    }
}