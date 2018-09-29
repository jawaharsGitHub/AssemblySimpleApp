using DataAccess.PrimaryTypes;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucAssembly : UserControl
    {
        public ucAssembly()
        {
            InitializeComponent();

            dgvAssembly.DataSource = Assembly.GetAll();
        }
    }
}
