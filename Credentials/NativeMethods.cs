using System.Runtime.InteropServices;

namespace LogScraper.Credentials
{
    public static partial class NativeMethods
    {

#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

        [DllImport("Advapi32.dll", SetLastError = true, EntryPoint = "CredReadW", CharSet = CharSet.Unicode)]
        public static extern bool CredRead(string target, CredentialType type, int reservedFlag, out nint credentialPtr);

        [DllImport("Advapi32.dll", SetLastError = true, EntryPoint = "CredWriteW", CharSet = CharSet.Unicode)]
        public static extern bool CredWrite([In] ref Credential userCredential, [In] uint flags);

        [DllImport("Advapi32.dll", SetLastError = true)]
        public static extern bool CredFree([In] nint buffer);

        [DllImport("Advapi32.dll", SetLastError = true, EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode)]
        public static extern bool CredDelete(string target, CredentialType type, int flags);

#pragma warning restore CA1401 // P/Invokes should not be visible
#pragma warning restore SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

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

        public enum CredentialPersist : uint
        {
            Session = 1,
            LocalMachine = 2,
            Enterprise = 3
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct Credential
        {
            public uint Flags;
            public CredentialType Type;
            public string TargetName;
            public string Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public uint CredentialBlobSize;
            public nint CredentialBlob;
            public uint Persist;
            public uint AttributeCount;
            public nint Attributes;
            public string TargetAlias;
            public string UserName;
        }
    }
}