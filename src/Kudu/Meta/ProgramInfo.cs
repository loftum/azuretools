using System.Diagnostics;

namespace Kudu.Meta
{
    public static class ProgramInfo
    {
        public static string FullName => $"{Name} {Version}";
        public static readonly string Name;
        public static string Version;

        static ProgramInfo()
        {
            Name = Process.GetCurrentProcess().ProcessName;
            Version = typeof (ProgramInfo).Assembly.GetName().Version.ToString();
        }
    }
}