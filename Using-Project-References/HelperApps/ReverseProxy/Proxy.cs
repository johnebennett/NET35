/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 4/02/2025
 * Time: 4:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Linq;
using ReverseProxy.Actions;

namespace ReverseProxy
{
	/// <summary>
	/// Description of Proxy.
	/// </summary>
	public class Proxy
	{
		//private List<string> blockedIPs;
		//private List<string> blockedCIDRs;
		
		private List<IPRange> blockedRanges = new List<IPRange>();
		private HashSet<string> blockedIPs = new HashSet<string>();

		public void Execute()
		{
		    string configFilePath = "DomainConfig.json";
		    List<DomainConfig> domainConfigs = DomainConfig.LoadConfig(configFilePath);
/* 			var allowedHosts = domainConfigs
				.Select(dc => dc.Domain.ToLower())
				.ToList();
 */
		    configFilePath = "blocked_ips.json";
		    LoadBlockedIPs(configFilePath);    
		    if (blockedIPs == null) Console.WriteLine("blockedIPs IS NULL");
		    Console.WriteLine("Blocked IP Check Count: " + blockedIPs.Count());
		    
		    configFilePath = "blocked_ranges.json";
		    LoadBlockedRanges(configFilePath);
		    if (blockedRanges == null) Console.WriteLine("blocked Ranges IS NULL");
		    Console.WriteLine("blocked Range Check Count: " + blockedRanges.Count());
		    
		    HttpListener listener = new HttpListener();
		    listener.Prefixes.Add("http://*:80/");
		    listener.Start();
		
		    Console.WriteLine("Proxy server is running...");
		
		    while (true)
		    {
		        HttpListenerContext context = listener.GetContext();
		        
		        string RequesterIP = context.Request.RemoteEndPoint.Address.ToString();
		        
		        string HostId = context.Request.Headers["Host"];
		        Console.WriteLine("Request from: " + HostId);
		        CommonMethods.WriteToOutputLog("Request from: " + HostId);
		        string PathAndQuery = context.Request.Url.PathAndQuery;
		        Console.WriteLine("Request URL: " + PathAndQuery);
		        CommonMethods.WriteToOutputLog("Request Url: " + PathAndQuery);
		        string Requester = context.Request.RemoteEndPoint.Address.ToString();
		        Console.WriteLine("Requester: " + Requester);
		        CommonMethods.WriteToOutputLog("Requester: " + Requester);		        
		        CommonMethods.WriteToReferrerLog(Requester);

				// --------------------------------------------
				// 1. Reject if Host header not in allowed list
				// --------------------------------------------
				/*
				bool isAllowedHost =
					allowedHosts.Any(h => HostId.StartsWith(h));
				*/

				var matchedConfig = domainConfigs.FirstOrDefault(c =>
					c.Domains.Any(d => d.Equals(HostId, StringComparison.OrdinalIgnoreCase)));

				//if (!isAllowedHost)
				if (matchedConfig == null)
				{
					context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
					context.Response.StatusDescription = "Forbidden";
					context.Response.Close();

					Console.WriteLine("Rejected request — Host not allowed: " + HostId);
					CommonMethods.WriteToOutputLog("Rejected Host: " + HostId);
					Console.WriteLine("Time: " + DateTime.Now.ToString("yyyyMMdd-HH:mm:ss"));

					continue;
				}

				// A domain is unrestricted when both allow lists are absent or empty.
				// When configured, reject every client that is not explicitly allowed.
				if (!matchedConfig.IsClientIpAllowed(RequesterIP))
				{
					context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
					context.Response.StatusDescription = "Forbidden";
					context.Response.Close();

					Console.WriteLine(string.Format("Rejected request to {0} from client IP not allowed for this site: {1}", HostId, RequesterIP));
					CommonMethods.WriteToOutputLog(string.Format("Rejected client IP for Host {0}: {1}", HostId, RequesterIP));
					continue;
				}

		        // check if IP is in the blocklist
		        if (IsIPBlocked(RequesterIP))
		        {
		        	// return forbidden response if the IP is blocked
		        	context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
		        	context.Response.StatusDescription = "Forbidden";
		        	context.Response.Close();
		        	Console.WriteLine(string.Format("Blocked request from {0}", RequesterIP));
		        	CommonMethods.WriteToOutputLog(string.Format("Blocked request from {0}", RequesterIP));
		        	continue;
		        }		        
		        
		        ProcessRequest(context, domainConfigs); // Pass the domainConfigs to ProcessRequest
		    }
		}
	    
	    private Blocklist LoadBlocklist(string filePath)
	    {
	    	Console.WriteLine("Inside LoadBlocklist");
	    	string json = File.ReadAllText(filePath);
	    	return JsonConvert.DeserializeObject<Blocklist>(json);
	    }
	
