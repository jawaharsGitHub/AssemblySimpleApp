namespace AdangalApp
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
            this.ddlDistrict = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStatusCheck = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTaluk = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbVillages = new System.Windows.Forms.ComboBox();
            this.btnReady = new System.Windows.Forms.Button();
            this.btnReadFile = new System.Windows.Forms.Button();
            this.cmbFulfilled = new System.Windows.Forms.ComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtVaruvai = new System.Windows.Forms.TextBox();
            this.txtVattam = new System.Windows.Forms.TextBox();
            this.txtFirka = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLoadProcessed = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ddlProcessedFiles = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnPercentage = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAddNewSurvey = new System.Windows.Forms.TextBox();
            this.btnAddNewSurvey = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cmbItemToBeAdded = new System.Windows.Forms.ComboBox();
            this.cmbSurveyNo = new System.Windows.Forms.ComboBox();
            this.cmbSubdivNo = new System.Windows.Forms.ComboBox();
            this.lblSurveyNo = new System.Windows.Forms.Label();
            this.lblSubdiv = new System.Windows.Forms.Label();
            this.chkEdit = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(818, 427);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(181, 129);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1011, 427);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Test Sum";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1005, 463);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(11, 431);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 301);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // ddlPattaTypes
            // 
            this.ddlPattaTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPattaTypes.FormattingEnabled = true;
            this.ddlPattaTypes.Location = new System.Drawing.Point(17, 35);
            this.ddlPattaTypes.Name = "ddlPattaTypes";
            this.ddlPattaTypes.Size = new System.Drawing.Size(186, 21);
            this.ddlPattaTypes.TabIndex = 12;
            this.ddlPattaTypes.SelectedIndexChanged += new System.EventHandler(this.ddlPattaTypes_SelectedIndexChanged);
            // 
            // ddlListType
            // 
            this.ddlListType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlListType.FormattingEnabled = true;
            this.ddlListType.Location = new System.Drawing.Point(402, 403);
            this.ddlListType.Name = "ddlListType";
            this.ddlListType.Size = new System.Drawing.Size(121, 21);
            this.ddlListType.TabIndex = 5;
            this.ddlListType.SelectedIndexChanged += new System.EventHandler(this.ddlListType_SelectedIndexChanged);
            this.ddlListType.DataSourceChanged += new System.EventHandler(this.ddlListType_DataSourceChanged);
            // 
            // ddlLandTypes
            // 
            this.ddlLandTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLandTypes.FormattingEnabled = true;
            this.ddlLandTypes.Location = new System.Drawing.Point(17, 78);
            this.ddlLandTypes.Name = "ddlLandTypes";
            this.ddlLandTypes.Size = new System.Drawing.Size(121, 21);
            this.ddlLandTypes.TabIndex = 13;
            this.ddlLandTypes.SelectedIndexChanged += new System.EventHandler(this.ddlLandTypes_SelectedIndexChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Location = new System.Drawing.Point(48, 109);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(121, 40);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Generate Adangal";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(36, 412);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(66, 13);
            this.lblMessage.TabIndex = 8;
            this.lblMessage.Text = "[lblMessage]";
            // 
            // ddlDistrict
            // 
            this.ddlDistrict.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDistrict.FormattingEnabled = true;
            this.ddlDistrict.Location = new System.Drawing.Point(23, 31);
            this.ddlDistrict.Name = "ddlDistrict";
            this.ddlDistrict.Size = new System.Drawing.Size(186, 21);
            this.ddlDistrict.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "மாவட்டம்";
            // 
            // btnStatusCheck
            // 
            this.btnStatusCheck.Enabled = false;
            this.btnStatusCheck.Location = new System.Drawing.Point(19, 61);
            this.btnStatusCheck.Name = "btnStatusCheck";
            this.btnStatusCheck.Size = new System.Drawing.Size(133, 148);
            this.btnStatusCheck.TabIndex = 12;
            this.btnStatusCheck.Text = "status";
            this.btnStatusCheck.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "வட்டம்";
            // 
            // cmbTaluk
            // 
            this.cmbTaluk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTaluk.FormattingEnabled = true;
            this.cmbTaluk.Location = new System.Drawing.Point(23, 71);
            this.cmbTaluk.Name = "cmbTaluk";
            this.cmbTaluk.Size = new System.Drawing.Size(186, 21);
            this.cmbTaluk.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "வருவாய் கிராமம்";
            // 
            // cmbVillages
            // 
            this.cmbVillages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVillages.FormattingEnabled = true;
            this.cmbVillages.Location = new System.Drawing.Point(19, 186);
            this.cmbVillages.Name = "cmbVillages";
            this.cmbVillages.Size = new System.Drawing.Size(225, 21);
            this.cmbVillages.TabIndex = 4;
            this.cmbVillages.SelectedIndexChanged += new System.EventHandler(this.cmbVillages_SelectedIndexChanged_1);
            // 
            // btnReady
            // 
            this.btnReady.Enabled = false;
            this.btnReady.Location = new System.Drawing.Point(19, 19);
            this.btnReady.Name = "btnReady";
            this.btnReady.Size = new System.Drawing.Size(133, 35);
            this.btnReady.TabIndex = 9;
            this.btnReady.Text = "Ready For Print?";
            this.btnReady.UseVisualStyleBackColor = true;
            this.btnReady.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // btnReadFile
            // 
            this.btnReadFile.Enabled = false;
            this.btnReadFile.Location = new System.Drawing.Point(11, 19);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(112, 23);
            this.btnReadFile.TabIndex = 6;
            this.btnReadFile.Text = "Select Folder...";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // cmbFulfilled
            // 
            this.cmbFulfilled.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFulfilled.FormattingEnabled = true;
            this.cmbFulfilled.Location = new System.Drawing.Point(17, 124);
            this.cmbFulfilled.Name = "cmbFulfilled";
            this.cmbFulfilled.Size = new System.Drawing.Size(121, 21);
            this.cmbFulfilled.TabIndex = 14;
            this.cmbFulfilled.SelectedIndexChanged += new System.EventHandler(this.cmbFulfilled_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(20, 19);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(117, 20);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete Non Existing.";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtVaruvai);
            this.groupBox1.Controls.Add(this.txtVattam);
            this.groupBox1.Controls.Add(this.txtFirka);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ddlDistrict);
            this.groupBox1.Controls.Add(this.cmbTaluk);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbVillages);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(17, -2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 242);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "STEP-1";
            // 
            // txtVaruvai
            // 
            this.txtVaruvai.Location = new System.Drawing.Point(19, 210);
            this.txtVaruvai.Name = "txtVaruvai";
            this.txtVaruvai.Size = new System.Drawing.Size(184, 20);
            this.txtVaruvai.TabIndex = 5;
            // 
            // txtVattam
            // 
            this.txtVattam.Location = new System.Drawing.Point(24, 96);
            this.txtVattam.Name = "txtVattam";
            this.txtVattam.Size = new System.Drawing.Size(184, 20);
            this.txtVattam.TabIndex = 2;
            // 
            // txtFirka
            // 
            this.txtFirka.Location = new System.Drawing.Point(23, 140);
            this.txtFirka.Name = "txtFirka";
            this.txtFirka.Size = new System.Drawing.Size(100, 20);
            this.txtFirka.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Firkaa (உள்வட்டம்)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnLoadProcessed);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.ddlProcessedFiles);
            this.groupBox2.Controls.Add(this.btnReadFile);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(17, 255);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(253, 121);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "STEP-2";
            // 
            // btnLoadProcessed
            // 
            this.btnLoadProcessed.Enabled = false;
            this.btnLoadProcessed.Location = new System.Drawing.Point(46, 88);
            this.btnLoadProcessed.Name = "btnLoadProcessed";
            this.btnLoadProcessed.Size = new System.Drawing.Size(112, 23);
            this.btnLoadProcessed.TabIndex = 8;
            this.btnLoadProcessed.Text = "Load Files";
            this.btnLoadProcessed.UseVisualStyleBackColor = true;
            this.btnLoadProcessed.Click += new System.EventHandler(this.btnLoadProcessed_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "OR";
            // 
            // ddlProcessedFiles
            // 
            this.ddlProcessedFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlProcessedFiles.FormattingEnabled = true;
            this.ddlProcessedFiles.Location = new System.Drawing.Point(11, 61);
            this.ddlProcessedFiles.Name = "ddlProcessedFiles";
            this.ddlProcessedFiles.Size = new System.Drawing.Size(213, 21);
            this.ddlProcessedFiles.TabIndex = 7;
            this.ddlProcessedFiles.SelectedIndexChanged += new System.EventHandler(this.ddlProcessedFiles_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnReady);
            this.groupBox3.Controls.Add(this.btnStatusCheck);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(294, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(178, 236);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "STEP-3";
            // 
            // btnPercentage
            // 
            this.btnPercentage.Enabled = false;
            this.btnPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 35F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPercentage.Location = new System.Drawing.Point(17, 19);
            this.btnPercentage.Name = "btnPercentage";
            this.btnPercentage.Size = new System.Drawing.Size(208, 70);
            this.btnPercentage.TabIndex = 18;
            this.btnPercentage.Text = "%";
            this.btnPercentage.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnAdd);
            this.groupBox4.Controls.Add(this.btnDelete);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(294, 255);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(178, 79);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "STEP-4";
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(20, 45);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(117, 22);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add  Non Existing.";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnPercentage);
            this.groupBox5.Controls.Add(this.btnGenerate);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(485, 179);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(234, 165);
            this.groupBox5.TabIndex = 24;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "STEP-6 (LAST)";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.ddlPattaTypes);
            this.groupBox6.Controls.Add(this.ddlLandTypes);
            this.groupBox6.Controls.Add(this.cmbFulfilled);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(485, 13);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(234, 153);
            this.groupBox6.TabIndex = 25;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "STEP-5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Other Check";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Adangal  Check";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Patta Check";
            // 
            // txtAddNewSurvey
            // 
            this.txtAddNewSurvey.Enabled = false;
            this.txtAddNewSurvey.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtAddNewSurvey.Location = new System.Drawing.Point(28, 43);
            this.txtAddNewSurvey.Multiline = true;
            this.txtAddNewSurvey.Name = "txtAddNewSurvey";
            this.txtAddNewSurvey.Size = new System.Drawing.Size(408, 278);
            this.txtAddNewSurvey.TabIndex = 26;
            // 
            // btnAddNewSurvey
            // 
            this.btnAddNewSurvey.Location = new System.Drawing.Point(153, 348);
            this.btnAddNewSurvey.Name = "btnAddNewSurvey";
            this.btnAddNewSurvey.Size = new System.Drawing.Size(75, 23);
            this.btnAddNewSurvey.TabIndex = 27;
            this.btnAddNewSurvey.Text = "Add";
            this.btnAddNewSurvey.UseVisualStyleBackColor = true;
            this.btnAddNewSurvey.Click += new System.EventHandler(this.btnAddNewSurvey_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cmbItemToBeAdded);
            this.groupBox7.Controls.Add(this.txtAddNewSurvey);
            this.groupBox7.Controls.Add(this.btnAddNewSurvey);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(753, 23);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(455, 382);
            this.groupBox7.TabIndex = 28;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Add Missed Survey";
            // 
            // cmbItemToBeAdded
            // 
            this.cmbItemToBeAdded.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemToBeAdded.FormattingEnabled = true;
            this.cmbItemToBeAdded.Location = new System.Drawing.Point(315, 16);
            this.cmbItemToBeAdded.Name = "cmbItemToBeAdded";
            this.cmbItemToBeAdded.Size = new System.Drawing.Size(121, 21);
            this.cmbItemToBeAdded.TabIndex = 29;
            // 
            // cmbSurveyNo
            // 
            this.cmbSurveyNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSurveyNo.FormattingEnabled = true;
            this.cmbSurveyNo.Location = new System.Drawing.Point(226, 404);
            this.cmbSurveyNo.Name = "cmbSurveyNo";
            this.cmbSurveyNo.Size = new System.Drawing.Size(65, 21);
            this.cmbSurveyNo.TabIndex = 29;
            // 
            // cmbSubdivNo
            // 
            this.cmbSubdivNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubdivNo.FormattingEnabled = true;
            this.cmbSubdivNo.Location = new System.Drawing.Point(309, 404);
            this.cmbSubdivNo.Name = "cmbSubdivNo";
            this.cmbSubdivNo.Size = new System.Drawing.Size(57, 21);
            this.cmbSubdivNo.TabIndex = 30;
            // 
            // lblSurveyNo
            // 
            this.lblSurveyNo.AutoSize = true;
            this.lblSurveyNo.Location = new System.Drawing.Point(226, 388);
            this.lblSurveyNo.Name = "lblSurveyNo";
            this.lblSurveyNo.Size = new System.Drawing.Size(52, 13);
            this.lblSurveyNo.TabIndex = 31;
            this.lblSurveyNo.Text = "surveyNo";
            // 
            // lblSubdiv
            // 
            this.lblSubdiv.AutoSize = true;
            this.lblSubdiv.Location = new System.Drawing.Point(307, 389);
            this.lblSubdiv.Name = "lblSubdiv";
            this.lblSubdiv.Size = new System.Drawing.Size(53, 13);
            this.lblSubdiv.TabIndex = 32;
            this.lblSubdiv.Text = "subdiv no";
            // 
            // chkEdit
            // 
            this.chkEdit.AutoSize = true;
            this.chkEdit.Location = new System.Drawing.Point(533, 406);
            this.chkEdit.Name = "chkEdit";
            this.chkEdit.Size = new System.Drawing.Size(92, 17);
            this.chkEdit.TabIndex = 20;
            this.chkEdit.Text = "EDIT MODE?";
            this.chkEdit.UseVisualStyleBackColor = true;
            this.chkEdit.CheckedChanged += new System.EventHandler(this.chkEdit_CheckedChanged);
            // 
            // vao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 557);
            this.Controls.Add(this.chkEdit);
            this.Controls.Add(this.lblSubdiv);
            this.Controls.Add(this.lblSurveyNo);
            this.Controls.Add(this.cmbSubdivNo);
            this.Controls.Add(this.cmbSurveyNo);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ddlListType);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "vao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "vao";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.vao_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
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
        private System.Windows.Forms.ComboBox ddlDistrict;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStatusCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTaluk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbVillages;
        private System.Windows.Forms.Button btnReady;
        private System.Windows.Forms.Button btnReadFile;
        private System.Windows.Forms.ComboBox cmbFulfilled;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtAddNewSurvey;
        private System.Windows.Forms.Button btnAddNewSurvey;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ComboBox cmbItemToBeAdded;
        private System.Windows.Forms.ComboBox cmbSurveyNo;
        private System.Windows.Forms.ComboBox cmbSubdivNo;
        private System.Windows.Forms.Label lblSurveyNo;
        private System.Windows.Forms.Label lblSubdiv;
        private System.Windows.Forms.CheckBox chkEdit;
        private System.Windows.Forms.TextBox txtFirka;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ddlProcessedFiles;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtVaruvai;
        private System.Windows.Forms.TextBox txtVattam;
        private System.Windows.Forms.Button btnPercentage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnLoadProcessed;
    }
}