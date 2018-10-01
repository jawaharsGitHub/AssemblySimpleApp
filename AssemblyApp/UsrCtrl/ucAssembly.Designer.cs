namespace CenturyFinCorpApp.UsrCtrl
{
    partial class ucAssembly
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvAssembly = new System.Windows.Forms.DataGridView();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cmbDistrict = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssembly)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAssembly
            // 
            this.dgvAssembly.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssembly.Location = new System.Drawing.Point(42, 109);
            this.dgvAssembly.Name = "dgvAssembly";
            this.dgvAssembly.Size = new System.Drawing.Size(721, 492);
            this.dgvAssembly.TabIndex = 0;
            this.dgvAssembly.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvAssembly_MouseDoubleClick);
            // 
            // cmbFilter
            // 
            this.cmbFilter.DisplayMember = "value";
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Location = new System.Drawing.Point(589, 72);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(174, 21);
            this.cmbFilter.TabIndex = 1;
            this.cmbFilter.ValueMember = "key";
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(348, 72);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(195, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // cmbDistrict
            // 
            this.cmbDistrict.DisplayMember = "Name";
            this.cmbDistrict.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDistrict.FormattingEnabled = true;
            this.cmbDistrict.Location = new System.Drawing.Point(111, 71);
            this.cmbDistrict.Name = "cmbDistrict";
            this.cmbDistrict.Size = new System.Drawing.Size(174, 21);
            this.cmbDistrict.TabIndex = 3;
            this.cmbDistrict.ValueMember = "DistrictId";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Maroon;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(231, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(325, 39);
            this.label1.TabIndex = 5;
            this.label1.Text = "சட்டமன்ற தொகுதி";
            // 
            // ucAssembly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDistrict);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cmbFilter);
            this.Controls.Add(this.dgvAssembly);
            this.Name = "ucAssembly";
            this.Size = new System.Drawing.Size(841, 582);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssembly)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAssembly;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox cmbDistrict;
        private System.Windows.Forms.Label label1;
    }
}
