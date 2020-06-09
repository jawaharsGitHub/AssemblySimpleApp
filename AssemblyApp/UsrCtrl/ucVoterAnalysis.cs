using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Common.ExtensionMethod;
using Newtonsoft.Json;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucVoterAnalysis : UserControl
    {

        List<VoterList> fullList = new List<VoterList>();
        string folderPath = @"F:\NTK\VotersAnalysis\";
        string reProcessFile = "";
        BoothDetail bd;

        string fileEntryPath = "";
        int lastPageNumberToProcess;
        StringBuilder exceptionText;

        public ucVoterAnalysis()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            // var filePath = @"F:\NTK\jawa - 2021\211-RMD\Voter List For Analysis\ac210333.txt";

            //int year = 2019;
            //var filePath = @"F:\NTK\jawa - 2021\211-RMD\Voter List For Analysis\ac210333.txt";
            reProcessFile = "";
            int year = 2020;
            var filePath = @"F:\NTK\jawa - 2021\211-RMD\Voter List For Analysis\ac211200.txt";

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
                bd.PartPlaceName = fpSPlitted[8].Split('-')[2].Replace("பிரிவின் எண் மற்றும் பெயர்", "$").Split('$')[1];
            else
                bd.PartPlaceName = fpSPlitted[8].Split('-')[0].Replace("பிரிவின் எண் மற்றும் பெயர்", "$").Split('$')[1];

            var otherDetails = fpSPlitted[8].Split('-')[5].Split(' ');

            if (year == 2020)
            {
                bd.MainCityOrVillage = otherDetails[2];
                bd.Zone = otherDetails[3];
                bd.Birga = otherDetails[5];
                bd.PoliceStation = otherDetails[6];
                bd.Taluk = otherDetails[7];
                bd.District = fpSPlitted[8].Split('-')[6];
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

            bd.PartPlaceName = fpSPlitted[10];

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

            reProcessFile = $"{folderPath}{bd.AssemblyNo}\\{bd.PartNo}";

            var lastPage = allPageContent.Substring(allPageContent.IndexOf("வாக்காளர்களின் தொகுப்பு"));

            var onlyVotersPages = allPageContent.Substring(allPageContent.IndexOf("பக்கம் 2"), allPageContent.IndexOf("வாக்காளர்களின் தொகுப்பு") - allPageContent.IndexOf("பக்கம் 2"));

            var totalPages = firstPage.Substring(firstPage.IndexOf("மொத்த பக்கங்கள்"), firstPage.IndexOf("பக்கம்") - firstPage.IndexOf("மொத்த பக்கங்கள்"));

            var NoOfpagesToProcess = totalPages.Replace("மொத்த பக்கங்கள்", "").Replace("-", "").Trim().ToInt32() - 3; // -3 means - not consider page 1, page 2 and last page

            lastPageNumberToProcess = NoOfpagesToProcess + 2;

            exceptionText = new StringBuilder();




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

                ProcessPage(pageNumber, pageContent);
            }


            // After all page processed
            if (Directory.Exists(reProcessFile) == false)
                Directory.CreateDirectory(reProcessFile);

            if (string.IsNullOrEmpty(exceptionText.ToString()) == false)
            {
                var file = Path.Combine(reProcessFile, $"Log-{bd.AssemblyNo}-{bd.PartNo}.txt");
                ReplaceToFile(file, exceptionText.ToString());
            }
            else
            {
                var file4 = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{DateTime.Now.ToLocalTime().ToString().Replace(":", "~")}-OK.txt");
                ReplaceToFile(file4, onlyVotersPages);
            }

            exceptionText.Clear();

            var fd = JsonConvert.SerializeObject(fullList, Formatting.Indented);
            var file3 = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{DateTime.Now.ToLocalTime().ToString().Replace(":", "~")}.json");
            ReplaceToFile(file3, fd);

            MessageBox.Show("DONE!");

            dataGridView1.DataSource = fullList;

            var errorPages = (from fl in fullList
                              where fl.MayError
                              group fl by fl.PageNo into ng
                              select "Page-" + ng.Key.ToString()).ToList();

            errorPages.Insert(0, "--SELECT PAGE--");

            chkPageList.DataSource = errorPages;


        }

        private void ReplaceToFile(string path, string content)
        {
            if (File.Exists(path))
                File.Delete(path);

            File.WriteAllText(path, content);

        }


        public (bool, string, bool) GetOne(string content)
        {
            try
            {

                var d = content.Contains(":") ? content.Split(':')[1] : content.Split(' ')[1];

                var isEmpty = string.IsNullOrEmpty(d);

                if (isEmpty)
                {

                }

                return (true, d, isEmpty);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString(), false);
            }

        }

        public (bool, string, bool) GetTwo(string content)
        {
            try
            {
                var d = content.Contains(":") ?
                        content.Split(':')[1].TrimStart().Split(' ')[0] : content.Split(' ')[1];
                var isEmpty = string.IsNullOrEmpty(d);

                if (isEmpty)
                {

                }
                return (true, d, isEmpty);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString(), false);
            }

        }

        private string AddNameLog(StringBuilder sb, int pn, string name)
        {
            sb.AppendLine($"PageNo:{pn}-{name}");
            return $"{pn}-{name}";
        }

        private string AddLog(StringBuilder sb, int pn, VoterList vl)
        {
            sb.AppendLine($"PageNo:{vl.PageNo}-RowNo:{vl.RowNo}-{vl}");
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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

                ProcessPage(selectedPage, text, txtMissingRow.Text.ToInt32());
            }

        }

        private void ProcessPage(int pageNumber, string pageContent, int errorRowNumber = 0)
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
            fileEntryPath = $@"{folderPath}{bd.AssemblyNo}{bd.PartNo}{bd.AssemblyNo}{bd.PartNo}{pageNumber}.txt";

            var recordCount = (from p in data.Split(' ').ToList()
                               where p.Contains("Photo")
                               select p).ToList().Count;




            data = data.Replace("Photo", "").Replace("is", "").Replace("Available", "");  // Rempve Photo is AVailable.

            data = data.Replace("தந்தை பெயர்", "$FATHER")
                       .Replace("கணவர் பெயர்", "$HUSBAND")
                       //.Replace("கணவர் பெய", "$HUSBAND-1:") // scenerio 1
                       .Replace("தாய் பெயர்", "$MOTHER")
                       .Replace("இதரர் பெயர்", "$OTHERS")
                       // NOTE: dont change this order of Replace
                       .Replace("பெயர்", "$NAME")  // Rempoe Photo is AVailable.

                       .Replace("வீட்டு எண்", "$ADDRESS")
                       .Replace("வீட்டு என்", "$ADDRESS")
                        .Replace("வீட்டு தன்", "$ADDRESS")
                        .Replace("வீட்டு பண்", "$ADDRESS")
                        .Replace("வீட்டு கண்", "$ADDRESS")
                       //.Replace("வீட்டு", "$ADDRESS:D")

                       .Replace("வயது", "$AGE")

                       .Replace("பாலினம்", "$SEX");

            var splittedData = data.Split('$').ToList();

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
                AddNameLog(exceptionText, pageNumber, $"NAME COUNT MISMATCH - recordCount:{names.Count} - NAME:{recordCount}");

            /* FATHER-NAME */
            if (isFNameIssue)
                AddNameLog(exceptionText, pageNumber, $"FATHER COUNT MISMATCH - recordCount:{names.Count} - FATHER:{fatherOrHusband.Count}");

            /* ADDRESS */
            if (isAddressIssue)
                AddNameLog(exceptionText, pageNumber, $"ADDRESS COUNT MISMATCH - recordCount:{names.Count} - ADDRESS:{address.Count}");

            /* AGE */
            if (isAgeIssue)
                AddNameLog(exceptionText, pageNumber, $"AGE COUNT MISMATCH - recordCount:{recordCount} - AGE:{age.Count}");

            /* SEX */
            if (isSexIssue)
                AddNameLog(exceptionText, pageNumber, $"SEX COUNT MISMATCH - recordCount:{recordCount} - SEX:{sex.Count}");

            var voterList = new List<VoterList>();



            // 1. Name
            names.ForEach(fe =>
            {
                var nm = GetOne(fe);


                voterList.Add(new VoterList()
                {
                    Name = nm.Item1 ? nm.Item2 : AddNameLog(exceptionText, pageNumber, "#NERROR#"),
                    MayError = mayHaveError ? mayHaveError : (nm.Item3)

                });
            });

            for (int index = 0; index < voterList.Count; index++)
            {
                voterList[index].PageNo = pageNumber;
                voterList[index].RowNo = (index / 3) + 1;
            }

            // 2. HorFName
            for (int index = 0; index < fatherOrHusband.Count; index++)
            {
                var nm = GetOne(fatherOrHusband[index]);
                voterList[index].HorFName = nm.Item1 ? nm.Item2 : AddLog(exceptionText, pageNumber, voterList[index]);
                if (voterList[index].MayError == false) voterList[index].MayError = nm.Item3;

            }


            // 3. HomeAddress
            for (int index = 0; index < address.Count; index++)
            {
                var nm = GetOne(address[index]);
                voterList[index].HomeAddress = nm.Item1 ? nm.Item2 : AddLog(exceptionText, pageNumber, voterList[index]);
                if (voterList[index].MayError == false) voterList[index].MayError = nm.Item3;
            }

            // 4. Age
            for (int index = 0; index < age.Count; index++)
            {
                var nm = GetOne(age[index]);
                if (voterList[index].MayError == false) voterList[index].MayError = nm.Item3;
                var a = 0;

                try
                {
                    a = nm.Item2.ToInt32();
                }
                catch (Exception)
                {

                }

                if (a == 0) voterList[index].IsDeleted = true;

                voterList[index].Age = nm.Item1 ? a : -999;
            }

            // 5. Sex
            for (int index = 0; index < sex.Count; index++)
            {
                var nm = GetTwo(sex[index]);
                voterList[index].Sex = nm.Item1 ? nm.Item2 : AddLog(exceptionText, pageNumber, voterList[index]);

                if (voterList[index].MayError == false) voterList[index].MayError = nm.Item3;
            }

            //Append to final list
            fullList.AddRange(voterList);

            if (errorRowNumber > 0)
            {
                var jsonFile = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}-ReRun.json");
                var fd = JsonConvert.SerializeObject(voterList, Formatting.Indented);
                ReplaceToFile(jsonFile, fd);
            }


            if (voterList[0].MayError)
            {
                if (Directory.Exists(reProcessFile) == false)
                    Directory.CreateDirectory(reProcessFile);

                var file = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}.txt");
                ReplaceToFile(file, pageContent);

                //write as json
                var jsonFile = Path.Combine(reProcessFile, $"{bd.AssemblyNo}-{bd.PartNo}-{pageNumber}-{lastPageNumberToProcess}-Error.json");
                var fd = JsonConvert.SerializeObject(voterList, Formatting.Indented);
                ReplaceToFile(jsonFile, fd);

            }
        }
    }


    public class BoothDetail
    {
        public string PartNo { get; set; }

        public string PartPlaceName { get; set; }
        public string PartLocationAddress { get; set; }

        public string Type { get; set; }

        public string AssemblyName { get; set; }

        public string ParlimentNo { get; set; }

        public string ParlimentName { get; set; }

        public string AssemblyNo { get; set; }

        public string EligibilityDay { get; set; }

        public string ReleaseDate { get; set; }

        public string MainCityOrVillage { get; set; }

        public string Zone { get; set; }

        public string Birga { get; set; }

        public string PoliceStation { get; set; }

        public string Taluk { get; set; }

        public string District { get; set; }

        public int Pincode { get; set; }

        public int StartNo { get; set; }
        public int EndNo { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int ThirdGender { get; set; }

        public int TotalVoters { get; set; }

        //// Last Page Data

        //// Base
        //public int BaseMale { get; set; }
        //public int BaseFemale { get; set; }
        //public int BaseThirdGender { get; set; }
        //public int BaseTotalVoters { get; set; }


        //// ADD
        //public int AddMale { get; set; }
        //public int AddFemale { get; set; }
        //public int AddThirdGender { get; set; }
        //public int AddTotalVoters { get; set; }

        //// DELETE
        //public int DeleteMale { get; set; }
        //public int DeleteFemale { get; set; }
        //public int DeleteThirdGender { get; set; }
        //public int DeleteTotalVoters { get; set; }

        //// TOTAL
        //public int TotalMale { get; set; }
        //public int TotalFemale { get; set; }
        //public int TotalThirdGender { get; set; }
        //public int TotalTotalVoters { get; set; }


    }

    public class VoterList
    {
        public string Name { get; set; }
        public string HorFName { get; set; }

        public string HomeAddress { get; set; }

        public int Age { get; set; }

        public string Sex { get; set; }

        public int PageNo { get; set; }

        public int RowNo { get; set; }

        public bool IsDeleted { get; set; }

        public bool MayError { get; set; }

        public override string ToString()
        {
            return $"{Name}-{HorFName}-{HomeAddress}-{Age}-{Sex}";
        }
    }

}



