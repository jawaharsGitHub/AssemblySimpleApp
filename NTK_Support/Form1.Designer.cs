namespace NTK_Support
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtAssNo = new System.Windows.Forms.TextBox();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.lblPartStartNo = new System.Windows.Forms.Label();
            this.txtPartEndNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnProgress = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "AssemblyNo";
            // 
            // txtAssNo
            // 
            this.txtAssNo.Location = new System.Drawing.Point(144, 19);
            this.txtAssNo.Name = "txtAssNo";
            this.txtAssNo.Size = new System.Drawing.Size(100, 20);
            this.txtAssNo.TabIndex = 1;
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(144, 59);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(100, 20);
            this.txtStart.TabIndex = 3;
            // 
            // lblPartStartNo
            // 
            this.lblPartStartNo.AutoSize = true;
            this.lblPartStartNo.Location = new System.Drawing.Point(68, 59);
            this.lblPartStartNo.Name = "lblPartStartNo";
            this.lblPartStartNo.Size = new System.Drawing.Size(70, 13);
            this.lblPartStartNo.TabIndex = 2;
            this.lblPartStartNo.Text = "lPart Start No";
            // 
            // txtPartEndNo
            // 
            this.txtPartEndNo.Location = new System.Drawing.Point(356, 66);
            this.txtPartEndNo.Name = "txtPartEndNo";
            this.txtPartEndNo.Size = new System.Drawing.Size(100, 20);
            this.txtPartEndNo.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "lPart End  No";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(135, 122);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(109, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start...";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnProgress
            // 
            this.btnProgress.Location = new System.Drawing.Point(12, 191);
            this.btnProgress.Name = "btnProgress";
            this.btnProgress.Size = new System.Drawing.Size(274, 23);
            this.btnProgress.TabIndex = 7;
            this.btnProgress.Text = "PROGRESS..";
            this.btnProgress.UseVisualStyleBackColor = true;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(486, 177);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(35, 13);
            this.lblError.TabIndex = 8;
            this.lblError.Text = "label3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnProgress);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtPartEndNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtStart);
            this.Controls.Add(this.lblPartStartNo);
            this.Controls.Add(this.txtAssNo);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAssNo;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.Label lblPartStartNo;
        private System.Windows.Forms.TextBox txtPartEndNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnProgress;
        private System.Windows.Forms.Label lblError;
    }
}

