using System;
using System.Threading.Tasks;
using www778878net.log;

namespace www778878net.log
{
  public interface ILog78
  {
    Task DETAIL(string summary, object? message = null, int level = 10);
    Task DEBUG(string summary, object? message = null, int level = 20);
    Task INFO(string summary, object? message = null, int level = 30);
    Task WARN(string summary, object? message = null, int level = 50);
    Task ERROR(string summary, object? message = null, int level = 60);
    Task ERROR(Exception error, string? summary = null, int level = 60);
    void SetEnvironment(Log78.Environment env);
    void SetupLevel(int fileLevel, int consoleLevel, int apiLevel);
    void AddDebugKey(string key);
    // 添加其他需要公开的方法...
  }
}