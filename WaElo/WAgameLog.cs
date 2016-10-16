using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace WaElo
{
  public class WAgameLog : IDisposable
  {
    private bool disposedValue = false;
    private StreamReader sr;
    private List<Team> teams;

    public List<Team> Teams => teams;

    public WAgameLog(string fileName)
    {
      teams = new List<Team>();
      sr = new StreamReader(fileName,Encoding.GetEncoding(1252));
      string line;
      Regex regex;
      for(;;)
      {
        line = sr.ReadLine();
        if (line == "")
          break;
      }
      regex = new Regex(@"^(\w+):\s+""(.+)""\s+as ""(.+)""");
      for(;;)
      {
        line = sr.ReadLine();
        if (line == "")
          break;
        else
        {
          var match = regex.Match(line);
          teams.Add(new Team(match.Groups[1].Value, match.Groups[3].Value, match.Groups[2].Value));
        }
      }
      regex = new Regex(@"^\[\d{2}:\d{2}:\d{2}\.\d{2}\] (?:\[.+\] .+|•{3} (?:Damage dealt: (?<DMG>.+)$|resetting Jet Pack fuel use to \d+|Game Ends -|.+ \(.+\) .+)|\*{3} (?:(?<DROP>.+) disconnected due to (.+)$|.+ \((?<DROP>.+)\) forced out by disconnection of host))");
      var dmgRegex = new Regex(@"\d+ (?:\((?<KILLS>\d) kill\) )?to (?<KILLED>.+) \(.+\)");
      for(;;)
      {
        line = sr.ReadLine();
        if (line == "")
          break;
        else
        {
          var match = regex.Match(line);
          var dropGroup = match.Groups["DROP"];
          if (dropGroup.Success)
            teams.First(t => t.PlayerName == dropGroup.Value).Dropped = true;
          var dmgGroup = match.Groups["DMG"];
          if(dmgGroup.Success)
          {
            var damages = dmgGroup.Value.Split(new string[] { ", " }, StringSplitOptions.None);
            foreach(var damage in damages)
            {
              var groups = dmgRegex.Match(damage).Groups;
              var killsGroup = groups["KILLS"];
              if (killsGroup.Success)
                Teams.First(t => t.Name == groups["KILLED"].Value).Killed += int.Parse(killsGroup.Value);
            }
          }
        }
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
          sr.Close();
        disposedValue = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
    }

  }

  public class Team
  {
    public string Color { get; set; }

    public string PlayerName { get; set; }

    public string Name { get; set; }

    public int Killed { get; set; }

    public bool Dropped { get; set; }

    public Team(string color, string name,string playerName)
    {
      Color = color;
      Name = name;
      PlayerName = playerName;
      Killed = 0;
      Dropped = false;
    }

    public override string ToString()
    {
      return $"{Color}\t{Name}\t{PlayerName}\t{Dropped}\t{Killed}";
    }
  }
}
