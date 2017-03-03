using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
    public static partial class Kernel32
    {
        public static FILETIME ToFILETIME(this SYSTEMTIME st)
        {
            var ft = new FILETIME();
            SystemTimeToFileTime(ref st, ref ft);
            return ft;
        }

        public static SYSTEMTIME ToSYSTEMTIME(this FILETIME ft)
        {
            var st = new SYSTEMTIME();
            FileTimeToSystemTime(ref ft, ref st);
            return st;
        }
    }
}