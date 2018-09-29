namespace CenturyFinCorpApp.UsrCtrl
{
    partial class ucZonal
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
            this.dgvZonal = new System.Windows.Forms.DataGridView();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvZonal)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvZonal
            // 
            this.dgvZonal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvZonal.Location = new System.Drawing.Point(40, 42);
            this.dgvZonal.Name = "dgvZonal";
            this.dgvZonal.Size = new System.Drawing.Size(1109, 764);
            this.dgvZonal.TabIndex = 0;
            // 
            // cmbFilter
            // 
            this.cmbFilter.DisplayMember = "value";
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Location = new System.Drawing.Point(641, 15);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(173, 21);
            this.cmbFilter.TabIndex = 1;
            this.cmbFilter.ValueMember = "key";
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // ucZonal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbFilter);
            this.Controls.Add(this.dgvZonal);
            this.Name = "ucZonal";
            this.Size = new System.Drawing.Size(1186, 887);
            ((System.ComponentModel.ISupportInitialize)(this.dgvZonal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvZonal;
        private System.Windows.Forms.ComboBox cmbFilter;
    }
}
