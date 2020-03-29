namespace CenturyFinCorpApp.UsrCtrl
{
    partial class ucMemberVerify
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
            this.dgvMember = new System.Windows.Forms.DataGridView();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnNameAddressVerify = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Maroon;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(187, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(381, 39);
            this.label1.TabIndex = 6;
            this.label1.Text = "வெளிநாட்டு கிளைகள்";
            // 
            // dgvMember
            // 
            this.dgvMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMember.Location = new System.Drawing.Point(18, 60);
            this.dgvMember.Name = "dgvMember";
            this.dgvMember.Size = new System.Drawing.Size(939, 468);
            this.dgvMember.TabIndex = 7;
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(657, 18);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(115, 23);
            this.btnVerify.TabIndex = 8;
            this.btnVerify.Text = "Phone Verify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnNameAddressVerify
            // 
            this.btnNameAddressVerify.Location = new System.Drawing.Point(800, 18);
            this.btnNameAddressVerify.Name = "btnNameAddressVerify";
            this.btnNameAddressVerify.Size = new System.Drawing.Size(115, 23);
            this.btnNameAddressVerify.TabIndex = 9;
            this.btnNameAddressVerify.Text = "Name And Address";
            this.btnNameAddressVerify.UseVisualStyleBackColor = true;
            this.btnNameAddressVerify.Click += new System.EventHandler(this.btnNameAddressVerify_Click);
            // 
            // ucMemberVerify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnNameAddressVerify);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.dgvMember);
            this.Controls.Add(this.label1);
            this.Name = "ucMemberVerify";
            this.Size = new System.Drawing.Size(1201, 556);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvMember;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnNameAddressVerify;
    }
}
