/*
 * Created by SharpDevelop.
 * User: John
 * Date: 15/08/2019
 * Time: 3:39 PM
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
	/// Description of BuildReportListForReportId.
	/// </summary>
	public class BuildReportListForReportId : ActionBase
	{
		private string FModuleName = "PaymentOperations";
		private ProcessPreviewReportRequestItem FRequestItem;
			
		/*
		public static string Execute(int MasterId,
									 RequestCommonItem CommonItem,
									 JObject DataObject,
									 APISettingsItem SettingsItem,
									 out string ErrorMsg)
		*/
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			BuildReportListForReportId inst = new BuildReportListForReportId();
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
				FRequestItem = new ProcessPreviewReportRequestItem();
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
				
				StringBuilder Entries = 
					new global::UsefulDBMethods.Reporting.ReportUtilities().BuildReportFileList(
						FbConnectionString, FRequestItem.ReportId, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
						this.CommonCollection, this.DataCollection, "Items", out ErrorMsg);
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();						
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
			
			/*
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
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
					
					if (string.IsNullOrEmpty(Result))
					{
						DataTable tbl = 
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.None, out Result);
						if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count > 0))
						{
							StringBuilder Entries = new StringBuilder();
							Entries.AppendLine(JsonUtilities.StartJson());
							
							Result = CommonMethods.BuildJsonArrayFromDataTable(tbl, "whyNotCancelled", ref Entries);

							Entries.AppendLine(JsonUtilities.EndJson());
							Result = Entries.ToString();
						}
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
			*/
		}
	}
