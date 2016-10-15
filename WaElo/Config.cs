using System.Xml.Linq;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;

namespace WaElo
{
  public class Config
  {
    private const string FILENAME = "waelo.xml";

    public static XElement Root { get; private set; }

    public static ObservableCollection<User> Users { get; private set; }

    static Config()
    {
      if (File.Exists(FILENAME))
        Root = XElement.Load(FILENAME);
      else
        Root = new XElement("waelo");
      Root.Changed += (s, e) => Root.Save(FILENAME);
      Users = new ObservableCollection<User>(Root.Elements(nameof(User)).Select(ele => new User(ele)));
    }
  }
}
