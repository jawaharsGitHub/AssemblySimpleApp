namespace CenturyFinCorpApp.UsrCtrl
{
    partial class frmBatches
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
            this.btnYearlyBatch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(85, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate Daily Txn";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnYearlyBatch
            // 
            this.btnYearlyBatch.Location = new System.Drawing.Point(85, 137);
            this.btnYearlyBatch.Name = "btnYearlyBatch";
            this.btnYearlyBatch.Size = new System.Drawing.Size(133, 44);
            this.btnYearlyBatch.TabIndex = 1;
            this.btnYearlyBatch.Text = "Run Yearly Batch";
            this.btnYearlyBatch.UseVisualStyleBackColor = true;
            this.btnYearlyBatch.Click += new System.EventHandler(this.btnYearlyBatch_Click);
            // 
            // frmBatches
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnYearlyBatch);
            this.Controls.Add(this.button1);
            this.Name = "frmBatches";
            this.Size = new System.Drawing.Size(684, 540);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnYearlyBatch;
    }
}
