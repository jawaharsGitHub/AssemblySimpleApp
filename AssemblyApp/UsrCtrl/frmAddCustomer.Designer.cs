namespace CenturyFinCorpApp
{
    partial class frmAddCustomer
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblCustomerNo = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtCustomerNo = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.chkExistingCustomer = new System.Windows.Forms.CheckBox();
            this.cmbExistingCustomer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoan = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtInterest = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNoteCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbReturnType = new System.Windows.Forms.ComboBox();
            this.cmbReturnDay = new System.Windows.Forms.ComboBox();
            this.cmbCollectionSpot = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(32, 96);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(78, 29);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(233, 93);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(357, 35);
            this.txtName.TabIndex = 2;
            // 
            // lblCustomerNo
            // 
            this.lblCustomerNo.AutoSize = true;
            this.lblCustomerNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerNo.Location = new System.Drawing.Point(783, 130);
            this.lblCustomerNo.Name = "lblCustomerNo";
            this.lblCustomerNo.Size = new System.Drawing.Size(161, 29);
            this.lblCustomerNo.TabIndex = 2;
            this.lblCustomerNo.Text = "Customer No.";
            // 
            // txtPhone
            // 
            this.txtPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhone.Location = new System.Drawing.Point(233, 147);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(357, 35);
            this.txtPhone.TabIndex = 3;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhone.Location = new System.Drawing.Point(27, 153);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(83, 29);
            this.lblPhone.TabIndex = 4;
            this.lblPhone.Text = "Phone";
            // 
            // txtCustomerNo
            // 
            this.txtCustomerNo.AutoSize = true;
            this.txtCustomerNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerNo.Location = new System.Drawing.Point(963, 129);
            this.txtCustomerNo.Name = "txtCustomerNo";
            this.txtCustomerNo.Size = new System.Drawing.Size(169, 29);
            this.txtCustomerNo.TabIndex = 6;
            this.txtCustomerNo.Text = "[Customer No]";
            this.txtCustomerNo.UseWaitCursor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(466, 213);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(167, 81);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add Customer";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // chkExistingCustomer
            // 
            this.chkExistingCustomer.AutoSize = true;
            this.chkExistingCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkExistingCustomer.Location = new System.Drawing.Point(12, 24);
            this.chkExistingCustomer.Name = "chkExistingCustomer";
            this.chkExistingCustomer.Size = new System.Drawing.Size(238, 33);
            this.chkExistingCustomer.TabIndex = 9;
            this.chkExistingCustomer.Text = "Existing Customer?";
            this.chkExistingCustomer.UseVisualStyleBackColor = true;
            this.chkExistingCustomer.CheckedChanged += new System.EventHandler(this.chkExistingCustomer_CheckedChanged);
            // 
            // cmbExistingCustomer
            // 
            this.cmbExistingCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbExistingCustomer.FormattingEnabled = true;
            this.cmbExistingCustomer.Location = new System.Drawing.Point(247, 22);
            this.cmbExistingCustomer.Name = "cmbExistingCustomer";
            this.cmbExistingCustomer.Size = new System.Drawing.Size(361, 24);
            this.cmbExistingCustomer.TabIndex = 1;
            this.cmbExistingCustomer.TextChanged += new System.EventHandler(this.cmbExistingCustomer_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 29);
            this.label1.TabIndex = 11;
            this.label1.Text = "Loan Amount";
            // 
            // txtLoan
            // 
            this.txtLoan.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoan.Location = new System.Drawing.Point(233, 202);
            this.txtLoan.Name = "txtLoan";
            this.txtLoan.Size = new System.Drawing.Size(135, 35);
            this.txtLoan.TabIndex = 4;
            this.txtLoan.Leave += new System.EventHandler(this.txtLoan_Leave);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(782, 252);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(86, 31);
            this.lblMessage.TabIndex = 15;
            this.lblMessage.Text = "label3";
            // 
            // txtInterest
            // 
            this.txtInterest.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInterest.Location = new System.Drawing.Point(233, 246);
            this.txtInterest.Name = "txtInterest";
            this.txtInterest.Size = new System.Drawing.Size(135, 35);
            this.txtInterest.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 29);
            this.label3.TabIndex = 16;
            this.label3.Text = "Interest Amount";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(247, 316);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 310);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 29);
            this.label2.TabIndex = 18;
            this.label2.Text = "Amount Given Date";
            // 
            // lblNoteCount
            // 
            this.lblNoteCount.AutoSize = true;
            this.lblNoteCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoteCount.Location = new System.Drawing.Point(971, 171);
            this.lblNoteCount.Name = "lblNoteCount";
            this.lblNoteCount.Size = new System.Drawing.Size(117, 29);
            this.lblNoteCount.TabIndex = 20;
            this.lblNoteCount.Text = "[Note No]";
            this.lblNoteCount.UseWaitCursor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(791, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 29);
            this.label5.TabIndex = 19;
            this.label5.Text = "Note Count No";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(27, 360);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 29);
            this.label4.TabIndex = 21;
            this.label4.Text = "Return Type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(27, 409);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 29);
            this.label6.TabIndex = 22;
            this.label6.Text = "Return Day";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(27, 461);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(177, 29);
            this.label7.TabIndex = 23;
            this.label7.Text = "Collection Spot";
            // 
            // cmbReturnType
            // 
            this.cmbReturnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReturnType.FormattingEnabled = true;
            this.cmbReturnType.Location = new System.Drawing.Point(233, 367);
            this.cmbReturnType.Name = "cmbReturnType";
            this.cmbReturnType.Size = new System.Drawing.Size(121, 21);
            this.cmbReturnType.TabIndex = 24;
            // 
            // cmbReturnDay
            // 
            this.cmbReturnDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReturnDay.FormattingEnabled = true;
            this.cmbReturnDay.Location = new System.Drawing.Point(233, 417);
            this.cmbReturnDay.Name = "cmbReturnDay";
            this.cmbReturnDay.Size = new System.Drawing.Size(121, 21);
            this.cmbReturnDay.TabIndex = 25;
            // 
            // cmbCollectionSpot
            // 
            this.cmbCollectionSpot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCollectionSpot.FormattingEnabled = true;
            this.cmbCollectionSpot.Location = new System.Drawing.Point(233, 470);
            this.cmbCollectionSpot.Name = "cmbCollectionSpot";
            this.cmbCollectionSpot.Size = new System.Drawing.Size(225, 21);
            this.cmbCollectionSpot.TabIndex = 26;
            // 
            // frmAddCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbCollectionSpot);
            this.Controls.Add(this.cmbReturnDay);
            this.Controls.Add(this.cmbReturnType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblNoteCount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.txtInterest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txtLoan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbExistingCustomer);
            this.Controls.Add(this.chkExistingCustomer);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtCustomerNo);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.lblCustomerNo);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Name = "frmAddCustomer";
            this.Size = new System.Drawing.Size(1224, 597);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblCustomerNo;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label txtCustomerNo;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.CheckBox chkExistingCustomer;
        private System.Windows.Forms.ComboBox cmbExistingCustomer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLoan;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtInterest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblNoteCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbReturnType;
        private System.Windows.Forms.ComboBox cmbReturnDay;
        private System.Windows.Forms.ComboBox cmbCollectionSpot;
    }
}