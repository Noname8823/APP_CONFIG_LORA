using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM32_WL55
{
    public partial class Form1 : Form
    {
        private const int MaxSensorCount = 8;
        private const int MaxRegisterLength = 7;
        private const int FixedBaudRate = 9600;
        private const int MaxRxBufferSize = 4096;

        private const int AtTimeoutMs = 2000;
        private const int SensorTimeoutMs = 10000;
        private const int GetConfigTimeoutMs = 8000;
        private const int GetSensorsTimeoutMs = 8000;
        /*
         * USB-RS485/driver cần một khoảng ngắn sau khi mở COM.
         * Đồng thời retry AT để tự phục hồi nếu STM32 còn một dòng RX dở.
         */
        private const int PortOpenSettleMs = 300;
        private const int HandshakeRetryCount = 3;
        private const int HandshakeRetryDelayMs = 200;
        private const int RxLinePurgeDelayMs = 80;

        private readonly SerialPort serial = new SerialPort();
        private readonly StringBuilder rxBuffer = new StringBuilder();

        /*
         * Chỉ cho phép một command đang chờ phản hồi.
         */
        private readonly SemaphoreSlim commandGate =
            new SemaphoreSlim(1, 1);

        /*
         * Phản hồi cho command thông thường:
         * OK hoặc ERR.
         */
        private TaskCompletionSource<string>
            pendingSimpleCommand;

        /*
         * Phản hồi kiểm tra cảm biến:
         * SENSORTEST OK hoặc SENSORTEST ERR.
         */
        private TaskCompletionSource<string>
            pendingSensorTest;

        /*
         * Phản hồi AT+GETCFG kết thúc bằng END.
         */
        private TaskCompletionSource<bool>
            pendingGetConfig;

        private TaskCompletionSource<bool>
            pendingGetSensors;

        /*
         * Danh sách tạm nhận từ STM32.
         *
         * Chỉ khi nhận END mới thay dữ liệu trên bảng.
         */
        private readonly List<SensorConfigItem>
            receivedConfigBuffer =
                new List<SensorConfigItem>();

        private bool stm32Ready;
        private bool isBusy;

        public Form1()
        {
            InitializeComponent();

            LoadDefaultValues();
            InitializeSensorInputs();
            InitializeSensorGrid();

            numNodeId.ValueChanged -=
                numNodeId_ValueChanged;

            numNodeId.ValueChanged +=
                numNodeId_ValueChanged;

            UpdateRoleByNodeId();
            RefreshComPorts();

            serial.DataReceived -= Serial_DataReceived;
            serial.DataReceived += Serial_DataReceived;

            btnRefresh.Click -= BtnRefresh_Click;
            btnRefresh.Click += BtnRefresh_Click;

            btnConnect.Click -= BtnConnect_Click;
            btnConnect.Click += BtnConnect_Click;

            btnTestAT.Click -= BtnTestAT_Click;
            btnTestAT.Click += BtnTestAT_Click;

            btnGetConfig.Click -= BtnGetConfig_Click;
            btnGetConfig.Click += BtnGetConfig_Click;

            btnApply.Click -= BtnApply_Click;
            btnApply.Click += BtnApply_Click;

            btnSave.Click -= BtnSave_Click;
            btnSave.Click += BtnSave_Click;

            btnGetInput.Click -= BtnGetInput_Click;
            btnGetInput.Click += BtnGetInput_Click;

            btnSensorAdd.Click -= BtnSensorAdd_Click;
            btnSensorAdd.Click += BtnSensorAdd_Click;

            btnGetSensor.Click -= BtnGetSensor_Click;
            btnGetSensor.Click += BtnGetSensor_Click;

            btnSensorRemove.Click -= BtnSensorRemove_Click;
            btnSensorRemove.Click += BtnSensorRemove_Click;

            btnChangeSlaveId.Click -= BtnChangeSlaveId_Click;

            btnChangeSlaveId.Click += BtnChangeSlaveId_Click;

            dgvSensorList.CellDoubleClick -=
                DgvSensorList_CellDoubleClick;

            dgvSensorList.CellDoubleClick +=
                DgvSensorList_CellDoubleClick;

            dgvSensorList.KeyDown -=
                DgvSensorList_KeyDown;

            dgvSensorList.KeyDown +=
                DgvSensorList_KeyDown;

            dgvSensorList.SelectionChanged -=
                DgvSensorList_SelectionChanged;

            dgvSensorList.SelectionChanged +=
                DgvSensorList_SelectionChanged;

            stm32Ready = false;
            isBusy = false;

            UpdateConnectionControls();
        }

        /* =====================================================
         * DEFAULT VALUES
         * ===================================================== */

        private void LoadDefaultValues()
        {
            cmbBaud.Items.Clear();
            cmbBaud.Items.Add("9600");
            cmbBaud.SelectedIndex = 0;
            cmbBaud.Enabled = false;
            cmbBaud.DropDownStyle =
                ComboBoxStyle.DropDownList;

            numNodeId.Minimum = 0;
            numNodeId.Maximum = 255;
            numNodeId.Value = 1;

            numDestId.Minimum = 0;
            numDestId.Maximum = 255;
            numDestId.Value = 2;

            numPower.Minimum = -9;
            numPower.Maximum = 22;
            numPower.Value = 14;

            cmbRole.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbRole.Items.Clear();

            cmbRole.Items.Add(
                new OptionItem(
                    "TX - Node phát",
                    "TX"));

            cmbRole.Items.Add(
                new OptionItem(
                    "RX - Node nhận",
                    "RX"));

            cmbRole.SelectedIndex = 0;
            cmbRole.Enabled = false;
            cmbRole.TabStop = false;

            cmbFreq.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbFreq.Items.Clear();

            cmbFreq.Items.Add(
                new OptionItem(
                    "433 MHz",
                    433000000));

            cmbFreq.Items.Add(
                new OptionItem(
                    "470 MHz",
                    470000000));
            cmbFreq.Items.Add(
                new OptionItem(
                    "868 MHz",
                    868000000));

            cmbFreq.Items.Add(
                new OptionItem(
                    "915 MHz",
                    915000000));

            cmbFreq.Items.Add(
                new OptionItem(
                    "920 MHz",
                    920000000));

            cmbFreq.SelectedIndex = 4;

            cmbBw.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbBw.Items.Clear();

            cmbBw.Items.Add(
                new OptionItem("125 kHz", 0));

            cmbBw.Items.Add(
                new OptionItem("250 kHz", 1));

            cmbBw.Items.Add(
                new OptionItem("500 kHz", 2));

            cmbBw.SelectedIndex = 0;

            cmbSf.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbSf.Items.Clear();

            for (int sf = 7; sf <= 12; sf++)
            {
                cmbSf.Items.Add(
                    new OptionItem(
                        "SF" + sf,
                        sf));
            }

            cmbSf.SelectedIndex = 3;

            cmbCr.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbCr.Items.Clear();

            cmbCr.Items.Add(
                new OptionItem("4/5", 1));

            cmbCr.Items.Add(
                new OptionItem("4/6", 2));

            cmbCr.Items.Add(
                new OptionItem("4/7", 3));

            cmbCr.Items.Add(
                new OptionItem("4/8", 4));

            cmbCr.SelectedIndex = 0;

            txtLog.Multiline = true;
            txtLog.ScrollBars =
                ScrollBars.Vertical;

            txtLog.ReadOnly = true;
        }

        /* =====================================================
         * SENSOR INPUT CONTROLS
         * ===================================================== */

        private void InitializeSensorInputs()
        {
            /*
             * Slave ID: hiển thị 01..08,
             * giá trị thật là 1..8.
             */
            cmbSensorSlave.Items.Clear();

            for (int id = 1;
                 id <= MaxSensorCount;
                 id++)
            {
                cmbSensorSlave.Items.Add(
                    new OptionItem(
                        id.ToString("D2"),
                        id));
            }

            cmbSensorSlave.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbSensorSlave.SelectedIndex = 0;

            /*
             * Function Code.
             */
            cmbSensorFunction.Items.Clear();

            cmbSensorFunction.Items.Add(
                new OptionItem("03", 3));

            cmbSensorFunction.Items.Add(
                new OptionItem("04", 4));

            cmbSensorFunction.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbSensorFunction.SelectedIndex = 0;

            /*
             * Start Register cho phép nhập.
             */
            cmbStartRegister.Items.Clear();

            cmbStartRegister.Items.Add("0x0000");
            cmbStartRegister.Items.Add("0x0001");
            cmbStartRegister.Items.Add("0x0010");
            cmbStartRegister.Items.Add("0x0100");

            cmbStartRegister.DropDownStyle =
                ComboBoxStyle.DropDown;

            cmbStartRegister.Text = "0x0000";

            /*
             * Register Length:
             * hiển thị 0001..0007.
             */
            cmbRegisterLength.Items.Clear();

            for (int length = 1;
                 length <= MaxRegisterLength;
                 length++)
            {
                cmbRegisterLength.Items.Add(
                    new OptionItem(
                        length.ToString("D4"),
                        length));
            }

            cmbRegisterLength.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbRegisterLength.SelectedIndex = 0;
                        /*
             * Slave ID mới dùng cho chức năng
             * đổi địa chỉ thật của cảm biến.
             */
            cmbChangeSlaveId.Items.Clear();

            for (int id = 1;
                 id <= MaxSensorCount;
                 id++)
            {
                cmbChangeSlaveId.Items.Add(
                    new OptionItem(
                        id.ToString("D2"),
                        id));
            }

            cmbChangeSlaveId.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbChangeSlaveId.SelectedIndex = 0;
        }
        /* =====================================================
         * SENSOR GRID
         * ===================================================== */

        private void InitializeSensorGrid()
        {
            dgvSensorList.AllowUserToAddRows =
                false;

            dgvSensorList.AllowUserToDeleteRows =
                false;

            dgvSensorList.AllowUserToOrderColumns =
                false;

            dgvSensorList.ReadOnly = true;
            dgvSensorList.RowHeadersVisible = false;
            dgvSensorList.MultiSelect = false;

            dgvSensorList.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvSensorList.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvSensorList.Columns.Count != 4)
            {
                dgvSensorList.Columns.Clear();

                dgvSensorList.Columns.Add(
                    "colSlaveId",
                    "Slave ID");

                dgvSensorList.Columns.Add(
                    "colFunctionCode",
                    "Function Code");

                dgvSensorList.Columns.Add(
                    "colStartRegister",
                    "Start Register");

                dgvSensorList.Columns.Add(
                    "colRegisterLength",
                    "Register Length");
            }
            else
            {
                dgvSensorList.Columns[0].Name =
                    "colSlaveId";

                dgvSensorList.Columns[0].HeaderText =
                    "Slave ID";

                dgvSensorList.Columns[1].Name =
                    "colFunctionCode";

                dgvSensorList.Columns[1].HeaderText =
                    "Function Code";

                dgvSensorList.Columns[2].Name =
                    "colStartRegister";

                dgvSensorList.Columns[2].HeaderText =
                    "Start Register";

                dgvSensorList.Columns[3].Name =
                    "colRegisterLength";

                dgvSensorList.Columns[3].HeaderText =
                    "Register Length";
            }

            dgvSensorList.EnableHeadersVisualStyles =
                false;

            dgvSensorList
                .ColumnHeadersDefaultCellStyle
                .BackColor = Color.Black;

            dgvSensorList
                .ColumnHeadersDefaultCellStyle
                .ForeColor = Color.White;

            dgvSensorList
                .ColumnHeadersDefaultCellStyle
                .Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvSensorList
                .ColumnHeadersDefaultCellStyle
                .Font =
                new Font(
                    dgvSensorList.Font,
                    FontStyle.Bold);

            dgvSensorList
                .DefaultCellStyle
                .Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvSensorList
                .DefaultCellStyle
                .SelectionBackColor =
                Color.Black;

            dgvSensorList
                .DefaultCellStyle
                .SelectionForeColor =
                Color.White;

            dgvSensorList.Rows.Clear();
        }


        /* =====================================================
         * CONTROL STATE
         * ===================================================== */

        private void UpdateConnectionControls()
        {
            bool portOpen;

            try
            {
                portOpen = serial.IsOpen;
            }
            catch
            {
                portOpen = false;
            }

            bool ready =
                portOpen &&
                stm32Ready &&
                !isBusy;

            btnConnect.Enabled = !isBusy;

            btnRefresh.Enabled =
                !portOpen &&
                !isBusy;

            cmbPort.Enabled =
                !portOpen &&
                !isBusy;

            btnTestAT.Enabled =
                portOpen &&
                !isBusy;

            btnGetConfig.Enabled = ready;
            btnGetSensor.Enabled = ready;
            btnGetInput.Enabled = ready;
            btnApply.Enabled = ready;
            btnSave.Enabled = ready;
            btnSensorAdd.Enabled = ready;

            bool rowSelected =
                dgvSensorList.SelectedRows.Count > 0;

            btnChangeSlaveId.Enabled =
                ready &&
                rowSelected;

            cmbChangeSlaveId.Enabled =
                ready &&
                rowSelected;

            btnSensorRemove.Enabled =
                ready &&
                rowSelected;

            cmbSensorSlave.Enabled = ready;
            cmbSensorFunction.Enabled = ready;
            cmbStartRegister.Enabled = ready;
            cmbRegisterLength.Enabled = ready;

        }

        private bool CheckDeviceReady()
        {
            if (!serial.IsOpen)
            {
                MessageBox.Show(
                    "Chưa kết nối COM port.",
                    "Connection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (!stm32Ready)
            {
                MessageBox.Show(
                    "COM đã mở nhưng STM32 chưa phản hồi AT.\r\n\r\n" +
                    "Kiểm tra PA1=0, A/B/GND và baudrate 9600.",
                    "STM32 chưa sẵn sàng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private void SetBusy(bool busy)
        {
            isBusy = busy;
            UpdateConnectionControls();
        }

        /* =====================================================
         * COM
         * ===================================================== */

        private void RefreshComPorts()
        {
            string oldPort = cmbPort.Text;

            cmbPort.Items.Clear();

            string[] ports =
                SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                cmbPort.Items.Add(port);
            }

            if (ports.Length == 0)
            {
                Log("Không tìm thấy COM port.");
                return;
            }

            int oldIndex =
                cmbPort.Items.IndexOf(oldPort);

            cmbPort.SelectedIndex =
                oldIndex >= 0
                    ? oldIndex
                    : 0;
        }

        /*
         * Kết thúc bất kỳ dòng lệnh dở nào đang nằm trong parser STM32,
         * sau đó xóa dữ liệu trả về của dòng rác trước khi gửi AT thật.
         */


        private async Task<string>
    ProbeStm32Async()
        {
            Exception lastError = null;

            /*
             * Retry chỉ để xử lý trường hợp USB-RS485
             * vừa mở cổng chưa ổn định.
             *
             * Sau khi sửa firmware, ERR:UNKNOWN không còn
             * được coi là lỗi có thể bỏ qua.
             */
            for (int attempt = 1;
                 attempt <= 3;
                 attempt++)
            {
                try
                {
                    return await SendSimpleCommandAsync(
                        "AT",
                        AtTimeoutMs,
                        false);
                }
                catch (TimeoutException ex)
                {
                    lastError = ex;

                    if (attempt >= 3)
                    {
                        throw;
                    }

                    Log(
                        "STM32 chưa phản hồi, thử lại lần " +
                        (attempt + 1) +
                        "...");

                    await Task.Delay(200);
                }
            }

            throw new InvalidOperationException(
                "Không kết nối được STM32.",
                lastError);
        }


        private async void BtnConnect_Click(
    object sender,
    EventArgs e)
        {
            /*
             * Disconnect.
             */
            if (serial.IsOpen)
            {
                CancelPendingOperations(
                    "COM đã bị ngắt.");

                stm32Ready = false;
                rxBuffer.Clear();

                try
                {
                    serial.DiscardInBuffer();
                    serial.DiscardOutBuffer();
                }
                catch
                {
                }

                try
                {
                    serial.Close();
                }
                catch
                {
                }

                btnConnect.Text = "Connect";

                Log("Disconnected.");

                UpdateConnectionControls();
                return;
            }

            if (string.IsNullOrWhiteSpace(
                    cmbPort.Text))
            {
                MessageBox.Show(
                    "Chưa chọn COM port.",
                    "Connection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            SetBusy(true);

            try
            {
                stm32Ready = false;
                rxBuffer.Clear();

                pendingSimpleCommand = null;
                pendingSensorTest = null;
                pendingGetConfig = null;
                pendingGetSensors = null;

                serial.PortName = cmbPort.Text;
                serial.BaudRate = FixedBaudRate;
                serial.DataBits = 8;
                serial.Parity = Parity.None;
                serial.StopBits = StopBits.One;
                serial.Handshake = Handshake.None;

                serial.DtrEnable = false;
                serial.RtsEnable = false;

                serial.Encoding = Encoding.ASCII;

                /*
                 * Protocol lệnh chỉ dùng LF.
                 */
                serial.NewLine = "\n";

                serial.ReadTimeout = 500;
                serial.WriteTimeout = 1000;

                serial.Open();

                try
                {
                    serial.DiscardInBuffer();
                    serial.DiscardOutBuffer();
                }
                catch
                {
                }

                btnConnect.Text = "Disconnect";

                Log(
                    "Connected to " +
                    serial.PortName +
                    " - 9600 8N1.");

                /*
                 * Chờ driver USB-RS485 ổn định.
                 */
                await Task.Delay(300);

                string response =
                    await ProbeStm32Async();

                if (!response.StartsWith(
                        "OK",
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "STM32 phản hồi không hợp lệ: " +
                        response);
                }

                stm32Ready = true;

                Log(
                    "STM32 ready - config mode hoạt động.");
            }
            catch (Exception ex)
            {
                stm32Ready = false;
                rxBuffer.Clear();

                CancelPendingOperations(
                    "Kết nối thất bại.");

                try
                {
                    if (serial.IsOpen)
                    {
                        serial.Close();
                    }
                }
                catch
                {
                }

                btnConnect.Text = "Connect";

                MessageBox.Show(
                    ex.Message,
                    "Connect Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void BtnRefresh_Click(
            object sender,
            EventArgs e)
        {
            RefreshComPorts();
            Log("Refresh COM ports.");
        }

        /* =====================================================
         * MAIN COMMAND BUTTONS
         * ===================================================== */

        private async void BtnTestAT_Click(
            object sender,
            EventArgs e)
        {
            if (!serial.IsOpen)
            {
                return;
            }

            SetBusy(true);

            try
            {
                await SendSimpleCommandAsync(
                    "AT",
                    AtTimeoutMs,
                    false);

                stm32Ready = true;

                Log("Test AT thành công.");
            }
            catch (Exception ex)
            {
                stm32Ready = false;

                MessageBox.Show(
                    ex.Message,
                    "AT Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void BtnGetConfig_Click(
            object sender,
            EventArgs e)
        {
            if (!CheckDeviceReady())
            {
                return;
            }

            SetBusy(true);

            try
            {
                /*
                 * Chỉ lấy cấu hình LoRa.
                 * Không thay đổi bảng Sensor.
                 */
                await GetLoRaConfigAsync();

                Log("Đã nhận cấu hình LoRa.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Get LoRa Config Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void BtnGetSensor_Click(
            object sender,
            EventArgs e)
        {
            if (!CheckDeviceReady())
            {
                return;
            }

            SetBusy(true);

            try
            {
                /*
                 * Chỉ lấy danh sách cảm biến.
                 * Không đọc lại cấu hình LoRa.
                 */
                await GetSensorsAsync();

                Log(
                    "Đã nhận " +
                    dgvSensorList.Rows.Count +
                    " sensor đang Enable.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Get Sensor Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        /*
         * Apply cả LoRa và Sensor.
         *
         * Mỗi command phải nhận OK mới gửi command sau.
         */
        private async void BtnApply_Click(
    object sender,
    EventArgs e)
        {
            if (!CheckDeviceReady())
            {
                return;
            }

            SetBusy(true);

            try
            {
                await ApplyLoRaConfigAsync();

                Log(
                    "Cấu hình LoRa đã được cập nhật vào RAM STM32. " +
                    "Nhấn Save Flash để lưu vĩnh viễn.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Apply LoRa Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void BtnSave_Click(
    object sender,
    EventArgs e)
        {
            if (!CheckDeviceReady())
            {
                return;
            }

            SetBusy(true);

            try
            {
                /*
                 * Không gửi lại LoRa.
                 * Không gửi lại Sensor.
                 *
                 * Chỉ lưu cấu hình hiện đang nằm trong RAM STM32
                 * xuống Flash.
                 */
                string response =
                    await SendSimpleCommandAsync(
                        "AT+SAVE",
                        10000,
                        true);

                Log(
                    "Save Flash thành công: " +
                    response);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Save Flash Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }
        private void BtnGetInput_Click(
            object sender,
            EventArgs e)
        {
            if (!CheckDeviceReady())
            {
                return;
            }

            /*
             * GETIN có thể trả DATA thay vì OK,
             * nên chỉ gửi command.
             */
            SendRawCommand("AT+GETIN");
        }

        /* =====================================================
         * APPLY LORA
         * ===================================================== */

        private async Task ApplyLoRaConfigAsync()
        {
            UpdateRoleByNodeId();

            int nodeId =
                (int)numNodeId.Value;

            int destinationId =
                (int)numDestId.Value;

            int frequency =
                GetSelectedIntValue(
                    cmbFreq);

            int bandwidth =
                GetSelectedIntValue(
                    cmbBw);

            int spreadingFactor =
                GetSelectedIntValue(
                    cmbSf);

            int codingRate =
                GetSelectedIntValue(
                    cmbCr);

            int txPower =
                (int)numPower.Value;

            string command =
                string.Format(
                    "AT+SETLORA={0},{1},{2},{3},{4},{5},{6}",
                    nodeId,
                    destinationId,
                    frequency,
                    bandwidth,
                    spreadingFactor,
                    codingRate,
                    txPower);

            string response =
                await SendSimpleCommandWithRetryAsync(
                    command,
                    4000,
                    3);

            Log(
                "Apply LoRa thành công: " +
                response);
        }
        /* =====================================================
         * APPLY SENSOR
         * ===================================================== */

        private async Task ApplySensorConfigAsync()
        {
            int sensorCount =
                dgvSensorList.Rows.Count;

            if (sensorCount > MaxSensorCount)
            {
                throw new InvalidOperationException(
                    "Chỉ hỗ trợ tối đa 8 cảm biến.");
            }

            HashSet<int> usedSlaveIds =
                new HashSet<int>();

            /*
             * Chỉ gửi những sensor đang có trong bảng.
             */
            for (int index = 0;
                 index < sensorCount;
                 index++)
            {
                DataGridViewRow row =
                    dgvSensorList.Rows[index];

                SensorConfigItem sensor =
                    ReadSensorFromRow(row);

                if (!usedSlaveIds.Add(sensor.SlaveId))
                {
                    throw new InvalidOperationException(
                        "Slave ID " +
                        sensor.SlaveId.ToString("D2") +
                        " đang bị trùng.");
                }

                string command =
                    string.Format(
                        "AT+SENSOR={0},1,{1},{2},0x{3:X4},{4}",
                        index,
                        sensor.SlaveId,
                        sensor.FunctionCode,
                        sensor.StartRegister,
                        sensor.RegisterLength);

                await SendSimpleCommandAsync(
                    command,
                    3000,
                    true);

                await Task.Delay(200);
            }

            /*
             * Một lệnh duy nhất để tắt toàn bộ slot còn dư.
             */
            await SendSimpleCommandAsync(
                "AT+SENSORCOUNT=" + sensorCount,
                3000,
                true);

            await Task.Delay(200);
        }

        /* =====================================================
         * ADD / UPDATE / REMOVE SENSOR
         * ===================================================== */

        private async void BtnSensorAdd_Click(
    object sender,
    EventArgs e)
        {
            if (!CheckDeviceReady())
            {
                return;
            }

            SensorConfigItem sensor;
            int newIndex;

            try
            {
                if (dgvSensorList.Rows.Count >=
                    MaxSensorCount)
                {
                    throw new InvalidOperationException(
                        "Chỉ hỗ trợ tối đa 8 cảm biến.");
                }

                sensor =
                    ReadSensorFromInputs();

                if (SensorSlaveExists(
                        sensor.SlaveId,
                        -1))
                {
                    throw new InvalidOperationException(
                        "Slave ID " +
                        sensor.SlaveId.ToString("D2") +
                        " đã có trong bảng.");
                }

                /*
                 * Index mới chính là số dòng hiện tại,
                 * trước khi thêm dòng.
                 */
                newIndex =
                    dgvSensorList.Rows.Count;
                sensor.Index =
                   newIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Add Sensor Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            SetBusy(true);

            try
            {
                /*
                 * Gửi đúng một sensor mới xuống RAM STM32.
                 */
                string command =
                    string.Format(
                        "AT+SENSOR={0},1,{1},{2},0x{3:X4},{4}",
                        newIndex,
                        sensor.SlaveId,
                        sensor.FunctionCode,
                        sensor.StartRegister,
                        sensor.RegisterLength);

                await SendSimpleCommandAsync(
                    command,
                    4000,
                    true);

                /*
                 * Chỉ thêm lên bảng sau khi STM32 đã trả OK.
                 */
                AddSensorToGrid(sensor);
                SortSensorGridBySlaveId();

                dgvSensorList.ClearSelection();

                Log(
                    "Đã thêm sensor vào RAM STM32: " +
                    "IDX=" +
                    newIndex +
                    " SID=" +
                    sensor.SlaveId.ToString("D2") +
                    " FC=" +
                    sensor.FunctionCode.ToString("D2") +
                    " REG=0x" +
                    sensor.StartRegister.ToString("X4") +
                    " CNT=" +
                    sensor.RegisterLength.ToString("D4") +
                    ". Nhấn Save Flash để lưu.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Add Sensor Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }
        private async void BtnSensorRemove_Click(
    object sender,
    EventArgs e)
        {
            if (!CheckDeviceReady())
            {
                return;
            }

            if (dgvSensorList.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Hãy chọn một sensor trong bảng.",
                    "Remove Sensor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DataGridViewRow selectedRow =
                dgvSensorList.SelectedRows[0];

            SensorConfigItem selectedSensor;

            try
            {
                /*
                 * selectedSensor.Index là slot thật trong STM32.
                 * Không nhất thiết giống selectedRow.Index
                 * vì bảng đang được sort theo Slave ID.
                 */
                selectedSensor =
                    ReadSensorFromRow(selectedRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Remove Sensor Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult confirm =
                MessageBox.Show(
                    "Xóa sensor này khỏi STM32?\r\n\r\n" +
                    "Slave ID: " +
                    selectedSensor.SlaveId.ToString("D2") +
                    "\r\n" +
                    "STM32 Index: " +
                    selectedSensor.Index +
                    "\r\n\r\n" +
                    "Sau khi xóa, nhấn Save Flash để lưu vĩnh viễn.",
                    "Confirm Remove Sensor",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            SetBusy(true);

            try
            {
                int deletedStm32Index =
                    selectedSensor.Index;

                /*
                 * Chỉ gửi đúng một command xuống STM32.
                 */
                string response =
                    await SendSimpleCommandAsync(
                        "AT+DELSENSOR=" +
                        deletedStm32Index,
                        4000,
                        true);

                /*
                 * STM32 đã trả OK thì mới xóa khỏi bảng.
                 */
                dgvSensorList.Rows.Remove(
                    selectedRow);

                /*
                 * Firmware đã dịch các slot phía sau lên một vị trí.
                 * Cập nhật lại IDX thật đang lưu trong row.Tag.
                 */
                foreach (DataGridViewRow row
                         in dgvSensorList.Rows)
                {
                    if (row.Tag == null)
                    {
                        continue;
                    }

                    int currentIndex =
                        Convert.ToInt32(row.Tag);

                    if (currentIndex >
                        deletedStm32Index)
                    {
                        row.Tag =
                            currentIndex - 1;
                    }
                }

                SortSensorGridBySlaveId();

                Log(
                    "Đã xóa sensor khỏi RAM STM32: " +
                    "SID=" +
                    selectedSensor.SlaveId.ToString("D2") +
                    " IDX=" +
                    deletedStm32Index +
                    ". Response: " +
                    response +
                    ". Nhấn Save Flash để lưu.");

                UpdateConnectionControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Remove Sensor Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                /*
                 * Đồng bộ lại bảng theo dữ liệu thật trong STM32.
                 */
                try
                {
                    await GetSensorsAsync();
                }
                catch
                {
                }
            }
            finally
            {
                SetBusy(false);
            }
        }
        private async Task<string>
    SendSimpleCommandWithRetryAsync(
        string command,
        int timeoutMs,
        int retryCount)
        {
            Exception lastError = null;

            for (int attempt = 1;
                 attempt <= retryCount;
                 attempt++)
            {
                try
                {
                    return await SendSimpleCommandAsync(
                        command,
                        timeoutMs,
                        true);
                }
                catch (TimeoutException ex)
                {
                    lastError = ex;

                    Log(
                        "Không nhận phản hồi " +
                        command +
                        ", thử lại " +
                        attempt +
                        "/" +
                        retryCount +
                        ".");

                    if (attempt >= retryCount)
                    {
                        break;
                    }

                    /*
                     * Chờ STM32 và bộ chuyển USB-RS485 ổn định.
                     */
                    await Task.Delay(500);
                }
            }

            throw new TimeoutException(
                "Không nhận được phản hồi sau " +
                retryCount +
                " lần:\r\n" +
                command,
                lastError);
        }
        private void DgvSensorList_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                BtnSensorRemove_Click(
                    sender,
                    EventArgs.Empty);

                e.Handled = true;
            }
        }

        private void DgvSensorList_SelectionChanged(
    object sender,
    EventArgs e)
        {
            if (dgvSensorList.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow row =
                        dgvSensorList.SelectedRows[0];

                    SensorConfigItem sensor =
                        ReadSensorFromRow(row);

                    /*
                     * Nạp cấu hình sensor đang chọn.
                     */
                    SelectComboByValue(
                        cmbSensorSlave,
                        sensor.SlaveId);

                    SelectComboByValue(
                        cmbSensorFunction,
                        sensor.FunctionCode);

                    cmbStartRegister.Text =
                        "0x" +
                        sensor.StartRegister.ToString("X4");

                    SelectComboByValue(
                        cmbRegisterLength,
                        sensor.RegisterLength);

                    /*
                     * Mặc định ID mới bằng ID hiện tại.
                     * Người dùng sẽ chọn ID khác.
                     */
                    SelectComboByValue(
                        cmbChangeSlaveId,
                        sensor.SlaveId);
                }
                catch (Exception ex)
                {
                    Log(
                        "Không đọc được sensor đang chọn: " +
                        ex.Message);
                }
            }

            UpdateConnectionControls();
        }

        private void DgvSensorList_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            SensorConfigItem sensor =
                ReadSensorFromRow(
                    dgvSensorList.Rows[e.RowIndex]);

            SelectComboByValue(
                cmbSensorSlave,
                sensor.SlaveId);

            SelectComboByValue(
                cmbSensorFunction,
                sensor.FunctionCode);

            cmbStartRegister.Text =
                "0x" +
                sensor.StartRegister.ToString("X4");

            SelectComboByValue(
                cmbRegisterLength,
                sensor.RegisterLength);
        }

        private async void BtnChangeSlaveId_Click(
            object sender,
            EventArgs e)
        {
            if (!CheckDeviceReady())
            {
                return;
            }

            if (dgvSensorList.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Hãy chọn cảm biến cần đổi Slave ID.",
                    "Change Slave ID",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DataGridViewRow selectedRow =
                dgvSensorList.SelectedRows[0];

            int selectedIndex =
                selectedRow.Index;

            SensorConfigItem sensor;

            int oldSlaveId;
            int newSlaveId;

            try
            {
                sensor =
                    ReadSensorFromRow(selectedRow);

                oldSlaveId =
                    sensor.SlaveId;

                newSlaveId =
                    GetSelectedIntValue(
                        cmbChangeSlaveId);

                if (oldSlaveId == newSlaveId)
                {
                    throw new InvalidOperationException(
                        "Slave ID mới đang giống Slave ID hiện tại.");
                }

                /*
                 * Không cho đổi sang ID đã tồn tại
                 * trong một dòng sensor khác.
                 */
                if (SensorSlaveExists(
                        newSlaveId,
                        selectedIndex))
                {
                    throw new InvalidOperationException(
                        "Slave ID " +
                        newSlaveId.ToString("D2") +
                        " đã được cảm biến khác sử dụng.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Change Slave ID",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult confirm =
                MessageBox.Show(
                    "Sắp đổi Slave ID thật bên trong cảm biến.\r\n\r\n" +
                    "ID hiện tại: " +
                    oldSlaveId.ToString("D2") +
                    "\r\n" +
                    "ID mới: " +
                    newSlaveId.ToString("D2") +
                    "\r\n\r\n" +
                    "Chỉ được nối một cảm biến trên bus " +
                    "trong lúc đổi địa chỉ.\r\n\r\n" +
                    "Tiếp tục?",
                    "Confirm Change Slave ID",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            SetBusy(true);

            bool physicalIdChanged = false;

            try
            {
                string command =
                    string.Format(
                        "AT+CHANGESID={0},{1}",
                        oldSlaveId,
                        newSlaveId);

                /*
                 * Firmware sẽ:
                 * 1. Gửi OLD_ID 10 NEW_ID CRC
                 * 2. Kiểm tra cảm biến bằng ID mới
                 * 3. Trả OK hoặc ERR
                 */
                string response =
                    await SendSimpleCommandAsync(
                        command,
                        8000,
                        true);

                physicalIdChanged = true;

                Log(
                    "Đã đổi ID thật của cảm biến: " +
                    oldSlaveId.ToString("D2") +
                    " -> " +
                    newSlaveId.ToString("D2") +
                    ". Response: " +
                    response);

                /*
                 * Cập nhật ID trong dòng cấu hình.
                 */
                sensor.SlaveId =
                    newSlaveId;

                WriteSensorToRow(
                    selectedRow,
                    sensor);

                SelectComboByValue(
                    cmbSensorSlave,
                    newSlaveId);

                SelectComboByValue(
                    cmbChangeSlaveId,
                    newSlaveId);

                /*
                 * Ghi lại danh sách sensor vào RAM STM32.
                 */
                await Task.Delay(200);

                await ApplySensorConfigAsync();

                await Task.Delay(200);

                /*
                 * Lưu cấu hình STM32 xuống Flash.
                 */
                string saveResponse =
                    await SendSimpleCommandAsync(
                        "AT+SAVE",
                        4000,
                        true);

                await Task.Delay(200);


                Log(
                    "Change Slave ID + Save Flash thành công: " +
                    saveResponse);

                MessageBox.Show(
                    "Đổi Slave ID thành công.\r\n\r\n" +
                    "ID cũ: " +
                    oldSlaveId.ToString("D2") +
                    "\r\n" +
                    "ID mới: " +
                    newSlaveId.ToString("D2"),
                    "Change Slave ID",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string message;

                if (physicalIdChanged)
                {
                    message =
                        "ID thật của cảm biến có thể đã được đổi, " +
                        "nhưng bước cập nhật/lưu cấu hình STM32 bị lỗi.\r\n\r\n" +
                        ex.Message;
                }
                else
                {
                    message =
                        "Không đổi được Slave ID của cảm biến.\r\n\r\n" +
                        ex.Message;
                }

                MessageBox.Show(
                    message,
                    "Change Slave ID Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }
        /* =====================================================
         * COMMAND WAITING
         * ===================================================== */

        private async Task<string>
            SendSimpleCommandAsync(
                string command,
                int timeoutMs,
                bool requireReady)
        {
            if (!serial.IsOpen)
            {
                throw new InvalidOperationException(
                    "COM port đã bị ngắt.");
            }

            if (requireReady &&
                !stm32Ready)
            {
                throw new InvalidOperationException(
                    "STM32 chưa sẵn sàng.");
            }

            await commandGate.WaitAsync();

            TaskCompletionSource<string> completion =
                new TaskCompletionSource<string>(
                    TaskCreationOptions
                        .RunContinuationsAsynchronously);

            pendingSimpleCommand = completion;

            try
            {
                SendRawCommand(command);

                Task timeoutTask =
                    Task.Delay(timeoutMs);

                Task completed =
                    await Task.WhenAny(
                        completion.Task,
                        timeoutTask);

                if (completed != completion.Task)
                {
                    throw new TimeoutException(
                        "Timeout khi chờ phản hồi lệnh:\r\n" +
                        command);
                }

                return await completion.Task;
            }
            finally
            {
                if (ReferenceEquals(
                        pendingSimpleCommand,
                        completion))
                {
                    pendingSimpleCommand = null;
                }

                commandGate.Release();
            }
        }

        private async Task<string>
            TestPhysicalSensorAsync(
                int slaveId,
                int functionCode,
                int startRegister,
                int registerLength)
        {
            if (!serial.IsOpen ||
                !stm32Ready)
            {
                throw new InvalidOperationException(
                    "STM32 chưa sẵn sàng.");
            }

            await commandGate.WaitAsync();

            TaskCompletionSource<string> completion =
                new TaskCompletionSource<string>(
                    TaskCreationOptions
                        .RunContinuationsAsynchronously);

            pendingSensorTest = completion;

            try
            {
                string command =
                    string.Format(
                        "AT+TESTSENSOR={0},{1},0x{2:X4},{3}",
                        slaveId,
                        functionCode,
                        startRegister,
                        registerLength);

                SendRawCommand(command);

                Log(
                    "Đang test: SID=" +
                    slaveId.ToString("D2") +
                    " FC=" +
                    functionCode.ToString("D2") +
                    " REG=0x" +
                    startRegister.ToString("X4") +
                    " CNT=" +
                    registerLength.ToString("D4"));

                Task timeoutTask =
                    Task.Delay(SensorTimeoutMs);

                Task completed =
                    await Task.WhenAny(
                        completion.Task,
                        timeoutTask);

                if (completed != completion.Task)
                {
                    throw new TimeoutException(
                        "Cảm biến không phản hồi trong 10 giây.\r\n\r\n" +
                        "Kiểm tra nguồn, A/B, GND, Slave ID, " +
                        "Function Code, thanh ghi và baudrate 9600.");
                }

                return await completion.Task;
            }
            finally
            {
                if (ReferenceEquals(
                        pendingSensorTest,
                        completion))
                {
                    pendingSensorTest = null;
                }

                commandGate.Release();
            }
        }

        private async Task GetLoRaConfigAsync()
        {
            if (!serial.IsOpen ||
                !stm32Ready)
            {
                throw new InvalidOperationException(
                    "STM32 chưa sẵn sàng.");
            }

            await commandGate.WaitAsync();

            TaskCompletionSource<bool> completion =
                new TaskCompletionSource<bool>(
                    TaskCreationOptions
                        .RunContinuationsAsynchronously);

            pendingGetConfig = completion;

            try
            {
                SendRawCommand("AT+GETCFG");

                Task timeoutTask =
                    Task.Delay(GetConfigTimeoutMs);

                Task completed =
                    await Task.WhenAny(
                        completion.Task,
                        timeoutTask);

                if (completed != completion.Task)
                {
                    throw new TimeoutException(
                        "Không nhận được END từ AT+GETCFG.");
                }

                await completion.Task;
            }
            finally
            {
                if (ReferenceEquals(
                        pendingGetConfig,
                        completion))
                {
                    pendingGetConfig = null;
                }

                commandGate.Release();
            }
        }
        private async Task GetSensorsAsync()
        {
            if (!serial.IsOpen ||
                !stm32Ready)
            {
                throw new InvalidOperationException(
                    "STM32 chưa sẵn sàng.");
            }

            await commandGate.WaitAsync();

            TaskCompletionSource<bool> completion =
                new TaskCompletionSource<bool>(
                    TaskCreationOptions
                        .RunContinuationsAsynchronously);

            pendingGetSensors = completion;

            /*
             * Xóa buffer tạm trước khi nhận danh sách mới.
             * Chưa xóa bảng ngay để tránh bảng bị mất nếu timeout.
             */
            receivedConfigBuffer.Clear();

            try
            {
                SendRawCommand("AT+GETSENSOR");

                Task timeoutTask =
                    Task.Delay(GetSensorsTimeoutMs);

                Task completed =
                    await Task.WhenAny(
                        completion.Task,
                        timeoutTask);

                if (completed != completion.Task)
                {
                    throw new TimeoutException(
                        "Không nhận được END từ AT+GETSENSOR.");
                }

                await completion.Task;

                /*
                 * Chỉ cập nhật bảng sau khi đã nhận đủ SENSOR...END.
                 */
                ApplyReceivedConfigToGrid();
            }
            finally
            {
                if (ReferenceEquals(
                        pendingGetSensors,
                        completion))
                {
                    pendingGetSensors = null;
                }

                commandGate.Release();
            }
        }
        private void SendRawCommand(
            string command)
        {
            if (!serial.IsOpen)
            {
                throw new InvalidOperationException(
                    "COM port đã bị ngắt.");
            }

            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException(
                    "Command đang trống.",
                    nameof(command));
            }

            /*
             * Xóa CR/LF nếu người gọi vô tình truyền kèm.
             */
            string cleanCommand =
                command.TrimEnd('\r', '\n');

            /*
             * Gửi toàn bộ command và một LF trong cùng một lần Write.
             *
             * Frame thực tế:
             * AT 0A
             *
             * Không dùng WriteLine() để tránh phụ thuộc NewLine.
             */
            string frame =
                cleanCommand + "\n";

            byte[] data =
                Encoding.ASCII.GetBytes(frame);

            serial.Write(
                data,
                0,
                data.Length);

            Log("TX: " + cleanCommand);
        }

        /* =====================================================
         * SERIAL RECEIVE
         * ===================================================== */

        private void Serial_DataReceived(
            object sender,
            SerialDataReceivedEventArgs e)
        {
            try
            {
                string data =
                    serial.ReadExisting();

                BeginInvoke(
                    new Action(
                        delegate
                        {
                            rxBuffer.Append(data);

                            if (rxBuffer.Length >
                                MaxRxBufferSize)
                            {
                                rxBuffer.Clear();

                                Log(
                                    "RX buffer overflow, " +
                                    "đã xóa dữ liệu.");
                            }
                            else
                            {
                                ProcessReceivedBuffer();
                            }
                        }));
            }
            catch (Exception ex)
            {
                try
                {
                    BeginInvoke(
                        new Action(
                            delegate
                            {
                                Log(
                                    "Receive error: " +
                                    ex.Message);
                            }));
                }
                catch
                {
                }
            }
        }

        private void ProcessReceivedBuffer()
        {
            string all =
                rxBuffer.ToString();

            int consumed = 0;

            while (true)
            {
                int indexR =
                    all.IndexOf('\r', consumed);

                int indexN =
                    all.IndexOf('\n', consumed);

                int lineEnd;

                if (indexR < 0 &&
                    indexN < 0)
                {
                    break;
                }

                if (indexR < 0)
                {
                    lineEnd = indexN;
                }
                else if (indexN < 0)
                {
                    lineEnd = indexR;
                }
                else
                {
                    lineEnd =
                        Math.Min(indexR, indexN);
                }

                string line =
                    all.Substring(
                        consumed,
                        lineEnd - consumed)
                       .Trim();

                int next =
                    lineEnd + 1;

                if (next < all.Length)
                {
                    char first =
                        all[lineEnd];

                    char second =
                        all[next];

                    if ((first == '\r' &&
                         second == '\n') ||
                        (first == '\n' &&
                         second == '\r'))
                    {
                        next++;
                    }
                }

                consumed = next;

                if (line.Length > 0)
                {
                    Log("RX: " + line);
                    ParseLine(line);
                }
            }

            if (consumed > 0)
            {
                rxBuffer.Remove(
                    0,
                    consumed);
            }
        }

        private void ParseLine(
            string line)
        {
            if (line.StartsWith(
                    "SENSORTEST OK",
                    StringComparison.OrdinalIgnoreCase))
            {
                if (pendingSensorTest != null)
                {
                    pendingSensorTest
                        .TrySetResult(line);
                }

                return;
            }

            if (line.StartsWith(
                    "SENSORTEST ERR",
                    StringComparison.OrdinalIgnoreCase))
            {
                if (pendingSensorTest != null)
                {
                    pendingSensorTest
                        .TrySetException(
                            new InvalidOperationException(
                                line));
                }

                return;
            }

            if (line.StartsWith(
                    "READY CONFIG",
                    StringComparison.OrdinalIgnoreCase))
            {
                stm32Ready = true;
                UpdateConnectionControls();
                return;
            }

            if (line.StartsWith(
        "SENSOR ",
        StringComparison.OrdinalIgnoreCase))
            {
                /*
                 * Chỉ nhận SENSOR khi đang thực hiện GETSENSOR.
                 */
                if (pendingGetSensors != null)
                {
                    ParseSensorConfigLine(line);
                }

                return;
            }

            if (line.StartsWith(
                    "CFG",
                    StringComparison.OrdinalIgnoreCase))
            {
                /*
                 * Chỉ nhận CFG khi đang thực hiện GETCFG.
                 */
                if (pendingGetConfig != null)
                {
                    ParseConfig(line);
                }

                return;
            }

            if (line.Equals(
                    "END",
                    StringComparison.OrdinalIgnoreCase))
            {
                /*
                 * Mỗi thời điểm chỉ có một command được chạy
                 * nhờ commandGate.
                 */
                if (pendingGetSensors != null)
                {
                    pendingGetSensors
                        .TrySetResult(true);
                }
                else if (pendingGetConfig != null)
                {
                    pendingGetConfig
                        .TrySetResult(true);
                }

                return;
            }

            if (line.StartsWith(
                    "OK",
                    StringComparison.OrdinalIgnoreCase))
            {
                if (pendingSimpleCommand != null)
                {
                    pendingSimpleCommand
                        .TrySetResult(line);
                }

                return;
            }

            if (line.StartsWith(
                    "ERR",
                    StringComparison.OrdinalIgnoreCase))
            {
                Exception error =
                    new InvalidOperationException(line);

                if (pendingSensorTest != null)
                {
                    pendingSensorTest
                        .TrySetException(error);
                }
                else if (pendingGetSensors != null)
                {
                    pendingGetSensors
                        .TrySetException(error);
                }
                else if (pendingGetConfig != null)
                {
                    pendingGetConfig
                        .TrySetException(error);
                }
                else if (pendingSimpleCommand != null)
                {
                    pendingSimpleCommand
                        .TrySetException(error);
                }

                return;
            }

            Log("DATA: " + line);
        }

        /* =====================================================
         * GET CONFIG PARSING
         * ===================================================== */

        private void ParseSensorConfigLine(
            string line)
        {
            if (pendingGetSensors == null)
            {
                return;
            }

            try
            {
                Dictionary<string, string> values =
                    ParseKeyValues(line);

                if (!values.ContainsKey("EN") ||
                    !values.ContainsKey("SID") ||
                    !values.ContainsKey("FC") ||
                    !values.ContainsKey("REG") ||
                    !values.ContainsKey("CNT"))
                {
                    throw new FormatException(
                        "Dòng SENSOR thiếu trường.");
                }

                int enabled =
                    Convert.ToInt32(values["EN"]);

                if (enabled == 0)
                {
                    return;
                }

                SensorConfigItem sensor =
                    new SensorConfigItem();

                sensor.Index =
                    values.ContainsKey("IDX")
                        ? Convert.ToInt32(values["IDX"])
                        : receivedConfigBuffer.Count;

                sensor.SlaveId =
                    Convert.ToInt32(values["SID"]);

                sensor.FunctionCode =
                    Convert.ToInt32(values["FC"]);

                sensor.StartRegister =
                    ParseRegisterAddress(
                        values["REG"]);

                sensor.RegisterLength =
                    Convert.ToInt32(
                        values["CNT"]);

                ValidateSensorValues(sensor);

                foreach (SensorConfigItem existing
                         in receivedConfigBuffer)
                {
                    if (existing.SlaveId ==
                        sensor.SlaveId)
                    {
                        throw new InvalidOperationException(
                            "Slave ID trùng trong cấu hình STM32: " +
                            sensor.SlaveId.ToString("D2"));
                    }
                }

                receivedConfigBuffer.Add(sensor);
            }
            catch (Exception ex)
            {
                Log(
                    "Parse SENSOR fail: " +
                    ex.Message);
            }
        }

        private void ApplyReceivedConfigToGrid()
        {
            receivedConfigBuffer.Sort(
                delegate (
                    SensorConfigItem left,
                    SensorConfigItem right)
                {
                    int compare =
                        left.SlaveId.CompareTo(
                            right.SlaveId);

                    if (compare != 0)
                    {
                        return compare;
                    }

                    return left.Index.CompareTo(
                        right.Index);
                });

            dgvSensorList.Rows.Clear();

            foreach (SensorConfigItem sensor
                     in receivedConfigBuffer)
            {
                AddSensorToGrid(sensor);
            }

            dgvSensorList.ClearSelection();

            UpdateConnectionControls();
        }
        private Dictionary<string, string>
            ParseKeyValues(
                string line)
        {
            Dictionary<string, string> result =
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase);

            string[] parts =
                line.Split(
                    new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                int equalIndex =
                    part.IndexOf('=');

                if (equalIndex <= 0 ||
                    equalIndex >=
                    part.Length - 1)
                {
                    continue;
                }

                result[
                    part.Substring(0, equalIndex)] =
                    part.Substring(equalIndex + 1);
            }

            return result;
        }

        private void ParseConfig(
            string line)
        {
            try
            {
                string[] parts =
                    line.Split(
                        new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in parts)
                {
                    if (item.StartsWith("ID="))
                    {
                        SetNumericValue(
                            numNodeId,
                            item.Substring(3));
                    }
                    else if (item.StartsWith("NODE="))
                    {
                        SetNumericValue(
                            numNodeId,
                            item.Substring(5));
                    }
                    else if (item.StartsWith("DST="))
                    {
                        SetNumericValue(
                            numDestId,
                            item.Substring(4));
                    }
                    else if (item.StartsWith("ROLE="))
                    {
                        SelectComboByValue(
                            cmbRole,
                            item.Substring(5));
                    }
                    else if (item.StartsWith("FREQ="))
                    {
                        SelectComboByValue(
                            cmbFreq,
                            int.Parse(
                                item.Substring(5)));
                    }
                    else if (item.StartsWith("BW="))
                    {
                        SelectComboByValue(
                            cmbBw,
                            int.Parse(
                                item.Substring(3)));
                    }
                    else if (item.StartsWith("SF="))
                    {
                        SelectComboByValue(
                            cmbSf,
                            int.Parse(
                                item.Substring(3)));
                    }
                    else if (item.StartsWith("CR="))
                    {
                        SelectComboByValue(
                            cmbCr,
                            int.Parse(
                                item.Substring(3)));
                    }
                    else if (item.StartsWith("PWR="))
                    {
                        SetNumericValue(
                            numPower,
                            item.Substring(4));
                    }
                }
            }
            catch (Exception ex)
            {
                Log(
                    "Parse CFG fail: " +
                    ex.Message);
            }
        }

        private void SetNumericValue(
            NumericUpDown control,
            string text)
        {
            decimal value =
                Convert.ToDecimal(text);

            if (value < control.Minimum ||
                value > control.Maximum)
            {
                return;
            }

            control.Value = value;
        }

        /* =====================================================
         * SENSOR DATA HELPERS
         * ===================================================== */

        private SensorConfigItem
            ReadSensorFromInputs()
        {
            SensorConfigItem sensor =
                new SensorConfigItem();

            sensor.SlaveId =
                GetSelectedIntValue(
                    cmbSensorSlave);

            sensor.FunctionCode =
                GetSelectedIntValue(
                    cmbSensorFunction);

            sensor.StartRegister =
                ParseRegisterAddress(
                    cmbStartRegister.Text);

            sensor.RegisterLength =
                GetSelectedIntValue(
                    cmbRegisterLength);

            ValidateSensorValues(sensor);

            return sensor;
        }

        private void SortSensorGridBySlaveId()
        {
            if (dgvSensorList.Rows.Count <= 1)
            {
                return;
            }

            dgvSensorList.Sort(
                dgvSensorList.Columns["colSlaveId"],
                System.ComponentModel
                    .ListSortDirection
                    .Ascending);

            dgvSensorList.ClearSelection();

            UpdateConnectionControls();
        }
        private SensorConfigItem ReadSensorFromRow(
    DataGridViewRow row)
        {
            SensorConfigItem sensor =
                new SensorConfigItem();

            sensor.Index =
                row.Tag != null
                    ? Convert.ToInt32(row.Tag)
                    : row.Index;

            sensor.SlaveId =
                Convert.ToInt32(
                    row.Cells["colSlaveId"].Value);

            sensor.FunctionCode =
                Convert.ToInt32(
                    row.Cells["colFunctionCode"].Value);

            sensor.StartRegister =
                ParseRegisterAddress(
                    Convert.ToString(
                        row.Cells["colStartRegister"].Value));

            sensor.RegisterLength =
                Convert.ToInt32(
                    row.Cells["colRegisterLength"].Value);

            ValidateSensorValues(sensor);

            return sensor;
        }
        private void AddSensorToGrid(
    SensorConfigItem sensor)
        {
            int rowIndex =
                dgvSensorList.Rows.Add(
                    sensor.SlaveId.ToString("D2"),
                    sensor.FunctionCode.ToString("D2"),
                    "0x" +
                        sensor.StartRegister.ToString("X4"),
                    sensor.RegisterLength.ToString("D4"));

            dgvSensorList.Rows[rowIndex].Tag =
                sensor.Index;
        }
        private void WriteSensorToRow(
            DataGridViewRow row,
            SensorConfigItem sensor)
        {
            row.Cells["colSlaveId"].Value =
                sensor.SlaveId.ToString("D2");

            row.Cells["colFunctionCode"].Value =
                sensor.FunctionCode.ToString("D2");

            row.Cells["colStartRegister"].Value =
                "0x" +
                sensor.StartRegister.ToString("X4");

            row.Cells["colRegisterLength"].Value =
                sensor.RegisterLength.ToString("D4");
        }

        private void ValidateSensorValues(
            SensorConfigItem sensor)
        {
            if (sensor.SlaveId < 1 ||
                sensor.SlaveId > MaxSensorCount)
            {
                throw new InvalidOperationException(
                    "Slave ID chỉ được từ 01 đến 08.");
            }

            if (sensor.FunctionCode != 3 &&
                sensor.FunctionCode != 4)
            {
                throw new InvalidOperationException(
                    "Function Code chỉ được 03 hoặc 04.");
            }

            if (sensor.StartRegister < 0 ||
                sensor.StartRegister > 0xFFFF)
            {
                throw new InvalidOperationException(
                    "Start Register không hợp lệ.");
            }

            if (sensor.RegisterLength < 1 ||
                sensor.RegisterLength >
                    MaxRegisterLength)
            {
                throw new InvalidOperationException(
                    "Register Length chỉ được 0001 đến 0007.");
            }
        }

        private bool SensorSlaveExists(
            int slaveId,
            int excludedRowIndex)
        {
            foreach (DataGridViewRow row
                     in dgvSensorList.Rows)
            {
                if (row.Index == excludedRowIndex)
                {
                    continue;
                }

                object value =
                    row.Cells[
                        "colSlaveId"].Value;

                if (value != null &&
                    Convert.ToInt32(value) ==
                    slaveId)
                {
                    return true;
                }
            }

            return false;
        }

        private int ParseRegisterAddress(
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new FormatException(
                    "Start Register đang trống.");
            }

            text = text.Trim();

            int value;

            if (text.StartsWith(
                    "0x",
                    StringComparison.OrdinalIgnoreCase))
            {
                value =
                    Convert.ToInt32(
                        text.Substring(2),
                        16);
            }
            else
            {
                value = Convert.ToInt32(text);
            }

            if (value < 0 ||
                value > 0xFFFF)
            {
                throw new FormatException(
                    "Start Register phải từ 0x0000 đến 0xFFFF.");
            }

            return value;
        }

        /* =====================================================
         * ROLE AND OPTION ITEMS
         * ===================================================== */

        private void numNodeId_ValueChanged(
            object sender,
            EventArgs e)
        {
            UpdateRoleByNodeId();
        }

        private void UpdateRoleByNodeId()
        {
            SelectComboByValue(
                cmbRole,
                numNodeId.Value == 0
                    ? "RX"
                    : "TX");

            cmbRole.Enabled = false;
        }

        private int GetSelectedIntValue(
            ComboBox comboBox)
        {
            OptionItem item =
                comboBox.SelectedItem
                as OptionItem;

            if (item == null)
            {
                throw new InvalidOperationException(
                    "Chưa chọn giá trị cho " +
                    comboBox.Name + ".");
            }

            return Convert.ToInt32(item.Value);
        }

        private string GetSelectedStringValue(
            ComboBox comboBox)
        {
            OptionItem item =
                comboBox.SelectedItem
                as OptionItem;

            if (item == null)
            {
                throw new InvalidOperationException(
                    "Chưa chọn giá trị cho " +
                    comboBox.Name + ".");
            }

            return Convert.ToString(item.Value);
        }

        private void SelectComboByValue(
            ComboBox comboBox,
            object value)
        {
            if (value == null)
            {
                return;
            }

            foreach (object rawItem
                     in comboBox.Items)
            {
                OptionItem item =
                    rawItem as OptionItem;

                if (item != null &&
                    item.Value.ToString() ==
                    value.ToString())
                {
                    comboBox.SelectedItem = item;
                    return;
                }
            }
        }

        /* =====================================================
         * CLEANUP
         * ===================================================== */

        private void CancelPendingOperations(
    string reason)
        {
            Exception error =
                new InvalidOperationException(reason);

            TaskCompletionSource<string> simple =
                pendingSimpleCommand;

            TaskCompletionSource<string> sensor =
                pendingSensorTest;

            TaskCompletionSource<bool> config =
                pendingGetConfig;

            TaskCompletionSource<bool> sensors =
                pendingGetSensors;

            pendingSimpleCommand = null;
            pendingSensorTest = null;
            pendingGetConfig = null;
            pendingGetSensors = null;

            if (simple != null)
            {
                simple.TrySetException(error);
            }

            if (sensor != null)
            {
                sensor.TrySetException(error);
            }

            if (config != null)
            {
                config.TrySetException(error);
            }

            if (sensors != null)
            {
                sensors.TrySetException(error);
            }
        }

        private void Log(string text)
        {
            txtLog.AppendText(
                DateTime.Now.ToString("HH:mm:ss") +
                "  " +
                text +
                Environment.NewLine);
        }

        protected override void OnFormClosing(
            FormClosingEventArgs e)
        {
            CancelPendingOperations(
                "Form đang đóng.");

            try
            {
                if (serial.IsOpen)
                {
                    serial.Close();
                }

                serial.Dispose();
                commandGate.Dispose();
            }
            catch
            {
            }

            base.OnFormClosing(e);
        }

        /* =====================================================
         * DESIGNER EVENT STUBS
         * ===================================================== */

        private void Form1_Load(
            object sender,
            EventArgs e)
        {
        }

        private void groupBox1_Enter(
            object sender,
            EventArgs e)
        {
        }

        private void groupBox2_Enter(
            object sender,
            EventArgs e)
        {
        }

        private void groupBox3_Enter(
            object sender,
            EventArgs e)
        {
        }

        private void groupBox5_Enter(
            object sender,
            EventArgs e)
        {
        }

        private void label1_Click(
            object sender,
            EventArgs e)
        {
        }

        private void label3_Click(
            object sender,
            EventArgs e)
        {
        }

        private void label4_Click(
            object sender,
            EventArgs e)
        {
        }

        private void label10_Click(
            object sender,
            EventArgs e)
        {
        }

        private void label14_Click(
            object sender,
            EventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(
            object sender,
            EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
        }

        private void btnApply_Click_1(
            object sender,
            EventArgs e)
        {
        }

        private void btnSave_Click_1(
            object sender,
            EventArgs e)
        {
        }

        private void button1_Click(
            object sender,
            EventArgs e)
        {
        }

        private void label14_Click_1(object sender, EventArgs e)
        {

        }

        private void cmbPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnChangeSlaveId_Click_1(object sender, EventArgs e)
        {

        }
    }

    internal sealed class SensorConfigItem
    {
        public int Index { get; set; }
        public int SlaveId { get; set; }
        public int FunctionCode { get; set; }
        public int StartRegister { get; set; }
        public int RegisterLength { get; set; }
    }

    public class OptionItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public OptionItem(
            string text,
            object value)
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