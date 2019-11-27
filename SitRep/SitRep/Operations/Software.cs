using System;
using System.Collections.Generic;
using Microsoft.Win32;


namespace SitRep.Operations.Software
{
    class Software
    {
        public Software() { }

        public Software(string bitness) { } //To be implemented with command line parsing.

        public void Enumerate()
        {
            var registryKeyDictionary = new Dictionary<string, string>
            {
                { "64_Bit_Applications", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" },
                { "32_Bit_Applications",  @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall" }
            };

            foreach (KeyValuePair<string, string> registryKeyPair in registryKeyDictionary)
            {
                if (CheckHKLMPermissions(registryKeyPair.Value))
                {
                    Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                    Console.WriteLine($"{registryKeyPair.Key}");
                    Console.WriteLine(new String('-', registryKeyPair.Key.Length));
                    SortedDictionary<string, string> resultantData = QueryHKLMRegistry(registryKeyPair.Value);

                    //Let's do this in a janky way, since I don't want to include a DLL reference for LINQ support
                    //Yes, I know it's ugly.
                    int keysLength = 0;

                    foreach (var item in resultantData)
                    {
                        if (item.Key.Length > keysLength)
                        {
                            keysLength = item.Key.Length;
                        }
                    }

                    foreach (KeyValuePair<string, string> results in resultantData)
                    {
                        Console.WriteLine($"{results.Key.PadRight(keysLength)} : {results.Value}");
                    }

                    //Janky formatting for reading...
                    Console.WriteLine(new String('=', GlobalVars.ConsoleLength));
                }
            }
        }


        private bool CheckHKLMPermissions(String registryentry)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryentry))
                {
                    string[] subkeys = key.GetSubKeyNames();

                    if (subkeys.Length >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private SortedDictionary<string, string> QueryHKLMRegistry(String registryentry)
        {
            SortedDictionary<string, string> registryResults = new SortedDictionary<string, string>();

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryentry))
                {
                    foreach (string subkeyValue in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkeyValue))
                        {
                            string currentValue = null;
                            string displayVersion = null;

                            try
                            {
                                currentValue = subkey.GetValue("DisplayName").ToString();
                            }
                            catch
                            {
                                //Filler
                            }

                            try
                            {
                                displayVersion = subkey.GetValue("DisplayVersion").ToString();
                            }
                            catch
                            {
                                //Filler
                            }


                            if (!string.IsNullOrEmpty(currentValue) && !currentValue.Contains("Update for"))
                            {
                                if (!string.IsNullOrEmpty(displayVersion))
                                {
                                    registryResults.Add(currentValue, displayVersion);
                                }
                                else
                                {
                                    registryResults.Add(currentValue, "Unknown");
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                //Nothing to do...
                //Filler
            }

            return registryResults;
        }

    }
}