using System;
using System.Collections.Generic;
using System.Management;
using SitRep;


namespace SitRep.Operations.ConnectedDrives
{
    class ConnectedDrives
    {
        public ConnectedDrives() { }


        public void GetDrives ()
        {
            try
            {
                InvokeWMI();
            }
            catch
            {
                Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                Console.WriteLine("Error retrieving drive information.");
            }
        }


        private void InvokeWMI()
        {
            List<string> networkeddrives = new List<string>();
            List<string> logicaldrives = new List<string>();

            SelectQuery wmiobject = new SelectQuery("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectSearcher wmiquery = new ManagementObjectSearcher(wmiobject);

            foreach (ManagementObject disk in wmiquery.Get())
            {
                if (Convert.ToUInt32(disk["DriveType"]) == 4)
                {
                    string networkdriveinformation = $"{Convert.ToString(disk["Name"])}\tUNCPATH => {Convert.ToString(disk["ProviderName"])}";
                    networkeddrives.Add(networkdriveinformation);

                }
                else
                {
                    string localdriveinformation = $"{Convert.ToString(disk["Name"])}";
                    logicaldrives.Add(localdriveinformation);
                }
            }
         
            if (logicaldrives.Count >= 1)
            {
                Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                Console.WriteLine("Physical Drives");

                foreach (string item in logicaldrives)
                {
                    Console.WriteLine(item);
                }
            }

            if (networkeddrives.Count >= 1)
            {
                Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                Console.WriteLine("Networked Drives");

                foreach (string item in networkeddrives)
                {
                    Console.WriteLine(item);
                }
            }
        }

    }
}