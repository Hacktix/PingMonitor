// Decompiled with JetBrains decompiler
// Type: PingMonitor.HelpForm
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
  public class HelpForm : Form
  {
    private IContainer components = (IContainer) null;
    private Label label1;
    private Button closeButton;
    private Label helptextLabel;

    public HelpForm()
    {
      this.InitializeComponent();
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (HelpForm));
      this.label1 = new Label();
      this.closeButton = new Button();
      this.helptextLabel = new Label();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(162, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "Ping Monitor - Help";
      this.closeButton.FlatStyle = FlatStyle.Flat;
      this.closeButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.closeButton.Location = new Point(603, 12);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new Size(25, 25);
      this.closeButton.TabIndex = 3;
      this.closeButton.Text = "X";
      this.closeButton.UseVisualStyleBackColor = true;
      this.closeButton.Click += new EventHandler(this.onClose);
      this.helptextLabel.AutoSize = true;
      this.helptextLabel.Location = new Point(13, 51);
      this.helptextLabel.Name = "helptextLabel";
      this.helptextLabel.Size = new Size(562, 143);
      this.helptextLabel.TabIndex = 4;
      this.helptextLabel.Text = componentResourceManager.GetString("helptextLabel.Text");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(30, 30, 30);
      this.ClientSize = new Size(640, 480);
      this.Controls.Add((Control) this.helptextLabel);
      this.Controls.Add((Control) this.closeButton);
      this.Controls.Add((Control) this.label1);
      this.ForeColor = Color.White;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "HelpForm";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Ping Monitor Help";
      this.Load += new EventHandler(this.onLoad);
      this.Shown += new EventHandler(this.onShow);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
