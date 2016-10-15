using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;

namespace WaElo
{
  public class User:INotifyPropertyChanged
  {
    private XElement xe;
    private string name;
    private double elo;
    private int win;
    private int lose;

    public event PropertyChangedEventHandler PropertyChanged;

    public string Name
    {
      get { return name; }
      set
      {
        name = value;
        xe.Element(nameof(Name)).SetValue(name);
        OnPropertyChanged(nameof(Name));
      }
    }

    public double Elo
    {
      get { return elo; }
      set
      {
        elo = value;
        xe.Element(nameof(Elo)).SetValue(elo);
        OnPropertyChanged(nameof(Elo));
      }
    }

    public int Win
    {
      get { return win; }
      set
      {
        win = value;
        xe.Element(nameof(Win)).SetValue(win);
        OnPropertyChanged(nameof(Win));
      }
    }

    public int Lose
    {
      get { return lose; }
      set
      {
        lose = value;
        xe.Element(nameof(Lose)).SetValue(lose);
        OnPropertyChanged(nameof(Lose));
      }
    }

    public User(string name)
    {
      this.name = name;
      elo = 1600;
      win = 0;
      lose = 0;
      xe = new XElement(nameof(User),
        new XElement(nameof(Name), name),
        new XElement(nameof(Elo), elo),
        new XElement(nameof(Win), win),
        new XElement(nameof(Lose), lose));
      Config.Root.Add(xe);
      Config.Users.Add(this);
    }

    public User(XElement xe)
    {
      this.xe = xe;
      name = xe.Element(nameof(Name)).Value;
      elo = (double)xe.Element(nameof(Elo));
      win = (int)xe.Element(nameof(Win));
      lose = (int)xe.Element(nameof(Lose));
    }

    public void Delete()
    {
      xe.Remove();
      Config.Users.Remove(this);
    }

    public void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
