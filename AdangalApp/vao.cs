using AdangalApp.AdangalTypes;
using Common;
using Common.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


namespace AdangalApp
{
    public partial class vao : Form
    {
        readonly int recordPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["PageRecordCount"]);
        readonly int pageTotalrecordPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["pageTotalrecordPerPage"]);
        readonly string empty = "";
        readonly string tamilMoththam = "மொத்தம்";
        readonly string kiraamaMoththam = "கிராம மொத்தம்";
        readonly string ERROR = "ERROR";

        string pdfvillageName = "";


        string chittaPdfFile = "";
        string chittaTxtFile = "";
        string aRegFile = "";
        string chittaContent = "";
        string aRegContent = "";

        int pageNumber = 0;
        bool IsEnterKey = false;

        List<LandDetail> WholeLandList;
        List<Adangal> AdangalOriginalList;
        List<Adangal> PurambokkuAdangalList;
        List<Adangal> fullAdangalFromjson;
        List<string> relationTypes;
        List<string> relationTypesCorrect;
        PattaList pattaList;
        Patta pattaSingle;
        public static LogHelper logHelper;
        //LogHelper CommonlogHelper;

        string firstPage;
        string plainPage;
        string certSinglePage;
        string notesPage;
        string leftEmpty;
        string leftCertEmpty;
        string rightCertEmpty;

        List<PageTotal> pageTotalList = null;
        List<PageTotal> pageTotal2List = null;
        List<PageTotal> pageTotal3List = null;
        public static LoadedFileDetail loadedFile = new LoadedFileDetail();

        List<string> notInPdfToBeAdded;
        List<string> notInOnlineToBeDeleted;
        string reqFileFolderPath = "";
        string updatedHeader = "";

        string title = ConfigurationManager.AppSettings["title"];
        string header = ConfigurationManager.AppSettings["header"];
        bool isTestingMode = Convert.ToBoolean(ConfigurationManager.AppSettings["isTesting"]);
        int TestingPage = ConfigurationManager.AppSettings["TestingPage"].ToInt32();
        string ChittaPdfFile = ConfigurationManager.AppSettings["ChittaPdfFile"];
        string ChittaTxtFile = ConfigurationManager.AppSettings["ChittaTxtFile"];
        string AregFile = ConfigurationManager.AppSettings["AregFile"];
        bool needTheervaiTest = Convert.ToBoolean(ConfigurationManager.AppSettings["needTheervaiTest"]);
        bool canAddMissedSurvey = Convert.ToBoolean(ConfigurationManager.AppSettings["canAddMissedSurvey"]);
        bool pcEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["pc"]);
        int pasali = Convert.ToInt32(ConfigurationManager.AppSettings["PasaliEn"]);

        int prepasali;

        List<KeyValue> wrongName = new List<KeyValue>();
        List<KeyValue> correctName = new List<KeyValue>();
        List<string> pdfPattaNo = new List<string>();
        List<string> txtPattaNo = new List<string>();

        public vao()
        {
            try
            {
                

                InitializeComponent();
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                SetTestMode();
                txtRecCount.Text = recordPerPage.ToString();
                BindDropdown(cmbFulfilled, GetFullfilledOptions(), "Caption", "Id");
                prepasali = (pasali - 1);
                dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.Yellow;                

                try
                {
                    BindDropdown(ddlDistrict, DataAccess.GetDistricts(), "Display", "Value");
                    var processedFiles = DataAccess.GetLoadedFileDetails();
                    if (processedFiles.Count > 0)
                        ddlProcessedFiles.DataSource = processedFiles;

                    //BindDropdown(ddlProcessedFiles, processedFiles, "VillageName", "VillageCode");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                LogMessage($"================={DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")}========================");
                LogMessage("STARTED....");
                relationTypes = new List<string>() {
                        "தந்தைத",
                        "கணவன",
                        "காப்பாளர்",
                        "மைகன",
                        "மைைனவி",
                        //2
                        "கணவன்ச",
                        "மைகன்ச",
                        "தாய"
                    };
                relationTypesCorrect = new List<string>() {
                        "தந்தை",
                        "கணவன்",
                        "காப்பாளர்",
                        "மகன்",
                        "மனைவி"
                    };

            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }





        private void BindDropdown(ComboBox cb, object dataSource, string DisplayMember, string ValueMember)
        {
            try
            {
                cb.DisplayMember = DisplayMember;
                cb.ValueMember = ValueMember;
                cb.DataSource = dataSource;
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }



        private void ProcessNames()
        {
            try
            {
                var namesContent = File.ReadAllText(chittaTxtFile).Split(Environment.NewLine.ToCharArray());

                var filteredContent = namesContent.Where(w =>
                                            w.Contains("digitally") == false &&
                                            w.Contains("Page") == false &&
                                            w.Contains("Taluk") == false &&
                                            w.Contains("District") == false &&
                                            w.Contains("| | | ற றவு") == false &&
                                            w.Contains(pdfvillageName) == false &&
                                            w.Trim() != empty).ToList();

                List<KeyValue> nameAndPatta = new List<KeyValue>();

                int processedRow = 0;

                for (int i = 0; i <= filteredContent.Count - 1; i++)
                {
                    try
                    {
                        if (filteredContent[i].Contains("உறவினர்‌ பெயர்"))
                        {
                            var pattaENRow = pdfPattaNo[processedRow];
                            var nameRow = filteredContent[i + 1];
                            //var nm = relationTypesCorrect.Intersect(nameRow.Split('|').ToList());
                            var lst = nameRow.Split('|').ToList().Where(w => w.Trim() != "").ToList();
                            txtPattaNo.Add(pattaENRow.Split(' ').Last());
                            correctName.Add(new KeyValue(nameRow, pattaENRow.ToInt32()));
                            processedRow += 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError($"Error during process names @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
                        continue;
                    }
                }

                if (processedRow == 0)
                {
                    MessageBox.Show("File Corrupted!");
                    LogMessage("File Corrupted!");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }


        }
        private void ProcessAreg()
        {
            try
            {
                LogMessage($"STARTED PROCESSING AREG PDF FILE @ {DateTime.Now.ToLongTimeString()}");
                aRegContent = aRegFile.GetPdfContent();

                PurambokkuAdangalList = new List<Adangal>();

                //var names = GetTestPoramNames();

                var aregPatta = aRegContent.Split(Environment.NewLine.ToCharArray()).Where(w => w.Contains("புறமேபாககு")).ToList();
                int randomNo = 0;
                foreach (var item in aregPatta)
                {

                    List<string> d = null;
                    try
                    {
                        d = item.Split(' ').Where(w => w.Trim() != "").ToList();
                        var pointedIndex = d.LastIndexOf("0");
                        var parappu = $"{d[pointedIndex - 4]}.{d[pointedIndex - 3]}";

                        if (randomNo >= 9)
                            randomNo = 0;
                        PurambokkuAdangalList.Add(new Adangal()
                        {
                            NilaAlavaiEn = d[0].ToInt32(),
                            UtpirivuEn = d[1],
                            OwnerName = d.Last(), //names[randomNo],
                            Parappu = parappu,
                            LandType = LandType.Porambokku
                        });
                        randomNo += 1;

                    }
                    catch (Exception ex)
                    {
                        if (d.Count >= 2)
                        {
                            PurambokkuAdangalList.Add(new Adangal()
                            {
                                NilaAlavaiEn = d[0].ToInt32(),
                                UtpirivuEn = d[1],
                                LandType = LandType.PorambokkuError,
                                //CorrectNameRow = d.ListToString("%")
                            });
                            LogMessage($"Error Processing Purambokku record @ {d[0].ToInt32()} - {ex.ToString()}");
                        }
                        else
                        {
                            PurambokkuAdangalList.Add(new Adangal()
                            {
                                NilaAlavaiEn = 0,
                                UtpirivuEn = "$",
                                LandType = LandType.PorambokkuError
                            });
                            LogMessage($"Big Error Processing Purambokku record @ {d.Count()} items - {ex.ToString()}");
                        }

                    }
                }

                AdangalOriginalList.AddRange(PurambokkuAdangalList);
                LogMessage($"COMPLETED PROCESSING AREG PDF FILE @ {DateTime.Now.ToLongTimeString()}");
                fullAdangalFromjson = DataAccess.AdangalToJson(AdangalOriginalList);
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }

        }

        private void LoadPdfPattaNo()
        {
            LogMessage($"READING DATA FROM CHITTA PDF fILE - {chittaPdfFile}");

            var pattaas = chittaContent.Replace("பட்டா எண்    :", "$"); //("பட்டா எண்", "$");
            var data = pattaas.Split('$').ToList();

            data.RemoveAt(0); // first is empty data
            List<ChittaData> cds = new List<ChittaData>();

            LogMessage($"STARTED PATTA NO FROM PDF via CHITTA PDF FILE @ {DateTime.Now.ToLongTimeString()}");
            for (int i = 0; i <= data.Count - 1; i++)
            {
                List<string> fullData = null;

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

                var pattaNO = fullData.First().Trim(); //.Replace(":", "").Trim();
                var isNo = pattaNO.isNumber();

                if (isNo == false)
                    continue;

                pdfPattaNo.Add(pattaNO);
            }
        }

        private void ProcessChittaFile()
        {
            try
            {
                LogMessage($"READING DATA FROM CHITTA PDF fILE - {chittaPdfFile}");

                var pattaas = chittaContent.Replace("பட்டா எண்    :", "$"); //("பட்டா எண்", "$");
                var data = pattaas.Split('$').ToList();
                pdfvillageName = data.First().Split(':')[3].Trim();
                SetVillage();

                //waitForm.Close();

                //waitForm.Show(this);

                data.RemoveAt(0); // first is empty data

                List<ChittaData> cds = new List<ChittaData>();
                bool isFullBreakData, isPartialBreakData = false;
                List<string> brkData;
                List<string> nonBkData;
                pattaList = new PattaList();

                LogMessage($"STARTED PROCESSING CHITTA PDF FILE @ {DateTime.Now.ToLongTimeString()}");

                //var nameList = GetTestNames();

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

                        var pattaNO = fullData.First().Trim(); //.Replace(":", "").Trim();
                        var isNo = pattaNO.isNumber();

                        #endregion

                        #region "Identify PattaType"

                        if (isNo == false)
                        {
                            pattaList.AddAndUpdatePattaAndOwnerNameinList(pattaSingle, PattaType.InValidPatta, fullData);
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
                            pattaList.AddAndUpdatePattaAndOwnerNameinList(pattaSingle, PattaType.Zero, fullData);
                            continue;
                        }

                        else if (totalRecord == memberData.Count)
                        {
                            var pt = isAllmemberDataValid(memberData, totalData);
                            if (pt == PattaType.KnownError || pt == PattaType.TotalRecordIssue)
                            {
                                pattaList.AddAndUpdatePattaAndOwnerNameinList(pattaSingle, pt, fullData);
                                continue;
                            }
                            pattaSingle.PattaType = pt;
                        }

                        else if ((totalRecord * 2) == memberData.Count)
                        {
                            var pt = isAllmemberBreakDataValid(memberData, totalData);
                            if (pt == PattaType.KnownError || pt == PattaType.TotalRecordIssue)
                            {
                                pattaList.AddAndUpdatePattaAndOwnerNameinList(pattaSingle, pt, fullData);
                                continue;
                            }
                            pattaSingle.PattaType = pt;
                            isFullBreakData = pt == PattaType.Valid; // if (pt == PattaType.Valid) 
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
                                pattaList.AddAndUpdatePattaAndOwnerNameinList(pattaSingle, PattaType.Unknown, fullData);
                                continue;
                            }
                        }

                        #endregion

                        #region "Process Owner Name"

                        // Gets the name
                        pattaSingle.isVagai = (headerData.Count > 3);

                        var nameRow = headerData[1];

                        //wrongName.Add(new KeyValue(nameRow, pattaSingle.PattaEn));

                        if (relationTypes.Any(a => nameRow.Split(' ').ToList().Contains(a))) // have valid names.
                        {
                            var delitList = relationTypes.Intersect(nameRow.Split(' ').ToList()).ToList();

                            if (delitList.Count == 1)
                            {
                                var delimit = delitList[0];
                                //pattaSingle.PattaTharar = nameRow.Replace(delimit, "$").Split('$')[1];
                                var d = nameRow.Replace(delimit, "$").Split('$');
                                //pattaSingle.PattaTharar = ApplyUnicode(d[1]);
                                //pattaSingle.PattaTharar = $"{d[1]} {delimit} {d[0]}";
                                var fn = d[1];
                                var ln = d[0];
                                var correctNameRow = correctName.Where(w => w.Value == pattaSingle.PattaEn).First().Caption;

                                var cn = ExtractCorrectName(d[1], d[0], correctNameRow);

                                Debug.WriteLine($"correctname [{pattaSingle.PattaEn}] : {correctNameRow}");
                                Debug.WriteLine($"wrongname : {ln} - {fn}");
                                Debug.WriteLine($"correctName : {cn.Caption} - {cn.Caption2}");
                                Debug.WriteLine($"-----------------------------------------");

                                pattaSingle.PattaTharar = $"{cn.Caption}";

                                if (cn.Caption == ERROR) // || cn.Caption2 == ERROR
                                {
                                    pattaSingle.PattaType = PattaType.NameIssue;
                                    pattaSingle.PattaTharar = fn;

                                }

                                pattaSingle.NameRow = correctNameRow.Replace("|", "");
                                //var randomNo = new Random().Next(0, 99);
                                //pattaSingle.PattaTharar = $"{nameList[randomNo]}";
                            }
                            else
                            {
                                // ERROR!
                                pattaList.AddAndUpdatePattaAndOwnerNameinList(pattaSingle, PattaType.TwoNameDelimit, fullData);
                                continue;
                            }
                        }
                        else
                        {
                            // ERROR!
                            pattaList.AddAndUpdatePattaAndOwnerNameinList(pattaSingle, PattaType.NameIssue, fullData);
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

                        pattaList.AddAndUpdatePattaAndOwnerNameinList(pattaSingle, pattaSingle.PattaType, fullData);

                    }
                    catch (Exception ex)
                    {
                        if (pattaSingle.PattaType == PattaType.Valid)
                            pattaList.AddAndUpdatePattaAndOwnerNameinList(pattaSingle, PattaType.ValidException, fullData);
                        else
                            pattaList.AddAndUpdatePattaAndOwnerNameinList(pattaSingle, PattaType.Exception, fullData);

                        LogMessage($"ERROR WHILE PROCESS patta - {pattaSingle.PattaEn} - {ex.ToString()}");
                        continue;
                    }
                }
                LogMessage($"COMPLETED PROCESSING CHITTA PDF FILE @ {DateTime.Now.ToLongTimeString()}");

                WholeLandList = pattaList.SelectMany(x => x.landDetails.Select(y => y)).ToList();



                AdangalOriginalList = (from wl in WholeLandList
                                   .Where(w => w.LandType != LandType.Zero)
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
                                           //Anupathaarar = wl.Anupathaarar,
                                           LandType = wl.LandType,
                                           LandStatus = wl.LandStatus,
                                           PattaEn = wl.PattaEn,
                                           //CorrectNameRow = wl.CorrectNameRow
                                       }).ToList();
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }

        }

        private KeyValue ExtractCorrectName(string wrongFirstName, string wrongLastName, string correctNameRow)
        {

            var haveFirstName = correctNameRow.Contains(wrongFirstName.Replace("1", "").Replace("|", "").Trim());
            var haveLastName = correctNameRow.Contains(wrongLastName.Replace("1", "").Replace("|", "").Trim());

            var cnList = correctNameRow.Split(' ').Where(w => w.Trim() != empty && w.Trim() != "|" && w.Trim() != "1").ToList();
            var wFnList = wrongFirstName.Split(' ').Where(w => w.Trim() != empty && w.Trim() != "1").ToList();
            var wLnList = wrongLastName.Split(' ').Where(w => w.Trim() != empty && w.Trim() != "1").ToList();

            var matchedFirstName = MaxMatch(cnList, wFnList);
            var matchedLastName = MaxMatch(cnList, wLnList);

            return new KeyValue() { Caption = matchedFirstName, Caption2 = matchedLastName };

        }

        private string MaxMatch(List<string> cnList, List<string> wFnList)
        {
            int flagCount = 0;
            string mathedText = "";
            int stopFlag = 0;

            for (int i = 0; i <= cnList.Count - 1; i++)
            {
                if (stopFlag == 1) break;
                flagCount = 0;

                var wrongArrText = wFnList[0].ToCharArray().Where(w => w != '‌').ToList();
                var correctArrText = cnList[i].ToCharArray().Where(w => w != '‌').ToList();

                var wrongArr = wrongArrText.Select(s => (int)s).ToList();
                var correctArr = correctArrText.Select(s => (int)s).ToList();
                int loopCout = Math.Min(wrongArr.Count, correctArr.Count);

                var isSame = (wFnList[0].Trim() == cnList[i].Trim());
                int perc = 0;

                if ((wrongArr.Count - correctArr.Count <= 2) && isSame == false)
                {
                    for (int ci = 0; ci <= loopCout - 1; ci++)
                    {
                        if (correctArr.Contains(wrongArr[ci]))
                            flagCount += 1;
                    }
                    perc = correctArr.Count.PercentageBtwIntNo(flagCount);
                }

                if (perc >= 60 || isSame)
                {
                    mathedText = cnList[i].ToString();
                    if (wFnList.Count > 1)
                        mathedText += $" {cnList[i + 1].ToString()}";
                    stopFlag = 1;
                }
            }
            return stopFlag == 1 ? mathedText.Trim() : ERROR;
        }

        private List<string> GetTestNames()
        {
            return File.ReadAllLines(@"F:\AssemblySimpleApp\AdangalApp\samplename.txt").ToList();
        }

        private List<string> GetTestPoramNames()
        {
            return File.ReadAllLines(@"F:\AssemblySimpleApp\AdangalApp\samplenamePorambokku.txt").ToList();
        }
        private void ProcessFullReport()
        {
            try
            {
                LogMessage($"STARTED FULL REPORT");
                // Full Report
                FinalReport fr = new FinalReport(pattaList);
                BindDropdown(ddlPattaTypes, fr.CountData, "DisplayMember", "Id");
                BindDropdown(ddlLandTypes, GetLandTypes(), "DisplayMember", "Id");
                BindDropdown(ddlListType, GetListTypes(), "Caption", "Id");
                BindDropdown(cmbLandStatus, GetLandStatusOptions(), "DisplayMember", "Id");

                LoadSurveyAndSubdiv();
                btnReady.Enabled = cmbFulfilled.Enabled = chkEdit.Enabled = true;
                CalculateTotalPages(recordPerPage);

                if (fr.IsFullProcessed == false)
                {
                    MessageBox.Show($"{fr.NotProcessedData} not processes");
                    LogMessage($"{fr.NotProcessedData} not processes");
                }

                LogMessage($"COMPLETED FULL REPORT");
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }
        private List<KeyValue> GetListTypes()
        {
            return new List<KeyValue> {
                new KeyValue() { Id = 4, Caption = "JSON-ADANGAL" },
                new KeyValue() { Id = 1, Caption = "PATTA" },
                new KeyValue() { Id = 2, Caption = "LANDDETAIL" },
                new KeyValue() { Id = 3, Caption = "ADANGAL" },

            };

        }
        private List<KeyValue> GetFullfilledOptions()
        {
            return new List<KeyValue> {
                new KeyValue() { Id = -1, Caption = "--select--" },
                new KeyValue() { Id = 1, Caption = "Fullfilled" },
                new KeyValue() { Id = 2, Caption = "Extend" },
            };

        }

        private List<KeyValue> GetLandStatusOptions()
        {
            var landStatusSource = new List<KeyValue>();
            landStatusSource.Add(new KeyValue() { Caption = "--select--", Id = -1 });

            foreach (LandStatus ls in Enum.GetValues(typeof(LandStatus)))
            {
                landStatusSource.Add(new KeyValue()
                {
                    Caption = Enum.GetName(typeof(LandStatus), ls),
                    Value = fullAdangalFromjson.Where(w => w.LandStatus == ls).Count(),
                    Id = (int)ls
                });
            }

            landStatusSource.Add(new KeyValue()
            {
                Caption = "Issue",
                Value = fullAdangalFromjson.Where(w => w.LandType != LandType.Porambokku && w.PattaEn == 0).Count(),
                Id = 101
            });

            //var cd = fullAdangalFromjson.Where(w => w.LandType != LandType.Porambokku && w.PattaEn == 0).ToList();

            //cd.ForEach(fee => {

            //    DataAccess.DeleteAdangalFile(fee);end
            //    });

            //MessageBox.Show("Deleted!");

            //landStatusSource.Add(new KeyValue() { Id = 100, Caption = "SomeDots" });
            return landStatusSource;
        }

        private List<KeyValue> GetLandTypes()
        {
            var landTypeSource = new List<KeyValue>();
            landTypeSource.Add(new KeyValue() { Caption = "--select--", Id = -2 });
            landTypeSource.Add(new KeyValue() { Caption = "ALL", Id = -1, Value = fullAdangalFromjson.Count });

            foreach (LandType rt in Enum.GetValues(typeof(LandType)))
            {
                landTypeSource.Add(new KeyValue()
                {
                    Caption = Enum.GetName(typeof(LandType), rt),
                    Value = fullAdangalFromjson.Where(w => w.LandType == rt).Count(),
                    Id = (int)rt
                });
            }

            return landTypeSource;
        }
        private List<LandDetail> ProcessLandType(List<string> actualData, List<string> breakData = null)
        {
            var landList = new List<LandDetail>();
            try
            {
                LandDetail land = null;
                for (int index = 0; index <= actualData.Count - 1; index++)
                {
                    land = new LandDetail();

                    var ad = actualData[index].Split('-').Where(w => w.Trim() != "").Select(s => s.Trim() + "-").ToList();

                    if (breakData != null && breakData.Count > 0)
                    {
                        land.PulaEn = ad[0].Split(' ')[1] + breakData[index];
                        land.NansaiParappu = ad[1] + ad[2].Split(' ')[0];
                    }
                    else
                    {

                        if (ad[1].Split(' ').Count() == 2)
                        {
                            land.PulaEn = ad[0].Split(' ')[1] + ad[1].Split(' ')[0];
                            land.NansaiParappu = ad[1].Split(' ')[1] + ad[2].Split(' ')[0];
                        }
                        else if (ad[1].Split(' ').Count() == 1)
                        {
                            land.PulaEn = ad[0].Split(' ')[1];
                            land.NansaiParappu = ad[1].Split(' ')[0] + ad[2].Split(' ')[0];
                        }
                        else
                            throw new Exception();
                    }
                    land.NansaiTheervai = ad[2].Split(' ')[1];

                    land.PunsaiParappu = ad[2].Split(' ')[2] + ad[3].Split(' ')[0];
                    land.PunsaiTheervai = ad[3].Split(' ')[1];

                    land.MaanavariParappu = ad[3].Split(' ')[2] + ad[4].Split(' ')[0];
                    land.MaanavariTheervai = ad[4].Split(' ')[1].Replace("-", "");


                    landList.Add(land);
                }
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
                throw ex;
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
            try
            {
                if (IsValidTotalRecord(totalData) == false)
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
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
                return PattaType.KnownError;
            }
        }
        private PattaType isAllmemberDataValid(List<string> memberData, string totalData)
        {
            try
            {
                if (IsValidTotalRecord(totalData) == false)
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
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
                return PattaType.KnownError;
            }
        }
        private bool IsValidTotalRecord(string total)
        {
            return total.Split('-').Count() == 4;
        }
        private (bool status, List<string> bk, List<string> nobk) IsPartialBreak(List<string> memberData)
        {
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
                        //isPrevBkrec = true;
                    }
                    else if (memberData[mi].Contains('-') && memberData[mi + 1].Contains('-'))
                    {
                        nonBkData.Add(memberData[mi]);
                        //isPrevBkrec = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
                return (done, brkData, nonBkData); ;
            }

            return (done, brkData, nonBkData);

        }
        public bool IsSomeDots(List<string> memberData)
        {
            try
            {
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

            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
            return false;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
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
                dataGridView1.DataSource = AdangalOriginalList;
            else if (selected == 4)
                dataGridView1.DataSource = fullAdangalFromjson;

        }

        private void ddlLandTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLandTypes.SelectedIndex == 0) return;
            if (fullAdangalFromjson != null)
            {
                waitForm.Show(this);
                var selItem = (ddlLandTypes.SelectedItem as KeyValue);
                if (selItem.Id == -1)
                    dataGridView1.DataSource = fullAdangalFromjson.OrderBy(o => o.NilaAlavaiEn).ToList();
                else
                    dataGridView1.DataSource = fullAdangalFromjson.Where(w => (int)w.LandType == selItem.Id).OrderBy(o => o.NilaAlavaiEn).ToList();

                if (selItem.Id == 3 || selItem.Id == 4) // porambokku/porambokku error
                {
                    GridColumnVisibility(true);
                    dataGridView1.Columns["OwnerName"].DisplayIndex = 3;
                }
                waitForm.Close();
            }
        }

        private void cmbLandStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLandStatus.SelectedIndex == 0) return;

            fullAdangalFromjson = dataGridView1.DataSource as List<Adangal>;

            if (fullAdangalFromjson != null)
            {
                var selItem = (cmbLandStatus.SelectedItem as KeyValue);

                waitForm.Show(this);
                if (selItem.Id == 100)
                    dataGridView1.DataSource = GetAdangalForSomeDots();
                else if (selItem.Id == 101)
                    dataGridView1.DataSource = fullAdangalFromjson.Where(w => w.LandType != LandType.Porambokku && w.PattaEn == 0).ToList();

                //if (selItem.Id == 1) // Deleted
                //    dataGridView1.DataSource = DataAccess.GetDeletedAdangal();
                else
                    dataGridView1.DataSource = fullAdangalFromjson.Where(w => (int)w.LandStatus == selItem.Id).OrderBy(o => o.NilaAlavaiEn).ToList();

                waitForm.Close();
            }
        }

        private void ddlPattaTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlPattaTypes.SelectedIndex == 0) return;
            //waitForm.Show(this);
            //var selected = ddlListType.SelectedValue.ToInt32();

            //if (selected == 1)
            //    dataGridView1.DataSource = pattaList.Where(w => (int)w.PattaType == ddlPattaTypes.SelectedValue.ToInt32()).ToList();
            //else if (selected == 2)
            //    dataGridView1.DataSource = pattaList.Where(w => (int)w.PattaType == ddlPattaTypes.SelectedValue.ToInt32()).ToList()
            //                                .SelectMany(x => x.landDetails.Select(y => y)).ToList();

            //waitForm.Close();

            if (ddlPattaTypes.SelectedIndex == 0) return;
            waitForm.Show(this);
            var selected = ddlPattaTypes.SelectedItem as KeyValue;

            //if (selected == 1)
            dataGridView1.DataSource = pattaList.Where(w => (int)w.PattaType == selected.Id).ToList();
            //else if (selected == 2)
            //    dataGridView1.DataSource = pattaList.Where(w => (int)w.PattaType == ddlPattaTypes.SelectedValue.ToInt32()).ToList()
            //                                .SelectMany(x => x.landDetails.Select(y => y)).ToList();

            waitForm.Close();
        }
        private string GetLeftEmptyPage()
        {
            string leftPage = null;
            try
            {
                // LeftEmptyPage
                leftPage = FileContentReader.LeftPageTableTemplate;
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
                //header = header.Replace("[pasali]", pasali.ToString()).Replace("[maavattam]", maavattam).Replace("[vattam]", vattam).Replace("[varuvvaikiraamam]", $"{sVillageCode} - {tamilvillageName}");
                leftPage = leftPage.Replace("[header]", updatedHeader);
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
            return leftPage;
        }

        private void SetHeader()
        {
            updatedHeader = header.Replace("[pasali]", pasali.ToString())
                                    .Replace("[maavattam]", loadedFile.MaavattamNameTamil)
                                    .Replace("[vattam]", loadedFile.VattamNameTamil)
                                    .Replace("[varuvvaikiraamam]", $"{loadedFile.VillageCode} - {loadedFile.VillageNameTamil}")
                                    .Replace("[t]", "&emsp;&emsp;");
        }

        /// <summary>
        /// left and right cert pages.
        /// </summary>
        /// <returns></returns>
        private string GetPage2()
        {
            pageNumber += 1;
            var sb = new StringBuilder();
            sb.Append(leftCertEmpty);
            sb.Append(rightCertEmpty.Replace("[pageNo]", pageNumber.ToString()));
            return sb.ToString();
        }

        private string GetCertPage()
        {
            pageNumber += 1;
            var sb = new StringBuilder();
            //sb.Append(leftCertEmpty);
            sb.Append(certSinglePage.Replace("[pageNo]", pageNumber.ToString()));
            return sb.ToString();
        }




        private string GetRightEmptyPage()
        {
            string rightPage = null;

            try
            {
                rightPage = FileContentReader.RightPageTableTemplate;
                var rowTemplate = FileContentReader.RightPageRowTemplate;
                var totalRowTemplate = FileContentReader.RightPageTotalTemplate;

                var sb = new StringBuilder();

                for (int i = 1; i <= recordPerPage; i++)
                    sb.Append(rowTemplate);

                rightPage = rightPage.Replace("[datarows]", sb.ToString());
                rightPage = rightPage.Replace("[totalrow]", totalRowTemplate);

                pageNumber += 1;
                rightPage = rightPage.Replace("[pageNo]", pageNumber.ToString());
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }

            return rightPage;
        }

        /// <summary>
        /// Left cert and summary page.
        /// </summary>
        /// <returns></returns>
        private string GetPage3()
        {
            pageNumber += 1;
            var sb = new StringBuilder();
            sb.Append(leftCertEmpty);
            sb.Append("[summaryPages]");
            return sb.ToString();
        }

        private string GetSumPage()
        {
            pageNumber += 1;
            var sb = new StringBuilder();
            //sb.Append(leftCertEmpty);
            sb.Append("[summaryPages]");
            return sb.ToString();
        }

        /// <summary>
        /// Building and right empty pages
        /// </summary>
        /// <returns></returns>
        private string GetPage4()
        {
            //pageNumber += 1;
            var sb = new StringBuilder();
            sb.Append("[building]");
            sb.Append(GetRightEmptyPage());
            return sb.ToString();
        }

        private string GetBuildingPg()
        {
            //pageNumber += 1;
            var sb = new StringBuilder();
            sb.Append("[building]");
            //sb.Append(GetRightEmptyPage());
            return sb.ToString();
        }

        private string GetPage4SoftCopy()
        {
            //pageNumber += 1;
            var sb = new StringBuilder();
            sb.Append("[building]");
            //sb.Append(GetRightEmptyPage());
            return sb.ToString();
        }
        private string GetInitialPages()
        {
            var initialPages = new StringBuilder();

            initialPages.Append(firstPage);
            initialPages.Append(plainPage);
            initialPages.Append(GetCertPage());
            //initialPages.Append(GetEmptyPages(1));
            //initialPages.Append(GetPage2());
            //initialPages.Append(GetPage3());
            initialPages.Append(GetBuildingPg());
            initialPages.Append(GetSumPage());
            initialPages.Append(GetNotesPages(4));
            //initialPages.Append(GetPage4());


            int InitialEmptyPages = Convert.ToInt32(ConfigurationManager.AppSettings["InitialEmptyPages"]);
            initialPages.Append(GetEmptyPages(InitialEmptyPages));

            return initialPages.ToString();

        }

        private string GetInitialPagesSoftCopy()
        {
            var initialPages = new StringBuilder();

            initialPages.Append(firstPage);
            //initialPages.Append(GetEmptyPages(1));
            initialPages.Append(leftCertEmpty);
            initialPages.Append("[summaryPages]");
            initialPages.Append("[building]");
            initialPages.Append(GetNotesPages(4));
            //int InitialEmptyPages = Convert.ToInt32(ConfigurationManager.AppSettings["InitialEmptyPages"]);
            //initialPages.Append(GetEmptyPages(InitialEmptyPages));

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

        private string GetNotesPages(int pageCount)
        {
            var sb = new StringBuilder();

            for (int i = 1; i <= pageCount; i++)
            {
                sb.Append(notesPage);
            }

            return sb.ToString();

        }

        int totalPageIndexTracker = 0;
        private string GetPageTotal(List<PageTotal> source, List<PageTotal> destination, bool isSoftCopy = false)
        {
            StringBuilder totalContent = new StringBuilder();

            try
            {
                var landTypeGroup = (from wl in source
                                     where wl.LandType != LandType.Zero
                                     group wl by wl.LandType into newGrp
                                     select newGrp).ToList();


                landTypeGroup.ToList().ForEach(fe =>
                {
                    var dataToProcess = fe.ToList();

                    var pageCount = dataToProcess.Count / pageTotalrecordPerPage;
                    if (dataToProcess.Count % pageTotalrecordPerPage > 0) pageCount += 1;

                    var tableTemplate22 = FileContentReader.PageTotalTableTemplate;
                    var rowTemplate22 = FileContentReader.PageTotalRowTemplate;
                    var landType = fe.Key.ToName();
                    bool isRightSide = false;
                    for (int i = 0; i <= pageCount - 1; i++)
                    {
                        totalPageIndexTracker += 1;
                        isRightSide = totalPageIndexTracker.IsEven();

                        if (isRightSide)
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

                        var totalRows = row.Replace("[pageNo]", $"<b>{tamilMoththam}</b>")
                                                  .Replace("[parappu]", $"<b>{AdangalFn.GetSumThreeDotNo(temData.Select(s => s.ParappuTotal).ToList())}</b>")
                                                  .Replace("[theervai]", $"<b>{totalTheervai}</b>");

                        // Created a sub list item!
                        destination.Add(new PageTotal()
                        {
                            ParappuTotal = AdangalFn.GetSumThreeDotNo(temData.Select(s => s.ParappuTotal).ToList()),
                            TheervaiTotal = temData.Sum(s => s.TheervaiTotal),
                            LandType = fe.Key,
                            PageNo = pageNumber
                        });


                        tbl = tbl.Replace("[datarows]", sb.ToString());
                        tbl = tbl.Replace("[totalrow]", totalRows);
                        tbl = tbl.Replace("[landtype]", landType);
                        tbl = tbl.Replace("[header]", updatedHeader);
                        if (isSoftCopy)
                            tbl = tbl.Replace("[pageNo]", empty);
                        else
                            tbl = tbl.Replace("[pageNo]", isRightSide ? pageNumber.ToString() : empty);

                        if (fe.Key == LandType.Porambokku)
                        {
                            tbl = tbl.Replace("சாகுபடி", fe.Key.ToName());
                        }
                        totalContent.Append(tbl);
                    }

                });
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }

            return totalContent.ToString();
        }
        private void SetSummaryPageDetails(List<PageTotal> source)
        {
            List<Summary> summaryList = new List<Summary>();

            try
            {
                source.ForEach(ff =>
                {
                    summaryList.Add(new Summary()
                    {
                        Id = (int)ff.LandType,
                        Parappu = ff.ParappuTotal,
                        Pakkam = pageRangeList[(int)ff.LandType].Caption,
                        Vibaram = ff.LandType.ToName()
                    });
                });
                DataAccess.SaveSummary(summaryList);

            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }

        private string GetSummaryPage(bool isInitialSummaryPage3 = false, bool isSoftCopy = false)
        {
            StringBuilder totalContent = new StringBuilder();

            List<Summary> summaryList = DataAccess.GetSummary();

            try
            {
                var tbl = FileContentReader.SummaryTableTemplate;
                var row = FileContentReader.SummayRowTemplate;

                totalPageIndexTracker += 1;
                var isRightSide = totalPageIndexTracker.IsEven();

                if (isRightSide)
                    pageNumber += 1;

                string dataRows = "";
                StringBuilder sb = new StringBuilder();

                summaryList.ForEach(ff =>
                {
                    dataRows = row.Replace("[vibaram]", ff.Vibaram)
                                          .Replace("[pageNo]", ff.Pakkam)
                                          .Replace("[parappu]", ff.Parappu);
                    sb.Append(dataRows);

                });

                var totalRows = row.Replace("[vibaram]", $"<span style='font - size:20px;'><b>{kiraamaMoththam}</b></span>")
                                          .Replace("[pageNo]", empty)
                                          .Replace("[parappu]", $"<span style='font - size:20px;'><b>{AdangalFn.GetSumThreeDotNo(summaryList.Where(w => w.Id != -1).Select(s => s.Parappu).ToList())}</b></span>");

                tbl = tbl.Replace("[datarows]", sb.ToString());
                tbl = tbl.Replace("[totalrow]", totalRows);
                tbl = tbl.Replace("[header]", updatedHeader);

                if (isSoftCopy)
                    tbl = tbl.Replace("[pageNo]", empty);
                if (isInitialSummaryPage3)
                    tbl = tbl.Replace("[pageNo]", "3");
                else
                    tbl = tbl.Replace("[pageNo]", isRightSide ? pageNumber.ToString() : empty);
                tbl = tbl.Replace("[landtype]", "மொத்த கிராம விபரம்");
                totalContent.Append(tbl);
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
            return totalContent.ToString();
        }

        private string GetGovtBuildingPage()
        {
            StringBuilder totalContent = new StringBuilder();

            var gd = (from gb in DataAccess.GetGovtBuilding()
                      group gb by gb.GroupId into newGroup
                      select newGroup).ToList();
            try
            {
                var tbl = FileContentReader.GovtBuildingTableTemplate;
                var row = FileContentReader.GovtBuildingRowTemplate;

                totalPageIndexTracker += 1;
                var isRightSide = totalPageIndexTracker.IsEven();

                if (isRightSide)
                    pageNumber += 1;

                string dataRows = "";
                StringBuilder sb = new StringBuilder();

                gd.ForEach(fe =>
                {

                    List<GovtBuilding> govtBuildings = fe.ToList();

                    for (int ff = 0; ff <= govtBuildings.Count - 1; ff++)
                    {
                        dataRows = row.Replace("[pulaen]", govtBuildings[ff].PulaEn)
                                              .Replace("[parappu]", govtBuildings[ff].Parappu);

                        if (ff == 0)
                        {
                            dataRows = dataRows.Replace("[vibaram]", govtBuildings[ff].Vibaram)
                                              .Replace("[buildingname]", govtBuildings[ff].BuildingName)
                                              .Replace("[rowspan]", govtBuildings.Count().ToString());

                        }
                        else
                        {
                            dataRows = dataRows.Replace("<td rowspan='[rowspan]' style='vertical-align:middle;'>[vibaram]</td>", empty)
                                               .Replace("<td rowspan='[rowspan]' style='vertical-align:middle;'>[buildingname]</td>", empty);
                        }

                        sb.Append(dataRows);

                    }

                });

                tbl = tbl.Replace("[datarows]", sb.ToString());
                tbl = tbl.Replace("[header]", updatedHeader);
                tbl = tbl.Replace("[pageNo]", isRightSide ? pageNumber.ToString() : empty);
                tbl = tbl.Replace("[landtype]", "அரசு கட்டிடங்கள் விபரம்");
                totalContent.Append(tbl);
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
            return totalContent.ToString();
        }

        private bool IsUserAuthenticated()
        {
            string actualCode = "";
            //string expectedCode = "";
            actualCode = General.ShowPrompt("Passcode", "Verification");

            var now = DateTime.Now;
            var quarter = now.Minute % 15 == 0 ? (now.Minute / 15) : (now.Minute / 15) + 1;
            var pc = (1983 + now.Month + now.Day + now.Hour + quarter + now.Minute).ToString();

            string expectedCode = "";
            pc.ToList().ForEach(c => expectedCode += (c.ToString().ToInt32() + 1));
            //expectedCode = expectedCode;

            if (actualCode != expectedCode)
            {
                MessageBox.Show("Sorry , wrong code!");
                return false; ;
            }
            else
            {
                MessageBox.Show("Success!");
                return true;
            }


        }

        List<KeyValue> pageRangeList = new List<KeyValue>();

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            LogMessage($"STEP-5 - Generate Started");

            if (pcEnabled)
            {
                if (IsUserAuthenticated() == false) return;
            }

            try
            {
                LogMessage($"STARTED HTML GENERATION @ {DateTime.Now.ToLongTimeString()}");
                pageNumber = 0;

                StringBuilder allContent = new StringBuilder();
                pageTotalList = new List<PageTotal>();
                pageTotal2List = new List<PageTotal>();
                pageTotal3List = new List<PageTotal>();

                var mainHtml = FileContentReader.MainHtml;
                string initialPages = GetInitialPages();

                mainHtml = mainHtml.Replace("[initialPages]", initialPages);

                var landTypeGroup = (from wl in fullAdangalFromjson
                                     where wl.LandType != LandType.Zero
                                     group wl by wl.LandType into newGrp
                                     select newGrp).ToList();

                var rowTemplate22 = FileContentReader.LeftPageRowTemplate;
                var totalTemplate22 = FileContentReader.LeftPageTotalTemplate;
                var tableTemplate22 = FileContentReader.LeftPageTableTemplate;

                landTypeGroup.ForEach(fe =>
                {
                    var dataToProcess = fe.ToList();

                    var pageCount = dataToProcess.Count / recordPerPage;
                    if (dataToProcess.Count % recordPerPage > 0) pageCount += 1;
                    var landType = fe.Key.ToName();
                    string pageRange = "";

                    if (isTestingMode)
                    {
                        pageCount = pageCount >= TestingPage ? TestingPage : pageCount;
                    }

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
                        var totalparappu = AdangalFn.GetSumThreeDotNo(temData.Select(s => s.Parappu).ToList());
                        //var totalTheervai = temData.Sum(s => Convert.ToDecimal(s.Theervai));

                        decimal totalTheervai = 0;
                        if (fe.Key != LandType.Porambokku)
                            totalTheervai = temData.Sum(s => Convert.ToDecimal(s.Theervai));

                        var total = totalTemplate.Replace("[moththaparappu]", totalparappu).Replace("[moththatheervai]", totalTheervai == 0 ? "" : totalTheervai.ToString());

                        leftPage = leftPage.Replace("[totalrow]", total);
                        leftPage = leftPage.Replace("[landtype]", landType);
                        leftPage = leftPage.Replace("[header]", updatedHeader);

                        allContent.Append(leftPage);
                        allContent.Append(GetRightEmptyPage()); // right page

                        if (i == 0)
                        {
                            pageRange = $"{pageNumber}-";
                        }
                        if (i == pageCount - 1)
                        {
                            pageRange += $"{pageNumber}";
                            pageRangeList.Add(new KeyValue() { Id = (int)fe.Key, Caption = pageRange });
                        }

                        pageTotalList.Add(new PageTotal()
                        {
                            PageNo = pageNumber,
                            ParappuTotal = totalparappu,
                            TheervaiTotal = totalTheervai,
                            LandType = fe.Key
                        });
                    }
                });

                allContent.Append(GetEmptyPages(1));  // add 4 empty pages.
                allContent.Append(GetPageTotal(pageTotalList, pageTotal2List));
                if (isTestingMode == false)
                {
                    allContent.Append(GetPageTotal(pageTotal2List, pageTotal3List));
                    SetSummaryPageDetails(pageTotal3List);
                    //allContent.Append(GetOverallTotal(pageTotal3List));
                }
                else
                {
                    SetSummaryPageDetails(pageTotal2List);
                    //allContent.Append(GetOverallTotal(pageTotal2List));
                }

                allContent.Append(GetSummaryPage());
                int FinalEmptyPages = Convert.ToInt32(ConfigurationManager.AppSettings["FinalEmptyPages"]);
                allContent.Append(GetEmptyPages(FinalEmptyPages));

                // Final Touch
                mainHtml = mainHtml.Replace("[summaryPages]", GetSummaryPage(true));
                mainHtml = mainHtml.Replace("[building]", GetGovtBuildingPage());
                mainHtml = mainHtml.Replace("[allPageData]", allContent.ToString());
                mainHtml = mainHtml.Replace("[certifed]", GetCertifiedContent());

                var fPath = AdangalConstant.CreateAndReadPath($"{loadedFile.VillageName}-Result");
                var filePath = Path.Combine(fPath, $"{loadedFile.VillageName}-{DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss")}.htm");
                File.AppendAllText(filePath, mainHtml);

                LogMessage($"COMPLETED HTML GENERATION @ {filePath}");
                Process.Start(filePath);

                LogMessage($"STEP-5 - Generate Completed");
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }
        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            lblMessage.Text = $"Record Count: {dataGridView1.Rows.Count} ";

        }

        private bool NoInternet()
        {
            if (General.CheckForInternetConnection() == false)
            {
                MessageBox.Show("No Internet Connection!");
                LogError($"No Internet Connection!");
                return true;

            }
            else
            {
                return false;
            }

        }

        int ddlValue = 0;
        private void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selItem = (ComboData)ddlDistrict.SelectedItem;
                if (ddlDistrict.SelectedIndex == 0 || ddlValue == selItem.Value)
                    return;

                ddlValue = selItem.Value;
                waitForm.Show(this);
                LogMessage("STEP-1 STARTED");
                if (NoInternet())
                {
                    waitForm.Close();
                    return;
                }


                if (ddlDistrict.SelectedItem != null)
                {

                    LogMessage($"STEP-1 - Maavattam: {selItem.Value}-{selItem.Display}");

                    var url = $"https://eservices.tn.gov.in/eservicesnew/land/ajax.html?page=taluk&districtCode={selItem.Value}";
                    var response = WebReader.CallHttpWebRequest(url);

                    BindDropdown(cmbTaluk, WebReader.xmlToDynamic(response, "taluk"), "Display", "Value");
                    cmbTaluk.SelectedIndexChanged += new EventHandler(cmbTaluk_SelectedIndexChanged);
                }
                waitForm.Close();
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }

        }
        private void cmbTaluk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (NoInternet()) return;

                if (ddlDistrict.SelectedItem != null)
                {
                    var disValue = ((ComboData)ddlDistrict.SelectedItem).Value;
                    var talValue = ((ComboData)cmbTaluk.SelectedItem).Value;
                    if (talValue == -1) return;
                    var tv = talValue.ToString().PadLeft(2, '0');
                    LogMessage($"STEP-1 - Vattam: {talValue}-{((ComboData)cmbTaluk.SelectedItem).Display}");

                    var url = $"https://eservices.tn.gov.in/eservicesnew/land/ajax.html?page=village&districtCode={disValue}&talukCode={tv}";
                    var response = WebReader.CallHttpWebRequest(url);

                    BindDropdown(cmbVillages
                        , WebReader.xmlToDynamic(response, "village"), "Display", "Value");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }

        }
        private void vao_Load(object sender, EventArgs e)
        {
            ddlDistrict.SelectedIndexChanged += new System.EventHandler(this.ddlDistrict_SelectedIndexChanged);
        }
        private void button2_Click_1(object sender, EventArgs e)
        {

            try
            {
                LogMessage($"STEP-3 - Raedy For Print - Started");
                List<KeyValue> onlineData;

                if (DataAccess.IsSubDivFileExist()) onlineData = DataAccess.GetSubdiv();
                else if (NoInternet()) return;
                else onlineData = SaveLandCount();

                fullAdangalFromjson = DataAccess.GetActiveAdangal();
                LoadSurveyAndSubdiv();

                var expLandDetails = (onlineData
                                    .OrderBy(o => o.Value)
                                    .ThenBy(o => o.Caption, new AlphanumericComparer())
                                    .Select(s => (s.Value.ToString().Trim() + "~" + s.Caption.Trim()).Trim())).ToList();

                List<string> actualLandDetails;

                actualLandDetails = fullAdangalFromjson
                                       .OrderBy(o => o.NilaAlavaiEn)
                                       .ThenBy(o => o.UtpirivuEn, new AlphanumericComparer())
                                       .Select(s =>
                                       (s.NilaAlavaiEn.ToString().Trim() + "~" + s.UtpirivuEn.Trim()).Trim())
                                       .ToList();



                notInPdfToBeAdded = expLandDetails.Except(actualLandDetails).ToList();
                notInOnlineToBeDeleted = actualLandDetails.Except(expLandDetails).ToList();

                //notInPdfToBeAdded =  notInPdfToBeAdded.OrderByDescending(o => o).ThenByDescending(o => o.UtpirivuEn, new AlphanumericComparer());

                if (canAddMissedSurvey)
                {
                    cmbItemToBeAdded.DataSource = notInPdfToBeAdded;
                    SortMissedSurveys();
                    return;
                }



                btnDelete.Enabled = (notInOnlineToBeDeleted.Count > 0);
                btnAdd.Enabled = (notInPdfToBeAdded.Count > 0);

                var (r, status) = IsReadyToPrint();
                btnStatusCheck.Text = status;
                btnStatusCheck.BackColor = r ? Color.Green : Color.Red;


                var wrongNameCount = fullAdangalFromjson.Where(w => w.LandStatus == LandStatus.WrongName).Count();

                var percCompleted = onlineData.Count.PercentageBtwDecNo(actualLandDetails.Count - (notInOnlineToBeDeleted.Count + wrongNameCount), 2);

                if (loadedFile.InitialPercentage == null)
                {
                    loadedFile = DataAccess.UpdatePercentage(loadedFile, percCompleted);
                }
                if (percCompleted.ToInt32() == 100)
                {
                    var correctedCount = fullAdangalFromjson.Where(w => w.LandStatus != LandStatus.NoChange).Count();
                    var correctedPerc = 100 - onlineData.Count.PercentageBtwDecNo(correctedCount, 2);
                    loadedFile = DataAccess.UpdateCorrectedPerc(loadedFile, correctedPerc);
                }
                btnPercentage.Text = percCompleted.ToString() + "%";

                if (percCompleted == 100)
                {
                    btnGenerate.Enabled = true;
                    btnPercentage.BackColor = Color.Green;
                }
                else
                {
                    btnGenerate.Enabled = false;
                    btnPercentage.BackColor = Color.Red;
                }

                btnGenerate.Enabled = isTestingMode;

                cmbItemToBeAdded.DataSource = notInPdfToBeAdded;
                SortMissedSurveys();
                LogMessage($"STEP-3 - Raedy For Print - Completed");
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }
        private (bool r, string status) IsReadyToPrint()
        {
            StringBuilder status = new StringBuilder();
            bool result = true;
            try
            {
                var errorCount = DataAccess.GetErrorAdangal().Count;

                if (notInPdfToBeAdded.Count != 0)
                {
                    status.AppendLine($"ADD:{notInPdfToBeAdded.Count}");
                    result = false;
                }

                if (notInOnlineToBeDeleted.Count != 0)
                {
                    status.AppendLine($"DELETE:{notInOnlineToBeDeleted.Count}");
                    result = false;
                }

                if (errorCount != 0)
                {
                    status.AppendLine($"ERROR REC:{errorCount}");
                    result = false;
                }
                var pattaNameIssue = fullAdangalFromjson.Where(w => w.LandStatus == LandStatus.WrongName);

                if (pattaNameIssue.Count() != 0)
                {
                    status.AppendLine($"NAME ISSUE :{pattaNameIssue.Count()}");
                    result = false;
                }

                var someDotsIssue = GetAdangalForSomeDots();

                if (someDotsIssue.Count() != 0)
                {
                    status.AppendLine($"SomeDots ISSUE :{someDotsIssue.Count()}");
                    result = false;
                }


                // Validate Land Type.
                lblLandTypeError.Visible = lblLandStatusError.Visible = lblPattaCheckError.Visible = false;
                var totalAdangal = fullAdangalFromjson.Count;

                var landTypeCheck = ddlLandTypes.DataSource as List<KeyValue>;
                var totalLandTypes = (landTypeCheck[2].Value + landTypeCheck[3].Value + landTypeCheck[4].Value + landTypeCheck[5].Value);

                if (totalAdangal != totalLandTypes || landTypeCheck[1].Value != totalLandTypes)
                {
                    status.AppendLine($"LandType ISSUE");
                    lblLandTypeError.Visible = true;
                    result = false;
                }

                // Validate Land Status.
                var landStatusCheck = cmbLandStatus.DataSource as List<KeyValue>;
                var totalLandStatus = landStatusCheck.Sum(s => s.Value);

                if (totalAdangal != totalLandStatus)
                {
                    status.AppendLine($"LandStatus ISSUE");
                    lblLandStatusError.Visible = true;
                    result = false;
                }

                var pattaCount = pattaList.Count;

                // Validate Patta types.
                var pattaCheck = ddlPattaTypes.DataSource as List<KeyValue>;
                var totalPattaStatus = pattaCheck.Skip(2).Sum(s => s.Value);

                if (pattaCount != totalPattaStatus || pattaCheck.Skip(9).Sum(s => s.Value) != 0)
                {
                    status.AppendLine($"Patta ISSUE");
                    lblPattaCheckError.Visible = true;
                    result = false;
                }

                var needChangePorambokku = fullAdangalFromjson.Where(w => w.LandType == LandType.Porambokku || w.LandType == LandType.Porambokku)
                    .Where(w => w.LandStatus == LandStatus.NoChange).Count();

                if (needChangePorambokku > 0)
                {
                    status.AppendLine($"Porambokku: {needChangePorambokku}");
                    result = false;
                }


                LogMessage($"ready: {result} STATUS: {status.ToString()}");
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
            return (result, status.ToString());

        }
        private List<KeyValue> SaveLandCount()
        {
            try
            {
                waitForm.Show(this);
                LogMessage($"GETTING LAND COUNT - FROM INTERNET");
                var totalLandList = new List<KeyValue>();

                if (loadedFile.VillageCode > 0)
                {
                    var disValue = loadedFile.MaavattamCode;
                    var talValue = loadedFile.VattamCode.ToString().PadLeft(2, '0');
                    var villageValue = loadedFile.VillageCode.ToString().PadLeft(3, '0');

                    var url = "";
                    int continueCheckCount = 0;
                    int maxSearch = 200;

                    for (int i = 1; i <= maxSearch; i++)
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
                        if (i == maxSearch)
                        {
                            MessageBox.Show($"Max reached to {i}");
                            maxSearch += 25;
                        }
                    }
                }
                LogMessage($"GETTING LAND COUNT - COMPLETED");
                waitForm.Close();
                return DataAccess.SubdivToJson(totalLandList);

            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
                return null;
            }


        }

        private void PreLoadFile()
        {
            SetHeader();
            firstPage = FileContentReader.FirstPageTemplate;
            notesPage = FileContentReader.NotesPageTemplate;

            title = title.Replace("[pasali]", pasali.ToString()).Replace("[br]", "</br>").Replace("[varuvvaikiraamam]", loadedFile.VillageNameTamil);
            firstPage = firstPage.Replace("[title]", title);
            plainPage = FileContentReader.EmptyPageTemplate;
            certSinglePage = FileContentReader.CertPageTemplate;
            leftEmpty = GetLeftEmptyPage();
            leftCertEmpty = FileContentReader.LeftPageCertTableTemplate;
            leftCertEmpty = leftCertEmpty.Replace("[header]", updatedHeader);
            rightCertEmpty = FileContentReader.RightPageCertTableTemplate;
        }

        private void LoadFileDetails()
        {
            var selectedMaavatta = (ddlDistrict.SelectedItem as ComboData);
            var selectedVattam = (cmbTaluk.SelectedItem as ComboData);
            var selectedVillage = (cmbVillages.SelectedItem as ComboData);

            //var isVillageSelected = (selectedVillage.Value != -1);
            loadedFile.MaavattamCode = selectedMaavatta.Value;
            loadedFile.MaavattamName = selectedMaavatta.Display;
            loadedFile.MaavattamNameTamil = selectedMaavatta.DisplayTamil;

            loadedFile.VattamCode = selectedVattam.Value;
            loadedFile.VattamName = selectedVattam.Display;
            loadedFile.VattamNameTamil = txtVattam.Text.Trim();

            loadedFile.FirkaName = txtFirka.Text.Trim();

            loadedFile.VillageCode = selectedVillage.Value;
            loadedFile.VillageName = selectedVillage.Display;
            loadedFile.VillageNameTamil = txtVaruvai.Text.Trim();


        }

        WaitWnd.WaitWndFun waitForm = new WaitWnd.WaitWndFun();

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            try
            {
                LogMessage($"STEP-2 - Started - First time Load");
                if (txtVattam.Text.Trim() == empty || txtFirka.Text.Trim() == empty || txtVaruvai.Text.Trim() == empty)
                {
                    MessageBox.Show("Vattam, Firka and Village in tamil font is mandatory!");
                    LogMessage($"STEP-2 - Value missing: Vattam, Firka and Village in tamil font");
                    return;
                }

                // Getting subdiv file
                //List<KeyValue> onlineData;
                //if (DataAccess.IsSubDivFileExist()) onlineData = DataAccess.GetSubdiv();

                if (NoInternet()) return;
                if (DataAccess.IsSubDivFileExist() == false)
                {
                    SaveLandCount();
                    btnLoadFirstTIme.Enabled = true;
                }
                else
                {




                    //List<KeyValue> list = DataAccess.GetSubdiv();
                    //AdangalConverter.ProcessAdangal(list);

                }

                // Check for adangal data sync!


                //FolderBrowserDialog fbd = new FolderBrowserDialog();
                //if (DialogResult.OK == fbd.ShowDialog())
                //    reqFileFolderPath = fbd.SelectedPath;
                //else
                //return;

                //var selectedFolder = General.SelectFolderPath();
                //if (selectedFolder == null) return;
                //reqFileFolderPath = ""; // DataAccess.GetSubdiv();

                // Loading basic file details...
                LoadFileDetails();
                LogMessage($"STEP-2 - VattamNameTamil: {loadedFile.VattamNameTamil} FirkaName: {loadedFile.FirkaName} VillageNameTamil: {loadedFile.VillageNameTamil}");
                PreLoadFile();

                //if (haveValidFiles(reqFileFolderPath))
                //{

                //LogMessage($"You have all required valid files to proceed process.");



                /*
                 * 
                 * //waitForm.Show(this);
                 * pattaList = new PattaList();

                chittaPdfFile = General.CombinePath(reqFileFolderPath, ChittaPdfFile);
                chittaTxtFile = General.CombinePath(reqFileFolderPath, ChittaTxtFile);
                aRegFile = General.CombinePath(reqFileFolderPath, AregFile);
                chittaContent = chittaPdfFile.GetPdfContent();
                LoadPdfPattaNo();
                //**
                var pattaas = chittaContent.Replace("பட்டா எண்    :", "$"); //("பட்டா எண்", "$");
                var data = pattaas.Split('$').ToList();
                pdfvillageName = data.First().Split(':')[3].Trim();
                waitForm.Close();
                if (DialogResult.No == MessageBox.Show($"{pdfvillageName} village?", "Confirm", MessageBoxButtons.YesNo))
                {
                    LogMessage($"REJECTED THE  VILLAGE PDF FILE- {pdfvillageName}");
                    return;
                }
                waitForm.Show(this);
                //**
                ProcessNames();
                ProcessChittaFile(); // Nansai, Pun, Maa,
                ProcessAreg();  // Puram

                DataAccess.SavePattaList(pattaList);
                DataAccess.SaveWholeLandList(WholeLandList);
                DataAccess.SaveAdangalOriginalList(AdangalOriginalList);
                EnableReadyForNew();
                ProcessFullReport();

                DataAccess.AddNewLoadedFile(loadedFile);
                LogMessage($"Add/Update Loaded File.");
                waitForm.Close();
                */

                //}
                //else
                //{
                //  LogMessage($"Some file missing in the folder?");
                //MessageBox.Show("Some file missing in the folder?");
                //}

                LogMessage($"STEP-2 - Completed - - First time Load");


            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }

        }
        private bool haveValidFiles(string folderPath)
        {
            try
            {

                var files = Directory.GetFiles(folderPath);

                var filesCount = files.Count();

                if (filesCount != 3)
                {
                    MessageBox.Show("Some file missing!");
                    return false;
                }

                var pdffilesCount = files.Where(w => w.EndsWith(".pdf")).Count();
                var txtfilesCount = files.Where(w => w.EndsWith(".txt")).Count();

                if (pdffilesCount != 2)
                {
                    MessageBox.Show("Some txt pdf file missing!");
                    return false;
                }
                if (txtfilesCount != 1)
                {
                    MessageBox.Show("Some txt or pdf file missing!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
                return false;
            }

        }

        private void UpadteLogPath(string villageName)
        {
            var logFolder = AdangalConstant.CreateAndReadPath($"{villageName}-Log", villageName);
            //logHelper  =null;
            logHelper = new LogHelper("AdangalLog", logFolder, Environment.UserName, villageName);
        }
        private void cmbVillages_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbVillages.SelectedIndex == 0) return;
            var selItem = (ComboData)cmbVillages.SelectedItem;
            LoadFileDetails();
            AdangalConstant.villageName = selItem.Display;
            UpadteLogPath(selItem.Display);
            DataAccess.SetVillageName();

            if (selItem.Value != -1)
                LogMessage($"STEP - 1 - Village: {selItem.Value}-{selItem.Display}");


            var fileStatus = AdangalConverter.IsSync();

            MessageBox.Show(fileStatus);

            btnLoadFirstTIme.Enabled = true;

            //btnReadFile.Enabled = (DataAccess.IsSubDivFileExist() == false);

            //CheckJsonExist();


        }

        private void CheckJsonExist()
        {
            SetVillage();
            if (DataAccess.IsAdangalFileExist())
            {
                MessageBox.Show($"File already processed for {loadedFile.VillageName}, Please use that?", "Already Exist", MessageBoxButtons.OK);
                btnReadFile.Enabled = false;
            }
            else
            {
                btnReadFile.Enabled = true;
            }
        }

        private void SetVillage()
        {
            AdangalConstant.villageName = ddlProcessedFiles.SelectedItem.ToString();
            DataAccess.SetVillageName();
        }
        private void LoadSurveyAndSubdiv()
        {
            try
            {
                LogMessage($"LOADING Survey and subdiv no.");
                cmbSurveyNo.SelectedIndexChanged -= CmbSurveyNo_SelectedIndexChanged;
                var d = (from a in fullAdangalFromjson
                         group a by a.NilaAlavaiEn into newGrp
                         select newGrp).ToList();

                var surevyNos = d.Select(s => s.Key).ToList();

                cmbSurveyNo.DataSource = d.Select(s => s.Key).OrderBy(o => o).ToList();
                lblSurveyNo.Text = $"survey no({surevyNos.Count})";
                cmbSurveyNo.SelectedIndexChanged += CmbSurveyNo_SelectedIndexChanged;

            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }

        }
        private void CmbSurveyNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbSubdivNo.SelectedIndexChanged -= CmbSubdivNo_SelectedIndexChanged;
                var d = (from a in fullAdangalFromjson
                         group a by a.NilaAlavaiEn into newGrp
                         from ng in newGrp
                         where ng.NilaAlavaiEn == cmbSurveyNo.SelectedItem.ToInt32()
                         select ng.UtpirivuEn).ToList();

                cmbSubdivNo.DataSource = d.OrderBy(o => o, new AlphanumericComparer()).ToList();

                cmbSubdivNo.SelectedIndexChanged += CmbSubdivNo_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }
        private void CmbSubdivNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = fullAdangalFromjson.Where(w =>
                                                                w.NilaAlavaiEn == cmbSurveyNo.SelectedItem.ToInt32() &&
                                                                w.UtpirivuEn == cmbSubdivNo.SelectedItem.ToString()).ToList();
        }
        private void EnableReadyForNew()
        {
            var selectedMaavatta = (ddlDistrict.SelectedItem as ComboData);
            var selectedVattam = (cmbTaluk.SelectedItem as ComboData);
            var selectedVillage = (cmbVillages.SelectedItem as ComboData);

            //var isVillageSelected = (selectedVillage.Value != -1);
            loadedFile.MaavattamNameTamil = selectedMaavatta.DisplayTamil;
            loadedFile.MaavattamCode = selectedMaavatta.Value;
            loadedFile.VattamCode = selectedVattam.Value;
            loadedFile.VillageName = selectedVillage.Display;
            loadedFile.VillageCode = selectedVillage.Value;

            //var canEnable = (isVillageSelected && ddlListType.SelectedValue.ToInt32() == 4);

            //if (canEnable)
            //BindDropdown(cmbLandStatus, GetLandStatusOptions(), "DisplayMember", "Id");

            //btnReadFile.Enabled = isVillageSelected;
        }

        private void EnableReadyForExist()
        {
            //if ((ddlListType.SelectedValue.ToInt32() == 4))
            BindDropdown(cmbLandStatus, GetLandStatusOptions(), "DisplayMember", "Id");
            BindDropdown(ddlLandTypes, GetLandTypes(), "DisplayMember", "Id");
        }

        private void SetTestMode()
        {
            ddlPattaTypes.Visible = cmbFulfilled.Visible =
                lblPattaCheck.Visible = ddlListType.Visible = btnGenerate.Enabled =
                btnSoftGen.Enabled = isTestingMode;

            grpTheervaiTest.Visible = needTheervaiTest;
            //txtAddNewSurvey.Enabled = btnReady.Enabled = canAddMissedSurvey;
        }
        private void cmbFulfilled_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFulfilled.SelectedIndex == 0) return;
            var selValue = ((KeyValue)cmbFulfilled.SelectedItem).Id;

            waitForm.Show(this);
            if (selValue == 1) dataGridView1.DataSource = GetFullfilledAdangal();
            else if (selValue == 2) dataGridView1.DataSource = GetExtendedAdangal();
            waitForm.Close();
        }

        private List<Adangal> GetAdangalForSomeDots()
        {
            return fullAdangalFromjson.Where(w => w.UtpirivuEn.Contains("ே") || w.UtpirivuEn.Contains("\0")).ToList();
        }


        private List<Adangal> GetFullfilledAdangal()
        {
            return fullAdangalFromjson.Where(w => string.IsNullOrEmpty(w.UtpirivuEn) || w.UtpirivuEn == "-").ToList();
        }

        private List<Adangal> GetExtendedAdangal()
        {
            return fullAdangalFromjson.Where(w => !string.IsNullOrEmpty(w.UtpirivuEn) && w.UtpirivuEn != "-").ToList();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LogMessage($"STEP-4 - Delete Started");
                fullAdangalFromjson = DataAccess.SetDeleteFlag(notInOnlineToBeDeleted);
                dataGridView1.DataSource = fullAdangalFromjson;
                LogMessage($"set delete flag to {notInOnlineToBeDeleted.Count} land");
                LogMessage($"STEP-4 - Delete Completed");
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }
        private void EditCancel()
        {
            dataGridView1.CurrentCell.Style.BackColor = Color.Red;
            dataGridView1.CurrentCell.Style.ForeColor = Color.Yellow;
        }
        private void EditSuccess()
        {
            dataGridView1.CurrentCell.Style.BackColor = Color.LightGreen;
            dataGridView1.CurrentCell.Style.ForeColor = Color.White;
            this.dataGridView1.ClearSelection();
            IsEnterKey = false; // reset flag after ecery success edit.
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if (ddlListType.SelectedIndex != 0) return;

                if (IsEnterKey == false)
                {
                    EditCancel();
                    return;
                }

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

                // Edit Name
                if (owningColumnName == "OwnerName")
                {
                    DataAccess.UpdateOwnerName(cus, cellValue);
                    EditSuccess();
                    //button2_Click_1(null, null);
                    return;
                }

                if (owningColumnName == "LandStatus")
                {
                    DataAccess.UpdateLandStatus(cus);
                    EditSuccess();
                    //button2_Click_1(null, null);
                    return;
                }

                if (owningColumnName == "PattaEn")
                {
                    DataAccess.UpdatePattaEN(cus);
                    EditSuccess();
                    //button2_Click_1(null, null);
                    return;
                }

            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }
        public static string GetGridCellValue(DataGridView grid, int rowIndex, string columnName)
        {
            var cellValue = Convert.ToString(grid.Rows[rowIndex].Cells[columnName].Value);
            return (cellValue == string.Empty) ? null : cellValue;
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            IsEnterKey = (keyData == Keys.Enter);
            return false;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            LogMessage($"STEP-4 - Add Started");
            txtAddNewSurvey.Enabled = btnAddNewSurvey.Enabled = cmbItemToBeAdded.Enabled = true;
            LogMessage($"STEP-4 - Add Completed");
        }



        private void btnAddNewSurvey_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAddNewSurvey.Text.Trim() == empty)
                {
                    MessageBox.Show("Please provide data to process");
                    return;
                }
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
                var firstRowWithName = sec.Replace("புல எண்", "$").Split('$').ToList()[0];
                if (firstRowWithName.Contains("2."))
                {
                    firstRowWithName = firstRowWithName.Replace("2.", "$").Split('$')[0];
                }
                var name = AdangalConverter.GetOwnerName(firstRowWithName);

                //var surveysubdiv = cmbItemToBeAdded.SelectedItem.ToString().Split('~').ToList();

                int addedCount = 0;
                neededData.ToList().ForEach(fe =>
                {
                    var rowData = fe.Split('\t').ToList();
                    var adangal = AdangalConverter.GetAdangalFromCopiedData(rowData, pattaEn, name);

                    if (notInPdfToBeAdded.Contains($"{adangal.NilaAlavaiEn}~{adangal.UtpirivuEn}"))
                    {
                        DataAccess.AddNewAdangal(adangal);
                        DataAccess.SaveMissedAdangal(adangal);
                        addedCount += 1;
                        LogMessage($"Added new land {adangal.ToString()}");
                    }
                });

                MessageBox.Show($"added {addedCount} land details");
                button2_Click_1(null, null);

                txtAddNewSurvey.Clear();
            }
            catch (Exception ex)
            {
                LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }
        //private string GetOwnerName(string nameRow)
        //{
        //    try
        //    {
        //        nameRow = nameRow.Split(' ').ToList().Where(w => w.Trim() != "").ToList()[1];
        //        string name = "";
        //        if (relationTypesCorrect.Any(a => nameRow.Split('\t').ToList().Contains(a))) // have valid names.
        //        {
        //            var delitList = relationTypesCorrect.Intersect(nameRow.Split('\t').ToList()).ToList();

        //            if (delitList.Count == 1)
        //            {
        //                var delimit = delitList[0];
        //                name = nameRow.Replace(delimit, "$").Split('$')[1];
        //                var d = nameRow.Replace(delimit, "$").Split('$');
        //                name = $"{d[1]}";
        //            }
        //            else
        //            {
        //                // ERROR!
        //                MessageBox.Show("Error!");
        //            }
        //        }

        //        return name.Trim().Replace("\t", "").Replace("-", "");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
        //        return null;
        //    }
        //}
        //private Adangal GetAdangalFromCopiedData(List<string> data, string pattaEn, string ownerName)
        //{
        //    var adangal = new Adangal();
        //    try
        //    {
        //        // Test for both fullfilled and extend also
        //        adangal.NilaAlavaiEn = data[0].ToInt32();
        //        adangal.UtpirivuEn = data[1];

        //        var (lt, par, thee) = GetLandDetails(data);
        //        adangal.Parappu = par.Trim().Replace(" ", "").Replace("-", ".");
        //        adangal.Theervai = thee;
        //        adangal.LandType = lt;
        //        //adangal.Anupathaarar = $"{pattaEn}-{ownerName}";
        //        adangal.OwnerName = ownerName;
        //        adangal.PattaEn = pattaEn.ToInt32();
        //        adangal.LandStatus = LandStatus.Added;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
        //    }

        //    return adangal;
        //}
        //private (LandType lt, string par, string thee) GetLandDetails(List<string> data)
        //{
        //    LandType ld = LandType.Zero;
        //    var parappu = "";
        //    var theervai = "";
        //    try
        //    {
        //        int i = 0;
        //        if (data[2] != "--" && data[3] != "--")
        //        {
        //            ld = LandType.Punsai;
        //            parappu = data[2];
        //            theervai = data[3];
        //            i += 1;
        //        }
        //        if (data[4] != "--" && data[5] != "--")
        //        {
        //            ld = LandType.Nansai;
        //            parappu = data[4];
        //            theervai = data[5];
        //            i += 1;
        //        }
        //        if (data[6] != "--" && data[7] != "--")
        //        {
        //            ld = LandType.Maanaavari;
        //            parappu = data[6];
        //            theervai = data[7];
        //            i += 1;
        //        }
        //        if (i > 1)
        //        {
        //            ld = LandType.Zero;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
        //    }

        //    return (ld, parappu, theervai);
        //}
        public static void LogMessage(string message)
        {
            try
            {
                if (logHelper != null)
                    logHelper.WriteAdangalLog(message);
            }
            catch (Exception ex)
            {

                MessageBox.Show("erro" + ex.ToString());
            }


        }

        private void LogError(string message)
        {
            try
            {
                MessageBox.Show(message);
                if (logHelper != null)
                    logHelper.WriteAdangalLog(message);
            }
            catch (Exception ex)
            {

                MessageBox.Show("erro" + ex.ToString());
            }


        }

        private void chkEdit_CheckedChanged(object sender, EventArgs e)
        {
            waitForm.Show(this);
            var visibility = !chkEdit.Checked;

            dataGridView1.Columns["CorrectNameRow"].DisplayIndex = 0;
            GridColumnVisibility(visibility);
            waitForm.Close();
        }

        private void GridColumnVisibility(bool visibility)
        {
            dataGridView1.Columns["NilaAlavaiEn"].Visible = visibility;
            dataGridView1.Columns["UtpirivuEn"].Visible = visibility;
            dataGridView1.Columns["Parappu"].Visible = visibility;
            dataGridView1.Columns["Theervai"].Visible = visibility;
            dataGridView1.Columns["Anupathaarar"].Visible = visibility;
            dataGridView1.Columns["LandType"].Visible = visibility;
            dataGridView1.Columns["IsFullfilled"].Visible = visibility;
        }


        private string GetCertifiedContent()
        {
            var content = FileContentReader.CertifiedContent;
            content = content.Replace("[vattam]", loadedFile.VattamNameTamil)
                                .Replace("[firka]", loadedFile.FirkaName)
                                .Replace("[varuvaikiraamam]", loadedFile.VillageNameTamil)
                                .Replace("[pasali]", pasali.ToString())
                                .Replace("[totalpages]", pageNumber.ToString())
                                .Replace("[last-pasali]", prepasali.ToString());

            return content;

        }


        private void btnLoadProcessed_Click(object sender, EventArgs e)
        {
            if (ddlProcessedFiles.SelectedIndex == 0) return;

            waitForm.Show(this);
            LogMessage($"STEP-2 - Started - Existing file Load");


            fullAdangalFromjson = DataAccess.GetActiveAdangal();
            LogMessage($"READED DATA FROM EXISTING JSON FILE");
            dataGridView1.DataSource = fullAdangalFromjson;
            LoadSurveyAndSubdiv();
            PreLoadFile();
            CalculateTotalPages(recordPerPage);

            //if (canAddMissedSurvey)
            //{
            //    LogMessage($"STEP-2 - LOADING JUST ADANGAL FOR ADDING NEW SURVEY ONLY.");
            //}
            //else
            //{
            //    pattaList = DataAccess.GetPattaList();
            //    WholeLandList = DataAccess.GetWholeLandList();

            //    ProcessFullReport();
            //    LogMessage($"STEP-2 - Completed - Existing file Load");
            //}

            LogMessage($"STEP-2 - Completed - Existing file Load");
            EnableReadyForExist();

            waitForm.Close();
        }

        private void ddlProcessedFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProcessedFiles.SelectedIndex == 0)
            {
                btnLoadProcessed.Enabled = false;
                return;
            }

            // Load Basic Details
            //loadedFile = (ddlProcessedFiles.SelectedItem as LoadedFileDetail);
            UpadteLogPath(ddlProcessedFiles.SelectedItem.ToString());

            btnLoadProcessed.Enabled = true;
            SetVillage();
        }

        private void ddlListType_DataSourceChanged(object sender, EventArgs e)
        {
            ddlListType.Enabled = isTestingMode;
        }

        private void btnBookView_Click(object sender, EventArgs e)
        {

        }

        private int PgSize = 0;
        private int CurrentPageIndex = 1;
        private int TotalPage = 0;
        private int startPage = 7;

        private void CalculateTotalPages(int rec)
        {
            if (fullAdangalFromjson != null)
            {
                PgSize = rec;
                int rowCount = fullAdangalFromjson.Count;
                TotalPage = (rowCount / PgSize);
                // if any row left after calculated pages, add one more page 
                if (rowCount % PgSize > 0)
                    TotalPage += 1;
            }


        }

        private List<Adangal> GetCurrentRecords(int page)
        {
            List<Adangal> curentRecords;

            if (page == 1)
            {
                curentRecords = fullAdangalFromjson.Take(PgSize).ToList();
            }
            else
            {
                int PreviousPageOffSet = (page - 1) * PgSize;
                curentRecords = fullAdangalFromjson.Skip(PreviousPageOffSet).Take(PgSize).ToList();
            }

            lblPageNo.Text = $"பக்கம்: {page + 6} / {TotalPage + 6}";

            return curentRecords;
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            if (fullAdangalFromjson == null) return;
            CurrentPageIndex = 1;
            dataGridView1.DataSource = GetCurrentRecords(CurrentPageIndex);

        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (fullAdangalFromjson == null) return;
            if (CurrentPageIndex < TotalPage)
            {
                CurrentPageIndex++;
                dataGridView1.DataSource = GetCurrentRecords(CurrentPageIndex);
            }

        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (fullAdangalFromjson == null) return;
            if (CurrentPageIndex > 1)
            {
                CurrentPageIndex--;
                dataGridView1.DataSource = GetCurrentRecords(CurrentPageIndex);
            }

        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            if (fullAdangalFromjson == null) return;
            CurrentPageIndex = TotalPage;
            dataGridView1.DataSource = GetCurrentRecords(CurrentPageIndex);

        }

        private void btnGoToPage_Click(object sender, EventArgs e)
        {
            if (fullAdangalFromjson == null) return;
            dataGridView1.DataSource = GetCurrentRecords(txtGoto.Text.ToInt32() - 6);
        }

        private void txtRecCount_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPages(txtRecCount.Text.ToInt32());
        }


        private void SortMissedSurveys()
        {
            var dc = (cmbItemToBeAdded.DataSource as List<string>);
            List<string> data;

            if (SortByDesc)
                data = dc.OrderByDescending(o => o.Split('~')[0].ToInt32()).ThenBy(o => o.Split('~')[1], new AlphanumericComparer()).ToList();
            else
                data = dc.OrderBy(o => o.Split('~')[0].ToInt32()).ThenBy(o => o.Split('~')[1], new AlphanumericComparer()).ToList();

            cmbItemToBeAdded.DataSource = null;
            cmbItemToBeAdded.DataSource = data;
        }

        bool SortByDesc = false;
        private void btnSwap_Click(object sender, EventArgs e)
        {
            SortByDesc = !SortByDesc;
            SortMissedSurveys();
        }

        //private void btnSyncNew_Click(object sender, EventArgs e)
        //{
        //    var selectedFiles = General.SelectFilesInDialog(Directory.GetParent(DataAccess.MissedAdangalPath).FullName, "MissedAdangal");
        //    int addedCount = 0;
        //    int existCount = 0;
        //    selectedFiles.ForEach(fe =>
        //    {

        //        var missedJsonData = DataAccess.GetMissedAdangal(fe);
        //        missedJsonData.ForEach(loop =>
        //        {

        //            if (DataAccess.AddNewAdangal(loop))
        //            {
        //                addedCount += 1;
        //            }
        //            else
        //            {
        //                LogMessage($"Adangal already exist for sur-no: {loop.NilaAlavaiEn} subdiv-no: {loop.UtpirivuEn}");
        //                existCount += 1;
        //            }

        //        });
        //    });

        //    MessageBox.Show($"Added: {addedCount}{Environment.NewLine}Exist Not Added: {existCount}");

        //}

        private void btnSoftGen_Click(object sender, EventArgs e)
        {
            LogMessage($"STEP-5 - Soft Copy Generate Started");

            if (pcEnabled)
            {
                if (IsUserAuthenticated() == false) return;
            }

            try
            {
                LogMessage($"STARTED HTML GENERATION @ {DateTime.Now.ToLongTimeString()}");
                pageNumber = 6;

                StringBuilder allContent = new StringBuilder();
                pageTotalList = new List<PageTotal>();
                pageTotal2List = new List<PageTotal>();
                pageTotal3List = new List<PageTotal>();

                var mainHtml = FileContentReader.MainHtml;
                string initialPages = GetInitialPagesSoftCopy();

                mainHtml = mainHtml.Replace("[initialPages]", initialPages);

                var landTypeGroup = (from wl in fullAdangalFromjson
                                     where wl.LandType != LandType.Zero
                                     group wl by wl.LandType into newGrp
                                     select newGrp).ToList();

                var rowTemplate22 = FileContentReader.LeftPageRowTemplate;
                var totalTemplate22 = FileContentReader.LeftPageTotalTemplate;
                var tableTemplate22 = FileContentReader.LeftPageTableTemplateSoftCopy;

                landTypeGroup.ForEach(fe =>
                {
                    var dataToProcess = fe.ToList();

                    var pageCount = dataToProcess.Count / recordPerPage;
                    if (dataToProcess.Count % recordPerPage > 0) pageCount += 1;
                    var landType = fe.Key.ToName();
                    string pageRange = "";

                    if (isTestingMode)
                    {
                        pageCount = pageCount >= TestingPage ? TestingPage : pageCount;
                    }

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
                        var totalparappu = AdangalFn.GetSumThreeDotNo(temData.Select(s => s.Parappu).ToList());
                        //var totalTheervai = temData.Sum(s => Convert.ToDecimal(s.Theervai));

                        decimal totalTheervai = 0;
                        if (fe.Key != LandType.Porambokku)
                            totalTheervai = temData.Sum(s => Convert.ToDecimal(s.Theervai));

                        var total = totalTemplate.Replace("[moththaparappu]", totalparappu).Replace("[moththatheervai]", totalTheervai == 0 ? "" : totalTheervai.ToString());

                        leftPage = leftPage.Replace("[totalrow]", total);
                        leftPage = leftPage.Replace("[landtype]", landType);
                        leftPage = leftPage.Replace("[header]", updatedHeader);
                        pageNumber += 1;
                        leftPage = leftPage.Replace("[pageNo]", pageNumber.ToString());


                        allContent.Append(leftPage);
                        //allContent.Append(GetRightEmptyPage()); // right page

                        if (i == 0)
                        {
                            pageRange = $"{pageNumber}-";
                        }
                        if (i == pageCount - 1)
                        {
                            pageRange += $"{pageNumber}";
                            pageRangeList.Add(new KeyValue() { Id = (int)fe.Key, Caption = pageRange });
                        }

                        pageTotalList.Add(new PageTotal()
                        {
                            PageNo = pageNumber,
                            ParappuTotal = totalparappu,
                            TheervaiTotal = totalTheervai,
                            LandType = fe.Key
                        });
                    }
                });

                //allContent.Append(GetEmptyPages(1));  // add 4 empty pages.
                allContent.Append(GetPageTotal(pageTotalList, pageTotal2List, true));
                if (isTestingMode == false)
                {
                    allContent.Append(GetPageTotal(pageTotal2List, pageTotal3List, true));
                    SetSummaryPageDetails(pageTotal3List);
                    //allContent.Append(GetOverallTotal(pageTotal3List));
                }
                else
                {
                    SetSummaryPageDetails(pageTotal2List);
                    //allContent.Append(GetOverallTotal(pageTotal2List));
                }

                allContent.Append(GetSummaryPage(isSoftCopy: true));
                int FinalEmptyPages = Convert.ToInt32(ConfigurationManager.AppSettings["FinalEmptyPages"]);
                //allContent.Append(GetEmptyPages(FinalEmptyPages));

                // Final Touch
                mainHtml = mainHtml.Replace("[summaryPages]", GetSummaryPage(true, isSoftCopy: true));
                mainHtml = mainHtml.Replace("[building]", GetGovtBuildingPage());
                mainHtml = mainHtml.Replace("[allPageData]", allContent.ToString());
                mainHtml = mainHtml.Replace("[certifed]", GetCertifiedContent());

                var fPath = AdangalConstant.CreateAndReadPath($"{loadedFile.VillageName}-Result");
                var filePath = Path.Combine(fPath, $"{loadedFile.VillageName}-{DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss")}.htm");
                File.AppendAllText(filePath, mainHtml);

                LogMessage($"COMPLETED HTML GENERATION @ {filePath}");
                Process.Start(filePath);

                LogMessage($"STEP-5 - Generate Completed");
            }
            catch
            {

            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            //if (dataGridView1.SelectedRows.Count == 1 && e.Button == MouseButtons.Right)
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip strip = new ContextMenuStrip();

                var selectedRow = dataGridView1.SelectedRows;

                List<Adangal> selectedAdangal = new List<Adangal>();

                foreach (DataGridViewRow item in selectedRow)
                {
                    selectedAdangal.Add((Adangal)item.DataBoundItem);
                }

                strip.Tag = selectedAdangal;
                strip.Items.Add("ReCreate").Name = "ReCreateName";
                strip.Items.Add("Delete").Name = "DeleteName";
                strip.Show(dataGridView1, new Point(e.X, e.Y));

                strip.ItemClicked += Strip_ItemClicked;

            }
        }

        private void Strip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var txn = (List<Adangal>)((ContextMenuStrip)sender).Tag;

            var kv = new List<KeyValue>(); // { Value = txn.NilaAlavaiEn, Caption = txn.UtpirivuEn };

            txn.ForEach(fe =>
            {
                kv.Add(new KeyValue() { Value = fe.NilaAlavaiEn, Caption = fe.UtpirivuEn });
            });


            var list = new List<KeyValue>(kv); // { kv };

            if (e.ClickedItem.Name == "ReCreateName")
            {
                AdangalConverter.ProcessAdangal(list, true);
            }
            else if (e.ClickedItem.Name == "DeleteName")
            {
                waitForm.Show(this);
                DataAccess.DeleteAdangalFile(txn);
                dataGridView1.DataSource = DataAccess.GetActiveAdangal();
                waitForm.Close();
            }


        }

        private void btnLoadFirstTIme_Click(object sender, EventArgs e)
        {

            //var list = new List<KeyValue>() { 
            //    new KeyValue() { Value = 66, Caption = "2E" } ,
            //    new KeyValue() { Value = 37, Caption = "2E" }
            //};

            List<KeyValue> list = DataAccess.GetSubdiv();
            AdangalConverter.ProcessAdangal(list);
        }
    }

}