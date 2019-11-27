using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SitRep.Options
{
    class CommandOptions
    {
        [Option('a', "default", HelpText = "Default, all checks.", Required = false, Default = false)]
        public bool Defaultsettings { get; set; }

        [Option('i', "interfaces", HelpText = "Get network interfaces", Required = false)]
        public bool Interfaces { get; set; }

        [Option('m', "mapped-drives", HelpText = "Get mapped network drives", Required = false)]
        public bool Mappeddrives { get; set; }

        [Option('d', "domain", HelpText = "Get domain information", Required = false)]
        public bool Domain { get; set; }

        [Option('w', "whoami", HelpText = "Information on who we are.", Required = false)]
        public bool Whoami { get; set; }
    }
}
