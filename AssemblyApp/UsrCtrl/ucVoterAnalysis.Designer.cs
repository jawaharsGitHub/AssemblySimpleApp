namespace CenturyFinCorpApp.UsrCtrl
{
    partial class ucVoterAnalysis
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
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.chkDebugMode = new System.Windows.Forms.CheckBox();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.txtRow = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chkPageList = new System.Windows.Forms.ComboBox();
            this.txtMissingRow = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtFIlterPn = new System.Windows.Forms.TextBox();
            this.txtFIlterRn = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbFIlter = new System.Windows.Forms.ComboBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.cmbAss = new System.Windows.Forms.ComboBox();
            this.cmbBooths = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(33, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(-15, -15);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // chkDebugMode
            // 
            this.chkDebugMode.AutoSize = true;
            this.chkDebugMode.ForeColor = System.Drawing.Color.Yellow;
            this.chkDebugMode.Location = new System.Drawing.Point(216, 29);
            this.chkDebugMode.Name = "chkDebugMode";
            this.chkDebugMode.Size = new System.Drawing.Size(64, 17);
            this.chkDebugMode.TabIndex = 2;
            this.chkDebugMode.Text = "Debug?";
            this.chkDebugMode.UseVisualStyleBackColor = true;
            this.chkDebugMode.CheckedChanged += new System.EventHandler(this.chkDebugMode_CheckedChanged);
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(338, 28);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(100, 20);
            this.txtPage.TabIndex = 3;
            // 
            // txtRow
            // 
            this.txtRow.Location = new System.Drawing.Point(531, 28);
            this.txtRow.Name = "txtRow";
            this.txtRow.Size = new System.Drawing.Size(100, 20);
            this.txtRow.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(286, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "PageNo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(482, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "RowNo";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(33, 112);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1173, 504);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // chkPageList
            // 
            this.chkPageList.FormattingEnabled = true;
            this.chkPageList.Location = new System.Drawing.Point(678, 15);
            this.chkPageList.Name = "chkPageList";
            this.chkPageList.Size = new System.Drawing.Size(133, 21);
            this.chkPageList.TabIndex = 8;
            // 
            // txtMissingRow
            // 
            this.txtMissingRow.Location = new System.Drawing.Point(826, 15);
            this.txtMissingRow.Name = "txtMissingRow";
            this.txtMissingRow.Size = new System.Drawing.Size(59, 20);
            this.txtMissingRow.TabIndex = 9;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(738, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Re-Process";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtFIlterPn
            // 
            this.txtFIlterPn.Location = new System.Drawing.Point(295, 76);
            this.txtFIlterPn.Name = "txtFIlterPn";
            this.txtFIlterPn.Size = new System.Drawing.Size(42, 20);
            this.txtFIlterPn.TabIndex = 11;
            // 
            // txtFIlterRn
            // 
            this.txtFIlterRn.Location = new System.Drawing.Point(365, 73);
            this.txtFIlterRn.Name = "txtFIlterRn";
            this.txtFIlterRn.Size = new System.Drawing.Size(43, 20);
            this.txtFIlterRn.TabIndex = 12;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(423, 71);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(125, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "Filter";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Yellow;
            this.label3.Location = new System.Drawing.Point(270, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "pn";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Yellow;
            this.label4.Location = new System.Drawing.Point(343, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "rn";
            // 
            // cmbFIlter
            // 
            this.cmbFIlter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFIlter.FormattingEnabled = true;
            this.cmbFIlter.Location = new System.Drawing.Point(563, 76);
            this.cmbFIlter.Name = "cmbFIlter";
            this.cmbFIlter.Size = new System.Drawing.Size(133, 21);
            this.cmbFIlter.TabIndex = 16;
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.ForeColor = System.Drawing.Color.Yellow;
            this.lblDetails.Location = new System.Drawing.Point(1089, 11);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(83, 25);
            this.lblDetails.TabIndex = 17;
            this.lblDetails.Text = "[Details]";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(713, 74);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(98, 23);
            this.button4.TabIndex = 18;
            this.button4.Text = "Refresh..";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(843, 76);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(125, 23);
            this.button5.TabIndex = 19;
            this.button5.Text = "Download FIle";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // cmbAss
            // 
            this.cmbAss.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAss.FormattingEnabled = true;
            this.cmbAss.Location = new System.Drawing.Point(33, 79);
            this.cmbAss.Name = "cmbAss";
            this.cmbAss.Size = new System.Drawing.Size(90, 21);
            this.cmbAss.TabIndex = 20;
            this.cmbAss.SelectedIndexChanged += new System.EventHandler(this.cmbAss_SelectedIndexChanged);
            // 
            // cmbBooths
            // 
            this.cmbBooths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBooths.FormattingEnabled = true;
            this.cmbBooths.Location = new System.Drawing.Point(149, 79);
            this.cmbBooths.Name = "cmbBooths";
            this.cmbBooths.Size = new System.Drawing.Size(79, 21);
            this.cmbBooths.TabIndex = 21;
            this.cmbBooths.SelectedIndexChanged += new System.EventHandler(this.cmbBooths_SelectedIndexChanged);
            // 
            // ucVoterAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.Controls.Add(this.cmbBooths);
            this.Controls.Add(this.cmbAss);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.cmbFIlter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtFIlterRn);
            this.Controls.Add(this.txtFIlterPn);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtMissingRow);
            this.Controls.Add(this.chkPageList);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRow);
            this.Controls.Add(this.txtPage);
            this.Controls.Add(this.chkDebugMode);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Name = "ucVoterAnalysis";
            this.Size = new System.Drawing.Size(1246, 649);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox chkDebugMode;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.TextBox txtRow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox chkPageList;
        private System.Windows.Forms.TextBox txtMissingRow;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtFIlterPn;
        private System.Windows.Forms.TextBox txtFIlterRn;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox cmbFIlter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox cmbAss;
        private System.Windows.Forms.ComboBox cmbBooths;
    }
}
