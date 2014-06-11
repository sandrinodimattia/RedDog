using System.Runtime.InteropServices;

namespace RedDog.Storage.Files.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public class NETRESOURCE
    {
        public ResourceScope dwScope = 0;
        public ResourceType dwType = 0;
        public ResourceDisplayType dwDisplayType = 0;
        public ResourceUsage dwUsage = 0;
        public string lpLocalName = null;
        public string lpRemoteName = null;
        public string lpComment = null;
        public string lpProvider = null;
    }
}