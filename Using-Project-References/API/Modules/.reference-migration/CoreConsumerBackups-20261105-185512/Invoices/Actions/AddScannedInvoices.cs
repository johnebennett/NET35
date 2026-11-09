/*
 * Created by SharpDevelop.
 * User: John
 * Date: 18/05/2020
 * Time: 1:23 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
namespace Actions.Invoices
{
	/// <summary>
	/// Description of AddScannedInvoices.
	/// </summary>
	public class AddScannedInvoices : ActionBase
	{
		private string FModuleName = "Invoices";
		//private SetupPageEntriesRequestItem FRequestItem;
		private List<string> FFileList = new List<string>();
		private int FFileListCount = 0;
		private string FBaseDir = "";
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			AddScannedInvoices inst = new AddScannedInvoices();
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try
			{
				Result = inst.Init();				
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					return "";
				}
				
				Result = inst.Validate();
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					return "";
				}

				Result = inst.Output();
			}
			finally
			{
				inst.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string Init()
		{
			string Method = "Init";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				/*
				FRequestItem = new SetupPageEntriesRequestItem();
				Result = FRequestItem.Load((ActionBase)this);
				*/
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = InitFilesItems();
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		private string InitFilesItems()
		{
			string Result = "";
			string Method = "InitFilesItems";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				FBaseDir = CommonUtilities.FolderHelpers.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
				if (string.IsNullOrEmpty(Result))
				{
					if (FBaseDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
					    FBaseDir = Directory.GetParent(FBaseDir).FullName;
				    FBaseDir = Directory.GetParent(FBaseDir).FullName;
					if (!FBaseDir.EndsWith(Path.DirectorySeparatorChar.ToString())) FBaseDir += Path.DirectorySeparatorChar;
					WindowsEventLoggingUtilities.WriteToInfoEvent("BaseDir: " + FBaseDir, FModuleName, MasterId);
				}
				
				JArray FilesObject = (JArray)DataObject["files"];
				if (FilesObject == null)
				{
					Result = "FilesObject IS NULL";
				}
				
				string CurrentFileName = "";
				string CurrentFilePath = "";
				string FileNameOnly = "";
				if (string.IsNullOrEmpty(Result))
				{
					FFileListCount = FilesObject.Count;
					
					if (FilesObject.Count > 0)
					{
						foreach (JObject Item in FilesObject)
						{
							if ((Item != null) && (Item["file"] != null))
							{
								CurrentFileName = FBaseDir + (string)Item["file"];
								CurrentFilePath = Path.GetDirectoryName(CurrentFileName);
								if (!CurrentFilePath.EndsWith(Path.DirectorySeparatorChar.ToString())) CurrentFilePath += Path.DirectorySeparatorChar;
								FileNameOnly = CommonItem.CompanyId + "_" + CommonItem.ClientId + "_" + Path.GetFileName(CurrentFileName);								
								WindowsEventLoggingUtilities.WriteToInfoEvent("Path: " + CurrentFilePath + Environment.NewLine + "FileName: " + FileNameOnly, Method, 0);
								//FFileList.Add(FBaseDir + CurrentFilePath + FileNameOnly);
								FFileList.Add(CurrentFilePath + FileNameOnly);
							}
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string Validate()
		{
			string Method = "Validate";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				//Result = FRequestItem.Validate();
				if (string.IsNullOrEmpty(Result))
				{
					Result = ValidateFilesItems();
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		private string ValidateFilesItems()
		{
			string Result = "";
			string Method = "ValidateFilesItems";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
				}
				if (string.IsNullOrEmpty(Result) && (FFileList.Count == 0))
				{
					Result = "There are no files to process";
				}
				else if (string.IsNullOrEmpty(Result) && (FFileList.Count != FFileListCount))
				{
					Result = "FFileList.Count ({0}) != FFileListCount {1}";
					Result = string.Format(Result, FFileList.Count, FFileListCount);
				}
				
				// validate that all the items in the FFileList exist
				if (string.IsNullOrEmpty(Result))
				{
					foreach (string FileName in FFileList)
					{
						if (!File.Exists(FileName))
						{
							Result = "The file: " + FileName + " does not exist";
							break;
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string Output()
		{
			string Method = "Output";
			string Result = "";
			string ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = CommonMethods.EAInvoicesImportGroup(FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, "\\", ref Entries);
				}
				
				string ShowPost = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ShowPost = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.ShowPost", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(ShowPost)) ErrorMsg = "Cannot find the Show Post entry";
					if (string.IsNullOrEmpty(ErrorMsg) && (ShowPost == "N")) ErrorMsg = "Cannot Process the invoices. ShowPost IS NO";
				}
				
				string DropLocation = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					DropLocation = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.DropLocation", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(DropLocation)) ErrorMsg = "Cannot find the Drop Location entry";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						DropLocation = DropLocation.Replace("__", "\\");
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = CopyFilesToDropLocation(DropLocation);
				}				
				
				/*
				Int64 NewId = 0;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					NewId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							FbConnectionString, "I_ISE_ID_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && (NewId < 0)) ErrorMsg = "Invalid NewId Generated value.";
				}
				*/
				
				string QueryText = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.SQL1", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(QueryText)) ErrorMsg = "Cannot find the first SQL entry";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						QueryText = QueryText.Replace("@", "'");
						QueryText = QueryText.Replace("__", "\\");
						//QueryText = QueryText.Replace("i_ise_id", NewId.ToString());
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
					}
				}
				
				string CommandLine = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.SQL2", out ErrorMsg);
					DataTable tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && (tbl.Rows.Count == 0)) ErrorMsg = "No command line row entry";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						object Obj = tbl.Rows[0]["ISE_CommandLine"];
						CommandLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						if (string.IsNullOrEmpty(CommandLine)) ErrorMsg = "No command line value returned";
					}					
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					string Location = CommonUtilities.FolderHelpers.TestRPOLocation(FbConnectionString, CommonItem.CompanyIdValue, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(Location)) ErrorMsg = "Undefined Processor Location";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg = UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(CommandLine, Location, 1000);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogResponse(Method, "");
					/*
					Result = 
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("Method", "AddScannedInvoices") +
						JsonUtilities.AddPropertyNameAndValue("Status", "SUCCESS", false) +
						JsonUtilities.EndJson();						
					*/
				}
				else
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
			return Result;
		}
		
		private string CopyFilesToDropLocation(string DropLocation)
		{
			string Result = "";
			string Method = "CopyFilesToDropLocation";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("BaseDir: " + FBaseDir, FModuleName, MasterId);
				
				//string CurrentFileName = "";
				string CurrentFilePath = "";
				string FileNameOnly = "";
				string SaveFileName = "";
				string Str ="";
				List<string> Errors = new List<string>();
				if (string.IsNullOrEmpty(Result))
				{
					if (FFileList.Count > 0)
					{
						foreach (string Item in FFileList)
						{
							if (!string.IsNullOrEmpty(Item))
							{
								CurrentFilePath = Path.GetDirectoryName(Item);
								if (!CurrentFilePath.EndsWith(Path.DirectorySeparatorChar.ToString())) 
									CurrentFilePath += Path.DirectorySeparatorChar;
								
								FileNameOnly = Path.GetFileName(Item);
								SaveFileName = FileNameOnly;
								/*
								WindowsEventLoggingUtilities.WriteToInfoEvent(
									"Path: " + CurrentFilePath + Environment.NewLine + 
									"FileName: " + FileNameOnly + Environment.NewLine +
									"SaveFileName: " + SaveFileName, Method, 0);
								*/
								Str = CommonItem.CompanyId + "_";
								if (SaveFileName.StartsWith(Str)) SaveFileName = SaveFileName.Substring(Str.Length);
								Str = CommonItem.ClientId + "_";
								if (SaveFileName.StartsWith(Str)) SaveFileName = SaveFileName.Substring(Str.Length);
								SaveFileName = DropLocation + SaveFileName;
								/*
								WindowsEventLoggingUtilities.WriteToInfoEvent(
									"Path: " + CurrentFilePath + Environment.NewLine + 
									"FileName: " + FileNameOnly + Environment.NewLine +
									"SaveFileName: " + SaveFileName, Method, 0);
								*/
								Directory.CreateDirectory(Path.GetDirectoryName(SaveFileName));
								File.Copy(Item, SaveFileName, true);
								if (!File.Exists(SaveFileName)) Errors.Add(SaveFileName);
							}
						}
					}
					if (Errors.Count > 0)
					{
						Result = "Error saving the following files:" + Environment.NewLine + string.Join(",", Errors.ToArray()) ;						
					}
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
	}
}