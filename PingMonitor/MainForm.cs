// Decompiled with JetBrains decompiler
// Type: PingMonitor.MainForm
// Assembly: PingMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C7F21AE2-E822-440B-97D1-7ECAE911FD7D
// Assembly location: G:\Projects\PingMonitor\PingMonitor\bin\Debug\PingMonitor.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PingMonitor
{
    public class MainForm : Form
    {
        public static int lastX = 0;
        public static int lastY = 0;
        private static List<Device> devices = new List<Device>();
        private static List<Thread> threads = new List<Thread>();
        private static List<Thread> arpThreads = new List<Thread>();
        private Device dragDevice = (Device)null;
        private int dragOffX = 0;
        private int dragOffY = 0;
        private int dragIndex = -1;
        private int selectedIndex = -1;
        private IContainer components = (IContainer)null;
        private Label titleLabel;
        private Label copyrightLabel;
        private Button closeButton;
        private Button minimizeButton;
        private Button helpButton;
        private ContextMenuStrip mapContextMenu;
        private ToolStripMenuItem addDeviceButton;
        private Label statusLabel;
        private ToolStripMenuItem editDeviceButton;
        private ToolStripMenuItem deleteDeviceButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem importButton;
        private ToolStripMenuItem exportButton;
        private SaveFileDialog exportDialog;
        private OpenFileDialog importDialog;

        public MainForm()
        {
            this.InitializeComponent();
        }

        private int getSelectedDeviceIndex()
        {
            int x = this.PointToClient(Cursor.Position).X;
            int y = this.PointToClient(Cursor.Position).Y;
            for (int index = 0; index < MainForm.devices.Count; ++index)
            {
                Device device = MainForm.devices[index];
                if (x > device.X - 32 && x < device.X + 32 && y > device.Y - 32 && y < device.Y + 32)
                    return index;
            }
            return -1;
        }

        private void onMouseDown(object sender, MouseEventArgs e)
        {
            int x = this.PointToClient(Cursor.Position).X;
            int y = this.PointToClient(Cursor.Position).Y;
            for (int index = 0; index < MainForm.devices.Count; ++index)
            {
                Device device = MainForm.devices[index];
                if (x > device.X - 32 && x < device.X + 32 && y > device.Y - 32 && y < device.Y + 32)
                {
                    this.dragDevice = device;
                    this.dragOffX = device.X - x;
                    this.dragOffY = device.Y - y;
                    this.dragIndex = index;
                    break;
                }
            }
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragDevice == null)
                return;
            Point client = this.PointToClient(Cursor.Position);
            int x = client.X;
            client = this.PointToClient(Cursor.Position);
            int y = client.Y;
            this.dragDevice.X = x + this.dragOffX;
            this.dragDevice.Y = y + this.dragOffY;
            MainForm.devices[this.dragIndex] = this.dragDevice;
            this.drawMap();
        }

        private void shutdown()
        {
            this.statusLabel.Visible = true;
            foreach (Thread thread in MainForm.threads)
                thread.Abort();
            foreach (Thread arpThread in MainForm.arpThreads)
                arpThread.Abort();
        }

        private void drawMap()
        {
            try
            {
                Bitmap bitmap = new Bitmap(this.Width, this.Height);
                using (Graphics graphics = Graphics.FromImage((Image)bitmap))
                {
                    graphics.Clear(Color.FromArgb(20, 20, 20));
                    foreach (Device device in MainForm.devices)
                    {
                        graphics.DrawImage(Image.FromFile("device.png"), device.X - 32, device.Y - 32);
                        Brush brush = (Brush)new SolidBrush(device.Status == DeviceStatus.Pending ? Color.Gray : (device.Status == DeviceStatus.Online ? device.OnlineColor : (device.Status == DeviceStatus.Offline ? device.OfflineColor : device.ArpErrorColor)));
                        SizeF sizeF = graphics.MeasureString(device.Name, new FontConverter().ConvertFromString("Microsoft Sans Serif") as Font);
                        float width1 = sizeF.Width;
                        graphics.DrawString(device.Name, new FontConverter().ConvertFromString("Microsoft Sans Serif") as Font, brush, (float)((double)device.X - (double)width1 / 2.0 + 12.0), (float)(device.Y + 40));
                        graphics.FillEllipse(brush, (float)((double)device.X - (double)width1 / 2.0 - 10.0), (float)(device.Y + 40), 15f, 15f);
                        sizeF = graphics.MeasureString("(" + device.IP.ToString() + ")", new FontConverter().ConvertFromString("Microsoft Sans Serif") as Font);
                        float width2 = sizeF.Width;
                        graphics.DrawString("(" + device.IP.ToString() + ")", new FontConverter().ConvertFromString("Microsoft Sans Serif") as Font, (Brush)new SolidBrush(Color.White), (float)device.X - width2 / 2f, (float)(device.Y - 50));
                    }
                }
                using (Graphics graphics = this.CreateGraphics())
                    graphics.DrawImage((Image)bitmap, 0, 0);
                GC.Collect();
            }
            catch (InvalidOperationException ex)
            {
                Thread.Sleep(10);
                this.drawMap();
            }
        }

        private void addDevice(Device device)
        {
            this.exportButton.Enabled = true;
            MainForm.devices.Add(device);
            if (device.EnableARP)
            {
                Thread thread = new Thread((ThreadStart)(() => this.checkDeviceARP(device)));
                MainForm.arpThreads.Add(thread);
                thread.Start();
                device.ARPThreadIndex = MainForm.arpThreads.Count - 1;
            }
            else
            {
                Thread thread = new Thread((ThreadStart)(() => this.checkDevice(device)));
                MainForm.threads.Add(thread);
                thread.Start();
                device.PingThreadIndex = MainForm.threads.Count - 1;
            }
            this.drawMap();
        }

        private void checkDeviceARP(Device device)
        {
            while (true)
            {
                try
                {
                    device.CheckARP();
                }
                catch (Exception ex)
                {
                }
                this.drawMap();
                Thread.Sleep(device.ARPInterval);
            }
        }

        private void checkDevice(Device device)
        {
            while (true)
            {
                try
                {
                    device.Check();
                }
                catch (Exception ex)
                {
                }
                this.drawMap();
                Thread.Sleep(device.PingInterval);
            }
        }

        private void onClose(object sender, EventArgs e)
        {
            this.shutdown();
            for (int index = 0; index < 10; ++index)
            {
                this.Opacity = this.Opacity - 0.1;
                Thread.Sleep(10);
            }
            this.Close();
        }

        private void onMinimize(object sender, EventArgs e)
        {
            for (int index = 0; index < 10; ++index)
            {
                this.Opacity = this.Opacity - 0.1;
                Thread.Sleep(10);
            }
            this.WindowState = FormWindowState.Minimized;
            this.Opacity = 1.0;
        }

        private void onHelp(object sender, EventArgs e)
        {
            int num = (int)new HelpForm().ShowDialog();
        }

        private void onClick(object sender, MouseEventArgs e)
        {
            this.selectedIndex = this.getSelectedDeviceIndex();
            if (e.Button != MouseButtons.Right)
                return;
            MainForm.lastX = this.PointToClient(Cursor.Position).X;
            MainForm.lastY = this.PointToClient(Cursor.Position).Y;
            if (this.getSelectedDeviceIndex() != -1)
            {
                this.editDeviceButton.Enabled = true;
                this.deleteDeviceButton.Enabled = true;
            }
            else
            {
                this.editDeviceButton.Enabled = false;
                this.deleteDeviceButton.Enabled = false;
            }
            this.mapContextMenu.Show(this.PointToClient(Cursor.Position));
        }

        private void onAddDevice(object sender, EventArgs e)
        {
            AddDeviceForm addDeviceForm = new AddDeviceForm();
            int num = (int)addDeviceForm.ShowDialog();
            if (addDeviceForm.AddDevice == null)
                return;
            this.addDevice(addDeviceForm.AddDevice);
        }

        private void onShow(object sender, EventArgs e)
        {
            this.drawMap();
            this.WindowState = FormWindowState.Normal;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        private void onMouseUp(object sender, MouseEventArgs e)
        {
            this.dragDevice = (Device)null;
            this.dragIndex = -1;
        }

        private void onDeleteDevice(object sender, EventArgs e)
        {
            Device device = MainForm.devices[this.selectedIndex];
            if (device.PingThreadIndex != -1)
            {
                MainForm.threads[device.PingThreadIndex].Abort();
                MainForm.threads.RemoveAt(device.PingThreadIndex);
            }
            if (device.ARPThreadIndex != -1)
            {
                MainForm.arpThreads[device.ARPThreadIndex].Abort();
                MainForm.arpThreads.RemoveAt(device.ARPThreadIndex);
            }
            MainForm.devices.RemoveAt(this.selectedIndex);
            if (MainForm.devices.Count == 0)
                this.exportButton.Enabled = false;
            this.drawMap();
        }

        private void onExport(object sender, EventArgs e)
        {
            if (this.exportDialog.ShowDialog() != DialogResult.OK)
                return;
            Serializer.Save(this.exportDialog.FileName, (object)MainForm.devices);
        }

        private void onDoubleClick(object sender, EventArgs e)
        {
            int selectedDeviceIndex = this.getSelectedDeviceIndex();
            if (selectedDeviceIndex == -1)
                return;
            int num = (int)new DeviceInfoForm(MainForm.devices[selectedDeviceIndex]).ShowDialog();
        }

        private void onImport(object sender, EventArgs e)
        {
            if (this.importDialog.ShowDialog() != DialogResult.OK)
                return;
            shutdown();
            this.statusLabel.Visible = false;
            devices.Clear();
            foreach (Device device in Serializer.Load<List<Device>>(this.importDialog.FileName))
                this.addDevice(device);
        }

        private void onEditDevice(object sender, EventArgs e)
        {
            Device device = MainForm.devices[this.selectedIndex];
            EditDeviceForm editDeviceForm = new EditDeviceForm(device);
            int num = (int)editDeviceForm.ShowDialog();
            if (device.PingThreadIndex != -1)
            {
                MainForm.threads[device.PingThreadIndex].Abort();
                MainForm.threads.RemoveAt(device.PingThreadIndex);
            }
            if (device.ARPThreadIndex != -1)
            {
                MainForm.arpThreads[device.ARPThreadIndex].Abort();
                MainForm.arpThreads.RemoveAt(device.ARPThreadIndex);
            }
            MainForm.devices.RemoveAt(this.selectedIndex);
            this.addDevice(editDeviceForm.EditDevice);
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.mapContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addDeviceButton = new System.Windows.Forms.ToolStripMenuItem();
            this.editDeviceButton = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDeviceButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importButton = new System.Windows.Forms.ToolStripMenuItem();
            this.exportButton = new System.Windows.Forms.ToolStripMenuItem();
            this.statusLabel = new System.Windows.Forms.Label();
            this.exportDialog = new System.Windows.Forms.SaveFileDialog();
            this.importDialog = new System.Windows.Forms.OpenFileDialog();
            this.mapContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(10, 6);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(166, 20);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Ping Monitor V1.1.1";
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.copyrightLabel.Location = new System.Drawing.Point(180, 13);
            this.copyrightLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(128, 13);
            this.copyrightLabel.TabIndex = 1;
            this.copyrightLabel.Text = "(© Markus Zechner, 2019)";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(1883, 12);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(25, 25);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "X";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.onClose);
            // 
            // minimizeButton
            // 
            this.minimizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minimizeButton.Location = new System.Drawing.Point(1852, 12);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(25, 25);
            this.minimizeButton.TabIndex = 3;
            this.minimizeButton.Text = "_";
            this.minimizeButton.UseVisualStyleBackColor = true;
            this.minimizeButton.Click += new System.EventHandler(this.onMinimize);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpButton.Location = new System.Drawing.Point(1821, 12);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(25, 25);
            this.helpButton.TabIndex = 4;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.onHelp);
            // 
            // mapContextMenu
            // 
            this.mapContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addDeviceButton,
            this.editDeviceButton,
            this.deleteDeviceButton,
            this.toolStripSeparator1,
            this.importButton,
            this.exportButton});
            this.mapContextMenu.Name = "mapContextMenu";
            this.mapContextMenu.Size = new System.Drawing.Size(170, 120);
            // 
            // addDeviceButton
            // 
            this.addDeviceButton.Name = "addDeviceButton";
            this.addDeviceButton.Size = new System.Drawing.Size(169, 22);
            this.addDeviceButton.Text = "Add Device...";
            this.addDeviceButton.Click += new System.EventHandler(this.onAddDevice);
            // 
            // editDeviceButton
            // 
            this.editDeviceButton.Enabled = false;
            this.editDeviceButton.Name = "editDeviceButton";
            this.editDeviceButton.Size = new System.Drawing.Size(169, 22);
            this.editDeviceButton.Text = "Edit...";
            this.editDeviceButton.Click += new System.EventHandler(this.onEditDevice);
            // 
            // deleteDeviceButton
            // 
            this.deleteDeviceButton.Enabled = false;
            this.deleteDeviceButton.Name = "deleteDeviceButton";
            this.deleteDeviceButton.Size = new System.Drawing.Size(169, 22);
            this.deleteDeviceButton.Text = "Delete";
            this.deleteDeviceButton.Click += new System.EventHandler(this.onDeleteDevice);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
            // 
            // importButton
            // 
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(169, 22);
            this.importButton.Text = "Import from File...";
            this.importButton.Click += new System.EventHandler(this.onImport);
            // 
            // exportButton
            // 
            this.exportButton.Enabled = false;
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(169, 22);
            this.exportButton.Text = "Export to File...";
            this.exportButton.Click += new System.EventHandler(this.onExport);
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.statusLabel.Location = new System.Drawing.Point(1672, 19);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(144, 13);
            this.statusLabel.TabIndex = 5;
            this.statusLabel.Text = "Waiting for Threads to close...";
            this.statusLabel.Visible = false;
            // 
            // exportDialog
            // 
            this.exportDialog.DefaultExt = "pmc";
            this.exportDialog.FileName = "config.pmc";
            this.exportDialog.Filter = "PingMonitor Configuration Files|*.pmc";
            this.exportDialog.Title = "Export Configuration";
            // 
            // importDialog
            // 
            this.importDialog.DefaultExt = "pmc";
            this.importDialog.Filter = "PingMonitor Configuration Files|*.pmc";
            this.importDialog.Title = "Import Configuration";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.minimizeButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.titleLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ping Monitor";
            this.Shown += new System.EventHandler(this.onShow);
            this.DoubleClick += new System.EventHandler(this.onDoubleClick);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.onClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.onMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.onMouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.onMouseUp);
            this.mapContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
