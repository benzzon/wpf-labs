using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Runtime.CompilerServices;

namespace LabsUI.Logging
{
    public class NLogService : INLogService
    {
        private readonly ILogger _nLogger;

        public NLogService()
        {
            _nLogger = LogManager.GetCurrentClassLogger();
        }

        public void LogInfo(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string caller = "")
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
            _nLogger.Info(message + " (" + className + "-" + caller + ")");
        }

        public void LogError(Exception ex, string message)
        {
            _nLogger.Error(ex, message);
        }
    }
}
