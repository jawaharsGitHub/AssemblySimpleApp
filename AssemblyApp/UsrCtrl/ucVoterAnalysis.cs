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
        public ucVoterAnalysis()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var fullList = new List<VoterList>();

            var filePath = @"F:\NTK\jawa - 2021\211-RMD\Voter List For Analysis\ac210333.txt";

            var allPageContent = File.ReadAllText(filePath);

            var firstPage = allPageContent.Substring(0, allPageContent.IndexOf("சட்டமன்றத் தொகுதி எண் மற்றும் பெயர்"));

            var lastPage = allPageContent.Substring(allPageContent.IndexOf("வாக்காளர்களின் தொகுப்பு"));

            var onlyVotersPages = allPageContent.Substring(allPageContent.IndexOf("பக்கம் 2"), allPageContent.IndexOf("வாக்காளர்களின் தொகுப்பு") - allPageContent.IndexOf("பக்கம் 2"));

            var totalPages = firstPage.Substring(firstPage.IndexOf("மொத்த பக்கங்கள்"), firstPage.IndexOf("பக்கம்") - firstPage.IndexOf("மொத்த பக்கங்கள்"));

            var NoOfpagesToProcess = totalPages.Replace("மொத்த பக்கங்கள்", "").Replace("-", "").Trim().ToInt32() - 3; // -3 means - not consider page 1, page 2 and last page

            var exceptionText = new StringBuilder();

            var fileEntryPath = "";

            for (int i = 0; i < NoOfpagesToProcess; i++)
            {
                // processing page number
                var pageNumber = i + 3;

                var startIndex = IndexOf(onlyVotersPages, pageNumber - 1); //onlyVotersPages.IndexOf($"பக்கம் {i + 2}");
                var lastIndex = IndexOf(onlyVotersPages, pageNumber) - startIndex;

                var pageContent = onlyVotersPages.Substring(startIndex, lastIndex);

                var data = pageContent;

                bool mayHaveError = false;

                data = data.Replace("Photo", "").Replace("is", "").Replace("Available", "");  // Rempve Photo is AVailable.

                data = data.Replace("தந்தை பெயர்", "$FATHER")
                           .Replace("கணவர் பெயர்", "$HUSBAND")
                           .Replace("கணவர் பெய", "$HUSBAND-1:") // scenerio 1
                           .Replace("தாய் பெயர்", "$MOTHER")
                           .Replace("இதரர் பெயர்", "$OTHERS")
                           // NOTE: dont change this order of Replace
                           .Replace("பெயர்", "$NAME")  // Rempoe Photo is AVailable.

                           .Replace("வீட்டு எண்", "$ADDRESS")
                           .Replace("வீட்டு", "$ADDRESS:D")

                           .Replace("வயது", "$AGE")

                           .Replace("பாலினம்", "$SEX");

                var splittedData = data.Split('$').ToList();
                splittedData.RemoveRange(0, 3);

                // no of voters in a page
                var pageRecordCount = splittedData.Count; //

                var names = splittedData.Where(w => w.StartsWith("NAME")).ToList();


                var fatherOrHusband = splittedData.Where(
                                            w => w.StartsWith("FATHER") ||
                                            w.StartsWith("MOTHER") || w.StartsWith("HUSBAND") ||
                                            w.StartsWith("OTHERS")).ToList();

                if (fatherOrHusband.Count != names.Count)
                {
                    AddNameLog(exceptionText, pageNumber, $"FATHER COUNT MISMATCH - NAME:{names.Count} - AGE:{fatherOrHusband.Count}");
                    mayHaveError = true;
                }




                var address = splittedData.Where(w => w.StartsWith("ADDRESS")).ToList();

                if (address.Count != names.Count)
                {
                    mayHaveError = true;
                    AddNameLog(exceptionText, pageNumber, $"ADDRESS COUNT MISMATCH - NAME:{names.Count} - AGE:{address.Count}");

                    for (int index = 0; index < fatherOrHusband.Count; index++)
                    {
                        if (fatherOrHusband[index].Contains("HUSBAND-1:"))
                            address.Insert(index, "$ADDRESS:SDELETEDADDRESS");
                    }
                }

                var age = splittedData.Where(w => w.StartsWith("AGE")).ToList();
                age.RemoveAt(age.Count - 1);

                if (age.Count != names.Count)
                {
                    mayHaveError = true;
                    AddNameLog(exceptionText, pageNumber, $"AGE COUNT MISMATCH - NAME:{names.Count} - AGE:{age.Count}");
                }

                var sex = splittedData.Where(w => w.StartsWith("SEX")).ToList();

                if (sex.Count != names.Count)
                {
                    mayHaveError = true;
                    AddNameLog(exceptionText, pageNumber, $"SEX COUNT MISMATCH - NAME:{names.Count} - SEX:{sex.Count}");
                }


                var voterList = new List<VoterList>();



                fileEntryPath = $@"F:\NTK\VotersAnalysis\{pageNumber}.txt";

                // 1. Name
                names.ForEach(fe =>
                {
                    var nm = GetOne(fe);

                    voterList.Add(new VoterList()
                    {
                        Name = nm.Item1 ? nm.Item2 : AddNameLog(exceptionText, pageNumber, "#NERROR#"), //fe.Contains(":") ? fe.Split(':')[1] : fe.Split(' ')[1],
                        MayError = mayHaveError

                    });
                });

                for (int index = 0; index < voterList.Count; index++)
                {
                    voterList[index].PageNo = pageNumber;
                    voterList[index].RowNo = (index / 3) + 1;
                }

                var s = "";
                // 2. HorFName
                for (int index = 0; index < fatherOrHusband.Count; index++)
                {
                    var nm = GetOne(fatherOrHusband[index]);
                    voterList[index].HorFName = nm.Item1 ? nm.Item2 : AddLog(exceptionText, pageNumber, voterList[index]); //fatherOrHusband[index].Split(':')[1];
                }


                // 3. HomeAddress
                for (int index = 0; index < address.Count; index++)
                {
                    var nm = GetOne(address[index]);
                    voterList[index].HomeAddress = nm.Item1 ? nm.Item2 : AddLog(exceptionText, pageNumber, voterList[index]);  // address[index].Split(':')[1];
                }

                // 4. Age
                for (int index = 0; index < age.Count; index++)
                {
                    var nm = GetOne(age[index]);
                    try
                    {
                        try
                        {
                            voterList[index].Age = nm.Item1 ? nm.Item2.ToInt32() : -999;

                            if (voterList[index].Age == 0)
                                voterList[index].IsDeleted = true;
                        }
                        catch (Exception exc)
                        {
                            voterList[index].Age = 0;
                            voterList[index].IsDeleted = true;
                            if (sex.Count != names.Count)
                            {
                                sex.Insert(index, "SEX:DELETED-S");
                            }
                            exceptionText.AppendLine($"{pageNumber}-{voterList[index - 1]}-DELETED");

                        }

                        //age[index].Split(':')[1].ToInt32();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }

                // 5. Sex
                for (int index = 0; index < sex.Count; index++)
                {
                    var nm = GetTwo(sex[index]);
                    voterList[index].Sex = nm.Item1 ? nm.Item2 : AddLog(exceptionText, pageNumber, voterList[index]);
                }

                //Append to final list
                fullList.AddRange(voterList);
            }


            if (string.IsNullOrEmpty(exceptionText.ToString()) == false)
                File.AppendAllText(fileEntryPath, exceptionText.ToString());

            exceptionText.Clear();

            var fd = JsonConvert.SerializeObject(fullList, Formatting.Indented);

            MessageBox.Show("DONE!");

        }


        public (bool, string) GetOne(string content)
        {
            try
            {

                var d = content.Contains(":") ? content.Split(':')[1] : content.Split(' ')[1];
                return (true, d);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }

        }

        public (bool, string) GetTwo(string content)
        {
            try
            {
                var d = content.Contains(":") ?
                        content.Split(':')[1].TrimStart().Split(' ')[0] : content.Split(' ')[1];
                return (true, d);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }

        }

        private string AddNameLog(StringBuilder sb, int pn, string name)
        {
            sb.AppendLine($"{pn}-{name}");
            return $"{pn}-{name}";
        }

        private string AddLog(StringBuilder sb, int pn, VoterList vl)
        {
            sb.AppendLine($"{pn}-{vl}");
            return $"{pn}-{vl}";
        }


        private int IndexOf(string allContent, int pageNo)
        {
            return allContent.IndexOf($"பக்கம் {pageNo}");
        }

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
