using System;

namespace RedDog.Storage.Files
{
    public class FilesMappedDriveException : Exception
    {
        public uint ErrorCode
        {
            get;
            set;
        }

        public FilesMappedDriveException(string message, uint errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}