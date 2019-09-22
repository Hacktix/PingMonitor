// Decompiled with JetBrains decompiler
// Type: PingMonitor.AddDeviceForm
// Assembly: PingMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C7F21AE2-E822-440B-97D1-7ECAE911FD7D
// Assembly location: G:\Projects\PingMonitor\PingMonitor\bin\Debug\PingMonitor.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PingMonitor
{
  public class AddDeviceForm : Form
  {
    private static string lastError = "";
    private Color onlineColor = Color.FromArgb(91, 245, 68);
    private Color offlineColor = Color.FromArgb(235, 19, 19);
    private Color arpErrorColor = Color.FromArgb((int) byte.MaxValue, 81, 0);
    public Device AddDevice = (Device) null;
    private IContainer components = (IContainer) null;
    private Label titleLabel;
    private GroupBox basicBox;
    private TextBox nameBox;
    private Label nameLabel;
    private Button closeButton;
    private Label ipLabel;
    private TextBox ipBox;
    private TextBox descBox;
    private Label descLabel;
    private Label timeoutLabel;
    private Label timeoutMsLabel;
    private NumericUpDown timeoutBox;
    private Label intervalMsLabel;
    private NumericUpDown intervalBox;
    private Label intervalLabel;
    private GroupBox advancedBox;
    private Label arpIntervalMsLabel;
    private TextBox macBox;
    private NumericUpDown arpIntervalBox;
    private Label macLabel;
    private Label arpIntervalLabel;
    private CheckBox arpCheckbox;
    private Button addButton;
    private GroupBox designBox;
    private PictureBox offlineBox;
    private PictureBox onlineBox;
    private Label offlineLabel;
    private Label onlineLabel;
    private Button changeOfflineButton;
    private Button changeOnlineButton;
    private ColorDialog colorDialog;
    private Label macInfoLabel;
    private Button changeArpErrorButton;
    private PictureBox arpErrorBox;
    private Label arpErrorLabel;

    public AddDeviceForm()
    {
      this.InitializeComponent();
    }

    private void onClose(object sender, EventArgs e)
    {
      if (MessageBox.Show((IWin32Window) this, "Are you sure that you want to close? Any entered values will be lost.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      for (int index = 0; index < 10; ++index)
      {
        this.Opacity = this.Opacity - 0.1;
        Thread.Sleep(10);
      }
      this.Close();
    }

    private void onLoad(object sender, EventArgs e)
    {
      this.Opacity = 0.0;
    }

    private void onShow(object sender, EventArgs e)
    {
      this.drawStatusIcons();
      for (int index = 0; index < 10; ++index)
      {
        this.Opacity = this.Opacity + 0.1;
        Thread.Sleep(10);
      }
    }

    private void changeArpEnabled(object sender, EventArgs e)
    {
      this.macLabel.Enabled = !this.macLabel.Enabled;
      this.macBox.Enabled = !this.macBox.Enabled;
      this.macInfoLabel.Enabled = !this.macInfoLabel.Enabled;
      this.arpIntervalLabel.Enabled = !this.arpIntervalLabel.Enabled;
      this.arpIntervalBox.Enabled = !this.arpIntervalBox.Enabled;
      this.arpIntervalMsLabel.Enabled = !this.arpIntervalMsLabel.Enabled;
      this.arpErrorBox.Enabled = !this.arpErrorBox.Enabled;
      this.arpErrorLabel.Enabled = !this.arpErrorLabel.Enabled;
      this.changeArpErrorButton.Enabled = !this.changeArpErrorButton.Enabled;
    }

    private void drawStatusIcons()
    {
      Image image1 = (Image) new Bitmap(25, 25);
      using (Graphics graphics = Graphics.FromImage(image1))
        graphics.FillEllipse((Brush) new SolidBrush(this.onlineColor), 5, 5, 15, 15);
      this.onlineBox.Image = image1;
      Image image2 = (Image) new Bitmap(25, 25);
      using (Graphics graphics = Graphics.FromImage(image2))
        graphics.FillEllipse((Brush) new SolidBrush(this.offlineColor), 5, 5, 15, 15);
      this.offlineBox.Image = image2;
      Image image3 = (Image) new Bitmap(25, 25);
      using (Graphics graphics = Graphics.FromImage(image3))
        graphics.FillEllipse((Brush) new SolidBrush(this.arpErrorColor), 5, 5, 15, 15);
      this.arpErrorBox.Image = image3;
    }

    private void onChangeOnlineColor(object sender, EventArgs e)
    {
      this.colorDialog.Color = this.onlineColor;
      if (this.colorDialog.ShowDialog() != DialogResult.OK)
        return;
      this.onlineColor = this.colorDialog.Color;
      this.drawStatusIcons();
    }

    private void onChangeOfflineColor(object sender, EventArgs e)
    {
      this.colorDialog.Color = this.offlineColor;
      if (this.colorDialog.ShowDialog() != DialogResult.OK)
        return;
      this.offlineColor = this.colorDialog.Color;
      this.drawStatusIcons();
    }

    private void onChangeArpError(object sender, EventArgs e)
    {
      this.colorDialog.Color = this.arpErrorColor;
      if (this.colorDialog.ShowDialog() != DialogResult.OK)
        return;
      this.arpErrorColor = this.colorDialog.Color;
      this.drawStatusIcons();
    }

    private bool validationError(string error)
    {
      AddDeviceForm.lastError = error;
      return false;
    }

    private bool checkValues()
    {
      if (this.nameBox.Text.Length < 1)
        return this.validationError("Please enter a device name!");
      try
      {
        IPAddress.Parse(this.ipBox.Text);
      }
      catch (Exception ex)
      {
        return this.validationError("Please enter a valid IP address!");
      }
      if (this.arpCheckbox.Checked && (this.macBox.Text.Length > 0 && !Regex.IsMatch(this.macBox.Text, "([0-9a-fA-F][0-9a-fA-F]:){5}([0-9a-fA-F][0-9a-fA-F])")))
        return this.validationError("Please enter a valid MAC address! (Format: XX:XX:XX:XX:XX:XX)");
      return true;
    }

    private void onAddDevice(object sender, EventArgs e)
    {
      if (!this.checkValues())
      {
        int num = (int) MessageBox.Show((IWin32Window) this, AddDeviceForm.lastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int lastX = MainForm.lastX;
        int lastY = MainForm.lastY;
        string text1 = this.nameBox.Text;
        string text2 = this.descBox.Text;
        IPAddress iP = IPAddress.Parse(this.ipBox.Text);
        int pingInterval = (int) this.intervalBox.Value;
        int pingTimeout = (int) this.timeoutBox.Value;
        bool flag = this.arpCheckbox.Checked;
        string mAC = flag ? this.macBox.Text : (string) null;
        int aRPInterval = flag ? (int) this.arpIntervalBox.Value : -1;
        this.AddDevice = flag ? new Device(lastX, lastY, this.onlineColor, this.offlineColor, this.arpErrorColor, text1, text2, iP, pingInterval, pingTimeout, mAC, aRPInterval) : new Device(lastX, lastY, this.onlineColor, this.offlineColor, this.arpErrorColor, text1, text2, iP, pingInterval, pingTimeout);
        for (int index = 0; index < 10; ++index)
        {
          this.Opacity = this.Opacity - 0.1;
          Thread.Sleep(10);
        }
        this.Close();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddDeviceForm));
      this.titleLabel = new Label();
      this.basicBox = new GroupBox();
      this.descBox = new TextBox();
      this.descLabel = new Label();
      this.timeoutLabel = new Label();
      this.timeoutMsLabel = new Label();
      this.timeoutBox = new NumericUpDown();
      this.intervalMsLabel = new Label();
      this.intervalBox = new NumericUpDown();
      this.intervalLabel = new Label();
      this.ipBox = new TextBox();
      this.ipLabel = new Label();
      this.nameBox = new TextBox();
      this.nameLabel = new Label();
      this.closeButton = new Button();
      this.advancedBox = new GroupBox();
      this.macInfoLabel = new Label();
      this.arpIntervalMsLabel = new Label();
      this.macBox = new TextBox();
      this.arpIntervalBox = new NumericUpDown();
      this.macLabel = new Label();
      this.arpIntervalLabel = new Label();
      this.arpCheckbox = new CheckBox();
      this.addButton = new Button();
      this.designBox = new GroupBox();
      this.changeOfflineButton = new Button();
      this.changeOnlineButton = new Button();
      this.offlineBox = new PictureBox();
      this.onlineBox = new PictureBox();
      this.offlineLabel = new Label();
      this.onlineLabel = new Label();
      this.colorDialog = new ColorDialog();
      this.arpErrorLabel = new Label();
      this.arpErrorBox = new PictureBox();
      this.changeArpErrorButton = new Button();
      this.basicBox.SuspendLayout();
      this.timeoutBox.BeginInit();
      this.intervalBox.BeginInit();
      this.advancedBox.SuspendLayout();
      this.arpIntervalBox.BeginInit();
      this.designBox.SuspendLayout();
      ((ISupportInitialize) this.offlineBox).BeginInit();
      ((ISupportInitialize) this.onlineBox).BeginInit();
      ((ISupportInitialize) this.arpErrorBox).BeginInit();
      this.SuspendLayout();
      this.titleLabel.AutoSize = true;
      this.titleLabel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.titleLabel.Location = new Point(12, 9);
      this.titleLabel.Name = "titleLabel";
      this.titleLabel.Size = new Size(100, 20);
      this.titleLabel.TabIndex = 1;
      this.titleLabel.Text = "Add Device";
      this.basicBox.Controls.Add((Control) this.descBox);
      this.basicBox.Controls.Add((Control) this.descLabel);
      this.basicBox.Controls.Add((Control) this.timeoutLabel);
      this.basicBox.Controls.Add((Control) this.timeoutMsLabel);
      this.basicBox.Controls.Add((Control) this.timeoutBox);
      this.basicBox.Controls.Add((Control) this.intervalMsLabel);
      this.basicBox.Controls.Add((Control) this.intervalBox);
      this.basicBox.Controls.Add((Control) this.intervalLabel);
      this.basicBox.Controls.Add((Control) this.ipBox);
      this.basicBox.Controls.Add((Control) this.ipLabel);
      this.basicBox.Controls.Add((Control) this.nameBox);
      this.basicBox.Controls.Add((Control) this.nameLabel);
      this.basicBox.ForeColor = Color.White;
      this.basicBox.Location = new Point(16, 41);
      this.basicBox.Name = "basicBox";
      this.basicBox.Size = new Size(612, 143);
      this.basicBox.TabIndex = 2;
      this.basicBox.TabStop = false;
      this.basicBox.Text = "Basic Options";
      this.descBox.BackColor = Color.FromArgb(20, 20, 20);
      this.descBox.BorderStyle = BorderStyle.FixedSingle;
      this.descBox.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.descBox.ForeColor = Color.White;
      this.descBox.Location = new Point(319, 53);
      this.descBox.Multiline = true;
      this.descBox.Name = "descBox";
      this.descBox.Size = new Size(274, 72);
      this.descBox.TabIndex = 11;
      this.descLabel.AutoSize = true;
      this.descLabel.Location = new Point(316, 29);
      this.descLabel.Name = "descLabel";
      this.descLabel.Size = new Size(100, 13);
      this.descLabel.TabIndex = 10;
      this.descLabel.Text = "Device Description:";
      this.timeoutLabel.AutoSize = true;
      this.timeoutLabel.Location = new Point(6, 107);
      this.timeoutLabel.Name = "timeoutLabel";
      this.timeoutLabel.Size = new Size(72, 13);
      this.timeoutLabel.TabIndex = 9;
      this.timeoutLabel.Text = "Ping Timeout:";
      this.timeoutMsLabel.AutoSize = true;
      this.timeoutMsLabel.Location = new Point(276, 107);
      this.timeoutMsLabel.Name = "timeoutMsLabel";
      this.timeoutMsLabel.Size = new Size(20, 13);
      this.timeoutMsLabel.TabIndex = 8;
      this.timeoutMsLabel.Text = "ms";
      this.timeoutBox.BackColor = Color.FromArgb(20, 20, 20);
      this.timeoutBox.BorderStyle = BorderStyle.FixedSingle;
      this.timeoutBox.ForeColor = Color.White;
      this.timeoutBox.Location = new Point(97, 105);
      this.timeoutBox.Maximum = new Decimal(new int[4]
      {
        10000,
        0,
        0,
        0
      });
      this.timeoutBox.Minimum = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      this.timeoutBox.Name = "timeoutBox";
      this.timeoutBox.Size = new Size(173, 20);
      this.timeoutBox.TabIndex = 7;
      this.timeoutBox.Value = new Decimal(new int[4]
      {
        50,
        0,
        0,
        0
      });
      this.intervalMsLabel.AutoSize = true;
      this.intervalMsLabel.Location = new Point(276, 81);
      this.intervalMsLabel.Name = "intervalMsLabel";
      this.intervalMsLabel.Size = new Size(20, 13);
      this.intervalMsLabel.TabIndex = 6;
      this.intervalMsLabel.Text = "ms";
      this.intervalBox.BackColor = Color.FromArgb(20, 20, 20);
      this.intervalBox.BorderStyle = BorderStyle.FixedSingle;
      this.intervalBox.ForeColor = Color.White;
      this.intervalBox.Location = new Point(97, 79);
      this.intervalBox.Maximum = new Decimal(new int[4]
      {
        10000,
        0,
        0,
        0
      });
      this.intervalBox.Minimum = new Decimal(new int[4]
      {
        500,
        0,
        0,
        0
      });
      this.intervalBox.Name = "intervalBox";
      this.intervalBox.Size = new Size(173, 20);
      this.intervalBox.TabIndex = 5;
      this.intervalBox.Value = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.intervalLabel.AutoSize = true;
      this.intervalLabel.Location = new Point(6, 81);
      this.intervalLabel.Name = "intervalLabel";
      this.intervalLabel.Size = new Size(69, 13);
      this.intervalLabel.TabIndex = 4;
      this.intervalLabel.Text = "Ping Interval:";
      this.ipBox.BackColor = Color.FromArgb(20, 20, 20);
      this.ipBox.BorderStyle = BorderStyle.FixedSingle;
      this.ipBox.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.ipBox.ForeColor = Color.White;
      this.ipBox.Location = new Point(97, 53);
      this.ipBox.Name = "ipBox";
      this.ipBox.Size = new Size(196, 20);
      this.ipBox.TabIndex = 3;
      this.ipLabel.AutoSize = true;
      this.ipLabel.Location = new Point(6, 55);
      this.ipLabel.Name = "ipLabel";
      this.ipLabel.Size = new Size(57, 13);
      this.ipLabel.TabIndex = 2;
      this.ipLabel.Text = "Device IP:";
      this.nameBox.BackColor = Color.FromArgb(20, 20, 20);
      this.nameBox.BorderStyle = BorderStyle.FixedSingle;
      this.nameBox.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.nameBox.ForeColor = Color.White;
      this.nameBox.Location = new Point(97, 27);
      this.nameBox.Name = "nameBox";
      this.nameBox.Size = new Size(196, 20);
      this.nameBox.TabIndex = 1;
      this.nameLabel.AutoSize = true;
      this.nameLabel.Location = new Point(6, 29);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new Size(75, 13);
      this.nameLabel.TabIndex = 0;
      this.nameLabel.Text = "Device Name:";
      this.closeButton.FlatStyle = FlatStyle.Flat;
      this.closeButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.closeButton.Location = new Point(603, 12);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new Size(25, 25);
      this.closeButton.TabIndex = 4;
      this.closeButton.Text = "X";
      this.closeButton.UseVisualStyleBackColor = true;
      this.closeButton.Click += new EventHandler(this.onClose);
      this.advancedBox.Controls.Add((Control) this.macInfoLabel);
      this.advancedBox.Controls.Add((Control) this.arpIntervalMsLabel);
      this.advancedBox.Controls.Add((Control) this.macBox);
      this.advancedBox.Controls.Add((Control) this.arpIntervalBox);
      this.advancedBox.Controls.Add((Control) this.macLabel);
      this.advancedBox.Controls.Add((Control) this.arpIntervalLabel);
      this.advancedBox.Controls.Add((Control) this.arpCheckbox);
      this.advancedBox.ForeColor = Color.White;
      this.advancedBox.Location = new Point(16, 205);
      this.advancedBox.Name = "advancedBox";
      this.advancedBox.Size = new Size(612, 107);
      this.advancedBox.TabIndex = 5;
      this.advancedBox.TabStop = false;
      this.advancedBox.Text = "Advanced Options";
      this.macInfoLabel.AutoSize = true;
      this.macInfoLabel.Enabled = false;
      this.macInfoLabel.Location = new Point(299, 50);
      this.macInfoLabel.Name = "macInfoLabel";
      this.macInfoLabel.Size = new Size(200, 13);
      this.macInfoLabel.TabIndex = 15;
      this.macInfoLabel.Text = "(leave empty for automatic determination)";
      this.arpIntervalMsLabel.AutoSize = true;
      this.arpIntervalMsLabel.Enabled = false;
      this.arpIntervalMsLabel.Location = new Point(276, 76);
      this.arpIntervalMsLabel.Name = "arpIntervalMsLabel";
      this.arpIntervalMsLabel.Size = new Size(20, 13);
      this.arpIntervalMsLabel.TabIndex = 14;
      this.arpIntervalMsLabel.Text = "ms";
      this.macBox.BackColor = Color.FromArgb(20, 20, 20);
      this.macBox.BorderStyle = BorderStyle.FixedSingle;
      this.macBox.Enabled = false;
      this.macBox.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.macBox.ForeColor = Color.White;
      this.macBox.Location = new Point(97, 48);
      this.macBox.Name = "macBox";
      this.macBox.Size = new Size(196, 20);
      this.macBox.TabIndex = 12;
      this.arpIntervalBox.BackColor = Color.FromArgb(20, 20, 20);
      this.arpIntervalBox.BorderStyle = BorderStyle.FixedSingle;
      this.arpIntervalBox.Enabled = false;
      this.arpIntervalBox.ForeColor = Color.White;
      this.arpIntervalBox.Location = new Point(97, 74);
      this.arpIntervalBox.Maximum = new Decimal(new int[4]
      {
        10000,
        0,
        0,
        0
      });
      this.arpIntervalBox.Minimum = new Decimal(new int[4]
      {
        500,
        0,
        0,
        0
      });
      this.arpIntervalBox.Name = "arpIntervalBox";
      this.arpIntervalBox.Size = new Size(173, 20);
      this.arpIntervalBox.TabIndex = 13;
      this.arpIntervalBox.Value = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.macLabel.AutoSize = true;
      this.macLabel.Enabled = false;
      this.macLabel.Location = new Point(7, 50);
      this.macLabel.Name = "macLabel";
      this.macLabel.Size = new Size(74, 13);
      this.macLabel.TabIndex = 12;
      this.macLabel.Text = "MAC Address:";
      this.arpIntervalLabel.AutoSize = true;
      this.arpIntervalLabel.Enabled = false;
      this.arpIntervalLabel.Location = new Point(6, 76);
      this.arpIntervalLabel.Name = "arpIntervalLabel";
      this.arpIntervalLabel.Size = new Size(70, 13);
      this.arpIntervalLabel.TabIndex = 12;
      this.arpIntervalLabel.Text = "ARP Interval:";
      this.arpCheckbox.AutoSize = true;
      this.arpCheckbox.Location = new Point(9, 25);
      this.arpCheckbox.Name = "arpCheckbox";
      this.arpCheckbox.Size = new Size(135, 17);
      this.arpCheckbox.TabIndex = 0;
      this.arpCheckbox.Text = "Perform ARP Requests";
      this.arpCheckbox.UseVisualStyleBackColor = true;
      this.arpCheckbox.CheckStateChanged += new EventHandler(this.changeArpEnabled);
      this.addButton.FlatStyle = FlatStyle.Flat;
      this.addButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.addButton.Location = new Point(16, 443);
      this.addButton.Name = "addButton";
      this.addButton.Size = new Size(612, 25);
      this.addButton.TabIndex = 6;
      this.addButton.Text = "Add Device";
      this.addButton.UseVisualStyleBackColor = true;
      this.addButton.Click += new EventHandler(this.onAddDevice);
      this.designBox.Controls.Add((Control) this.changeArpErrorButton);
      this.designBox.Controls.Add((Control) this.arpErrorBox);
      this.designBox.Controls.Add((Control) this.arpErrorLabel);
      this.designBox.Controls.Add((Control) this.changeOfflineButton);
      this.designBox.Controls.Add((Control) this.changeOnlineButton);
      this.designBox.Controls.Add((Control) this.offlineBox);
      this.designBox.Controls.Add((Control) this.onlineBox);
      this.designBox.Controls.Add((Control) this.offlineLabel);
      this.designBox.Controls.Add((Control) this.onlineLabel);
      this.designBox.ForeColor = Color.White;
      this.designBox.Location = new Point(16, 327);
      this.designBox.Name = "designBox";
      this.designBox.Size = new Size(612, 100);
      this.designBox.TabIndex = 7;
      this.designBox.TabStop = false;
      this.designBox.Text = "Design Options";
      this.changeOfflineButton.FlatStyle = FlatStyle.Flat;
      this.changeOfflineButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.changeOfflineButton.Location = new Point(157, 55);
      this.changeOfflineButton.Name = "changeOfflineButton";
      this.changeOfflineButton.Size = new Size(113, 25);
      this.changeOfflineButton.TabIndex = 19;
      this.changeOfflineButton.Text = "Change Color";
      this.changeOfflineButton.UseVisualStyleBackColor = true;
      this.changeOfflineButton.Click += new EventHandler(this.onChangeOfflineColor);
      this.changeOnlineButton.FlatStyle = FlatStyle.Flat;
      this.changeOnlineButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.changeOnlineButton.Location = new Point(157, 24);
      this.changeOnlineButton.Name = "changeOnlineButton";
      this.changeOnlineButton.Size = new Size(113, 25);
      this.changeOnlineButton.TabIndex = 8;
      this.changeOnlineButton.Text = "Change Color";
      this.changeOnlineButton.UseVisualStyleBackColor = true;
      this.changeOnlineButton.Click += new EventHandler(this.onChangeOnlineColor);
      this.offlineBox.Location = new Point(116, 55);
      this.offlineBox.Name = "offlineBox";
      this.offlineBox.Size = new Size(25, 25);
      this.offlineBox.TabIndex = 18;
      this.offlineBox.TabStop = false;
      this.onlineBox.Location = new Point(116, 24);
      this.onlineBox.Name = "onlineBox";
      this.onlineBox.Size = new Size(25, 25);
      this.onlineBox.TabIndex = 17;
      this.onlineBox.TabStop = false;
      this.offlineLabel.AutoSize = true;
      this.offlineLabel.Location = new Point(7, 61);
      this.offlineLabel.Name = "offlineLabel";
      this.offlineLabel.Size = new Size(100, 13);
      this.offlineLabel.TabIndex = 16;
      this.offlineLabel.Text = "Offline Status Color:";
      this.onlineLabel.AutoSize = true;
      this.onlineLabel.Location = new Point(7, 29);
      this.onlineLabel.Name = "onlineLabel";
      this.onlineLabel.Size = new Size(100, 13);
      this.onlineLabel.TabIndex = 15;
      this.onlineLabel.Text = "Online Status Color:";
      this.colorDialog.AnyColor = true;
      this.colorDialog.FullOpen = true;
      this.colorDialog.SolidColorOnly = true;
      this.arpErrorLabel.AutoSize = true;
      this.arpErrorLabel.Enabled = false;
      this.arpErrorLabel.Location = new Point(316, 29);
      this.arpErrorLabel.Name = "arpErrorLabel";
      this.arpErrorLabel.Size = new Size(84, 13);
      this.arpErrorLabel.TabIndex = 20;
      this.arpErrorLabel.Text = "ARP Error Color:";
      this.arpErrorBox.Enabled = false;
      this.arpErrorBox.Location = new Point(422, 24);
      this.arpErrorBox.Name = "arpErrorBox";
      this.arpErrorBox.Size = new Size(25, 25);
      this.arpErrorBox.TabIndex = 21;
      this.arpErrorBox.TabStop = false;
      this.changeArpErrorButton.Enabled = false;
      this.changeArpErrorButton.FlatStyle = FlatStyle.Flat;
      this.changeArpErrorButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.changeArpErrorButton.Location = new Point(460, 23);
      this.changeArpErrorButton.Name = "changeArpErrorButton";
      this.changeArpErrorButton.Size = new Size(113, 25);
      this.changeArpErrorButton.TabIndex = 22;
      this.changeArpErrorButton.Text = "Change Color";
      this.changeArpErrorButton.UseVisualStyleBackColor = true;
      this.changeArpErrorButton.Click += new EventHandler(this.onChangeArpError);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(30, 30, 30);
      this.ClientSize = new Size(640, 480);
      this.Controls.Add((Control) this.designBox);
      this.Controls.Add((Control) this.addButton);
      this.Controls.Add((Control) this.advancedBox);
      this.Controls.Add((Control) this.closeButton);
      this.Controls.Add((Control) this.basicBox);
      this.Controls.Add((Control) this.titleLabel);
      this.ForeColor = Color.White;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "AddDeviceForm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Device";
      this.Load += new EventHandler(this.onLoad);
      this.Shown += new EventHandler(this.onShow);
      this.basicBox.ResumeLayout(false);
      this.basicBox.PerformLayout();
      this.timeoutBox.EndInit();
      this.intervalBox.EndInit();
      this.advancedBox.ResumeLayout(false);
      this.advancedBox.PerformLayout();
      this.arpIntervalBox.EndInit();
      this.designBox.ResumeLayout(false);
      this.designBox.PerformLayout();
      ((ISupportInitialize) this.offlineBox).EndInit();
      ((ISupportInitialize) this.onlineBox).EndInit();
      ((ISupportInitialize) this.arpErrorBox).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