		/*
	    private bool IsIPBlocked(string requesterIP)
	    {
	    	bool Result = false;
	        // For simplicity, just checking exact IP matches here, but this can be expanded for ranges.
	        Console.WriteLine("Checking IP: " + requesterIP);

	        if (blockedIPs.Contains(requesterIP)) Result = true;
	       
	        if (!Result)
	        {
	        	Console.WriteLine("Checking IP: " + requesterIP + " in CIDR List");
	        	foreach (var cidr in blockedCIDRs)
	       		{
		       		if (CIDRHelper.IsIPInRange(requesterIP, cidr))
		       		{
		       			Result = true;
		       			break;
		       		}
		       	}
	       }	       
	       
	        return Result;
		}		
		*/
		
		private void ProcessRequest(HttpListenerContext context, List<DomainConfig> domainConfigs)
		{
		    HttpListenerRequest request = context.Request;
		    HttpListenerResponse response = context.Response;
		
		    // Determine target server from config
		    string requestHost = request.Url.Host;
			requestHost = request.Headers["Host"];
		    //DomainConfig config = domainConfigs.FirstOrDefault(dc => dc.Domain.Equals(requestHost, StringComparison.OrdinalIgnoreCase));
			DomainConfig config = domainConfigs.FirstOrDefault(dc => dc.Domains.Any(d => d.Equals(requestHost,  StringComparison.OrdinalIgnoreCase)));

		    if (config == null)
		    {
		        response.StatusCode = (int)HttpStatusCode.NotFound;
		        response.Close();
		        return;
		    }
		
			string targetUrl = string.Format("{0}://{1}:{2}{3}",
			    request.IsSecureConnection ? "https" : "http",
			    config.TargetIp,
			    config.TargetPort,
			    request.Url.PathAndQuery);
		
		    try
		    {
		        HttpWebRequest proxyRequest = (HttpWebRequest)WebRequest.Create(targetUrl);
				proxyRequest.AllowAutoRedirect = false; // Prevent automatic redirects
		        proxyRequest.Method = request.HttpMethod;
		        
		        string HostId = request.Headers["Host"];
		        Console.WriteLine("Request from: " + HostId);
		        CommonMethods.WriteToOutputLog("Request from: " + HostId);
		        string PathAndQuery = request.Url.PathAndQuery;
		        Console.WriteLine("Request URL: " + PathAndQuery);
		        CommonMethods.WriteToOutputLog("Request Url: " + PathAndQuery);
		        string Requester = request.RemoteEndPoint.Address.ToString();
		        Console.WriteLine("Requester: " + Requester);
		        CommonMethods.WriteToOutputLog("Requester: " + Requester);		        
		        CommonMethods.WriteToReferrerLog(Requester);
		
				foreach (string headerKey in request.Headers.AllKeys)
				{
					string headerValue = IsSensitiveHeader(headerKey)
						? "[REDACTED]"
						: request.Headers[headerKey];
					CommonMethods.WriteToOutputLog(string.Format("Header: {0} = {1}: ", headerKey, headerValue));
				}
				
		        // Copy headers manually, avoiding restricted ones
		        /*
		        foreach (string header in request.Headers)
		        {
		            if (!IsRestrictedHeader(header))  // Workaround for .NET 4.5
		            {
		                proxyRequest.Headers[header] = request.Headers[header];
		            }
		        }
		        */
		        
		        foreach (string headerKey in request.Headers.AllKeys)
				{
				    if (!WebHeaderCollection.IsRestricted(headerKey)) // Avoid restricted headers
				    {
				        proxyRequest.Headers[headerKey] = request.Headers[headerKey];
				    }
				}

		        // Host is restricted and must be set through its property. Preserve
		        // the public host for IIS bindings, generated URLs, and cookies.
		        proxyRequest.Host = HostId;

		        // Replace any client-supplied forwarding metadata with values that
		        // this proxy observed after validating Host against DomainConfig.
		        proxyRequest.Headers["X-Forwarded-Host"] = HostId;
		        proxyRequest.Headers["X-Forwarded-Proto"] = request.IsSecureConnection ? "https" : "http";

		
		        // Set allowed properties separately
		        proxyRequest.ContentType = request.ContentType;
		        proxyRequest.UserAgent = request.UserAgent;
		        //proxyRequest.Accept = request.Headers["Accept"];
		        
		        string AcceptHeader = request.Headers["Accept"];
		        
		        if (!string.IsNullOrEmpty(AcceptHeader))
		        {
		        	proxyRequest.Accept = AcceptHeader;
		        }
		
		        if (request.ContentLength64 > 0)
		        {
		            proxyRequest.ContentLength = request.ContentLength64;
		            using (Stream requestStream = proxyRequest.GetRequestStream())
		            {
		                CopyStream(request.InputStream, requestStream); // Manual Copy
		            }
		        }
		
		        // Send the request and get the response
		        using (HttpWebResponse proxyResponse = (HttpWebResponse)proxyRequest.GetResponse())
		        {
		            response.StatusCode = (int)proxyResponse.StatusCode;
		            response.ContentType = proxyResponse.ContentType;
		
		            foreach (string header in proxyResponse.Headers)
		            {
		                // Set-Cookie must not be combined because Expires values
		                // contain commas. Preserve each cookie as a separate header.
		                if (!IsRestrictedHeader(header) &&
		                    !header.Equals("Set-Cookie", StringComparison.OrdinalIgnoreCase))
		                {
		                    response.Headers[header] = proxyResponse.Headers[header];
		                }
		            }

		            string[] setCookieHeaders = proxyResponse.Headers.GetValues("Set-Cookie");
		            if (setCookieHeaders != null)
		            {
		                foreach (string setCookieHeader in setCookieHeaders)
		                {
		                    response.AppendHeader("Set-Cookie", setCookieHeader);
		                }
		            }
		
		            using (Stream responseStream = proxyResponse.GetResponseStream())
		            {
		                CopyStream(responseStream, response.OutputStream); // Manual Copy
		            }
		        }
		    }
		    catch (WebException ex)
		    {
				HttpWebResponse errorResponse = ex.Response as HttpWebResponse;
				if (errorResponse != null)
		        {
		            response.StatusCode = (int)errorResponse.StatusCode;
		            using (Stream errorStream = errorResponse.GetResponseStream())
		            {
		                if (errorStream != null)
		                {
		                    CopyStream(errorStream, response.OutputStream);
		                }
		            }
		        }
		        else
		        {
		            response.StatusCode = (int)HttpStatusCode.InternalServerError;
		            Console.WriteLine("Error not logged");
		        }
		    }
		    finally
		    {
		        response.Close();
		    }
		}
		
