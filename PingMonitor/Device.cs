// Decompiled with JetBrains decompiler
// Type: PingMonitor.Device
// Assembly: PingMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C7F21AE2-E822-440B-97D1-7ECAE911FD7D
// Assembly location: G:\Projects\PingMonitor\PingMonitor\bin\Debug\PingMonitor.exe

using System;
using System.Drawing;
using System.IO;
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
        public string MAC = (string)null;
        public int ARPInterval = -1;
        public int PingThreadIndex = -1;
        public int ARPThreadIndex = -1;
        public DeviceStatus Status = DeviceStatus.Pending;
        public IPStatus LastPingResponse = IPStatus.Unknown;
        public int LastARPResponse = -1;
        public string LastARPAddress = (string)null;
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
        public DateTime LastSuccessfulUpdate = DateTime.MinValue;
        public DeviceStatus[] StatusHistory = new DeviceStatus[612];

        private int historyCounter = 0;
        private int historyIndex = 0;
        private DeviceStatus lastStatus = DeviceStatus.Pending;

        private bool init = false;

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

        private void updateHistory()
        {
            if (historyCounter < 2)
                historyCounter++;
            else
            {
                historyCounter = 0;
                if(historyIndex < StatusHistory.Length)
                    StatusHistory[historyIndex++] = Status;
                else
                {
                    for (int i = 0; i < StatusHistory.Length - 1; i++)
                        StatusHistory[i] = StatusHistory[i + 1];
                    StatusHistory[StatusHistory.Length - 1] = Status;
                }
            }
        }

        public void resetHistory()
        {
            StatusHistory = new DeviceStatus[612];
            LastSuccessfulUpdate = DateTime.MinValue;
            historyCounter = 0;
            historyIndex = 0;
        }

        private void updateLog()
        {
            if(lastStatus != this.Status)
            {
                lastStatus = this.Status;
                File.AppendAllText("latest.log", "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "-DEVICE-" + Name + "] Status changed to " + Status.ToString() + (Status == DeviceStatus.ARPError ? " (Error: " + LastARPResponse + ")" : Status == DeviceStatus.Offline ? " (Error: " + LastPingResponse.ToString() + ")" : "") + Environment.NewLine);
            }
        }

        public void Check()
        {
            if(!init)
            {
                init = true;
                File.AppendAllText("latest.log", "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "-DEVICE-" + Name + "] Added to network map (IP: " + IP.ToString() + ")" + Environment.NewLine);
                File.AppendAllText("latest.log", "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "-DEVICE-" + Name + "] Initiated Ping Scan (Interval=" + PingInterval + "ms, Timeout=" + PingTimeout + "ms)" + Environment.NewLine);
            }
            Ping ping = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            byte[] bytes = Encoding.ASCII.GetBytes("Ping Monitor(tm) (c) Markus Zechner 2019");
            PingReply pingReply = (PingReply)null;
            try
            {
                pingReply = ping.Send(this.IP, this.PingTimeout, bytes, options);
                this.LastPingResponse = pingReply.Status;
            }
            catch (Exception ex)
            {
                this.Status = DeviceStatus.Offline;
                updateHistory();
                updateLog();
            }
            if (pingReply == null)
                return;
            this.Status = pingReply.Status != IPStatus.Success || this.Status == DeviceStatus.ARPError ? DeviceStatus.Offline : DeviceStatus.Online;
            if(Status == DeviceStatus.Online)
                this.LastSuccessfulUpdate = DateTime.Now;
            updateHistory();
            updateLog();
        }

        public void CheckARP()
        {
            if (!init)
            {
                init = true;
                File.AppendAllText("latest.log", "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "-DEVICE-" + Name + "] Added to network map (IP: " + IP.ToString() + ")" + Environment.NewLine);
                File.AppendAllText("latest.log", "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "-DEVICE-" + Name + "] Initiated ARP Scan (Interval=" + ARPInterval + "ms, MAC=" + (MAC != null && MAC.Length > 0 ? MAC : "<none>") + ")" + Environment.NewLine);
            }
            uint uint32 = BitConverter.ToUInt32(this.IP.GetAddressBytes(), 0);
            byte[] pMacAddr = new byte[6];
            int length = pMacAddr.Length;
            int num = Device.SendARP(uint32, 0U, pMacAddr, ref length);
            this.LastARPResponse = num;
            if ((uint)num > 0U)
            {
                this.Status = DeviceStatus.ARPError;
                for (int i = 0; i < 5; i++)
                    updateHistory();
                updateLog();
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
                    updateHistory();
                    updateLog();
                }
                else
                {
                    this.Status = DeviceStatus.Online;
                    if (this.MAC.Length == 0)
                        this.MAC = lower;
                    this.LastSuccessfulUpdate = DateTime.Now;
                    updateHistory();
                    updateLog();
                }
            }
        }
    }
}
