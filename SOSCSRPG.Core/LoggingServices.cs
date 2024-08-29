using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Core
{

    public static class LoggingServices
    {
        private const string LOG_FILE_DIRECTORY = "Logs";

        static LoggingServices()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LOG_FILE_DIRECTORY);

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public static void Log(Exception exception, bool isInnerException = false)
        {
            using (StreamWriter sw = new StreamWriter(LogFileName(), true))
            {
                sw.WriteLine(isInnerException ? "INNER EXCEPTION" : $"EXCEPTION: {DateTime.Now}");
                sw.WriteLine(new string(isInnerException ? '-' : '=', 40));
                sw.WriteLine($"{exception.Message}");
                sw.WriteLine($"{exception.StackTrace}");

                sw.WriteLine();

                if (exception.InnerException != null)
                {
                    Log(exception.InnerException, true);
                }
            }
        }

        private static string LogFileName()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LOG_FILE_DIRECTORY, $"SOSCSRPG_{DateTime.Now:yyyMMdd}.log");
        }
    }
}

