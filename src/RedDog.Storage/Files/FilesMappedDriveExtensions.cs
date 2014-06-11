using System;
using System.IO;
using System.Text;

using Microsoft.WindowsAzure.Storage.File;

namespace RedDog.Storage.Files
{
    public static class FilesMappedDriveExtensions
    {
        public static void Mount(this CloudFileShare share, string driveLetter, bool force = true)
        {
            if (!share.ServiceClient.Credentials.IsSharedKey)
                throw new FilesMappedDriveException("Creating a mapped drive from a CloudFileShare is only possible with SharedKey credentials.", 0);

            // Format the path \\myaccount.file.core.windows.net\myshare
            string path = String.Format(@"\\{0}\{1}", share.Uri.Host, share.Name);

            // Mount the drive.
            FilesMappedDrive.Mount(driveLetter, path, share.ServiceClient.Credentials.AccountName,
                Convert.ToBase64String(share.ServiceClient.Credentials.ExportKey()), force);
        }
    }
}
