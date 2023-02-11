using System;
using System.Management;

namespace Filesystem.Partition
{
    public class Disk
    {
        public object deviceId;
        public string partitionQueryText;
        public ManagementObjectSearcher partitionQuery;

        public string logicalDriveQueryText;
        public ManagementObjectSearcher logicalDriveQuery;

        public string physicalName;
        public string diskName;
        public string diskModel;
        public string diskInterface;
        public UInt16[] capabilities;
        public bool mediaLoaded;
        public string mediaType;
        public UInt32 mediaSignature;
        public string mediaStatus;

        public string driveName;
        public string driveId;
        public bool driveCompressed;
        public UInt32 driveType;
        public string fileSystem;
        public UInt64 freeSpace;
        public UInt64 totalSpace;
        public uint driveMediaType;
        public string volumeName;
        public string volumeSerial;
        public Disk(ManagementObject d)
        {
            deviceId = d.Properties["DeviceId"].Value;
            //Console.WriteLine("Device");
            //Console.WriteLine(d);
            partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath);
            partitionQuery = new ManagementObjectSearcher(partitionQueryText);
            foreach (ManagementObject p in partitionQuery.Get())
            {
                //Console.WriteLine("Partition");
                //Console.WriteLine(p);
                logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", p.Path.RelativePath);
                logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
                foreach (ManagementObject ld in logicalDriveQuery.Get())
                {
                    //Console.WriteLine("Logical drive");
                    //Console.WriteLine(ld);

                    physicalName = Convert.ToString(d.Properties["Name"].Value); // \\.\PHYSICALDRIVE2
                    diskName = Convert.ToString(d.Properties["Caption"].Value); // WDC WD5001AALS-xxxxxx
                    diskModel = Convert.ToString(d.Properties["Model"].Value); // WDC WD5001AALS-xxxxxx
                    diskInterface = Convert.ToString(d.Properties["InterfaceType"].Value); // IDE
                    capabilities = (UInt16[])d.Properties["Capabilities"].Value; // 3,4 - random access, supports writing
                    mediaLoaded = Convert.ToBoolean(d.Properties["MediaLoaded"].Value); // bool
                    mediaType = Convert.ToString(d.Properties["MediaType"].Value); // Fixed hard disk media
                    mediaSignature = Convert.ToUInt32(d.Properties["Signature"].Value); // int32
                    mediaStatus = Convert.ToString(d.Properties["Status"].Value); // OK

                    driveName = Convert.ToString(ld.Properties["Name"].Value); // C:
                    driveId = Convert.ToString(ld.Properties["DeviceId"].Value); // C:
                    driveCompressed = Convert.ToBoolean(ld.Properties["Compressed"].Value);
                    driveType = Convert.ToUInt32(ld.Properties["DriveType"].Value); // C: - 3
                    fileSystem = Convert.ToString(ld.Properties["FileSystem"].Value); // NTFS
                    freeSpace = Convert.ToUInt64(ld.Properties["FreeSpace"].Value); // in bytes
                    totalSpace = Convert.ToUInt64(ld.Properties["Size"].Value); // in bytes
                    driveMediaType = Convert.ToUInt32(ld.Properties["MediaType"].Value); // c: 12
                    volumeName = Convert.ToString(ld.Properties["VolumeName"].Value); // System
                    volumeSerial = Convert.ToString(ld.Properties["VolumeSerialNumber"].Value); // 12345678

                }
            }
        }
        public void Display()
        {
            Console.WriteLine("PhysicalName: {0}", physicalName);
            Console.WriteLine("DiskName: {0}", diskName);
            Console.WriteLine("DiskModel: {0}", diskModel);
            Console.WriteLine("DiskInterface: {0}", diskInterface);
            // Console.WriteLine("Capabilities: {0}", capabilities);
            Console.WriteLine("MediaLoaded: {0}", mediaLoaded);
            Console.WriteLine("MediaType: {0}", mediaType);
            Console.WriteLine("MediaSignature: {0}", mediaSignature);
            Console.WriteLine("MediaStatus: {0}", mediaStatus);

            Console.WriteLine("DriveName: {0}", driveName);
            Console.WriteLine("DriveId: {0}", driveId);
            Console.WriteLine("DriveCompressed: {0}", driveCompressed);
            Console.WriteLine("DriveType: {0}", driveType);
            Console.WriteLine("FileSystem: {0}", fileSystem);
            Console.WriteLine("FreeSpace: {0}", freeSpace);
            Console.WriteLine("TotalSpace: {0}", totalSpace);
            Console.WriteLine("DriveMediaType: {0}", driveMediaType);
            Console.WriteLine("VolumeName: {0}", volumeName);
            Console.WriteLine("VolumeSerial: {0}", volumeSerial);

            Console.WriteLine(new string('-', 79));
        }
    }
}
