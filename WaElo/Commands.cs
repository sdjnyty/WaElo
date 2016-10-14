using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace WaElo
{
  public static class Commands
  {
    public static ICommand AddUserCommand { get; }
    public static ICommand SelectWinnerCommand { get; }
    public static ICommand SelectLoserCommand { get; }
    public static ICommand SubmitResultCommand { get; }

    static Commands()
    {
      AddUserCommand = new AddUserCommand();
      SelectWinnerCommand = new SelectWinnerCommand();
      SelectLoserCommand = new SelectLoserCommand();
      SubmitResultCommand = new SubmitResultCommand();
    }
  }

  public class AddUserCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      Config.Root.Add(new XElement("User",
        new XElement("Name", parameter),
        new XElement("Elo", 1600)));
    }
  }

  public class SelectWinnerCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      (Application.Current as App).Model.Winner = parameter as XElement;
    }
  }

  public class SelectLoserCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      (Application.Current as App).Model.Loser = parameter as XElement;
    }
  }

  public class SubmitResultCommand : ICommand
  {
    private const double constK= 32.0;

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      var winner = (Application.Current as App).Model.Winner.Element("Elo");
      var loser = (Application.Current as App).Model.Loser.Element("Elo");
      var winnerElo = (int)winner;
      var loserElo = (int)loser;
      winner.SetValue((int)(winnerElo + constK * (1 - 1.0 / (1.0 + Math.Pow(10.0, (loserElo-winnerElo) / 400.0)))));
      loser.SetValue((int)(loserElo + constK * (0 - 1.0 / (1.0 + Math.Pow(10.0, ( winnerElo-loserElo) / 400.0)))));
    }
  }
}
