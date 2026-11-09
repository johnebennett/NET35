/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 4/02/2025
 * Time: 1:33 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
namespace ReverseProxy
{
	public class Blocklist
	{
		public List<string> BlockedIps { get; set; }
		public List<string> BlockedCidrs { get; set; }
	}

	public static class CIDRHelper
	{
	    public static bool IsIPInRange(string ip, string cidr)
	    {
	        string[] parts = cidr.Split('/');
	        if (parts.Length != 2)
	            return false;
	
	        string ipAddress = parts[0];
	        int maskLength = int.Parse(parts[1]);
	
	        // Convert IP to bytes
	        byte[] ipBytes = IPAddress.Parse(ip).GetAddressBytes();
	        byte[] rangeBytes = IPAddress.Parse(ipAddress).GetAddressBytes();
	
	        if (ipBytes.Length != rangeBytes.Length)
	            return false;
	
	        // Apply the mask to both the IP and range
	        uint ipValue = ConvertToUInt32(ipBytes);
	        uint rangeValue = ConvertToUInt32(rangeBytes);
	        uint mask = GetMask(maskLength);
	
	        return (ipValue & mask) == (rangeValue & mask);
	    }
	
	    private static uint ConvertToUInt32(byte[] bytes)
	    {
	        if (bytes.Length != 4) throw new ArgumentException("Only IPv4 addresses are supported.");
	        return (uint)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
	    }
	
	    private static uint GetMask(int prefixLength)
	    {
	        if (prefixLength < 0 || prefixLength > 32)
	            throw new ArgumentOutOfRangeException("Prefix length must be between 0 and 32.");
	        
	        return (uint)(~((1U << (32 - prefixLength)) - 1));
	    }
	}
}