/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 7/10/2024
 * Time: 4:23 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;

using System.Text;
using System.IO;

namespace Actions.FileWatcher
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		private string FModuleName = "FileWatcher.CommonMethods";
		
		public CommonMethods()
		{
		}
		
		public static string LoadTestRequestItem(ActionBase ActionBase,
		                                         ref TestRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadTestRequestItem";
			object Obj = null;
			try 
			{
				Obj = ActionBase.GetCollectionValue("Data", "TestString");
				TempItem.TestString = (Obj == null) ? "" : Convert.ToString(Obj);
				Console.WriteLine("TestString: " + TempItem.TestString);
				Obj = ActionBase.GetCollectionValue("Data", "SenderName");
				TempItem.SenderName  = (Obj == null) ? "" : Convert.ToString(Obj);
				Console.WriteLine("SenderName: " + TempItem.SenderName);
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			return Result;
		}
		
		public static string StringReverse(string s)
		{
			char[] charArray = s.ToCharArray();
			Array.Reverse(charArray);
			return new string(charArray);
		}
		
		public static string TestResponse(TestRequestItem RequestItem, ref StringBuilder Entries)
		{
			string Result = "";
			string Str = "";
			
			Entries.Append(CommonUtilities.JsonUtilities.AddPropertyNameAndValue("TestString", RequestItem.TestString));
			Entries.Append(CommonUtilities.JsonUtilities.AddPropertyNameAndValue("SenderName", RequestItem.SenderName));			
			Str = StringReverse(RequestItem.TestString);
			Entries.Append(CommonUtilities.JsonUtilities.AddPropertyNameAndValue("gnirtStseT", Str));
			Str = StringReverse(RequestItem.SenderName);
			Entries.Append(CommonUtilities.JsonUtilities.AddPropertyNameAndValue("emaNredneS", Str, false));
			return Result;
		}
		
		public static string LoadFilePropertiesRequestItem(ActionBase ActionBase,
		                                                   ref FilePropertiesRequestItem TempItem)
		{
			string Result = "";
			const string Method = "LoadFilePropertiesRequestItem";
			object Obj = null;
			try 
			{
				Obj = ActionBase.GetCollectionValue("Data", "SourceFolder");
				TempItem.SourceFolder = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DestinationFolder");
				TempItem.DestinationFolder = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FileName");
				TempItem.FileName = (Obj == null) ? "" : Convert.ToString(Obj);
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			return Result;
		}
		
		public static string GetFilePropertiesResults(FilePropertiesRequestItem RequestItem, 
		                                              ref StringBuilder Entries)
		{
			string Result = "";
			//string Str = "";
			
			if (string.IsNullOrEmpty(Result))
			{
				/*
				Result = SourceFileExists(RequestItem.FileName);
				Str = (string.IsNullOrEmpty(Result)) ? "Yes" : "No";
   				Entries.AppendLine(CommonUtilities.JsonUtilities.AddPropertyNameAndValue("FileExists", Str, false));
				if (string.IsNullOrEmpty(Result))
				{
					Result = GetSourceFileProperties(RequestItem.FileName, ref Entries);
				}
   				*/
   				try
   				{
   					Entries.AppendLine(JsonUtilities.StartJsonObject("SourceFile"));
					Result = ProcessSourceFile(RequestItem, ref Entries);
   				}
   				finally
   				{
   					Entries.AppendLine(JsonUtilities.EndJsonObject());
   				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				try
				{
					Entries.AppendLine(",");
					Entries.AppendLine(JsonUtilities.StartJsonObject("DestinationFile"));
					Result = ProcessDestinationFile(RequestItem, ref Entries);
				}
				finally
				{
					Entries.AppendLine(JsonUtilities.EndJsonObject());				
				}			
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				try
				{
					Entries.AppendLine(",");
					Entries.AppendLine(JsonUtilities.StartJsonObject("Recommendations"));
					Result = ProcessRecommendations(RequestItem, ref Entries);
				}
				finally
				{
					Entries.AppendLine(JsonUtilities.EndJsonObject());
				}
			}
			
			return Result;
		}
		
		private static string ProcessSourceFile(FilePropertiesRequestItem RequestItem,
		                                        ref StringBuilder Entries)
		{
			string Result = "";
			
			string PathDelim = Path.DirectorySeparatorChar.ToString();
			
			string FullFileName = RequestItem.SourceFolder;
			if (!FullFileName.EndsWith(PathDelim))
				FullFileName += PathDelim;
			FullFileName += RequestItem.FileName;
			
			bool FileExists = File.Exists(FullFileName);
			string Str = (FileExists) ? "Yes" : "No";
  			Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileExists", Str, FileExists));			
  			
  			if ((string.IsNullOrEmpty(Result)) && FileExists)
  			{
  				Result = GetSourceFileProperties(FullFileName, ref Entries);
  			}
			
			return Result;
		}
		
		private static string GetSourceFileProperties(string FileName, 
		                                              ref StringBuilder Entries)
		{
			string Result = "";
			
			try
			{
				FileInfo info = new FileInfo(FileName);
				
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileName", info.Name));
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FullPath", info.FullName));
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("Size", info.Length));
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("CreationTime", info.CreationTime.ToString()));
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("LastAccessTime", info.LastAccessTime.ToString()));
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("LastWriteTime", info.LastWriteTime.ToString(), false));
			}
			catch (Exception Ex)
			{
				Result = Ex.Message;
			}			
			
			return Result;
		}

		
		private static string ProcessDestinationFile(FilePropertiesRequestItem RequestItem,
		                                             ref StringBuilder Entries)
		{
			string Result = "";
			
			string PathDelim = Path.DirectorySeparatorChar.ToString();
			
			string FullFileName = RequestItem.DestinationFolder;
			if (!FullFileName.EndsWith(PathDelim))
				FullFileName += PathDelim;
			FullFileName += RequestItem.FileName;
			
			bool FileExists = File.Exists(FullFileName);
			string Str = (FileExists) ? "Yes" : "No";
  			Entries.AppendLine(CommonUtilities.JsonUtilities.AddPropertyNameAndValue("FileExists", Str, FileExists));			
  			
  			if ((string.IsNullOrEmpty(Result)) && FileExists)
  			{
  				Result = GetDestinationFileProperties(FullFileName, ref Entries);
  			}
			
			return Result;
		}
		
		private static string GetDestinationFileProperties(string FileName, 
		                                                   ref StringBuilder Entries)
		{
			string Result = "";
			
			try
			{
				FileInfo info = new FileInfo(FileName);
				
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileName", info.Name));
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FullPath", info.FullName));
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("Size", info.Length));
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("CreationTime", info.CreationTime.ToString()));
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("LastAccessTime", info.LastAccessTime.ToString()));
   				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("LastWriteTime", info.LastWriteTime.ToString(), false));
			}
			catch (Exception Ex)
			{
				Result = Ex.Message;
			}			
			
			return Result;
		}
		
		private static string ProcessRecommendations(FilePropertiesRequestItem RequestItem,
		                                             ref StringBuilder Entries)
		{
			string Result = "";
			string Action = "Incomplete";				
			
			string CurrentResults = 
						JsonUtilities.StartJson() +
						Entries.ToString() + "}" +
						JsonUtilities.EndJson();
			
			JObject currentJson = JObject.Parse(CurrentResults);
			string Str = (string)currentJson["DestinationFile"]["FileExists"];
			bool DestFileExists = CommonUtilities.StringUtilities.SameText(Str, "Yes");
			DateTime ZeroDate = MiscUtilities.YYYYMMDDToDateTime("19000101");
			DateTime SourceFileWriteTime = MiscUtilities.YYYYMMDDToDateTime("19000101");
			DateTime DestFileWriteTime = MiscUtilities.YYYYMMDDToDateTime("19000101");
			
			if (!DestFileExists)
			{
				Action = "Copy";
			}
			else
			{
				if (string.IsNullOrEmpty(Result))
				{
					Str = JsonUtilities.GetJsonPathValueStr(
						currentJson, "SourceFile.LastWriteTime", out Result);
					if (!string.IsNullOrEmpty(Result))
					{
						Action = "Error in SourceFile.LastWriteTime";
					}
					else
					{
						DateTime.TryParse(Str, out SourceFileWriteTime);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Str = JsonUtilities.GetJsonPathValueStr(
						currentJson, "DestinationFile.LastWriteTime", out Result);
					if (!string.IsNullOrEmpty(Result))
					{
						Action = "Error in DestinationFile.LastWriteTime";
					}
					else
					{
						DateTime.TryParse(Str, out DestFileWriteTime);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Action = (SourceFileWriteTime > DestFileWriteTime) ? "Copy" : "";
				}
			}
			
			Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("Action", Action, false));
			
			return Result;
		}
	}
}
