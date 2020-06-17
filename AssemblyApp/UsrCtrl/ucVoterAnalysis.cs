using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucVoterAnalysis : UserControl
    {

        List<VoterList> fullList = new List<VoterList>();
        string folderPath = @"F:\NTK\VotersAnalysis\";
        string docPath = @"F:\NTK\VotersAnalysis\VoterList\doc";


        string reProcessFile = "";
        BoothDetail bd;
        bool haveErrorinFile = false;
        int lastPageNumberToProcess;
        List<string> logs;
        string voterFilePath = "";
        string boothDetailPath = "";


        public ucVoterAnalysis()
        {
            InitializeComponent();
            LoadBooths();
        }




        private void button1_Click(object sender, EventArgs e)
        {
            cmbFIlter.DataSource = GetOptions();
            reProcessFile = "";

            //int year = 2019;
            //var filePath = $@"{folderPath}ac210333.txt";

            var allFiles = Directory.GetFiles(docPath).ToList();

            int year = 2020;

            foreach (var item in allFiles)
            {
                string partNo = "";
                string assNo = "";

                var filePath = item; // $@"{folderPath}ac211200.txt";

                try
                {
                    var ufn = (new FileInfo(filePath)).Name.Split('.')[0];

                    assNo = ufn.Substring(2, 3);
                    partNo = ufn.Substring(5, 3);

                    voterFilePath = Path.Combine(AppConfiguration.AssemblyVotersFolder, $"{assNo}");

                    boothDetailPath = Path.Combine(voterFilePath, $"{assNo}-BoothDetail.json");



                    voterFilePath = Path.Combine(voterFilePath, $"{assNo}-{partNo}.json");

                    this.cmbFIlter.SelectedIndexChanged += new System.EventHandler(this.cmbFIlter_SelectedIndexChanged);

                    //if (File.Exists(voterFilePath) == false)
                    //{
                    //    //File.Create(voterFilePath);
                    //}
                    //else
                    //{
                    //    if (chkDebugMode.Checked == false)
                    //    {
                    //        MessageBox.Show("Willload an existing data!!");
                    //        // Load and exit
                    //        fullList = VoterList.GetAll(voterFilePath);
                    //        dataGridView1.DataSource = fullList;
                    //        //this.cmbFIlter.SelectedIndexChanged += new System.EventHandler(this.cmbFIlter_SelectedIndexChanged);
                    //        this.cmbFIlter.SelectedIndex = 8; // may error.
                    //        SetErrorDetail();
                    //        return;
                    //    }
                    //}

                }
                catch (Exception ex)
                {
                    General.WriteLog($"Error in FileName - {ex.ToString()}", assNo, partNo, 0);
                    //MessageBox.Show("Invalid file name");
                }



                var allPageContent = File.ReadAllText(filePath);

                //Process First page
                var firstPage = allPageContent.Substring(0, allPageContent.IndexOf("சட்டமன்றத் தொகுதி எண் மற்றும் பெயர்"));


                try
                {


                    // First Page Details
                    List<string> toReplace = new List<string>()
                            {
                                "வாக்காளர் பட்டியல்",
                                "சட்டமன்றத் தொகுதி எண்",
                                "பாகம் எண்",
                                "சட்டமன்றத் தொகுதி அடங்கியுள்ள நாடாளுமன்றத் தொகுதியின் எண்",
                                "திருத்தப்படும் ஆண்டு",
                                "தகுதியேற்படுத்தும் நாள்",
                                "பட்டியல் வகை",
                                "பாகத்தின் விவரங்கள்",
                                "வாக்குச் சாவடியின் விவரங்கள்",
                                "வாக்குச்சாவடியின் எண் மற்றும் பெயர்",
                                "வாக்குச்சாவடியின் வகைப்பாடு",
                                "தொடங்கும் வரிசை எண்",
                                "முடியும் வரிசை எண்"
                            };

                    toReplace.ForEach(fe =>
                      {
                          firstPage = firstPage.Replace(fe, $"${fe}");
                      });


                    var fpSPlitted = firstPage.Split('$');

                    bd = new BoothDetail();

                    if (year == 2020)
                    {
                        if (fpSPlitted[2].Split(':').Count() > 1)
                        {
                            var assembly2020 = fpSPlitted[2].Split(':')[1].Trim().Split('-');
                            bd.AssemblyNo = assembly2020[0].Trim();
                            bd.AssemblyName = assembly2020[1];
                        }
                        else
                        {
                            var assembly2020 = fpSPlitted[2].Split('-');
                            bd.AssemblyNo = assembly2020[1].Trim();
                            bd.AssemblyName = assembly2020[2].Trim();

                        }

                    }
                    else
                    {
                        var assembly2019 = fpSPlitted[2].Split('-');
                        bd.AssemblyNo = assembly2019[1].Trim();
                        bd.AssemblyName = assembly2019[2];
                    }


                    bd.PartNo = fpSPlitted[3].Split(' ')[2].Trim();

                    if (string.IsNullOrEmpty(bd.PartNo)) bd.PartNo = partNo;

                    if (year == 2020)
                    {
                        var parliment = fpSPlitted[4].Split('-');
                        bd.ParlimentNo = parliment[1];
                        bd.ParlimentName = parliment[2].Trim().Split('1')[0].Trim();
                    }
                    else
                    {
                        var parliment2019 = fpSPlitted[4].Split(':')[1].Trim().Split('-');
                        bd.ParlimentNo = parliment2019[0].Trim();
                        bd.ParlimentName = parliment2019[1].Trim().Split('1')[0].Trim();
                    }

                    bd.EligibilityDay = fpSPlitted[6].Split(' ')[2].Trim();
                    bd.ReleaseDate = fpSPlitted[7].Replace("பட்டியல் வெளியிடப்பட்ட நாள்", "$").Split('$')[1].Split(' ')[1];

                    if (year == 2020)
                    {
                        if (fpSPlitted[8].Split('-').Count() > 2)
                        {
                            bd.PartPlaceName = fpSPlitted[8].Split('-')[2].Replace("பிரிவின் எண் மற்றும் பெயர்", "$").Split('$')[1].Trim();
                        }
                        else
                        {
                            bd.PartPlaceName = fpSPlitted[8].Split('-')[0].Replace("பிரிவின் எண் மற்றும் பெயர்", "$").Split('$')[1].Replace("999. அயல்நாடு வாழ் வாக்காளர்கள்", "$").Split('$')[0];
                        }
                    }
                    else
                    {
                        bd.PartPlaceName = fpSPlitted[8].Split('-')[0].Replace("பிரிவின் எண் மற்றும் பெயர்", "$").Split('$')[1].Trim();
                    }


                    var colonCount = fpSPlitted[8].Count(c => c == ':');


                    try
                    {
                        var otherDetails = fpSPlitted[8].Split('-')[5].Split(' ');
                        if (year == 2020)
                        {
                            bd.MainCityOrVillage = otherDetails[2];
                            bd.Zone = otherDetails[3].Trim();
                            bd.Birga = otherDetails[5];
                            bd.PoliceStation = otherDetails[6];
                            bd.Taluk = otherDetails[7];
                            bd.District = fpSPlitted[8].Split('-')[6].Trim();
                            bd.Pincode = fpSPlitted[8].Split('-')[12].Split(' ')[2].ToInt32();
                        }
                        else
                        {
                            bd.MainCityOrVillage = fpSPlitted[8].Split('-')[2].Split(' ')[10].Trim();
                            bd.Zone = fpSPlitted[8].Split('-')[3].Trim();
                            bd.Birga = fpSPlitted[8].Split('-')[4].Trim();
                            bd.PoliceStation = fpSPlitted[8].Split('-')[5].Split(' ')[1];
                            bd.Taluk = fpSPlitted[8].Split('-')[5].Split(' ')[2].Trim();
                            bd.District = fpSPlitted[8].Split('-')[5].Split(' ')[3].Trim();
                            bd.Pincode = fpSPlitted[8].Split('-')[5].Split(' ')[4].Trim().ToInt32();
                        }

                    }
                    catch (Exception ex)
                    {

                    }



                    bd.Type = fpSPlitted[9].Replace("வாக்குச் சாவடியின் விவரங்கள்", "").Trim();

                    //bd.PartLocationAddress = fpSPlitted[11].Replace("எண்ணிக்கை", "$").Split('$')[1].Split('4')[0].Trim();

                    try
                    {
                        bd.StartNo = fpSPlitted[12].Replace("தொடங்கும் வரிசை எண்", "").Trim().ToInt32();
                    }
                    catch (Exception)
                    {

                        bd.StartNo = 1;
                    }


                    var voteDetails = fpSPlitted[13];

                    List<string> toReplaceVoteDetail = new List<string>()
            {
                "முடியும் வரிசை எண்",
                "நிகர வாக்காளர்களின் எண்ணிக்கை",
                "பெண் மூன்றாம் பாலினம்",
                "மொத்தம்"
            };

                    toReplaceVoteDetail.ForEach(fe =>
                                {
                                    voteDetails = voteDetails.Replace(fe, $"$");

                                });

                    var splitVoter = voteDetails.Split('$');
                    var forNo = splitVoter[1].Split(' ');

                    bd.EndNo = forNo[1].Trim().ToInt32();

                    if (year == 2020)
                        try
                        {
                            bd.Male = forNo[4].ToInt32();
                        }
                        catch (Exception)
                        {

                            bd.Male = forNo[3].ToInt32();
                        }

                    else
                        bd.Male = forNo[3].ToInt32();

                    if (year == 2020)
                    {
                        var genderVotes = splitVoter[3].Split(' ');
                        bd.TotalVoters = splitVoter[4].Split(' ')[1].Trim().ToInt32();
                        bd.Female = genderVotes[1].Trim().ToInt32();
                        bd.ThirdGender = genderVotes[2].Trim().ToInt32();
                    }
                    else
                    {
                        var genderVotes = splitVoter[4].Split(' ');
                        bd.TotalVoters = splitVoter[1].Split(' ')[1].Trim().ToInt32();
                        bd.Female = genderVotes[1].Trim().ToInt32();
                        bd.ThirdGender = genderVotes[2].Trim().ToInt32();
                    }

                    /*************************************************************/

                    General.CreateFileIfNotExist(boothDetailPath);
                    BoothDetail.Save(bd, boothDetailPath);

                    /*************************************************************/

                }
                catch (Exception ex)
                {
                    General.WriteLog($"Error in First Page", assNo, partNo, 1);
                }



                //reProcessFile = $"{folderPath}{bd.AssemblyNo}\\{bd.PartNo}";

                //var lastPage = allPageContent.Substring(allPageContent.IndexOf("வாக்காளர்களின் தொகுப்பு"));

                var onlyVotersPages = allPageContent.Substring(allPageContent.IndexOf("பக்கம் 2"), allPageContent.IndexOf("வாக்காளர்களின் தொகுப்பு") - allPageContent.IndexOf("பக்கம் 2"));

                var totalPages = firstPage.Substring(firstPage.IndexOf("மொத்த பக்கங்கள்"), firstPage.IndexOf("பக்கம்") - firstPage.IndexOf("மொத்த பக்கங்கள்"));

                var NoOfpagesToProcess = totalPages.Replace("மொத்த பக்கங்கள்", "").Replace("-", "").Trim().ToInt32() - 3; // -3 means - not consider page 1, page 2 and last page

                lastPageNumberToProcess = NoOfpagesToProcess + 2;

                logs = new List<string>();

                bool isProcessed = true;
                for (int i = 0; i < NoOfpagesToProcess; i++)
                {

                    // processing page number
                    var pageNumber = i + 3;

                    if (chkDebugMode.Checked)
                    {
                        if (pageNumber != txtPage.Text.ToInt32())
                        {
                            continue;
                        }
                    }

                    var startIndex = IndexOf(onlyVotersPages, pageNumber - 1); //onlyVotersPages.IndexOf($"பக்கம் {i + 2}");
                    var lastIndex = IndexOf(onlyVotersPages, pageNumber) - startIndex;

                    var pageContent = onlyVotersPages.Substring(startIndex, lastIndex);


                    if (ProcessPage(pageNumber, pageContent, assNo, partNo) == true)
                    {
                        haveErrorinFile = true;
                    }
                    else
                    {
                        General.WriteLog($"Error in PageProcess", assNo, partNo, pageNumber);
                        isProcessed = false;
                        break;
                    }


                }

                if (isProcessed == false)
                    continue;


                string flag = "OK";
                //LOG FILE IF EXCEPTION OCCURS
                //if (logs.Count > 0 || haveErrorinFile)
                //{
                //    var file = Path.Combine(reProcessFile, $"Log-{bd.AssemblyNo}-{bd.PartNo}-Exception.txt");

                //    var delData = logs.Where(w => w.StartsWith("DEL-")).Distinct().Reverse().ToList();
                //    var errData = logs.Where(w => w.StartsWith("DEL-") == false).ToList();

                //    StringBuilder logText = new StringBuilder();
                //    logText.AppendLine("==================DELETED RECORD========================================");
                //    delData.ForEach(fe => logText.AppendLine(fe.Trim()));
                //    logText.AppendLine("==================ERROR RECORD========================================");
                //    errData.ForEach(fe => logText.AppendLine(fe));


                //    General.WriteToFile(file, logText.ToString());
                //    flag = "ERRORFILE";
                //}

                var file4 = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{DateTime.Now.ToLocalTime().ToString().Replace(":", "~")}-{flag}.txt");
                General.WriteToFile(file4, onlyVotersPages);

                if (chkDebugMode.Checked == false) // We should save ony in run modeNOT IN DEBUG MODE.
                {
                    //if (DialogResult.Yes == MessageBox.Show($"You want Save for {assNo}-{partNo}", "", MessageBoxButtons.YesNo))
                    //{


                    if (File.Exists(voterFilePath) == false)
                    {
                        var myFile = File.Create(voterFilePath);
                        myFile.Close();
                    }
                    else
                    {
                        File.Delete(voterFilePath);
                    }

                    VoterList.Save(fullList, voterFilePath);



                    //}
                }

                logs.Clear();

                // PROCESSED WHOLE JSON FILE.
                //var fd = JsonConvert.SerializeObject(fullList, Formatting.Indented);
                //var file3 = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{DateTime.Now.ToLocalTime().ToString().Replace(":", "~")}.json");
                //General.WriteToFile(file3, fd);

                

                // Save into voter perc File
                // var dataSou = new List<KeyValuePair<string, string>>();

                var maleCount = fullList.Where(w => w.Sex.Trim().Split(' ')[0].Trim() == "ஆண்").Count();
                var femaleCount = fullList.Where(w => w.Sex.Trim().Split(' ')[0].Trim() == "பெண்").Count();

                var allAges = fullList.Select(s => s.Age).ToList();
                var allC = fullList.Select(s => s.Age).ToList().Count();

                var twenty = GetLessAgeCOunt(allAges, 20);
                var thirty = GetAgeCOunt(allAges, 21, 30);
                var forty = GetAgeCOunt(allAges, 31, 40);
                var fifty = GetAgeCOunt(allAges, 41, 50);
                var sixty = GetAgeCOunt(allAges, 51, 60);
                var aboveSixty = GetMoreAgeCOunt(allAges, 61);

                var newBoothPerc = new VotePercDetail()
                {
                    AssemblyNo = assNo.ToInt32(),
                    BoothNo = partNo.ToInt32(),
                    Male = maleCount,
                    Female = femaleCount,
                    Third = 0,
                    MaleP = PercInDec(maleCount, allC),
                    FemaleP = PercInDec(femaleCount, allC),
                    ThirdP = 0,
                    to20 = PercInDec(twenty, allC),
                    to30 = PercInDec(thirty, allC),
                    to40 = PercInDec(forty, allC),
                    to50 = PercInDec(fifty, allC),
                    to60 = PercInDec(sixty, allC),
                    Above60 = PercInDec(aboveSixty, allC)
                };

                VotePercDetail.Save(newBoothPerc);

                fullList.Clear();

            }

            MessageBox.Show("ALL DONE!");

            dataGridView1.DataSource = fullList;

            //var st = new StringBuilder();

            //var er = (from f in fullList.Where(w => w.NameError || w.GenderError).ToList()
            //          group f by $"{f.PageNo}-{f.RowNo}" into ng
            //          select ng.Key).ToList();



            // fullList.ForEach(fe => st.AppendLine(fe.ToString()));
            //fullList.Where(w => w.ErrorType == ErrorType.BREAK1).ToList().ForEach(fe => st.AppendLine(fe.ToString()));

            //fullList.Where(w => er.Contains($"{w.PageNo}-{w.RowNo}")).ToList().ForEach(fe => st.AppendLine(fe.ToString()));


            var errorPages = (from fl in fullList
                              where fl.MayError
                              group fl by fl.PageNo into ng
                              select "Page-" + ng.Key.ToString()).ToList();

            errorPages.Insert(0, "--SELECT PAGE--");

            chkPageList.DataSource = errorPages;
            SetErrorDetail();

        }

        private void SetErrorDetail()
        {
            var errorPerc = fullList.Where(w => string.IsNullOrEmpty(w.Name.Trim())).Count();
            errorPerc += fullList.Where(w => string.IsNullOrEmpty(w.HorFName.Trim())).Count();
            errorPerc += fullList.Where(w => string.IsNullOrEmpty(w.HomeAddress.Trim())).Count();
            errorPerc += fullList.Where(w => string.IsNullOrEmpty(w.HorFName.Trim())).Count();
            errorPerc += fullList.Where(w => w.Age == 0).Count();
            errorPerc += fullList.Where(w => w.MayError).Count();


            lblDetails.Text = $"Total Records:{fullList.Count} {Environment.NewLine} " +
                $"Error Records:{errorPerc} {Environment.NewLine} " +
                $"Error - {Math.Round((Convert.ToDecimal(errorPerc) / Convert.ToDecimal(fullList.Count) * 100), 2)}%";
        }

        public (bool, string, bool) GetName(string content)
        {
            try
            {
                string d = "";

                if (content.Contains(":"))
                    d = content.Split(':')[1];
                else
                    d = content.Split(' ')[1];

                var isEmpty = string.IsNullOrEmpty(d);

                return (true, d.Replace("\r\n", "*"), isEmpty);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString(), false);
            }
        }

        public (bool, string, bool) GetFNname(string content)
        {
            try
            {
                string d = content.Contains(":") ? content.Split(':')[1] : content.Split(' ')[1];
                var isEmpty = string.IsNullOrEmpty(d.Trim()); // || !d.Contains('-');
                return (true, d.Replace("\r\n", "*"), isEmpty);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString(), false);
            }
        }

        public (bool, string, bool) GetAge(string content)
        {
            try
            {
                string d = content.Contains(":") ? content.Split(':')[1] : content.Split(' ')[1];
                var isEmpty = string.IsNullOrEmpty(d);
                return (true, d, isEmpty);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString(), false);
            }
        }

        public (bool, string, bool, bool) GetAddress(string content)
        {
            bool isDel = false;
            try
            {
                string d = content.Contains(":") ? content.Split(':')[1] : content.Split(' ')[1];

                isDel = content.Contains("ADDRESS-D");

                var isEmpty = string.IsNullOrEmpty(d);
                return (true, d.Replace("\r\n", "*"), isEmpty, isDel);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString(), false, isDel);
            }
        }

        public (bool, string, bool) GetGender(string content)
        {
            try
            {
                //var d = content.Contains(":") ?
                //        content.Split(':')[1].TrimStart().Split(' ')[0] : content.Split(' ')[1];

                var d = content.Contains(":") ?
                        content.Split(':')[1] : content.Split(' ')[1];


                var isEmpty = string.IsNullOrEmpty(d);
                return (true, d.Replace("\r\n", "*"), isEmpty);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString(), false);
            }
        }



        private string AddNameLog(int pn, string name)
        {
            //sb.AppendLine($"PageNo:{pn}-{name}");
            logs.Add($"PageNo:{pn}-{name}");
            return $"{pn}-{name}";
        }

        private string AddLog(int pn, VoterList vl)
        {
            //sb.AppendLine($"PageNo:{vl.PageNo}-RowNo:{vl.RowNo}-{vl}");
            logs.Add($"PageNo:{vl.PageNo}-RowNo:{vl.RowNo}-{vl}");
            return $"{pn}-{vl}";
        }

        private string AddDeleteItemLog(int pn, VoterList vl)
        {
            logs.Insert(0, $"DEL-PageNo:{vl.PageNo}-RowNo:{vl.RowNo}-{vl.Name}");
            return $"{pn}-{vl}";
        }


        private int IndexOf(string allContent, int pageNo)
        {
            return allContent.IndexOf($"பக்கம் {pageNo}");
        }

        private void chkDebugMode_CheckedChanged(object sender, EventArgs e)
        {
            txtPage.Enabled = txtRow.Enabled = chkDebugMode.Enabled;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //if (chkPageList.SelectedIndex == 0)
            //{
            //    MessageBox.Show("Select page to reprocess");
            //    return;
            //}

            //if (string.IsNullOrEmpty(txtMissingRow.Text.Trim()))
            //{
            //    MessageBox.Show("Provide the deleted row number");
            //    return;
            //}

            //var selectedPage = chkPageList.SelectedValue.ToString().Split('-')[1].Trim().ToInt32();
            ////Page - 6
            //if (DialogResult.Yes ==
            //MessageBox.Show($"You want to reprocess for Assembly - {bd.AssemblyName} Booth - {bd.PartNo} PageNo {selectedPage}", "", MessageBoxButtons.YesNoCancel))
            //{
            //    var pageText = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{selectedPage}-{31}.txt");

            //    var text = File.ReadAllText(pageText);

            //    //if(ProcessPage(selectedPage, text, txtMissingRow.Text.ToInt32()))
            //    //{
            //    //    var jsonFile = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{selectedPage}-{lastPageNumberToProcess}-ReRun-Failed.json");
            //    //    var fd = JsonConvert.SerializeObject(voterList, Formatting.Indented);
            //    //    WriteToFile(jsonFile, fd);
            //    //}
            //    ProcessPage(selectedPage, text, txtMissingRow.Text.ToInt32());

            //}

        }

        string data = "";
        private bool ProcessPage(int pageNumber, string pageContent, string assNum, string partNum, int errorRowNumber = 0)
        {

            try
            {


                // processing page number

                /*
                 * ONE DELETE
                 * NO DELETE BUT DATA SWAPPED OR MISARRANGED
                 * MORE THAN ONE DELETE? not yet done.
                 */


                data = pageContent;

                data = data.Replace("சட்டமன்றத் தொகுதி எண் மற்றும் பெயர்", "*");
                data = data.Replace("பிரிவு எண் மற்றும் பெயர்", "*");

                bool mayHaveError = false;

                var recordCount = (from p in data.Split(' ').ToList()
                                   where p.Contains("Photo")
                                   select p).ToList().Count;

                data = data.Replace("Photo", "").Replace("is", "").Replace("Available", "");  // Rempve Photo is AVailable.

                data = data.Replace("தந்தை பெயர்", "$FATHER")
                           .Replace("கணவர் பெயர்", "$FATHER")
                           //.Replace("கணவர் பெய", "$HUSBAND-1:") // scenerio 1
                           .Replace("தாய் பெயர்", "$FATHER")
                           .Replace("இதரர் பெயர்", "$FATHER")
                           // NOTE: dont change this order of Replace
                           .Replace("பெயர்", "$NAME")  // Rempoe Photo is AVailable.

                           .Replace("வீட்டு எண்", "$ADDRESS")
                           .Replace("வீட்டு என்", "$ADDRESS-D")
                            .Replace("வீட்டு தன்", "$ADDRESS-D")
                            .Replace("வீட்டு பண்", "$ADDRESS-D")
                            .Replace("வீட்டு கண்", "$ADDRESS-D")
                            .Replace("வீட்டு தடை", "$ADDRESS-D")
                           //.Replace("வீட்டு", "$ADDRESS:D")

                           .Replace("வயது", "$AGE")

                           .Replace("பாலினம்", "$SEX");


                //data = data.Replace("தந்தை பெயர்", "$F")
                //           .Replace("கணவர் பெயர்", "$F")
                //           //.Replace("கணவர் பெய", "$HUSBAND-1:") // scenerio 1
                //           .Replace("தாய் பெயர்", "$F")
                //           .Replace("இதரர் பெயர்", "$F")
                //           // NOTE: dont change this order of Replace
                //           .Replace("பெயர்", "$N")  // Rempoe Photo is AVailable.

                //           .Replace("வீட்டு எண்", "$A")
                //           .Replace("வீட்டு என்", "$A")
                //            .Replace("வீட்டு தன்", "$A")
                //            .Replace("வீட்டு பண்", "$A")
                //            .Replace("வீட்டு கண்", "$A")
                //            .Replace("வீட்டு கண்", "$A")
                //           //.Replace("வீட்டு", "$ADDRESS:D")

                //           .Replace("வயது", "$G")

                //           .Replace("பாலினம்", "$X");

                var splittedData = data.Split('$').ToList();

                splittedData.RemoveRange(0, 1);

                // no of voters in a page
                var pageRecordCount = splittedData.Count; //


                var names = splittedData.Where(w => w.StartsWith("NAME")).ToList();

                var fatherOrHusband = splittedData.Where(
                                           w => w.StartsWith("FATHER") ||
                                           w.StartsWith("MOTHER") || w.StartsWith("HUSBAND") ||
                                           w.StartsWith("OTHERS")).ToList();

                var address = splittedData.Where(w => w.StartsWith("ADDRESS")).ToList();

                var age = splittedData.Where(w => w.StartsWith("AGE")).ToList();
                age.RemoveAt(age.Count - 1);

                var sex = splittedData.Where(w => w.StartsWith("SEX")).ToList();

                var isReProcess = (errorRowNumber > 0);
                var insertIndex = (errorRowNumber - 1);

                var isNameIssue = (names.Count != recordCount);
                var isFNameIssue = (fatherOrHusband.Count != recordCount);
                var isAddressIssue = (address.Count != recordCount);
                var isAgeIssue = (age.Count != recordCount);
                var isSexIssue = (sex.Count != recordCount);

                //if (isReProcess)
                //{
                //    if (isNameIssue) names.Insert(insertIndex, "NN");
                //    if (isFNameIssue) fatherOrHusband.Insert(insertIndex, "NF");
                //    if (isAddressIssue) address.Insert(insertIndex, "ND");
                //    if (isAgeIssue) age.Insert(insertIndex, "NG");
                //    if (isSexIssue) sex.Insert(insertIndex, "NS");

                //    isNameIssue = (names.Count != recordCount);
                //    isFNameIssue = (fatherOrHusband.Count != recordCount);
                //    isAddressIssue = (address.Count != recordCount);
                //    isAgeIssue = (age.Count != recordCount);
                //    isSexIssue = (sex.Count != recordCount);
                //}

                if (isNameIssue)
                    General.WriteLog("Error in Names", assNum, partNum, pageNumber);
                if (isFNameIssue)
                    General.WriteLog("Error in FatherNames", assNum, partNum, pageNumber);
                if (isAddressIssue)
                    General.WriteLog("Error in Address", assNum, partNum, pageNumber);
                if (isAgeIssue)
                    General.WriteLog("Error in Age", assNum, partNum, pageNumber);
                if (isSexIssue)
                    General.WriteLog("Error in Gender", assNum, partNum, pageNumber);


                /* NAME */
                if (isNameIssue || isFNameIssue || isAddressIssue || isAgeIssue || isSexIssue)
                {
                    mayHaveError = true;
                    var jsonFile = Path.Combine(docPath, bd.AssemblyNo, bd.PartNo, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}-Data.txt");
                    var jsonFileCon = Path.Combine(docPath, bd.AssemblyNo, bd.PartNo, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}-Content.txt");


                    //var fd = JsonConvert.SerializeObject(voterList, Formatting.Indented);
                    General.CreateFolderIfNotExist(new FileInfo(jsonFile).DirectoryName);
                    General.WriteToFile(jsonFile, data);
                    General.WriteToFile(jsonFileCon, pageContent);
                    return false;
                }

                if (isNameIssue)
                    AddNameLog(pageNumber, $"NAME COUNT MISMATCH - recordCount:{names.Count} - NAME:{recordCount}");

                /* FATHER-NAME */
                if (isFNameIssue)
                    AddNameLog(pageNumber, $"FATHER COUNT MISMATCH - recordCount:{names.Count} - FATHER:{fatherOrHusband.Count}");

                /* ADDRESS */
                if (isAddressIssue)
                    AddNameLog(pageNumber, $"ADDRESS COUNT MISMATCH - recordCount:{names.Count} - ADDRESS:{address.Count}");

                /* AGE */
                if (isAgeIssue)
                    AddNameLog(pageNumber, $"AGE COUNT MISMATCH - recordCount:{recordCount} - AGE:{age.Count}");

                /* SEX */
                if (isSexIssue)
                    AddNameLog(pageNumber, $"SEX COUNT MISMATCH - recordCount:{recordCount} - SEX:{sex.Count}");

                var voterList = new List<VoterList>();

                // 1. Name
                names.ForEach(fe =>
                {
                    var nm = GetName(fe);
                    var vl = new VoterList()
                    {
                        Name = nm.Item1 ? nm.Item2 : AddNameLog(pageNumber, "#NERROR#"),
                        MayError = mayHaveError ? mayHaveError : (nm.Item3)

                    };

                    if (vl.MayError)
                        AddNameLog(pageNumber, $"NAME ERROR @ {voterList.Count + 1}");

                    voterList.Add(vl);
                });

                sb2.AppendLine($"------------{pageNumber}------------");

                for (int index = 0; index < voterList.Count; index++)
                {

                    voterList[index].PageNo = pageNumber;
                    voterList[index].RowNo = (index / 3) + 1;
                    voterList[index].SNo = index + 1;
                    if (voterList[index].RowNo.ToString() == txtRow.Text.Trim())
                    {

                    }
                    // Process record by record 
                    DoHFname(fatherOrHusband[index], pageNumber, voterList[index], index);
                    DoAddress(address[index], pageNumber, voterList[index], index);
                    DoAge(age[index], pageNumber, voterList[index], index);
                    DoGender(sex[index], pageNumber, voterList[index], index);

                }


                // 1.1 add pageNo and index
                for (int index = 0; index < voterList.Count; index++)
                {
                    voterList[index].PageNo = pageNumber;
                    voterList[index].RowNo = (index / 3) + 1;
                    voterList[index].SNo = index + 1;
                }

                // 2. HorFName
                for (int index = 0; index < fatherOrHusband.Count; index++)
                {
                    //DoHFname(fatherOrHusband[index], pageNumber, voterList[index], index);

                    //if (voterList[index].MayError)
                    //    voterList.Where(w => w.RowNo == voterList[index].RowNo).ToList().ForEach(fe => fe.MayError = true);
                }


                // 3. HomeAddress
                for (int index = 0; index < address.Count; index++)
                {
                    DoAddress(address[index], pageNumber, voterList[index], index);
                }

                // 4. Age
                for (int index = 0; index < age.Count; index++)
                {
                    DoAge(age[index], pageNumber, voterList[index], index);
                }

                // 5. Sex
                for (int index = 0; index < sex.Count; index++)
                {
                    DoGender(sex[index], pageNumber, voterList[index], index);
                }

                //Append to final list


                var groupByRow = (from v in voterList
                                  group v by v.RowNo into ng
                                  select ng).ToList();


                //groupByRow.ForEach(fe =>
                //{
                //    var threeRec = fe.ToList();
                //    //var E = false;
                //    if (threeRec[0].HorFName.Contains('-') == false && threeRec[1].HorFName.Contains('-') == true && threeRec[2].HorFName.Contains('-') == true)
                //    {
                //        if (threeRec[0].HorFName.Count(c => c == '*') <= 1)
                //        {

                //            if ((threeRec[0].HorFName.Count(c => c == '*') == threeRec[0].Name.Count(c => c == '*')) == false)
                //            {

                //                if ((threeRec[0].HorFName.Contains("-") == false && threeRec[0].Name.Contains("-") == false) == false)
                //                {
                //                    //if(threeRec[0].HorFName)
                //                    if (threeRec[0].HorFName.Trim() == string.Empty)
                //                    {
                //                        threeRec[0].ErrorType = ErrorType.BREAK1EMPTY;
                //                    }
                //                    else
                //                    {
                //                        threeRec[0].ErrorType = ErrorType.BREAK1;
                //                    }

                //                }
                //            }
                //        }
                //    }




                //    //if(threeRec.Count == 3)
                //    //{
                //    //    if (!threeRec[0].HorFName.Contains('-') 
                //    //    && threeRec[1].HorFName.Contains('-') 
                //    //    && threeRec[2].HorFName.Contains('-') 
                //    //    && threeRec[0].HorFName.Count(c => c == '*') == threeRec[1].HorFName.Count(c => c == '*'))
                //    //    {
                //    //        threeRec[0].Err = true;
                //    //    }

                //    //}
                //    //for (int ff = 0; ff < threeRec.Count; ff++)
                //    //{
                //    //    threeRec[ff].Index = ff + 1;

                //    //    var nec = threeRec[ff].HorFName.Count(c => c == '*');
                //    //    threeRec[ff].NameErrorCount = nec;
                //    //    if (nec > 1)
                //    //        threeRec[ff].NameError = true;

                //    //    var gec = threeRec[ff].Sex.Split(' ').Where(w => w == "*").Count();
                //    //    threeRec[ff].GenderErrorCount = gec;
                //    //    if (ff != 2 && threeRec.Count > 2 && gec > 1)
                //    //        threeRec[ff].GenderError = true;
                //    //}

                //});

                fullList.AddRange(voterList);

                if (lastPageNumberToProcess == pageNumber)
                {
                    bd.NewVoters = voterList.Count;
                    BoothDetail.UpdateNewVoters(bd, boothDetailPath);
                }

                //if (errorRowNumber > 0)
                //{
                //    var jsonFile = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}-ReRun.json");
                //    var fd = JsonConvert.SerializeObject(voterList, Formatting.Indented);
                //    General.WriteToFile(jsonFile, fd);
                //    logs.Clear();
                //}


                //if (voterList.Any(a => a.MayError))
                //{
                //    if (Directory.Exists(reProcessFile) == false)
                //        Directory.CreateDirectory(reProcessFile);

                //    var file = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}.txt");
                //    General.WriteToFile(file, pageContent);

                //    //write as json
                //    var jsonFile = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}-Error.json");
                //    var fd = JsonConvert.SerializeObject(voterList, Formatting.Indented);
                //    General.WriteToFile(jsonFile, fd);
                //}

                return true;//voterList.Any(a => a.MayError);

            }
            catch (Exception ex)
            {

                return false;
            }


        }

        StringBuilder sb2 = new StringBuilder();

        private void DoHFname(string fName, int pn, VoterList vl, int ind)
        {
            var nm = GetFNname(fName);
            vl.HorFName = nm.Item1 ? nm.Item2 : AddLog(pn, vl);
            //sb2.AppendLine($"{fName.Replace("\r\n", "@") [fName.Replace("\r\n", "@").Count(c => c == '@')]}  -->  {nm.Item2.Replace("\r\n", "$") [nm.Item2.Replace("\r\n", "@").Count(c => c == '@')]}");
            //sb2.Append($"{pn}~{vl.RowNo}~{vl.Name.Trim()}~");
            //sb2.Append(fName.Replace("\r\n", "@") + "===>");
            //sb2.Append($"[{fName.Replace("\r\n", "@").Count(c => c == '@')}]");
            //sb2.Append(nm.Item2.Replace("\r\n", "$") + "--->");
            //sb2.Append($"[{nm.Item2.Replace("\r\n", "$").Count(c => c == '$')}]");
            //sb2.Append(Environment.NewLine);

            if (vl.MayError == false) vl.MayError = nm.Item3;
            if (vl.MayError)
                AddNameLog(pn, $"FATHERNAME ERROR @ {ind + 1}");
        }

        private void DoAddress(string add, int pn, VoterList vl, int ind)
        {
            var nm = GetAddress(add);
            vl.HomeAddress = nm.Item1 ? nm.Item2 : AddLog(pn, vl);
            if (vl.MayError == false) vl.MayError = nm.Item3;
            if (vl.MayError)
                AddNameLog(pn, $"HOMEADDRESS ERROR @ {ind + 1}");
            vl.IsDeleted = nm.Item4;
            if (nm.Item4)
                AddDeleteItemLog(pn, vl);
        }

        private void DoAge(string age, int pn, VoterList vl, int ind)
        {
            var nm = GetAge(age);
            if (vl.MayError == false) vl.MayError = nm.Item3;
            if (vl.MayError)
                AddNameLog(pn, $"AGE ERROR @ {ind + 1}");
            var a = 0;
            try
            {
                a = nm.Item2.ToInt32();
            }
            catch (Exception)
            {

            }
            if (a == 0)
            {
                vl.IsDeleted = true;
                AddDeleteItemLog(pn, vl);
            }
            vl.Age = nm.Item1 ? a : -999;
        }

        private void DoGender(string gender, int pn, VoterList vl, int ind)
        {
            var nm = GetGender(gender);
            vl.Sex = nm.Item1 ? nm.Item2 : AddLog(pn, vl);

            if (vl.MayError)
                AddNameLog(pn, $"SEX ERROR @ {ind + 1}");

            if (vl.MayError == false) vl.MayError = nm.Item3;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            var fData = fullList.Where(w => w.PageNo == txtFIlterPn.Text.ToInt32());

            if (txtFIlterRn.Text.Trim() != "")
            {
                fData = fData.Where(w => w.RowNo == txtFIlterRn.Text.ToInt32());
            }

            dataGridView1.DataSource = fData.ToList();

        }

        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {

                   new KeyValuePair<int, string>(1, "Any Null?"),
                   new KeyValuePair<int, string>(2, "Name Null?"),
                   new KeyValuePair<int, string>(3, "FNAME Null?"),
                   new KeyValuePair<int, string>(4, "Address Null?"),
                   new KeyValuePair<int, string>(5, "Age Null?"),
                   new KeyValuePair<int, string>(6, "Gender Null?"),
                   new KeyValuePair<int, string>(7, "Delete Page"),
                   new KeyValuePair<int, string>(8, "> 1 Deleted Page"),
                   new KeyValuePair<int, string>(9, "May Error Page"),
                   new KeyValuePair<int, string>(10, "Name Issue"),
                   new KeyValuePair<int, string>(11, "FNAME Issue"),
                   new KeyValuePair<int, string>(12, "Address Issue"),
                   new KeyValuePair<int, string>(13, "Age Issue"),
                   new KeyValuePair<int, string>(14, "Gender Issue"),
                   new KeyValuePair<int, string>(15, "Manual Edit"),
               };

            return myKeyValuePair;

        }

        List<VoterList> filteredData = new List<VoterList>();

        private void cmbFIlter_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (fullList.Count == 0) return;

            var value = ((KeyValuePair<int, string>)cmbFIlter.SelectedItem).Key;

            if (value == 1)
            {
                filteredData = fullList.Where(w =>
                string.IsNullOrEmpty(w.Name.Trim()) ||
                string.IsNullOrEmpty(w.HorFName.Trim()) ||
                string.IsNullOrEmpty(w.HomeAddress.Trim()) ||
                w.Age == 0 ||
                string.IsNullOrEmpty(w.Sex.Trim())
                ).ToList();
            }
            if (value == 2)
            {
                filteredData = fullList.Where(w =>
                string.IsNullOrEmpty(w.Name.Trim())
                ).ToList();
            }
            if (value == 3)
            {
                filteredData = fullList.Where(w =>
                string.IsNullOrEmpty(w.HorFName.Trim())
                ).ToList();
            }
            if (value == 4)
            {
                filteredData = fullList.Where(w =>
                string.IsNullOrEmpty(w.HomeAddress.Trim())
                ).ToList();
            }
            if (value == 5)
            {
                filteredData = fullList.Where(w =>
                w.Age == 0
                ).ToList();
            }
            if (value == 6)
            {
                var da =
                filteredData = fullList.Where(w =>
                string.IsNullOrEmpty(w.Sex.Trim())
                ).ToList();
            }
            else if (value == 7)
            {
                filteredData = fullList.Where(w =>
                w.IsDeleted
                ).ToList();
            }
            else if (value == 8)
            {
                var da = (from f in fullList
                          where f.IsDeleted
                          group f by f.PageNo into ng
                          select ng).ToList();

                da.ForEach(fe =>
                {

                    if (fe.ToList().Count > 1)
                    {
                        filteredData.AddRange(fe.ToList());
                    }

                });

            }
            else if (value == 9)
            {
                filteredData = fullList.Where(w =>
                w.MayError
                ).ToList();
            }
            else if (value == 10)
            {
                filteredData = fullList.Where(w =>
                w.Name.Trim().Split(' ').Count() > 2
                ).ToList();
            }
            else if (value == 11)
            {
                filteredData = fullList.Where(w =>
                w.HorFName.Trim().Split(' ').Count() > 2
                ).ToList();
            }
            else if (value == 12)
            {
                filteredData = fullList.Where(w =>
                w.HomeAddress.Trim().Split(' ').Count() > 1
                ).ToList();
            }
            else if (value == 13)
            {
                filteredData = fullList.Where(w =>
                w.Age.ToString().Trim().Split(' ').Count() > 1
                ).ToList();
            }
            else if (value == 14)
            {
                filteredData = fullList.Where(w =>
                w.Sex.Trim().Split(' ').Count() > 1
                ).ToList();
            }
            else if (value == 15)
            {
                filteredData = fullList.Where(w =>
                w.IsManualEdit
                ).ToList();
            }

            dataGridView1.DataSource = filteredData;

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (sender as DataGridView);
            int rowIndex = grid.CurrentCell.RowIndex;
            string owningColumnName = grid.CurrentCell.OwningColumn.Name;
            string cellValue = FormGeneral.GetGridCellValue(grid, rowIndex, owningColumnName);
            VoterList cus = grid.Rows[grid.CurrentCell.RowIndex].DataBoundItem as VoterList;
            //Update data

            VoterList.UpdateVoterDetails(cus, voterFilePath);

            SetErrorDetail();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            cmbFIlter_SelectedIndexChanged(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var sbDbError = new StringBuilder();

            for (int i = 10; i <= 336; i++)
            {
                var fileName = $"ac211{i:D3}.pdf";

                try
                {

                    WebClient webClient = new WebClient();
                    webClient.DownloadFile($"https://www.elections.tn.gov.in/SSR2020_14022020/dt27/ac211/{fileName}", $@"F:\NTK\VotersAnalysis\VoterList\{fileName}");
                }
                catch (Exception)
                {
                    sbDbError.AppendLine(fileName);

                }

            }

        }


        private void LoadBooths()
        {
            var allAssFiles = (from f in Directory.GetDirectories(AppConfiguration.AssemblyVotersFolder).ToList()
                               select new KeyValuePair<string, string>(new DirectoryInfo(f).Name, f)).ToList();


            allAssFiles.Insert(0, new KeyValuePair<string, string>("0", "--select--"));
            cmbAss.DataSource = allAssFiles;
            cmbAss.ValueMember = "Value";
            cmbAss.DisplayMember = "Key";
        }


        private void cmbAss_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAss.SelectedIndex == 0) return;

            var values = cmbAss.SelectedItem;

            var fol = (KeyValuePair<string, string>)values;

            var allBoothFiles = (from f in Directory.GetFiles(fol.Value).ToList()
                                 select new KeyValuePair<string, string>(new FileInfo(f).Name, f)).ToList();

            allBoothFiles.Insert(0, new KeyValuePair<string, string>("0", "--select--"));
            allBoothFiles.Insert(1, new KeyValuePair<string, string>("ALL", fol.Value));

            cmbBooths.DataSource = allBoothFiles;
            cmbBooths.ValueMember = "Value";
            cmbBooths.DisplayMember = "Key";

        }

        private void cmbBooths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBooths.SelectedIndex == 0) return;

            var keyValue = (KeyValuePair<string, string>)cmbBooths.SelectedItem;
            //
            List<VoterList> da = new List<VoterList>();

            if (keyValue.Key.Trim() == "ALL")
            {

                var allAssFiles = (from f in Directory.GetFiles(keyValue.Value).ToList()
                                   select new KeyValuePair<string, string>(new DirectoryInfo(f).Name, f))
                                   .Where(w => w.Key.Contains("BoothDetail") == false)
                                   .ToList();

                allAssFiles.ForEach(fe =>
                {
                    da.AddRange(VoterList.GetAll(fe.Value));
                });
            }
            else
            {

                da = VoterList.GetAll(keyValue.Value);
            }


            var dataSou = new List<KeyValuePair<string, string>>();

            var maleCount = da.Where(w => w.Sex.Trim().Split(' ')[0].Trim() == "ஆண்").Count();
            var femaleCount = da.Where(w => w.Sex.Trim().Split(' ')[0].Trim() == "பெண்").Count();

            var allAges = da.Select(s => s.Age).ToList();
            var allC = da.Select(s => s.Age).ToList().Count();

            var twenty = GetLessAgeCOunt(allAges, 20);
            var thirty = GetAgeCOunt(allAges, 21, 30);
            var forty = GetAgeCOunt(allAges, 31, 40);
            var fifty = GetAgeCOunt(allAges, 41, 50);
            var sixty = GetAgeCOunt(allAges, 51, 60);
            var aboveSixty = GetMoreAgeCOunt(allAges, 61);

            lblDetails.Text = $"Total: {da.Count}{Environment.NewLine}ஆண்: {maleCount}{Environment.NewLine}பெண்: {femaleCount}{Environment.NewLine}";

            lblDetails.Text += $"18-20: {twenty}({Perc(twenty, allC)}){Environment.NewLine}21-30: {thirty}({Perc(thirty, allC)}){Environment.NewLine}" +
                $"31-40: {forty}({Perc(forty, allC)}){Environment.NewLine}41-50: {fifty}({Perc(fifty, allC)}){Environment.NewLine}" +
                $"51-60: {sixty}({Perc(sixty, allC)}){Environment.NewLine}Above 60: {aboveSixty}({Perc(aboveSixty, allC)}){Environment.NewLine}";

            dataSou.Add(new KeyValuePair<string, string>("18-20", Perc(twenty, allC)));
            dataSou.Add(new KeyValuePair<string, string>("21-30", Perc(thirty, allC)));
            dataSou.Add(new KeyValuePair<string, string>("31-40", Perc(forty, allC)));
            dataSou.Add(new KeyValuePair<string, string>("41-50", Perc(fifty, allC)));
            dataSou.Add(new KeyValuePair<string, string>("51-60", Perc(sixty, allC)));
            dataSou.Add(new KeyValuePair<string, string>("Above60", Perc(aboveSixty, allC)));

            var t1 = dataSou.OrderByDescending(o =>
            Convert.ToDecimal(o.Value.Split(' ')[1].Replace("(", "").Replace(")", "").Replace("%", ""))
            ).ToList();

            t1.Insert(0, new KeyValuePair<string, string>("பெண்", Perc(femaleCount, allC)));
            t1.Insert(0, new KeyValuePair<string, string>("ஆண்", Perc(maleCount, allC)));
            t1.Insert(0, new KeyValuePair<string, string>("Total", da.Count.ToString()));

            dataGridView1.DataSource = t1;



        }

        private int GetAgeCOunt(List<int> ages, int st, int en)
        {
            return ages.Count(c => c >= st && c <= en);
        }

        private int GetLessAgeCOunt(List<int> ages, int age)
        {
            return ages.Count(c => c <= age);
        }

        private int GetMoreAgeCOunt(List<int> ages, int age)
        {
            return ages.Count(c => c >= age);
        }

        private string Perc(int den, int nom)
        {
            return den + " (" + (Math.Round((Convert.ToDecimal(den) / Convert.ToDecimal(nom)) * 100, 1)).ToString() + "%" + ")";
        }

        private decimal PercInDec(int den, int nom)
        {
            return Math.Round((Convert.ToDecimal(den) / Convert.ToDecimal(nom)) * 100, 1);
        }
    }

    


}
