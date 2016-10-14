using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace WaElo
{
  public static class Config
  {
    private const string FILENAME = "waelo.xml";

    public static XElement Root { get; private set; }

    static Config()
    {
      if (File.Exists(FILENAME))
        Root = XElement.Load(FILENAME);
      else
        Root = new XElement("waelo");
      Root.Changed += (s, e) => Root.Save(FILENAME);
    }
  }
}
