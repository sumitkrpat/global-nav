using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GlobalNavService.Utils.Helpers
{
   public static class IPRestrictionHelper
    {

       public static string GetIP4Address(string userHostAddress)
       {
           if (Dns.GetHostAddresses("localhost").Select(_ => _.ToString()).Contains(userHostAddress))
           {
               var firstOrDefault = Dns.GetHostAddresses("localhost").FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork);
               if (firstOrDefault != null)
                   return firstOrDefault.ToString();
           }

           var ip4Address = String.Empty;

           foreach (var ipa in Dns.GetHostAddresses(userHostAddress).Where(ipa => ipa.AddressFamily == AddressFamily.InterNetwork))
           {
               ip4Address = ipa.ToString();
               break;
           }

           if (ip4Address != String.Empty)
           {
               return ip4Address;
           }

           foreach (var ipa in Dns.GetHostAddresses(Dns.GetHostName()).Where(ipa => ipa.AddressFamily == AddressFamily.InterNetwork))
           {
               ip4Address = ipa.ToString();
               break;
           }
           return ip4Address;
       }

       public static bool IsSameIPAddress(string sourceIP, string requestIp)
       {
           IPAddress leftIP;
           IPAddress rightIP;

           if (IPAddress.TryParse(sourceIP, out leftIP) && IPAddress.TryParse(requestIp, out rightIP))
           {
               return leftIP.Equals(rightIP);
           }

           if (sourceIP.Contains("/"))
           {
               return IsInRange(requestIp, sourceIP);
           }

           return true;
       }

       public static bool IsInRange(string ipAddress, string CIDRmask)
       {
           string[] parts = CIDRmask.Split('/');

           int IP_addr = BitConverter.ToInt32(IPAddress.Parse(parts[0]).GetAddressBytes(), 0);
           int CIDR_addr = BitConverter.ToInt32(IPAddress.Parse(ipAddress).GetAddressBytes(), 0);
           int CIDR_mask = IPAddress.HostToNetworkOrder(-1 << (32 - int.Parse(parts[1])));

           return ((IP_addr & CIDR_mask) == (CIDR_addr & CIDR_mask));
       }

    }
}
