using System;
using System.Text;
using System.Runtime.InteropServices;

namespace RedDog.Storage.Files.Native
{
    internal static class NetworkApi
    {
        [DllImport("mpr.dll", EntryPoint = "WNetAddConnection2")]
        public static extern uint WNetAddConnection2(NETRESOURCE lpNetResource, string lpPassword, string lpUsername, uint dwFlags);

        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2")]
        public static extern uint WNetCancelConnection2(string lpName, uint dwFlags, bool fForce);

        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WNetGetConnection([MarshalAs(UnmanagedType.LPTStr)] string localName, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName, ref int length);

        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetEnumResource(IntPtr hEnum, ref int lpcCount, IntPtr lpBuffer, ref int lpBufferSize);

        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetOpenEnum(ResourceScope dwScope, ResourceType dwType, ResourceUsage dwUsage, [MarshalAs(UnmanagedType.AsAny)] [In] Object lpNetResource, out IntPtr lphEnum);

        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetCloseEnum(IntPtr hEnum);
    }
}