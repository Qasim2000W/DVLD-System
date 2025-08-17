using System;
using System.Diagnostics;

namespace ClsDataAccess
{
    public class ClsEventLog
    {
        

        public enum ENTypeMessage
        {
            information=1,
            warning=2,
            Error=3
        }

        public static void EventLogger(string Message , ENTypeMessage eNType)
        {
            string SourceName = "DVLD";

            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
            }

            switch (eNType)
            {
                case ENTypeMessage.information:
                EventLog.WriteEntry(SourceName, Message, EventLogEntryType.Information);
                    break;

                case ENTypeMessage.warning:
                EventLog.WriteEntry(SourceName, Message, EventLogEntryType.Warning);
                    break;

                case ENTypeMessage.Error:
                EventLog.WriteEntry(SourceName, Message, EventLogEntryType.Error);
                    break;
            }
        }
    }
}