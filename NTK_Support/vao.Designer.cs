namespace NTK_Support
{
    partial class vao
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ddlPattaTypes = new System.Windows.Forms.ComboBox();
            this.ddlListType = new System.Windows.Forms.ComboBox();
            this.ddlLandTypes = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnSaveJson = new System.Windows.Forms.Button();
            this.ddlDistrict = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStatusCheck = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(508, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(167, 56);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(688, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Test Sum";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(684, 14);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 149);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 301);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
            // 
            // ddlPattaTypes
            // 
            this.ddlPattaTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPattaTypes.FormattingEnabled = true;
            this.ddlPattaTypes.Location = new System.Drawing.Point(12, 89);
            this.ddlPattaTypes.Name = "ddlPattaTypes";
            this.ddlPattaTypes.Size = new System.Drawing.Size(186, 21);
            this.ddlPattaTypes.TabIndex = 4;
            this.ddlPattaTypes.SelectedIndexChanged += new System.EventHandler(this.ddlPattaTypes_SelectedIndexChanged);
            // 
            // ddlListType
            // 
            this.ddlListType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlListType.FormattingEnabled = true;
            this.ddlListType.Location = new System.Drawing.Point(379, 89);
            this.ddlListType.Name = "ddlListType";
            this.ddlListType.Size = new System.Drawing.Size(121, 21);
            this.ddlListType.TabIndex = 5;
            this.ddlListType.SelectedIndexChanged += new System.EventHandler(this.ddlListType_SelectedIndexChanged);
            // 
            // ddlLandTypes
            // 
            this.ddlLandTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLandTypes.FormattingEnabled = true;
            this.ddlLandTypes.Location = new System.Drawing.Point(225, 89);
            this.ddlLandTypes.Name = "ddlLandTypes";
            this.ddlLandTypes.Size = new System.Drawing.Size(121, 21);
            this.ddlLandTypes.TabIndex = 6;
            this.ddlLandTypes.SelectedIndexChanged += new System.EventHandler(this.ddlLandTypes_SelectedIndexChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Location = new System.Drawing.Point(634, 111);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(101, 23);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Generate Html";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(306, 11);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(66, 13);
            this.lblMessage.TabIndex = 8;
            this.lblMessage.Text = "[lblMessage]";
            // 
            // btnSaveJson
            // 
            this.btnSaveJson.Enabled = false;
            this.btnSaveJson.Location = new System.Drawing.Point(531, 111);
            this.btnSaveJson.Name = "btnSaveJson";
            this.btnSaveJson.Size = new System.Drawing.Size(87, 23);
            this.btnSaveJson.TabIndex = 9;
            this.btnSaveJson.Text = "Save as JSON";
            this.btnSaveJson.UseVisualStyleBackColor = true;
            this.btnSaveJson.Click += new System.EventHandler(this.button2_Click);
            // 
            // ddlDistrict
            // 
            this.ddlDistrict.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDistrict.FormattingEnabled = true;
            this.ddlDistrict.Location = new System.Drawing.Point(26, 29);
            this.ddlDistrict.Name = "ddlDistrict";
            this.ddlDistrict.Size = new System.Drawing.Size(186, 21);
            this.ddlDistrict.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "மாவட்டம்";
            // 
            // btnStatusCheck
            // 
            this.btnStatusCheck.Enabled = false;
            this.btnStatusCheck.Location = new System.Drawing.Point(278, 29);
            this.btnStatusCheck.Name = "btnStatusCheck";
            this.btnStatusCheck.Size = new System.Drawing.Size(128, 40);
            this.btnStatusCheck.TabIndex = 12;
            this.btnStatusCheck.Text = "status";
            this.btnStatusCheck.UseVisualStyleBackColor = true;
            // 
            // vao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnStatusCheck);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ddlDistrict);
            this.Controls.Add(this.btnSaveJson);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.ddlLandTypes);
            this.Controls.Add(this.ddlListType);
            this.Controls.Add(this.ddlPattaTypes);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "vao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "vao";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox ddlPattaTypes;
        private System.Windows.Forms.ComboBox ddlListType;
        private System.Windows.Forms.ComboBox ddlLandTypes;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnSaveJson;
        private System.Windows.Forms.ComboBox ddlDistrict;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStatusCheck;
    }
}