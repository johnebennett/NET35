/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 3/12/2024
 * Time: 1:45 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace ReverseProxy
{
	/// <summary>
	/// Description of DomainConfig.
	/// </summary>
	public class DomainConfig
	{
	    public List<string> Domains { get; set; }
	    public string TargetMachineName { get; set; }
	    public string TargetIp { get; set; }
	    public int TargetPort { get; set; }
		public List<string> AllowedPaths { get; set; }
		public List<string> AllowedClientIps { get; set; }
		public List<string> AllowedClientRanges { get; set; }

		[JsonIgnore]
		private HashSet<IPAddress> allowedClientIpAddresses = new HashSet<IPAddress>();

		[JsonIgnore]
		private List<ClientIpRange> allowedClientIpRanges = new List<ClientIpRange>();

		[JsonIgnore]
		public bool HasClientIpRestriction
		{
			get
			{
				return (AllowedClientIps != null && AllowedClientIps.Count > 0) ||
					(AllowedClientRanges != null && AllowedClientRanges.Count > 0);
			}
		}

		public bool IsClientIpAllowed(string clientIp)
		{
			if (!HasClientIpRestriction)
			{
				return true;
			}

			IPAddress address;
			if (!IPAddress.TryParse(clientIp, out address))
			{
				return false;
			}

			address = NormalizeAddress(address);
			if (allowedClientIpAddresses.Contains(address))
			{
				return true;
			}

			foreach (ClientIpRange range in allowedClientIpRanges)
			{
				if (range.Contains(address))
				{
					return true;
				}
			}

			return false;
		}

		private void InitializeClientIpRestrictions()
		{
			allowedClientIpAddresses = new HashSet<IPAddress>();
			allowedClientIpRanges = new List<ClientIpRange>();

			if (AllowedClientIps != null)
			{
				foreach (string configuredIp in AllowedClientIps)
				{
					if (string.IsNullOrWhiteSpace(configuredIp))
					{
						throw new FormatException("AllowedClientIps contains an empty value.");
					}

					IPAddress address;
					if (!IPAddress.TryParse(configuredIp.Trim(), out address))
					{
						throw new FormatException("Invalid allowed client IP address: " + configuredIp);
					}

					allowedClientIpAddresses.Add(NormalizeAddress(address));
				}
			}

			if (AllowedClientRanges != null)
			{
				foreach (string configuredRange in AllowedClientRanges)
				{
					allowedClientIpRanges.Add(new ClientIpRange(configuredRange));
				}
			}
		}

		private static IPAddress NormalizeAddress(IPAddress address)
		{
			return address.IsIPv4MappedToIPv6 ? address.MapToIPv4() : address;
		}

	
		public static List<DomainConfig> LoadConfig(string configFilePath)
		{
		    string json = File.ReadAllText(configFilePath);
		    List<DomainConfig> configs = JsonConvert.DeserializeObject<List<DomainConfig>>(json);
		    if (configs == null)
		    {
		    	throw new FormatException("Domain configuration is empty or invalid.");
		    }

		    foreach (DomainConfig config in configs)
		    {
		    	if (config == null)
		    	{
		    		throw new FormatException("Domain configuration contains a null entry.");
		    	}

		    	try
		    	{
		    		config.InitializeClientIpRestrictions();
		    	}
		    	catch (Exception ex)
		    	{
		    		string domains = config.Domains == null ? "(no domains)" : string.Join(", ", config.Domains);
		    		throw new FormatException("Invalid client IP restriction for domain(s) " + domains + ": " + ex.Message, ex);
		    	}
		    }

		    return configs;
		}
	}

	internal sealed class ClientIpRange
	{
		private readonly byte[] networkBytes;
		private readonly int prefixLength;

		public ClientIpRange(string cidr)
		{
			if (string.IsNullOrWhiteSpace(cidr))
			{
				throw new FormatException("AllowedClientRanges contains an empty value.");
			}

			string[] parts = cidr.Trim().Split('/');
			IPAddress network;
			int prefix;
			if (parts.Length != 2 || !IPAddress.TryParse(parts[0], out network) || !int.TryParse(parts[1], out prefix))
			{
				throw new FormatException("Invalid allowed client CIDR range: " + cidr);
			}

			network = network.IsIPv4MappedToIPv6 ? network.MapToIPv4() : network;
			networkBytes = network.GetAddressBytes();
			int maximumPrefix = networkBytes.Length * 8;
			if (prefix < 0 || prefix > maximumPrefix)
			{
				throw new FormatException("Invalid prefix length in allowed client CIDR range: " + cidr);
			}

			prefixLength = prefix;
		}

		public bool Contains(IPAddress address)
		{
			if (address.IsIPv4MappedToIPv6)
			{
				address = address.MapToIPv4();
			}

			byte[] addressBytes = address.GetAddressBytes();
			if (addressBytes.Length != networkBytes.Length)
			{
				return false;
			}

			int wholeBytes = prefixLength / 8;
			int remainingBits = prefixLength % 8;
			for (int i = 0; i < wholeBytes; i++)
			{
				if (addressBytes[i] != networkBytes[i])
				{
					return false;
				}
			}

			if (remainingBits == 0)
			{
				return true;
			}

			int mask = 0xFF << (8 - remainingBits);
			return (addressBytes[wholeBytes] & mask) == (networkBytes[wholeBytes] & mask);
		}
	}
}
