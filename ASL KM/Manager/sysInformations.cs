using ASL_KM.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace ASL_KM.Manager
{
    public class sysInformations
    {
        
        public List<Network> GetNetworkList()
        {
            List<Network> list = new List<Network>();
            foreach (var network in NetworkInterface.GetAllNetworkInterfaces().Where(c => c.OperationalStatus == OperationalStatus.Up && !String.IsNullOrEmpty(c.GetPhysicalAddress().ToString())))
            {
                list.Add(new Network
                {
                    Adi = network.Name,
                    Aciqlama = network.Description,
                    MacAdress = network.GetPhysicalAddress().ToString()
                });
            }
            return list;
        }
        private Bios GetBiosInfo()
        {
            ManagementObjectSearcher management = new ManagementObjectSearcher("Select SMBIOSBIOSVersion, Caption, Manufacturer, SerialNumber, ReleaseDate from WIN32_BIOS");
            ManagementObjectCollection collection = management.Get();
            
            Bios bios = new Bios();
            foreach (var prop in collection)
            {
                bios.Version = prop["SMBIOSBIOSVersion"].ToString();
                bios.Caption = prop["Caption"].ToString();
                bios.Manufacturer = prop["Manufacturer"].ToString();
                bios.SerialNumber = prop["SerialNumber"].ToString();
                bios.RealeaseDate = ManagementDateTimeConverter.ToDateTime(prop["ReleaseDate"].ToString()).ToString();
            }
            return bios;
        }
        private BaseBoard BaseBoardInfo()
        {            
            ManagementObjectSearcher management = new ManagementObjectSearcher("Select Name, Caption, Manufacturer, SerialNumber, Product from Win32_BaseBoard");
            ManagementObjectCollection collection = management.Get();

            BaseBoard baseBoard  = new BaseBoard();
            foreach (var prop in collection)
            {
                baseBoard.Name = prop["Name"].ToString();
                baseBoard.Manufacturer = prop["Manufacturer"].ToString();
                baseBoard.SerialNumber = prop["SerialNumber"].ToString();
                baseBoard.Product = prop["Product"].ToString();
            }
            return baseBoard;
        }
        private CPU CPUInfo()
        {
            ManagementObjectSearcher management = new ManagementObjectSearcher("Select Name, Caption, ProcessorId, DeviceID, NumberOfCores from Win32_Processor");
            ManagementObjectCollection collection = management.Get();

            CPU cpu = new CPU();
            foreach (var prop in collection)
            {
                cpu.Name = prop["Name"].ToString();
                cpu.Caption = prop["Caption"].ToString();
                cpu.ProcessorId = prop["ProcessorId"].ToString();
                cpu.DeviceID = prop["DeviceID"].ToString();
                cpu.NumberOfCores = prop["NumberOfCores"].ToString();
            }
            return cpu;
        }
        private OS OSInfo()
        {
            ManagementObjectSearcher management = new ManagementObjectSearcher("Select Name, Caption, SerialNumber, RegisteredUser from Win32_OperatingSystem");
            ManagementObjectCollection collection = management.Get();

            OS os = new OS();
            foreach (var prop in collection)
            {
                os.Name = prop["Name"].ToString();
                os.Caption = prop["Caption"].ToString();
                os.SerialNumber = prop["SerialNumber"].ToString();
                os.RegisteredUser = prop["RegisteredUser"].ToString();
            }
            return os;
        }
        public List<DiskDrive> DiskDriveInfo()
        {
            List<DiskDrive> list = new List<DiskDrive>();
            ManagementObjectSearcher DiskManegement = new ManagementObjectSearcher("Select  Name, SerialNumber, Caption, Model, DeviceID from Win32_DiskDrive");
            foreach (ManagementObject drive in DiskManegement.Get())
            {    
                ManagementObjectSearcher PartitionManegement = new ManagementObjectSearcher(String.Format("associators of {{{0}}} where AssocClass=Win32_DiskDriveToDiskPartition", drive.Path.RelativePath));
                foreach (ManagementObject Partition in PartitionManegement.Get())
                {
                    ManagementObjectSearcher LogicalDiskManegement = new ManagementObjectSearcher(String.Format("associators of {{{0}}} where AssocClass=Win32_LogicalDiskToPartition", Partition.Path.RelativePath));
                    foreach (ManagementObject LogicalDisk in LogicalDiskManegement.Get())
                    {
                        list.Add(new DiskDrive
                        {
                            Name = drive["Name"].ToString(),
                            Caption = drive["Caption"].ToString(),
                            DeviceID = drive["DeviceID"].ToString(),
                            Model = drive["Model"].ToString(),
                            SerialNumber = drive["SerialNumber"].ToString(),
                            PartitionName = LogicalDisk["Name"].ToString(),
                            MediaType = LogicalDisk["MediaType"].ToString(),
                            VolumeSerialNumber = LogicalDisk["VolumeSerialNumber"].ToString(),
                            FileSystem = LogicalDisk["FileSystem"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public List<Bios> ListBiosInfo()
        {
            List<Bios> list = new List<Bios>();
            list.Add(GetBiosInfo());

            return list;
        }
        public List<BaseBoard> ListBaseBoardInfo()
        {
            List<BaseBoard> list = new List<BaseBoard>();
            list.Add(BaseBoardInfo());

            return list;
        }
        public List<CPU> ListCPUInfo()
        {
            List<CPU> list = new List<CPU>();
            list.Add(CPUInfo());

            return list;
        }
        public List<OS> ListOSInfo()
        {
            List<OS> list = new List<OS>();
            list.Add(OSInfo());

            return list;
        }
    }
}
