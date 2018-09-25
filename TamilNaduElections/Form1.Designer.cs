namespace TamilNaduElections
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
            this.btnThiruvallur = new System.Windows.Forms.Button();
            this.btnChennai = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnThiruvallur
            // 
            this.btnThiruvallur.Location = new System.Drawing.Point(477, 33);
            this.btnThiruvallur.Name = "btnThiruvallur";
            this.btnThiruvallur.Size = new System.Drawing.Size(75, 61);
            this.btnThiruvallur.TabIndex = 0;
            this.btnThiruvallur.Text = "Thiruvallur";
            this.btnThiruvallur.UseVisualStyleBackColor = true;
            // 
            // btnChennai
            // 
            this.btnChennai.Location = new System.Drawing.Point(478, 95);
            this.btnChennai.Name = "btnChennai";
            this.btnChennai.Size = new System.Drawing.Size(75, 33);
            this.btnChennai.TabIndex = 1;
            this.btnChennai.Text = "Chennai";
            this.btnChennai.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(477, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 61);
            this.button1.TabIndex = 2;
            this.button1.Text = "Kancheepuram";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 723);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnChennai);
            this.Controls.Add(this.btnThiruvallur);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnThiruvallur;
        private System.Windows.Forms.Button btnChennai;
        private System.Windows.Forms.Button button1;
    }
}

