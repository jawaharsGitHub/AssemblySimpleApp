namespace CenturyFinCorpApp
{
    partial class frmCustomers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.rdbActive = new System.Windows.Forms.RadioButton();
            this.rdbClosed = new System.Windows.Forms.RadioButton();
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnAddCustomer = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFilters = new System.Windows.Forms.ComboBox();
            this.chkAllColumns = new System.Windows.Forms.CheckBox();
            this.lblRowCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(15, 114);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1237, 527);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            this.dataGridView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDoubleClick);
            // 
            // rdbActive
            // 
            this.rdbActive.AutoSize = true;
            this.rdbActive.Location = new System.Drawing.Point(24, 14);
            this.rdbActive.Name = "rdbActive";
            this.rdbActive.Size = new System.Drawing.Size(116, 17);
            this.rdbActive.TabIndex = 1;
            this.rdbActive.Tag = "RN";
            this.rdbActive.Text = "RUNNING NOTES";
            this.rdbActive.UseVisualStyleBackColor = true;
            this.rdbActive.CheckedChanged += new System.EventHandler(this.rdbActive_CheckedChanged);
            // 
            // rdbClosed
            // 
            this.rdbClosed.AutoSize = true;
            this.rdbClosed.Location = new System.Drawing.Point(168, 13);
            this.rdbClosed.Name = "rdbClosed";
            this.rdbClosed.Size = new System.Drawing.Size(88, 17);
            this.rdbClosed.TabIndex = 2;
            this.rdbClosed.Tag = "CN";
            this.rdbClosed.Text = "Closed Notes";
            this.rdbClosed.UseVisualStyleBackColor = true;
            this.rdbClosed.CheckedChanged += new System.EventHandler(this.rdbClosed_CheckedChanged);
            // 
            // rdbAll
            // 
            this.rdbAll.AutoSize = true;
            this.rdbAll.Location = new System.Drawing.Point(299, 14);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(67, 17);
            this.rdbAll.TabIndex = 3;
            this.rdbAll.Tag = "AN";
            this.rdbAll.Text = "All Notes";
            this.rdbAll.UseVisualStyleBackColor = true;
            this.rdbAll.CheckedChanged += new System.EventHandler(this.rdbAll_CheckedChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(507, 13);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(190, 20);
            this.txtSearch.TabIndex = 4;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(723, 13);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // btnAddCustomer
            // 
            this.btnAddCustomer.Location = new System.Drawing.Point(1126, 10);
            this.btnAddCustomer.Name = "btnAddCustomer";
            this.btnAddCustomer.Size = new System.Drawing.Size(102, 23);
            this.btnAddCustomer.TabIndex = 7;
            this.btnAddCustomer.Text = "Add Customer";
            this.btnAddCustomer.UseVisualStyleBackColor = true;
            this.btnAddCustomer.Click += new System.EventHandler(this.btnAddCustomer_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(938, 11);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1067, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "label1";
            // 
            // cmbFilters
            // 
            this.cmbFilters.DisplayMember = "value";
            this.cmbFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilters.FormattingEnabled = true;
            this.cmbFilters.Location = new System.Drawing.Point(24, 77);
            this.cmbFilters.Name = "cmbFilters";
            this.cmbFilters.Size = new System.Drawing.Size(176, 21);
            this.cmbFilters.TabIndex = 11;
            this.cmbFilters.ValueMember = "key";
            this.cmbFilters.SelectedIndexChanged += new System.EventHandler(this.cmbFilters_SelectedIndexChanged);
            // 
            // chkAllColumns
            // 
            this.chkAllColumns.AutoSize = true;
            this.chkAllColumns.Checked = true;
            this.chkAllColumns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllColumns.Location = new System.Drawing.Point(254, 77);
            this.chkAllColumns.Name = "chkAllColumns";
            this.chkAllColumns.Size = new System.Drawing.Size(110, 17);
            this.chkAllColumns.TabIndex = 12;
            this.chkAllColumns.Text = "Show All Columns";
            this.chkAllColumns.UseVisualStyleBackColor = true;
            this.chkAllColumns.CheckedChanged += new System.EventHandler(this.chkAllColumns_CheckedChanged);
            // 
            // lblRowCount
            // 
            this.lblRowCount.AutoSize = true;
            this.lblRowCount.Location = new System.Drawing.Point(855, 95);
            this.lblRowCount.Name = "lblRowCount";
            this.lblRowCount.Size = new System.Drawing.Size(67, 13);
            this.lblRowCount.TabIndex = 13;
            this.lblRowCount.Text = "lblRowCount";
            // 
            // frmCustomers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblRowCount);
            this.Controls.Add(this.chkAllColumns);
            this.Controls.Add(this.cmbFilters);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnAddCustomer);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.rdbAll);
            this.Controls.Add(this.rdbClosed);
            this.Controls.Add(this.rdbActive);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmCustomers";
            this.Size = new System.Drawing.Size(1370, 641);
            this.Load += new System.EventHandler(this.frmCustomers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RadioButton rdbActive;
        private System.Windows.Forms.RadioButton rdbClosed;
        private System.Windows.Forms.RadioButton rdbAll;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnAddCustomer;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbFilters;
        private System.Windows.Forms.CheckBox chkAllColumns;
        private System.Windows.Forms.Label lblRowCount;
    }
}