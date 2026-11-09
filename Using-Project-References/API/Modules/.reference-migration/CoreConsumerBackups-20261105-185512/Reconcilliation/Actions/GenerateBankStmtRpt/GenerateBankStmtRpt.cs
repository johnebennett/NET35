/*
 * Created by SharpDevelop.
 * User: John
 * Date: 24/10/2019
 * Time: 10:20 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;

/*
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
*/

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation.GenerateBankStmtRpt
{
	/// <summary>
	/// Description of GenerateBankStmtRpt.
	/// </summary>
	public class GenerateBankStmtRpt : ActionBase
	{
		private string FModuleName = "Reconcilliation";
		private GenerateBankStmtRptRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			GenerateBankStmtRpt inst = new GenerateBankStmtRpt();
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
				FRequestItem = new GenerateBankStmtRptRequestItem();
				Result = FRequestItem.Load((ActionBase)this);
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
                //int NewId = 
				//UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
				//FbConnectionString, "I_SYS_ID_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out Result);
				string ReportSource = "SYSTEM-EOMREPORT#2$" + FRequestItem.BankIdValue;

                string OutputFileName = "";
                if (string.IsNullOrEmpty(Result))
                {
                    long UniqueId = (long)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
                    Int64 NewId =
                        CommonUtilities.SystemReportUtilities.Execute(
                            FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
                            UniqueId, ReportSource, out Result, out OutputFileName);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        JsonUtilities.StartJson() +
                        JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false) +
                        JsonUtilities.EndJson();
                }
                else
                {
                    Result = LogOnProcessingError(Method, Result, "");
                }

                //if (string.IsNullOrEmpty(Result))
				//{
				//	Result = 
				//		CommonMethods.InsertEASystemReportRecord(
				//			FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, NewId, FRequestItem.BankId, ReportSource);
				//}
				
				//if (string.IsNullOrEmpty(Result))
				//{
				//	Result = CommonMethods.PerformStep1(FbConnectionString, NewId, CommonItem.CompanyIdValue, CommonItem.ClientIdValue);
				//}
				
				//string SqlToUse = "";
				//string OutputFileName = "";
				//string OutputFileNameOnly = "";
				//string Extension = "";
				//if (string.IsNullOrEmpty(Result))
				//{
				//	Result = 
				//		CommonMethods.GetSqlToUse(
				//			FbConnectionString, CommonItem.CompanyIdValue, NewId, out SqlToUse, out OutputFileName, out OutputFileNameOnly, out Extension);
				//}
				
				//if (string.IsNullOrEmpty(Result))
				//{
				//	Result = CommonMethods.WriteOutputFile(FbConnectionString, SqlToUse, OutputFileName, Extension);
				//}
				
				//if (!string.IsNullOrEmpty(Result))
				//{
				//	Result = LogOnProcessingError(Method, Result, "");
				//}
				//else
				//{
				//	Result =
				//		JsonUtilities.StartJson() +
				//		JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileNameOnly + Extension, false) +
				//		JsonUtilities.EndJson();
				//}
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
	}
}
