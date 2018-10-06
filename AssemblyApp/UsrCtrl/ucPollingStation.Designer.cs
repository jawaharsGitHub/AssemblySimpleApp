namespace CenturyFinCorpApp.UsrCtrl
{
    partial class ucPollingStation
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cmbAssembly = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(44, 85);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1004, 619);
            this.dataGridView1.TabIndex = 0;
            // 
            // cmbAssembly
            // 
            this.cmbAssembly.DisplayMember = "AssemblyFullName";
            this.cmbAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssembly.FormattingEnabled = true;
            this.cmbAssembly.Location = new System.Drawing.Point(359, 37);
            this.cmbAssembly.Name = "cmbAssembly";
            this.cmbAssembly.Size = new System.Drawing.Size(202, 21);
            this.cmbAssembly.TabIndex = 1;
            this.cmbAssembly.ValueMember = "AssemblyNo";
            this.cmbAssembly.SelectedIndexChanged += new System.EventHandler(this.cmbAssembly_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(67, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // ucPollingStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAssembly);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ucPollingStation";
            this.Size = new System.Drawing.Size(1152, 823);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cmbAssembly;
        private System.Windows.Forms.Label label1;
    }
}
