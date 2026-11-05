using System;
using System.IO;
using System.Reflection;

namespace CommonUtilities
{
    public delegate string EventRecordWriter(string level, string source, int eventId, string message);

    public partial class WindowsEventLoggingUtilities
    {
        private static readonly object WriterLock = new object();
        private static EventRecordWriter recordWriter;
        [ThreadStatic]
        private static bool writingRecord;

        /// <summary>Configure once at application startup, before starting workers.</summary>
        public static void ConfigureEventRecordWriter(EventRecordWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            lock (WriterLock)
            {
                if (recordWriter != null && recordWriter != writer)
                    throw new InvalidOperationException("The event record writer is already configured.");
                recordWriter = writer;
            }
        }

        private static string LogEventViewerRecord(string level, string source, int eventId, string message)
        {
            // This check must remain relative to CommonUtilities, not the database writer assembly.
            string directory = FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
            if (!File.Exists(Path.Combine(directory, "Event_Viewer.txt"))) return "";
            if (writingRecord)
            {
                WriteToErrorEvent(message, source, eventId);
                return "";
            }
            try
            {
                writingRecord = true;
                EventRecordWriter writer;
                lock (WriterLock) { writer = recordWriter; }
                if (writer == null) return "EVENT_VIEWER logging is enabled but no event record writer is configured.";
                return writer(level, source, eventId, message) ?? "";
            }
            catch (Exception ex)
            {
                return "Error writing EVENT_VIEWER record: " + ex.Message;
            }
            finally { writingRecord = false; }
        }
    }
}
