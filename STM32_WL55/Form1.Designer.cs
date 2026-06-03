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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cmbBaud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numPower = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnGetConfig = new System.Windows.Forms.Button();
            this.btnTestAT = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDestId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNodeId)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
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
            this.groupBox2.Size = new System.Drawing.Size(279, 310);
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
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 273);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "TX Power";
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
            this.groupBox3.Controls.Add(this.btnApply);
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
            this.btnGetInput.Location = new System.Drawing.Point(409, 28);
            this.btnGetInput.Name = "btnGetInput";
            this.btnGetInput.Size = new System.Drawing.Size(75, 23);
            this.btnGetInput.TabIndex = 4;
            this.btnGetInput.Text = "Get INPUT";
            this.btnGetInput.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(314, 28);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save Flash";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(212, 28);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click_1);
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
            this.groupBox4.Size = new System.Drawing.Size(490, 216);
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
            this.txtLog.Size = new System.Drawing.Size(484, 196);
            this.txtLog.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

