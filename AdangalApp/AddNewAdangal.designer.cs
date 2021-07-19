using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdangalApp
{
    partial class AddNewAdangal
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

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ddlLandTypes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOwnerName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTheervai = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtParappu = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUtpirivu = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNilaalavaiEN = new System.Windows.Forms.TextBox();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.NilaAlavaiEn = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMessage);
            this.groupBox1.Controls.Add(this.ddlLandTypes);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtOwnerName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtTheervai);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtParappu);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtUtpirivu);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtNilaalavaiEN);
            this.groupBox1.Controls.Add(this.btnAddNew);
            this.groupBox1.Controls.Add(this.NilaAlavaiEn);
            this.groupBox1.Location = new System.Drawing.Point(65, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 299);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add New Record";
            // 
            // ddlLandTypes
            // 
            this.ddlLandTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLandTypes.FormattingEnabled = true;
            this.ddlLandTypes.Location = new System.Drawing.Point(106, 208);
            this.ddlLandTypes.Name = "ddlLandTypes";
            this.ddlLandTypes.Size = new System.Drawing.Size(162, 21);
            this.ddlLandTypes.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 216);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Land Type";
            // 
            // txtOwnerName
            // 
            this.txtOwnerName.Location = new System.Drawing.Point(107, 178);
            this.txtOwnerName.Name = "txtOwnerName";
            this.txtOwnerName.Size = new System.Drawing.Size(100, 20);
            this.txtOwnerName.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Name";
            // 
            // txtTheervai
            // 
            this.txtTheervai.Location = new System.Drawing.Point(107, 146);
            this.txtTheervai.Name = "txtTheervai";
            this.txtTheervai.Size = new System.Drawing.Size(100, 20);
            this.txtTheervai.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Theervai";
            // 
            // txtParappu
            // 
            this.txtParappu.Location = new System.Drawing.Point(107, 116);
            this.txtParappu.Name = "txtParappu";
            this.txtParappu.Size = new System.Drawing.Size(100, 20);
            this.txtParappu.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Parappu";
            // 
            // txtUtpirivu
            // 
            this.txtUtpirivu.Location = new System.Drawing.Point(107, 81);
            this.txtUtpirivu.Name = "txtUtpirivu";
            this.txtUtpirivu.Size = new System.Drawing.Size(100, 20);
            this.txtUtpirivu.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Utpirivu";
            // 
            // txtNilaalavaiEN
            // 
            this.txtNilaalavaiEN.Location = new System.Drawing.Point(107, 46);
            this.txtNilaalavaiEN.Name = "txtNilaalavaiEN";
            this.txtNilaalavaiEN.Size = new System.Drawing.Size(100, 20);
            this.txtNilaalavaiEN.TabIndex = 2;
            // 
            // btnAddNew
            // 
            this.btnAddNew.Location = new System.Drawing.Point(89, 244);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(75, 23);
            this.btnAddNew.TabIndex = 1;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // NilaAlavaiEn
            // 
            this.NilaAlavaiEn.AutoSize = true;
            this.NilaAlavaiEn.Location = new System.Drawing.Point(34, 49);
            this.NilaAlavaiEn.Name = "NilaAlavaiEn";
            this.NilaAlavaiEn.Size = new System.Drawing.Size(67, 13);
            this.NilaAlavaiEn.TabIndex = 0;
            this.NilaAlavaiEn.Text = "NilaAlavaiEn";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(33, 20);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(60, 13);
            this.lblMessage.TabIndex = 15;
            this.lblMessage.Text = "lblMessage";
            // 
            // AddNewAdangal
            // 
            this.ClientSize = new System.Drawing.Size(472, 379);
            this.Controls.Add(this.groupBox1);
            this.Name = "AddNewAdangal";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtOwnerName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTheervai;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtParappu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUtpirivu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNilaalavaiEN;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Label NilaAlavaiEn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlLandTypes;
        private System.Windows.Forms.Label lblMessage;
    }
}
