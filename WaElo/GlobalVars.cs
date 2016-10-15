using System.Windows;
using Microsoft.Win32;

namespace WaElo
{
  public class GlobalVars : NotifyPropertyChangedBase
  {
    public static GlobalVars Instance => Application.Current.FindResource("GlobalVars") as GlobalVars;
    private User winner;
    private User loser;
    private string wAPath = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Team17SoftwareLTD\WormsArmageddon").GetValue("PATH") as string;

    public User Winner
    {
      get { return winner; }
      set
      {
        winner = value;
        OnPropertyChanged(nameof(Winner));
      }
    }

    public User Loser
    {
      get { return loser; }
      set
      {
        loser = value;
        OnPropertyChanged(nameof(Loser));
      }
    }

    public string WAPath => wAPath;
  }
}
