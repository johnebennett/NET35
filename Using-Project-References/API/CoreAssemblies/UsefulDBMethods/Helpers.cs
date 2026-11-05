/*
 * Created by SharpDevelop.
 * User: John
 * Date: 22/07/2019
 * Time: 12:43 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using System.Data;
using System.Diagnostics;

using CommonUtilities;

using Newtonsoft.Json.Linq;

namespace UsefulDBMethods
{
	public enum FirebirdQueryCheckTypes 
	{
		None = 0,
		HasTables = 1,
		HasRows = 2,
		HasTablesAndRows = 4
	}
	
	/// <summary>
	/// General UsefulDBMethods Helper methods 
	/// </summary>
	public class Helpers
	{
		private string FModuleName = "";
		
		public string ModuleName 
		{
			get { return FModuleName; }
		}
		
		public Helpers()
		{
			FModuleName = "UsefulDBMethods";
		}
		
		public static string WriteParamsToInfoEvent(string QueryText, List<FbParameterItem> Params, string ModuleName, int MasterId)
		{
			string Result = "";
			string Msg = "QueryText:" + Environment.NewLine + QueryText;
			//if (Params.Count > 0) Msg += Environment.NewLine;
			UsefulDBMethods.Helpers inst = new UsefulDBMethods.Helpers();
			if (string.IsNullOrEmpty(ModuleName)) ModuleName = inst.ModuleName;
			
			try 
			{
				if (Params != null)
				{
					foreach (FbParameterItem Param in Params)
					{
						Msg += Environment.NewLine;
						Msg = Msg + Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue);
					}
				}
				
				Result = WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, ModuleName, MasterId);
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			
			return Result;
		}
		
		public static DataTable GetFirstDataTableEx(string ConnectionString, 
		                                            string QueryText, 
		                                            List<FbParameterItem> Params, 
		                                            int Timeout, 
		                                            FirebirdQueryCheckTypes CheckType,
		                                            out string ErrorMsg)
		{
			DataTable dt = null;
			try 
			{
				ErrorMsg = "";
				dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
				if ((dt == null) && ((CheckType == FirebirdQueryCheckTypes.HasTables) || CheckType == FirebirdQueryCheckTypes.HasTablesAndRows))
				{
					ErrorMsg = "No tables in the dataset to process";
				}
				else if ((dt.Rows.Count == 0) && ((CheckType == FirebirdQueryCheckTypes.HasRows) || (CheckType == FirebirdQueryCheckTypes.HasTablesAndRows)))
				{
					ErrorMsg = "No rows in the dataset to process";
				}
			}					
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return dt;
		}
		
		public static DataTable GetFirstDataTableEx(string connectionString, 
		                                            string QueryText, 
		                                            FirebirdQueryCheckTypes CheckType, 
		                                            out string ErrorMsg)
		{
			return UsefulDBMethods.Helpers.GetFirstDataTableEx(connectionString, QueryText, null, 0, CheckType, out ErrorMsg);
		}
		
		public static string RunEAMainOfficeProcess(string ConnectionString,
		                                            Int64 CompanyClientId)
		{
			string Result = "";
			
			try 
			{
				Helpers inst = new Helpers();
				string QueryText = "EXECUTE PROCEDURE EAMainOfficeProcess(@CompanyClientId)";
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				WriteParamsToInfoEvent(QueryText, Params, inst.ModuleName, 0);
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		public static string ProcessTestRPOCommandLine(string CommandLine,
		                                               string TestRPOLocation,
		                                               int Tries)
		{
			string Result = "";
			string Method = "ProcessTestRPOCommandLine";
			
			if (string.IsNullOrEmpty(CommandLine))
			{
				Result = "Invalid or empty Command Line provided in " + Method;
				return Result;
			}
			
			if (string.IsNullOrEmpty(TestRPOLocation))
			{
				Result = "Invalid or empty location in " + Method;
				return Result;
			}
			
			if (Tries <= 0) Tries = 1200; // 20 minutes
			
			string CmdBase = TestRPOLocation;
			if (string.IsNullOrEmpty(Result))
			{
				if (!CmdBase.EndsWith(@"\")) CmdBase += @"\";
				CmdBase += "{0}";
				string AppName = CommandLine.Substring(0, CommandLine.IndexOf(" /"));
				AppName = string.Format(CmdBase, AppName);
				
				string ArgsBase = CommandLine.Substring(CommandLine.IndexOf("/"));
				
				Process runApp = new Process();
				runApp.StartInfo.FileName = AppName;
				runApp.StartInfo.Arguments = ArgsBase;
                WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + runApp.StartInfo.FileName, Method, 0);
                WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + runApp.StartInfo.Arguments, Method, 0);
                runApp.Start();
				for (int Index = 0; Index < Tries; Index++)
				{
					if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
				}
				if (!runApp.HasExited)
				{
					Result = "Application did not finish in time.";
				}
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName + " Result: " + Result, Method, 0);
			}
			return Result;
		}
	}
}
