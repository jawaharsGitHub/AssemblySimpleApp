namespace CenturyFinCorpApp.UsrCtrl
{
    partial class ucAssembly
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
            this.dgvAssembly = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssembly)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAssembly
            // 
            this.dgvAssembly.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssembly.Location = new System.Drawing.Point(42, 79);
            this.dgvAssembly.Name = "dgvAssembly";
            this.dgvAssembly.Size = new System.Drawing.Size(721, 492);
            this.dgvAssembly.TabIndex = 0;
            // 
            // ucAssembly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvAssembly);
            this.Name = "ucAssembly";
            this.Size = new System.Drawing.Size(841, 582);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssembly)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAssembly;
    }
}
