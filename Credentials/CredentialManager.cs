using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;

namespace LogScraper.Credentials
{
    public static class CredentialManager
    {
        private const string applicationName = "LogScraper";

        public static void SaveCredentials(string target, string username, string password)
        {
            var securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }

            var credential = new NativeMethods.Credential
            {
                TargetName = target,
                Type = NativeMethods.CredentialType.Generic,
                UserName = username,
                CredentialBlob = Marshal.SecureStringToCoTaskMemUnicode(securePassword),
                CredentialBlobSize = (uint)password.Length * 2,
                Persist = (uint)NativeMethods.CredentialPersist.LocalMachine
            };

            if (!NativeMethods.CredWrite(ref credential, 0))
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(lastError);
            }

            Marshal.ZeroFreeCoTaskMemUnicode(credential.CredentialBlob);
        }

        public static NetworkCredential GetCredentials(string target)
        {
            if (NativeMethods.CredRead(target, NativeMethods.CredentialType.Generic, 0, out nint credPtr))
            {
                var credential = (NativeMethods.Credential)Marshal.PtrToStructure(credPtr, typeof(NativeMethods.Credential));
                var password = credential.CredentialBlobSize == 0 ? "" : Marshal.PtrToStringUni(credential.CredentialBlob, (int)credential.CredentialBlobSize / 2);

                NativeMethods.CredFree(credPtr);
                return new NetworkCredential(credential.UserName, password);
            }

            return null;
        }

        public static void RemoveCredentials(string target)
        {
            if (!NativeMethods.CredDelete(target, NativeMethods.CredentialType.Generic, 0))
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(lastError);
            }
        }

        public static string GenerateTargetLogProvider(string LogProviderName, string subContext)
        {
            return applicationName + ":" 
                + LogProviderName 
                + ((subContext == null) ? "" : ":" + subContext );
        }
    }
}