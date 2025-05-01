using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;

namespace LogScraper.Credentials
{
    /// <summary>
    /// Provides methods to manage credentials, including saving, retrieving, and removing them.
    /// </summary>
    public static class CredentialManager
    {
        // The application name used as a prefix for credential targets.
        private const string applicationName = "LogScraper";

        /// <summary>
        /// Saves credentials to the Windows Credential Manager.
        /// </summary>
        /// <param name="target">The target identifier for the credentials.</param>
        /// <param name="username">The username to save.</param>
        /// <param name="password">The password to save.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">Thrown if saving the credentials fails.</exception>
        public static void SaveCredentials(string target, string username, string password)
        {
            // Convert the password to a SecureString for secure handling.
            var securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }

            // Create a credential structure to store the credentials.
            var credential = new NativeMethods.Credential
            {
                TargetName = target,
                Type = NativeMethods.CredentialType.Generic,
                UserName = username,
                CredentialBlob = Marshal.SecureStringToCoTaskMemUnicode(securePassword),
                CredentialBlobSize = (uint)password.Length * 2, // Size in bytes (UTF-16 encoding).
                Persist = (uint)NativeMethods.CredentialPersist.LocalMachine // Persist credentials locally.
            };

            // Attempt to write the credentials to the Credential Manager.
            if (!NativeMethods.CredWrite(ref credential, 0))
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(lastError);
            }

            // Free the memory allocated for the password.
            Marshal.ZeroFreeCoTaskMemUnicode(credential.CredentialBlob);
        }

        /// <summary>
        /// Retrieves credentials from the Windows Credential Manager.
        /// </summary>
        /// <param name="target">The target identifier for the credentials.</param>
        /// <returns>A <see cref="NetworkCredential"/> object containing the username and password, or null if not found.</returns>
        public static NetworkCredential GetCredentials(string target)
        {
            // Attempt to read the credentials from the Credential Manager.
            if (NativeMethods.CredRead(target, NativeMethods.CredentialType.Generic, 0, out nint credPtr))
            {
                // Marshal the credential structure from the unmanaged memory.
                var credential = (NativeMethods.Credential)Marshal.PtrToStructure(credPtr, typeof(NativeMethods.Credential));
                var password = credential.CredentialBlobSize == 0
                    ? ""
                    : Marshal.PtrToStringUni(credential.CredentialBlob, (int)credential.CredentialBlobSize / 2);

                // Free the unmanaged memory allocated for the credential.
                NativeMethods.CredFree(credPtr);

                // Return the credentials as a NetworkCredential object.
                return new NetworkCredential(credential.UserName, password);
            }

            // Return null if the credentials are not found.
            return null;
        }

        /// <summary>
        /// Removes credentials from the Windows Credential Manager.
        /// </summary>
        /// <param name="target">The target identifier for the credentials.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">Thrown if removing the credentials fails.</exception>
        public static void RemoveCredentials(string target)
        {
            // Attempt to delete the credentials from the Credential Manager.
            if (!NativeMethods.CredDelete(target, NativeMethods.CredentialType.Generic, 0))
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(lastError);
            }
        }

        /// <summary>
        /// Generates a target string for a log provider, optionally including a sub-context.
        /// </summary>
        /// <param name="LogProviderName">The name of the log provider.</param>
        /// <param name="subContext">An optional sub-context for the log provider.</param>
        /// <returns>A formatted target string.</returns>
        public static string GenerateTargetLogProvider(string LogProviderName, string subContext)
        {
            // Combine the application name, log provider name, and sub-context into a single string.
            return applicationName + ":"
                + LogProviderName
                + ((subContext == null) ? "" : ":" + subContext);
        }
    }
}