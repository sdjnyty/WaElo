using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WaElo
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private Model model;
    public Model Model { get { return model; } }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
      model = FindResource("Model") as Model;
    }
  }
}
