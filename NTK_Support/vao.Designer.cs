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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(35, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(372, 56);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(519, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(413, 8);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(45, 105);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(743, 333);
            this.dataGridView1.TabIndex = 3;
            // 
            // ddlPattaTypes
            // 
            this.ddlPattaTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPattaTypes.FormattingEnabled = true;
            this.ddlPattaTypes.Location = new System.Drawing.Point(322, 74);
            this.ddlPattaTypes.Name = "ddlPattaTypes";
            this.ddlPattaTypes.Size = new System.Drawing.Size(121, 21);
            this.ddlPattaTypes.TabIndex = 4;
            this.ddlPattaTypes.SelectedIndexChanged += new System.EventHandler(this.ddlPattaTypes_SelectedIndexChanged);
            // 
            // ddlListType
            // 
            this.ddlListType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlListType.FormattingEnabled = true;
            this.ddlListType.Location = new System.Drawing.Point(473, 74);
            this.ddlListType.Name = "ddlListType";
            this.ddlListType.Size = new System.Drawing.Size(121, 21);
            this.ddlListType.TabIndex = 5;
            this.ddlListType.SelectedIndexChanged += new System.EventHandler(this.ddlListType_SelectedIndexChanged);
            // 
            // ddlLandTypes
            // 
            this.ddlLandTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLandTypes.FormattingEnabled = true;
            this.ddlLandTypes.Location = new System.Drawing.Point(620, 74);
            this.ddlLandTypes.Name = "ddlLandTypes";
            this.ddlLandTypes.Size = new System.Drawing.Size(121, 21);
            this.ddlLandTypes.TabIndex = 6;
            this.ddlLandTypes.SelectedIndexChanged += new System.EventHandler(this.ddlLandTypes_SelectedIndexChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(652, 22);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Generate Html";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // vao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.ddlLandTypes);
            this.Controls.Add(this.ddlListType);
            this.Controls.Add(this.ddlPattaTypes);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "vao";
            this.Text = "vao";
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
    }
}