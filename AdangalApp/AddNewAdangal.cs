using AdangalApp.AdangalTypes;
using Common;
using Common.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdangalApp
{
    partial class AddNewAdangal : Form
    {

        public AddNewAdangal()
        {
            InitializeComponent();
            BindDropdown(ddlLandTypes, GetLandTypes(), "DisplayMember", "Id");
        }
        public List<KeyValue> GetLandTypes()
        {
            var landTypeSource = new List<KeyValue>();
            landTypeSource.Add(new KeyValue() { Caption = "--select--", Id = -2 });
            //landTypeSource.Add(new KeyValue() { Caption = "ALL", Id = -1, Value = fullAdangalFromjson.Count });

            foreach (LandType rt in Enum.GetValues(typeof(LandType)))
            {
                landTypeSource.Add(new KeyValue()
                {
                    Caption = Enum.GetName(typeof(LandType), rt),
                    //Value = fullAdangalFromjson.Where(w => w.LandType == rt).Count(),
                    Id = (int)rt
                });
            }

            return landTypeSource;
        }

        public void BindDropdown(ComboBox cb, object dataSource, string DisplayMember, string ValueMember)
        {
            try
            {
                cb.DisplayMember = DisplayMember;
                cb.ValueMember = ValueMember;
                cb.DataSource = dataSource;
            }
            catch (Exception ex)
            {
                //LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }
        }


        private void btnAddNew_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtParappu.Text) == false && AdangalFn.IsValidParappu(txtParappu.Text) == false)
            {
                MessageBox.Show("invalid parappu");
                return;
            }

            if (string.IsNullOrEmpty(txtTheervai.Text) == false && AdangalFn.IsValidTheervai(txtTheervai.Text) == false)
            {
                MessageBox.Show("invalid theervai");
                return;
            }

            var newAdangal = new Adangal()
            {
                NilaAlavaiEn = txtNilaalavaiEN.Text.ToInt32(),
                UtpirivuEn = txtUtpirivu.Text,
                Parappu = txtParappu.Text,
                Theervai = txtTheervai.Text,
                OwnerName = txtOwnerName.Text,
                LandType = (LandType)(ddlLandTypes.SelectedItem as KeyValue).Id
            };

            if(DataAccess.IsAdangalAlreadyExist(newAdangal))
            {
                if(DialogResult.Yes == 
                    MessageBox.Show($"Already exist for {newAdangal.NilaAlavaiEn}-{newAdangal.UtpirivuEn}", "", MessageBoxButtons.YesNo))
                {
                    DataAccess.AddNewAdangalEvenExist(newAdangal);
                }
            }
            else
            {
                DataAccess.AddNewAdangal(newAdangal);
            }
            
            

            txtNilaalavaiEN.Text = "";
            txtUtpirivu.Text = "";
            txtParappu.Text = "";
            txtTheervai.Text = "";
            txtOwnerName.Text = "";
            ddlLandTypes.SelectedIndex = 0;

            txtNilaalavaiEN.Focus();

            lblMessage.Text = $"Added for {newAdangal.ToString()}";


        }
    }
}
