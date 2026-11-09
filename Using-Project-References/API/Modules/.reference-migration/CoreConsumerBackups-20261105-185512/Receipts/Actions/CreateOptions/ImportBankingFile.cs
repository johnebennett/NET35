/*
 * Created by SharpDevelop.
 * User: John
 * Date: 24/04/2020
 * Time: 7:03 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
namespace Actions.Receipts.CreateOptions
{
	/// <summary>
	/// Description of ImportBankingFile.
	/// </summary>
	public class ImportBankingFile : ActionBase
	{
		private string FModuleName = "Receipts";
		private ImportBankingFileRequestItem FRequestItem;		
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

			ImportBankingFile inst = new ImportBankingFile();
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
				UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
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
				FRequestItem = new ImportBankingFileRequestItem();
				Result = FRequestItem.Load((ActionBase)this);
				
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
				
				if (string.IsNullOrEmpty(Result))
				{
					FFileListCount = FilesObject.Count;
					
					if (FilesObject.Count > 0)
					{
						foreach (JObject Item in FilesObject)
						{
							if ((Item != null) && (Item["file"] != null))
							{
								FFileList.Add(FBaseDir + (string)Item["file"]);
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = FRequestItem.Validate();
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
			string Method = "MethodName";
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
				else if (string.IsNullOrEmpty(Result) && ((FFileList.Count != FFileListCount) && (FFileList.Count != 1)))
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
					string FileName = System.IO.Path.GetFileName(FFileList[0]);
					ErrorMsg = 
						CommonMethods.EABankImportFileScreenGroup(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, 
							CommonItem.ClientIdValue,
							FRequestItem.BankImportTypeId, FileName, FRequestItem.BankNameId,
							FRequestItem.StartingReceiptNumber, FRequestItem.ReceiptSetId, FRequestItem.PageNumber,
							FRequestItem.BankStmtDate, ref Entries);
				}
				
				string Path = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.whycannotimportfile";
					string CannotImport = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Source:" + Entries.ToString(), Method, 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Path:" + Path, Method, 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("CannotImport:" + CannotImport, Method, 0);
					if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(CannotImport)) ErrorMsg = CannotImport;
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.copyto";
					string CopyTo = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(CopyTo))
					{
						ErrorMsg = "Unable to get the CopyTo location";
					}
					else if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(CopyTo))
					{
						ErrorMsg = CopyFileToLocation(FFileList[0], CopyTo);
					}
				}
				
				int NewId = 0;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					NewId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							FbConnectionString, "EBA_ID_Gen", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.sqlinsert";
					string SqlInsert = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SqlInsert))
					{
						ErrorMsg = "Unable to get the SqlInsert value";
					}
					else if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(SqlInsert))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent(SqlInsert, FModuleName, MasterId);
						SqlInsert = SqlInsert.Replace("@", System.IO.Path.DirectorySeparatorChar.ToString());
						SqlInsert = SqlInsert.Replace("\"", "'");
						SqlInsert = SqlInsert.Replace("XXXXX", NewId.ToString());
						SqlInsert = SqlInsert.Replace("__", "'");
						WindowsEventLoggingUtilities.WriteToInfoEvent(SqlInsert, FModuleName, MasterId);
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, SqlInsert);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.sqltogetcommandline";
					string CommandLineSql = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(CommandLineSql))
					{
						ErrorMsg = "Unable to get the Command Line Sql";
					}
					else if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(CommandLineSql))
					{
						CommandLineSql = CommandLineSql.Replace("XXXXX", NewId.ToString());
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, CommandLineSql);
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							ErrorMsg = ProcessCommandLine(CommandLineSql);
						}
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = CommonMethods.EABankRec(FbConnectionString, CommonItem.CompanyIdValue, NewId);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					/*
					Result =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					*/
					Result = LogResponse(Method, "");
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
		
		private string CopyFileToLocation(string FileName, string CopyToLocation)
		{
			string Result = "";
			string Method = "CopyFileToLocation";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				CopyToLocation = CopyToLocation.Replace("@", Path.DirectorySeparatorChar.ToString());
				string FileNameOnly = Path.GetFileName(CopyToLocation);
				WindowsEventLoggingUtilities.WriteToInfoEvent("FileNameOnly: " + FileNameOnly, FModuleName, MasterId);
				/*
				if (!string.IsNullOrEmpty(FileNameOnly)) 
					CopyToLocation = Path.GetDirectoryName(CopyToLocation);
				//if (!CopyToLocation.EndsWith(Path.DirectorySeparatorChar.ToString())) CopyToLocation += Path.DirectorySeparatorChar.ToString();
				//if (CopyToLocation.EndsWith(Path.DirectorySeparatorChar.ToString()) 
				WindowsEventLoggingUtilities.WriteToInfoEvent("CopyToLocation: " + CopyToLocation, FModuleName, MasterId);
				*/
				File.Copy(FileName, CopyToLocation, true);
				if (!File.Exists(CopyToLocation)) Result = "Cannot file the file: " + CopyToLocation;
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
		
		private string ProcessCommandLine(string CommandLineSql)
		{
			string Result = "";
			string Method = "ProcessCommandLine";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string CommandLine = "";
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, CommandLineSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}				
				
				object Obj = null;
				if (string.IsNullOrEmpty(Result) && (tbl != null))
				{
					foreach(DataRow Row in tbl.Rows)
					{
						Obj = Row["EAB_COMMANDLINE"];
						CommandLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						WindowsEventLoggingUtilities.WriteToInfoEvent("CommandLine: " + CommandLine, Method, 0);
					}
				}
				
				if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(CommandLine))
				{					
					//WindowsEventLoggingUtilities.WriteToInfoEvent("Before CommandLine: " + CommandLine, Method, 0);
					Result = PerformTestRPO(CommandLine);
					//WindowsEventLoggingUtilities.WriteToInfoEvent("After CommandLine: " + CommandLine, Method, 0);
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
		
		private string PerformTestRPO(string CommandLine)
		{
			string Result = "";
			string Method = "PerformTestRPO";
			string CurrentDirectory = Directory.GetCurrentDirectory();
			string WorkingDirectory = CommonUtilities.FolderHelpers.TestRPOLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
			string CmdBase = "";
			
			if (string.IsNullOrEmpty(Result))
			{
				if (!WorkingDirectory.EndsWith(Path.DirectorySeparatorChar.ToString())) WorkingDirectory += Path.DirectorySeparatorChar.ToString();
				//WindowsEventLoggingUtilities.WriteToInfoEvent("CurrentDirectory: " + CurrentDirectory, "SystemReportUtilities", 0);
				//WindowsEventLoggingUtilities.WriteToInfoEvent("WorkingDirectory: " + WorkingDirectory, "SystemReportUtilities", 0);
				CmdBase = WorkingDirectory + "{0}";
				
				Directory.SetCurrentDirectory(WorkingDirectory);
				try
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("New CurrentDirectory: " + Directory.GetCurrentDirectory(), "SystemReportUtilities", 0);
					string AppName = CommandLine.Substring(0, CommandLine.IndexOf(" /"));
					AppName = string.Format(CmdBase, AppName);
					
					string ArgsBase = CommandLine.Substring(CommandLine.IndexOf("/"));
					WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, Method, 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, Method, 0);
					
					Process runApp = new Process();
					runApp.StartInfo.FileName = AppName;
					runApp.StartInfo.Arguments = ArgsBase;
					runApp.Start();
					const int tries = 1000;
					for (int Index = 0; Index < tries; Index++)
					{
						if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
					}
					if (!runApp.HasExited)
					{
						Result = "Application did not finish in time.";
					}
					
					//WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName + " Result: " + Result, Method, 0);
				}
				finally
				{
					Directory.SetCurrentDirectory(CurrentDirectory);					
				}
			}
			return Result;				
		}
	}
}
