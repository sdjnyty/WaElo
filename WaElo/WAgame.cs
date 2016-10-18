using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace WaElo
{
  public class WAgame
  {
    private string fullFileName;

    public string FullFileName { get { return fullFileName; } }

    public string FileName { get { return Path.GetFileName(fullFileName); } }

    public DateTime CreationTime { get; }

    public WAgame(string fileName)
    {
      fullFileName = fileName;
      CreationTime = new FileInfo(fileName).CreationTime;
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
  }
}
