// Decompiled with JetBrains decompiler
// Type: PingMonitor.Serializer
// Assembly: PingMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C7F21AE2-E822-440B-97D1-7ECAE911FD7D
// Assembly location: G:\Projects\PingMonitor\PingMonitor\bin\Debug\PingMonitor.exe

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PingMonitor
{
  public static class Serializer
  {
    public static void Save(string filePath, object objToSerialize)
    {
      try
      {
        using (Stream serializationStream = (Stream) File.Open(filePath, FileMode.Create))
          new BinaryFormatter().Serialize(serializationStream, objToSerialize);
      }
      catch (IOException ex)
      {
      }
    }

    public static T Load<T>(string filePath) where T : new()
    {
      T obj = Activator.CreateInstance<T>();
      try
      {
        using (Stream serializationStream = (Stream) File.Open(filePath, FileMode.Open))
          obj = (T) new BinaryFormatter().Deserialize(serializationStream);
      }
      catch (IOException ex)
      {
      }
      return obj;
    }
  }
}
