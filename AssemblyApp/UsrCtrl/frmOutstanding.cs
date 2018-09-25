using Common;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmOutstanding : UserControl
    {
        public frmOutstanding()
        {
            InitializeComponent();

            var json = File.ReadAllText(AppConfiguration.OutstandingFile);

            dynamic data = JsonConvert.DeserializeObject(json, typeof(object));

            dataGridView1.DataSource = data;
        }
    }
}
