/*
 * Created by SharpDevelop.
 * User: John
 * Date: 10/08/2019
 * Time: 7:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

/*
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
*/

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of PaymentListReport.
	/// </summary>
	public class PaymentListReport : ActionBase
	{
			private string FModuleName = "PaymentOperations";
			private PaymentListReportRequestItem FRequestItem;
			
			public static string Execute(int MasterId,
										 APISettingsItem SettingsItem,
										 string Request,
										 out string ErrorMsg)
			{
				string Method = "Execute";
				string Result = "";
				ErrorMsg = "";

				PaymentListReport inst = new PaymentListReport();
				//if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
					FRequestItem = new PaymentListReportRequestItem();
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
				string ErrorMsg = "";
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
				LogOnInsideMethod(Method);
				try 
				{
					Result = BuildOutput(out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg))
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
			
			private string BuildOutput(out string ErrorMsg)
			{
				ErrorMsg = "";
				string Result = "";
				string Method = "BuildOutput";
				
				LogOnInsideMethod(Method);
				try
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
					string ReportSql = "";
					int EventRunId = 0;
					
					ErrorMsg = CommonMethods.BuildReportSql((ActionBase)this, FRequestItem.ReportId, out ReportSql, out EventRunId);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Result = LogResponse(Method, Result);
					}
					
					/*
					StringBuilder entries = new StringBuilder();
					entries.AppendLine(JsonUtilities.StartJson());
						
					ErrorMsg = BuildOutputEntries(ref entries);				
					if (!string.IsNullOrEmpty(ErrorMsg)) return "";
					
					entries.AppendLine(JsonUtilities.EndJson());
					Result = entries.ToString();
					*/
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
			
			private string BuildOutputEntries(ref StringBuilder Entries)
			{
				string Method = "BuildOutputEntries";
				string Result = "";			
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
				
				LogOnInsideMethod(Method);
				try
				{
					string QueryText = "";
					List<FbParameterItem> Params = new List<FbParameterItem>();
					//Params.Add(new FbParameterItem("DisplayType", FSearchGridRequestItem.DisplayType));
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
					
					foreach (FbParameterItem Param in Params)
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent(
							Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue), FModuleName, MasterId);
					}

					DataTable dt = null;
					try 
					{
						dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);
						if (dt == null)
						{
							Result = "No tables in the dataset to process";
							Result = LoggingEntry(Method, Result, "");
							return Result;
						}
					}					
					catch (Exception Ex)
					{
						Result = LogOnException(Method, Ex, "");
						return Result;
					}
					
					string Str = "";
					string ColName = "";
					string ColValue = "";
					List<string> RowList = new List<string>();
					List<string> StrList = new List<string>();
					object Obj = null;
					Entries.AppendLine(JsonUtilities.StartJsonArray("Items"));
					foreach (DataRow ThisRow in dt.Rows)
					{
						StrList.Clear();
						foreach (DataColumn ThisCol in dt.Columns)
						{
							ColName = ThisCol.ColumnName;
							Obj = ThisRow[ColName];
							ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
							StrList.Add(JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
						}
						Str =
							JsonUtilities.StartJsonObject("") +
							string.Join(",", StrList.ToArray()) +
							JsonUtilities.EndJsonObject();
						RowList.Add(Str);
					}
					Entries.Append(string.Join(",", RowList.ToArray()));
					Entries.AppendLine(JsonUtilities.EndJsonArray());
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
