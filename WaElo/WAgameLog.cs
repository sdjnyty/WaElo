using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace WaElo
{
  public class WAgameLog : IDisposable
  {
    private bool disposedValue = false;
    private StreamReader sr;
    private string test;
    private List<Ally> allies;

    public string Test => test;

    public List<Ally> Allies => allies;

    public WAgameLog(string fileName)
    {
      allies = new List<Ally>();
      sr = new StreamReader(fileName);
      string line;
      Regex regex;
      for(;;)
      {
        line = sr.ReadLine();
        if (line == "")
          break;
      }
      regex = new Regex(@"(\w+):\s*""(.+)"" as ""(.+)""");
      for(;;)
      {
        line = sr.ReadLine();
        if (line == "")
          break;
        else
        {
          var match = regex.Match(line);
          var color = match.Groups[0].Value;
          var ally = Allies.FirstOrDefault(t => t.Color == color);
          if (ally == null)
            allies.Add(new Ally(color));
          
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

  public class Ally
  {
    public string Color { get; set; }

    public List<Player> Players { get; set; }

    public Ally(string color)
    {
      Color = color;
      Players = new List<Player>();
    }
  }

  public class Player
  {
    public string Machine { get; set; }
    
    public string Name { get; set; }
    
    public string TeamName { get; set; }
  }
}
