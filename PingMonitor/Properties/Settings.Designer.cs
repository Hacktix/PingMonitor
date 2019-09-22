// Decompiled with JetBrains decompiler
// Type: PingMonitor.Properties.Settings
// Assembly: PingMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C7F21AE2-E822-440B-97D1-7ECAE911FD7D
// Assembly location: G:\Projects\PingMonitor\PingMonitor\bin\Debug\PingMonitor.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace PingMonitor.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings defaultInstance = Settings.defaultInstance;
        return defaultInstance;
      }
    }
  }
}
