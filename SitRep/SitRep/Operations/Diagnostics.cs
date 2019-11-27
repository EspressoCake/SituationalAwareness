using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using SitRep;


namespace SitRep.Operations.Diagnostics
{
    class DiagnosticData
    {
        public DiagnosticData()
        {
            var uptime = Uptime();
            var wmiData = OperatingSystemWMIInformation();

            if (wmiData.Count >= 1)
            {
                foreach (KeyValuePair<string, string> item in wmiData)
                {
                    Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                    Console.WriteLine("{0}\t{1}", item.Key.PadRight(20), item.Value);
                }
            }
            else
            {
                Console.WriteLine("WMI Querying Unsuccessful...");
            }

            if (uptime != null)
            {
                int weeks = uptime.Days / 7;
                int days = uptime.Days % 7;

                Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                Console.WriteLine("System Uptime");
                Console.WriteLine("Weeks:   {0}", weeks);
                Console.WriteLine("Days:    {0}", days);
                Console.WriteLine("Hours:   {0}", uptime.Hours);
                Console.WriteLine("Minutes: {0}", uptime.Minutes);
            }
            else
            {
                Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                Console.WriteLine("System Uptime Unable to be Discerned.");
            }
        }


        private TimeSpan Uptime()
        {
            var start = Stopwatch.StartNew();
            var eachStart = Stopwatch.StartNew();
            var ticks = Stopwatch.GetTimestamp();
            var uptime = ((double)ticks) / Stopwatch.Frequency;
            var uptimeTimeSpan = TimeSpan.FromSeconds(uptime);

            return uptimeTimeSpan.Subtract(start.Elapsed);
        }


        private IDictionary<string, string> OperatingSystemWMIInformation ()
        {
            IDictionary<string, string> osReturnedValues = new Dictionary<string, string>();

            try
            {
                string query = "SELECT * FROM Win32_OperatingSystem";

                ManagementObjectSearcher operatingsystemsearcher = new ManagementObjectSearcher(query);
                foreach(ManagementObject information in operatingsystemsearcher.Get())
                {
                    string oscaption = information["Caption"].ToString();
                    string version = information["Version"].ToString();
                    string architecture = information["OSArchitecture"].ToString();
                    string description = information["Description"].ToString();

                    osReturnedValues.Add("System:", oscaption);
                    osReturnedValues.Add("Version:", version);
                    osReturnedValues.Add("Bitness:", architecture);
                    osReturnedValues.Add("Description:", description);

                    return osReturnedValues;
                }
            }
            catch
            {
                //Filler
            }

            return osReturnedValues;
        }
    }
}
