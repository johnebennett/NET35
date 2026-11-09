/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 5/02/2025
 * Time: 5:16 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Linq;

namespace ReverseProxy
{
	/*
	public class IPBlocker
	{
	    private HashSet<string> blockedIPs;
	    private List<IPRange> blockedRanges;
	
	    public IPBlocker()
	    {
	        LoadBlockedIPs("blocked_ips.json");
	        LoadBlockedRanges("blocked_ranges.json");
	    }
	
	    private void LoadBlockedIPs(string filePath)
	    {
	        if (File.Exists(filePath))
	        {
	            var json = File.ReadAllText(filePath);
	            var data = JsonConvert.DeserializeObject<BlockedIPList>(json);
	            blockedIPs = new HashSet<string>(data.BlockedIPs);
	        }
	        else
	        {
	            blockedIPs = new HashSet<string>();
	        }
	    }
	
		private void LoadBlockedRanges(string filePath)
		{
		    if (File.Exists(filePath))
		    {
		        var json = File.ReadAllText(filePath);
		        var data = JsonConvert.DeserializeObject<BlockedRangeList>(json);
		
		        blockedRanges = (data != null && data.BlockedRanges != null) 
		            ? data.BlockedRanges.Select(ParseIPRange).ToList() 
		            : new List<IPRange>();
		    }
		    else
		    {
		        blockedRanges = new List<IPRange>();
		    }
		}
	
	    public bool IsIPBlocked(string ipAddress)
	    {
	        if (blockedIPs.Contains(ipAddress))
	        {
	            return true;
	        }
	
	        IPAddress ip;
	        if (IPAddress.TryParse(ipAddress, out ip))
	        {
	            foreach (var range in blockedRanges)
	            {
	                if (range.Contains(ip))
	                {
	                    return true;
	                }
	            }
	        }
	
	        return false;
	    }
	
	    private IPRange ParseIPRange(string cidr)
	    {
	        var parts = cidr.Split('/');
	        if (parts.Length != 2) throw new ArgumentException("Invalid CIDR format");
	
	        IPAddress startIP = IPAddress.Parse(parts[0]);
	        int prefixLength = int.Parse(parts[1]);
	
	        return new IPRange(startIP, prefixLength);
	    }
	}
	*/
	
	public class BlockedIPList
	{
	    public List<string> BlockedIPs { get; set; }
	}
	
	public class BlockedRangeList
	{
	    public List<string> BlockedRanges { get; set; }
	}
	
	public class IPRange
	{
	    private readonly IPAddress start;
	    private readonly IPAddress end;
	
	    public IPRange(string cidr)
	    {
	        if (cidr.Contains("/"))
	        {
	            var parts = cidr.Split('/');
	            start = IPAddress.Parse(parts[0]);
	            end = GetBroadcastAddress(start, int.Parse(parts[1]));
	        }
	        else
	        {
	            start = end = IPAddress.Parse(cidr);
	        }
	    }
	
	    private IPAddress GetBroadcastAddress(IPAddress ip, int prefix)
	    {
	        uint ipAsInt = BitConverter.ToUInt32(ip.GetAddressBytes().Reverse().ToArray(), 0);
	        uint mask = 0xFFFFFFFF << (32 - prefix);
	        uint endIp = ipAsInt | ~mask;
	        return new IPAddress(BitConverter.GetBytes(endIp).Reverse().ToArray());
	    }
	
	    public bool Contains(string ip)
	    {
	        var checkIp = IPAddress.Parse(ip);
	        var checkIpInt = BitConverter.ToUInt32(checkIp.GetAddressBytes().Reverse().ToArray(), 0);
	        var startInt = BitConverter.ToUInt32(start.GetAddressBytes().Reverse().ToArray(), 0);
	        var endInt = BitConverter.ToUInt32(end.GetAddressBytes().Reverse().ToArray(), 0);
	
	        return checkIpInt >= startInt && checkIpInt <= endInt;
	    }
	}
}