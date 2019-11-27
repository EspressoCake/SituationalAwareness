using System;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;


namespace SitRep.Operations.Domain
{
    class DomainInformation
    {
        public bool DomainJoined { get; private set; }

        public DomainInformation() { }

        public void GetCurrentDomain()
        {
            string PCDomain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

            if (!string.IsNullOrEmpty(PCDomain))
            {
                Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                Console.WriteLine("Doamin Type:  Domain Joined");
                Console.WriteLine("Domain Name:  {0}", PCDomain);
                Console.WriteLine("Current User: {0}", Environment.UserName);
                DomainJoined = true;
            }
            else
            {
                Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                Console.WriteLine("Domain Type:       Local");
                Console.WriteLine("Workstation Name:  {0}", Environment.UserDomainName);
                Console.WriteLine("Current User:      {0}", Environment.UserName);
            }
        }


        public void CurrentPermissions()
        {
            if (!this.DomainJoined)
            {       
                UserPrincipal user = null;
                PrincipalContext context = new PrincipalContext(ContextType.Machine, Environment.MachineName);
                List<string> localgroupslist = new List<string>();

                if ((user = UserPrincipal.FindByIdentity(context, WindowsIdentity.GetCurrent().Name)) != null)
                {
                    PrincipalSearchResult<Principal> groups = user.GetGroups();
                    foreach (GroupPrincipal individualgroup in groups)
                    {
                        localgroupslist.Add(individualgroup.ToString());
                    }
                }

                if (localgroupslist.Count >= 1)
                {
                    localgroupslist.Sort();
                    Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                    Console.WriteLine("Groups: {0}", string.Join(", ", localgroupslist.ToArray()));
                }
            }
        }
    }
}


