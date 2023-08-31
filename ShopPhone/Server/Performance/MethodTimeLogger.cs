
using log4net;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore.Extensions;
using System.Reflection;

namespace ShopPhone.Server.Performance;


/// <summary>
/// It must has the Class Name MethodTimeLogger
/// </summary>
public static class MethodTimeLogger
{
    public static ILog Logger =null!;
    
    public static void Log(MethodBase methodBase, TimeSpan timeSpan, string message)
    {
        // Save Log4Net   
        Logger.Info($"{methodBase.DeclaringType!.Name} {methodBase.Name} {timeSpan} {message}");
        
        // Dummy file just for testing
        //File.AppendAllText(@"C:\Temp\ShopPhone\my
        //s.txt", $"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}  {timeSpan} {message} \n");
    }

}
