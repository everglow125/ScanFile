using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScanFile
{
    public class MachineUtil
    {
        /// <summary>
        /// 获取本地IP
        /// </summary>
        /// <returns></returns>
        public static string Get_UserIP()
        {
            string ip = "";
            string strHostName = Dns.GetHostName(); //得到本机的主机名
            IPHostEntry ipEntry = Dns.GetHostByName(strHostName); //取得本机IP
            if (ipEntry.AddressList.Length > 0)
            {
                ip = ipEntry.AddressList[0].ToString();
            }
            return ip;
            //string userip = "";
            //string name = Dns.GetHostName();
            //IPAddress[] ips = Dns.GetHostAddresses(name);
            //foreach (IPAddress ip in ips)
            //    userip += ip.ToString() + "|";//所有IP
            //return userip;
        }
        /// <summary>
        /// 获取主机域名
        /// </summary>
        /// <returns></returns>
        public static string Get_HostName()
        {
            return Dns.GetHostName();
        }
        /// <summary>
        /// 获取CPU编号
        /// </summary>
        /// <returns>返回一个字符串类型</returns>
        public static string Get_CPUID()
        {
            try
            {
                //需要在解决方案中引用System.Management.DLL文件
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                string strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 获取第一分区硬盘编号
        /// </summary>
        /// <returns>返回一个字符串类型</returns>
        public static string GetHardDiskID()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string strHardDiskID = null;
                foreach (ManagementObject mo in searcher.Get())
                {
                    strHardDiskID = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return strHardDiskID;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 获取网卡的MAC地址
        /// </summary>
        /// <returns>返回一个string</returns>
        public static string GetNetCardMAC()
        {
            try
            {
                string stringMAC = "";
                ManagementClass MC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection MOC = MC.GetInstances();
                foreach (ManagementObject MO in MOC)
                {
                    if ((bool)MO["IPEnabled"] == true)
                    {
                        stringMAC += MO["MACAddress"].ToString();
                    }
                }
                return stringMAC;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 获取当前网卡IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetNetCardIP()
        {
            try
            {
                string stringIP = "";
                ManagementClass MC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection MOC = MC.GetInstances();
                foreach (ManagementObject MO in MOC)
                {
                    if ((bool)MO["IPEnabled"] == true)
                    {
                        string[] IPAddresses = (string[])MO["IPAddress"];
                        if (IPAddresses.Length > 0)
                        {
                            stringIP = IPAddresses[0].ToString();
                        }
                    }
                }
                return stringIP;
            }
            catch
            {
                return "";
            }
        }
        public static string GetOutIP()
        {
            string strUrl = "http://www.ip138.com/ip2city.asp"; //获得IP的网址了 
            Uri uri = new Uri(strUrl);
            System.Net.WebRequest wr = System.Net.WebRequest.Create(uri);
            System.IO.Stream s = wr.GetResponse().GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(s, Encoding.Default);
            string all = sr.ReadToEnd(); //读取网站的数据           
            int i = all.IndexOf("[") + 1;
            string tempip = all.Substring(i, 15);
            string ip = tempip.Replace("]", "").Replace(" ", "");//找出i
            //也可用
            //new Regex(@"ClientIP: \[([\d.]+?)\]").Match(new System.Net.WebClient().DownloadString("http://www.skyiv.com/info/")).Groups[1].Value;
            return ip;
        }
    }
}
