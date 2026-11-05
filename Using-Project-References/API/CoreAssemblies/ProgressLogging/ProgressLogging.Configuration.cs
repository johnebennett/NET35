using System;

namespace EAOfficeAPI
{
    public partial class ProgressLogging
    {
        // Char.IsWhiteSpace preserves whitespace validation on .NET Framework 3.5.
        private static bool IsWhiteSpace(string value)
        {
            foreach (char character in value)
                if (!char.IsWhiteSpace(character)) return false;
            return true;
        }

        private static readonly object ProviderLock = new object();
        private static Func<string> connectionStringProvider;
        [ThreadStatic]
        private static bool resolvingConnectionString;

        /// <summary>Configure once before any implicit Firebird logging calls.</summary>
        public static void ConfigureConnectionStringProvider(Func<string> provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            lock (ProviderLock)
            {
                if (connectionStringProvider != null && connectionStringProvider != provider)
                    throw new InvalidOperationException("The progress logging connection provider is already configured.");
                connectionStringProvider = provider;
            }
        }

        private static bool TryGetConnectionString(out string connectionString, out string errorMsg)
        {
            connectionString = "";
            errorMsg = "";
            if (resolvingConnectionString)
            {
                errorMsg = "Recursive progress logging connection provider invocation.";
                return false;
            }
            try
            {
                resolvingConnectionString = true;
                Func<string> provider;
                lock (ProviderLock) { provider = connectionStringProvider; }
                if (provider == null)
                    errorMsg = "No Firebird progress logging connection provider is configured.";
                else
                {
                    connectionString = provider();
                    if (string.IsNullOrEmpty(connectionString) || IsWhiteSpace(connectionString))
                        errorMsg = "The Firebird progress logging connection provider returned an empty connection string.";
                }
            }
            catch (Exception ex) { errorMsg = "Error obtaining the Firebird logging connection string: " + ex.Message; }
            finally { resolvingConnectionString = false; }
            return errorMsg.Length == 0;
        }
    }
}
