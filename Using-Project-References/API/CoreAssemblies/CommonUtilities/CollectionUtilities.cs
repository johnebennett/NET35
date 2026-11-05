/*
 * Created by SharpDevelop.
 * User: John
 * Date: 08/04/2020
 * Time: 6:47 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace CommonUtilities
{
	/// <summary>
	/// Utilities the work with Collections
	/// eg: NameValueCollections etc
	/// </summary>
	public class CollectionUtilities
	{
		public CollectionUtilities()
		{
		}
		
		#region NameValueCollection Utilities
		
		public static NameValueCollection MakeCollection(JObject Source, out string ErrorMsg)
		{
			NameValueCollection Result = new NameValueCollection();
			string Method = "MakeCollection";
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				Dictionary<string, string> dict;

				if (Source == null)
				{
					ErrorMsg = "Source DataObject IS NULL";
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					dict = Source.ToObject<Dictionary<string, string>>();
					if (dict == null) ErrorMsg = "DataObject Dictionary IS NULL";
					
					if (string.IsNullOrEmpty(ErrorMsg) && (dict != null))
					{
						foreach (KeyValuePair<string, string> Item in dict)
						{
							Result.Add(Item.Key, Item.Value);
						}
					}
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		public static NameValueCollection MakeCollection(string JsonSource, out string ErrorMsg)
		{
			ErrorMsg = "";
			JObject Source = JsonConvert.DeserializeObject<JObject>(JsonSource);
			return MakeCollection(Source, out ErrorMsg);
		}
		
		
		public static object GetCollectionValue(NameValueCollection Source, string Key)
		{
			object Result = Source[Key];
			return Result;
		}
		
		public static string GetCollectionValue(NameValueCollection Source, string Key, string DefaultValue)
		{
			string Result = DefaultValue;
			object CollObj = GetCollectionValue(Source, Key);
			if (CollObj != null) Result = Convert.ToString(CollObj);
			return Result;
		}
		
		public static int GetCollectionValue(NameValueCollection Source, string Key, int DefaultValue)
		{
			int Result = DefaultValue;
			object CollObj = GetCollectionValue(Source, Key);
			if (CollObj != null) Result = Convert.ToInt32(CollObj);
			return Result;
		}
		
		public static string GetCollectionValue(NameValueCollection Source, string Key, string DefaultValue, out bool KeyFound)
		{
			KeyFound = false;
			string Result = DefaultValue;
			object CollObj = GetCollectionValue(Source, Key);
			KeyFound = (CollObj != null);
			if (CollObj != null) Result = Convert.ToString(CollObj);
			return Result;
		}
		
		public static int GetCollectionValue(NameValueCollection Source, string Key, int DefaultValue, out bool KeyFound)
		{
			KeyFound = false;
			int Result = DefaultValue;
			string Value = GetCollectionValue(Source, Key, "", out KeyFound);
			//object CollObj = GetCollectionValue(Source, Key);
			//KeyFound = (CollObj != null);
			//if (CollObj != null) Result = Convert.ToInt32(CollObj);
			//string StrValue = (CollObj == null) ? "" : Convert.ToString(CollObj);
			Result = StringUtilities.StrToIntDef(Value, DefaultValue);
			return Result;
		}
		
		public static string GetCollectionInfo(NameValueCollection Source)
		{
			string Str = "";
			//string Method = "GetCollectionInfo";
			StringBuilder StrList = new StringBuilder();
			
			StrList.AppendLine("Collection.Count: " + Source.Count.ToString());
			if (Source.Count > 0)
			{
				foreach (string TheKey in Source.Keys)
				{
					Str = TheKey + ":" + Source.Get(TheKey);
					StrList.AppendLine(Str);
				}
			}
			
			//WindowsEventLoggingUtilities.WriteToInfoEvent(string.Join(Environment.NewLine, StrList.ToArray()), Method, 0);
			return StrList.ToString();
		}
		
		public void WriteCollectionInfo(NameValueCollection Source)
		{
			string Method = "WriteCollectionInfo";
			string Str = GetCollectionInfo(Source);
			
			WindowsEventLoggingUtilities.WriteToInfoEvent(Str, Method, 0);
		}
		#endregion
	}
}
