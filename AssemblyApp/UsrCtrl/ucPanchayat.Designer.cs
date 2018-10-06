namespace CenturyFinCorpApp.UsrCtrl
{
    partial class ucPanchayat
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(25, 105);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1206, 523);
            this.dataGridView1.TabIndex = 0;
            // 
            // cmbAssembly
            // 
            this.cmbAssembly.DisplayMember = "AssemblyFullName";
            this.cmbAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssembly.FormattingEnabled = true;
            this.cmbAssembly.Location = new System.Drawing.Point(60, 78);
            this.cmbAssembly.Name = "cmbAssembly";
            this.cmbAssembly.Size = new System.Drawing.Size(195, 21);
            this.cmbAssembly.TabIndex = 1;
            this.cmbAssembly.ValueMember = "AssemblyNo";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(329, 78);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(220, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // cmbSort
            // 
            this.cmbSort.DisplayMember = "value";
            this.cmbSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.Location = new System.Drawing.Point(644, 78);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(209, 21);
            this.cmbSort.TabIndex = 3;
            this.cmbSort.ValueMember = "key";
            this.cmbSort.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Maroon;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(235, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(374, 39);
            this.label1.TabIndex = 6;
            this.label1.Text = "பஞ்சாயத்து கிளைகள்";
            // 
            // ucPanchayat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSort);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cmbAssembly);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ucPanchayat";
            this.Size = new System.Drawing.Size(1271, 620);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cmbAssembly;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox cmbSort;
        private System.Windows.Forms.Label label1;
    }
}
