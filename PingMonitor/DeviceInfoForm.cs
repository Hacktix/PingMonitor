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
        private IContainer components = (IContainer)null;
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
        private TextBox lastUpdateBox;
        private Label lastUpdateLabel;
        private Label graphLabel;
        private PictureBox uptimeBar;
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
            this.lastUpdateBox.Text = this.device.LastSuccessfulUpdate != DateTime.MinValue ? getTimeDiffString(this.device.LastSuccessfulUpdate) : "never";
            drawUptimeGraph();
        }

        private void drawUptimeGraph()
        {
            Bitmap bmp = new Bitmap(uptimeBar.Width, uptimeBar.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < this.device.StatusHistory.Length; i++)
                {
                    Color col = this.device.StatusHistory[i] == DeviceStatus.Pending ? Color.Gray : this.device.StatusHistory[i] == DeviceStatus.Online ? this.device.OnlineColor : this.device.StatusHistory[i] == DeviceStatus.Offline ? device.OfflineColor : device.ArpErrorColor;
                    g.DrawLine(new Pen(new SolidBrush(col)), i, 0, i, uptimeBar.Height);
                }
            }
            uptimeBar.Image = bmp;
        }

        private string getTimeDiffString(DateTime dt)
        {
            TimeSpan diff = DateTime.Now - dt;
            int h = diff.Hours;
            int m = diff.Minutes;
            int s = diff.Seconds;
            return h == 0 && m == 0 && s == 0 ? "now" : string.Format("{0:00}:{1:00}:{2:00} ago", h, m, s);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.closeButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.ipLabel = new System.Windows.Forms.Label();
            this.macLabel = new System.Windows.Forms.Label();
            this.descLabel = new System.Windows.Forms.Label();
            this.descBox = new System.Windows.Forms.TextBox();
            this.infoUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.pingStatusLabel = new System.Windows.Forms.Label();
            this.arpStatusLabel = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.macBox = new System.Windows.Forms.TextBox();
            this.pingStatusBox = new System.Windows.Forms.TextBox();
            this.arpStatusBox = new System.Windows.Forms.TextBox();
            this.lastUpdateBox = new System.Windows.Forms.TextBox();
            this.lastUpdateLabel = new System.Windows.Forms.Label();
            this.graphLabel = new System.Windows.Forms.Label();
            this.uptimeBar = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.uptimeBar)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(603, 12);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(25, 25);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "X";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.onClose);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(12, 13);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(160, 20);
            this.titleLabel.TabIndex = 5;
            this.titleLabel.Text = "Device Information";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(13, 48);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(78, 13);
            this.nameLabel.TabIndex = 6;
            this.nameLabel.Text = "Device Name: ";
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(13, 70);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(60, 13);
            this.ipLabel.TabIndex = 7;
            this.ipLabel.Text = "Device IP: ";
            // 
            // macLabel
            // 
            this.macLabel.AutoSize = true;
            this.macLabel.Location = new System.Drawing.Point(13, 91);
            this.macLabel.Name = "macLabel";
            this.macLabel.Size = new System.Drawing.Size(73, 13);
            this.macLabel.TabIndex = 8;
            this.macLabel.Text = "Device MAC: ";
            // 
            // descLabel
            // 
            this.descLabel.AutoSize = true;
            this.descLabel.Location = new System.Drawing.Point(13, 131);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(63, 13);
            this.descLabel.TabIndex = 9;
            this.descLabel.Text = "Description:";
            // 
            // descBox
            // 
            this.descBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.descBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.descBox.ForeColor = System.Drawing.Color.White;
            this.descBox.Location = new System.Drawing.Point(16, 147);
            this.descBox.Multiline = true;
            this.descBox.Name = "descBox";
            this.descBox.ReadOnly = true;
            this.descBox.Size = new System.Drawing.Size(612, 130);
            this.descBox.TabIndex = 10;
            // 
            // infoUpdateTimer
            // 
            this.infoUpdateTimer.Tick += new System.EventHandler(this.onUpdateTick);
            // 
            // pingStatusLabel
            // 
            this.pingStatusLabel.AutoSize = true;
            this.pingStatusLabel.Location = new System.Drawing.Point(13, 297);
            this.pingStatusLabel.Name = "pingStatusLabel";
            this.pingStatusLabel.Size = new System.Drawing.Size(87, 13);
            this.pingStatusLabel.TabIndex = 11;
            this.pingStatusLabel.Text = "Last Ping Status:";
            // 
            // arpStatusLabel
            // 
            this.arpStatusLabel.AutoSize = true;
            this.arpStatusLabel.Location = new System.Drawing.Point(13, 325);
            this.arpStatusLabel.Name = "arpStatusLabel";
            this.arpStatusLabel.Size = new System.Drawing.Size(88, 13);
            this.arpStatusLabel.TabIndex = 12;
            this.arpStatusLabel.Text = "Last ARP Status:";
            // 
            // nameBox
            // 
            this.nameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.nameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nameBox.ForeColor = System.Drawing.Color.White;
            this.nameBox.Location = new System.Drawing.Point(97, 46);
            this.nameBox.Name = "nameBox";
            this.nameBox.ReadOnly = true;
            this.nameBox.Size = new System.Drawing.Size(531, 20);
            this.nameBox.TabIndex = 13;
            // 
            // ipBox
            // 
            this.ipBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ipBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ipBox.ForeColor = System.Drawing.Color.White;
            this.ipBox.Location = new System.Drawing.Point(97, 68);
            this.ipBox.Name = "ipBox";
            this.ipBox.ReadOnly = true;
            this.ipBox.Size = new System.Drawing.Size(531, 20);
            this.ipBox.TabIndex = 14;
            // 
            // macBox
            // 
            this.macBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.macBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.macBox.ForeColor = System.Drawing.Color.White;
            this.macBox.Location = new System.Drawing.Point(97, 89);
            this.macBox.Name = "macBox";
            this.macBox.ReadOnly = true;
            this.macBox.Size = new System.Drawing.Size(531, 20);
            this.macBox.TabIndex = 15;
            // 
            // pingStatusBox
            // 
            this.pingStatusBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.pingStatusBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pingStatusBox.ForeColor = System.Drawing.Color.White;
            this.pingStatusBox.Location = new System.Drawing.Point(137, 295);
            this.pingStatusBox.Name = "pingStatusBox";
            this.pingStatusBox.ReadOnly = true;
            this.pingStatusBox.Size = new System.Drawing.Size(491, 20);
            this.pingStatusBox.TabIndex = 16;
            // 
            // arpStatusBox
            // 
            this.arpStatusBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.arpStatusBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.arpStatusBox.ForeColor = System.Drawing.Color.White;
            this.arpStatusBox.Location = new System.Drawing.Point(137, 323);
            this.arpStatusBox.Name = "arpStatusBox";
            this.arpStatusBox.ReadOnly = true;
            this.arpStatusBox.Size = new System.Drawing.Size(491, 20);
            this.arpStatusBox.TabIndex = 17;
            // 
            // lastUpdateBox
            // 
            this.lastUpdateBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.lastUpdateBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lastUpdateBox.ForeColor = System.Drawing.Color.White;
            this.lastUpdateBox.Location = new System.Drawing.Point(137, 352);
            this.lastUpdateBox.Name = "lastUpdateBox";
            this.lastUpdateBox.ReadOnly = true;
            this.lastUpdateBox.Size = new System.Drawing.Size(491, 20);
            this.lastUpdateBox.TabIndex = 18;
            // 
            // lastUpdateLabel
            // 
            this.lastUpdateLabel.AutoSize = true;
            this.lastUpdateLabel.Location = new System.Drawing.Point(12, 354);
            this.lastUpdateLabel.Name = "lastUpdateLabel";
            this.lastUpdateLabel.Size = new System.Drawing.Size(119, 13);
            this.lastUpdateLabel.TabIndex = 19;
            this.lastUpdateLabel.Text = "Last successful update:";
            // 
            // graphLabel
            // 
            this.graphLabel.AutoSize = true;
            this.graphLabel.Location = new System.Drawing.Point(13, 379);
            this.graphLabel.Name = "graphLabel";
            this.graphLabel.Size = new System.Drawing.Size(93, 13);
            this.graphLabel.TabIndex = 20;
            this.graphLabel.Text = "Visualized Uptime:";
            // 
            // uptimeBar
            // 
            this.uptimeBar.Location = new System.Drawing.Point(16, 408);
            this.uptimeBar.Name = "uptimeBar";
            this.uptimeBar.Size = new System.Drawing.Size(612, 42);
            this.uptimeBar.TabIndex = 21;
            this.uptimeBar.TabStop = false;
            // 
            // DeviceInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.uptimeBar);
            this.Controls.Add(this.graphLabel);
            this.Controls.Add(this.lastUpdateLabel);
            this.Controls.Add(this.lastUpdateBox);
            this.Controls.Add(this.arpStatusBox);
            this.Controls.Add(this.pingStatusBox);
            this.Controls.Add(this.macBox);
            this.Controls.Add(this.ipBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.arpStatusLabel);
            this.Controls.Add(this.pingStatusLabel);
            this.Controls.Add(this.descBox);
            this.Controls.Add(this.descLabel);
            this.Controls.Add(this.macLabel);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.closeButton);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DeviceInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device Info";
            this.Load += new System.EventHandler(this.onLoad);
            this.Shown += new System.EventHandler(this.onShow);
            ((System.ComponentModel.ISupportInitialize)(this.uptimeBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
