using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucVoterAnalysis : UserControl
    {

        List<VoterList> fullList = new List<VoterList>();
        //string folderPath = @"F:\NTK\VotersAnalysis\";
        string txtPath = ""; //@"F:\NTK\VotersAnalysis\VoterList\doc";


        string reProcessFile = "";
        BoothDetail bd;
        bool haveErrorinFile = false;
        int lastPageNumberToProcess;
        List<string> logs;
        string voterFilePath = "";
        string boothDetailPath = "";
        string errorFolder = "";
        string DoneFolder = "";
        string logErrorPath = "";
        string voterIdPath = "";
        //string reportPath = "";


        public ucVoterAnalysis()
        {
            InitializeComponent();
            LoadBooths();
        }



        public static byte[] ReadFile(string fp)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(fp, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length    
                buffer = new byte[length];            // create buffer     
                int count;                            // actual number of bytes read     
                int sum = 0;                          // total number of bytes read    

                // read until Read method returns 0 (end of the stream has been reached)    
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }


        private void button1_Click(object sender, EventArgs e)
        {

            reProcessFile = "";

            //FolderBrowserDialog fbd = new FolderBrowserDialog();

            ////string folderPath = "";
            //if (DialogResult.OK == fbd.ShowDialog())
            //{
            //    txtPath = fbd.SelectedPath;
            //}


            // txtPath = @"F:\NTK\VotersAnalysis\VoterList\211\docx";

            txtPath = @"F:\NTK\VotersAnalysis\VoterList\210\docx";



            //int year = 2019;
            //var filePath = $@"{folderPath}ac210333.txt";

            var allFiles = Directory.GetFiles(txtPath).ToList();

            int year = 2020;

            errorFolder = Path.Combine(Directory.GetParent(txtPath).FullName, "ErrorFile");
            DoneFolder = Path.Combine(Directory.GetParent(txtPath).FullName, "Done");


            logErrorPath = Path.Combine(Directory.GetParent(txtPath).FullName, $"Log -{DateTime.Now.ToLongTimeString().Replace(":", "-")}");  //$@"F:\NTK\VotersAnalysis\VoterList\Log-{DateTime.Now.ToLongTimeString().Replace(":", "-")}";

            General.CreateFolderIfNotExist(errorFolder);
            General.CreateFolderIfNotExist(DoneFolder);


            foreach (var filePath in allFiles)
            {
                string partNo = "";
                string assNo = "";

                var fileInfo = new FileInfo(filePath);

                try
                {
                    var ufn = fileInfo.Name.Split('.')[0];

                    assNo = ufn.Substring(2, 3);
                    partNo = ufn.Substring(5, 3);

                    var basePath = Path.Combine(AppConfiguration.AssemblyVotersFolder, $"{assNo}");

                    boothDetailPath = Path.Combine(basePath, $"{assNo}-BoothDetail.json");
                    voterFilePath = Path.Combine(basePath, $"{assNo}-{partNo}.json");
                    voterIdPath = Path.Combine(Directory.GetParent(txtPath).FullName, "VoterIds", $"{assNo}-{partNo}");
                    General.CreateFolderIfNotExist(voterIdPath);
                }
                catch (Exception ex)
                {
                    General.WriteLog(logErrorPath, $"Error in FileName - {ex.ToString()}", assNo, partNo, 0);
                }

                //DocxToText dtt1 = new DocxToText(filePath);
                //var allPageContent1 = dtt1.ExtractText();

                //DocxToText dtt2 = new DocxToText(filePath);
                //var allPageContent2 = dtt2.ExtractText();

                DocxToText dtt = new DocxToText(filePath);
                var allPageContent = dtt.ExtractText();

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


                    bd.PartNo = fpSPlitted[3].Split(' ')[2].Replace("|", "").Replace("/", "").Trim();

                    if (string.IsNullOrEmpty(bd.PartNo)) bd.PartNo = partNo;

                    bd.ParlimentNo = "35"; // parliment[1];
                    bd.ParlimentName = "இராமநாதபுரம்"; // parliment[2].Trim().Split('1')[0].Trim();

                    //if (year == 2020)
                    //{
                    //    var parliment = fpSPlitted[4].Split('-');
                    //    bd.ParlimentNo = "35"; // parliment[1];
                    //    bd.ParlimentName = "இராமநாதபுரம்"; // parliment[2].Trim().Split('1')[0].Trim();
                    //}
                    //else
                    //{
                    //    var parliment2019 = fpSPlitted[4].Split(':')[1].Trim().Split('-');
                    //    bd.ParlimentNo = parliment2019[0].Trim();
                    //    bd.ParlimentName = parliment2019[1].Trim().Split('1')[0].Trim();
                    //}

                    //bd.EligibilityDay = fpSPlitted[6].Split(' ')[2].Trim();
                    //bd.ReleaseDate = fpSPlitted[7].Replace("பட்டியல் வெளியிடப்பட்ட நாள்", "$").Split('$')[1].Split(' ')[1];

                    //if (year == 2020)
                    //{
                    //    if (fpSPlitted[8].Split('-').Count() > 2)
                    //    {
                    //        bd.PartPlaceName = fpSPlitted[8].Split('-')[2].Replace("பிரிவின் எண் மற்றும் பெயர்", "$").Split('$')[1].Trim();
                    //    }
                    //    else
                    //    {
                    //        bd.PartPlaceName = fpSPlitted[8].Split('-')[0].Replace("பிரிவின் எண் மற்றும் பெயர்", "$").Split('$')[1].Replace("999. அயல்நாடு வாழ் வாக்காளர்கள்", "$").Split('$')[0];
                    //    }
                    //}
                    //else
                    //{
                    //    bd.PartPlaceName = fpSPlitted[8].Split('-')[0].Replace("பிரிவின் எண் மற்றும் பெயர்", "$").Split('$')[1].Trim();
                    //}


                    //var colonCount = fpSPlitted[8].Count(c => c == ':');


                    //try
                    //{
                    //    var otherDetails = fpSPlitted[8].Split('-')[5].Split(' ');
                    //    if (year == 2020)
                    //    {
                    //        bd.MainCityOrVillage = otherDetails[2];
                    //        bd.Zone = otherDetails[3].Trim();
                    //        bd.Birga = otherDetails[5];
                    //        bd.PoliceStation = otherDetails[6];
                    //        bd.Taluk = otherDetails[7];
                    //        bd.District = fpSPlitted[8].Split('-')[6].Trim();
                    //        bd.Pincode = fpSPlitted[8].Split('-')[12].Split(' ')[2].ToInt32();
                    //    }
                    //    else
                    //    {
                    //        bd.MainCityOrVillage = fpSPlitted[8].Split('-')[2].Split(' ')[10].Trim();
                    //        bd.Zone = fpSPlitted[8].Split('-')[3].Trim();
                    //        bd.Birga = fpSPlitted[8].Split('-')[4].Trim();
                    //        bd.PoliceStation = fpSPlitted[8].Split('-')[5].Split(' ')[1];
                    //        bd.Taluk = fpSPlitted[8].Split('-')[5].Split(' ')[2].Trim();
                    //        bd.District = fpSPlitted[8].Split('-')[5].Split(' ')[3].Trim();
                    //        bd.Pincode = fpSPlitted[8].Split('-')[5].Split(' ')[4].Trim().ToInt32();
                    //    }

                    //}
                    //catch (Exception ex)
                    //{
                    //    General.WriteLog(logErrorPath, $"Error in First Page otherDetails Details", assNo, partNo, 1);
                    //}


                
                    bd.BoothType = fpSPlitted[9].Replace("வாக்குச் சாவடியின் விவரங்கள்", "").Trim();

                    //bd.PartLocationAddress = fpSPlitted[11].Replace("எண்ணிக்கை", "$").Split('$')[1].Split('4')[0].Trim();

                    //try
                    //{
                    //    bd.StartNo = fpSPlitted[12].Replace("தொடங்கும் வரிசை எண்", "").Trim().ToInt32();
                    //}
                    //catch (Exception)
                    //{
                    //    bd.StartNo = 1;
                    //    General.WriteLog(logErrorPath, $"Error in First Page StartNo", assNo, partNo, 1);
                    //}


                    //var voteDetails = fpSPlitted[13];

                    //List<string> toReplaceVoteDetail = new List<string>()
                    //    {
                    //        "முடியும் வரிசை எண்",
                    //        "நிகர வாக்காளர்களின் எண்ணிக்கை",
                    //        "பெண் மூன்றாம் பாலினம்",
                    //        "மொத்தம்"
                    //    };

                    //toReplaceVoteDetail.ForEach(fe =>
                    //            {
                    //                voteDetails = voteDetails.Replace(fe, $"$");

                    //            });

                    //var splitVoter = voteDetails.Split('$');
                    //var forNo = splitVoter[1].Split(' ');

                    //bd.EndNo = forNo[1].Trim().ToInt32();

                    //if (year == 2020)
                    //    try
                    //    {
                    //        bd.Male = forNo[4].ToInt32();
                    //    }
                    //    catch (Exception)
                    //    {
                    //        bd.Male = forNo[3].ToInt32();
                    //        General.WriteLog(logErrorPath, $"Error in First Page-Male Detail", assNo, partNo, 1);
                    //    }

                    //else
                    //    bd.Male = forNo[3].ToInt32();

                    //if (year == 2020)
                    //{
                    //    var genderVotes = splitVoter[3].Split(' ');
                    //    bd.TotalVoters = splitVoter[4].Split(' ')[1].Trim().ToInt32();
                    //    bd.Female = genderVotes[1].Trim().ToInt32();
                    //    bd.ThirdGender = genderVotes[2].Trim().ToInt32();
                    //}
                    //else
                    //{
                    //    var genderVotes = splitVoter[4].Split(' ');
                    //    bd.TotalVoters = splitVoter[1].Split(' ')[1].Trim().ToInt32();
                    //    bd.Female = genderVotes[1].Trim().ToInt32();
                    //    bd.ThirdGender = genderVotes[2].Trim().ToInt32();
                    //}

                    /*************************************************************/

                    General.CreateFileIfNotExist(boothDetailPath);
                    BoothDetail.Save(bd, boothDetailPath);

                    /*************************************************************/

                }
                catch (Exception ex)
                {
                    General.WriteLog(logErrorPath, $"Error in First Page", assNo, partNo, 1);
                }

                //var lastPage = allPageContent.Substring(allPageContent.IndexOf("வாக்காளர்களின் தொகுப்பு"));

                var onlyVotersPages = allPageContent.Substring(allPageContent.IndexOf("பக்கம் 2"), allPageContent.IndexOf("வாக்காளர்களின் தொகுப்பு") - allPageContent.IndexOf("பக்கம் 2"));

                var totalPages = firstPage.Substring(firstPage.IndexOf("மொத்த பக்கங்கள்"), firstPage.LastIndexOf("பக்கம்") - firstPage.IndexOf("மொத்த பக்கங்கள்"));

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

                    if (pageNumber == 7)
                    {
                        var tt = "";
                    }
                    var pageContent = onlyVotersPages.Substring(startIndex, lastIndex);

                    if (ProcessPage(pageNumber, pageContent, assNo, partNo) == false)
                    {
                        General.WriteLog(logErrorPath, $"Error in PageProcess", assNo, partNo, pageNumber);
                        isProcessed = false;
                        //break;
                    }

                    //isProcessed = true;
                }

                var newFileName = Path.Combine(DoneFolder, fileInfo.Name);

                if (isProcessed == false)
                {
                    General.CreateFileIfNotExist(newFileName);
                    continue;
                }


                string flag = "OK";

                var file4 = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{DateTime.Now.ToLocalTime().ToString().Replace(":", "~")}-{flag}.txt");
                General.WriteToFile(file4, onlyVotersPages);

                logs.Clear();

                var acData = fullList.Where(w => w.Sex == null);

                var maleCount = fullList.Where(w => w.Sex.Trim().Split(' ')[0].Trim() == "ஆண்").Count();
                var femaleCount = fullList.Where(w => w.Sex.Trim().Split(' ')[0].Trim() == "பெண்").Count();
                var thirdGenderCount = fullList.Where(w => w.Sex.Trim().Split(' ')[0].Trim() == "திருநங்கை").Count();

                var allAges = fullList.Select(s => s.Age).ToList();
                var totalVoters = allAges.Count();

                //var twenty = GetLessAgeCount(allAges, 20);
                //var thirty = GetAgeCount(allAges, 21, 30);
                //var forty = GetAgeCount(allAges, 31, 40);
                //var fifty = GetAgeCount(allAges, 41, 50);
                //var sixty = GetAgeCount(allAges, 51, 60);
                //var aboveSixty = GetMoreAgeCount(allAges, 61);

                var twenty = GetLessAgeCount(allAges, 35);
                var thirty = GetAgeCount(allAges, 36, 45);
                var forty = GetAgeCount(allAges, 46, 55);
                var fifty = GetAgeCount(allAges, 56, 65);
                var sixty = GetMoreAgeCount(allAges, 66); ; // GetAgeCount(allAges, 51, 60);
                var aboveSixty = 0; // GetMoreAgeCount(allAges, 66);

                var newBoothPerc = new VotePercDetail()
                {
                    AssemblyNo = assNo.ToInt32(),
                    BoothNo = partNo.ToInt32(),
                    Total = totalVoters,
                    Male = maleCount,
                    Female = femaleCount,
                    Third = thirdGenderCount,
                    to20 = twenty,
                    to30 = thirty,
                    to40 = forty,
                    to50 = fifty,
                    to60 = sixty,
                    Above60 = aboveSixty,
                    MaleP = PercInDec(maleCount, totalVoters),
                    FemaleP = PercInDec(femaleCount, totalVoters),
                    ThirdP = PercInDec(thirdGenderCount, totalVoters),
                    to20P = PercInDec(twenty, totalVoters),
                    to30P = PercInDec(thirty, totalVoters),
                    to40P = PercInDec(forty, totalVoters),
                    to50P = PercInDec(fifty, totalVoters),
                    to60P = PercInDec(sixty, totalVoters),
                    Above60P = PercInDec(aboveSixty, totalVoters)
                };

                VotePercDetail.Save(newBoothPerc);

                try
                {
                    //File.Move(filePath, newFileName);
                    File.Copy(filePath, newFileName);
                }
                catch (Exception ex)
                {
                }
                fullList.Clear();
            }

            MessageBox.Show("ALL DONE!");

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



        public (bool, string, bool) GetName(string content, string assNo, string partNo)
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
                General.WriteLog(logErrorPath, $"Error in GETNAME", assNo, partNo, 1);
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

                var isEmpty = false;

                if (d.Trim().ToLower() == "-1")
                {
                }
                else
                {
                    isEmpty = string.IsNullOrEmpty(d);
                }
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
            var searchStr = $"பக்கம் {pageNo}";
            var pakkam = "பக்கம்";

            var index = allContent.IndexOf(searchStr);

            if (index < 0)
            {
                return allContent.IndexOf(pakkam, allContent.IndexOf($"பக்கம் {pageNo - 1}") + 4);
            }
            else
            {
                return index;
            }
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

                //if (pageNumber == 47)
                //{
                //    var s = "";
                //}

                data = pageContent;

                if (data.IndexOf("பெயர்") < data.IndexOf("சட்டமன்றத் தொகுதி எண் மற்றும் பெயர்"))
                    data = data.Remove(data.IndexOf("பெயர்"), 3);

                data = data.Replace("சட்டமன்றத் தொகுதி எண் மற்றும் பெயர்", "*");
                data = data.Replace("பிரிவு எண் மற்றும் பெயர்", "*");

                bool mayHaveError = false;

                int recordCount = 0;


                try
                {

                    recordCount = (from p in data.Split(' ').ToList()
                                   where p.Contains("Photo")
                                   select p).ToList().Count;

                    var recordCount2 = (from p in data.Split(' ').ToList()
                                        where p.Contains("ilable")
                                        select p).ToList().Count;

                    recordCount = Math.Max(recordCount, recordCount2);

                    recordCount = recordCount > 30 ? 30 : recordCount;

                    pageContent = pageContent.Replace("W RM", "WRM");
                    var keyList = new List<string>() { "WRM", "JRR", "XOE", "FXJ", "STG" };

                    if (pageNumber == 22)
                    {
                        //var allLinesTest = (from f in pageContent.Split(' ').ToList()
                        //                    where keyList.Any(a => f.Trim().StartsWith(a))
                        //                    select f.Trim().Substring(0, 10)).ToList();

                        var one = pageContent.Split(' ').ToList();

                        foreach (var item in one)
                        {
                            if (keyList.Any(a => item.Trim().StartsWith(a)))
                            {
                                if (item.Length < 10)
                                {
                                    var it = item.Trim().Substring(0, 10);
                                }
                            }
                        }
                    }


                    var allLines = (from f in pageContent.Split(' ').ToList()
                                    where keyList.Any(a => f.Trim().StartsWith(a))
                                    select f.Trim().Substring(0, 10)).ToList();

                    var fillFileName = "";

                    if (allLines.Count == recordCount)
                        fillFileName = Path.Combine(voterIdPath, pageNumber + ".txt");
                    else
                        fillFileName = Path.Combine(voterIdPath, pageNumber + "_ERROR.txt");


                    var r = new StringBuilder();

                    // Save voter Ids.
                    allLines.ForEach(fe =>
                    {
                        r.AppendLine(fe);
                    });

                    General.ReplaceLog(fillFileName, r.ToString());
                }
                catch (Exception ex)
                {
                    //fillFileName = Path.Combine(voterIdPath, pageNumber + "_ERROR.txt");
                    General.ReplaceLog(Path.Combine(voterIdPath, pageNumber + "_FULL-ERROR.txt"), "FULL-ERROR");
                }


                data = data.Replace("Photo", "").Replace("is", "").Replace("Available", "");  // Rempve Photo is AVailable.

                data = data.Replace("தந்தை பெயர்", "$FATHER")
                           .Replace("கணவர் பெயர்", "$FATHER")
                           //.Replace("கணவர் பெய", "$HUSBAND-1:") // scenerio 1
                           .Replace("தாய் பெயர்", "$FATHER")
                           .Replace("இதரர் பெயர்", "$FATHER")
                            .Replace("மனைவி பெயர்", "$FATHER");

                bool needCorrection = false;

                if (data.Contains("தந்தை") ||
                    data.Contains("கணவர்") ||
                    data.Contains("தாய்") ||
                    data.Contains("இதரர்") ||
                    data.Contains("மனைவி"))
                {
                    needCorrection = true;
                }

                // NOTE: dont change this order of Replace
                data = data.Replace("பெயர்", "$NAME")  // Rempoe Photo is AVailable.

                           .Replace("வீட்டு எண்", "$ADDRESS")
                           .Replace("வீட்டு என்", "$ADDRESS-D")
                            .Replace("வீட்டு தன்", "$ADDRESS-D")
                            .Replace("வீட்டு பண்", "$ADDRESS-D")
                            .Replace("வீட்டு கண்", "$ADDRESS-D")
                            .Replace("வீட்டு தடை", "$ADDRESS-D")
                           //.Replace("வீட்டு", "$ADDRESS:D")
                           .Replace("வயது", "$AGE")
                           .Replace("மூன்றாம் பாலினம்", "திருநங்கை")
                           .Replace("மூன்றாம்", "திருநங்கை")
                           .Replace("பாலினம்", "$SEX");

                var splittedData = data.Split('$').ToList();

                splittedData.RemoveRange(0, 1);

                // no of voters in a page
                var pageRecordCount = splittedData.Count;

                var names = splittedData.Where(w => w.StartsWith("NAME")).ToList();

                var fatherOrHusband = splittedData.Where(
                                           w => w.StartsWith("FATHER") ||
                                           w.StartsWith("MOTHER") || w.StartsWith("HUSBAND") ||
                                           w.StartsWith("OTHERS")).ToList();

                if (fatherOrHusband.Count > recordCount)
                {
                    for (int i = 0; i < fatherOrHusband.Count - 1; i++)
                    {
                        if (fatherOrHusband[i].Replace("FATHER", "").Replace(":", "").Replace(".", "").Replace("-", "").Trim() == string.Empty)
                            fatherOrHusband.Remove(fatherOrHusband[i]);
                    }
                }

                var address = splittedData.Where(w => w.StartsWith("ADDRESS")).ToList();

                var age = splittedData.Where(w => w.StartsWith("AGE")).ToList();
                age.RemoveAt(age.Count - 1);

                if (age.Count > recordCount)
                {
                    for (int i = 0; i < age.Count - 1; i++)
                    {
                        try
                        {
                            Convert.ToInt32(GetAge(age[i]).Item2);
                        }
                        catch (Exception)
                        {
                            age.Remove(age[i]);
                        }
                    }
                }


                if (address.Count > recordCount)
                {
                    address.RemoveRange(0, address.Count - age.Count);
                }


                if (names.Count > recordCount)
                {
                    for (int i = 0; i < names.Count - 1; i++)
                    {
                        if (names[i].Replace("NAME", "").Replace(":", "").Trim() == string.Empty)
                            names.Remove(names[i]);
                    }
                }

                if (needCorrection && names.Count != age.Count && names.Count - age.Count >= 0 && names.Count > recordCount)
                {
                    names.RemoveRange(0, names.Count - age.Count);
                    General.WriteLog(logErrorPath, "Error in Name, may be look at the supporter's name (father name)", assNum, partNum, pageNumber);
                }

                var sex = splittedData.Where(w => w.StartsWith("SEX")).ToList();

                if (sex.Count != recordCount)
                {
                    var sexNames = new List<string>() { "திருநங்கை", "ஆண்", "பெண்" };

                    for (int i = 0; i < sex.Count - 1; i++)
                    {
                        if (sex[i].Contains(':') && sexNames.Contains(sex[i].Split(':')[1].Trim().Split(' ')[0]))
                        { }
                        else if (sex[i].Contains(':') == false && sexNames.Contains(sex[i].Split(' ')[1].Trim().Split(' ')[0]))
                        { }
                        else
                            sex.Remove(sex[i]);
                    }


                }

                var isReProcess = (errorRowNumber > 0);
                var insertIndex = (errorRowNumber - 1);

                var isNameIssue = (names.Count != recordCount);
                var isFNameIssue = (fatherOrHusband.Count != recordCount);
                var isAddressIssue = (address.Count != recordCount);
                var isAgeIssue = (age.Count != recordCount);
                var isSexIssue = (sex.Count != recordCount);

                if (isNameIssue)
                {
                    var missedNames = new List<string>();
                    var missedCount = (recordCount - names.Count);

                    for (int i = 0; i < missedCount; i++)
                    {
                        missedNames.Add("Name:MISSED");
                    }
                    names.InsertRange(names.Count, missedNames);
                    General.WriteLog(logErrorPath, "Error in Names", assNum, partNum, pageNumber);
                }
                if (isFNameIssue)
                {
                    var missedNames = new List<string>();
                    var missedCount = (recordCount - fatherOrHusband.Count);

                    for (int i = 0; i < missedCount; i++)
                    {
                        missedNames.Add("FATHER:MISSED");
                    }
                    fatherOrHusband.InsertRange(fatherOrHusband.Count, missedNames);
                    General.WriteLog(logErrorPath, "Error in FatherNames", assNum, partNum, pageNumber);
                }
                if (isAddressIssue)
                {
                    var missedNames = new List<string>();
                    var missedCount = (recordCount - address.Count);

                    for (int i = 0; i < missedCount; i++)
                    {
                        address.Add("ADDRESS:MISSED");
                    }
                    address.InsertRange(address.Count, missedNames);
                    General.WriteLog(logErrorPath, "Error in Address", assNum, partNum, pageNumber);
                }
                if (isAgeIssue)
                {
                    var missedNames = new List<string>();
                    var missedCount = (recordCount - age.Count);

                    for (int i = 0; i < missedCount; i++)
                    {
                        age.Add("AGE:-1");
                    }
                    age.InsertRange(age.Count, missedNames);
                    General.WriteLog(logErrorPath, "Error in Age", assNum, partNum, pageNumber);
                }
                if (isSexIssue)
                {
                    var missedNames = new List<string>();
                    var missedCount = (recordCount - sex.Count);

                    for (int i = 0; i < missedCount; i++)
                    {
                        sex.Add("SEX:MISSED");
                    }
                    sex.InsertRange(sex.Count, missedNames);
                    General.WriteLog(logErrorPath, "Error in Gender", assNum, partNum, pageNumber);
                }


                if (isAgeIssue || isSexIssue)
                {
                    mayHaveError = true;


                    var jsonFile = Path.Combine(errorFolder, bd.PartNo, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}-Data.txt");
                    var jsonFileCon = Path.Combine(errorFolder, bd.PartNo, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}-Content.txt");

                    //var fd = JsonConvert.SerializeObject(voterList, Formatting.Indented);
                    General.CreateFolderIfNotExist(new FileInfo(jsonFile).DirectoryName);
                    General.WriteToFile(jsonFile, data);
                    General.WriteToFile(jsonFileCon, pageContent);
                    //return false;
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


                //names.ForEach(fe =>
                //{
                //    var nm = GetName(fe, assNum, partNum);
                //    var vl = new VoterList()
                //    {
                //        Name = nm.Item1 ? nm.Item2 : AddNameLog(pageNumber, "#NERROR#"),
                //        MayError = mayHaveError ? mayHaveError : (nm.Item3)

                //    };

                //    if (vl.MayError)
                //        AddNameLog(pageNumber, $"NAME ERROR @ {voterList.Count + 1}");

                //    voterList.Add(vl);
                //});

                for (int i = 0; i < recordCount; i++)
                {
                    var nm = GetName(names[i], assNum, partNum);
                    var vl = new VoterList()
                    {
                        Name = nm.Item1 ? nm.Item2 : AddNameLog(pageNumber, "#NERROR#"),
                        MayError = mayHaveError ? mayHaveError : (nm.Item3)
                    };

                    if (vl.MayError)
                        AddNameLog(pageNumber, $"NAME ERROR @ {voterList.Count + 1}");

                    voterList.Add(vl);
                }

                sb2.AppendLine($"------------{pageNumber}------------");

                //for (int index = 0; index < voterList.Count; index++)
                //{

                //    voterList[index].PageNo = pageNumber;
                //    voterList[index].RowNo = (index / 3) + 1;
                //    voterList[index].SNo = index + 1;
                //    if (voterList[index].RowNo.ToString() == txtRow.Text.Trim())
                //    {

                //    }
                //    // Process record by record 
                //    DoHFname(fatherOrHusband[index], pageNumber, voterList[index], index);
                //    DoAddress(address[index], pageNumber, voterList[index], index);
                //    DoAge(age[index], pageNumber, voterList[index], index);
                //    DoGender(sex[index], pageNumber, voterList[index], index);

                //}


                // 1. add pageNo and index
                for (int index = 0; index < voterList.Count; index++)
                {
                    voterList[index].PageNo = pageNumber;
                    voterList[index].RowNo = (index / 3) + 1;
                    voterList[index].SNo = index + 1;
                }

                // 2. HorFName
                for (int index = 0; index < recordCount; index++)
                {
                    DoHFname(fatherOrHusband[index], pageNumber, voterList[index], index);

                    //if (voterList[index].MayError)
                    //    voterList.Where(w => w.RowNo == voterList[index].RowNo).ToList().ForEach(fe => fe.MayError = true);
                }


                // 3. HomeAddress
                for (int index = 0; index < recordCount; index++)
                {
                    DoAddress(address[index], pageNumber, voterList[index], index);
                }

                // 4. Age
                for (int index = 0; index < recordCount; index++)
                {
                    DoAge(age[index], pageNumber, voterList[index], index);
                }

                // 5. Sex
                for (int index = 0; index < recordCount; index++)
                {
                    DoGender(sex[index], pageNumber, voterList[index], index);
                }

                var vlNonDel = voterList.Where(w => w.IsDeleted == false).ToList();

                fullList.AddRange(vlNonDel);

                if (lastPageNumberToProcess == pageNumber)
                {
                    bd.NewVoters = voterList.Count;
                    BoothDetail.UpdateNewVoters(bd, boothDetailPath);
                }

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
            if (a == 0)    // || a < 18
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
                    new KeyValuePair<int, string>(0, "By Booth No"),
                   new KeyValuePair<int, string>(1, "Booth Wise"),
                   new KeyValuePair<int, string>(2, "Paguthi Wise"),
                   new KeyValuePair<int, string>(3, "Panchayat Wise"),
                   new KeyValuePair<int, string>(33, "Order By Paguthi Type"),
                   new KeyValuePair<int, string>(4, "By Voters"),
                   new KeyValuePair<int, string>(5, "By Male"),
                   new KeyValuePair<int, string>(6, "By Female"),
                   new KeyValuePair<int, string>(7, "By 20"),
                    new KeyValuePair<int, string>(8, "By 30"),
                     new KeyValuePair<int, string>(9, "By 40"),
                      new KeyValuePair<int, string>(10, "By 50"),
                       new KeyValuePair<int, string>(11, "By 60"),
                       new KeyValuePair<int, string>(12, "By Above60"),
                       new KeyValuePair<int, string>(13, "By AgeWisePerc"),
                   //new KeyValuePair<int, string>(7, "By 20"),
                   //new KeyValuePair<int, string>(7, "Delete Page"),
                   //new KeyValuePair<int, string>(8, "> 1 Deleted Page"),
                   //new KeyValuePair<int, string>(9, "May Error Page"),
                   //new KeyValuePair<int, string>(10, "Name Issue"),
                   //new KeyValuePair<int, string>(11, "FNAME Issue"),
                   //new KeyValuePair<int, string>(12, "Address Issue"),
                   //new KeyValuePair<int, string>(13, "Age Issue"),
                   //new KeyValuePair<int, string>(14, "Gender Issue"),
                   //new KeyValuePair<int, string>(15, "Manual Edit"),
               };


            //var myKeyValuePair = new List<KeyValuePair<int, string>>()
            //   {

            //       new KeyValuePair<int, string>(1, "Any Null?"),
            //       new KeyValuePair<int, string>(2, "Name Null?"),
            //       new KeyValuePair<int, string>(3, "FNAME Null?"),
            //       new KeyValuePair<int, string>(4, "Address Null?"),
            //       new KeyValuePair<int, string>(5, "Age Null?"),
            //       new KeyValuePair<int, string>(6, "Gender Null?"),
            //       new KeyValuePair<int, string>(7, "Delete Page"),
            //       new KeyValuePair<int, string>(8, "> 1 Deleted Page"),
            //       new KeyValuePair<int, string>(9, "May Error Page"),
            //       new KeyValuePair<int, string>(10, "Name Issue"),
            //       new KeyValuePair<int, string>(11, "FNAME Issue"),
            //       new KeyValuePair<int, string>(12, "Address Issue"),
            //       new KeyValuePair<int, string>(13, "Age Issue"),
            //       new KeyValuePair<int, string>(14, "Gender Issue"),
            //       new KeyValuePair<int, string>(15, "Manual Edit"),
            //   };

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
            VotePercDetail cus = grid.Rows[grid.CurrentCell.RowIndex].DataBoundItem as VotePercDetail;
            //Update data

            if (owningColumnName == "PaguthiEnum")
            {
                VotePercDetail.UpdatePaguthiEnum(cus, cellValue);
                // SetErrorDetail();
            }
            else if (owningColumnName == "PaguthiType")
            {
                VotePercDetail.UpdatePaguthiEnum(cus, cellValue);
                // SetErrorDetail();
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            cmbFIlter_SelectedIndexChanged(null, null);
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
            if (cmbAss.SelectedIndex <= 0)
            {
                dataGridView1.DataSource = cmbPaguthi.DataSource = cmbBooths.DataSource = null;
                cmbPaguthi.Enabled = cmbBooths.Enabled = false;
                return;
            }

            cmbPaguthi.Enabled = cmbBooths.Enabled = true;

            var fol = (KeyValuePair<string, string>)cmbAss.SelectedItem;

            LoadPaguthi(fol.Key.ToInt32());
            //LoadBooths(fol.Value);

            cmbFIlter.DataSource = GetOptions();
        }

        private void LoadBooths(string fullPath)
        {
            var allBoothFiles = (from f in Directory.GetFiles(fullPath).ToList()
                                 select new KeyValuePair<string, string>(new FileInfo(f).Name, f)).ToList();

            allBoothFiles.Insert(0, new KeyValuePair<string, string>("ALL", "--select--"));

            cmbBooths.DataSource = allBoothFiles;
            cmbBooths.ValueMember = "Value";
            cmbBooths.DisplayMember = "Key";
        }

        private void LoadPaguthi(int assemblyId)
        {
            var ondriums = BaseData.GetPaguthiForAssembly(assemblyId);

            //ondriums.Insert(0, new BaseData() { OndriumId = 0, OndriumName = "--select--" });  //_<string, string>("0", "--select--"));  ;
            ondriums.Insert(0, new BaseData() { OndriumId = 0, OndriumName = "ALL" });  //

            cmbPaguthi.DataSource = ondriums;

            //lblCount.Text = $"{ondriums.Count} in {selectedDisName}"; 

            cmbPaguthi.DisplayMember = "OndriumFullName";
            cmbPaguthi.ValueMember = "OndriumId";
            //this.cmbPaguthi.SelectedIndexChanged += CmbOndrium_SelectedIndexChanged;
        }


        private void cmbPaguthi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaguthi.SelectedIndex < 0) return;

            var baseData = (BaseData)cmbPaguthi.SelectedItem;
            var assemblyId = ((KeyValuePair<string, string>)cmbAss.SelectedItem).Key.ToInt32();

            resultForSearch = VotePercDetail.GetForAssembly(assemblyId);

            List<VotePercDetail> filteredOne;
            if (cmbPaguthi.SelectedIndex == 0) // ALL
            {
                filteredOne = resultForSearch;
            }
            else
            {
                filteredOne = resultForSearch.Where(w => w.OndriumNo == baseData.OndriumId).ToList(); //VotePercDetail.GetForOndrium(baseData.OndriumId);
            }

            dataGridView1.DataSource = filteredOne;

        }

        List<VotePercDetail> resultForSearch;

        private void cmbBooths_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbBooths.SelectedIndex < 0)
            //    return;



            //if (cmbBooths.SelectedIndex == 0)
            //{
            //    var assemblyId = ((KeyValuePair<string, string>)cmbAss.SelectedItem).Key.ToInt32();
            //    resultForSearch = VotePercDetail.GetForAssembly(assemblyId);
            //}
            //else
            //{
            //    var keyValue = (KeyValuePair<string, string>)cmbBooths.SelectedItem;
            //    var boothNo = keyValue.Key.Split('-')[1].Split('.')[0].ToInt32();
            //    resultForSearch = VotePercDetail.GetForBooth(boothNo);
            //}

            //dataGridView1.DataSource = resultForSearch;

        }

        private int GetAgeCount(List<int> ages, int st, int en)
        {
            return ages.Count(c => c >= st && c <= en);
        }

        private int GetLessAgeCount(List<int> ages, int age)
        {
            return ages.Count(c => c <= age);
        }

        private int GetMoreAgeCount(List<int> ages, int age)
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


        private void btnSaveReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.DataSource == null)
                {
                    MessageBox.Show("select assembly first!");
                    return;
                }

                if (txtReportName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("provide file name");
                    return;
                }
                // CSV
                var gridData = (dataGridView1.DataSource as List<VotePercDetail>);

                var reportName = $"{txtReportName.Text.Trim()}";


                var reportPath = Path.Combine(cmbAss.SelectedValue.ToString(), "Report");
                General.CreateFolderIfNotExist(reportPath);

                var filePath = Path.Combine(cmbAss.SelectedValue.ToString(), $"{reportName}.csv");
                General.CreateFileIfNotExist(filePath);

                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    CreateHeader(gridData, sw);
                    CreateRows(gridData, sw);
                }

                // TXT
                var sbList = new StringBuilder();

                //var assemblyTotalVotes = resultForSearch.Sum(s => s.Total);

                var areaTotalVotes = gridData.Sum(s => s.Total);

                var areaMaleVotes = gridData.Sum(s => s.Male);
                var areaFemaleVotes = gridData.Sum(s => s.Female);
                var areaThirdVotes = gridData.Sum(s => s.Third);
                var maleP = PercInDec(areaMaleVotes, areaTotalVotes) + "%";
                var femaleP = PercInDec(areaFemaleVotes, areaTotalVotes) + "%";
                var thirdP = PercInDec(areaThirdVotes, areaTotalVotes) + "%";

                var t20 = gridData.Sum(s => s.to20);
                var t30 = gridData.Sum(s => s.to30);
                var t40 = gridData.Sum(s => s.to40);
                var t50 = gridData.Sum(s => s.to50);
                var t60 = gridData.Sum(s => s.to60);
                var above60 = gridData.Sum(s => s.Above60);


                sbList.AppendLine($"TOTAL VOTES: {areaTotalVotes.TokFormat()} (100%)");
                sbList.AppendLine($"MALE VOTES: {areaMaleVotes.TokFormat()} ({PercInDec(areaMaleVotes, areaTotalVotes)}%)");
                sbList.AppendLine($"FEMALE VOTES: {areaFemaleVotes.TokFormat()} ({PercInDec(areaFemaleVotes, areaTotalVotes)}%)");
                //sbList.AppendLine($"THIRD VOTES: {areaThirdVotes.TokFormat()} ({PercInDec(areaThirdVotes, areaTotalVotes)}%)");



                foreach (int i in Enum.GetValues(typeof(PaguthiEnum)))
                {
                    if (i == 0) continue;

                    var d = gridData.Where(w => w.PaguthiEnum == (PaguthiEnum)i).Sum(s => s.Total);

                    sbList.AppendLine($"{(PaguthiEnum)i}: {d.TokFormat()} ({PercInDec(d, areaTotalVotes)}%)");
                }

                var Odata = (from gd in gridData
                             group gd by gd.PaguthiEnum into ng
                             select ng).OrderBy(o => o.Key).ToList();

                Odata.ForEach(pe =>
                {

                    if (pe.First().PaguthiType == PaguthiType.N || pe.First().PaguthiType == PaguthiType.P)
                    {
                        var ondriumVoteCount = pe.ToList().Where(w => w.OndriumNo == w.OndriumNo).Sum(s => s.Total);
                        sbList.AppendLine($"{pe.Key.GetStringValue()} ({ondriumVoteCount} - {PercInDec(ondriumVoteCount, areaTotalVotes)}%)");
                        sbList.AppendLine($"-----------------------");
                    }
                    else
                    {
                        var ondriumVoteCount = pe.ToList().Where(w => w.OndriumNo == w.OndriumNo).Sum(s => s.Total);
                        sbList.AppendLine($"{pe.Key.GetStringValue()} ({ondriumVoteCount} - {PercInDec(ondriumVoteCount, areaTotalVotes)}%)");
                        sbList.AppendLine($"-----------------------");

                        var pData = pe.DistinctBy(d => d.PanchayatNo).ToList();

                        pData.ToList().ForEach(o =>
                        {
                            var pName = BaseData.GetPanchayatName(o.OndriumNo, o.PanchayatNo);
                            var voteCount = pe.ToList().Where(w => w.PanchayatNo == o.PanchayatNo).Sum(s => s.Total);
                            sbList.AppendLine($"{pName} ({voteCount} - {PercInDec(voteCount, areaTotalVotes)}%)");
                        });
                    }
                });





            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private static void CreateHeader<T>(List<T> list, StreamWriter sw)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length - 1; i++)
            {
                sw.Write(properties[i].Name + ",");
            }
            var lastProp = properties[properties.Length - 1].Name;
            sw.Write(lastProp + sw.NewLine);
        }

        private static void CreateRows<T>(List<T> list, StreamWriter sw)
        {
            foreach (var item in list)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length - 1; i++)
                {
                    var prop = properties[i];
                    sw.Write(prop.GetValue(item) + ",");
                }
                var lastProp = properties[properties.Length - 1];
                sw.Write(lastProp.GetValue(item) + sw.NewLine);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (cmbAss.SelectedIndex <= 0)
            {
                MessageBox.Show("Select assembly to update.");
                return;
            }

            int assemblyNo = ((KeyValuePair<string, string>)cmbAss.SelectedItem).Key.ToInt32();

            var votePerc = VotePercDetail.GetForAssembly(assemblyNo).ToList();

            if (votePerc.Count > 0)
            {

                var assemblyBoothLink = AssemblyBoothLink.GetForAssembly(assemblyNo).ToList();

                if (assemblyBoothLink.Count == 0)
                {
                    MessageBox.Show("Nothing to update.");
                    return;
                }

                MessageBox.Show($"{assemblyBoothLink.Count} - booth to update");

                assemblyBoothLink.ForEach(fef =>
                {

                    VotePercDetail.UpdatePaguthiDetails(fef.AssemblyNo, fef.PaguthiNo, fef.PanchayatId, fef.PaguthiType, fef.BoothNo);

                });



            }


        }


        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (resultForSearch == null)
            {
                MessageBox.Show("select assembly first!");
                return;
            }

            if (txtFromBooth.Text.Trim() == "")
            {
                MessageBox.Show("provide both from and to booth numbers.");
                return;
            }

            if (txtToBooth.Text.Trim() == "") txtToBooth.Text = txtFromBooth.Text;

            var filtered = resultForSearch.Where(w => w.BoothNo >= txtFromBooth.Text.ToInt32() && w.BoothNo <= txtToBooth.Text.ToInt32()).ToList();

            dataGridView1.DataSource = filtered;


        }

        private void button6_Click(object sender, EventArgs e)
        {
            var pt = (cmbPaguthi.SelectedItem as BaseData);
            VotePercDetail.UpdatePaguthiType(pt, PaguthiEnum.MANP);
        }

        private void cmbFIlter_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            var totalVotes = resultForSearch.Sum(s => s.Total);

            var value = ((KeyValuePair<int, string>)cmbFIlter.SelectedItem).Key;

            if (value == 0)
            {
                dataGridView1.DataSource = resultForSearch.OrderBy(o => o.BoothNo).ToList();
            }
            else if (value == 1)
            {
                dataGridView1.DataSource = resultForSearch.OrderByDescending(o => o.Total).ToList();
            }
            else if (value == 2)
            {
                int sNo = 1;
                dataGridView1.DataSource = (from r in resultForSearch
                                            group r by r.PaguthiEnum into ng
                                            select new
                                            {
                                                SerailNo = 0,
                                                Paguthi = ng.Key,
                                                Vote = ng.Sum(s => s.Total).TokFormat(),
                                                Perc = PercInDec(ng.Sum(s => s.Total), totalVotes)
                                            }).OrderByDescending(o => o.Perc).ToList().Select(s => new
                                            {
                                                SerailNo = sNo++,
                                                s.Paguthi,
                                                s.Vote,
                                                s.Perc
                                            }).ToList();
            }
            else if (value == 3)
            {
                int sNo = 1;
                var d = (from r in resultForSearch
                         group r by new { r.OndriumNo, r.PanchayatNo } into ng
                         select new
                         {
                             SerailNo = 0,
                             Ondrium = ng.First().PaguthiEnum,
                             Panchayat = BaseData.GetPanchayatName(ng.Key.OndriumNo, ng.Key.PanchayatNo),
                             Vote = ng.Sum(s => s.Total).TokFormat(),
                             Perc = PercInDec(ng.Sum(s => s.Total), totalVotes)
                         }).OrderByDescending(o => o.Perc).ToList().Select(s => new
                         {
                             SerailNo = sNo++,
                             s.Ondrium,
                             s.Panchayat,
                             s.Vote,
                             s.Perc
                         }).ToList();

                dataGridView1.DataSource = d.ToList();

            }

            else if (value == 33)
            {
                int sNo = 1;
                var d = (from r in resultForSearch
                         group r by new { r.OndriumNo, r.PanchayatNo } into ng
                         select new
                         {
                             SerailNo = 0,
                             Ondrium = ng.First().PaguthiEnum,
                             Panchayat = BaseData.GetPanchayatName(ng.First().OndriumNo, ng.First().PanchayatNo) + "  ",
                             Vote = ng.Sum(s => s.Total).TokFormat(),
                             Perc = PercInDec(ng.Sum(s => s.Total), totalVotes)
                         }).ToList();

                d = d.Select(s => new
                {
                    SerailNo = sNo++,
                    s.Ondrium,
                    s.Panchayat,
                    s.Vote,
                    s.Perc
                }).OrderBy(o => o.Ondrium).ToList();

                dataGridView1.DataSource = d.ToList();

            }
            else if (value == 4)
            {
                dataGridView1.DataSource = resultForSearch.OrderByDescending(o => o.Total).ToList();
            }
            else if (value == 5)
            {
                dataGridView1.DataSource = resultForSearch.OrderByDescending(o => o.Male).ToList();
            }
            else if (value == 6)
            {
                dataGridView1.DataSource = resultForSearch.OrderByDescending(o => o.Female).ToList();
            }
            else if (value == 7)
            {
                dataGridView1.DataSource = resultForSearch.OrderByDescending(o => o.to20).ToList();
            }
            else if (value == 8)
            {
                dataGridView1.DataSource = resultForSearch.OrderByDescending(o => o.to30).ToList();
            }
            else if (value == 9)
            {
                dataGridView1.DataSource = resultForSearch.OrderByDescending(o => o.to40).ToList();
            }
            else if (value == 10)
            {
                dataGridView1.DataSource = resultForSearch.OrderByDescending(o => o.to50).ToList();
            }
            else if (value == 11)
            {
                dataGridView1.DataSource = resultForSearch.OrderByDescending(o => o.to60).ToList();
            }
            else if (value == 12)
            {
                dataGridView1.DataSource = resultForSearch.OrderByDescending(o => o.Above60).ToList();
            }
            else if (value == 13)
            {
                var d =
                          new List<MyList>
                          {
                              new MyList() { Key = "18-20", Value = resultForSearch.Sum(s => s.to20).TokFormat(), Perc = Perc(resultForSearch.Sum(s => s.to20),totalVotes) },
                              new MyList() { Key = "21-30", Value = resultForSearch.Sum(s => s.to30).TokFormat(), Perc = Perc(resultForSearch.Sum(s => s.to30),totalVotes) },
                              new MyList() { Key = "31-40", Value = resultForSearch.Sum(s => s.to40).TokFormat(), Perc = Perc(resultForSearch.Sum(s => s.to40),totalVotes) },
                              new MyList() { Key = "41-50", Value = resultForSearch.Sum(s => s.to50).TokFormat(), Perc = Perc(resultForSearch.Sum(s => s.to50),totalVotes) },
                              new MyList() { Key = "51-60", Value = resultForSearch.Sum(s => s.to60).TokFormat(), Perc = Perc(resultForSearch.Sum(s => s.to60),totalVotes) },
                              new MyList() { Key = "60-க்கு மேல்", Value = resultForSearch.Sum(s => s.Above60).TokFormat(), Perc = Perc(resultForSearch.Sum(s => s.Above60),totalVotes) },
                          };

                dataGridView1.DataSource = d.OrderByDescending(o => o.Value).ToList();
            }
        }
    }

    public class MyList
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Perc { get; set; }
    }




}
