namespace CenturyFinCorpApp.UsrCtrl
{
    partial class frmGeneralReport
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
            this.dgvIncome = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblActual = new System.Windows.Forms.Label();
            this.lblExpected = new System.Windows.Forms.Label();
            this.chkAddSalary = new System.Windows.Forms.CheckBox();
            this.dgvNotePerMonth = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCustomerCount = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSalary = new System.Windows.Forms.Label();
            this.lblTotalAsset = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblOutStanding = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIncome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotePerMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvIncome
            // 
            this.dgvIncome.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIncome.Location = new System.Drawing.Point(15, 107);
            this.dgvIncome.Name = "dgvIncome";
            this.dgvIncome.Size = new System.Drawing.Size(445, 405);
            this.dgvIncome.TabIndex = 0;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(167, 58);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(42, 13);
            this.lblTotal.TabIndex = 12;
            this.lblTotal.Text = "TOTAL";
            // 
            // lblActual
            // 
            this.lblActual.AutoSize = true;
            this.lblActual.Location = new System.Drawing.Point(167, 34);
            this.lblActual.Name = "lblActual";
            this.lblActual.Size = new System.Drawing.Size(37, 13);
            this.lblActual.TabIndex = 11;
            this.lblActual.Text = "Actual";
            // 
            // lblExpected
            // 
            this.lblExpected.AutoSize = true;
            this.lblExpected.Location = new System.Drawing.Point(167, 7);
            this.lblExpected.Name = "lblExpected";
            this.lblExpected.Size = new System.Drawing.Size(52, 13);
            this.lblExpected.TabIndex = 10;
            this.lblExpected.Text = "Expected";
            // 
            // chkAddSalary
            // 
            this.chkAddSalary.AutoSize = true;
            this.chkAddSalary.Location = new System.Drawing.Point(39, 3);
            this.chkAddSalary.Name = "chkAddSalary";
            this.chkAddSalary.Size = new System.Drawing.Size(105, 17);
            this.chkAddSalary.TabIndex = 9;
            this.chkAddSalary.Text = "Consider Salary?";
            this.chkAddSalary.UseVisualStyleBackColor = true;
            this.chkAddSalary.CheckedChanged += new System.EventHandler(this.chkAddSalary_CheckedChanged);
            // 
            // dgvNotePerMonth
            // 
            this.dgvNotePerMonth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotePerMonth.Location = new System.Drawing.Point(521, 107);
            this.dgvNotePerMonth.Name = "dgvNotePerMonth";
            this.dgvNotePerMonth.Size = new System.Drawing.Size(569, 405);
            this.dgvNotePerMonth.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(528, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "NOTE PER MONTH:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 519);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "label1";
            // 
            // btnCustomerCount
            // 
            this.btnCustomerCount.Location = new System.Drawing.Point(1120, 107);
            this.btnCustomerCount.Name = "btnCustomerCount";
            this.btnCustomerCount.Size = new System.Drawing.Size(145, 23);
            this.btnCustomerCount.TabIndex = 23;
            this.btnCustomerCount.Text = "[Customer Count]";
            this.btnCustomerCount.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(518, 519);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "label2";
            // 
            // lblSalary
            // 
            this.lblSalary.AutoSize = true;
            this.lblSalary.Location = new System.Drawing.Point(167, 81);
            this.lblSalary.Name = "lblSalary";
            this.lblSalary.Size = new System.Drawing.Size(49, 13);
            this.lblSalary.TabIndex = 25;
            this.lblSalary.Text = "SALARY";
            // 
            // lblTotalAsset
            // 
            this.lblTotalAsset.AutoSize = true;
            this.lblTotalAsset.Location = new System.Drawing.Point(647, 34);
            this.lblTotalAsset.Name = "lblTotalAsset";
            this.lblTotalAsset.Size = new System.Drawing.Size(63, 13);
            this.lblTotalAsset.TabIndex = 29;
            this.lblTotalAsset.Text = "[TotalAsset]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(518, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Total Asset";
            // 
            // lblOutStanding
            // 
            this.lblOutStanding.AutoSize = true;
            this.lblOutStanding.Location = new System.Drawing.Point(647, 7);
            this.lblOutStanding.Name = "lblOutStanding";
            this.lblOutStanding.Size = new System.Drawing.Size(111, 13);
            this.lblOutStanding.TabIndex = 27;
            this.lblOutStanding.Text = "[OutSTanding Money]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(518, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "OutStanding Money";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1133, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "label6";
            // 
            // frmGeneralReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblTotalAsset);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblOutStanding);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblSalary);
            this.Controls.Add(this.dgvIncome);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCustomerCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvNotePerMonth);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblActual);
            this.Controls.Add(this.lblExpected);
            this.Controls.Add(this.chkAddSalary);
            this.Name = "frmGeneralReport";
            this.Size = new System.Drawing.Size(1286, 560);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIncome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotePerMonth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvIncome;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblActual;
        private System.Windows.Forms.Label lblExpected;
        private System.Windows.Forms.CheckBox chkAddSalary;
        private System.Windows.Forms.DataGridView dgvNotePerMonth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCustomerCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSalary;
        private System.Windows.Forms.Label lblTotalAsset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblOutStanding;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}
