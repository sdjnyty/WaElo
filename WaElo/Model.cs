using System.ComponentModel;
using System.Xml.Linq;

namespace WaElo
{
  public class Model : INotifyPropertyChanged
  {
    private XElement winner;
    private XElement loser;

    public XElement Winner
    {
      get { return winner; }
      set
      {
        winner = value;
        OnPropertyChanged(nameof(Winner));
      }
    }

    public XElement Loser
    {
      get { return loser; }
      set
      {
        loser = value;
        OnPropertyChanged(nameof(Loser));
      }
    }

    public Model()
    {

    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
