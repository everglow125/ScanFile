namespace ScanFile
{
    using System;
    using System.IO;
    using System.Management;
    using System.Net;
    using System.Text;

    public class MachineUtil
    {
        public static string Get_CPUID()
        {
            try
            {
                ManagementObjectCollection instances = new ManagementClass("Win32_Processor").GetInstances();
                string str = null;
                foreach (ManagementObject obj2 in instances)
                {
                    str = obj2.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        public static string Get_HostName()
        {
            return Dns.GetHostName();
        }

        public static string Get_UserIP()
        {
            string str = "";
            IPHostEntry hostByName = Dns.GetHostByName(Dns.GetHostName());
            if (hostByName.AddressList.Length > 0)
            {
                str = hostByName.AddressList[0].ToString();
            }
            return str;
        }

        public static string GetHardDiskID()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string str = null;
                foreach (ManagementObject obj2 in searcher.Get())
                {
                    str = obj2["SerialNumber"].ToString().Trim();
                    break;
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        public static string GetNetCardIP()
        {
            try
            {
                string str = "";
                ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
                foreach (ManagementObject obj2 in instances)
                {
                    if ((bool)obj2["IPEnabled"])
                    {
                        string[] strArray = (string[])obj2["IPAddress"];
                        if (strArray.Length > 0)
                        {
                            str = strArray[0].ToString();
                        }
                    }
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        public static string GetNetCardMAC()
        {
            try
            {
                string str = "";
                ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
                foreach (ManagementObject obj2 in instances)
                {
                    if ((bool)obj2["IPEnabled"])
                    {
                        str = str + obj2["MACAddress"].ToString();
                    }
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        public static string GetOutIP()
        {
            string uriString = "http://www.ip138.com/ip2city.asp";
            Uri requestUri = new Uri(uriString);
            string str2 = new StreamReader(WebRequest.Create(requestUri).GetResponse().GetResponseStream(), Encoding.Default).ReadToEnd();
            int startIndex = str2.IndexOf("[") + 1;
            return str2.Substring(startIndex, 15).Replace("]", "").Replace(" ", "");
        }
    }
}

