
namespace ATA_GUI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelTopBar = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonCloseWindows = new System.Windows.Forms.Button();
            this.labelLog = new System.Windows.Forms.Label();
            this.buttonKillAdb = new System.Windows.Forms.Button();
            this.buttonSyncApp = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCredits = new System.Windows.Forms.Button();
            this.buttonLogClear = new System.Windows.Forms.Button();
            this.backgroundWorkerSync = new System.ComponentModel.BackgroundWorker();
            this.groupBoxDeviceInfo = new System.Windows.Forms.GroupBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelAV = new System.Windows.Forms.Label();
            this.labelD = new System.Windows.Forms.Label();
            this.labelB = new System.Windows.Forms.Label();
            this.labelModel = new System.Windows.Forms.Label();
            this.labelManu = new System.Windows.Forms.Label();
            this.labelCA = new System.Windows.Forms.Label();
            this.labelBU = new System.Windows.Forms.Label();
            this.labelAndroidVersion = new System.Windows.Forms.Label();
            this.labelProductDevice = new System.Windows.Forms.Label();
            this.labelProductBoard = new System.Windows.Forms.Label();
            this.labelProductModel = new System.Windows.Forms.Label();
            this.labelManufacturer = new System.Windows.Forms.Label();
            this.labelCpuAbilitis = new System.Windows.Forms.Label();
            this.labelBuildUser = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSystem = new System.Windows.Forms.TabPage();
            this.groupBoxADBNet = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonDisconnectIP = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.buttonConnectToIP = new System.Windows.Forms.Button();
            this.panelSystem = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonCheckPermissions = new System.Windows.Forms.Button();
            this.buttonGrantDump = new System.Windows.Forms.Button();
            this.buttonGrantPermission = new System.Windows.Forms.Button();
            this.buttonReloadApps = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtoNonSystemApp = new System.Windows.Forms.RadioButton();
            this.radioButtonSystemApp = new System.Windows.Forms.RadioButton();
            this.checkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.checkedListBoxApp = new System.Windows.Forms.CheckedListBox();
            this.buttonUninstallApk = new System.Windows.Forms.Button();
            this.buttonApk = new System.Windows.Forms.Button();
            this.groupBoxRebootMenu = new System.Windows.Forms.GroupBox();
            this.buttonRS = new System.Windows.Forms.Button();
            this.buttonRR = new System.Windows.Forms.Button();
            this.buttonRF = new System.Windows.Forms.Button();
            this.tabPageFastboot = new System.Windows.Forms.TabPage();
            this.panelFastboot = new System.Windows.Forms.Panel();
            this.buttonBootloaderMenu = new System.Windows.Forms.Button();
            this.buttonRebootRecovery = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.labelUDT = new System.Windows.Forms.Label();
            this.labelCDT = new System.Windows.Forms.Label();
            this.labelBootloaderStatus = new System.Windows.Forms.Label();
            this.labelDataType = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonHardReset = new System.Windows.Forms.Button();
            this.buttonRebootToSystem = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxDirImg = new System.Windows.Forms.TextBox();
            this.radioButtonRom = new System.Windows.Forms.RadioButton();
            this.buttonSearchFileFastboot = new System.Windows.Forms.Button();
            this.radioButtonCache = new System.Windows.Forms.RadioButton();
            this.buttonFlashImg = new System.Windows.Forms.Button();
            this.radioButtonVendor = new System.Windows.Forms.RadioButton();
            this.pictureBoxLoading2 = new System.Windows.Forms.PictureBox();
            this.radioButtonSystem = new System.Windows.Forms.RadioButton();
            this.radioButtonBoot = new System.Windows.Forms.RadioButton();
            this.radioButtonRecovery = new System.Windows.Forms.RadioButton();
            this.radioButtonBootloader = new System.Windows.Forms.RadioButton();
            this.radioButtonRadio = new System.Windows.Forms.RadioButton();
            this.tabPageRecovery = new System.Windows.Forms.TabPage();
            this.buttonInstallZip = new System.Windows.Forms.Button();
            this.textBoxDirFile = new System.Windows.Forms.TextBox();
            this.buttonSearchFile = new System.Windows.Forms.Button();
            this.openFileDialogAPK = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogZip = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorkerZip = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerFlashImg = new System.ComponentModel.BackgroundWorker();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonDE = new System.Windows.Forms.Button();
            this.panelTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBoxDeviceInfo.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSystem.SuspendLayout();
            this.groupBoxADBNet.SuspendLayout();
            this.panelSystem.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBoxRebootMenu.SuspendLayout();
            this.tabPageFastboot.SuspendLayout();
            this.panelFastboot.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading2)).BeginInit();
            this.tabPageRecovery.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTopBar
            // 
            this.panelTopBar.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelTopBar.Controls.Add(this.pictureBox1);
            this.panelTopBar.Controls.Add(this.button1);
            this.panelTopBar.Controls.Add(this.buttonCloseWindows);
            this.panelTopBar.Location = new System.Drawing.Point(0, -3);
            this.panelTopBar.Name = "panelTopBar";
            this.panelTopBar.Size = new System.Drawing.Size(856, 39);
            this.panelTopBar.TabIndex = 0;
            this.panelTopBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelTopBar_MouseMove);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ATA_GUI.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(8, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(79, 33);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(769, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 40);
            this.button1.TabIndex = 2;
            this.button1.TabStop = false;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCloseWindows
            // 
            this.buttonCloseWindows.BackColor = System.Drawing.Color.Red;
            this.buttonCloseWindows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCloseWindows.ForeColor = System.Drawing.Color.Black;
            this.buttonCloseWindows.Image = ((System.Drawing.Image)(resources.GetObject("buttonCloseWindows.Image")));
            this.buttonCloseWindows.Location = new System.Drawing.Point(811, 0);
            this.buttonCloseWindows.Name = "buttonCloseWindows";
            this.buttonCloseWindows.Size = new System.Drawing.Size(43, 42);
            this.buttonCloseWindows.TabIndex = 1;
            this.buttonCloseWindows.TabStop = false;
            this.buttonCloseWindows.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonCloseWindows.UseVisualStyleBackColor = false;
            this.buttonCloseWindows.Click += new System.EventHandler(this.buttonCloseWindows_Click);
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(440, 366);
            this.labelLog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLog.Name = "labelLog";
            this.labelLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelLog.Size = new System.Drawing.Size(28, 13);
            this.labelLog.TabIndex = 24;
            this.labelLog.Text = "Log:";
            // 
            // buttonKillAdb
            // 
            this.buttonKillAdb.BackColor = System.Drawing.Color.White;
            this.buttonKillAdb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonKillAdb.Location = new System.Drawing.Point(323, 15);
            this.buttonKillAdb.Name = "buttonKillAdb";
            this.buttonKillAdb.Size = new System.Drawing.Size(87, 23);
            this.buttonKillAdb.TabIndex = 29;
            this.buttonKillAdb.Text = "Kill Adb";
            this.buttonKillAdb.UseVisualStyleBackColor = false;
            this.buttonKillAdb.Click += new System.EventHandler(this.buttonKillAdb_Click);
            // 
            // buttonSyncApp
            // 
            this.buttonSyncApp.BackColor = System.Drawing.Color.White;
            this.buttonSyncApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSyncApp.ForeColor = System.Drawing.Color.Black;
            this.buttonSyncApp.Location = new System.Drawing.Point(13, 15);
            this.buttonSyncApp.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSyncApp.Name = "buttonSyncApp";
            this.buttonSyncApp.Size = new System.Drawing.Size(118, 23);
            this.buttonSyncApp.TabIndex = 0;
            this.buttonSyncApp.Text = "Sync Smartphone";
            this.buttonSyncApp.UseVisualStyleBackColor = false;
            this.buttonSyncApp.Click += new System.EventHandler(this.buttonSyncApp_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonCredits);
            this.groupBox1.Controls.Add(this.buttonLogClear);
            this.groupBox1.Controls.Add(this.buttonSyncApp);
            this.groupBox1.Controls.Add(this.buttonKillAdb);
            this.groupBox1.Location = new System.Drawing.Point(12, 366);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 84);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tool Box";
            // 
            // buttonCredits
            // 
            this.buttonCredits.BackColor = System.Drawing.Color.White;
            this.buttonCredits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCredits.ForeColor = System.Drawing.Color.Black;
            this.buttonCredits.Location = new System.Drawing.Point(13, 44);
            this.buttonCredits.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCredits.Name = "buttonCredits";
            this.buttonCredits.Size = new System.Drawing.Size(118, 23);
            this.buttonCredits.TabIndex = 32;
            this.buttonCredits.Text = "About";
            this.buttonCredits.UseVisualStyleBackColor = false;
            this.buttonCredits.Click += new System.EventHandler(this.buttonCredits_Click);
            // 
            // buttonLogClear
            // 
            this.buttonLogClear.BackColor = System.Drawing.Color.White;
            this.buttonLogClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogClear.ForeColor = System.Drawing.Color.Black;
            this.buttonLogClear.Location = new System.Drawing.Point(323, 44);
            this.buttonLogClear.Name = "buttonLogClear";
            this.buttonLogClear.Size = new System.Drawing.Size(87, 23);
            this.buttonLogClear.TabIndex = 31;
            this.buttonLogClear.Text = "Clear log";
            this.buttonLogClear.UseVisualStyleBackColor = false;
            this.buttonLogClear.Click += new System.EventHandler(this.buttonLogClear_Click);
            // 
            // backgroundWorkerSync
            // 
            this.backgroundWorkerSync.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSync_DoWork);
            // 
            // groupBoxDeviceInfo
            // 
            this.groupBoxDeviceInfo.Controls.Add(this.labelStatus);
            this.groupBoxDeviceInfo.Controls.Add(this.label4);
            this.groupBoxDeviceInfo.Controls.Add(this.labelIP);
            this.groupBoxDeviceInfo.Controls.Add(this.label1);
            this.groupBoxDeviceInfo.Controls.Add(this.labelAV);
            this.groupBoxDeviceInfo.Controls.Add(this.labelD);
            this.groupBoxDeviceInfo.Controls.Add(this.labelB);
            this.groupBoxDeviceInfo.Controls.Add(this.labelModel);
            this.groupBoxDeviceInfo.Controls.Add(this.labelManu);
            this.groupBoxDeviceInfo.Controls.Add(this.labelCA);
            this.groupBoxDeviceInfo.Controls.Add(this.labelBU);
            this.groupBoxDeviceInfo.Controls.Add(this.labelAndroidVersion);
            this.groupBoxDeviceInfo.Controls.Add(this.labelProductDevice);
            this.groupBoxDeviceInfo.Controls.Add(this.labelProductBoard);
            this.groupBoxDeviceInfo.Controls.Add(this.labelProductModel);
            this.groupBoxDeviceInfo.Controls.Add(this.labelManufacturer);
            this.groupBoxDeviceInfo.Controls.Add(this.labelCpuAbilitis);
            this.groupBoxDeviceInfo.Controls.Add(this.labelBuildUser);
            this.groupBoxDeviceInfo.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDeviceInfo.Name = "groupBoxDeviceInfo";
            this.groupBoxDeviceInfo.Size = new System.Drawing.Size(257, 157);
            this.groupBoxDeviceInfo.TabIndex = 32;
            this.groupBoxDeviceInfo.TabStop = false;
            this.groupBoxDeviceInfo.Text = "Device Info";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(86, 120);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(44, 13);
            this.labelStatus.TabIndex = 17;
            this.labelStatus.Text = "EMPTY";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Connected via: ";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(86, 107);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(44, 13);
            this.labelIP.TabIndex = 15;
            this.labelIP.Text = "EMPTY";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "IP: ";
            // 
            // labelAV
            // 
            this.labelAV.AutoSize = true;
            this.labelAV.Location = new System.Drawing.Point(86, 94);
            this.labelAV.Name = "labelAV";
            this.labelAV.Size = new System.Drawing.Size(44, 13);
            this.labelAV.TabIndex = 13;
            this.labelAV.Text = "EMPTY";
            // 
            // labelD
            // 
            this.labelD.AutoSize = true;
            this.labelD.Location = new System.Drawing.Point(86, 81);
            this.labelD.Name = "labelD";
            this.labelD.Size = new System.Drawing.Size(44, 13);
            this.labelD.TabIndex = 12;
            this.labelD.Text = "EMPTY";
            // 
            // labelB
            // 
            this.labelB.AutoSize = true;
            this.labelB.Location = new System.Drawing.Point(86, 68);
            this.labelB.Name = "labelB";
            this.labelB.Size = new System.Drawing.Size(44, 13);
            this.labelB.TabIndex = 11;
            this.labelB.Text = "EMPTY";
            // 
            // labelModel
            // 
            this.labelModel.AutoSize = true;
            this.labelModel.Location = new System.Drawing.Point(86, 55);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(44, 13);
            this.labelModel.TabIndex = 10;
            this.labelModel.Text = "EMPTY";
            // 
            // labelManu
            // 
            this.labelManu.AutoSize = true;
            this.labelManu.Location = new System.Drawing.Point(86, 42);
            this.labelManu.Name = "labelManu";
            this.labelManu.Size = new System.Drawing.Size(44, 13);
            this.labelManu.TabIndex = 9;
            this.labelManu.Text = "EMPTY";
            // 
            // labelCA
            // 
            this.labelCA.AutoSize = true;
            this.labelCA.Location = new System.Drawing.Point(86, 29);
            this.labelCA.Name = "labelCA";
            this.labelCA.Size = new System.Drawing.Size(44, 13);
            this.labelCA.TabIndex = 8;
            this.labelCA.Text = "EMPTY";
            // 
            // labelBU
            // 
            this.labelBU.AutoSize = true;
            this.labelBU.Location = new System.Drawing.Point(86, 16);
            this.labelBU.Name = "labelBU";
            this.labelBU.Size = new System.Drawing.Size(44, 13);
            this.labelBU.TabIndex = 7;
            this.labelBU.Text = "EMPTY";
            // 
            // labelAndroidVersion
            // 
            this.labelAndroidVersion.AutoSize = true;
            this.labelAndroidVersion.Location = new System.Drawing.Point(6, 94);
            this.labelAndroidVersion.Name = "labelAndroidVersion";
            this.labelAndroidVersion.Size = new System.Drawing.Size(84, 13);
            this.labelAndroidVersion.TabIndex = 6;
            this.labelAndroidVersion.Text = "Android Version:";
            // 
            // labelProductDevice
            // 
            this.labelProductDevice.AutoSize = true;
            this.labelProductDevice.Location = new System.Drawing.Point(6, 81);
            this.labelProductDevice.Name = "labelProductDevice";
            this.labelProductDevice.Size = new System.Drawing.Size(44, 13);
            this.labelProductDevice.TabIndex = 5;
            this.labelProductDevice.Text = "Device:";
            // 
            // labelProductBoard
            // 
            this.labelProductBoard.AutoSize = true;
            this.labelProductBoard.Location = new System.Drawing.Point(6, 68);
            this.labelProductBoard.Name = "labelProductBoard";
            this.labelProductBoard.Size = new System.Drawing.Size(38, 13);
            this.labelProductBoard.TabIndex = 4;
            this.labelProductBoard.Text = "Board:";
            // 
            // labelProductModel
            // 
            this.labelProductModel.AutoSize = true;
            this.labelProductModel.Location = new System.Drawing.Point(6, 55);
            this.labelProductModel.Name = "labelProductModel";
            this.labelProductModel.Size = new System.Drawing.Size(39, 13);
            this.labelProductModel.TabIndex = 3;
            this.labelProductModel.Text = "Model:";
            // 
            // labelManufacturer
            // 
            this.labelManufacturer.AutoSize = true;
            this.labelManufacturer.Location = new System.Drawing.Point(6, 42);
            this.labelManufacturer.Name = "labelManufacturer";
            this.labelManufacturer.Size = new System.Drawing.Size(73, 13);
            this.labelManufacturer.TabIndex = 2;
            this.labelManufacturer.Text = "Manufacturer:";
            // 
            // labelCpuAbilitis
            // 
            this.labelCpuAbilitis.AutoSize = true;
            this.labelCpuAbilitis.Location = new System.Drawing.Point(6, 29);
            this.labelCpuAbilitis.Name = "labelCpuAbilitis";
            this.labelCpuAbilitis.Size = new System.Drawing.Size(61, 13);
            this.labelCpuAbilitis.TabIndex = 1;
            this.labelCpuAbilitis.Text = "Cpu Abilitis:";
            // 
            // labelBuildUser
            // 
            this.labelBuildUser.AutoSize = true;
            this.labelBuildUser.Location = new System.Drawing.Point(6, 16);
            this.labelBuildUser.Name = "labelBuildUser";
            this.labelBuildUser.Size = new System.Drawing.Size(58, 13);
            this.labelBuildUser.TabIndex = 0;
            this.labelBuildUser.Text = "Build User:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSystem);
            this.tabControl1.Controls.Add(this.tabPageFastboot);
            this.tabControl1.Controls.Add(this.tabPageRecovery);
            this.tabControl1.Location = new System.Drawing.Point(12, 42);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(828, 321);
            this.tabControl1.TabIndex = 33;
            // 
            // tabPageSystem
            // 
            this.tabPageSystem.Controls.Add(this.groupBoxADBNet);
            this.tabPageSystem.Controls.Add(this.panelSystem);
            this.tabPageSystem.Controls.Add(this.groupBoxRebootMenu);
            this.tabPageSystem.Location = new System.Drawing.Point(4, 22);
            this.tabPageSystem.Name = "tabPageSystem";
            this.tabPageSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSystem.Size = new System.Drawing.Size(820, 295);
            this.tabPageSystem.TabIndex = 0;
            this.tabPageSystem.Text = "System";
            this.tabPageSystem.UseVisualStyleBackColor = true;
            // 
            // groupBoxADBNet
            // 
            this.groupBoxADBNet.Controls.Add(this.label2);
            this.groupBoxADBNet.Controls.Add(this.buttonDisconnectIP);
            this.groupBoxADBNet.Controls.Add(this.textBoxIP);
            this.groupBoxADBNet.Controls.Add(this.buttonConnectToIP);
            this.groupBoxADBNet.Location = new System.Drawing.Point(157, 172);
            this.groupBoxADBNet.Name = "groupBoxADBNet";
            this.groupBoxADBNet.Size = new System.Drawing.Size(213, 119);
            this.groupBoxADBNet.TabIndex = 37;
            this.groupBoxADBNet.TabStop = false;
            this.groupBoxADBNet.Text = "ADB OVER NETWORK MENU";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "IP:";
            // 
            // buttonDisconnectIP
            // 
            this.buttonDisconnectIP.BackColor = System.Drawing.Color.White;
            this.buttonDisconnectIP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDisconnectIP.ForeColor = System.Drawing.Color.Black;
            this.buttonDisconnectIP.Location = new System.Drawing.Point(120, 52);
            this.buttonDisconnectIP.Name = "buttonDisconnectIP";
            this.buttonDisconnectIP.Size = new System.Drawing.Size(76, 23);
            this.buttonDisconnectIP.TabIndex = 38;
            this.buttonDisconnectIP.Text = "Disconnect";
            this.buttonDisconnectIP.UseVisualStyleBackColor = false;
            this.buttonDisconnectIP.Click += new System.EventHandler(this.buttonDisconnectIP_Click);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(29, 26);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(167, 20);
            this.textBoxIP.TabIndex = 37;
            // 
            // buttonConnectToIP
            // 
            this.buttonConnectToIP.BackColor = System.Drawing.Color.White;
            this.buttonConnectToIP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnectToIP.ForeColor = System.Drawing.Color.Black;
            this.buttonConnectToIP.Location = new System.Drawing.Point(29, 52);
            this.buttonConnectToIP.Name = "buttonConnectToIP";
            this.buttonConnectToIP.Size = new System.Drawing.Size(76, 23);
            this.buttonConnectToIP.TabIndex = 35;
            this.buttonConnectToIP.Text = "Connect";
            this.buttonConnectToIP.UseVisualStyleBackColor = false;
            this.buttonConnectToIP.Click += new System.EventHandler(this.buttonConnectToIP_Click);
            // 
            // panelSystem
            // 
            this.panelSystem.Controls.Add(this.groupBox3);
            this.panelSystem.Controls.Add(this.groupBoxDeviceInfo);
            this.panelSystem.Location = new System.Drawing.Point(3, 3);
            this.panelSystem.Name = "panelSystem";
            this.panelSystem.Size = new System.Drawing.Size(811, 163);
            this.panelSystem.TabIndex = 37;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonDE);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.buttonReloadApps);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.checkBoxSelectAll);
            this.groupBox3.Controls.Add(this.textBoxSearch);
            this.groupBox3.Controls.Add(this.checkedListBoxApp);
            this.groupBox3.Controls.Add(this.buttonUninstallApk);
            this.groupBox3.Controls.Add(this.buttonApk);
            this.groupBox3.Location = new System.Drawing.Point(266, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(542, 157);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "APK Menu";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonCheckPermissions);
            this.groupBox2.Controls.Add(this.buttonGrantDump);
            this.groupBox2.Controls.Add(this.buttonGrantPermission);
            this.groupBox2.Location = new System.Drawing.Point(142, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(155, 136);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Grant Permission Menu";
            // 
            // buttonCheckPermissions
            // 
            this.buttonCheckPermissions.BackColor = System.Drawing.Color.White;
            this.buttonCheckPermissions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCheckPermissions.ForeColor = System.Drawing.Color.Black;
            this.buttonCheckPermissions.Location = new System.Drawing.Point(5, 76);
            this.buttonCheckPermissions.Name = "buttonCheckPermissions";
            this.buttonCheckPermissions.Size = new System.Drawing.Size(146, 23);
            this.buttonCheckPermissions.TabIndex = 42;
            this.buttonCheckPermissions.Text = "Check granted permissions";
            this.buttonCheckPermissions.UseVisualStyleBackColor = false;
            this.buttonCheckPermissions.Click += new System.EventHandler(this.buttonCheckPermissions_Click);
            // 
            // buttonGrantDump
            // 
            this.buttonGrantDump.BackColor = System.Drawing.Color.White;
            this.buttonGrantDump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGrantDump.ForeColor = System.Drawing.Color.Black;
            this.buttonGrantDump.Location = new System.Drawing.Point(5, 47);
            this.buttonGrantDump.Name = "buttonGrantDump";
            this.buttonGrantDump.Size = new System.Drawing.Size(146, 23);
            this.buttonGrantDump.TabIndex = 41;
            this.buttonGrantDump.Text = "DUMP";
            this.buttonGrantDump.UseVisualStyleBackColor = false;
            this.buttonGrantDump.Click += new System.EventHandler(this.buttonGrantDump_Click);
            // 
            // buttonGrantPermission
            // 
            this.buttonGrantPermission.BackColor = System.Drawing.Color.White;
            this.buttonGrantPermission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGrantPermission.ForeColor = System.Drawing.Color.Black;
            this.buttonGrantPermission.Location = new System.Drawing.Point(5, 19);
            this.buttonGrantPermission.Name = "buttonGrantPermission";
            this.buttonGrantPermission.Size = new System.Drawing.Size(146, 23);
            this.buttonGrantPermission.TabIndex = 40;
            this.buttonGrantPermission.Text = "Write_Secure_Settings";
            this.buttonGrantPermission.UseVisualStyleBackColor = false;
            this.buttonGrantPermission.Click += new System.EventHandler(this.buttonGrantPermission_Click);
            // 
            // buttonReloadApps
            // 
            this.buttonReloadApps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReloadApps.ForeColor = System.Drawing.Color.Black;
            this.buttonReloadApps.Image = global::ATA_GUI.Properties.Resources.reload;
            this.buttonReloadApps.Location = new System.Drawing.Point(508, 15);
            this.buttonReloadApps.Name = "buttonReloadApps";
            this.buttonReloadApps.Size = new System.Drawing.Size(28, 20);
            this.buttonReloadApps.TabIndex = 39;
            this.buttonReloadApps.UseVisualStyleBackColor = true;
            this.buttonReloadApps.Click += new System.EventHandler(this.buttonReloadApps_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.radioButtoNonSystemApp);
            this.panel1.Controls.Add(this.radioButtonSystemApp);
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(6, 104);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(131, 48);
            this.panel1.TabIndex = 38;
            // 
            // radioButtoNonSystemApp
            // 
            this.radioButtoNonSystemApp.AutoSize = true;
            this.radioButtoNonSystemApp.ForeColor = System.Drawing.Color.Black;
            this.radioButtoNonSystemApp.Location = new System.Drawing.Point(3, 26);
            this.radioButtoNonSystemApp.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtoNonSystemApp.Name = "radioButtoNonSystemApp";
            this.radioButtoNonSystemApp.Size = new System.Drawing.Size(104, 17);
            this.radioButtoNonSystemApp.TabIndex = 11;
            this.radioButtoNonSystemApp.TabStop = true;
            this.radioButtoNonSystemApp.Text = "Non System App";
            this.radioButtoNonSystemApp.UseVisualStyleBackColor = true;
            this.radioButtoNonSystemApp.CheckedChanged += new System.EventHandler(this.radioButtoNonSystemApp_CheckedChanged);
            // 
            // radioButtonSystemApp
            // 
            this.radioButtonSystemApp.AutoSize = true;
            this.radioButtonSystemApp.ForeColor = System.Drawing.Color.Black;
            this.radioButtonSystemApp.Location = new System.Drawing.Point(3, 5);
            this.radioButtonSystemApp.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonSystemApp.Name = "radioButtonSystemApp";
            this.radioButtonSystemApp.Size = new System.Drawing.Size(81, 17);
            this.radioButtonSystemApp.TabIndex = 10;
            this.radioButtonSystemApp.TabStop = true;
            this.radioButtonSystemApp.Text = "System App";
            this.radioButtonSystemApp.UseVisualStyleBackColor = true;
            this.radioButtonSystemApp.CheckedChanged += new System.EventHandler(this.radioButtonSystemApp_CheckedChanged);
            // 
            // checkBoxSelectAll
            // 
            this.checkBoxSelectAll.AutoSize = true;
            this.checkBoxSelectAll.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSelectAll.Location = new System.Drawing.Point(302, 18);
            this.checkBoxSelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxSelectAll.Name = "checkBoxSelectAll";
            this.checkBoxSelectAll.Size = new System.Drawing.Size(70, 17);
            this.checkBoxSelectAll.TabIndex = 37;
            this.checkBoxSelectAll.Text = "Select All";
            this.checkBoxSelectAll.UseVisualStyleBackColor = true;
            this.checkBoxSelectAll.CheckedChanged += new System.EventHandler(this.checkBoxSelectAll_CheckedChanged);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BackColor = System.Drawing.Color.White;
            this.textBoxSearch.ForeColor = System.Drawing.Color.Black;
            this.textBoxSearch.Location = new System.Drawing.Point(376, 15);
            this.textBoxSearch.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxSearch.Size = new System.Drawing.Size(128, 20);
            this.textBoxSearch.TabIndex = 37;
            this.textBoxSearch.Text = "Search";
            this.textBoxSearch.Click += new System.EventHandler(this.textboxClick);
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // checkedListBoxApp
            // 
            this.checkedListBoxApp.BackColor = System.Drawing.Color.White;
            this.checkedListBoxApp.ForeColor = System.Drawing.Color.Black;
            this.checkedListBoxApp.FormattingEnabled = true;
            this.checkedListBoxApp.Location = new System.Drawing.Point(302, 41);
            this.checkedListBoxApp.Margin = new System.Windows.Forms.Padding(2);
            this.checkedListBoxApp.Name = "checkedListBoxApp";
            this.checkedListBoxApp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkedListBoxApp.Size = new System.Drawing.Size(235, 109);
            this.checkedListBoxApp.TabIndex = 37;
            // 
            // buttonUninstallApk
            // 
            this.buttonUninstallApk.BackColor = System.Drawing.Color.White;
            this.buttonUninstallApk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUninstallApk.ForeColor = System.Drawing.Color.Black;
            this.buttonUninstallApk.Location = new System.Drawing.Point(5, 48);
            this.buttonUninstallApk.Name = "buttonUninstallApk";
            this.buttonUninstallApk.Size = new System.Drawing.Size(131, 23);
            this.buttonUninstallApk.TabIndex = 36;
            this.buttonUninstallApk.Text = "Uninstall App";
            this.buttonUninstallApk.UseVisualStyleBackColor = false;
            this.buttonUninstallApk.Click += new System.EventHandler(this.buttonUninstallApk_Click);
            // 
            // buttonApk
            // 
            this.buttonApk.BackColor = System.Drawing.Color.White;
            this.buttonApk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApk.ForeColor = System.Drawing.Color.Black;
            this.buttonApk.Location = new System.Drawing.Point(6, 19);
            this.buttonApk.Name = "buttonApk";
            this.buttonApk.Size = new System.Drawing.Size(131, 23);
            this.buttonApk.TabIndex = 35;
            this.buttonApk.Text = "Install APK";
            this.buttonApk.UseVisualStyleBackColor = false;
            this.buttonApk.Click += new System.EventHandler(this.buttonApk_Click);
            // 
            // groupBoxRebootMenu
            // 
            this.groupBoxRebootMenu.Controls.Add(this.buttonRS);
            this.groupBoxRebootMenu.Controls.Add(this.buttonRR);
            this.groupBoxRebootMenu.Controls.Add(this.buttonRF);
            this.groupBoxRebootMenu.Location = new System.Drawing.Point(3, 172);
            this.groupBoxRebootMenu.Name = "groupBoxRebootMenu";
            this.groupBoxRebootMenu.Size = new System.Drawing.Size(148, 119);
            this.groupBoxRebootMenu.TabIndex = 35;
            this.groupBoxRebootMenu.TabStop = false;
            this.groupBoxRebootMenu.Text = "Reboot Menu";
            // 
            // buttonRS
            // 
            this.buttonRS.BackColor = System.Drawing.Color.White;
            this.buttonRS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRS.ForeColor = System.Drawing.Color.Black;
            this.buttonRS.Location = new System.Drawing.Point(6, 28);
            this.buttonRS.Name = "buttonRS";
            this.buttonRS.Size = new System.Drawing.Size(131, 23);
            this.buttonRS.TabIndex = 1;
            this.buttonRS.Text = "Reboot Smartphone";
            this.buttonRS.UseVisualStyleBackColor = false;
            this.buttonRS.Click += new System.EventHandler(this.buttonRS_Click);
            // 
            // buttonRR
            // 
            this.buttonRR.BackColor = System.Drawing.Color.White;
            this.buttonRR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRR.ForeColor = System.Drawing.Color.Black;
            this.buttonRR.Location = new System.Drawing.Point(6, 57);
            this.buttonRR.Name = "buttonRR";
            this.buttonRR.Size = new System.Drawing.Size(131, 23);
            this.buttonRR.TabIndex = 2;
            this.buttonRR.Text = "Reboot Into Recovery";
            this.buttonRR.UseVisualStyleBackColor = false;
            this.buttonRR.Click += new System.EventHandler(this.buttonRR_Click);
            // 
            // buttonRF
            // 
            this.buttonRF.BackColor = System.Drawing.Color.White;
            this.buttonRF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRF.ForeColor = System.Drawing.Color.Black;
            this.buttonRF.Location = new System.Drawing.Point(6, 86);
            this.buttonRF.Name = "buttonRF";
            this.buttonRF.Size = new System.Drawing.Size(131, 23);
            this.buttonRF.TabIndex = 3;
            this.buttonRF.Text = "Reboot into Fastboot";
            this.buttonRF.UseVisualStyleBackColor = false;
            this.buttonRF.Click += new System.EventHandler(this.buttonRF_Click);
            // 
            // tabPageFastboot
            // 
            this.tabPageFastboot.Controls.Add(this.panelFastboot);
            this.tabPageFastboot.Location = new System.Drawing.Point(4, 22);
            this.tabPageFastboot.Name = "tabPageFastboot";
            this.tabPageFastboot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFastboot.Size = new System.Drawing.Size(820, 284);
            this.tabPageFastboot.TabIndex = 1;
            this.tabPageFastboot.Text = "Fastboot";
            this.tabPageFastboot.UseVisualStyleBackColor = true;
            // 
            // panelFastboot
            // 
            this.panelFastboot.Controls.Add(this.buttonBootloaderMenu);
            this.panelFastboot.Controls.Add(this.buttonRebootRecovery);
            this.panelFastboot.Controls.Add(this.groupBox5);
            this.panelFastboot.Controls.Add(this.buttonHardReset);
            this.panelFastboot.Controls.Add(this.buttonRebootToSystem);
            this.panelFastboot.Controls.Add(this.groupBox4);
            this.panelFastboot.Location = new System.Drawing.Point(1, 3);
            this.panelFastboot.Name = "panelFastboot";
            this.panelFastboot.Size = new System.Drawing.Size(819, 285);
            this.panelFastboot.TabIndex = 49;
            // 
            // buttonBootloaderMenu
            // 
            this.buttonBootloaderMenu.BackColor = System.Drawing.Color.White;
            this.buttonBootloaderMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBootloaderMenu.ForeColor = System.Drawing.Color.Black;
            this.buttonBootloaderMenu.Location = new System.Drawing.Point(448, 220);
            this.buttonBootloaderMenu.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBootloaderMenu.Name = "buttonBootloaderMenu";
            this.buttonBootloaderMenu.Size = new System.Drawing.Size(118, 23);
            this.buttonBootloaderMenu.TabIndex = 55;
            this.buttonBootloaderMenu.Text = "Bootloader Menu";
            this.buttonBootloaderMenu.UseVisualStyleBackColor = false;
            this.buttonBootloaderMenu.Click += new System.EventHandler(this.buttonBootloaderMenu_Click);
            // 
            // buttonRebootRecovery
            // 
            this.buttonRebootRecovery.BackColor = System.Drawing.Color.White;
            this.buttonRebootRecovery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRebootRecovery.ForeColor = System.Drawing.Color.Black;
            this.buttonRebootRecovery.Location = new System.Drawing.Point(448, 193);
            this.buttonRebootRecovery.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRebootRecovery.Name = "buttonRebootRecovery";
            this.buttonRebootRecovery.Size = new System.Drawing.Size(118, 23);
            this.buttonRebootRecovery.TabIndex = 54;
            this.buttonRebootRecovery.Text = "Reboot to Recovery";
            this.buttonRebootRecovery.UseVisualStyleBackColor = false;
            this.buttonRebootRecovery.Click += new System.EventHandler(this.buttonRebootRecovery_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.labelUDT);
            this.groupBox5.Controls.Add(this.labelCDT);
            this.groupBox5.Controls.Add(this.labelBootloaderStatus);
            this.groupBox5.Controls.Add(this.labelDataType);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Location = new System.Drawing.Point(428, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(385, 155);
            this.groupBox5.TabIndex = 51;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Device Info";
            // 
            // labelUDT
            // 
            this.labelUDT.AutoSize = true;
            this.labelUDT.Location = new System.Drawing.Point(132, 65);
            this.labelUDT.Name = "labelUDT";
            this.labelUDT.Size = new System.Drawing.Size(44, 13);
            this.labelUDT.TabIndex = 15;
            this.labelUDT.Text = "EMPTY";
            // 
            // labelCDT
            // 
            this.labelCDT.AutoSize = true;
            this.labelCDT.Location = new System.Drawing.Point(132, 52);
            this.labelCDT.Name = "labelCDT";
            this.labelCDT.Size = new System.Drawing.Size(44, 13);
            this.labelCDT.TabIndex = 14;
            this.labelCDT.Text = "EMPTY";
            // 
            // labelBootloaderStatus
            // 
            this.labelBootloaderStatus.AutoSize = true;
            this.labelBootloaderStatus.Location = new System.Drawing.Point(132, 39);
            this.labelBootloaderStatus.Name = "labelBootloaderStatus";
            this.labelBootloaderStatus.Size = new System.Drawing.Size(44, 13);
            this.labelBootloaderStatus.TabIndex = 13;
            this.labelBootloaderStatus.Text = "EMPTY";
            // 
            // labelDataType
            // 
            this.labelDataType.AutoSize = true;
            this.labelDataType.Location = new System.Drawing.Point(17, 65);
            this.labelDataType.Name = "labelDataType";
            this.labelDataType.Size = new System.Drawing.Size(60, 13);
            this.labelDataType.TabIndex = 12;
            this.labelDataType.Text = "Data Type:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Cache Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Bootloader Unlocked:";
            // 
            // buttonHardReset
            // 
            this.buttonHardReset.BackColor = System.Drawing.Color.White;
            this.buttonHardReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHardReset.ForeColor = System.Drawing.Color.Black;
            this.buttonHardReset.Location = new System.Drawing.Point(677, 166);
            this.buttonHardReset.Margin = new System.Windows.Forms.Padding(2);
            this.buttonHardReset.Name = "buttonHardReset";
            this.buttonHardReset.Size = new System.Drawing.Size(118, 23);
            this.buttonHardReset.TabIndex = 50;
            this.buttonHardReset.Text = "Hard Reset";
            this.buttonHardReset.UseVisualStyleBackColor = false;
            this.buttonHardReset.Click += new System.EventHandler(this.buttonHardReset_Click);
            // 
            // buttonRebootToSystem
            // 
            this.buttonRebootToSystem.BackColor = System.Drawing.Color.White;
            this.buttonRebootToSystem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRebootToSystem.ForeColor = System.Drawing.Color.Black;
            this.buttonRebootToSystem.Location = new System.Drawing.Point(448, 166);
            this.buttonRebootToSystem.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRebootToSystem.Name = "buttonRebootToSystem";
            this.buttonRebootToSystem.Size = new System.Drawing.Size(118, 23);
            this.buttonRebootToSystem.TabIndex = 49;
            this.buttonRebootToSystem.Text = "Reboot to System";
            this.buttonRebootToSystem.UseVisualStyleBackColor = false;
            this.buttonRebootToSystem.Click += new System.EventHandler(this.buttonRebootToSystem_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxDirImg);
            this.groupBox4.Controls.Add(this.radioButtonRom);
            this.groupBox4.Controls.Add(this.buttonSearchFileFastboot);
            this.groupBox4.Controls.Add(this.radioButtonCache);
            this.groupBox4.Controls.Add(this.buttonFlashImg);
            this.groupBox4.Controls.Add(this.radioButtonVendor);
            this.groupBox4.Controls.Add(this.pictureBoxLoading2);
            this.groupBox4.Controls.Add(this.radioButtonSystem);
            this.groupBox4.Controls.Add(this.radioButtonBoot);
            this.groupBox4.Controls.Add(this.radioButtonRecovery);
            this.groupBox4.Controls.Add(this.radioButtonBootloader);
            this.groupBox4.Controls.Add(this.radioButtonRadio);
            this.groupBox4.Location = new System.Drawing.Point(3, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(419, 272);
            this.groupBox4.TabIndex = 48;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Flash img";
            // 
            // textBoxDirImg
            // 
            this.textBoxDirImg.Location = new System.Drawing.Point(26, 41);
            this.textBoxDirImg.Name = "textBoxDirImg";
            this.textBoxDirImg.Size = new System.Drawing.Size(245, 20);
            this.textBoxDirImg.TabIndex = 37;
            // 
            // radioButtonRom
            // 
            this.radioButtonRom.AutoSize = true;
            this.radioButtonRom.Location = new System.Drawing.Point(279, 229);
            this.radioButtonRom.Name = "radioButtonRom";
            this.radioButtonRom.Size = new System.Drawing.Size(47, 17);
            this.radioButtonRom.TabIndex = 47;
            this.radioButtonRom.TabStop = true;
            this.radioButtonRom.Text = "Rom";
            this.radioButtonRom.UseVisualStyleBackColor = true;
            // 
            // buttonSearchFileFastboot
            // 
            this.buttonSearchFileFastboot.BackColor = System.Drawing.Color.White;
            this.buttonSearchFileFastboot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSearchFileFastboot.Location = new System.Drawing.Point(277, 39);
            this.buttonSearchFileFastboot.Name = "buttonSearchFileFastboot";
            this.buttonSearchFileFastboot.Size = new System.Drawing.Size(87, 23);
            this.buttonSearchFileFastboot.TabIndex = 36;
            this.buttonSearchFileFastboot.Text = "Search File";
            this.buttonSearchFileFastboot.UseVisualStyleBackColor = false;
            this.buttonSearchFileFastboot.Click += new System.EventHandler(this.buttonSearchFileFastboot_Click);
            // 
            // radioButtonCache
            // 
            this.radioButtonCache.AutoSize = true;
            this.radioButtonCache.Location = new System.Drawing.Point(279, 206);
            this.radioButtonCache.Name = "radioButtonCache";
            this.radioButtonCache.Size = new System.Drawing.Size(56, 17);
            this.radioButtonCache.TabIndex = 46;
            this.radioButtonCache.TabStop = true;
            this.radioButtonCache.Text = "Cache";
            this.radioButtonCache.UseVisualStyleBackColor = true;
            // 
            // buttonFlashImg
            // 
            this.buttonFlashImg.BackColor = System.Drawing.Color.White;
            this.buttonFlashImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFlashImg.Location = new System.Drawing.Point(114, 177);
            this.buttonFlashImg.Name = "buttonFlashImg";
            this.buttonFlashImg.Size = new System.Drawing.Size(87, 23);
            this.buttonFlashImg.TabIndex = 38;
            this.buttonFlashImg.Text = "Flash img";
            this.buttonFlashImg.UseVisualStyleBackColor = false;
            this.buttonFlashImg.Click += new System.EventHandler(this.buttonFlashImg_Click);
            // 
            // radioButtonVendor
            // 
            this.radioButtonVendor.AutoSize = true;
            this.radioButtonVendor.Location = new System.Drawing.Point(279, 183);
            this.radioButtonVendor.Name = "radioButtonVendor";
            this.radioButtonVendor.Size = new System.Drawing.Size(59, 17);
            this.radioButtonVendor.TabIndex = 45;
            this.radioButtonVendor.TabStop = true;
            this.radioButtonVendor.Text = "Vendor";
            this.radioButtonVendor.UseVisualStyleBackColor = true;
            // 
            // pictureBoxLoading2
            // 
            this.pictureBoxLoading2.Image = global::ATA_GUI.Properties.Resources.loader;
            this.pictureBoxLoading2.Location = new System.Drawing.Point(140, 114);
            this.pictureBoxLoading2.Name = "pictureBoxLoading2";
            this.pictureBoxLoading2.Size = new System.Drawing.Size(34, 33);
            this.pictureBoxLoading2.TabIndex = 39;
            this.pictureBoxLoading2.TabStop = false;
            // 
            // radioButtonSystem
            // 
            this.radioButtonSystem.AutoSize = true;
            this.radioButtonSystem.Location = new System.Drawing.Point(279, 160);
            this.radioButtonSystem.Name = "radioButtonSystem";
            this.radioButtonSystem.Size = new System.Drawing.Size(59, 17);
            this.radioButtonSystem.TabIndex = 44;
            this.radioButtonSystem.TabStop = true;
            this.radioButtonSystem.Text = "System";
            this.radioButtonSystem.UseVisualStyleBackColor = true;
            // 
            // radioButtonBoot
            // 
            this.radioButtonBoot.AutoSize = true;
            this.radioButtonBoot.Location = new System.Drawing.Point(279, 68);
            this.radioButtonBoot.Name = "radioButtonBoot";
            this.radioButtonBoot.Size = new System.Drawing.Size(47, 17);
            this.radioButtonBoot.TabIndex = 40;
            this.radioButtonBoot.TabStop = true;
            this.radioButtonBoot.Text = "Boot";
            this.radioButtonBoot.UseVisualStyleBackColor = true;
            // 
            // radioButtonRecovery
            // 
            this.radioButtonRecovery.AutoSize = true;
            this.radioButtonRecovery.Location = new System.Drawing.Point(279, 137);
            this.radioButtonRecovery.Name = "radioButtonRecovery";
            this.radioButtonRecovery.Size = new System.Drawing.Size(71, 17);
            this.radioButtonRecovery.TabIndex = 43;
            this.radioButtonRecovery.TabStop = true;
            this.radioButtonRecovery.Text = "Recovery";
            this.radioButtonRecovery.UseVisualStyleBackColor = true;
            // 
            // radioButtonBootloader
            // 
            this.radioButtonBootloader.AutoSize = true;
            this.radioButtonBootloader.Location = new System.Drawing.Point(279, 91);
            this.radioButtonBootloader.Name = "radioButtonBootloader";
            this.radioButtonBootloader.Size = new System.Drawing.Size(76, 17);
            this.radioButtonBootloader.TabIndex = 41;
            this.radioButtonBootloader.TabStop = true;
            this.radioButtonBootloader.Text = "Bootloader";
            this.radioButtonBootloader.UseVisualStyleBackColor = true;
            // 
            // radioButtonRadio
            // 
            this.radioButtonRadio.AutoSize = true;
            this.radioButtonRadio.Location = new System.Drawing.Point(279, 114);
            this.radioButtonRadio.Name = "radioButtonRadio";
            this.radioButtonRadio.Size = new System.Drawing.Size(53, 17);
            this.radioButtonRadio.TabIndex = 42;
            this.radioButtonRadio.TabStop = true;
            this.radioButtonRadio.Text = "Radio";
            this.radioButtonRadio.UseVisualStyleBackColor = true;
            // 
            // tabPageRecovery
            // 
            this.tabPageRecovery.Controls.Add(this.buttonInstallZip);
            this.tabPageRecovery.Controls.Add(this.textBoxDirFile);
            this.tabPageRecovery.Controls.Add(this.buttonSearchFile);
            this.tabPageRecovery.Location = new System.Drawing.Point(4, 22);
            this.tabPageRecovery.Name = "tabPageRecovery";
            this.tabPageRecovery.Size = new System.Drawing.Size(820, 284);
            this.tabPageRecovery.TabIndex = 2;
            this.tabPageRecovery.Text = "Recovery";
            this.tabPageRecovery.UseVisualStyleBackColor = true;
            // 
            // buttonInstallZip
            // 
            this.buttonInstallZip.BackColor = System.Drawing.Color.White;
            this.buttonInstallZip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonInstallZip.Location = new System.Drawing.Point(365, 153);
            this.buttonInstallZip.Name = "buttonInstallZip";
            this.buttonInstallZip.Size = new System.Drawing.Size(87, 23);
            this.buttonInstallZip.TabIndex = 34;
            this.buttonInstallZip.Text = "Install";
            this.buttonInstallZip.UseVisualStyleBackColor = false;
            this.buttonInstallZip.Click += new System.EventHandler(this.buttonInstallZip_Click);
            // 
            // textBoxDirFile
            // 
            this.textBoxDirFile.Location = new System.Drawing.Point(250, 109);
            this.textBoxDirFile.Name = "textBoxDirFile";
            this.textBoxDirFile.Size = new System.Drawing.Size(245, 20);
            this.textBoxDirFile.TabIndex = 33;
            // 
            // buttonSearchFile
            // 
            this.buttonSearchFile.BackColor = System.Drawing.Color.White;
            this.buttonSearchFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSearchFile.Location = new System.Drawing.Point(501, 107);
            this.buttonSearchFile.Name = "buttonSearchFile";
            this.buttonSearchFile.Size = new System.Drawing.Size(87, 23);
            this.buttonSearchFile.TabIndex = 32;
            this.buttonSearchFile.Text = "Search File";
            this.buttonSearchFile.UseVisualStyleBackColor = false;
            this.buttonSearchFile.Click += new System.EventHandler(this.buttonSearchFile_Click);
            // 
            // openFileDialogAPK
            // 
            this.openFileDialogAPK.FileName = "openFileDialog";
            // 
            // openFileDialogZip
            // 
            this.openFileDialogZip.FileName = "openFileDialogZip";
            // 
            // backgroundWorkerZip
            // 
            this.backgroundWorkerZip.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerZip_DoWork);
            // 
            // backgroundWorkerFlashImg
            // 
            this.backgroundWorkerFlashImg.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerFlashImg_DoWork);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BackColor = System.Drawing.SystemColors.MenuText;
            this.richTextBoxLog.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBoxLog.Location = new System.Drawing.Point(443, 386);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(393, 64);
            this.richTextBoxLog.TabIndex = 38;
            this.richTextBoxLog.TabStop = false;
            this.richTextBoxLog.Text = "";
            // 
            // buttonDE
            // 
            this.buttonDE.BackColor = System.Drawing.Color.White;
            this.buttonDE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDE.ForeColor = System.Drawing.Color.Black;
            this.buttonDE.Location = new System.Drawing.Point(5, 77);
            this.buttonDE.Name = "buttonDE";
            this.buttonDE.Size = new System.Drawing.Size(131, 23);
            this.buttonDE.TabIndex = 41;
            this.buttonDE.Text = "Disabled/Enable";
            this.buttonDE.UseVisualStyleBackColor = false;
            this.buttonDE.Click += new System.EventHandler(this.buttonDE_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(852, 462);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.panelTopBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ATA GUI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.panelTopBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBoxDeviceInfo.ResumeLayout(false);
            this.groupBoxDeviceInfo.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSystem.ResumeLayout(false);
            this.groupBoxADBNet.ResumeLayout(false);
            this.groupBoxADBNet.PerformLayout();
            this.panelSystem.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBoxRebootMenu.ResumeLayout(false);
            this.tabPageFastboot.ResumeLayout(false);
            this.panelFastboot.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading2)).EndInit();
            this.tabPageRecovery.ResumeLayout(false);
            this.tabPageRecovery.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTopBar;
        private System.Windows.Forms.Button buttonCloseWindows;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.Button buttonKillAdb;
        private System.Windows.Forms.Button buttonSyncApp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSync;
        private System.Windows.Forms.GroupBox groupBoxDeviceInfo;
        private System.Windows.Forms.Label labelAV;
        private System.Windows.Forms.Label labelD;
        private System.Windows.Forms.Label labelB;
        private System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.Label labelManu;
        private System.Windows.Forms.Label labelCA;
        private System.Windows.Forms.Label labelBU;
        private System.Windows.Forms.Label labelAndroidVersion;
        private System.Windows.Forms.Label labelProductDevice;
        private System.Windows.Forms.Label labelProductBoard;
        private System.Windows.Forms.Label labelProductModel;
        private System.Windows.Forms.Label labelManufacturer;
        private System.Windows.Forms.Label labelCpuAbilitis;
        private System.Windows.Forms.Label labelBuildUser;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSystem;
        private System.Windows.Forms.TabPage tabPageFastboot;
        private System.Windows.Forms.TabPage tabPageRecovery;
        private System.Windows.Forms.Button buttonLogClear;
        private System.Windows.Forms.Button buttonRR;
        private System.Windows.Forms.Button buttonRF;
        private System.Windows.Forms.Button buttonRS;
        private System.Windows.Forms.GroupBox groupBoxRebootMenu;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonUninstallApk;
        private System.Windows.Forms.Button buttonApk;
        private System.Windows.Forms.OpenFileDialog openFileDialogAPK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButtoNonSystemApp;
        private System.Windows.Forms.RadioButton radioButtonSystemApp;
        private System.Windows.Forms.CheckBox checkBoxSelectAll;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.CheckedListBox checkedListBoxApp;
        private System.Windows.Forms.Button buttonReloadApps;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelSystem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBoxADBNet;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Button buttonConnectToIP;
        private System.Windows.Forms.Button buttonDisconnectIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonCheckPermissions;
        private System.Windows.Forms.Button buttonGrantDump;
        private System.Windows.Forms.Button buttonGrantPermission;
        private System.Windows.Forms.Button buttonInstallZip;
        private System.Windows.Forms.TextBox textBoxDirFile;
        private System.Windows.Forms.Button buttonSearchFile;
        private System.Windows.Forms.OpenFileDialog openFileDialogZip;
        private System.ComponentModel.BackgroundWorker backgroundWorkerZip;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxDirImg;
        private System.Windows.Forms.RadioButton radioButtonRom;
        private System.Windows.Forms.Button buttonSearchFileFastboot;
        private System.Windows.Forms.RadioButton radioButtonCache;
        private System.Windows.Forms.Button buttonFlashImg;
        private System.Windows.Forms.RadioButton radioButtonVendor;
        private System.Windows.Forms.PictureBox pictureBoxLoading2;
        private System.Windows.Forms.RadioButton radioButtonSystem;
        private System.Windows.Forms.RadioButton radioButtonBoot;
        private System.Windows.Forms.RadioButton radioButtonRecovery;
        private System.Windows.Forms.RadioButton radioButtonBootloader;
        private System.Windows.Forms.RadioButton radioButtonRadio;
        private System.ComponentModel.BackgroundWorker backgroundWorkerFlashImg;
        private System.Windows.Forms.Panel panelFastboot;
        private System.Windows.Forms.Button buttonRebootToSystem;
        private System.Windows.Forms.Button buttonHardReset;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label labelUDT;
        private System.Windows.Forms.Label labelCDT;
        private System.Windows.Forms.Label labelBootloaderStatus;
        private System.Windows.Forms.Label labelDataType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonBootloaderMenu;
        private System.Windows.Forms.Button buttonRebootRecovery;
        private System.Windows.Forms.Button buttonCredits;
        private System.Windows.Forms.Button buttonDE;
    }
}

