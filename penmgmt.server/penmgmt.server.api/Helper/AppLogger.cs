using System;

namespace PenMgmt.Common.Helper
{
    public class AppLogger : IAppLogger
    {
        public void LogMessage(string message)
        {
            LogMessageInternal(message);
        }

        public void LogWarning(string message)
        {
            LogMessageInternal(message);

        }
        public void LogError(string message)
        {
            LogMessageInternal(message);
        }

        private void LogMessageInternal(string message)
        {
            Console.WriteLine(message);
        }
    }
}