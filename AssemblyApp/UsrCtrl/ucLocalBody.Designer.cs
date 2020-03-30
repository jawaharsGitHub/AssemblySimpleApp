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
            this.grpOndrium = new System.Windows.Forms.GroupBox();
            this.txtOndName = new System.Windows.Forms.TextBox();
            this.txtOndId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbZonal = new System.Windows.Forms.ComboBox();
            this.cmbOndrium = new System.Windows.Forms.ComboBox();
            this.grpOndrium.SuspendLayout();
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
            this.label1.TabIndex = 6;
            this.label1.Text = "உள்ளாட்சி தேர்தல்";
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(24, 59);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(276, 236);
            this.txtData.TabIndex = 7;
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(425, 380);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(108, 50);
            this.btnProcess.TabIndex = 8;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // cmbSubItems
            // 
            this.cmbSubItems.FormattingEnabled = true;
            this.cmbSubItems.Location = new System.Drawing.Point(331, 167);
            this.cmbSubItems.Name = "cmbSubItems";
            this.cmbSubItems.Size = new System.Drawing.Size(121, 21);
            this.cmbSubItems.TabIndex = 11;
            this.cmbSubItems.SelectedIndexChanged += new System.EventHandler(this.cmbSubItems_SelectedIndexChanged);
            // 
            // grpOndrium
            // 
            this.grpOndrium.Controls.Add(this.txtOndName);
            this.grpOndrium.Controls.Add(this.txtOndId);
            this.grpOndrium.Controls.Add(this.label4);
            this.grpOndrium.Controls.Add(this.label5);
            this.grpOndrium.Location = new System.Drawing.Point(328, 211);
            this.grpOndrium.Name = "grpOndrium";
            this.grpOndrium.Size = new System.Drawing.Size(233, 136);
            this.grpOndrium.TabIndex = 12;
            this.grpOndrium.TabStop = false;
            this.grpOndrium.Text = "Ondrium Details";
            // 
            // txtOndName
            // 
            this.txtOndName.Location = new System.Drawing.Point(104, 78);
            this.txtOndName.Name = "txtOndName";
            this.txtOndName.Size = new System.Drawing.Size(101, 20);
            this.txtOndName.TabIndex = 18;
            // 
            // txtOndId
            // 
            this.txtOndId.Location = new System.Drawing.Point(104, 39);
            this.txtOndId.Name = "txtOndId";
            this.txtOndId.Size = new System.Drawing.Size(68, 20);
            this.txtOndId.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "OndriumName";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "OndriumId";
            // 
            // cmbZonal
            // 
            this.cmbZonal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZonal.FormattingEnabled = true;
            this.cmbZonal.Location = new System.Drawing.Point(331, 97);
            this.cmbZonal.Name = "cmbZonal";
            this.cmbZonal.Size = new System.Drawing.Size(121, 21);
            this.cmbZonal.TabIndex = 13;
            this.cmbZonal.SelectedIndexChanged += new System.EventHandler(this.cmbZonal_SelectedIndexChanged);
            // 
            // cmbOndrium
            // 
            this.cmbOndrium.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOndrium.FormattingEnabled = true;
            this.cmbOndrium.Location = new System.Drawing.Point(331, 130);
            this.cmbOndrium.Name = "cmbOndrium";
            this.cmbOndrium.Size = new System.Drawing.Size(121, 21);
            this.cmbOndrium.TabIndex = 14;
            // 
            // ucLocalBody
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbOndrium);
            this.Controls.Add(this.cmbZonal);
            this.Controls.Add(this.grpOndrium);
            this.Controls.Add(this.cmbSubItems);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.label1);
            this.Name = "ucLocalBody";
            this.Size = new System.Drawing.Size(703, 495);
            this.grpOndrium.ResumeLayout(false);
            this.grpOndrium.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.ComboBox cmbSubItems;
        private System.Windows.Forms.GroupBox grpOndrium;
        private System.Windows.Forms.TextBox txtOndName;
        private System.Windows.Forms.TextBox txtOndId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbZonal;
        private System.Windows.Forms.ComboBox cmbOndrium;
    }
}
