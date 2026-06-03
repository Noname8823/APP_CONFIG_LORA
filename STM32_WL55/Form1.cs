using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace STM32_WL55
{
    public partial class Form1 : Form
    {
        private SerialPort serial = new SerialPort();
        private StringBuilder rxBuffer = new StringBuilder();

        public Form1()
        {
            InitializeComponent();

            LoadDefaultValues();

            // Gắn sự kiện khi Node ID thay đổi
            numNodeId.ValueChanged -= numNodeId_ValueChanged;
            numNodeId.ValueChanged += numNodeId_ValueChanged;

            // Cập nhật Role ngay từ đầu theo Node ID mặc định
            UpdateRoleByNodeId();

            RefreshComPorts();

            serial.DataReceived += Serial_DataReceived;

            btnRefresh.Click += BtnRefresh_Click;
            btnConnect.Click += BtnConnect_Click;
            btnTestAT.Click += BtnTestAT_Click;
            btnGetConfig.Click += BtnGetConfig_Click;
            btnApply.Click += BtnApply_Click;
            btnSave.Click += BtnSave_Click;
            btnGetInput.Click += BtnGetInput_Click;
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void numNodeId_ValueChanged(object sender, EventArgs e)
        {
            UpdateRoleByNodeId();
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { }

        private void LoadDefaultValues()
        {
            cmbBaud.Items.Clear();
            cmbBaud.Items.Add("9600");
            cmbBaud.Items.Add("19200");
            cmbBaud.Items.Add("38400");
            cmbBaud.Items.Add("57600");
            cmbBaud.Items.Add("115200");
            cmbBaud.SelectedIndex = 0;

            numNodeId.Minimum = 0;
            numNodeId.Maximum = 255;
            numNodeId.Value = 1;

            numDestId.Minimum = 0;
            numDestId.Maximum = 255;
            numDestId.Value = 2;

            numPower.Minimum = -9;
            numPower.Maximum = 22;
            numPower.Value = 14;

            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.Items.Clear();
            cmbRole.Items.Add(new OptionItem("TX - Node phát", "TX"));
            cmbRole.Items.Add(new OptionItem("RX - Node nhận", "RX"));
            cmbRole.SelectedIndex = 0;

            // Khóa Role, không cho người dùng chọn
            cmbRole.Enabled = false;
            cmbRole.TabStop = false;

            cmbFreq.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFreq.Items.Clear();
            cmbFreq.Items.Add(new OptionItem("433 MHz", 433000000));
            cmbFreq.Items.Add(new OptionItem("470 MHz", 470000000));
            cmbFreq.Items.Add(new OptionItem("868 MHz", 868000000));
            cmbFreq.Items.Add(new OptionItem("915 MHz", 915000000));
            cmbFreq.SelectedIndex = 0;

            cmbBw.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBw.Items.Clear();
            cmbBw.Items.Add(new OptionItem("125 kHz", 0));
            cmbBw.Items.Add(new OptionItem("250 kHz", 1));
            cmbBw.Items.Add(new OptionItem("500 kHz", 2));
            cmbBw.SelectedIndex = 0;

            cmbSf.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSf.Items.Clear();
            cmbSf.Items.Add(new OptionItem("SF7", 7));
            cmbSf.Items.Add(new OptionItem("SF8", 8));
            cmbSf.Items.Add(new OptionItem("SF9", 9));
            cmbSf.Items.Add(new OptionItem("SF10", 10));
            cmbSf.Items.Add(new OptionItem("SF11", 11));
            cmbSf.Items.Add(new OptionItem("SF12", 12));
            cmbSf.SelectedIndex = 3;

            cmbCr.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCr.Items.Clear();
            cmbCr.Items.Add(new OptionItem("4/5", 1));
            cmbCr.Items.Add(new OptionItem("4/6", 2));
            cmbCr.Items.Add(new OptionItem("4/7", 3));
            cmbCr.Items.Add(new OptionItem("4/8", 4));
            cmbCr.SelectedIndex = 0;

            txtLog.Multiline = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.ReadOnly = true;
        }

        private void UpdateRoleByNodeId()
        {
            int nodeId = (int)numNodeId.Value;

            if (nodeId == 0)
            {
                // Node ID = 0 thì bắt buộc là RX
                SelectComboByValue(cmbRole, "RX");
            }
            else
            {
                // Node ID >= 1 thì bắt buộc là TX
                SelectComboByValue(cmbRole, "TX");
            }

            // Luôn khóa Role, không cho người dùng đụng tới
            cmbRole.Enabled = false;
            cmbRole.TabStop = false;
        }
        private void RefreshComPorts()
        {
            cmbPort.Items.Clear();

            string[] ports = SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                cmbPort.Items.Add(port);
            }

            if (ports.Length > 0)
            {
                cmbPort.SelectedIndex = 0;
            }
            else
            {
                Log("Không tìm thấy COM port");
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshComPorts();
            Log("Refresh COM ports");
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serial.IsOpen)
                {
                    serial.Close();
                    btnConnect.Text = "Connect";
                    Log("Disconnected");
                    return;
                }

                if (cmbPort.Text == "")
                {
                    MessageBox.Show("Chưa chọn COM port");
                    return;
                }

                serial.PortName = cmbPort.Text;
                serial.BaudRate = int.Parse(cmbBaud.Text);
                serial.DataBits = 8;
                serial.Parity = Parity.None;
                serial.StopBits = StopBits.One;
                serial.Handshake = Handshake.None;
                serial.Encoding = Encoding.ASCII;

                serial.NewLine = "\r\n";
                serial.ReadTimeout = 500;
                serial.WriteTimeout = 500;

                serial.Open();

                btnConnect.Text = "Disconnect";
                Log("Connected to " + serial.PortName);

                SendCommand("AT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COM Error");
            }
        }

        private void BtnTestAT_Click(object sender, EventArgs e)
        {
            SendCommand("AT");
        }

        private void BtnGetConfig_Click(object sender, EventArgs e)
        {
            SendCommand("AT+GETCFG");
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            if (!CheckSerialOpen())
            {
                return;
            }

            UpdateRoleByNodeId();

            int nodeId = (int)numNodeId.Value;
            int destId = (int)numDestId.Value;
            string role = GetSelectedStringValue(cmbRole);

            int frequency = GetSelectedIntValue(cmbFreq);
            int bandwidth = GetSelectedIntValue(cmbBw);
            int spreadingFactor = GetSelectedIntValue(cmbSf);
            int codingRate = GetSelectedIntValue(cmbCr);
            int power = (int)numPower.Value;

            SendCommandDelay("AT+SETID=" + nodeId);
            SendCommandDelay("AT+SETDST=" + destId);
            SendCommandDelay("AT+SETROLE=" + role);
            SendCommandDelay("AT+SETFREQ=" + frequency);
            SendCommandDelay("AT+SETBW=" + bandwidth);
            SendCommandDelay("AT+SETSF=" + spreadingFactor);
            SendCommandDelay("AT+SETCR=" + codingRate);
            SendCommandDelay("AT+SETPWR=" + power);

            SendCommandDelay("AT+GETCFG");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SendCommand("AT+SAVE");
        }

        private void BtnGetInput_Click(object sender, EventArgs e)
        {
            SendCommand("AT+GETIN");
        }

        private bool CheckSerialOpen()
        {
            if (!serial.IsOpen)
            {
                MessageBox.Show("Chưa Connect COM port");
                return false;
            }

            return true;
        }

        private void SendCommand(string cmd)
        {
            try
            {
                if (!serial.IsOpen)
                {
                    Log("COM chưa mở");
                    return;
                }

                serial.WriteLine(cmd);
                Log("TX: " + cmd);
            }
            catch (Exception ex)
            {
                Log("Send error: " + ex.Message);
            }
        }

        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serial.ReadExisting();

                BeginInvoke(new Action(() =>
                {
                    rxBuffer.Append(data);
                    ProcessReceivedBuffer();
                }));
            }
            catch
            {
            }
        }

        private void ProcessReceivedBuffer()
        {
            string all = rxBuffer.ToString();

            while (true)
            {
                int indexR = all.IndexOf('\r');
                int indexN = all.IndexOf('\n');

                int index;

                if (indexR < 0 && indexN < 0)
                {
                    break;
                }

                if (indexR < 0)
                {
                    index = indexN;
                }
                else if (indexN < 0)
                {
                    index = indexR;
                }
                else
                {
                    index = Math.Min(indexR, indexN);
                }

                string line = all.Substring(0, index).Trim();
                all = all.Substring(index + 1);

                if (line.Length > 0)
                {
                    Log("RX: " + line);
                    ParseLine(line);
                }
            }

            rxBuffer.Clear();
            rxBuffer.Append(all);
        }

        private void ParseLine(string line)
        {
            if (line.StartsWith("CFG"))
            {
                ParseConfig(line);
            }
            else
            {
                Log("DATA: " + line);
            }
        }
        private void SendCommandDelay(string cmd)
        {
            SendCommand(cmd);
            System.Threading.Thread.Sleep(120);
        }
        private void ParseConfig(string line)
        {
            try
            {
                string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in parts)
                {
                    if (item.StartsWith("ID="))
                    {
                        int id = int.Parse(item.Substring(3));
                        if (id >= numNodeId.Minimum && id <= numNodeId.Maximum)
                            numNodeId.Value = id;
                    }
                    else if (item.StartsWith("DST="))
                    {
                        int dst = int.Parse(item.Substring(4));
                        if (dst >= numDestId.Minimum && dst <= numDestId.Maximum)
                            numDestId.Value = dst;
                    }
                    else if (item.StartsWith("ROLE="))
                    {
                        string role = item.Substring(5);
                        SelectComboByValue(cmbRole, role);
                    }
                    else if (item.StartsWith("FREQ="))
                    {
                        int freq = int.Parse(item.Substring(5));
                        SelectComboByValue(cmbFreq, freq);
                    }
                    else if (item.StartsWith("BW="))
                    {
                        int bw = int.Parse(item.Substring(3));
                        SelectComboByValue(cmbBw, bw);
                    }
                    else if (item.StartsWith("SF="))
                    {
                        int sf = int.Parse(item.Substring(3));
                        SelectComboByValue(cmbSf, sf);
                    }
                    else if (item.StartsWith("CR="))
                    {
                        int cr = int.Parse(item.Substring(3));
                        SelectComboByValue(cmbCr, cr);
                    }
                    else if (item.StartsWith("PWR="))
                    {
                        int pwr = int.Parse(item.Substring(4));
                        if (pwr >= numPower.Minimum && pwr <= numPower.Maximum)
                            numPower.Value = pwr;
                    }
                }
            }
            catch
            {
                Log("Parse CFG fail");
            }
        }

        private int GetSelectedIntValue(ComboBox cmb)
        {
            OptionItem item = cmb.SelectedItem as OptionItem;
            if (item == null) return 0;
            return Convert.ToInt32(item.Value);
        }

        private string GetSelectedStringValue(ComboBox cmb)
        {
            OptionItem item = cmb.SelectedItem as OptionItem;
            if (item == null) return "";
            return item.Value.ToString();
        }

        private void SelectComboByValue(ComboBox cmb, object value)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                OptionItem item = cmb.Items[i] as OptionItem;
                if (item == null) continue;
                if (item.Value.ToString() == value.ToString())
                {
                    cmb.SelectedIndex = i;
                    return;
                }
            }
        }

        private void Log(string text)
        {
            txtLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + "  " + text + Environment.NewLine);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (serial.IsOpen)
            {
                serial.Close();
            }

            base.OnFormClosing(e);
        }

        private void btnApply_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {

        }
    }

    public class OptionItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public OptionItem(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}