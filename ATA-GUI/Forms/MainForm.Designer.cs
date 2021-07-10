
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.labelLog = new System.Windows.Forms.Label();
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
            this.checkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSystem = new System.Windows.Forms.TabPage();
            this.groupBoxAPKMenu = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.labelSelectedAppCount = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonReloadApps = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.textBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRestoreApp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonUninstallApp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPackageManager = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPermissionMenu = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonBloatwareDetecter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelTotalApps = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonInstallApp = new System.Windows.Forms.ToolStripButton();
            this.checkedListBoxApp = new System.Windows.Forms.CheckedListBox();
            this.groupBoxADBNet = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonDisconnectIP = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.buttonConnectToIP = new System.Windows.Forms.Button();
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
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxDirImg = new System.Windows.Forms.TextBox();
            this.radioButtonRom = new System.Windows.Forms.RadioButton();
            this.buttonSearchFileFastboot = new System.Windows.Forms.Button();
            this.radioButtonCache = new System.Windows.Forms.RadioButton();
            this.buttonFlashImg = new System.Windows.Forms.Button();
            this.radioButtonVendor = new System.Windows.Forms.RadioButton();
            this.radioButtonSystem = new System.Windows.Forms.RadioButton();
            this.radioButtonBoot = new System.Windows.Forms.RadioButton();
            this.radioButtonRecovery = new System.Windows.Forms.RadioButton();
            this.radioButtonBootloader = new System.Windows.Forms.RadioButton();
            this.radioButtonRadio = new System.Windows.Forms.RadioButton();
            this.tabPageRecovery = new System.Windows.Forms.TabPage();
            this.pictureBoxSearchFile = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonInstallZip = new System.Windows.Forms.Button();
            this.textBoxDirFile = new System.Windows.Forms.TextBox();
            this.openFileDialogAPK = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogZip = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorkerZip = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerFlashImg = new System.ComponentModel.BackgroundWorker();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.systemAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nonSystemAppToolStripMenuItem = new System.Windows.Forms.ToolStripComboBox();
            this.filterByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripFilterBy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemAppToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.nonSystemAppToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstalledAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disabledAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripPermissionMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.grantWriteSecureSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grantWriteSecureSettingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.revokeWriteSecureSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grantDUMPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grantDUMPToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.revokeDUMPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkGrantedPermissionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorkerAdbDownloader = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerExeDownloader = new System.ComponentModel.BackgroundWorker();
            this.buttonMobileScreenShare = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.panelTopBar = new System.Windows.Forms.Panel();
            this.labelTools = new System.Windows.Forms.Label();
            this.pictureBoxClose = new System.Windows.Forms.PictureBox();
            this.pictureBoxMinimize = new System.Windows.Forms.PictureBox();
            this.labelHelp = new System.Windows.Forms.Label();
            this.labelSettings = new System.Windows.Forms.Label();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.contextMenuStripHelp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoTutorialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.submitFeedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoTutorialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonDeviceLogs = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonReloadDevicesList = new System.Windows.Forms.Button();
            this.comboBoxDevices = new System.Windows.Forms.ComboBox();
            this.buttonSyncApp = new System.Windows.Forms.Button();
            this.contextMenuStripSearch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.duckduckgoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.googleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playMarketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.APKMirrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fDroidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTools = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemADBKill = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonTaskManager = new System.Windows.Forms.Button();
            this.groupBoxDeviceInfo.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSystem.SuspendLayout();
            this.groupBoxAPKMenu.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.groupBoxADBNet.SuspendLayout();
            this.groupBoxRebootMenu.SuspendLayout();
            this.tabPageFastboot.SuspendLayout();
            this.panelFastboot.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPageRecovery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearchFile)).BeginInit();
            this.contextMenuStripFilterBy.SuspendLayout();
            this.contextMenuStripPermissionMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMinimize)).BeginInit();
            this.contextMenuStripHelp.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStripSearch.SuspendLayout();
            this.contextMenuStripTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLog.Location = new System.Drawing.Point(513, 372);
            this.labelLog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLog.Name = "labelLog";
            this.labelLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelLog.Size = new System.Drawing.Size(28, 13);
            this.labelLog.TabIndex = 24;
            this.labelLog.Text = "Log:";
            // 
            // buttonLogClear
            // 
            this.buttonLogClear.BackColor = System.Drawing.Color.White;
            this.buttonLogClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLogClear.ForeColor = System.Drawing.Color.Black;
            this.buttonLogClear.Location = new System.Drawing.Point(423, 431);
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
            this.groupBoxDeviceInfo.Location = new System.Drawing.Point(6, 9);
            this.groupBoxDeviceInfo.Name = "groupBoxDeviceInfo";
            this.groupBoxDeviceInfo.Size = new System.Drawing.Size(321, 157);
            this.groupBoxDeviceInfo.TabIndex = 32;
            this.groupBoxDeviceInfo.TabStop = false;
            this.groupBoxDeviceInfo.Text = "Device Info";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(86, 120);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(65, 13);
            this.labelStatus.TabIndex = 17;
            this.labelStatus.Text = "UNKNOWN";
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
            // checkBoxSelectAll
            // 
            this.checkBoxSelectAll.AutoSize = true;
            this.checkBoxSelectAll.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSelectAll.Location = new System.Drawing.Point(8, 56);
            this.checkBoxSelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxSelectAll.Name = "checkBoxSelectAll";
            this.checkBoxSelectAll.Size = new System.Drawing.Size(70, 17);
            this.checkBoxSelectAll.TabIndex = 37;
            this.checkBoxSelectAll.Text = "Select All";
            this.checkBoxSelectAll.UseVisualStyleBackColor = true;
            this.checkBoxSelectAll.CheckedChanged += new System.EventHandler(this.checkBoxSelectAll_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSystem);
            this.tabControl1.Controls.Add(this.tabPageFastboot);
            this.tabControl1.Controls.Add(this.tabPageRecovery);
            this.tabControl1.Location = new System.Drawing.Point(8, 46);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(901, 321);
            this.tabControl1.TabIndex = 33;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPageSystem
            // 
            this.tabPageSystem.Controls.Add(this.groupBoxDeviceInfo);
            this.tabPageSystem.Controls.Add(this.groupBoxAPKMenu);
            this.tabPageSystem.Controls.Add(this.groupBoxADBNet);
            this.tabPageSystem.Controls.Add(this.groupBoxRebootMenu);
            this.tabPageSystem.Location = new System.Drawing.Point(4, 22);
            this.tabPageSystem.Name = "tabPageSystem";
            this.tabPageSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSystem.Size = new System.Drawing.Size(893, 295);
            this.tabPageSystem.TabIndex = 0;
            this.tabPageSystem.Text = "System";
            this.tabPageSystem.UseVisualStyleBackColor = true;
            // 
            // groupBoxAPKMenu
            // 
            this.groupBoxAPKMenu.Controls.Add(this.groupBox2);
            this.groupBoxAPKMenu.Controls.Add(this.labelSelectedAppCount);
            this.groupBoxAPKMenu.Controls.Add(this.groupBox6);
            this.groupBoxAPKMenu.Controls.Add(this.toolStrip1);
            this.groupBoxAPKMenu.Controls.Add(this.checkedListBoxApp);
            this.groupBoxAPKMenu.Controls.Add(this.checkBoxSelectAll);
            this.groupBoxAPKMenu.Location = new System.Drawing.Point(331, 9);
            this.groupBoxAPKMenu.Name = "groupBoxAPKMenu";
            this.groupBoxAPKMenu.Size = new System.Drawing.Size(556, 280);
            this.groupBoxAPKMenu.TabIndex = 36;
            this.groupBoxAPKMenu.TabStop = false;
            this.groupBoxAPKMenu.Text = "APK Menu";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.pictureBox3);
            this.groupBox2.Location = new System.Drawing.Point(396, 173);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(155, 102);
            this.groupBox2.TabIndex = 44;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Transfer";
            this.groupBox2.DragDrop += new System.Windows.Forms.DragEventHandler(this.groupBox2_DragDrop);
            this.groupBox2.DragEnter += new System.Windows.Forms.DragEventHandler(this.groupBox2_DragEnter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(55, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Drop File";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::ATA_GUI.Properties.Resources.icons8_drag_and_drop_48;
            this.pictureBox3.Location = new System.Drawing.Point(56, 19);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(51, 47);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // labelSelectedAppCount
            // 
            this.labelSelectedAppCount.AutoSize = true;
            this.labelSelectedAppCount.Location = new System.Drawing.Point(251, 57);
            this.labelSelectedAppCount.Name = "labelSelectedAppCount";
            this.labelSelectedAppCount.Size = new System.Drawing.Size(83, 13);
            this.labelSelectedAppCount.TabIndex = 44;
            this.labelSelectedAppCount.Text = "Selected App: 0";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.pictureBox2);
            this.groupBox6.Location = new System.Drawing.Point(396, 68);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(155, 103);
            this.groupBox6.TabIndex = 43;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Apk Installer";
            this.groupBox6.DragDrop += new System.Windows.Forms.DragEventHandler(this.groupBox6_DragDrop);
            this.groupBox6.DragEnter += new System.Windows.Forms.DragEventHandler(this.groupBox6_DragEnter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Drop Apk";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ATA_GUI.Properties.Resources.icons8_drag_and_drop_48;
            this.pictureBox2.Location = new System.Drawing.Point(56, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(51, 47);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonReloadApps,
            this.toolStripSeparator2,
            this.toolStripButtonFilter,
            this.toolStripSeparator6,
            this.textBoxSearch,
            this.toolStripSeparator1,
            this.toolStripButtonRestoreApp,
            this.toolStripSeparator7,
            this.toolStripButtonUninstallApp,
            this.toolStripSeparator5,
            this.toolStripButtonPackageManager,
            this.toolStripSeparator3,
            this.toolStripButtonPermissionMenu,
            this.toolStripSeparator4,
            this.toolStripButtonBloatwareDetecter,
            this.toolStripSeparator10,
            this.toolStripButtonSearch,
            this.toolStripSeparator9,
            this.toolStripLabelTotalApps,
            this.toolStripSeparator8,
            this.toolStripButtonInstallApp});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(550, 23);
            this.toolStrip1.TabIndex = 42;
            this.toolStrip1.Text = "toolStripAPKMenu";
            // 
            // toolStripButtonReloadApps
            // 
            this.toolStripButtonReloadApps.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReloadApps.Image = global::ATA_GUI.Properties.Resources.icons8_refresh_48;
            this.toolStripButtonReloadApps.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReloadApps.Name = "toolStripButtonReloadApps";
            this.toolStripButtonReloadApps.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReloadApps.Text = "Reload Apps";
            this.toolStripButtonReloadApps.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonFilter
            // 
            this.toolStripButtonFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFilter.Image = global::ATA_GUI.Properties.Resources.icons8_filter_48;
            this.toolStripButtonFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFilter.Name = "toolStripButtonFilter";
            this.toolStripButtonFilter.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonFilter.Text = "Filter Apps";
            this.toolStripButtonFilter.ToolTipText = "Apps Filter ";
            this.toolStripButtonFilter.Click += new System.EventHandler(this.toolStripButtonFilter_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 23);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BackColor = System.Drawing.Color.White;
            this.textBoxSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(150, 23);
            this.textBoxSearch.Text = "Search";
            this.textBoxSearch.Click += new System.EventHandler(this.textBoxSearch_Click);
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonRestoreApp
            // 
            this.toolStripButtonRestoreApp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRestoreApp.Image = global::ATA_GUI.Properties.Resources.icons8_restart_48;
            this.toolStripButtonRestoreApp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRestoreApp.Name = "toolStripButtonRestoreApp";
            this.toolStripButtonRestoreApp.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonRestoreApp.Tag = "";
            this.toolStripButtonRestoreApp.Text = "Restore App";
            this.toolStripButtonRestoreApp.Click += new System.EventHandler(this.toolStripButtonRestoreApp_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonUninstallApp
            // 
            this.toolStripButtonUninstallApp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUninstallApp.Image = global::ATA_GUI.Properties.Resources.icons8_remove_48;
            this.toolStripButtonUninstallApp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUninstallApp.Name = "toolStripButtonUninstallApp";
            this.toolStripButtonUninstallApp.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonUninstallApp.Text = "Uninstall App";
            this.toolStripButtonUninstallApp.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonPackageManager
            // 
            this.toolStripButtonPackageManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPackageManager.Image = global::ATA_GUI.Properties.Resources.icons8_support_48;
            this.toolStripButtonPackageManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPackageManager.Name = "toolStripButtonPackageManager";
            this.toolStripButtonPackageManager.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonPackageManager.Text = "Package Manager";
            this.toolStripButtonPackageManager.ToolTipText = "Package Menu";
            this.toolStripButtonPackageManager.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonPermissionMenu
            // 
            this.toolStripButtonPermissionMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPermissionMenu.Image = global::ATA_GUI.Properties.Resources.icons8_services_48;
            this.toolStripButtonPermissionMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPermissionMenu.Name = "toolStripButtonPermissionMenu";
            this.toolStripButtonPermissionMenu.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonPermissionMenu.Text = "toolStripButton1";
            this.toolStripButtonPermissionMenu.ToolTipText = "Permission Menu";
            this.toolStripButtonPermissionMenu.Click += new System.EventHandler(this.toolStripButtonPermissionMenu_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonBloatwareDetecter
            // 
            this.toolStripButtonBloatwareDetecter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBloatwareDetecter.Image = global::ATA_GUI.Properties.Resources.icons8_box_important_48;
            this.toolStripButtonBloatwareDetecter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBloatwareDetecter.Name = "toolStripButtonBloatwareDetecter";
            this.toolStripButtonBloatwareDetecter.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonBloatwareDetecter.Text = "Bloatware Detecter";
            this.toolStripButtonBloatwareDetecter.Click += new System.EventHandler(this.toolStripButtonBloatwareDetecter_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonSearch
            // 
            this.toolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSearch.Image = global::ATA_GUI.Properties.Resources.icons8_search_48;
            this.toolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSearch.Name = "toolStripButtonSearch";
            this.toolStripButtonSearch.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSearch.Text = "Search";
            this.toolStripButtonSearch.Click += new System.EventHandler(this.toolStripButtonSearch_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripLabelTotalApps
            // 
            this.toolStripLabelTotalApps.Name = "toolStripLabelTotalApps";
            this.toolStripLabelTotalApps.Size = new System.Drawing.Size(44, 15);
            this.toolStripLabelTotalApps.Text = "Total: 0";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonInstallApp
            // 
            this.toolStripButtonInstallApp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInstallApp.Image = global::ATA_GUI.Properties.Resources.icons8_software_installer_48;
            this.toolStripButtonInstallApp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInstallApp.Name = "toolStripButtonInstallApp";
            this.toolStripButtonInstallApp.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonInstallApp.Text = "Install App";
            this.toolStripButtonInstallApp.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // checkedListBoxApp
            // 
            this.checkedListBoxApp.BackColor = System.Drawing.Color.White;
            this.checkedListBoxApp.ForeColor = System.Drawing.Color.Black;
            this.checkedListBoxApp.FormattingEnabled = true;
            this.checkedListBoxApp.Location = new System.Drawing.Point(5, 75);
            this.checkedListBoxApp.Margin = new System.Windows.Forms.Padding(2);
            this.checkedListBoxApp.Name = "checkedListBoxApp";
            this.checkedListBoxApp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkedListBoxApp.Size = new System.Drawing.Size(386, 199);
            this.checkedListBoxApp.TabIndex = 37;
            this.checkedListBoxApp.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxApp_SelectedIndexChanged);
            // 
            // groupBoxADBNet
            // 
            this.groupBoxADBNet.Controls.Add(this.label2);
            this.groupBoxADBNet.Controls.Add(this.buttonDisconnectIP);
            this.groupBoxADBNet.Controls.Add(this.textBoxIP);
            this.groupBoxADBNet.Controls.Add(this.buttonConnectToIP);
            this.groupBoxADBNet.Location = new System.Drawing.Point(157, 170);
            this.groupBoxADBNet.Name = "groupBoxADBNet";
            this.groupBoxADBNet.Size = new System.Drawing.Size(170, 119);
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
            this.buttonDisconnectIP.Location = new System.Drawing.Point(89, 52);
            this.buttonDisconnectIP.Name = "buttonDisconnectIP";
            this.buttonDisconnectIP.Size = new System.Drawing.Size(73, 23);
            this.buttonDisconnectIP.TabIndex = 38;
            this.buttonDisconnectIP.Text = "Disconnect";
            this.buttonDisconnectIP.UseVisualStyleBackColor = false;
            this.buttonDisconnectIP.Click += new System.EventHandler(this.buttonDisconnectIP_Click);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(29, 26);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(129, 20);
            this.textBoxIP.TabIndex = 37;
            this.textBoxIP.Text = "192.168.";
            // 
            // buttonConnectToIP
            // 
            this.buttonConnectToIP.BackColor = System.Drawing.Color.White;
            this.buttonConnectToIP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnectToIP.ForeColor = System.Drawing.Color.Black;
            this.buttonConnectToIP.Location = new System.Drawing.Point(8, 52);
            this.buttonConnectToIP.Name = "buttonConnectToIP";
            this.buttonConnectToIP.Size = new System.Drawing.Size(73, 23);
            this.buttonConnectToIP.TabIndex = 35;
            this.buttonConnectToIP.Text = "Connect";
            this.buttonConnectToIP.UseVisualStyleBackColor = false;
            this.buttonConnectToIP.Click += new System.EventHandler(this.buttonConnectToIP_Click);
            // 
            // groupBoxRebootMenu
            // 
            this.groupBoxRebootMenu.Controls.Add(this.buttonRS);
            this.groupBoxRebootMenu.Controls.Add(this.buttonRR);
            this.groupBoxRebootMenu.Controls.Add(this.buttonRF);
            this.groupBoxRebootMenu.Location = new System.Drawing.Point(6, 170);
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
            this.tabPageFastboot.Size = new System.Drawing.Size(893, 295);
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
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.textBoxDirImg);
            this.groupBox4.Controls.Add(this.radioButtonRom);
            this.groupBox4.Controls.Add(this.buttonSearchFileFastboot);
            this.groupBox4.Controls.Add(this.radioButtonCache);
            this.groupBox4.Controls.Add(this.buttonFlashImg);
            this.groupBox4.Controls.Add(this.radioButtonVendor);
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
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(124, 123);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 49;
            this.label10.Text = "Flash .Img";
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
            this.buttonFlashImg.ForeColor = System.Drawing.Color.Transparent;
            this.buttonFlashImg.Image = global::ATA_GUI.Properties.Resources.icons8_flash_on_48;
            this.buttonFlashImg.Location = new System.Drawing.Point(114, 140);
            this.buttonFlashImg.Name = "buttonFlashImg";
            this.buttonFlashImg.Size = new System.Drawing.Size(69, 63);
            this.buttonFlashImg.TabIndex = 38;
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
            this.tabPageRecovery.Controls.Add(this.pictureBoxSearchFile);
            this.tabPageRecovery.Controls.Add(this.label6);
            this.tabPageRecovery.Controls.Add(this.buttonInstallZip);
            this.tabPageRecovery.Controls.Add(this.textBoxDirFile);
            this.tabPageRecovery.Location = new System.Drawing.Point(4, 22);
            this.tabPageRecovery.Name = "tabPageRecovery";
            this.tabPageRecovery.Size = new System.Drawing.Size(893, 295);
            this.tabPageRecovery.TabIndex = 2;
            this.tabPageRecovery.Text = "Recovery";
            this.tabPageRecovery.UseVisualStyleBackColor = true;
            // 
            // pictureBoxSearchFile
            // 
            this.pictureBoxSearchFile.Image = global::ATA_GUI.Properties.Resources.icons8_folder_48;
            this.pictureBoxSearchFile.Location = new System.Drawing.Point(533, 106);
            this.pictureBoxSearchFile.Name = "pictureBoxSearchFile";
            this.pictureBoxSearchFile.Size = new System.Drawing.Size(28, 20);
            this.pictureBoxSearchFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSearchFile.TabIndex = 36;
            this.pictureBoxSearchFile.TabStop = false;
            this.pictureBoxSearchFile.Click += new System.EventHandler(this.pictureBoxSearchFile_Click);
            this.pictureBoxSearchFile.MouseEnter += new System.EventHandler(this.pictureBoxSearchFile_MouseEnter);
            this.pictureBoxSearchFile.MouseLeave += new System.EventHandler(this.pictureBoxSearchFile_MouseLeave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Red;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(282, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(279, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "YOUR SMARTPHONE MUST BE IN RECOVERY MODE";
            // 
            // buttonInstallZip
            // 
            this.buttonInstallZip.BackColor = System.Drawing.Color.White;
            this.buttonInstallZip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonInstallZip.Location = new System.Drawing.Point(372, 153);
            this.buttonInstallZip.Name = "buttonInstallZip";
            this.buttonInstallZip.Size = new System.Drawing.Size(87, 23);
            this.buttonInstallZip.TabIndex = 34;
            this.buttonInstallZip.Text = "Install";
            this.buttonInstallZip.UseVisualStyleBackColor = false;
            this.buttonInstallZip.Click += new System.EventHandler(this.buttonInstallZip_Click);
            // 
            // textBoxDirFile
            // 
            this.textBoxDirFile.Location = new System.Drawing.Point(284, 106);
            this.textBoxDirFile.Name = "textBoxDirFile";
            this.textBoxDirFile.Size = new System.Drawing.Size(245, 20);
            this.textBoxDirFile.TabIndex = 33;
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
            this.richTextBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLog.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBoxLog.Location = new System.Drawing.Point(516, 390);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(393, 64);
            this.richTextBoxLog.TabIndex = 38;
            this.richTextBoxLog.TabStop = false;
            this.richTextBoxLog.Text = "";
            // 
            // systemAppToolStripMenuItem
            // 
            this.systemAppToolStripMenuItem.Name = "systemAppToolStripMenuItem";
            this.systemAppToolStripMenuItem.ShowShortcutKeys = false;
            this.systemAppToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.systemAppToolStripMenuItem.Text = "System App";
            // 
            // nonSystemAppToolStripMenuItem
            // 
            this.nonSystemAppToolStripMenuItem.Name = "nonSystemAppToolStripMenuItem";
            this.nonSystemAppToolStripMenuItem.Size = new System.Drawing.Size(180, 23);
            this.nonSystemAppToolStripMenuItem.Text = "Non System App";
            // 
            // filterByToolStripMenuItem
            // 
            this.filterByToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemAppToolStripMenuItem,
            this.nonSystemAppToolStripMenuItem});
            this.filterByToolStripMenuItem.Name = "filterByToolStripMenuItem";
            this.filterByToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.filterByToolStripMenuItem.Text = "Filter By";
            // 
            // contextMenuStripFilterBy
            // 
            this.contextMenuStripFilterBy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem,
            this.systemAppToolStripMenuItem1,
            this.nonSystemAppToolStripMenuItem1,
            this.uninstalledAppToolStripMenuItem,
            this.disabledAppToolStripMenuItem});
            this.contextMenuStripFilterBy.Name = "contextMenuStripFilterBy";
            this.contextMenuStripFilterBy.Size = new System.Drawing.Size(181, 136);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // systemAppToolStripMenuItem1
            // 
            this.systemAppToolStripMenuItem1.Name = "systemAppToolStripMenuItem1";
            this.systemAppToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.systemAppToolStripMenuItem1.Text = "System App";
            this.systemAppToolStripMenuItem1.Click += new System.EventHandler(this.systemAppToolStripMenuItem1_Click);
            // 
            // nonSystemAppToolStripMenuItem1
            // 
            this.nonSystemAppToolStripMenuItem1.Name = "nonSystemAppToolStripMenuItem1";
            this.nonSystemAppToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.nonSystemAppToolStripMenuItem1.Text = "Non System App";
            this.nonSystemAppToolStripMenuItem1.Click += new System.EventHandler(this.nonSystemAppToolStripMenuItem1_Click);
            // 
            // uninstalledAppToolStripMenuItem
            // 
            this.uninstalledAppToolStripMenuItem.Name = "uninstalledAppToolStripMenuItem";
            this.uninstalledAppToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.uninstalledAppToolStripMenuItem.Text = "Uninstalled App";
            this.uninstalledAppToolStripMenuItem.Click += new System.EventHandler(this.uninstalledAppToolStripMenuItem_Click);
            // 
            // disabledAppToolStripMenuItem
            // 
            this.disabledAppToolStripMenuItem.Name = "disabledAppToolStripMenuItem";
            this.disabledAppToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.disabledAppToolStripMenuItem.Text = "Disabled App";
            this.disabledAppToolStripMenuItem.Visible = false;
            this.disabledAppToolStripMenuItem.Click += new System.EventHandler(this.disabledAppToolStripMenuItem_Click);
            // 
            // contextMenuStripPermissionMenu
            // 
            this.contextMenuStripPermissionMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grantWriteSecureSettingsToolStripMenuItem,
            this.grantDUMPToolStripMenuItem,
            this.checkGrantedPermissionsToolStripMenuItem});
            this.contextMenuStripPermissionMenu.Name = "contextMenuStripPermissionMenu";
            this.contextMenuStripPermissionMenu.Size = new System.Drawing.Size(265, 70);
            // 
            // grantWriteSecureSettingsToolStripMenuItem
            // 
            this.grantWriteSecureSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grantWriteSecureSettingsToolStripMenuItem1,
            this.revokeWriteSecureSettingsToolStripMenuItem});
            this.grantWriteSecureSettingsToolStripMenuItem.Name = "grantWriteSecureSettingsToolStripMenuItem";
            this.grantWriteSecureSettingsToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.grantWriteSecureSettingsToolStripMenuItem.Text = "Grant/Revoke Write_Secure_Settings";
            // 
            // grantWriteSecureSettingsToolStripMenuItem1
            // 
            this.grantWriteSecureSettingsToolStripMenuItem1.Name = "grantWriteSecureSettingsToolStripMenuItem1";
            this.grantWriteSecureSettingsToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.grantWriteSecureSettingsToolStripMenuItem1.Text = "Grant Write_Secure_Settings";
            this.grantWriteSecureSettingsToolStripMenuItem1.Click += new System.EventHandler(this.grantWriteSecureSettingsToolStripMenuItem_Click);
            // 
            // revokeWriteSecureSettingsToolStripMenuItem
            // 
            this.revokeWriteSecureSettingsToolStripMenuItem.Name = "revokeWriteSecureSettingsToolStripMenuItem";
            this.revokeWriteSecureSettingsToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.revokeWriteSecureSettingsToolStripMenuItem.Text = "Revoke Write_Secure_Settings";
            this.revokeWriteSecureSettingsToolStripMenuItem.Click += new System.EventHandler(this.revokeWriteSecureSettingsToolStripMenuItem_Click);
            // 
            // grantDUMPToolStripMenuItem
            // 
            this.grantDUMPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grantDUMPToolStripMenuItem1,
            this.revokeDUMPToolStripMenuItem});
            this.grantDUMPToolStripMenuItem.Name = "grantDUMPToolStripMenuItem";
            this.grantDUMPToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.grantDUMPToolStripMenuItem.Text = "Grant/Revoke DUMP";
            // 
            // grantDUMPToolStripMenuItem1
            // 
            this.grantDUMPToolStripMenuItem1.Name = "grantDUMPToolStripMenuItem1";
            this.grantDUMPToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.grantDUMPToolStripMenuItem1.Text = "Grant DUMP";
            this.grantDUMPToolStripMenuItem1.Click += new System.EventHandler(this.grantDUMPToolStripMenuItem_Click);
            // 
            // revokeDUMPToolStripMenuItem
            // 
            this.revokeDUMPToolStripMenuItem.Name = "revokeDUMPToolStripMenuItem";
            this.revokeDUMPToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.revokeDUMPToolStripMenuItem.Text = "Revoke DUMP";
            this.revokeDUMPToolStripMenuItem.Click += new System.EventHandler(this.revokeDUMPToolStripMenuItem_Click);
            // 
            // checkGrantedPermissionsToolStripMenuItem
            // 
            this.checkGrantedPermissionsToolStripMenuItem.Image = global::ATA_GUI.Properties.Resources.icons8_privacy_policy_48;
            this.checkGrantedPermissionsToolStripMenuItem.Name = "checkGrantedPermissionsToolStripMenuItem";
            this.checkGrantedPermissionsToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.checkGrantedPermissionsToolStripMenuItem.Text = "Check granted permissions";
            this.checkGrantedPermissionsToolStripMenuItem.Click += new System.EventHandler(this.checkGrantedPermissionsToolStripMenuItem_Click);
            // 
            // backgroundWorkerAdbDownloader
            // 
            this.backgroundWorkerAdbDownloader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerAdbDownloader_DoWork);
            // 
            // backgroundWorkerExeDownloader
            // 
            this.backgroundWorkerExeDownloader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerExeDownloader_DoWork);
            // 
            // buttonMobileScreenShare
            // 
            this.buttonMobileScreenShare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMobileScreenShare.ForeColor = System.Drawing.Color.Transparent;
            this.buttonMobileScreenShare.Image = global::ATA_GUI.Properties.Resources.icons8_screensharing_48;
            this.buttonMobileScreenShare.Location = new System.Drawing.Point(351, 391);
            this.buttonMobileScreenShare.Name = "buttonMobileScreenShare";
            this.buttonMobileScreenShare.Size = new System.Drawing.Size(60, 63);
            this.buttonMobileScreenShare.TabIndex = 39;
            this.buttonMobileScreenShare.UseVisualStyleBackColor = true;
            this.buttonMobileScreenShare.Click += new System.EventHandler(this.buttonMobileScreenShare_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(345, 376);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "Screen Share";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = global::ATA_GUI.Properties.Resources.logo;
            this.pictureBoxLogo.Location = new System.Drawing.Point(8, 9);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(79, 33);
            this.pictureBoxLogo.TabIndex = 3;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.Click += new System.EventHandler(this.pictureBoxLogo_Click);
            // 
            // panelTopBar
            // 
            this.panelTopBar.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelTopBar.Controls.Add(this.labelTools);
            this.panelTopBar.Controls.Add(this.pictureBoxClose);
            this.panelTopBar.Controls.Add(this.pictureBoxMinimize);
            this.panelTopBar.Controls.Add(this.labelHelp);
            this.panelTopBar.Controls.Add(this.labelSettings);
            this.panelTopBar.Controls.Add(this.pictureBoxLogo);
            this.panelTopBar.Location = new System.Drawing.Point(0, -3);
            this.panelTopBar.Name = "panelTopBar";
            this.panelTopBar.Size = new System.Drawing.Size(919, 44);
            this.panelTopBar.TabIndex = 0;
            this.panelTopBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelTopBar_MouseMove);
            // 
            // labelTools
            // 
            this.labelTools.AutoSize = true;
            this.labelTools.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTools.ForeColor = System.Drawing.Color.White;
            this.labelTools.Location = new System.Drawing.Point(212, 13);
            this.labelTools.Name = "labelTools";
            this.labelTools.Size = new System.Drawing.Size(37, 15);
            this.labelTools.TabIndex = 42;
            this.labelTools.Text = "Tools";
            this.labelTools.Click += new System.EventHandler(this.labelTools_Click);
            this.labelTools.MouseEnter += new System.EventHandler(this.labelTools_MouseEnter);
            this.labelTools.MouseLeave += new System.EventHandler(this.labelTools_MouseLeave);
            // 
            // pictureBoxClose
            // 
            this.pictureBoxClose.Image = global::ATA_GUI.Properties.Resources.icons8_close_window_48;
            this.pictureBoxClose.Location = new System.Drawing.Point(870, 2);
            this.pictureBoxClose.Name = "pictureBoxClose";
            this.pictureBoxClose.Size = new System.Drawing.Size(46, 42);
            this.pictureBoxClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxClose.TabIndex = 41;
            this.pictureBoxClose.TabStop = false;
            this.pictureBoxClose.Click += new System.EventHandler(this.pictureBoxClose_Click);
            this.pictureBoxClose.MouseEnter += new System.EventHandler(this.pictureBoxClose_MouseEnter);
            this.pictureBoxClose.MouseLeave += new System.EventHandler(this.pictureBoxClose_MouseLeave);
            // 
            // pictureBoxMinimize
            // 
            this.pictureBoxMinimize.Image = global::ATA_GUI.Properties.Resources._;
            this.pictureBoxMinimize.Location = new System.Drawing.Point(828, 3);
            this.pictureBoxMinimize.Name = "pictureBoxMinimize";
            this.pictureBoxMinimize.Size = new System.Drawing.Size(43, 41);
            this.pictureBoxMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxMinimize.TabIndex = 6;
            this.pictureBoxMinimize.TabStop = false;
            this.pictureBoxMinimize.Click += new System.EventHandler(this.pictureBoxMinimize_Click);
            this.pictureBoxMinimize.MouseEnter += new System.EventHandler(this.pictureBoxMinimize_MouseEnter);
            this.pictureBoxMinimize.MouseLeave += new System.EventHandler(this.pictureBoxMinimize_MouseLeave);
            // 
            // labelHelp
            // 
            this.labelHelp.AutoSize = true;
            this.labelHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHelp.ForeColor = System.Drawing.Color.White;
            this.labelHelp.Location = new System.Drawing.Point(166, 13);
            this.labelHelp.Name = "labelHelp";
            this.labelHelp.Size = new System.Drawing.Size(33, 15);
            this.labelHelp.TabIndex = 5;
            this.labelHelp.Text = "Help";
            this.labelHelp.Click += new System.EventHandler(this.labelHelp_Click);
            this.labelHelp.MouseEnter += new System.EventHandler(this.labelHelp_MouseEnter);
            this.labelHelp.MouseLeave += new System.EventHandler(this.labelHelp_MouseLeave);
            // 
            // labelSettings
            // 
            this.labelSettings.AutoSize = true;
            this.labelSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSettings.ForeColor = System.Drawing.Color.White;
            this.labelSettings.Location = new System.Drawing.Point(103, 13);
            this.labelSettings.Name = "labelSettings";
            this.labelSettings.Size = new System.Drawing.Size(51, 15);
            this.labelSettings.TabIndex = 4;
            this.labelSettings.Text = "Settings";
            this.labelSettings.Click += new System.EventHandler(this.labelSettings_Click);
            this.labelSettings.MouseEnter += new System.EventHandler(this.labelSettings_MouseEnter);
            this.labelSettings.MouseLeave += new System.EventHandler(this.labelSettings_MouseLeave);
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(150, 150);
            // 
            // contextMenuStripHelp
            // 
            this.contextMenuStripHelp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.submitFeedbackToolStripMenuItem});
            this.contextMenuStripHelp.Name = "contextMenuStripHelp";
            this.contextMenuStripHelp.Size = new System.Drawing.Size(166, 48);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.videoTutorialToolStripMenuItem});
            this.helpToolStripMenuItem.Image = global::ATA_GUI.Properties.Resources.icons8_help_48;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.helpToolStripMenuItem.Text = "Open help";
            // 
            // videoTutorialToolStripMenuItem
            // 
            this.videoTutorialToolStripMenuItem.Name = "videoTutorialToolStripMenuItem";
            this.videoTutorialToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.videoTutorialToolStripMenuItem.Text = "Video Tutorial";
            this.videoTutorialToolStripMenuItem.Click += new System.EventHandler(this.videoTutorialToolStripMenuItem_Click);
            // 
            // submitFeedbackToolStripMenuItem
            // 
            this.submitFeedbackToolStripMenuItem.Image = global::ATA_GUI.Properties.Resources.icons8_get_help_48;
            this.submitFeedbackToolStripMenuItem.Name = "submitFeedbackToolStripMenuItem";
            this.submitFeedbackToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.submitFeedbackToolStripMenuItem.Text = "Submit Feedback";
            this.submitFeedbackToolStripMenuItem.Click += new System.EventHandler(this.submitFeedbackToolStripMenuItem_Click);
            // 
            // videoTutorialsToolStripMenuItem
            // 
            this.videoTutorialsToolStripMenuItem.Name = "videoTutorialsToolStripMenuItem";
            this.videoTutorialsToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // buttonDeviceLogs
            // 
            this.buttonDeviceLogs.BackColor = System.Drawing.Color.White;
            this.buttonDeviceLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDeviceLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeviceLogs.ForeColor = System.Drawing.Color.Black;
            this.buttonDeviceLogs.Location = new System.Drawing.Point(423, 402);
            this.buttonDeviceLogs.Name = "buttonDeviceLogs";
            this.buttonDeviceLogs.Size = new System.Drawing.Size(87, 23);
            this.buttonDeviceLogs.TabIndex = 41;
            this.buttonDeviceLogs.Text = "Device Logs";
            this.buttonDeviceLogs.UseVisualStyleBackColor = false;
            this.buttonDeviceLogs.Click += new System.EventHandler(this.buttonDeviceLogs_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonReloadDevicesList);
            this.groupBox1.Controls.Add(this.comboBoxDevices);
            this.groupBox1.Controls.Add(this.buttonSyncApp);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(7, 369);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 84);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device Sync Settings";
            // 
            // buttonReloadDevicesList
            // 
            this.buttonReloadDevicesList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReloadDevicesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReloadDevicesList.ForeColor = System.Drawing.Color.Black;
            this.buttonReloadDevicesList.Location = new System.Drawing.Point(172, 48);
            this.buttonReloadDevicesList.Name = "buttonReloadDevicesList";
            this.buttonReloadDevicesList.Size = new System.Drawing.Size(152, 23);
            this.buttonReloadDevicesList.TabIndex = 2;
            this.buttonReloadDevicesList.Text = "Refresh Device List";
            this.buttonReloadDevicesList.UseVisualStyleBackColor = true;
            this.buttonReloadDevicesList.Click += new System.EventHandler(this.buttonReloadDevicesList_Click);
            // 
            // comboBoxDevices
            // 
            this.comboBoxDevices.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxDevices.FormattingEnabled = true;
            this.comboBoxDevices.Location = new System.Drawing.Point(172, 26);
            this.comboBoxDevices.Name = "comboBoxDevices";
            this.comboBoxDevices.Size = new System.Drawing.Size(152, 21);
            this.comboBoxDevices.TabIndex = 1;
            this.comboBoxDevices.SelectedIndexChanged += new System.EventHandler(this.comboBoxDevices_SelectedIndexChanged);
            // 
            // buttonSyncApp
            // 
            this.buttonSyncApp.BackColor = System.Drawing.Color.White;
            this.buttonSyncApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSyncApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F);
            this.buttonSyncApp.ForeColor = System.Drawing.Color.Black;
            this.buttonSyncApp.Location = new System.Drawing.Point(17, 26);
            this.buttonSyncApp.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSyncApp.Name = "buttonSyncApp";
            this.buttonSyncApp.Size = new System.Drawing.Size(131, 45);
            this.buttonSyncApp.TabIndex = 0;
            this.buttonSyncApp.Text = "Sync Smartphone";
            this.buttonSyncApp.UseVisualStyleBackColor = false;
            this.buttonSyncApp.Click += new System.EventHandler(this.buttonSyncApp_Click);
            // 
            // contextMenuStripSearch
            // 
            this.contextMenuStripSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duckduckgoToolStripMenuItem,
            this.googleToolStripMenuItem,
            this.playMarketToolStripMenuItem,
            this.APKMirrorToolStripMenuItem,
            this.fDroidToolStripMenuItem});
            this.contextMenuStripSearch.Name = "contextMenuStripSearch";
            this.contextMenuStripSearch.Size = new System.Drawing.Size(142, 114);
            // 
            // duckduckgoToolStripMenuItem
            // 
            this.duckduckgoToolStripMenuItem.Image = global::ATA_GUI.Properties.Resources.icons8_duckduckgo_48;
            this.duckduckgoToolStripMenuItem.Name = "duckduckgoToolStripMenuItem";
            this.duckduckgoToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.duckduckgoToolStripMenuItem.Text = "Duckduckgo";
            this.duckduckgoToolStripMenuItem.Click += new System.EventHandler(this.duckduckgoToolStripMenuItem_Click);
            // 
            // googleToolStripMenuItem
            // 
            this.googleToolStripMenuItem.Image = global::ATA_GUI.Properties.Resources.icons8_google_48;
            this.googleToolStripMenuItem.Name = "googleToolStripMenuItem";
            this.googleToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.googleToolStripMenuItem.Text = "Google";
            this.googleToolStripMenuItem.Click += new System.EventHandler(this.googleToolStripMenuItem_Click);
            // 
            // playMarketToolStripMenuItem
            // 
            this.playMarketToolStripMenuItem.Image = global::ATA_GUI.Properties.Resources.playmarketicon;
            this.playMarketToolStripMenuItem.Name = "playMarketToolStripMenuItem";
            this.playMarketToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.playMarketToolStripMenuItem.Text = "Play Market";
            this.playMarketToolStripMenuItem.Click += new System.EventHandler(this.playMarketToolStripMenuItem_Click);
            // 
            // APKMirrorToolStripMenuItem
            // 
            this.APKMirrorToolStripMenuItem.Image = global::ATA_GUI.Properties.Resources.apkmirror;
            this.APKMirrorToolStripMenuItem.Name = "APKMirrorToolStripMenuItem";
            this.APKMirrorToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.APKMirrorToolStripMenuItem.Text = "APK Mirror";
            this.APKMirrorToolStripMenuItem.Click += new System.EventHandler(this.APKMirrorToolStripMenuItem_Click);
            // 
            // fDroidToolStripMenuItem
            // 
            this.fDroidToolStripMenuItem.Image = global::ATA_GUI.Properties.Resources.fdroidlogo;
            this.fDroidToolStripMenuItem.Name = "fDroidToolStripMenuItem";
            this.fDroidToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.fDroidToolStripMenuItem.Text = "F-Droid";
            this.fDroidToolStripMenuItem.Click += new System.EventHandler(this.fDroidToolStripMenuItem_Click);
            // 
            // contextMenuStripTools
            // 
            this.contextMenuStripTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStripTools.Name = "contextMenuStripHelp";
            this.contextMenuStripTools.Size = new System.Drawing.Size(98, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemADBKill});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem1.Text = "ADB";
            // 
            // toolStripMenuItemADBKill
            // 
            this.toolStripMenuItemADBKill.Name = "toolStripMenuItemADBKill";
            this.toolStripMenuItemADBKill.Size = new System.Drawing.Size(116, 22);
            this.toolStripMenuItemADBKill.Text = "Kill ADB";
            this.toolStripMenuItemADBKill.Click += new System.EventHandler(this.toolStripMenuItemADBKill_Click);
            // 
            // buttonTaskManager
            // 
            this.buttonTaskManager.BackColor = System.Drawing.Color.White;
            this.buttonTaskManager.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTaskManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTaskManager.ForeColor = System.Drawing.Color.Black;
            this.buttonTaskManager.Location = new System.Drawing.Point(423, 373);
            this.buttonTaskManager.Name = "buttonTaskManager";
            this.buttonTaskManager.Size = new System.Drawing.Size(87, 23);
            this.buttonTaskManager.TabIndex = 42;
            this.buttonTaskManager.Text = "Task Manager";
            this.buttonTaskManager.UseVisualStyleBackColor = false;
            this.buttonTaskManager.Click += new System.EventHandler(this.buttonTaskManager_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(914, 457);
            this.Controls.Add(this.buttonTaskManager);
            this.Controls.Add(this.buttonDeviceLogs);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.buttonMobileScreenShare);
            this.Controls.Add(this.buttonLogClear);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.panelTopBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ATA-GUI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.groupBoxDeviceInfo.ResumeLayout(false);
            this.groupBoxDeviceInfo.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSystem.ResumeLayout(false);
            this.groupBoxAPKMenu.ResumeLayout(false);
            this.groupBoxAPKMenu.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBoxADBNet.ResumeLayout(false);
            this.groupBoxADBNet.PerformLayout();
            this.groupBoxRebootMenu.ResumeLayout(false);
            this.tabPageFastboot.ResumeLayout(false);
            this.panelFastboot.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPageRecovery.ResumeLayout(false);
            this.tabPageRecovery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearchFile)).EndInit();
            this.contextMenuStripFilterBy.ResumeLayout(false);
            this.contextMenuStripPermissionMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelTopBar.ResumeLayout(false);
            this.panelTopBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMinimize)).EndInit();
            this.contextMenuStripHelp.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStripSearch.ResumeLayout(false);
            this.contextMenuStripTools.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelLog;
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
        private System.Windows.Forms.GroupBox groupBoxAPKMenu;
        private System.Windows.Forms.OpenFileDialog openFileDialogAPK;
        private System.Windows.Forms.CheckBox checkBoxSelectAll;
        private System.Windows.Forms.CheckedListBox checkedListBoxApp;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxADBNet;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Button buttonConnectToIP;
        private System.Windows.Forms.Button buttonDisconnectIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonInstallZip;
        private System.Windows.Forms.TextBox textBoxDirFile;
        private System.Windows.Forms.OpenFileDialog openFileDialogZip;
        private System.ComponentModel.BackgroundWorker backgroundWorkerZip;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxDirImg;
        private System.Windows.Forms.RadioButton radioButtonRom;
        private System.Windows.Forms.RadioButton radioButtonCache;
        private System.Windows.Forms.Button buttonFlashImg;
        private System.Windows.Forms.RadioButton radioButtonVendor;
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
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonReloadApps;
        private System.Windows.Forms.ToolStripButton toolStripButtonFilter;
        private System.Windows.Forms.ToolStripTextBox textBoxSearch;
        private System.Windows.Forms.ToolStripMenuItem systemAppToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox nonSystemAppToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterByToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonInstallApp;
        private System.Windows.Forms.ToolStripButton toolStripButtonUninstallApp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonPackageManager;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFilterBy;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemAppToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem nonSystemAppToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonPermissionMenu;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labelSelectedAppCount;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPermissionMenu;
        private System.Windows.Forms.ToolStripMenuItem grantWriteSecureSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grantDUMPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkGrantedPermissionsToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.ComponentModel.BackgroundWorker backgroundWorkerAdbDownloader;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTotalApps;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton toolStripButtonBloatwareDetecter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton toolStripButtonRestoreApp;
        private System.Windows.Forms.ToolStripMenuItem uninstalledAppToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorkerExeDownloader;
        private System.Windows.Forms.Button buttonMobileScreenShare;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Panel panelTopBar;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.Label labelHelp;
        private System.Windows.Forms.Label labelSettings;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripHelp;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem submitFeedbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videoTutorialsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videoTutorialToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBoxMinimize;
        private System.Windows.Forms.PictureBox pictureBoxClose;
        private System.Windows.Forms.Button buttonDeviceLogs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonReloadDevicesList;
        private System.Windows.Forms.ComboBox comboBoxDevices;
        private System.Windows.Forms.Button buttonSyncApp;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox pictureBoxSearchFile;
        private System.Windows.Forms.Button buttonSearchFileFastboot;
        private System.Windows.Forms.ToolStripMenuItem grantWriteSecureSettingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem revokeWriteSecureSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grantDUMPToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem revokeDUMPToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton toolStripButtonSearch;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSearch;
        private System.Windows.Forms.ToolStripMenuItem duckduckgoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem googleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playMarketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem APKMirrorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fDroidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disabledAppToolStripMenuItem;
        private System.Windows.Forms.Label labelTools;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTools;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemADBKill;
        private System.Windows.Forms.Button buttonTaskManager;
    }
}

