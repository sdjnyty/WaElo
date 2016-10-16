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
    private string fileName;
    private string map;

    public string FileName { get { return Path.GetFileName(fileName); } }

    public DateTime CreationTime { get; }

    public string Map
    {
      get
      {
        map = Path.GetTempFileName();
        using (var br = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
        {
          using (var fs = new FileStream(map, FileMode.Create))
          {
            br.ReadInt32();
            var mapLength = br.ReadInt32() - 8;
            if (br.ReadInt32() == 3)
            {
              var bytes = br.ReadBytes(mapLength);
              fs.Write(bytes, 0, bytes.Length);
            }
            else
            {
              map = @"E:\WA3721\User\Flags\China.bmp";
            }
          }
        }
        return map;
      }
    }

    public WAgame(string fileName)
    {
      this.fileName = fileName;
      CreationTime = new FileInfo(fileName).CreationTime;
    }

    public string MakeLog()
    {
      var p = new Process();
      p.StartInfo = new ProcessStartInfo
      {
        FileName = Path.Combine(GlobalVars.Instance.WAPath, "WA.exe"),
        Arguments = $"/getlog \"{FileName}\"",
        CreateNoWindow = true,
        WindowStyle = ProcessWindowStyle.Hidden
      };
      p.Start();
      p.WaitForExit();
      return Path.ChangeExtension(FileName, "log");
    }
  }
}
