using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace WaElo
{
  public class WAgame:IDisposable
  {
    private bool disposedValue = false;
    private string fullFileName;
    private BinaryReader br;
    private Lazy<WAgameMapType> mapType;

    public string FullFileName { get { return fullFileName; } }

    public string FileName { get { return Path.GetFileName(fullFileName); } }

    public DateTime CreationTime { get; }

    public WAgameMapType MapType { get; }

    public byte[] Map { get; }

    public WAgame(string fileName)
    {
      fullFileName = fileName;
      CreationTime = new FileInfo(fullFileName).CreationTime;
      br = new BinaryReader(new FileStream(fullFileName, FileMode.Open, FileAccess.Read, FileShare.Read));
    }

    public WAgameLog GetLog()
    {
      var p = new Process();
      p.StartInfo = new ProcessStartInfo
      {
        FileName = Path.Combine(GlobalVars.Instance.WAPath, "WA.exe"),
        Arguments = $"/getlog \"{fullFileName}\"",
        CreateNoWindow = true,
        WindowStyle = ProcessWindowStyle.Hidden
      };
      p.Start();
      p.WaitForExit();
      return new WAgameLog(Path.ChangeExtension(fullFileName, "log"));
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          br.Close();
        }
        disposedValue = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
    }
  }

  public enum WAgameMapType
  {
    Monochrome,
    Seeded,
    Colored
  }
}
