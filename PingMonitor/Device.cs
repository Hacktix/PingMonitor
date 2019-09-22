// Decompiled with JetBrains decompiler
// Type: PingMonitor.Device
// Assembly: PingMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C7F21AE2-E822-440B-97D1-7ECAE911FD7D
// Assembly location: G:\Projects\PingMonitor\PingMonitor\bin\Debug\PingMonitor.exe

using System;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace PingMonitor
{
  [Serializable]
  public class Device
  {
    public bool EnableARP = false;
    public string MAC = (string) null;
    public int ARPInterval = -1;
    public int PingThreadIndex = -1;
    public int ARPThreadIndex = -1;
    public DeviceStatus Status = DeviceStatus.Pending;
    public IPStatus LastPingResponse = IPStatus.Unknown;
    public int LastARPResponse = -1;
    public string LastARPAddress = (string) null;
    public int X;
    public int Y;
    public Color OnlineColor;
    public Color OfflineColor;
    public Color ArpErrorColor;
    public string Name;
    public string Description;
    public IPAddress IP;
    public int PingInterval;
    public int PingTimeout;

    public Device(int x, int y, Color onlineColor, Color offlineColor, Color arpErrorColor, string name, string description, IPAddress iP, int pingInterval, int pingTimeout)
    {
      this.X = x;
      this.Y = y;
      this.OnlineColor = onlineColor;
      this.OfflineColor = offlineColor;
      this.ArpErrorColor = arpErrorColor;
      this.Name = name;
      this.Description = description;
      this.IP = iP;
      this.PingInterval = pingInterval;
      this.PingTimeout = pingTimeout;
    }

    public Device(int x, int y, Color onlineColor, Color offlineColor, Color arpErrorColor, string name, string description, IPAddress iP, int pingInterval, int pingTimeout, string mAC, int aRPInterval)
      : this(x, y, onlineColor, offlineColor, arpErrorColor, name, description, iP, pingInterval, pingTimeout)
    {
      this.EnableARP = true;
      this.MAC = mAC;
      this.ARPInterval = aRPInterval;
    }

    [DllImport("iphlpapi.dll")]
    public static extern int SendARP(uint DestIP, uint SrcIP, byte[] pMacAddr, ref int PhyAddrLen);

    public void Check()
    {
      Ping ping = new Ping();
      PingOptions options = new PingOptions();
      options.DontFragment = true;
      byte[] bytes = Encoding.ASCII.GetBytes("Ping Monitor(tm) (c) Markus Zechner 2019");
      PingReply pingReply = (PingReply) null;
      try
      {
        pingReply = ping.Send(this.IP, this.PingTimeout, bytes, options);
        this.LastPingResponse = pingReply.Status;
      }
      catch (Exception ex)
      {
        this.Status = DeviceStatus.Offline;
      }
      if (pingReply == null)
        return;
      this.Status = pingReply.Status != IPStatus.Success || this.Status == DeviceStatus.ARPError ? DeviceStatus.Offline : DeviceStatus.Online;
    }

    public void CheckARP()
    {
      uint uint32 = BitConverter.ToUInt32(this.IP.GetAddressBytes(), 0);
      byte[] pMacAddr = new byte[6];
      int length = pMacAddr.Length;
      int num = Device.SendARP(uint32, 0U, pMacAddr, ref length);
      this.LastARPResponse = num;
      if ((uint) num > 0U)
      {
        this.Status = DeviceStatus.ARPError;
      }
      else
      {
        string[] strArray = new string[length];
        for (int index = 0; index < length; ++index)
          strArray[index] = pMacAddr[index].ToString("x2");
        string lower = string.Join(":", strArray).ToLower();
        this.LastARPAddress = lower.ToUpper();
        if (this.MAC.Length > 0 && !this.MAC.ToLower().Equals(lower))
        {
          this.Status = DeviceStatus.ARPError;
        }
        else
        {
          this.Status = DeviceStatus.Online;
          if (this.MAC.Length == 0)
            this.MAC = lower;
        }
      }
    }
  }
}
