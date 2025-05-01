using System.Runtime.InteropServices;

namespace LogScraper.Credentials
{
    /// <summary>
    /// Contains P/Invoke declarations and related structures/enumerations for interacting with the Windows Credential Manager.
    /// </summary>
    public static partial class NativeMethods
    {
#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

        /// <summary>
        /// Reads a credential from the Windows Credential Manager.
        /// </summary>
        /// <param name="target">The target identifier for the credential.</param>
        /// <param name="type">The type of the credential (e.g., Generic).</param>
        /// <param name="reservedFlag">Reserved; must be 0.</param>
        /// <param name="credentialPtr">Pointer to the retrieved credential.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        [DllImport("Advapi32.dll", SetLastError = true, EntryPoint = "CredReadW", CharSet = CharSet.Unicode)]
        public static extern bool CredRead(string target, CredentialType type, int reservedFlag, out nint credentialPtr);

        /// <summary>
        /// Writes a credential to the Windows Credential Manager.
        /// </summary>
        /// <param name="userCredential">The credential to write.</param>
        /// <param name="flags">Reserved; must be 0.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        [DllImport("Advapi32.dll", SetLastError = true, EntryPoint = "CredWriteW", CharSet = CharSet.Unicode)]
        public static extern bool CredWrite([In] ref Credential userCredential, [In] uint flags);

        /// <summary>
        /// Frees memory allocated for a credential by the Windows Credential Manager.
        /// </summary>
        /// <param name="buffer">Pointer to the memory to free.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        [DllImport("Advapi32.dll", SetLastError = true)]
        public static extern bool CredFree([In] nint buffer);

        /// <summary>
        /// Deletes a credential from the Windows Credential Manager.
        /// </summary>
        /// <param name="target">The target identifier for the credential.</param>
        /// <param name="type">The type of the credential (e.g., Generic).</param>
        /// <param name="flags">Reserved; must be 0.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        [DllImport("Advapi32.dll", SetLastError = true, EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode)]
        public static extern bool CredDelete(string target, CredentialType type, int flags);

#pragma warning restore CA1401 // P/Invokes should not be visible
#pragma warning restore SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

        /// <summary>
        /// Specifies the type of credential.
        /// </summary>
        public enum CredentialType : uint
        {
            Generic = 1,
            DomainPassword = 2,
            DomainCertificate = 3,
            DomainVisiblePassword = 4,
            GenericCertificate = 5,
            DomainExtended = 6,
            Maximum = 7,
            MaximumEx = Maximum + 1000
        }

        /// <summary>
        /// Specifies the persistence options for a credential.
        /// </summary>
        public enum CredentialPersist : uint
        {
            Session = 1,       // The credential persists for the current session only.
            LocalMachine = 2,  // The credential persists on the local machine.
            Enterprise = 3     // The credential persists across the enterprise.
        }

        /// <summary>
        /// Represents a credential structure used by the Windows Credential Manager.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct Credential
        {
            public uint Flags;                        // Flags associated with the credential.
            public CredentialType Type;              // The type of the credential.
            public string TargetName;                // The name of the target for the credential.
            public string Comment;                   // A comment associated with the credential.
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten; // The last modification time of the credential.
            public uint CredentialBlobSize;          // The size of the credential blob in bytes.
            public nint CredentialBlob;              // A pointer to the credential blob (e.g., password).
            public uint Persist;                     // The persistence option for the credential.
            public uint AttributeCount;              // The number of attributes associated with the credential.
            public nint Attributes;                  // A pointer to an array of attributes.
            public string TargetAlias;               // An alias for the target.
            public string UserName;                  // The username associated with the credential.
        }
    }
}