		// Workaround for .NET 4.5: Check restricted headers manually
		private bool IsRestrictedHeader(string header)
		{
		    string[] restrictedHeaders = new string[]
		    {
		        "Connection", "Host", "Content-Length", "Transfer-Encoding", "Expect"
		    };
		    return restrictedHeaders.Contains(header, StringComparer.OrdinalIgnoreCase);
		}

		private bool IsSensitiveHeader(string header)
		{
			return header.Equals("Authorization", StringComparison.OrdinalIgnoreCase) ||
				header.Equals("Cookie", StringComparison.OrdinalIgnoreCase) ||
				header.Equals("Proxy-Authorization", StringComparison.OrdinalIgnoreCase);
		}
		
		// Workaround for .NET 4.5: Copy streams manually
		private void CopyStream(Stream input, Stream output)
		{
		    byte[] buffer = new byte[8192];
		    int bytesRead;
		    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
		    {
		        output.Write(buffer, 0, bytesRead);
		    }
		}

	    private void LoadBlockedIPs(string filePath)
	    {
	        if (File.Exists(filePath))
	        {
	            var json = File.ReadAllText(filePath);
	            var data = JsonConvert.DeserializeObject<BlockedIPList>(json);
	
	            // **.NET 4.5 FIX** - Explicit null check
	            if (data != null && data.BlockedIPs != null)
	            {
	                blockedIPs = new HashSet<string>(data.BlockedIPs);
	            }
	        }
	    }
	
	    private void LoadBlockedRanges(string filePath)
	    {
	        if (File.Exists(filePath))
	        {
	            var json = File.ReadAllText(filePath);
	            var data = JsonConvert.DeserializeObject<BlockedRangeList>(json);
	
	            // **.NET 4.5 FIX** - Explicit null check
	            if (data != null && data.BlockedRanges != null)
	            {
	                blockedRanges = data.BlockedRanges.Select(ParseIPRange).ToList();
	            }
	            else
	            {
	                blockedRanges = new List<IPRange>();
	            }
	        }
	        else
	        {
	            blockedRanges = new List<IPRange>();
	        }
	    }
	
	    private IPRange ParseIPRange(string range)
	    {
	        return new IPRange(range); // Implements CIDR parsing
	    }
	
	    public bool IsIPBlocked(string ip)
	    {
	    	
	    	Console.WriteLine("Checking the IP Address -> " + ip);
	        // First, check against individual blocked IPs
	        if (blockedIPs.Contains(ip))
	        {
	            return true;
	        }
	
	        // Then, check against blocked ranges
	        foreach (var range in blockedRanges)
	        {
	            if (range.Contains(ip))
	            {
	                return true;
	            }
	        }
	
	        return false;
	    }

	}
}
