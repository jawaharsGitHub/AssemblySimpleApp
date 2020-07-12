using DocxToTextDemo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTK_Support
{
    public partial class DocxToTxt : Form
    {
        public DocxToTxt()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (DialogResult.OK == fbd.ShowDialog())
            {
                var allFIles = Directory.GetFiles(fbd.SelectedPath);

                allFIles.ToList().ForEach(fe =>
                {
                    DocxToText dtt = new DocxToText(fe);
                    var extractedText = dtt.ExtractText();
                }
                );
            }
        }
    }
}
