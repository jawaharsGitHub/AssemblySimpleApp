namespace CenturyFinCorpApp
{
    partial class frmDataCorrection
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
            this.btnAddCusIntoTxn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAddCusIntoTxn
            // 
            this.btnAddCusIntoTxn.Location = new System.Drawing.Point(105, 34);
            this.btnAddCusIntoTxn.Name = "btnAddCusIntoTxn";
            this.btnAddCusIntoTxn.Size = new System.Drawing.Size(212, 23);
            this.btnAddCusIntoTxn.TabIndex = 0;
            this.btnAddCusIntoTxn.Text = "Add Customer into Transaction";
            this.btnAddCusIntoTxn.UseVisualStyleBackColor = true;
            this.btnAddCusIntoTxn.Click += new System.EventHandler(this.btnAddCusIntoTxn_Click);
            // 
            // DataCorrection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnAddCusIntoTxn);
            this.Name = "DataCorrection";
            this.Text = "DataCorrection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddCusIntoTxn;
    }
}