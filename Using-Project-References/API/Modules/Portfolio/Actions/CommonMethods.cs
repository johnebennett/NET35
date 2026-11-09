/*
 * Created by SharpDevelop.
 * User: John
 * Date: 14/01/2019
 * Time: 11:07 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Diagnostics;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Portfolio
{
	/// <summary>
	/// Methods that are common to the Actions in the Portfolio module
	/// </summary>
	public class CommonMethods// : ActionBase
	{
		private string FModuleName = "Portfolio";
		
		public CommonMethods()
		{
		}		
		
		public static string GetModuleName() { return "Portfolio"; }
		
		public static string BuildFilterComboBoxEntries(string ConnectionString,
		                                                int CompanyId,
		                                                int MasterId,
		                                                int PortfolioId,
		                                                ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildFilterComboBoxEntries";			
			
			CommonMethods inst = new CommonMethods();
			string ModuleName = inst.FModuleName;
			
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, ModuleName, MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("PortfolioId: " + PortfolioId, ModuleName, MasterId);
				string QueryText = "SELECT * FROM EAGetFilterListByTypeId({0}, {1})";
				QueryText = string.Format(QueryText, CompanyId, PortfolioId);
				DataSet ds = FirebirdUtilities.GetDataSetFromQuery(ConnectionString, QueryText, out Result);
				if (!string.IsNullOrEmpty(Result))
				{
						Result = 
							"Error Executing Query" + Environment.NewLine +
							Result + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(Result, ModuleName, MasterId);
						return Result;
				}
				else if (ds.Tables.Count == 0) 
				{
					Result = "No tables in the dataset to process";
					return Result;
				}
				else
				{
					Result = BuildFilterComboBoxEntryItems("filters1", ds.Tables[0], ref Entries);
					if (!string.IsNullOrEmpty(Result)) return Result;
					
					Entries.Append(",");
					Result = BuildFilterComboBoxEntryItems("filters2", ds.Tables[0], ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		private static string BuildFilterComboBoxEntryItems(string ArrayName,
		                                                    DataTable Source,
		                                                    ref StringBuilder Entries)
		{
			string Result = "";
			const string Method = "BuildFilterComboBoxEntryItems";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, "", 0);
			
			try 
			{
				List<string> RowList = new List<string>();
				object Obj = null;
				string FilterName = "";
				string FilterId = "";
				string Str = "";

	            Entries.AppendLine(CommonUtilities.JsonUtilities.StartJsonArray(ArrayName));

				// Row entries
				RowList.Clear();
				Str =
					CommonUtilities.JsonUtilities.StartJsonObject("") +
					CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Name", "Select a Filter", true) +
					CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Value", "0", false) +
					CommonUtilities.JsonUtilities.EndJsonObject();
				RowList.Add(Str);
				foreach(DataRow ARow in Source.Rows)
				{
					Obj = ARow["FilterName"];
					FilterName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = ARow["FilterId"];
					FilterId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Str =
						CommonUtilities.JsonUtilities.StartJsonObject("") +
						CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Name", FilterName, true) +
						CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Value", FilterId, false) +
						CommonUtilities.JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				Entries.Append(string.Join(",", RowList.ToArray()));
	            Entries.AppendLine(CommonUtilities.JsonUtilities.EndJsonArray());
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		public static string BuildListReportEntries(string ConnectionString,
		                                            int CompanyId,
		                                            int MasterId,
		                                            int PortfolioId,
		                                            ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildListReportEntries";
			
			CommonMethods inst = new CommonMethods();
			string ModuleName = inst.FModuleName;
			
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, ModuleName, MasterId);
				string QueryText = "SELECT ReportName, ReportId FROM ListReports({0}, '{1}')";
				QueryText = string.Format(QueryText, CompanyId, "Portfolio" + PortfolioId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Query" + Environment.NewLine + QueryText, ModuleName, MasterId);
				DataSet ds = FirebirdUtilities.GetDataSetFromQuery(ConnectionString, QueryText, out Result);
				if (!string.IsNullOrEmpty(Result))
				{
						Result = 
							"Error Executing Query" + Environment.NewLine +
							Result + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(Result, ModuleName, MasterId);
						//Result = LogOnErrorSelectingData(Method, Result, QueryText);
				}
				else if (ds.Tables.Count == 0) 
				{
					Result = "No tables in the dataset to process";
					//Result = LogOnErrorSelectingData(Method, Result, QueryText);
				}				
				else Result = BuildListReportEntryItems(ref Entries, ds.Tables[0]);
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		private static string BuildListReportEntryItems(ref StringBuilder Entries,
		                                                System.Data.DataTable Source)
		{
			string Result = "";
			const string Method = "BuildListReportEntryItems";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, "", 0);
			
			try 
			{
				List<string> RowList = new List<string>();
				object Obj = null;
				string ReportName = "";
				string ReportId = "";
				string Str = "";

	            Entries.Append(CommonUtilities.JsonUtilities.StartJsonArray("reports"));

				// Row entries
				RowList.Clear();
				foreach (DataRow Row in Source.Rows)
				{
					Obj = Row["ReportName"];
					ReportName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["ReportId"];
					ReportId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Str =
						CommonUtilities.JsonUtilities.StartJsonObject("") +
						CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Name", ReportName, true) +
						CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Value", ReportId, false) +
						CommonUtilities.JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				Entries.Append(string.Join(",", RowList.ToArray()));
	            Entries.AppendLine(CommonUtilities.JsonUtilities.EndJsonArray());
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;			
		}
		
		public static string GetFilterValues(int Value,
		                                     JObject DataObject,
		                                     out string AskLater,
		                                     out string AskFor,
		                                     out string FromValue,
		                                     out string FromDate,
		                                     out string ToValue,
		                                     out string ToDate)
		{
			string Result = "";
			AskLater = "";
			AskFor = "";
			FromValue = "";
			FromDate = "";
			ToValue = "";
			ToDate = "";
			
			try 
			{
				string ValueStr = Convert.ToString(Value);
				AskLater = (string)DataObject["askLater" + ValueStr];
				AskFor = (string)DataObject["askFor" + ValueStr];
				FromValue = (string)DataObject["fromValue" + ValueStr];
				FromDate =  (string)DataObject["fromDate" + ValueStr];
				ToValue =  (string)DataObject["toValue" + ValueStr];
				ToDate =  (string)DataObject["toDate" + ValueStr];                                                                                            
			}
			catch (Exception Ex)
			{				
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		public static int FilterEventForPortfolio(string ConnectionString,
		                                          int CompanyId,
		                                          int ClientId,
		                                          int Filter1Id,
		                                          int Filter2Id,
		                                          string EventSql,
		                                          string Source,
		                                          out string ErrorMsg)
		{
			ErrorMsg = "";
			string MethodName = "FilterEventForPortfolio";
			//int Result = UsefulDBMethods.EventHelpers.GetNextFilterEventGeneratorId(ConnectionString, CompanyId, ClientId, out ErrorMsg);
			int Result = UsefulDBMethods.EventHelpers.SetupFilterEventRecord(ConnectionString, CompanyId, ClientId, 0, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error getting next generator: " + ErrorMsg, MethodName, 0);
				return 0;
			}
			
			// update the record with the other values
			string QueryText =
				" UPDATE EAFilterEvent" +
				"    SET ea_FilterEventSql = @EventSql," +
				"        ea_FilterInt1 = @Filter1Id," +
				"        ea_FilterInt2 = @Filter2Id," +
				"        ea_String1 = @Source" +
				" WHERE ea_FilterEventId = @EventId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@EventSql", EventSql));
			Params.Add(new FbParameterItem("@Filter1Id", Filter1Id));
			Params.Add(new FbParameterItem("@Filter2Id", Filter2Id));
			Params.Add(new FbParameterItem("@Source", Source));
			Params.Add(new FbParameterItem("@EventId", Result));
			
			ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			
			return Result;
		}
		
		/*
		public static string LoadBusinessContactsItem(JObject DataObject,
		                                              ref BusinessContactsItem TempItem)
		{
			string Result = "";
			string Method = "LoadBusinessContactsItem";
			try 
			{
				TempItem.PortfolioIdValue = (string)DataObject["PortfolioId"];
				TempItem.CategoryOnlyIdValue = (string)DataObject["CategoryOnlyId"];
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
		*/
		
		public static string LoadDisplayReportRequestItem(ActionBase AActionBase,
		                                                  ref DisplayReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDisplayReportRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "dlgType");
				TempItem.DlgType = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "passedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "buttonId");
				TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "portfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "categoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string LoadOwnersCertificateDialogContentRequestItem(ActionBase ActionBase,
		                                                                   ref OwnersCertificateDialogContentRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadOwnersCertificateDialogContentRequestItem";
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "CategoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "RecordId");
				TempItem.RecordIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string EAOCCertificateGroup(string ConnectionString,
		                                          Int64 CompanyId,
		                                          Int64 NewId,
		                                          int CategoryOnlyId,
		                                          Int64 AddedById,
		                                          ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAOCCertificateGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				string QueryText =
					"SELECT OutputLine FROM EAOCCerticateGroup(@CompanyId, @NewId, @CategoryOnlyId, @AddedById)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("NewId", NewId));
				Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
				Params.Add(new FbParameterItem("AddedById", AddedById));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					foreach (DataRow dataRow in tbl.Rows)
					{
						Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
					}
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException; 
			}
			
			return Result;
		}		                                        
		
		public static string EAOCCertificateAdd(string ConnectionString,
		                                        Int64 RecordId,
		                                        Int64 CompanyId,
		                                        int OCLevyBusinessId,
		                                        string OCAddrress,
		                                        string OCInPresenceOf,
		                                        string OCLot,
		                                        string OCPlanNo,
		                                        string OCPostalAddress,
		                                        string OCApplicant,
		                                        string OCDeliveryAddress,
		                                        string OCDate,
		                                        string OCDateIssued,
		                                        string OCEntityName)
		{
			string Result = "";
			string Method = "EAOCCertificateAdd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				string QueryText =
					"EXECUTE PROCEDURE EAOCCertificateAdd(" +
					"  @RecordId, @CompanyId, @PortfolioId, @Address, @InPresenceOf, @Lot, @PlanNo, @PostalAddress," +
				    "  @Applicant, @DeliveryAddress, @OCDate, @OCDateIssued, @EntityName)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RecordId", RecordId));
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("PortfolioId", OCLevyBusinessId));

				Params.Add(new FbParameterItem("Address", OCAddrress));
				Params.Add(new FbParameterItem("InPresenceOf", OCInPresenceOf));
				Params.Add(new FbParameterItem("Lot", OCLot));
				Params.Add(new FbParameterItem("PlanNo", OCPlanNo));
				Params.Add(new FbParameterItem("PostalAddress", OCPostalAddress));
				Params.Add(new FbParameterItem("Applicant", OCApplicant));
				Params.Add(new FbParameterItem("DeliveryAddress", OCDeliveryAddress));
				Params.Add(new FbParameterItem("OCDate", OCDate));
				Params.Add(new FbParameterItem("OCDateIssued", OCDateIssued));
				Params.Add(new FbParameterItem("EntityName", OCEntityName));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException; 
			}
			
			return Result;
		}		                                        
		
		public static string LoadUpdateOCCertificateRequestItem(ActionBase ActionBase,
		                                                        ref UpdateOCCertificateRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadUpdateOCCertificateRequestItem";
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "RecordId");
				TempItem.RecordIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Address");
				TempItem.Address = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "InPresenceOf");
				TempItem.InPresenceOf = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "LotNo");
				TempItem.LotNo = (Obj == null) ? "" : Convert.ToString(Obj);

				Obj = ActionBase.GetCollectionValue("Data", "PlanNo");
				TempItem.PlanNo = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PostalAddress");
				TempItem.PostalAddress = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Applicant");
				TempItem.Applicant = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DeliveryAddress");
				TempItem.DeliveryAddress = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "OCDate");
				TempItem.OCDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DateIssued");
				TempItem.IssuedDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "EntityName");
				TempItem.EntityName = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "CategoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NoteContent");
				TempItem.NoteContent = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string LoadDisplayOCCertificateReportRequestItem(ActionBase ActionBase,
		                                                               ref DisplayOCCertificateReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "DisplayOCCertificateReportRequestItem";
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "RecordId");
				TempItem.RecordIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "CategoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				
				Obj = ActionBase.GetCollectionValue("Data", "Address");
				TempItem.Address = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "InPresenceOf");
				TempItem.InPresenceOf = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "LotNo");
				TempItem.LotNo = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PlanNo");
				TempItem.PlanNo = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PostalAddress");
				TempItem.PostalAddress = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Applicant");
				TempItem.Applicant = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DeliveryAddress");
				TempItem.DeliveryAddress = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "OCDate");
				TempItem.OCDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DateIssued");
				TempItem.IssuedDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "EntityName");
				TempItem.EntityName = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NoteContent");
				TempItem.NoteContent = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string LoadDisplayMgmtPdfReportRequestItem(ActionBase ActionBase,
		                                                         ref DisplayMgmtPdfReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDisplayMgmtPdfReportRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "TransactionId");
				TempItem.TransactionIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "AccountId");
				TempItem.AccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BtnId");
				TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "PassedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string GetOCCertReportSource(string ConnectionString,
		                                           Int64 CompanyId,
		                                           int PassedId,
		                                           int CategoryOnlyId,
		                                           int ClientId,
		                                           out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetOCCertReportSource";
			StringBuilder Entries = new StringBuilder();
			ErrorMsg = CommonMethods.EAOCCertificateGroup(ConnectionString, CompanyId, PassedId, CategoryOnlyId, ClientId, ref Entries);
			string Str = "";
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				Str =
					JsonUtilities.StartJson() +
					Entries.ToString() +
					JsonUtilities.EndJson();
			
				WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, Method, 0);
				
				try 
				{
					if (Entries.Length > 0)
					{
						JObject jobj = JsonConvert.DeserializeObject<JObject>(Str);
						if (jobj == null)
						{
							Result = "Error deserializing the Request";
						}
						else
						{
							Result = (string)jobj["certificate_reportsource"];
							if (string.IsNullOrEmpty(Result)) ErrorMsg = "Error getting report source";
						}
					}
				}
				catch (Exception Ex)
				{
					ErrorMsg =
						Method + Environment.NewLine +
						Ex.Message + Environment.NewLine +
						Ex.StackTrace;					
				}
			}
			
			return Result;
		}
		
		public static string LoadDisplayStmtKitReportRequestItem(ActionBase ActionBase,
		                                                         ref DisplayStmtKitReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDisplayStmtKitReportRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "newId");
				TempItem.NewIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "accountId");
				TempItem.AccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "btnId");
				TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "passedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "categoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string LoadIncludeStmtKitForAccountRequestItem(ActionBase ActionBase,
		                                                             ref IncludeStmtKitForAccountRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadIncludeStmtKitForAccountRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "AccountId");
				TempItem.AccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "CategoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "IsChecked");
				TempItem.IsChecked = (Obj == null) ? "" : Convert.ToString(Obj);
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string LoadIncludeStmtKitForAccountReportRequestItem(ActionBase ActionBase,
		                                                                   ref IncludeStmtKitForAccountReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadIncludeStmtKitForAccountReportRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "AccountId");
				TempItem.AccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReportId");
				TempItem.ReportIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string LoadDisplayResBondsReportRequestItem(ActionBase ActionBase,
		                                                          ref DisplayResBondsReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDisplayResBondsReportRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "portfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "passedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "btnId");
				TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string LoadProcessResBondsDirectDebitRequestItem(ActionBase ActionBase,
		                                                               ref ProcessResBondsDirectDebitRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadProcessResBondsDirectDebitRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "portfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "bondNumber");
				TempItem.BondNumber = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string LoadProcessResBondsBPayRequestItem(ActionBase ActionBase,
		                                                        ref ProcessResBondsBPayRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadProcessResBondsBPayRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "portfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "billerCode");
				TempItem.BillerCode = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "releaseDate");
				TempItem.ReleaseDate = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string LoadCommBondsTransferRequestItem(ActionBase ActionBase,
		                                                      ref CommBondsTransferRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadCommBondsTransferRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
				TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Details");
				TempItem.Details = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "LodgedAmount");
				TempItem.LodgedAmount = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "LodgedOn");
				TempItem.LodgedOn = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "ExpiryDate");
				TempItem.ExpiryDate = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DoesNotExpire");
				TempItem.DoesNotExpire = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "BondHeld");
				TempItem.BondHeldValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "BSB");
				TempItem.BSB = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "AccountNumber");
				TempItem.AccountNumber = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ShortBankName");
				TempItem.ShortBankName = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}

        /*
        public static string Portfolio_CommBondsApplyChanges(string ConnectionString,
		                                                     Int64 CompanyId,
		                                                     Int64 ClientId,
		                                                     Int64 PortfolioId,
		                                                     int SubLedgerId,
		                                                     string Details,
		                                                     string LodgedAmount,
		                                                     string LodgedOn,
		                                                     string ExpiryDate,
		                                                     string DoesNotExpire,
		                                                     int BondHeld)
		{
			string Result = "";
			string Method = "Portfolio_CommBondsApplyChanges";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<string> RawScripts = new List<string>();
				List<string> ParameterList = new List<string>();
				List<FbParameterItem> Params = new List<FbParameterItem>();
				string QueryText = "";
				string DisplayStr = "";
				string ParamStr = "";
				Result = GetCommBondsUpdateScripts(ConnectionString, CompanyId, ClientId, PortfolioId, SubLedgerId, ref RawScripts);
				//Result = GetCommBondsSqlStatement(ConnectionString, CompanyId, ClientId, PortfolioId, SubLedgerId, ref RawScripts);
				if (string.IsNullOrEmpty(Result))
				{
					foreach (string Script in RawScripts)
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Processing Script" + Environment.NewLine + Script, Method, 0);
						ParameterList.Clear();
						Params.Clear();
						QueryText = Script;
						Result = GetSqlParameterList(QueryText, ref ParameterList);
						if (string.IsNullOrEmpty(Result))
						{
							DisplayStr = "";
							foreach (string Str in ParameterList)
							{
								ParamStr = Str;
								DisplayStr += ParamStr;
								DisplayStr += Environment.NewLine;
								ParamStr = ParamStr.Replace("@", "");
								QueryText = QueryText.Replace("@@" + ParamStr + "@@", "@" + ParamStr);
								switch (ParamStr.ToUpper())
								{
									case "LODGEDAMOUNT":
										Params.Add(new FbParameterItem(ParamStr, LodgedAmount));
									break;
									case "COMBOVALUE":
										Params.Add(new FbParameterItem(ParamStr, BondHeld));
									break;
									case "LODGEDNUMBER":
										Params.Add(new FbParameterItem(ParamStr, Details));
									break;
									case "LODGEDDATE":
										Params.Add(new FbParameterItem(ParamStr, LodgedOn));
									break;
									case "NOEXPIRYTICKBOX":
										Params.Add(new FbParameterItem(ParamStr, DoesNotExpire));
									break;
									case "EXPIRYDATE":
										Params.Add(new FbParameterItem(ParamStr, ExpiryDate));
									break;
									//case "BSB":
									//	Params.Add(new FbParameterItem(ParamStr, BSB));
									//break;
									//case "ACCOUNTNUMBER":
									//	Params.Add(new FbParameterItem(ParamStr, AccountNumber));
									//break;
									//case "SHORTBANKNAME":
									//	Params.Add(new FbParameterItem(ParamStr, ShortBankName));
									//break;
									//case "UNIQUEPAYMENTID":
									//	Params.Add(new FbParameterItem(ParamStr, UniqueId));
									//break;
								}
							}
							WindowsEventLoggingUtilities.WriteToInfoEvent("Processing Script" + Environment.NewLine + QueryText, Method, 0);
							WindowsEventLoggingUtilities.WriteToInfoEvent("Parameters" + Environment.NewLine + DisplayStr, Method, 0);
							UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
							if (string.IsNullOrEmpty(Result))
							{
								Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
							}
						}
						else
						{
							WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		*/

        public static string Portfolio_CommBondsApplyChanges(string ConnectionString,
                                                             Int64 CompanyId,
                                                             Int64 ClientId,
                                                             Int64 PortfolioId,
                                                             int SubLedgerId,
                                                             string Details,
                                                             string LodgedAmount,
                                                             string LodgedOn,
                                                             string ExpiryDate,
                                                             string DoesNotExpire,
                                                             int BondHeld)
        {
            string Result = "";
            string Method = "Portfolio_CommBondsApplyChanges";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();
            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioCommBondsList(
                            ConnectionString, CompanyId, ClientId, (int)PortfolioId, SubLedgerId, ref TempEntries);
                }

                string Value = "";
                JObject JSONSource = null;
                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Value =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();

                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";

                    if (string.IsNullOrEmpty(Result))
                    {
                        //ActionBase.LogResponse(Method, TempEntries.ToString());
                        JSONSource = JObject.Parse(Value);
                        if (JSONSource == null) Result = "Cannot parse the Group source";
                        Value = "";
                    }
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.updatesql1", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first update query";
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("LodgedAmount", LodgedAmount));
                    Params.Add(new FbParameterItem("ComboValue", BondHeld));
                    Params.Add(new FbParameterItem("LodgedNumber", Details));
                    Params.Add(new FbParameterItem("LodgedDate", LodgedOn));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.updatesql2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second update query";
                }

                Params.Clear();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("NOEXPIRYTICKBOX", DoesNotExpire));
                    Params.Add(new FbParameterItem("EXPIRYDATE", ExpiryDate));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        /*
        public static string Portfolio_CommBondsOnTransferCmd(string ConnectionString,
		                                                      Int64 CompanyId,
		                                                      Int64 ClientId,
		                                                      Int64 PortfolioId,
		                                                      int SubLedgerId,
		                                                      string Details,
		                                                      string LodgedAmount,
		                                                      string LodgedOn,
		                                                      string ExpiryDate,
		                                                      string DoesNotExpire,
		                                                      int BondHeld,
		                                                      string BSB,
		                                                      string AccountNumber, 
		                                                      string ShortBankName,
		                                                      int UniqueId)
		{
            string Result = "";
			string Method = "Portfolio_CommBondsOnTransferCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<string> RawScripts = new List<string>();
				List<string> ParameterList = new List<string>();
				List<FbParameterItem> Params = new List<FbParameterItem>();
				string QueryText = "";
				string DisplayStr = "";
				string ParamStr = "";
				Result = GetCommBondsUpdateScripts(ConnectionString, CompanyId, ClientId, PortfolioId, SubLedgerId, ref RawScripts);
				if (string.IsNullOrEmpty(Result))
				{
					Result = GetCommBondsSqlStatement(ConnectionString, CompanyId, ClientId, PortfolioId, SubLedgerId, ref RawScripts);
				}
				if (string.IsNullOrEmpty(Result))
				{
					foreach (string Script in RawScripts)
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Processing Script" + Environment.NewLine + Script, Method, 0);
						ParameterList.Clear();
						Params.Clear();
						QueryText = Script;
						Result = GetSqlParameterList(QueryText, ref ParameterList);
						if (string.IsNullOrEmpty(Result))
						{
							DisplayStr = "";
							foreach (string Str in ParameterList)
							{
								ParamStr = Str;
								DisplayStr += ParamStr;
								DisplayStr += Environment.NewLine;
								ParamStr = ParamStr.Replace("@", "");
								QueryText = QueryText.Replace("@@" + ParamStr + "@@", "@" + ParamStr);
								switch (ParamStr.ToUpper())
								{
									case "LODGEDAMOUNT":
										Params.Add(new FbParameterItem(ParamStr, LodgedAmount));
									break;
									case "COMBOVALUE":
										Params.Add(new FbParameterItem(ParamStr, BondHeld));
									break;										
									case "LODGEDNUMBER":
										Params.Add(new FbParameterItem(ParamStr, Details));
									break;										
									case "LODGEDDATE":
										Params.Add(new FbParameterItem(ParamStr, LodgedOn));
									break;										
									case "NOEXPIRYTICKBOX":
										Params.Add(new FbParameterItem(ParamStr, DoesNotExpire));
									break;										
									case "EXPIRYDATE":
										Params.Add(new FbParameterItem(ParamStr, ExpiryDate));
									break;										
									case "BSB":
										Params.Add(new FbParameterItem(ParamStr, BSB));
									break;										
									case "ACCOUNTNUMBER":
										Params.Add(new FbParameterItem(ParamStr, AccountNumber));
									break;										
									case "SHORTBANKNAME":
										Params.Add(new FbParameterItem(ParamStr, ShortBankName));
									break;										
									case "UNIQUEPAYMENTID":
										Params.Add(new FbParameterItem(ParamStr, UniqueId));
									break;										
								}
							}
							WindowsEventLoggingUtilities.WriteToInfoEvent("Processing Script" + Environment.NewLine + QueryText, Method, 0);
							WindowsEventLoggingUtilities.WriteToInfoEvent("Parameters" + Environment.NewLine + DisplayStr, Method, 0);
							UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
							if (string.IsNullOrEmpty(Result))
							{
								Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
							}
						}
						else
						{
							WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		*/

        public static string Portfolio_CommBondsOnTransferCmd(string ConnectionString,
                                                              Int64 CompanyId,
                                                              Int64 ClientId,
                                                              Int64 PortfolioId,
                                                              int SubLedgerId,
                                                              string Details,
                                                              string LodgedAmount,
                                                              string LodgedOn,
                                                              string ExpiryDate,
                                                              string DoesNotExpire,
                                                              int BondHeld,
                                                              string BSB,
                                                              string AccountNumber,
                                                              string ShortBankName,
                                                              int UniqueId)
        {
            string Result = "";
            string Method = "Portfolio_CommBondsOnTransferCmd";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();
            ActionBase.LogOnInsideMethod(Method);
            try
            {
                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        Portfolio_CommBondsApplyChanges(
                            ConnectionString, CompanyId, ClientId, PortfolioId, SubLedgerId, Details,
                            LodgedAmount, LodgedOn, ExpiryDate, DoesNotExpire, BondHeld);
                }

                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioCommBondsList(
                            ConnectionString, CompanyId, ClientId, (int)PortfolioId, SubLedgerId, ref TempEntries);
                }

                string Value = "";
                JObject JSONSource = null;
                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Value =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();

                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";

                    if (string.IsNullOrEmpty(Result))
                    {
                        //ActionBase.LogResponse(Method, TempEntries.ToString());
                        JSONSource = JObject.Parse(Value);
                        if (JSONSource == null) Result = "Cannot parse the Group source";
                        Value = "";
                    }
                }

                string WhatToShow1 = "";
                string WhatToShow2 = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    WhatToShow1 = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.WhatToShow1", out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        WhatToShow2 = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.WhatToShow2", out Result);
                    }

                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(WhatToShow1) && string.IsNullOrEmpty(WhatToShow2))
                        Result = "Both of the ShowWhat values are empty";

                    if (string.IsNullOrEmpty(Result) && (WhatToShow1 == "N") && (WhatToShow2 == "N"))
                        Result = "Neither of the ShowWhat values are set";
                }

                if (string.IsNullOrEmpty(Result) && (WhatToShow1 == "Y"))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.Show1SQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Show1 query";
                }
                else if (string.IsNullOrEmpty(Result) && (WhatToShow2 == "Y"))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.Show2SQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second Show1 query";
                }

                if (string.IsNullOrEmpty(Result) && (QueryText != ""))
                {
                    QueryText = QueryText.Replace("__", "'");
                    Params.Add(new FbParameterItem("BSB", BSB));
                    Params.Add(new FbParameterItem("AccountNumber", AccountNumber));
                    Params.Add(new FbParameterItem("UniquePaymentId", UniqueId));
                    Params.Add(new FbParameterItem("ShortBankName", ShortBankName));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

                    if (string.IsNullOrEmpty(Result))
                        Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    //Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        private static string GetCommBondsUpdateScripts(string ConnectionString,
		                                                Int64 CompanyId,
		                                                Int64 ClientId,
		                                                Int64 PortfolioId,
		                                                int SubLedgerId,
		                                                ref List<string> UpdateScripts)
		{
			string Result = "";
			string Method = "GetCommBondsUpdateScripts";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			StringBuilder Entries = new StringBuilder();
			
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				Result =
					Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioCommBondsList(
						ConnectionString, CompanyId, ClientId, (int)PortfolioId, SubLedgerId, ref Entries);
				
				string Str = "";
				if (string.IsNullOrEmpty(Result))
				{
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				
					WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, Method, 0);
					
					if (Entries.Length > 0)
					{
						JObject jobj = JsonConvert.DeserializeObject<JObject>(Str);
						if (jobj == null)
						{
							Result = "Error deserializing the Request";
						}
						else
						{
							for (int Index = 1; Index <= 100; Index++)
							{
								Str = (string)jobj["updatesql" + Index];
								if (string.IsNullOrEmpty(Str)) break;
								else
								{
									Str = Str.Replace("__", "'");
									Str = Str.Replace("@:", "@");
									Str = Str.Replace("'@", "@");
									Str = Str.Replace("@'", "@");
									UpdateScripts.Add(Str);
								}
							}
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
				

			return Result;
		}
		
		private static string GetCommBondsSqlStatement(string ConnectionString,
		                                               Int64 CompanyId,
		                                               Int64 ClientId,
		                                               Int64 PortfolioId,
		                                               int SubLedgerId,
		                                               ref List<string> Scripts)
		{
			string Result = "";
			string Method = "GetCommBondsSqlStatement";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			StringBuilder Entries = new StringBuilder();
			
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				Result =
					Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioCommBondsList(
						ConnectionString, CompanyId, ClientId, (int)PortfolioId, SubLedgerId, ref Entries);
				
				string Str = "";
				if (string.IsNullOrEmpty(Result))
				{
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				
					WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, Method, 0);
					
					if (Entries.Length > 0)
					{
						JObject jobj = JsonConvert.DeserializeObject<JObject>(Str);
						if (jobj == null)
						{
							Result = "Error deserializing the Request";
						}
						else
						{
							Str = (string)jobj["SQLSTATEMENT"];
							Str = Str.Replace("__", "'");
							Str = Str.Replace("@:", "@");
							Str = Str.Replace("'@", "@");
							Str = Str.Replace("@'", "@");
							Scripts.Add(Str);
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
				

			return Result;
		}
		
		public static string  GetSqlParameterList(string SourceSql,
		                                          ref List<string> Parameters)
		{
			string Result = "";
			string Method = "GetSqlParameterList";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			string Str = "";

			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				List<int> ParameterStartPos = new List<int>();
				int StartPos = 0;
				int EndPos = 0;
				// Parameters start with @@ and end with the following @@
				while (StartPos >= 0)
				{
					StartPos = SourceSql.IndexOf("@@", StartPos);
					if (StartPos > 0)
					{
						EndPos = SourceSql.IndexOf("@@", StartPos + 2);
					}
					Str = SourceSql.Substring(StartPos, EndPos - StartPos + 2);
					//Parameters.Add(SourceSql.Substring(StartPos, EndPos - StartPos + 2));
					Parameters.Add(Str);
					StartPos = EndPos + 2;
				}
				
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string LoadCommBondsApplyChangesRequestItem(ActionBase ActionBase,
		                                                          ref CommBondsApplyChangesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadCommBondsApplyChangesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
				TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Details");
				TempItem.Details = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "LodgedAmount");
				TempItem.LodgedAmount = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "LodgedOn");
				TempItem.LodgedOn = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "ExpiryDate");
				TempItem.ExpiryDate = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DoesNotExpire");
				TempItem.DoesNotExpire = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "BondHeld");
				TempItem.BondHeldValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				/*
				Obj = ActionBase.GetCollectionValue("Data", "BSB");
				TempItem.BSB = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "AccountNumber");
				TempItem.AccountNumber = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ShortBankName");
				TempItem.ShortBankName = (Obj == null) ? "" : Convert.ToString(Obj);
				*/ 
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string Portfolio_EANotesAddGroup(string ConnectionString,
		                                               Int64 CompanyId,
		                                               Int64 ClientId,
		                                               Int64 PortfolioId,
		                                               int CategoryOnlyId,
		                                               string NoteToAdd,
		                                               string DefaultArrears,
		                                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "Portfolio_EANotesAddGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("PortfolioId", PortfolioId));
				string QueryText = "SELECT OutputLine FROM EANotesAddGroup(@CompanyId, @TransactionId, @CategoryOnlyId, @AddedById, @NoteToAdd, @DefaultArrears)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", PortfolioId));
				Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
				Params.Add(new FbParameterItem("AddedById", ClientId));
                NoteToAdd = NoteToAdd.Replace("'", "`");
                NoteToAdd = NoteToAdd.Replace("\"", "`");
                Params.Add(new FbParameterItem("NoteToAdd", NoteToAdd));
				Params.Add(new FbParameterItem("DefaultArrears", DefaultArrears));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					foreach (DataRow dataRow in tbl.Rows) 
					{
						object obj = dataRow["OutputLine"];
						Entries.AppendLine((obj == DBNull.Value) ? "" : Convert.ToString(obj));
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string LoadAddPortfolioCommentRequestItem(ActionBase ActionBase,
		                                                          ref AddPortfolioCommentRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAddPortfolioCommentRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "CategoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "AddNote");
				TempItem.AddNote = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NoteToAdd");
				TempItem.NoteToAdd = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DefaultArrears");
				TempItem.DefaultArrears = (Obj == null) ? "" : Convert.ToString(Obj);
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string LoadDisplayCommentDlgGridReportRequestItem(ActionBase ActionBase,
		                                                                ref DisplayCommentDlgGridReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDisplayCommentDlgGridReportRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				/*
				GatherDebtsRequestItem CommonRequestItem = new GatherDebtsRequestItem();
				Result = LoadGatherDebtsRequestItem(ActionBase, ref CommonRequestItem);
				*/

				AddPortfolioCommentRequestItem CommonRequestItem = new AddPortfolioCommentRequestItem();
				Result = LoadAddPortfolioCommentRequestItem(ActionBase, ref CommonRequestItem);
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.PortfolioIdValue = CommonRequestItem.PortfolioIdValue;
					TempItem.CategoryOnlyIdValue = CommonRequestItem.CategoryOnlyIdValue;
					TempItem.AddNote = CommonRequestItem.AddNote;
					TempItem.NoteToAdd = CommonRequestItem.NoteToAdd;
					TempItem.DefaultArrears = CommonRequestItem.DefaultArrears;

					Object Obj = ActionBase.GetCollectionValue("Data", "passedId");
					TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "btnId");
					TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string LoadResBondsApplyChangesRequestItem(ActionBase ActionBase,
		                                                         ref ResBondsApplyChangesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadResBondsApplyChangesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "CategoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "LodgedAmount");
				TempItem.LodgedAmountValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "LodgementNumber");
				TempItem.LodgementNumber = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "LodgedOn");
				TempItem.LodgedOn = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "BondNumber");
				TempItem.BondNumber = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Guarantee");
				TempItem.Guarantee = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		public static string Portfolio_ResBondsApplyChanges(string ConnectionString,
		                                                    Int64 CompanyId,
		                                                    Int64 ClientId,
		                                                    int PortfolioId,
		                                                    int CategoryOnlyId,
		                                                    double LodgedAmount,
		                                                    string LodgementNumber,
		                                                    string LodgedOn,
		                                                    string BondNumber,
		                                                    string Guarantee)
		{
			string Result = "";
			string Method = "Portfolio_ResBondsApplyChanges";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result =
						Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioResBondsList(
							ConnectionString, CompanyId, ClientId, PortfolioId, ref Entries);
				}
				
				string Source = Entries.ToString();
				string UpdateSql1 = "";
				string UpdateSql2 = "";
				string Path = "";
				if (string.IsNullOrEmpty(Result))
				{
					Path = "$.updatesql1";
					UpdateSql1 = JsonUtilities.GetJsonPathValueStr(Source, Path, out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(UpdateSql1)) Result = "Cannot find the first Update entry";
					if (string.IsNullOrEmpty(Result))
					{
						Path = "$.updatesql2";
						UpdateSql2 = JsonUtilities.GetJsonPathValueStr(Source, Path, out Result);
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(UpdateSql2)) Result = "Cannot find the second Update entry";
					}					
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Clear();
					Params.Add(new FbParameterItem("LODGEDAMOUNT", LodgedAmount));
					Params.Add(new FbParameterItem("LODGEDNUMBER", LodgementNumber));
					Params.Add(new FbParameterItem("LODGEDDATE", LodgedOn));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql1, Params, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, UpdateSql1, Params, 0);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Params.Clear();
					Params.Add(new FbParameterItem("BONDNUMBER", BondNumber));
					Params.Add(new FbParameterItem("BONDGUARANTEE", Guarantee));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql2, Params, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, UpdateSql2, Params, 0);
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string LoadEAResComSalesGroupRequestItem(ActionBase ActionBase,
		                                                       ref EAResComSalesGroupRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadEAResComSalesGroupRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NotesId");
				TempItem.NotesIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Option1Id");
				TempItem.Option1IdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Option2Id");
				TempItem.Option2IdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string EAResComSalesGroup(string ConnectionString,
		                                        Int64 CompanyId,
		                                        Int64 ClientId,
		                                        int TransactionId,
		                                        int NotesId,
		                                        int Option1,
		                                        int Option2,
		                                        ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAResComSalesGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (Entries.Length > 0) Entries.Append(",");
				
				string QueryText = 
					"SELECT OutputLine FROM EAResComSalesGroup(@CompanyId, @TransactionId, @LoginId, @NotesId, @Option1, @Option2)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("NotesId", NotesId));
				Params.Add(new FbParameterItem("Option1", Option1));
				Params.Add(new FbParameterItem("Option2", Option2));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				object Obj = null;
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						if (Entries.Length > 0) Entries.Append(",");
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["OUTPUTLINE"];
							Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
						}
					}
				}
				
				string Str = "";
				string NoteSql = "";
				string Notes = "";
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Start - OwnerInstructions", Method, 0);
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					WindowsEventLoggingUtilities.WriteToInfoEvent("Before", Method, 0);
					NoteSql = JsonUtilities.GetJsonPathValueStr(Str, "$.NoteSQL", out Result);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After", Method, 0);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(NoteSql)) Result = "Cannot find the Notes entry";
					if (string.IsNullOrEmpty(Result))
					{
						NoteSql = NoteSql.Replace("__", "'");
						WindowsEventLoggingUtilities.WriteToInfoEvent("OwnerNoteSql:" + Environment.NewLine + NoteSql, Method, 0);
						tbl =
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, NoteSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
						if (string.IsNullOrEmpty(Result))
						{
							foreach (DataRow Row in tbl.Rows)
							{
								Obj = Row[0];
								if (Obj != DBNull.Value)
								{
									byte[] byteArray = (byte[])Obj;
									Notes = System.Text.Encoding.Default.GetString(byteArray);

            					}
							}
							if (Entries.Length > 0) Entries.Append(",");
							Entries.Append(JsonUtilities.AddPropertyNameAndValue("NotesText", Notes, false));
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EAResComSalesGroup Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string CopyRequestObject(EAResComSalesGroupRequestItem From,
		                                       ref EAResComSalesGroupUpdateNoteRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.NotesIdValue = From.NotesIdValue;
                To.Option1IdValue = From.Option1IdValue;
                To.Option2IdValue = From.Option2IdValue;
                To.PortfolioIdValue = From.PortfolioIdValue;
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string LoadEAResComSalesGroupUpdateNoteRequestItem(ActionBase ActionBase,
		                                                                 ref EAResComSalesGroupUpdateNoteRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadEAResComSalesGroupUpdateNoteRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				EAResComSalesGroupRequestItem Item = new EAResComSalesGroupRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = Item.Load(ActionBase);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.PortfolioIdValue = Item.PortfolioIdValue;
					TempItem.Option1IdValue = Item.Option1IdValue;
					TempItem.Option2IdValue = Item.Option2IdValue;
					TempItem.NotesIdValue = Item.NotesIdValue;
					
					Object Obj = ActionBase.GetCollectionValue("Data", "NoteText");
					TempItem.NoteText = (Obj == null) ? "" : Convert.ToString(Obj);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string GetNoteText(string ConnectionString,
		                                 int CompanyId,
		                                 int ClientId,
		                                 EAResComSalesGroupRequestItem Request,
		                                 ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "GetNoteText";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						Actions.Portfolio.CommonMethods.EAResComSalesGroup(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.NotesId, Request.Option1Id, Request.Option2Id, ref TempEntries);
							
				}
				
				string Str = "";
				string Notes = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Str =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					Notes = JsonUtilities.GetJsonPathValueStr(Str, "$.NotesText", out Result);
				}
			
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("NoteText:" + Environment.NewLine + Notes, GetModuleName(), 0);
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("NotesText", Notes, false));
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After GetNoteText Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string CopyRequestObject(EAResComSalesGroupUpdateNoteRequestItem From,
		                                       ref EAResComSalesGroupRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.NotesIdValue = From.NotesIdValue;
                To.Option1IdValue = From.Option1IdValue;
                To.Option2IdValue = From.Option2IdValue;
                To.PortfolioIdValue = From.PortfolioIdValue;
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string UpdateNote(string ConnectionString,
		                                int CompanyId,
		                                int ClientId,
		                                EAResComSalesGroupUpdateNoteRequestItem Request)
		{
			string Result = "";
			string Method = "UpdateNote";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				EAResComSalesGroupRequestItem Item = new EAResComSalesGroupRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = CopyRequestObject(Request, ref Item);
				}
				
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						Actions.Portfolio.CommonMethods.EAResComSalesGroup(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.NotesId, Request.Option1Id, Request.Option2Id, ref TempEntries);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateNoteSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update note query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveNoteText(ConnectionString, QueryText, Request.NoteText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateNote Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string SaveNoteText(string ConnectionString,
		                                  string QueryText,
		                                  string NoteText)
		{
			string Result = "";
			string Method = "SaveNoteText";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					byte[] bytes = Encoding.ASCII.GetBytes(NoteText);
					Params.Add(new FbParameterItem("notepad", bytes));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
	}
}
