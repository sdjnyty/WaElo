using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Linq;

namespace WaElo
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private Model model;
    private Stack<History> history;
    private List<WAgame> wAgames;

    public Model Model { get { return model; } }

    public Stack<History> History { get { return history; } }

    public List<WAgame> WAgames { get { return wAgames; } }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
      model = FindResource("Model") as Model;
      history = new Stack<History>();
      wAgames = Directory.GetFiles(Path.Combine(GlobalVars.Instance.WAPath, @"User\Games"), "*Online*.WAgame").Select(f => new WAgame(f)).ToList();
    }
  }
}
