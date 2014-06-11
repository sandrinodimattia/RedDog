using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using RedDog.Storage.Files.Native;

namespace RedDog.Storage.Files
{
    public partial class FilesMappedDrive
    {
        private const string FilesHostnameSuffix = ".file.core.windows.net";

        private const string MountError = "Unable to mount drive '{0}' to '{1}' (Error: {2}).";

        private const string UnmountError = "Unable to unmount drive '{0}' (Error: {1}).";

        /// <summary>
        /// Create a mapped drive pointing to Azure files.
        /// </summary>
        /// <param name="driveLetter"></param>
        /// <param name="filesPath"></param>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="force"></param>
        public static void Mount(string driveLetter, string filesPath, string accountName, string accountKey, bool force = true)
        {
            if (String.IsNullOrEmpty(filesPath))
                throw new ArgumentException("The filesPath is required.", "filesPath");
            if (String.IsNullOrEmpty(accountName))
                throw new ArgumentException("The accountName is required.", "accountName");
            if (String.IsNullOrEmpty(accountKey))
                throw new ArgumentException("The accountKey is required.", "accountKey");
            driveLetter = ParseDriveLetter(driveLetter);

            // Define the new resource.
            var resource = new NETRESOURCE
            {
                dwScope = (ResourceScope)2,
                dwType = (ResourceType)1,
                dwDisplayType = (ResourceDisplayType)3,
                dwUsage = (ResourceUsage)1,
                lpRemoteName = filesPath,
                lpLocalName = driveLetter
            };

            // Close connection if it exists.
            if (force)
            {
                NetworkApi.WNetCancelConnection2(driveLetter, 0, true);
            }

            // Create the connection.
            var result = NetworkApi.WNetAddConnection2(resource, accountKey, accountName, 0);
            if (result != 0)
            {
                throw new FilesMappedDriveException(String.Format(MountError, driveLetter, filesPath, (SYSTEM_ERROR)result), result);
            }
        }

        /// <summary>
        /// Unmount a mapped drive.
        /// </summary>
        /// <param name="driveLetter"></param>
        public static void Unmount(string driveLetter)
        {
            driveLetter = ParseDriveLetter(driveLetter);

            // Unmount.
            var result = NetworkApi.WNetCancelConnection2(driveLetter, 0, true);
            if (result != 0)
            {
                throw new FilesMappedDriveException(String.Format(UnmountError, driveLetter, (SYSTEM_ERROR)result), result);
            }
        }

        /// <summary>
        /// Parse the drive letter to the 'X:' format.
        /// </summary>
        /// <param name="driveLetter"></param>
        /// <returns></returns>
        private static string ParseDriveLetter(string driveLetter)
        {
            if (String.IsNullOrEmpty(driveLetter))
                throw new ArgumentException("The driveLetter is required.", "driveLetter");

            // Clean drive letter.
            driveLetter = driveLetter.Trim('\\');
            if (!driveLetter.EndsWith(":"))
                driveLetter += ":";
            return driveLetter.ToUpper();
        }

        /// <summary>
        /// Get a list of all mounted shares.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IEnumerable<FilesMappedDrive> GetMountedShares(string filter = null)
        {
            return DriveInfo.GetDrives()
                .Where(d => d.DriveType == DriveType.Network)
                .Select(d => new FilesMappedDrive(d.Name, GetDriveConnection(d)))
                .Where(d => !String.IsNullOrEmpty(d.Path) && d.Path.IndexOf(filter ?? FilesHostnameSuffix, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Find the full path to a mapped drive.
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        private static string GetDriveConnection(DriveInfo drive)
        {
            int bufferLength = 200;
            var driveName = new StringBuilder(bufferLength);
            var returnCode = NetworkApi.WNetGetConnection(drive.Name.Substring(0, 2), driveName, ref bufferLength);
            if (returnCode == 0)
                return driveName.ToString();
            return null;
        }
        
    }
}