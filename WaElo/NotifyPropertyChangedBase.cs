using System.ComponentModel;

namespace WaElo
{
  public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
  {
    public virtual event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
