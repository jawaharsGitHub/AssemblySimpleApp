using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            bw.DoWork += (s, o) =>
            {

                for (int i = startBoothNo; i <= LastBoothNo; i++)
                {
                    var fileName = $"ac{assNo}{i:D3}.pdf";

                    try
                    {
                        btnProgress.BeginInvoke(new Action(() => btnProgress.Text = $"Processing {fileName}"));

                        WebClient webClient = new WebClient();
                        webClient.DownloadFile($"https://www.elections.tn.gov.in/SSR2020_14022020/dt27/ac{assNo}/{fileName}",
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
    }
}