/*
var d3 = allPageContent.Replace("தந்தை பெயர்", "FATHERNAME:");
var d1 = d3.Replace("தந்தை பெயர்:", "FATHERNAME:");
var d111 = d3.Replace("FATHERNAME::", "FATHERNAME:");
//var d2 = d1.Replace("தந்தை பெயர் ", "FATHERNAME:");


var d4 = d111.Replace("கணவர் பெயர்", "HUSBANDNAME:");
var d5 = d4.Replace("கணவர் பெயர்:", "HUSBANDNAME:");
var d6 = d5.Replace("HUSBANDNAME::", "HUSBANDNAME:");

var d7 = d6.Replace("பெயர்", "MYNAME:");
var d8 = d7.Replace("பெயர்:", "MYNAME:");
var d9 = d8.Replace("MYNAME::", "MYNAME:");

var d13 = d9.Replace("வீட்டு எண்", "HOMENO:");
var d14 = d13.Replace("வீட்டு எண்:", "HOMENO:");
var d15 = d14.Replace("HOMENO::", "HOMENO:");

var d16 = d15.Replace("வயது", "AGE:");
var d17 = d16.Replace("வயது:", "AGE:");
var d18 = d17.Replace("AGE::", "AGE:");

var d19 = d18.Replace("பாலினம்", "SEX:");
var d20 = d19.Replace("பாலினம்:", "SEX:");
var d21 = d20.Replace("SEX::", "SEX:");

// Father Name
var myFNames = d21.Replace("FATHERNAME:", "~");
myFNames = myFNames.Replace("HUSBANDNAME:", "~");

var mns = myFNames.Split('~').ToList();

var myFNameList = new List<VoterList>();

mns.ForEach(fe =>
{
    myFNameList.Add(
        new VoterList() { ForHName = fe.TrimStart().Split(' ').FirstOrDefault() }
        );

});

// My Name

// Father Name
var myNames = d21.Replace("MYNAME:", "~");
//myNames = myNames.Replace("HUSBANDNAME:", "~");

var mns1 = myNames.Split('~').ToList();

if (mns1.Count != myFNameList.Count)
{
    MessageBox.Show("Wrong!!");
}

for (int i = 0; i < mns1.Count; i++)
{
    myFNameList[i].Name = mns1[i].TrimStart().Split(' ').FirstOrDefault();
}

*/
