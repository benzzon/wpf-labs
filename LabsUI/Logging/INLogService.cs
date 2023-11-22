using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LabsUI.Logging
{
    public interface INLogService
    {
        void LogInfo(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string caller = "");
        void LogError(Exception ex, string message);
    }
}
