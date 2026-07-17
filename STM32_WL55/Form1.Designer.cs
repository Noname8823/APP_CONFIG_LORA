namespace STM32_WL55
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cmbBaud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numPower = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.cmbCr = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbSf = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbBw = new System.Windows.Forms.ComboBox();
            this.Bandwidth = new System.Windows.Forms.Label();
            this.cmbFreq = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numDestId = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numNodeId = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnGetInput = new System.Windows.Forms.Button();
            this.btnGetConfig = new System.Windows.Forms.Button();
            this.btnTestAT = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpSensorConfig = new System.Windows.Forms.GroupBox();
            this.btnSensorRemove = new System.Windows.Forms.Button();
            this.btnSensorUpdate = new System.Windows.Forms.Button();
            this.btnSensorAdd = new System.Windows.Forms.Button();
            this.cmbRegisterLength = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbStartRegister = new System.Windows.Forms.ComboBox();
            this.cmbSensorFunction = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbSensorSlave = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvSensorList = new System.Windows.Forms.DataGridView();
            this.colSlaveId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFunctionCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartRegister = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegisterLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDestId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNodeId)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.grpSensorConfig.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSensorList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.cmbBaud);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 95);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(664, 27);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(537, 27);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // cmbBaud
            // 
            this.cmbBaud.FormattingEnabled = true;
            this.cmbBaud.Location = new System.Drawing.Point(338, 29);
            this.cmbBaud.Name = "cmbBaud";
            this.cmbBaud.Size = new System.Drawing.Size(121, 21);
            this.cmbBaud.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(282, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baudrate";
            // 
            // cmbPort
            // 
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(110, 29);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(121, 21);
            this.cmbPort.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "COM Port";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numPower);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btnApply);
            this.groupBox2.Controls.Add(this.cmbCr);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.cmbSf);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cmbBw);
            this.groupBox2.Controls.Add(this.Bandwidth);
            this.groupBox2.Controls.Add(this.cmbFreq);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbRole);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numDestId);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numNodeId);
            this.groupBox2.Location = new System.Drawing.Point(5, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 343);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "LoRa Config";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // numPower
            // 
            this.numPower.Location = new System.Drawing.Point(117, 269);
            this.numPower.Name = "numPower";
            this.numPower.Size = new System.Drawing.Size(120, 20);
            this.numPower.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(325, 28);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save Flash";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 273);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "TX Power";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(183, 305);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click_1);
            // 
            // cmbCr
            // 
            this.cmbCr.FormattingEnabled = true;
            this.cmbCr.Location = new System.Drawing.Point(117, 234);
            this.cmbCr.Name = "cmbCr";
            this.cmbCr.Size = new System.Drawing.Size(121, 21);
            this.cmbCr.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 238);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Coding Rate";
            // 
            // cmbSf
            // 
            this.cmbSf.FormattingEnabled = true;
            this.cmbSf.Location = new System.Drawing.Point(117, 199);
            this.cmbSf.Name = "cmbSf";
            this.cmbSf.Size = new System.Drawing.Size(121, 21);
            this.cmbSf.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Spreading Factor";
            // 
            // cmbBw
            // 
            this.cmbBw.FormattingEnabled = true;
            this.cmbBw.Location = new System.Drawing.Point(117, 164);
            this.cmbBw.Name = "cmbBw";
            this.cmbBw.Size = new System.Drawing.Size(121, 21);
            this.cmbBw.TabIndex = 11;
            // 
            // Bandwidth
            // 
            this.Bandwidth.AutoSize = true;
            this.Bandwidth.Location = new System.Drawing.Point(22, 168);
            this.Bandwidth.Name = "Bandwidth";
            this.Bandwidth.Size = new System.Drawing.Size(57, 13);
            this.Bandwidth.TabIndex = 10;
            this.Bandwidth.Text = "Bandwidth";
            // 
            // cmbFreq
            // 
            this.cmbFreq.FormattingEnabled = true;
            this.cmbFreq.Location = new System.Drawing.Point(117, 129);
            this.cmbFreq.Name = "cmbFreq";
            this.cmbFreq.Size = new System.Drawing.Size(121, 21);
            this.cmbFreq.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Frequency";
            // 
            // cmbRole
            // 
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(117, 94);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(121, 21);
            this.cmbRole.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Role";
            // 
            // numDestId
            // 
            this.numDestId.Location = new System.Drawing.Point(117, 60);
            this.numDestId.Name = "numDestId";
            this.numDestId.Size = new System.Drawing.Size(120, 20);
            this.numDestId.TabIndex = 5;
            this.numDestId.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Destination ID";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Node ID";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // numNodeId
            // 
            this.numNodeId.Location = new System.Drawing.Point(117, 26);
            this.numNodeId.Name = "numNodeId";
            this.numNodeId.Size = new System.Drawing.Size(120, 20);
            this.numNodeId.TabIndex = 2;
            this.numNodeId.ValueChanged += new System.EventHandler(this.numNodeId_ValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnGetInput);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Controls.Add(this.btnGetConfig);
            this.groupBox3.Controls.Add(this.btnTestAT);
            this.groupBox3.Location = new System.Drawing.Point(297, 128);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(490, 73);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Command";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // btnGetInput
            // 
            this.btnGetInput.Location = new System.Drawing.Point(210, 28);
            this.btnGetInput.Name = "btnGetInput";
            this.btnGetInput.Size = new System.Drawing.Size(75, 23);
            this.btnGetInput.TabIndex = 4;
            this.btnGetInput.Text = "Get INPUT";
            this.btnGetInput.UseVisualStyleBackColor = true;
            // 
            // btnGetConfig
            // 
            this.btnGetConfig.Location = new System.Drawing.Point(107, 28);
            this.btnGetConfig.Name = "btnGetConfig";
            this.btnGetConfig.Size = new System.Drawing.Size(75, 23);
            this.btnGetConfig.TabIndex = 1;
            this.btnGetConfig.Text = "Get Config";
            this.btnGetConfig.UseVisualStyleBackColor = true;
            // 
            // btnTestAT
            // 
            this.btnTestAT.Location = new System.Drawing.Point(6, 28);
            this.btnTestAT.Name = "btnTestAT";
            this.btnTestAT.Size = new System.Drawing.Size(75, 23);
            this.btnTestAT.TabIndex = 0;
            this.btnTestAT.Text = "Test AT";
            this.btnTestAT.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtLog);
            this.groupBox4.Location = new System.Drawing.Point(297, 221);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(490, 250);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Log";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(5, 19);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(484, 225);
            this.txtLog.TabIndex = 0;
            // 
            // grpSensorConfig
            // 
            this.grpSensorConfig.Controls.Add(this.btnSensorRemove);
            this.grpSensorConfig.Controls.Add(this.btnSensorUpdate);
            this.grpSensorConfig.Controls.Add(this.btnSensorAdd);
            this.grpSensorConfig.Controls.Add(this.cmbRegisterLength);
            this.grpSensorConfig.Controls.Add(this.label13);
            this.grpSensorConfig.Controls.Add(this.label11);
            this.grpSensorConfig.Controls.Add(this.cmbStartRegister);
            this.grpSensorConfig.Controls.Add(this.cmbSensorFunction);
            this.grpSensorConfig.Controls.Add(this.label12);
            this.grpSensorConfig.Controls.Add(this.cmbSensorSlave);
            this.grpSensorConfig.Controls.Add(this.label10);
            this.grpSensorConfig.Location = new System.Drawing.Point(5, 477);
            this.grpSensorConfig.Name = "grpSensorConfig";
            this.grpSensorConfig.Size = new System.Drawing.Size(279, 208);
            this.grpSensorConfig.TabIndex = 4;
            this.grpSensorConfig.TabStop = false;
            this.grpSensorConfig.Text = "RS485 Sensor Config";
            this.grpSensorConfig.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // btnSensorRemove
            // 
            this.btnSensorRemove.Location = new System.Drawing.Point(183, 171);
            this.btnSensorRemove.Name = "btnSensorRemove";
            this.btnSensorRemove.Size = new System.Drawing.Size(75, 23);
            this.btnSensorRemove.TabIndex = 6;
            this.btnSensorRemove.Text = "Remove";
            this.btnSensorRemove.UseVisualStyleBackColor = true;
            // 
            // btnSensorUpdate
            // 
            this.btnSensorUpdate.Location = new System.Drawing.Point(102, 171);
            this.btnSensorUpdate.Name = "btnSensorUpdate";
            this.btnSensorUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnSensorUpdate.TabIndex = 6;
            this.btnSensorUpdate.Text = "Update";
            this.btnSensorUpdate.UseVisualStyleBackColor = true;
            // 
            // btnSensorAdd
            // 
            this.btnSensorAdd.Location = new System.Drawing.Point(21, 171);
            this.btnSensorAdd.Name = "btnSensorAdd";
            this.btnSensorAdd.Size = new System.Drawing.Size(75, 23);
            this.btnSensorAdd.TabIndex = 6;
            this.btnSensorAdd.Text = "Add";
            this.btnSensorAdd.UseVisualStyleBackColor = true;
            this.btnSensorAdd.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbRegisterLength
            // 
            this.cmbRegisterLength.FormattingEnabled = true;
            this.cmbRegisterLength.Location = new System.Drawing.Point(116, 130);
            this.cmbRegisterLength.Name = "cmbRegisterLength";
            this.cmbRegisterLength.Size = new System.Drawing.Size(121, 21);
            this.cmbRegisterLength.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 138);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(82, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "Register Length";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 105);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Start Register";
            // 
            // cmbStartRegister
            // 
            this.cmbStartRegister.FormattingEnabled = true;
            this.cmbStartRegister.Location = new System.Drawing.Point(116, 97);
            this.cmbStartRegister.Name = "cmbStartRegister";
            this.cmbStartRegister.Size = new System.Drawing.Size(121, 21);
            this.cmbStartRegister.TabIndex = 5;
            // 
            // cmbSensorFunction
            // 
            this.cmbSensorFunction.FormattingEnabled = true;
            this.cmbSensorFunction.Location = new System.Drawing.Point(116, 64);
            this.cmbSensorFunction.Name = "cmbSensorFunction";
            this.cmbSensorFunction.Size = new System.Drawing.Size(121, 21);
            this.cmbSensorFunction.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "Function Code";
            // 
            // cmbSensorSlave
            // 
            this.cmbSensorSlave.FormattingEnabled = true;
            this.cmbSensorSlave.Location = new System.Drawing.Point(117, 31);
            this.cmbSensorSlave.Name = "cmbSensorSlave";
            this.cmbSensorSlave.Size = new System.Drawing.Size(121, 21);
            this.cmbSensorSlave.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(22, 39);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Slave ID";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dgvSensorList);
            this.groupBox5.Location = new System.Drawing.Point(296, 477);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(490, 208);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sensor Lists";
            // 
            // dgvSensorList
            // 
            this.dgvSensorList.AllowUserToAddRows = false;
            this.dgvSensorList.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSensorList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSensorList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSensorList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSlaveId,
            this.colFunctionCode,
            this.colStartRegister,
            this.colRegisterLength});
            this.dgvSensorList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSensorList.Location = new System.Drawing.Point(3, 16);
            this.dgvSensorList.Name = "dgvSensorList";
            this.dgvSensorList.ReadOnly = true;
            this.dgvSensorList.RowHeadersVisible = false;
            this.dgvSensorList.Size = new System.Drawing.Size(484, 189);
            this.dgvSensorList.TabIndex = 0;
            this.dgvSensorList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // colSlaveId
            // 
            this.colSlaveId.HeaderText = "Slave ID";
            this.colSlaveId.Name = "colSlaveId";
            this.colSlaveId.ReadOnly = true;
            this.colSlaveId.Width = 120;
            // 
            // colFunctionCode
            // 
            this.colFunctionCode.HeaderText = "Function Code";
            this.colFunctionCode.Name = "colFunctionCode";
            this.colFunctionCode.ReadOnly = true;
            this.colFunctionCode.Width = 120;
            // 
            // colStartRegister
            // 
            this.colStartRegister.HeaderText = "Start Register";
            this.colStartRegister.Name = "colStartRegister";
            this.colStartRegister.ReadOnly = true;
            this.colStartRegister.Width = 120;
            // 
            // colRegisterLength
            // 
            this.colRegisterLength.HeaderText = "Register Length";
            this.colRegisterLength.Name = "colRegisterLength";
            this.colRegisterLength.ReadOnly = true;
            this.colRegisterLength.Width = 120;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 760);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.grpSensorConfig);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDestId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNodeId)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.grpSensorConfig.ResumeLayout(false);
            this.grpSensorConfig.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSensorList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cmbBaud;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numNodeId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numDestId;
        private System.Windows.Forms.ComboBox cmbSf;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbBw;
        private System.Windows.Forms.Label Bandwidth;
        private System.Windows.Forms.ComboBox cmbFreq;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbCr;
        private System.Windows.Forms.NumericUpDown numPower;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnGetInput;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnGetConfig;
        private System.Windows.Forms.Button btnTestAT;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox grpSensorConfig;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbSensorSlave;
        private System.Windows.Forms.ComboBox cmbSensorFunction;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbStartRegister;
        private System.Windows.Forms.ComboBox cmbRegisterLength;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dgvSensorList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSlaveId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFunctionCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartRegister;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegisterLength;
        private System.Windows.Forms.Button btnSensorAdd;
        private System.Windows.Forms.Button btnSensorRemove;
        private System.Windows.Forms.Button btnSensorUpdate;
    }
}

