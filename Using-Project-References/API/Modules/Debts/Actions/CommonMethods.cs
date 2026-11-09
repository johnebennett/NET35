/*
 * Created by SharpDevelop.
 * User: John
 * Date: 21/03/2020
 * Time: 4:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Debts
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string LoadGatherDebtsRequestItem(ActionBase ActionBase,
		                                                ref GatherDebtsRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGatherDebtsRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "OfficeId");
				TempItem.OfficeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReportsId");
				TempItem.ReportsIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ManagerId");
				TempItem.ManagerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PastEventsId");
				TempItem.PastEventsIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankTypesId");
				TempItem.BankTypesIdValue = (Obj == null) ? "" : Convert.ToString(Obj);				
				Obj = ActionBase.GetCollectionValue("Data", "LedgerTypeId");
				TempItem.LedgerTypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "LedgerId");
				TempItem.LedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "TransactionTypeId");
				TempItem.TransactionTypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "TransactionsId");
				TempItem.TransactionsIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "PrintGroupId");
				TempItem.PrintGroupIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ChargeOptionsId");
				TempItem.ChargeOptionsIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "RangeFrom");
				TempItem.RangeFromValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "RangeTo");
				TempItem.RangeToValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "MasterExclusionId");
				TempItem.MasterExclusionIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string Debts_GatherDebts(string ConnectionString,
		                                       Int64 CompanyId,
		                                       Int64 ClientId,
		                                       int MasterExclusionId,
		                                       int BranchId,
		                                       int ManagerId,
		                                       int LedgerId,
		                                       int LedgerTypeId,
		                                       int TransactionId,
		                                       int DaysFrom,
		                                       int DaysTo,
		                                       int ChargeOption,
		                                       int BankNameId,
		                                       int PrintGroupId,
		                                       int ReportId,
		                                       int TransactionTypeId,
		                                       int PastDebitId,
		                                       ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "Debts_GatherDebts";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText =
					" SELECT OUTPUTLINE FROM EADebtsGroup(@CompanyId, @BranchId, @ManagerId, @LedgerId, @SubLedgerId, @LedgerTypeId," +
					" @TransactionId, @DaysFrom, @DaysTo, @ChargeOption, @Options, @BankNameId, @RevenueId, @LoginId, @ReportId," +
					" @MasterExclusionId, @TransactionTypeId, @PastDebitEventId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("BranchId", BranchId));
				Params.Add(new FbParameterItem("ManagerId", ManagerId));
				Params.Add(new FbParameterItem("LedgerId", LedgerId));
				Params.Add(new FbParameterItem("SubLedgerId", 0));
				Params.Add(new FbParameterItem("LedgerTypeId", LedgerTypeId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("DaysFrom", DaysFrom));
				Params.Add(new FbParameterItem("DaysTo", DaysTo));
				Params.Add(new FbParameterItem("ChargeOption", ChargeOption));
				Params.Add(new FbParameterItem("Options", 0));
				Params.Add(new FbParameterItem("BankNameId", BankNameId));
				Params.Add(new FbParameterItem("RevenueId", PrintGroupId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("ReportId", ReportId));
				Params.Add(new FbParameterItem("MasterExclusionId", MasterExclusionId));
				Params.Add(new FbParameterItem("TransactionTypeId", TransactionTypeId));
				Params.Add(new FbParameterItem("PastDebitEventId", PastDebitId));
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
					foreach (DataRow Row in tbl.Rows)
					{
						Object Obj = Row["OUTPUTLINE"];
						Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Entries.Append(",");
					Result = Debts_GatherDebts_GridEntries(ConnectionString, ref Entries);
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
		
		public static string Debts_GatherDebts_GridEntries(string ConnectionString,
		                                                   ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "Debts_GatherDebts_GridEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				// Get the sqlstatement element from the Entries list
				// clean up the query text
				string QueryText = "";
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
							Str = (string)jobj["sqlstatement"];
							Str = Str.Replace("__", "'");
							Str = Str.Replace("@:", "@");
							Str = Str.Replace("'@", "@");
							Str = Str.Replace("@'", "@");
							QueryText = Str;
						}
					}
				}
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, Method, 0);
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = CommonUtilities.JsonUtilities.BuildJsonArrayFromDataTable(tbl, "gridEntries", ref Entries);
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
		
		public static string LoadDebts_ShowReportItemRequestItem(ActionBase ActionBase,
		                                                         ref Debts_ShowReportItemRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDebts_ShowReportItemRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				GatherDebtsRequestItem CommonRequestItem = new GatherDebtsRequestItem();
				Result = LoadGatherDebtsRequestItem(ActionBase, ref CommonRequestItem);
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.OfficeIdValue = CommonRequestItem.OfficeIdValue;
					TempItem.ReportsIdValue = CommonRequestItem.ReportsIdValue;
					TempItem.ManagerIdValue = CommonRequestItem.ManagerIdValue;
					TempItem.PastEventsIdValue = CommonRequestItem.PastEventsIdValue;
					TempItem.BankTypesIdValue = CommonRequestItem.BankTypesIdValue;
					TempItem.LedgerTypeIdValue = CommonRequestItem.LedgerTypeIdValue;
					TempItem.LedgerIdValue = CommonRequestItem.LedgerIdValue;
					TempItem.TransactionTypeIdValue = CommonRequestItem.TransactionTypeIdValue;
					TempItem.TransactionsIdValue = CommonRequestItem.TransactionsIdValue;
					TempItem.PrintGroupIdValue = CommonRequestItem.PrintGroupIdValue;
					TempItem.ChargeOptionsIdValue = CommonRequestItem.ChargeOptionsIdValue;
					TempItem.RangeFromValue = CommonRequestItem.RangeFromValue;
					TempItem.RangeToValue = CommonRequestItem.RangeToValue;
					TempItem.MasterExclusionIdValue = CommonRequestItem.MasterExclusionIdValue;
					Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
					TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ButtonId");
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
		
		public static string LoadDebts_UndoRedoRequestItem(ActionBase ActionBase,
		                                                   ref Debts_UndoRedoRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDebts_UndoRedoRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				GatherDebtsRequestItem CommonRequestItem = new GatherDebtsRequestItem();
				Result = LoadGatherDebtsRequestItem(ActionBase, ref CommonRequestItem);
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.OfficeIdValue = CommonRequestItem.OfficeIdValue;
					TempItem.ReportsIdValue = CommonRequestItem.ReportsIdValue;
					TempItem.ManagerIdValue = CommonRequestItem.ManagerIdValue;
					TempItem.PastEventsIdValue = CommonRequestItem.PastEventsIdValue;
					TempItem.BankTypesIdValue = CommonRequestItem.BankTypesIdValue;
					TempItem.LedgerTypeIdValue = CommonRequestItem.LedgerTypeIdValue;
					TempItem.LedgerIdValue = CommonRequestItem.LedgerIdValue;
					TempItem.TransactionTypeIdValue = CommonRequestItem.TransactionTypeIdValue;
					TempItem.TransactionsIdValue = CommonRequestItem.TransactionsIdValue;
					TempItem.PrintGroupIdValue = CommonRequestItem.PrintGroupIdValue;
					TempItem.ChargeOptionsIdValue = CommonRequestItem.ChargeOptionsIdValue;
					TempItem.RangeFromValue = CommonRequestItem.RangeFromValue;
					TempItem.RangeToValue = CommonRequestItem.RangeToValue;
					TempItem.MasterExclusionIdValue = CommonRequestItem.MasterExclusionIdValue;
					Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
					TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Status");
					TempItem.Status = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string LoadDebts_ReportActionRequestItem(ActionBase ActionBase,
		                                                       ref Debts_ReportActionRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDebts_ReportActionRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				GatherDebtsRequestItem CommonRequestItem = new GatherDebtsRequestItem();
				Result = LoadGatherDebtsRequestItem(ActionBase, ref CommonRequestItem);
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.OfficeIdValue = CommonRequestItem.OfficeIdValue;
					TempItem.ReportsIdValue = CommonRequestItem.ReportsIdValue;
					TempItem.ManagerIdValue = CommonRequestItem.ManagerIdValue;
					TempItem.PastEventsIdValue = CommonRequestItem.PastEventsIdValue;
					TempItem.BankTypesIdValue = CommonRequestItem.BankTypesIdValue;
					TempItem.LedgerTypeIdValue = CommonRequestItem.LedgerTypeIdValue;
					TempItem.LedgerIdValue = CommonRequestItem.LedgerIdValue;
					TempItem.TransactionTypeIdValue = CommonRequestItem.TransactionTypeIdValue;
					TempItem.TransactionsIdValue = CommonRequestItem.TransactionsIdValue;
					TempItem.PrintGroupIdValue = CommonRequestItem.PrintGroupIdValue;
					TempItem.ChargeOptionsIdValue = CommonRequestItem.ChargeOptionsIdValue;
					TempItem.RangeFromValue = CommonRequestItem.RangeFromValue;
					TempItem.RangeToValue = CommonRequestItem.RangeToValue;
					TempItem.MasterExclusionIdValue = CommonRequestItem.MasterExclusionIdValue;
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
		
		public static string LoadDebts_PastEventsActionRequestItem(ActionBase ActionBase,
		                                                           ref Debts_PastEventsActionRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDebts_PastEventsActionRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				GatherDebtsRequestItem CommonRequestItem = new GatherDebtsRequestItem();
				Result = LoadGatherDebtsRequestItem(ActionBase, ref CommonRequestItem);
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.OfficeIdValue = CommonRequestItem.OfficeIdValue;
					TempItem.ReportsIdValue = CommonRequestItem.ReportsIdValue;
					TempItem.ManagerIdValue = CommonRequestItem.ManagerIdValue;
					TempItem.PastEventsIdValue = CommonRequestItem.PastEventsIdValue;
					TempItem.BankTypesIdValue = CommonRequestItem.BankTypesIdValue;
					TempItem.LedgerTypeIdValue = CommonRequestItem.LedgerTypeIdValue;
					TempItem.LedgerIdValue = CommonRequestItem.LedgerIdValue;
					TempItem.TransactionTypeIdValue = CommonRequestItem.TransactionTypeIdValue;
					TempItem.TransactionsIdValue = CommonRequestItem.TransactionsIdValue;
					TempItem.PrintGroupIdValue = CommonRequestItem.PrintGroupIdValue;
					TempItem.ChargeOptionsIdValue = CommonRequestItem.ChargeOptionsIdValue;
					TempItem.RangeFromValue = CommonRequestItem.RangeFromValue;
					TempItem.RangeToValue = CommonRequestItem.RangeToValue;
					TempItem.MasterExclusionIdValue = CommonRequestItem.MasterExclusionIdValue;
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
		
		public static string LoadDebts_UndoRedoNonGridRequestItem(ActionBase ActionBase,
		                                                          ref Debts_UndoRedoNonGridRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDebts_UndoRedoNonGridRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				GatherDebtsRequestItem CommonRequestItem = new GatherDebtsRequestItem();
				Result = LoadGatherDebtsRequestItem(ActionBase, ref CommonRequestItem);
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.OfficeIdValue = CommonRequestItem.OfficeIdValue;
					TempItem.ReportsIdValue = CommonRequestItem.ReportsIdValue;
					TempItem.ManagerIdValue = CommonRequestItem.ManagerIdValue;
					TempItem.PastEventsIdValue = CommonRequestItem.PastEventsIdValue;
					TempItem.BankTypesIdValue = CommonRequestItem.BankTypesIdValue;
					TempItem.LedgerTypeIdValue = CommonRequestItem.LedgerTypeIdValue;
					TempItem.LedgerIdValue = CommonRequestItem.LedgerIdValue;
					TempItem.TransactionTypeIdValue = CommonRequestItem.TransactionTypeIdValue;
					TempItem.TransactionsIdValue = CommonRequestItem.TransactionsIdValue;
					TempItem.PrintGroupIdValue = CommonRequestItem.PrintGroupIdValue;
					TempItem.ChargeOptionsIdValue = CommonRequestItem.ChargeOptionsIdValue;
					TempItem.RangeFromValue = CommonRequestItem.RangeFromValue;
					TempItem.RangeToValue = CommonRequestItem.RangeToValue;
					TempItem.MasterExclusionIdValue = CommonRequestItem.MasterExclusionIdValue;
					Object Obj = ActionBase.GetCollectionValue("Data", "Source");
					TempItem.SourceValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string ExecuteSqlInsertScript(string ConnectionString,
		                                            Int64 CompanyId,
		                                            Int64 ClientId,
		                                            int MasterExclusionId,
		                                            int NewDevId,
		                                            int BranchId,
		                                            int ManagerId,
		                                            int LedgerId,
		                                            int LedgerTypeId,
		                                            int TransactionId,
		                                            int DaysFrom,
		                                            int DaysTo,		                                            
		                                            int ChargeOption,
		                                            int BankNameId,
		                                            int PrintGroupId,
		                                            int ReportId,
		                                            int TransactionTypeId,
		                                            int PastDebitId)
		{
			string Result = "";
			string Method = "ExecuteSqlInsertScript";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				/*
				int NewId =
					UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
						ConnectionString, "I_DEV_ID_GEN", (int)CompanyId, (int)ClientId, out Result);
				*/
				
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						Debts_GatherDebts(
							ConnectionString, CompanyId, ClientId,
							MasterExclusionId, BranchId, ManagerId, LedgerId,
							LedgerTypeId, TransactionId, DaysFrom, DaysTo,
							ChargeOption, BankNameId, PrintGroupId, ReportId,
							TransactionTypeId, PastDebitId, ref Entries);
				}
				
				string Str = "";
				
				if (string.IsNullOrEmpty(Result))
				{
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				}
				JObject JObj = null;
				if (Entries.Length > 0)
				{
					JObj = JObject.Parse(Str);
				}

				string QueryText = "";
				if (string.IsNullOrEmpty(Result) && (JObj != null))
				{
					QueryText = (string)JObj["sqlinsert"];
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + Environment.NewLine + QueryText, Method, 0);
					if (string.IsNullOrEmpty(QueryText)) Result = "Cannot determine the QueryText in " + Method;
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(QueryText))
				{
					// replace the parameter in the udpate script
					//Params.Add(new FbParameterItem("NoteText", FRequestItem.NoteToAdd));
					QueryText = QueryText.Replace("__", "'");
					QueryText = QueryText.Replace("~~", "'");
					QueryText = QueryText.Replace("XXXXX", NewDevId.ToString());
					WindowsEventLoggingUtilities.WriteToInfoEvent("Adjusted QueryText: " + Environment.NewLine + QueryText, Method, 0);
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, null, 0);
					Str = (Result == "") ? "No Issue" : Result;
					WindowsEventLoggingUtilities.WriteToInfoEvent("Execution Result: " + Environment.NewLine + Str, Method, 0);
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
		
		public static string ExecuteSqlInsertScript(string ConnectionString,
		                                            Int64 CompanyId,
		                                            Int64 ClientId,
		                                            int NewDevId,
		                                            StringBuilder Entries)
		{
			string Result = "";
			string Method = "ExecuteSqlInsertScript";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				/*
				int NewId =
					UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
						ConnectionString, "I_DEV_ID_GEN", (int)CompanyId, (int)ClientId, out Result);
				*/
				
				string Str = "";				
				if (string.IsNullOrEmpty(Result))
				{
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				}
				
				JObject JObj = null;
				if (Entries.Length > 0)
				{
					JObj = JObject.Parse(Str);
				}

				string QueryText = "";
				if (string.IsNullOrEmpty(Result) && (JObj != null))
				{
					QueryText = (string)JObj["sqlinsert"];
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + Environment.NewLine + QueryText, Method, 0);
					if (string.IsNullOrEmpty(QueryText)) Result = "Cannot determine the QueryText in " + Method;
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(QueryText))
				{
					// replace the parameter in the udpate script
					//Params.Add(new FbParameterItem("NoteText", FRequestItem.NoteToAdd));
					//QueryText = QueryText.Replace("__", "'");
					QueryText = QueryText.Replace("~~", "'");
					QueryText = QueryText.Replace("XXXXX", NewDevId.ToString());
					WindowsEventLoggingUtilities.WriteToInfoEvent("Adjusted QueryText: " + Environment.NewLine + QueryText, Method, 0);
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, null, 0);
					Str = (Result == "") ? "No Issue" : Result;
					WindowsEventLoggingUtilities.WriteToInfoEvent("Execution Result: " + Environment.NewLine + Str, Method, 0);
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
		
		public static int GetBulkEmailRunId(string ConnectionString,
		                                    Int64 CompanyId, 
		                                    Int64 ClientId,
		                                    int NewDevId,
		                                    StringBuilder Entries, 
		                                    out string ErrorMsg)
		{
			int Result = 0;
			ErrorMsg = "";
			string Method = "GetBulkEmailRunId";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string Str = "";				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				}
				
				JObject JObj = null;
				if (Entries.Length > 0)
				{
					JObj = JObject.Parse(Str);
				}

				string QueryText = "";
				if (string.IsNullOrEmpty(ErrorMsg) && (JObj != null))
				{
					QueryText = (string)JObj["sqltogetstatementrunid"];
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + Environment.NewLine + QueryText, Method, 0);
					if (string.IsNullOrEmpty(QueryText)) ErrorMsg = "Cannot determine the QueryText in " + Method;
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				DataTable tbl = null;
				object Obj = null;
				if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(QueryText))
				{
					// replace the parameter in the udpate script
					//Params.Add(new FbParameterItem("NoteText", FRequestItem.NoteToAdd));
					QueryText = QueryText.Replace("XXXXX", NewDevId.ToString());
					QueryText = QueryText.Replace(":devid", NewDevId.ToString());
					//QueryText = QueryText.Replace("__", "'");
					WindowsEventLoggingUtilities.WriteToInfoEvent("Adjusted QueryText: " + Environment.NewLine + QueryText, Method, 0);
					tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
					Str = (ErrorMsg == "") ? "No Issue" : ErrorMsg;
					WindowsEventLoggingUtilities.WriteToInfoEvent("Execution Result: " + Environment.NewLine + Str, Method, 0);
					foreach (DataRow row in tbl.Rows)
					{
						Obj = row["I_ASTR_LEDGERBULKID"];
						Result = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
					}
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
	}
}
