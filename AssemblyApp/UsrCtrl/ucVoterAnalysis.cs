﻿using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucVoterAnalysis : UserControl
    {

        List<VoterList> fullList = new List<VoterList>();
        string folderPath = @"F:\NTK\VotersAnalysis\";
        string reProcessFile = "";
        BoothDetail bd;
        bool haveErrorinFile = false;
        int lastPageNumberToProcess;
        List<string> logs;


        public ucVoterAnalysis()
        {
            InitializeComponent();
        }

        string voterFilePath = "";
        string boothDetailPath = "";

        private void button1_Click(object sender, EventArgs e)
        {
            cmbFIlter.DataSource = GetOptions();

            reProcessFile = "";

            //int year = 2019;
            //var filePath = $@"{folderPath}ac210333.txt";

            int year = 2020;
            var filePath = $@"{folderPath}ac211200.txt";
            //string voterFilePath = "";
            //string boothDetailPath = "";

            try
            {
                var ufn = (new FileInfo(filePath)).Name.Split('.')[0];
                voterFilePath = Path.Combine(AppConfiguration.AssemblyVotersFolder, $"{ufn.Substring(2, 3)}");

                boothDetailPath = Path.Combine(voterFilePath, $"{ufn.Substring(5, 3)}-BoothDetail.json");
                if (File.Exists(boothDetailPath) == false)
                {
                    File.Create(boothDetailPath);
                }

                voterFilePath = Path.Combine(voterFilePath, $"{ufn.Substring(5, 3)}.json");

                if (File.Exists(voterFilePath) == false)
                {
                    File.Create(voterFilePath);
                }
                else
                {
                    if (chkDebugMode.Checked == false)
                    {
                        MessageBox.Show("Willload an existing data!!");
                        // Load and exit
                        fullList = VoterList.GetAll(voterFilePath);
                        dataGridView1.DataSource = fullList;
                        SetErrorDetail();
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid file name");
            }


            var allPageContent = File.ReadAllText(filePath);

            //Process First page
            var firstPage = allPageContent.Substring(0, allPageContent.IndexOf("சட்டமன்றத் தொகுதி எண் மற்றும் பெயர்"));

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
                var assembly2020 = fpSPlitted[2].Split(':')[1].Trim().Split('-');
                bd.AssemblyNo = assembly2020[0].Trim();
                bd.AssemblyName = assembly2020[1];
            }
            else
            {
                var assembly2019 = fpSPlitted[2].Split('-');
                bd.AssemblyNo = assembly2019[1].Trim();
                bd.AssemblyName = assembly2019[2];
            }


            bd.PartNo = fpSPlitted[3].Split(' ')[2].Trim();

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
                bd.PartPlaceName = fpSPlitted[8].Split('-')[2].Replace("பிரிவின் எண் மற்றும் பெயர்", "$").Split('$')[1].Trim();
            else
                bd.PartPlaceName = fpSPlitted[8].Split('-')[0].Replace("பிரிவின் எண் மற்றும் பெயர்", "$").Split('$')[1].Trim();

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


            bd.Type = fpSPlitted[9].Replace("வாக்குச் சாவடியின் விவரங்கள்", "").Trim();

            bd.PartLocationAddress = fpSPlitted[11].Replace("எண்ணிக்கை", "$").Split('$')[1].Split('4')[0].Trim();

            bd.StartNo = fpSPlitted[12].Replace("தொடங்கும் வரிசை எண்", "").Trim().ToInt32();

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
                bd.Male = forNo[4].ToInt32();
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

            BoothDetail.Save(bd, boothDetailPath);

            /*************************************************************/

            reProcessFile = $"{folderPath}{bd.AssemblyNo}\\{bd.PartNo}";

            var lastPage = allPageContent.Substring(allPageContent.IndexOf("வாக்காளர்களின் தொகுப்பு"));

            var onlyVotersPages = allPageContent.Substring(allPageContent.IndexOf("பக்கம் 2"), allPageContent.IndexOf("வாக்காளர்களின் தொகுப்பு") - allPageContent.IndexOf("பக்கம் 2"));

            var totalPages = firstPage.Substring(firstPage.IndexOf("மொத்த பக்கங்கள்"), firstPage.IndexOf("பக்கம்") - firstPage.IndexOf("மொத்த பக்கங்கள்"));

            var NoOfpagesToProcess = totalPages.Replace("மொத்த பக்கங்கள்", "").Replace("-", "").Trim().ToInt32() - 3; // -3 means - not consider page 1, page 2 and last page

            lastPageNumberToProcess = NoOfpagesToProcess + 2;

            logs = new List<string>();

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

                if (ProcessPage(pageNumber, pageContent) == true)
                {
                    haveErrorinFile = true;
                }
            }

            string flag = "OK";
            //LOG FILE IF EXCEPTION OCCURS
            if (logs.Count > 0 || haveErrorinFile)
            {
                var file = Path.Combine(reProcessFile, $"Log-{bd.AssemblyNo}-{bd.PartNo}-Exception.txt");

                var delData = logs.Where(w => w.StartsWith("DEL-")).Distinct().Reverse().ToList();
                var errData = logs.Where(w => w.StartsWith("DEL-") == false).ToList();

                StringBuilder logText = new StringBuilder();
                logText.AppendLine("==================DELETED RECORD========================================");
                delData.ForEach(fe => logText.AppendLine(fe.Trim()));
                logText.AppendLine("==================ERROR RECORD========================================");
                errData.ForEach(fe => logText.AppendLine(fe));


                General.WriteToFile(file, logText.ToString());
                flag = "ERRORFILE";
            }

            var file4 = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{DateTime.Now.ToLocalTime().ToString().Replace(":", "~")}-{flag}.txt");
            General.WriteToFile(file4, onlyVotersPages);

            if (chkDebugMode.Checked == false) // We should save ony in run modeNOT IN DEBUG MODE.
                VoterList.Save(fullList, voterFilePath);

            logs.Clear();

            // PROCESSED WHOLE JSON FILE.
            var fd = JsonConvert.SerializeObject(fullList, Formatting.Indented);
            var file3 = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{DateTime.Now.ToLocalTime().ToString().Replace(":", "~")}.json");
            General.WriteToFile(file3, fd);

            MessageBox.Show("DONE!");

            dataGridView1.DataSource = fullList;

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

        public (bool, string, bool) GetOne(string content)
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


        public (bool, string, bool) GetFNname(string content)
        {
            try
            {
                string d = content.Contains(":") ? content.Split(':')[1] : content.Split(' ')[1];
                var isEmpty = string.IsNullOrEmpty(d) || !d.Contains('-');
                return (true, d, isEmpty);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString(), false);
            }
        }

        public (bool, string, bool) GetGender(string content)
        {
            try
            {
                var d = content.Contains(":") ?
                        content.Split(':')[1].TrimStart().Split(' ')[0] : content.Split(' ')[1];
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
                return (true, d, isEmpty, isDel);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString(), false, isDel);
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
            if (chkPageList.SelectedIndex == 0)
            {
                MessageBox.Show("Select page to reprocess");
                return;
            }

            if (string.IsNullOrEmpty(txtMissingRow.Text.Trim()))
            {
                MessageBox.Show("Provide the deleted row number");
                return;
            }

            var selectedPage = chkPageList.SelectedValue.ToString().Split('-')[1].Trim().ToInt32();
            //Page - 6
            if (DialogResult.Yes ==
            MessageBox.Show($"You want to reprocess for Assembly - {bd.AssemblyName} Booth - {bd.PartNo} PageNo {selectedPage}", "", MessageBoxButtons.YesNoCancel))
            {
                var pageText = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{selectedPage}-{31}.txt");

                var text = File.ReadAllText(pageText);

                //if(ProcessPage(selectedPage, text, txtMissingRow.Text.ToInt32()))
                //{
                //    var jsonFile = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{selectedPage}-{lastPageNumberToProcess}-ReRun-Failed.json");
                //    var fd = JsonConvert.SerializeObject(voterList, Formatting.Indented);
                //    WriteToFile(jsonFile, fd);
                //}
                ProcessPage(selectedPage, text, txtMissingRow.Text.ToInt32());

            }

        }

        private bool ProcessPage(int pageNumber, string pageContent, int errorRowNumber = 0)
        {
            // processing page number

            /*
             * ONE DELETE
             * NO DELETE BUT DATA SWAPPED OR MISARRANGED
             * MORE THAN ONE DELETE? not yet done.
             */


            var data = pageContent;

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
            //           //.Replace("வீட்டு", "$ADDRESS:D")

            //           .Replace("வயது", "$G")

            //           .Replace("பாலினம்", "$X");

            var splittedData = data.Split('$').ToList();

            //var test = data.Where(w => w == '$').Count();

            //var sbb = new List<string>();

            //int si = 0;
            //for (int i = 0; i < 150; i++)
            //{
            //    si = data.IndexOf('$', si);
            //    var ddd = data.Substring(si, 2);
            //    si += 1;
            //    sbb.Add(ddd);
            //}


            //var str = new StringBuilder();

            //for (int j = 0; j < 10; j++)
            //{
            //    for (int i = 0; i < 15; i++)
            //    {
            //        str.Append(sbb[i].Replace("$", "") + " ");
            //    }
            //    str.Append(Environment.NewLine);

            //    sbb.RemoveRange(0, 15);

            //}



            splittedData.RemoveRange(0, 1);

            //if (lastPageNumberToProcess == pageNumber)
            //    splittedData.RemoveRange(0, 2);
            //else
            //    splittedData.RemoveRange(0, 3);

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

            if (isReProcess)
            {
                if (isNameIssue) names.Insert(insertIndex, "NN");
                if (isFNameIssue) fatherOrHusband.Insert(insertIndex, "NF");
                if (isAddressIssue) address.Insert(insertIndex, "ND");
                if (isAgeIssue) age.Insert(insertIndex, "NG");
                if (isSexIssue) sex.Insert(insertIndex, "NS");

                isNameIssue = (names.Count != recordCount);
                isFNameIssue = (fatherOrHusband.Count != recordCount);
                isAddressIssue = (address.Count != recordCount);
                isAgeIssue = (age.Count != recordCount);
                isSexIssue = (sex.Count != recordCount);
            }


            /* NAME */
            if (isNameIssue || isFNameIssue || isAddressIssue || isAgeIssue || isSexIssue)
                mayHaveError = true;

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
                var nm = GetOne(fe);
                var vl = new VoterList()
                {
                    Name = nm.Item1 ? nm.Item2 : AddNameLog(pageNumber, "#NERROR#"),
                    MayError = mayHaveError ? mayHaveError : (nm.Item3)

                };

                if (vl.MayError)
                    AddNameLog(pageNumber, $"NAME ERROR @ {voterList.Count + 1}");
                voterList.Add(vl);
            });


            //for (int index = 0; index < voterList.Count; index++)
            //{

            //    voterList[index].PageNo = pageNumber;
            //    voterList[index].RowNo = (index / 3) + 1;
            //    voterList[index].SNo = index + 1;

            //    // Process record by record 
            //    DoHFname(fatherOrHusband[index], pageNumber, voterList[index], index);
            //    DoAddress(address[index], pageNumber, voterList[index], index);
            //    DoAge(age[index], pageNumber, voterList[index], index);
            //    DoGender(sex[index], pageNumber, voterList[index], index);

            //}


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
                DoHFname(fatherOrHusband[index], pageNumber, voterList[index], index);

                if (voterList[index].MayError)
                    voterList.Where(w => w.RowNo == voterList[index].RowNo).ToList().ForEach(fe => fe.MayError = true);


                //var nm = GetOne(fatherOrHusband[index]);
                //voterList[index].IsManualEdit = fatherOrHusband[index].Contains("\r\n");
                //voterList[index].HorFName = nm.Item1 ? nm.Item2 : AddLog(pageNumber, voterList[index]);
                //if (voterList[index].MayError == false) voterList[index].MayError = nm.Item3;
                //if (voterList[index].MayError)
                //    AddNameLog(pageNumber, $"FATHERNAME ERROR @ {index + 1}");
            }


            // 3. HomeAddress
            for (int index = 0; index < address.Count; index++)
            {
                DoAddress(address[index], pageNumber, voterList[index], index);

                //var nm = GetAddress(address[index]);

                //voterList[index].HomeAddress = nm.Item1 ? nm.Item2 : AddLog(pageNumber, voterList[index]);
                //if (voterList[index].MayError == false) voterList[index].MayError = nm.Item3;
                //if (voterList[index].MayError)
                //    AddNameLog(pageNumber, $"HOMEADDRESS ERROR @ {index + 1}");

                //voterList[index].IsDeleted = nm.Item4;

                //if (nm.Item4)
                //    AddDeleteItemLog(pageNumber, voterList[index]);
            }

            // 4. Age
            for (int index = 0; index < age.Count; index++)
            {

                DoAge(age[index], pageNumber, voterList[index], index);

                //var nm = GetOne(age[index]);
                //if (voterList[index].MayError == false) voterList[index].MayError = nm.Item3;
                //if (voterList[index].MayError)
                //    AddNameLog(pageNumber, $"AGE ERROR @ {index + 1}");
                //var a = 0;

                //try
                //{
                //    a = nm.Item2.ToInt32();
                //}
                //catch (Exception)
                //{

                //}

                //if (a == 0)
                //{
                //    voterList[index].IsDeleted = true;
                //    AddDeleteItemLog(pageNumber, voterList[index]);
                //}

                //voterList[index].Age = nm.Item1 ? a : -999;
            }

            // 5. Sex
            for (int index = 0; index < sex.Count; index++)
            {
                DoGender(sex[index], pageNumber, voterList[index], index);
                //var nm = GetTwo(sex[index]);
                //voterList[index].Sex = nm.Item1 ? nm.Item2 : AddLog(pageNumber, voterList[index]);
                //if (voterList[index].MayError)
                //    AddNameLog(pageNumber, $"SEX ERROR @ {index + 1}");

                //if (voterList[index].MayError == false) voterList[index].MayError = nm.Item3;
            }

            //Append to final list
            fullList.AddRange(voterList);

            if (errorRowNumber > 0)
            {
                var jsonFile = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}-ReRun.json");
                var fd = JsonConvert.SerializeObject(voterList, Formatting.Indented);
                General.WriteToFile(jsonFile, fd);
                logs.Clear();
            }


            if (voterList.Any(a => a.MayError))
            {
                if (Directory.Exists(reProcessFile) == false)
                    Directory.CreateDirectory(reProcessFile);

                var file = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}.txt");
                General.WriteToFile(file, pageContent);

                //write as json
                var jsonFile = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}-Error.json");
                var fd = JsonConvert.SerializeObject(voterList, Formatting.Indented);
                General.WriteToFile(jsonFile, fd);
            }

            return voterList.Any(a => a.MayError);
        }

        private void DoHFname(string fName, int pn, VoterList vl, int ind)
        {
            var nm = GetFNname(fName);
            vl.HorFName = nm.Item1 ? nm.Item2 : AddLog(pn, vl);
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
            var nm = GetOne(age);
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
    }
}
