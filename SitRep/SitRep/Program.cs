using SitRep.Operations.Network;
using SitRep.Operations.ConnectedDrives;
using SitRep.Operations.Diagnostics;
using SitRep.Operations.Domain;
using SitRep.Operations.Software;


namespace SitRep
{
    static class GlobalVars
    {
        public static int ConsoleLength = 90;
    }

    class Program
    {
        static void Main(string[] args)
        {
                DomainInformation currentdomain = new DomainInformation();
                currentdomain.GetCurrentDomain();
                currentdomain.CurrentPermissions();

                DiagnosticData uptime = new DiagnosticData();

                NetworkInterfaces netifaces = new NetworkInterfaces();
                netifaces.GrabInterfaces();

                ConnectedDrives netdrives = new ConnectedDrives();
                netdrives.GetDrives();

                Software installedSoftware = new Software();
                installedSoftware.Enumerate();
        }
    }
}
