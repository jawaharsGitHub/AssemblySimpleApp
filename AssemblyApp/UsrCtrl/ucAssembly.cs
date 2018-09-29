using DataAccess.PrimaryTypes;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucAssembly : UserControl
    {
        List<Assembly> assemblies;

        public ucAssembly()
        {
            InitializeComponent();


            assemblies = Assembly.GetAll();

            dgvAssembly.DataSource = assemblies;
            LoadFilter();
        }

        private void LoadFilter()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "By AssemblyNo"),
                   new KeyValuePair<int, string>(2, "By Name"),
                   new KeyValuePair<int, string>(3, "By Category"),
                   new KeyValuePair<int, string>(4, "By Electors")

               };

            cmbFilter.DataSource = myKeyValuePair;
        }

        private void cmbFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)cmbFilter.SelectedItem).Key;
            List<Assembly> filteredAssemblies = assemblies;

            if (value == 1) filteredAssemblies = assemblies.OrderBy(o => o.AssemblyNo).ToList();

            else if (value == 2) filteredAssemblies = assemblies.OrderBy(o => o.Name).ToList();

            else if (value == 3) filteredAssemblies = assemblies.OrderByDescending(o => o.Category).ToList();

            else if (value == 4) filteredAssemblies = assemblies.OrderByDescending(o => o.Electors).ToList();

            filteredAssemblies.ForEach(a =>
            {
                a.SNo = filteredAssemblies.IndexOf(a) + 1;

            });


            dgvAssembly.DataSource = filteredAssemblies;

        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

            dgvAssembly.DataSource = assemblies.Where(w => w.Name.ToLower().Contains(textBox1.Text.ToLower())).ToList();

        }
    }
}
