// Decompiled with JetBrains decompiler
// Type: PingMonitor.DeviceInfoForm
// Assembly: PingMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C7F21AE2-E822-440B-97D1-7ECAE911FD7D
// Assembly location: G:\Projects\PingMonitor\PingMonitor\bin\Debug\PingMonitor.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PingMonitor
{
  public class DeviceInfoForm : Form
  {
    private IContainer components = (IContainer) null;
    private Device device;
    private Button closeButton;
    private Label titleLabel;
    private Label nameLabel;
    private Label ipLabel;
    private Label macLabel;
    private Label descLabel;
    private TextBox descBox;
    private System.Windows.Forms.Timer infoUpdateTimer;
    private Label pingStatusLabel;
    private Label arpStatusLabel;
    private TextBox nameBox;
    private TextBox ipBox;
    private TextBox macBox;
    private TextBox pingStatusBox;
    private TextBox arpStatusBox;

    public DeviceInfoForm(Device device)
    {
      this.InitializeComponent();
      this.device = device;
      this.updateDeviceInfo();
      this.infoUpdateTimer.Start();
    }

    private void onClose(object sender, EventArgs e)
    {
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
      for (int index = 0; index < 10; ++index)
      {
        this.Opacity = this.Opacity + 0.1;
        Thread.Sleep(10);
      }
    }

    private void onUpdateTick(object sender, EventArgs e)
    {
      this.updateDeviceInfo();
    }

    private void updateDeviceInfo()
    {
      this.nameBox.Text = this.device.Name;
      this.ipBox.Text = this.device.IP.ToString();
      this.macBox.Text = this.device.MAC == null || this.device.MAC.Length <= 0 ? "<unknown>" : this.device.MAC.ToUpper();
      this.descBox.Text = this.device.Description;
      if (this.device.EnableARP)
      {
        this.pingStatusBox.Text = "<unknown>";
        this.arpStatusBox.Text = this.device.LastARPResponse.ToString() + " (" + (this.device.LastARPAddress != null ? this.device.LastARPAddress : "<unknown MAC>") + ")";
        this.arpStatusLabel.ForeColor = this.device.Status == DeviceStatus.ARPError ? this.device.ArpErrorColor : this.device.OnlineColor;
      }
      else
      {
        this.arpStatusBox.Text = "<unknown>";
        this.pingStatusBox.Text = this.device.LastPingResponse.ToString();
        this.pingStatusBox.ForeColor = this.device.Status == DeviceStatus.Online ? this.device.OnlineColor : this.device.OfflineColor;
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
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DeviceInfoForm));
      this.closeButton = new Button();
      this.titleLabel = new Label();
      this.nameLabel = new Label();
      this.ipLabel = new Label();
      this.macLabel = new Label();
      this.descLabel = new Label();
      this.descBox = new TextBox();
      this.infoUpdateTimer = new System.Windows.Forms.Timer(this.components);
      this.pingStatusLabel = new Label();
      this.arpStatusLabel = new Label();
      this.nameBox = new TextBox();
      this.ipBox = new TextBox();
      this.macBox = new TextBox();
      this.pingStatusBox = new TextBox();
      this.arpStatusBox = new TextBox();
      this.SuspendLayout();
      this.closeButton.FlatStyle = FlatStyle.Flat;
      this.closeButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.closeButton.Location = new Point(603, 12);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new Size(25, 25);
      this.closeButton.TabIndex = 4;
      this.closeButton.Text = "X";
      this.closeButton.UseVisualStyleBackColor = true;
      this.closeButton.Click += new EventHandler(this.onClose);
      this.titleLabel.AutoSize = true;
      this.titleLabel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.titleLabel.Location = new Point(12, 13);
      this.titleLabel.Name = "titleLabel";
      this.titleLabel.Size = new Size(160, 20);
      this.titleLabel.TabIndex = 5;
      this.titleLabel.Text = "Device Information";
      this.nameLabel.AutoSize = true;
      this.nameLabel.Location = new Point(13, 48);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new Size(78, 13);
      this.nameLabel.TabIndex = 6;
      this.nameLabel.Text = "Device Name: ";
      this.ipLabel.AutoSize = true;
      this.ipLabel.Location = new Point(13, 70);
      this.ipLabel.Name = "ipLabel";
      this.ipLabel.Size = new Size(60, 13);
      this.ipLabel.TabIndex = 7;
      this.ipLabel.Text = "Device IP: ";
      this.macLabel.AutoSize = true;
      this.macLabel.Location = new Point(13, 91);
      this.macLabel.Name = "macLabel";
      this.macLabel.Size = new Size(73, 13);
      this.macLabel.TabIndex = 8;
      this.macLabel.Text = "Device MAC: ";
      this.descLabel.AutoSize = true;
      this.descLabel.Location = new Point(13, 131);
      this.descLabel.Name = "descLabel";
      this.descLabel.Size = new Size(63, 13);
      this.descLabel.TabIndex = 9;
      this.descLabel.Text = "Description:";
      this.descBox.BackColor = Color.FromArgb(20, 20, 20);
      this.descBox.BorderStyle = BorderStyle.FixedSingle;
      this.descBox.ForeColor = Color.White;
      this.descBox.Location = new Point(16, 147);
      this.descBox.Multiline = true;
      this.descBox.Name = "descBox";
      this.descBox.ReadOnly = true;
      this.descBox.Size = new Size(612, 130);
      this.descBox.TabIndex = 10;
      this.infoUpdateTimer.Tick += new EventHandler(this.onUpdateTick);
      this.pingStatusLabel.AutoSize = true;
      this.pingStatusLabel.Location = new Point(13, 297);
      this.pingStatusLabel.Name = "pingStatusLabel";
      this.pingStatusLabel.Size = new Size(87, 13);
      this.pingStatusLabel.TabIndex = 11;
      this.pingStatusLabel.Text = "Last Ping Status:";
      this.arpStatusLabel.AutoSize = true;
      this.arpStatusLabel.Location = new Point(13, 325);
      this.arpStatusLabel.Name = "arpStatusLabel";
      this.arpStatusLabel.Size = new Size(88, 13);
      this.arpStatusLabel.TabIndex = 12;
      this.arpStatusLabel.Text = "Last ARP Status:";
      this.nameBox.BackColor = Color.FromArgb(20, 20, 20);
      this.nameBox.BorderStyle = BorderStyle.FixedSingle;
      this.nameBox.ForeColor = Color.White;
      this.nameBox.Location = new Point(97, 46);
      this.nameBox.Name = "nameBox";
      this.nameBox.ReadOnly = true;
      this.nameBox.Size = new Size(531, 20);
      this.nameBox.TabIndex = 13;
      this.ipBox.BackColor = Color.FromArgb(20, 20, 20);
      this.ipBox.BorderStyle = BorderStyle.FixedSingle;
      this.ipBox.ForeColor = Color.White;
      this.ipBox.Location = new Point(97, 68);
      this.ipBox.Name = "ipBox";
      this.ipBox.ReadOnly = true;
      this.ipBox.Size = new Size(531, 20);
      this.ipBox.TabIndex = 14;
      this.macBox.BackColor = Color.FromArgb(20, 20, 20);
      this.macBox.BorderStyle = BorderStyle.FixedSingle;
      this.macBox.ForeColor = Color.White;
      this.macBox.Location = new Point(97, 89);
      this.macBox.Name = "macBox";
      this.macBox.ReadOnly = true;
      this.macBox.Size = new Size(531, 20);
      this.macBox.TabIndex = 15;
      this.pingStatusBox.BackColor = Color.FromArgb(20, 20, 20);
      this.pingStatusBox.BorderStyle = BorderStyle.FixedSingle;
      this.pingStatusBox.ForeColor = Color.White;
      this.pingStatusBox.Location = new Point(106, 295);
      this.pingStatusBox.Name = "pingStatusBox";
      this.pingStatusBox.ReadOnly = true;
      this.pingStatusBox.Size = new Size(522, 20);
      this.pingStatusBox.TabIndex = 16;
      this.arpStatusBox.BackColor = Color.FromArgb(20, 20, 20);
      this.arpStatusBox.BorderStyle = BorderStyle.FixedSingle;
      this.arpStatusBox.ForeColor = Color.White;
      this.arpStatusBox.Location = new Point(106, 323);
      this.arpStatusBox.Name = "arpStatusBox";
      this.arpStatusBox.ReadOnly = true;
      this.arpStatusBox.Size = new Size(522, 20);
      this.arpStatusBox.TabIndex = 17;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(30, 30, 30);
      this.ClientSize = new Size(640, 480);
      this.Controls.Add((Control) this.arpStatusBox);
      this.Controls.Add((Control) this.pingStatusBox);
      this.Controls.Add((Control) this.macBox);
      this.Controls.Add((Control) this.ipBox);
      this.Controls.Add((Control) this.nameBox);
      this.Controls.Add((Control) this.arpStatusLabel);
      this.Controls.Add((Control) this.pingStatusLabel);
      this.Controls.Add((Control) this.descBox);
      this.Controls.Add((Control) this.descLabel);
      this.Controls.Add((Control) this.macLabel);
      this.Controls.Add((Control) this.ipLabel);
      this.Controls.Add((Control) this.nameLabel);
      this.Controls.Add((Control) this.titleLabel);
      this.Controls.Add((Control) this.closeButton);
      this.ForeColor = Color.White;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "DeviceInfoForm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Device Info";
      this.Load += new EventHandler(this.onLoad);
      this.Shown += new EventHandler(this.onShow);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
