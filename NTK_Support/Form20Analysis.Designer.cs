namespace NTK_Support
{
    partial class Form20Analysis
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
            this.btnStart = new System.Windows.Forms.Button();
            this.txtStartAssemblyNo = new System.Windows.Forms.TextBox();
            this.txtEndAssemblyNo = new System.Windows.Forms.TextBox();
            this.btnProgress = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.btnBasicAna = new System.Windows.Forms.Button();
            this.txtAssemblyNo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(61, 78);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtStartAssemblyNo
            // 
            this.txtStartAssemblyNo.Location = new System.Drawing.Point(25, 32);
            this.txtStartAssemblyNo.Name = "txtStartAssemblyNo";
            this.txtStartAssemblyNo.Size = new System.Drawing.Size(52, 20);
            this.txtStartAssemblyNo.TabIndex = 1;
            this.txtStartAssemblyNo.Text = "1";
            // 
            // txtEndAssemblyNo
            // 
            this.txtEndAssemblyNo.Location = new System.Drawing.Point(145, 32);
            this.txtEndAssemblyNo.Name = "txtEndAssemblyNo";
            this.txtEndAssemblyNo.Size = new System.Drawing.Size(100, 20);
            this.txtEndAssemblyNo.TabIndex = 2;
            this.txtEndAssemblyNo.Text = "234";
            // 
            // btnProgress
            // 
            this.btnProgress.Location = new System.Drawing.Point(170, 78);
            this.btnProgress.Name = "btnProgress";
            this.btnProgress.Size = new System.Drawing.Size(75, 23);
            this.btnProgress.TabIndex = 3;
            this.btnProgress.Text = "button1";
            this.btnProgress.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(194, 123);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(35, 13);
            this.lblError.TabIndex = 5;
            this.lblError.Text = "label2";
            // 
            // btnBasicAna
            // 
            this.btnBasicAna.Location = new System.Drawing.Point(25, 185);
            this.btnBasicAna.Name = "btnBasicAna";
            this.btnBasicAna.Size = new System.Drawing.Size(116, 23);
            this.btnBasicAna.TabIndex = 6;
            this.btnBasicAna.Text = "Basic Analysis";
            this.btnBasicAna.UseVisualStyleBackColor = true;
            this.btnBasicAna.Click += new System.EventHandler(this.btnBasicAna_Click);
            // 
            // txtAssemblyNo
            // 
            this.txtAssemblyNo.Location = new System.Drawing.Point(25, 159);
            this.txtAssemblyNo.Name = "txtAssemblyNo";
            this.txtAssemblyNo.Size = new System.Drawing.Size(52, 20);
            this.txtAssemblyNo.TabIndex = 7;
            // 
            // Form20Analysis
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.txtAssemblyNo);
            this.Controls.Add(this.btnBasicAna);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnProgress);
            this.Controls.Add(this.txtEndAssemblyNo);
            this.Controls.Add(this.txtStartAssemblyNo);
            this.Controls.Add(this.btnStart);
            this.Name = "Form20Analysis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtStartAssemblyNo;
        private System.Windows.Forms.TextBox txtEndAssemblyNo;
        private System.Windows.Forms.Button btnProgress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnBasicAna;
        private System.Windows.Forms.TextBox txtAssemblyNo;
    }
}

