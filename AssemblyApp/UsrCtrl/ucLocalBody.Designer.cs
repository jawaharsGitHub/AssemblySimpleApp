namespace CenturyFinCorpApp.UsrCtrl
{
    partial class ucLocalBody
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.TextBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.cmbSubItems = new System.Windows.Forms.ComboBox();
            this.cmbZonal = new System.Windows.Forms.ComboBox();
            this.cmbOndrium = new System.Windows.Forms.ComboBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblDone = new System.Windows.Forms.Label();
            this.cmbPanchayat = new System.Windows.Forms.ComboBox();
            this.btnPSProcess = new System.Windows.Forms.Button();
            this.cmbAssembly = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Maroon;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(184, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(316, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "உள்ளாட்சி தேர்தல்";
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(24, 59);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(276, 236);
            this.txtData.TabIndex = 1;
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(192, 318);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(108, 50);
            this.btnProcess.TabIndex = 5;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // cmbSubItems
            // 
            this.cmbSubItems.FormattingEnabled = true;
            this.cmbSubItems.Location = new System.Drawing.Point(51, 371);
            this.cmbSubItems.Name = "cmbSubItems";
            this.cmbSubItems.Size = new System.Drawing.Size(121, 21);
            this.cmbSubItems.TabIndex = 4;
            // 
            // cmbZonal
            // 
            this.cmbZonal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZonal.FormattingEnabled = true;
            this.cmbZonal.Location = new System.Drawing.Point(356, 69);
            this.cmbZonal.Name = "cmbZonal";
            this.cmbZonal.Size = new System.Drawing.Size(121, 21);
            this.cmbZonal.TabIndex = 2;
            this.cmbZonal.SelectedIndexChanged += new System.EventHandler(this.cmbZonal_SelectedIndexChanged);
            // 
            // cmbOndrium
            // 
            this.cmbOndrium.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOndrium.FormattingEnabled = true;
            this.cmbOndrium.Location = new System.Drawing.Point(356, 102);
            this.cmbOndrium.Name = "cmbOndrium";
            this.cmbOndrium.Size = new System.Drawing.Size(121, 21);
            this.cmbOndrium.TabIndex = 3;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.BackColor = System.Drawing.Color.Maroon;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.Color.Yellow;
            this.lblCount.Location = new System.Drawing.Point(33, 395);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(37, 39);
            this.lblCount.TabIndex = 7;
            this.lblCount.Text = "[]";
            // 
            // lblDone
            // 
            this.lblDone.AutoSize = true;
            this.lblDone.BackColor = System.Drawing.Color.Maroon;
            this.lblDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDone.ForeColor = System.Drawing.Color.Yellow;
            this.lblDone.Location = new System.Drawing.Point(33, 446);
            this.lblDone.Name = "lblDone";
            this.lblDone.Size = new System.Drawing.Size(37, 39);
            this.lblDone.TabIndex = 8;
            this.lblDone.Text = "[]";
            // 
            // cmbPanchayat
            // 
            this.cmbPanchayat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPanchayat.FormattingEnabled = true;
            this.cmbPanchayat.Location = new System.Drawing.Point(356, 146);
            this.cmbPanchayat.Name = "cmbPanchayat";
            this.cmbPanchayat.Size = new System.Drawing.Size(121, 21);
            this.cmbPanchayat.TabIndex = 9;
            // 
            // btnPSProcess
            // 
            this.btnPSProcess.Location = new System.Drawing.Point(477, 286);
            this.btnPSProcess.Name = "btnPSProcess";
            this.btnPSProcess.Size = new System.Drawing.Size(108, 50);
            this.btnPSProcess.TabIndex = 10;
            this.btnPSProcess.Text = "Process Polling Station";
            this.btnPSProcess.UseVisualStyleBackColor = true;
            this.btnPSProcess.Click += new System.EventHandler(this.btnPSProcess_Click);
            // 
            // cmbAssembly
            // 
            this.cmbAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssembly.FormattingEnabled = true;
            this.cmbAssembly.Location = new System.Drawing.Point(464, 243);
            this.cmbAssembly.Name = "cmbAssembly";
            this.cmbAssembly.Size = new System.Drawing.Size(121, 21);
            this.cmbAssembly.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(477, 371);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 50);
            this.button1.TabIndex = 12;
            this.button1.Text = "Select File...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(437, 36);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(213, 189);
            this.webBrowser1.TabIndex = 13;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(477, 446);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 14;
            // 
            // ucLocalBody
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmbAssembly);
            this.Controls.Add(this.btnPSProcess);
            this.Controls.Add(this.cmbPanchayat);
            this.Controls.Add(this.lblDone);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.cmbOndrium);
            this.Controls.Add(this.cmbZonal);
            this.Controls.Add(this.cmbSubItems);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.label1);
            this.Name = "ucLocalBody";
            this.Size = new System.Drawing.Size(703, 495);
            this.Load += new System.EventHandler(this.ucLocalBody_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.ComboBox cmbSubItems;
        private System.Windows.Forms.ComboBox cmbZonal;
        private System.Windows.Forms.ComboBox cmbOndrium;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblDone;
        private System.Windows.Forms.ComboBox cmbPanchayat;
        private System.Windows.Forms.Button btnPSProcess;
        private System.Windows.Forms.ComboBox cmbAssembly;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TextBox textBox1;
    }
}
