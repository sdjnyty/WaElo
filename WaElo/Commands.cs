using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace WaElo
{
  public static class Commands
  {
    public static ICommand AddUserCommand { get; }
    public static ICommand DeleteUserCommand { get; }
    public static ICommand SubmitResultCommand { get; }
    public static ICommand UndoCommand { get; }
    public static ICommand LastGameCommand { get; }

    static Commands()
    {
      AddUserCommand = new AddUserCommand();
      DeleteUserCommand = new DeleteUserCommand();
      SubmitResultCommand = new SubmitResultCommand();
      UndoCommand = new UndoCommand();
      LastGameCommand = new LastGameCommand();
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
      if (Config.Users.Any(u => u.Name == parameter as string))
      {
        MessageBox.Show("玩家已存在");
        return;
      }
      new User(parameter as string);
    }
  }

  public class DeleteUserCommand : ICommand
  {
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
      return parameter != null;
    }

    public void Execute(object parameter)
    {
      var user = parameter as User;
      if (MessageBox.Show($"确认要删除玩家 {user.Name} 吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
        return;
      user.Delete();
    }
  }

  public class SubmitResultCommand : ICommand
  {
    private const double constK = 32.0;
    private User winner { get { return GlobalVars.Instance.Winner; } }
    private User loser { get { return GlobalVars.Instance.Loser; } }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
      return winner != null && loser != null && winner != loser;
    }

    public void Execute(object parameter)
    {
      var winnerElo = winner.Elo;
      var loserElo = loser.Elo;
      (Application.Current as App).History.Push(new History()
      {
        Winner = winner.Name,
        Loser = loser.Name,
        WinnerElo = winnerElo,
        LoserElo = loserElo
      });
      winner.Elo = winnerElo + constK * (1 - 1.0 / (1.0 + Math.Pow(10.0, (loserElo - winnerElo) / 400.0)));
      loser.Elo = loserElo + constK * (0 - 1.0 / (1.0 + Math.Pow(10.0, (winnerElo - loserElo) / 400.0)));
      winner.Win += 1;
      loser.Lose += 1;
    }
  }

  public class UndoCommand : ICommand
  {
    private Stack<History> history;

    public UndoCommand()
    {
      history = (Application.Current as App).History;
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
      return history.Count > 0;
    }

    public void Execute(object parameter)
    {
      var aRecord = history.Pop();
      var winner = Config.Users.First(u => u.Name == aRecord.Winner);
      winner.Win -= 1;
      winner.Elo = aRecord.WinnerElo;
      var loser = Config.Users.First(u => u.Name == aRecord.Loser);
      loser.Lose -= 1;
      loser.Elo = aRecord.LoserElo;
    }
  }

  public class LastGameCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      var wAPath = GlobalVars.Instance.WAPath;
      var file = new DirectoryInfo(Path.Combine(wAPath, @"User\Games")).GetFiles("*Online*.WAgame").OrderByDescending(f => f.CreationTime).FirstOrDefault();
      if (file == null)
      {
        MessageBox.Show("没有录像");
        return;
      }
      using (var log = new WAgameLog(Path.ChangeExtension(file.FullName, "log")))
      {
        MessageBox.Show(log.Teams.Aggregate(new StringBuilder(), (sb, t) => sb.AppendLine(t.ToString()), sb => sb.ToString()));
      }
    }
  }
}
