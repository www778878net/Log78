using System;
using www778878net.log;

namespace Test78
{
  public class CustomLogEntry : LogEntry
  {
    public DateTime Date { get; set; }
    public string Weather { get; set; }

    public CustomLogEntry()
    {
      Date = DateTime.Now;
      Weather = "Unknown";
      Basic.HostName = Environment.MachineName;
      Basic.UserName = Environment.UserName;
    }

    public CustomLogEntry(string message = "", string summary = "")
    {
      Date = DateTime.Now;
      Weather = "Unknown";
      Basic.HostName = Environment.MachineName;
      Basic.UserName = Environment.UserName;
      Basic.Message = message;
      Basic.Summary = summary;
    }
  }
}