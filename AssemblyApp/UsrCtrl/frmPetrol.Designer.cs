namespace CenturyFinCorpApp.UsrCtrl
{
    partial class frmPetrol
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
            this.dgvPetrol = new System.Windows.Forms.DataGridView();
            this.lblAvgKm = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPetrol)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPetrol
            // 
            this.dgvPetrol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPetrol.Location = new System.Drawing.Point(164, 124);
            this.dgvPetrol.Name = "dgvPetrol";
            this.dgvPetrol.Size = new System.Drawing.Size(555, 338);
            this.dgvPetrol.TabIndex = 20;
            // 
            // lblAvgKm
            // 
            this.lblAvgKm.AutoSize = true;
            this.lblAvgKm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgKm.Location = new System.Drawing.Point(248, 63);
            this.lblAvgKm.Name = "lblAvgKm";
            this.lblAvgKm.Size = new System.Drawing.Size(152, 24);
            this.lblAvgKm.TabIndex = 21;
            this.lblAvgKm.Text = "[Avg km Per day]";
            // 
            // frmPetrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAvgKm);
            this.Controls.Add(this.dgvPetrol);
            this.Name = "frmPetrol";
            this.Size = new System.Drawing.Size(958, 654);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPetrol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPetrol;
        private System.Windows.Forms.Label lblAvgKm;
    }
}
