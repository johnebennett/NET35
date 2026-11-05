using System;
using CommonUtilities;

namespace EAOfficeAPI
{
    /// <summary>Application-wide logging composition. Call before constructing actions or starting workers.</summary>
    public static class CoreRuntimeInitialization
    {
        // Char.IsWhiteSpace preserves whitespace validation on .NET Framework 3.5.
        private static bool IsWhiteSpace(string value)
        {
            foreach (char character in value)
                if (!char.IsWhiteSpace(character)) return false;
            return true;
        }

        private static readonly object InitializationLock = new object();
        private static readonly Func<string> DefaultProvider = CommonDBMethods.Common.ConnectionString;
        private static Func<string> configuredProvider;

        public static void InitializeLogging()
        {
            InitializeLogging(DefaultProvider);
        }

        public static void InitializeLogging(APISettingsItem settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            InitializeLogging(delegate { return CommonDBMethods.Common.ConnectionString(settings); });
        }

        public static void InitializeLogging(Func<string> provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            lock (InitializationLock)
            {
                if (configuredProvider != null)
                {
                    if (configuredProvider != provider)
                        throw new InvalidOperationException("Core logging is already configured with another provider.");
                    return;
                }
                ProgressLogging.ConfigureConnectionStringProvider(provider);
                WindowsEventLoggingUtilities.ConfigureEventRecordWriter(delegate(string level, string source, int eventId, string message)
                {
                    string connectionString = provider();
                    if (string.IsNullOrEmpty(connectionString) || IsWhiteSpace(connectionString))
                        return "The EVENT_VIEWER connection provider returned an empty connection string.";
                    return UsefulDBMethods.EventViewerLogging.WriteRecord(connectionString, level, source, eventId, message);
                });
                configuredProvider = provider;
            }
        }
    }
}
