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
using DataAccess.PrimaryTypes;

namespace CenturyFinCorpApp
{
    public partial class DataHelper : UserControl
    {

        string[] lines;
        List<Assembly> assemblies = new List<Assembly>();

        public DataHelper()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // read file.

            string sPattern = "[0-9]+\\."; //@"^\d+\.$";

            var data = lines.ToList().Where(w => System.Text.RegularExpressions.Regex.IsMatch(w, sPattern)).ToList();
           

            data.ForEach(d =>
            {
                assemblies.Add(new Assembly() {
                    AssemblyNo = Convert.ToInt32(d.Split('.')[0]),
                    AssemblyName = d.Split('.')[1],
                    Category = d.Split('.')[1].Contains("(SC)") ? "SC" : ""
                });
            });

            // convert tp csv

            //var sb = new StringBuilder();
            //foreach (var a in assemblies.OrderBy(o => o.AssemblyNo).ToList())
            //{
            //    sb.AppendLine(a.AssemblyNo + "," + a.Name + "," + a.Category);
            //}




        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lines = File.ReadAllLines(openFileDialog1.FileName);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            var data = lines.ToList().Where(w => w.Contains("TOTAL ELECTORS")).ToList();

            if(assemblies.Count != data.Count)
            {
                MessageBox.Show("Sorry Data Mismatch");
                return;
            }

            for (int i = 0; i < data.Count; i++)
            {
                /// oldString.Substring(oldString.LastIndexOf("Files"));
                try
                {
                    assemblies[i].Electors = Convert.ToInt32(data[i].Substring(data[i].LastIndexOf("TOTAL ELECTORS")).Replace("TOTAL ELECTORS", "").Replace(":", "").Replace("\t", "").Trim());

                }
                catch (Exception)
                {

                    assemblies[i].Electors = 0000000;
                }

            }
            //data.ForEach(d =>
            //{
            //    var electors = d.Replace("TOTAL ELECTORS", "").Replace(":", "").Replace("\t", "").Trim();

            //});

            //// convert tp csv

            var sb = new StringBuilder();
            foreach (var a in assemblies.OrderBy(o => o.AssemblyNo).ToList())
            {
                sb.AppendLine(a.AssemblyNo + "," + a.AssemblyName + "," + a.Category + "," + a.Electors);
            }


        }
    }


    
}
