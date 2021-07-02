using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTK_Support
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            string folderPath = "";
            if (DialogResult.OK == fbd.ShowDialog())
            {
                folderPath = fbd.SelectedPath;
            }
            var sbDbError = new StringBuilder();

            var startBoothNo = Convert.ToInt32(txtStart.Text);
            var LastBoothNo = Convert.ToInt32(txtPartEndNo.Text);
            var assNo = txtAssNo.Text.Trim();
            //string releaseDat = "SSR2020_14022020";
            string releaseDat = "SSR2021_20012021";
            bw.DoWork += (s, o) =>
            {

                for (int i = startBoothNo; i <= LastBoothNo; i++)
                {
                    var fileName = $"ac{assNo}{i:D3}.pdf";

                    try
                    {
                        btnProgress.BeginInvoke(new Action(() => btnProgress.Text = $"Processing {fileName}"));
                        WebClient webClient = new WebClient();                       
                        webClient.DownloadFile($"https://www.elections.tn.gov.in/{releaseDat}/dt27/ac{assNo}/{fileName}",
                            $"{folderPath}/{fileName}");

                        btnProgress.BeginInvoke(new Action(() => btnProgress.Text = $"Done {fileName}"));

                    }
                    catch (Exception ex)
                    {
                        // sbDbError.AppendLine(fileName + ":" + ex.ToString());
                        sbDbError.AppendLine("FAILED:" + fileName);
                        label1.BeginInvoke(new Action(() => lblError.Text = sbDbError.ToString()));
                    }

                }

                MessageBox.Show("Completed!");
            };

            bw.RunWorkerAsync();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            object noEncodingDialog = "UTF8";
            string docPath = @"F:\NTK";
            string testFile = "ac211240.docx";

            //Document document = new Document();
            //document.LoadFromFile(@"D:\Work\Stephen\2011.12.05\Sample.doc");


            Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            Document document = application.Documents.Open(Path.Combine(docPath, testFile));
            application.ActiveDocument.SaveAs(Path.Combine(docPath, "ac211240_ed.txt"), WdSaveFormat.wdFormatUnicodeText); //,  ref noEncodingDialog);
            ((_Application)application).Quit();

            string readText = File.ReadAllText(Path.Combine(docPath, "ac211240_ed.txt"));

            byte[] bytes = Encoding.UTF8.GetBytes(readText);
            File.WriteAllBytes(Path.Combine(docPath, "ac211240_decoded.txt"), bytes);
        }
    }
}
