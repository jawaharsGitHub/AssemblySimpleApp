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

            for (int i = 0; i < NoOfpagesToProcess; i++)
            {
                // processing page number
                var pageNumber = i + 3;

                var startIndex = IndexOf(onlyVotersPages, pageNumber - 1); //onlyVotersPages.IndexOf($"பக்கம் {i + 2}");
                var lastIndex = IndexOf(onlyVotersPages, pageNumber) - startIndex;

                var pageContent = onlyVotersPages.Substring(startIndex, lastIndex);

                var data = pageContent;

                data = data.Replace("Photo", "").Replace("is", "").Replace("Available", "");  // Rempve Photo is AVailable.
                data = data.Replace("தந்தை பெயர்", "$FATHER")
                           .Replace("கணவர் பெயர்", "$HUSBAND")
                           .Replace("பெயர்", "$NAME")  // Rempoe Photo is AVailable.
                           .Replace("வீட்டு எண்", "$ADDRESS")
                           .Replace("வயது", "$AGE")
                           .Replace("பாலினம்", "$SEX");

                var splittedData = data.Split('$').ToList();
                splittedData.RemoveRange(0, 3);

                // no of voters in a page
                var pageRecordCount = splittedData.Count; //

                var names = splittedData.Where(w => w.StartsWith("NAME")).ToList();
                var fatherOrHusband = splittedData.Where(w => w.StartsWith("FATHER") || w.StartsWith("HUSBAND")).ToList();
                var address = splittedData.Where(w => w.StartsWith("ADDRESS")).ToList();
                var age = splittedData.Where(w => w.StartsWith("AGE")).ToList();
                age.RemoveAt(age.Count - 1);
                var sex = splittedData.Where(w => w.StartsWith("SEX")).ToList();

                var voterList = new List<VoterList>();

                var exceptionText = new StringBuilder();

                var fileEntryPath = @"C:\Users\Public\TestFolder\WriteText.txt";



                


                // 1. Name
                names.ForEach(fe =>
                {

                    voterList.Add(new VoterList()
                    {
                        Name = fe.Contains(":") ? fe.Split(':')[1] : fe.Split(' ')[1]
                    });
                });

                // 2. HorFName
                for (int index = 0; index < fatherOrHusband.Count; index++)
                {
                    voterList[index].HorFName = fatherOrHusband[index].Split(':')[1];
                }

                // 3. HomeAddress
                for (int index = 0; index < address.Count; index++)
                {
                    voterList[index].HomeAddress = address[index].Split(':')[1];
                }

                // 4. Age
                for (int index = 0; index < age.Count; index++)
                {
                    voterList[index].Age = age[index].Split(':')[1].ToInt32();
                }

                // 5. Sex
                for (int index = 0; index < sex.Count; index++)
                {
                    voterList[index].Sex =
                        sex[index].Contains(":") ?
                        sex[index].Split(':')[1].TrimStart().Split(' ')[0] : sex[index].Split(' ')[1];
                }

                fullList.AddRange(voterList);
                File.AppendAllText(fileEntryPath, exceptionText.ToString());
            }

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
