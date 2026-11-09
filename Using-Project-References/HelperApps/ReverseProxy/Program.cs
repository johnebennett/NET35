/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 3/12/2024
 * Time: 1:09 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ReverseProxy.Actions;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ReverseProxy
{
	public class Program
	{		
	    public static void Main(string[] args)
	    {
	        //StartProxy();
	        new Proxy().Execute();
	    }
	    
	}
}

