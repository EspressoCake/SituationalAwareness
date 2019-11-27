using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using SitRep;


namespace SitRep.Operations.Network
{
    class NetworkInterfaces
    {
        public NetworkInterfaces()
        {
            Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
            Console.WriteLine("Network Interfaces");
        }

        public void GrabInterfaces()
        {
            try
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface adapter in adapters)
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up && adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();

                        foreach (UnicastIPAddressInformation unicastinfo in properties.UnicastAddresses)
                        {
                            IPAddress currentunicast = null;
                            bool isValid = IPAddress.TryParse(unicastinfo.Address.ToString(), out currentunicast);

                            //Test IPv4, because of the default nature of the unicast address fetching both.
                            if (isValid && currentunicast.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                Console.WriteLine("Name: {0}\tAddress: {1}\tNetmask: {2}", adapter.Name.PadRight(30), unicastinfo.Address.ToString().PadRight(10), unicastinfo.IPv4Mask.ToString());
                            }
                           
                        }
                    }

                }
            }
            catch (NetworkInformationException)
            {
                Console.WriteLine("Error in grabbing network interfaces.");
            }

        }
    }
}
