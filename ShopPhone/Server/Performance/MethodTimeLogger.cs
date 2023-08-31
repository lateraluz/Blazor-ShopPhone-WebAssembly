
using Serilog;
using System.Reflection;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ShopPhone.Server.Performance;


/// <summary>
/// It must has the Class Name MethodTimeLogger
/// </summary>
public static class MethodTimeLogger
{
    public static ILogger Logger =null!;
    
    public static void Log(MethodBase methodBase, TimeSpan timeSpan, string message)
    {
        // Save Log4Net   
        Logger.LogInformation($"MethodTimeLogger Metrics {methodBase.DeclaringType!.Name} {methodBase.Name} {timeSpan} {message}");
        
        // Dummy file just for testing
        //File.AppendAllText(@"C:\Temp\ShopPhone\my
        //s.txt", $"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}  {timeSpan} {message} \n");
    }

}
