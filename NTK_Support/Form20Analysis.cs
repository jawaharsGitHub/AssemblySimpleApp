using Common.ExtensionMethod;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTK_Support
{
    public partial class Form20Analysis : Form
    {
        public Form20Analysis()
        {
            InitializeComponent();

            //var fileName = "F:/Form20/AC209.pdf";

            //PdfReader reader = new PdfReader(fileName);
            //int intPageNum = reader.NumberOfPages;
            //string[] words;
            //string line;

            //for (int i = 1; i <= intPageNum; i++)
            //{
            //    var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

            //    words = text.Split('\n');
            //    for (int j = 0, len = words.Length; j < len; j++)
            //    {
            //        line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));
            //    }
            //}
        }


        static string GetUrlSource(string url)
        {

            // var client = new HttpClient();
            //client.DefaultRequestHeaders.Add("User-Agent", "C# console program");

            //var content = client.GetStringAsync(url);

            //return content;

            var client = new WebClient();
            client.Headers.Add("User-Agent", "C# console program");

            return client.DownloadString(url);
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

            var startBoothNo = Convert.ToInt32(txtStartAssemblyNo.Text);
            var LastBoothNo = Convert.ToInt32(txtEndAssemblyNo.Text);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");

            var content = GetUrlSource("https://www.elections.tn.gov.in/Form20_TNLA2021.aspx");


            bw.DoWork += (s, o) =>
            {
                for (int i = startBoothNo; i <= LastBoothNo; i++)
                {
                    var fileName = $"AC{i:D3}.pdf";

                    string sPattern = $"Form20_TNLA2021/dt[0-9]+/{fileName}";

                    var matches = Regex.Matches(content, sPattern);

                    if (matches.Count == 1)
                    {
                        try
                        {

                            btnProgress.BeginInvoke(new Action(() => btnProgress.Text = $"Processing {fileName}"));
                            WebClient webClient = new WebClient();
                            webClient.DownloadFile($"https://www.elections.tn.gov.in/{matches[0].Value}",
                                $"{folderPath}/{fileName}");

                            btnProgress.BeginInvoke(new Action(() => btnProgress.Text = $"Done {fileName}"));

                        }
                        catch (Exception ex)
                        {
                            sbDbError.AppendLine("FAILED:" + fileName);
                            label1.BeginInvoke(new Action(() => lblError.Text = sbDbError.ToString()));
                        }
                    }
                    else
                    {
                        MessageBox.Show($"{fileName} not found");
                    }

                }

                MessageBox.Show("Completed!");
            };

            bw.RunWorkerAsync();
        }

        private void btnBasicAna_Click(object sender, EventArgs e)
        {
            var fileName = $"F:/Form20/AC{txtAssemblyNo.Text}.pdf";
            var pdfCon = GetPdfContent(fileName);


        }

        private List<PdfContent> GetPdfContent(string fileName)
        {
            PdfReader reader = new PdfReader(fileName);
            int intPageNum = reader.NumberOfPages;
            string[] words;

            var result = new List<PdfContent>();

            PdfContent page = null;
            for (int i = 1; i <= intPageNum; i++)
            {
                page = new PdfContent
                {
                    PageNo = i
                };

                List<string> pdfPageContent = new List<string>();

                var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

                words = text.Split('\n');
                for (int j = 0, len = words.Length; j < len; j++)
                {
                    pdfPageContent.Add(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j])));
                }

                page.PageContentLines = pdfPageContent;

                result.Add(page);

            }

            return result;
        }
    }

    public class PdfContent
    {

        public int PageNo { get; set; }

        public List<string> PageContentLines { get; set; }

    }
}
