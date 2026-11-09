/*
 * Created by SharpDevelop.
 * User: John
 * Date: 21/04/2020
 * Time: 12:26 PM
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
using UsefulDBMethods;
namespace Actions.Receipts
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public static string GetPrefix()
		{
			return "Receipts.";
		}

        public static string GetModuleName() { return "Actions.Receipts."; }

        public static string DeleteUserRun(string ConnectionString,
		                                   Int64 CompanyId,
		                                   int AddedById)
		{
			string Result = "";
			string Method = "DeleteUserRun";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "DELETE FROM EAReceiptDirectTemp WHERE COMPANYCLIENTID = @CompanyId AND ADDEDBYID = @AddedById";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("AddedById", AddedById));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
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
		
		
		public static string EAReceiptScreenEventAdd(string ConnectionString,
		                                             Int64 CompanyId,
		                                             Int64 MasterEventId,
		                                             int BranchId,
		                                             int ManagerId,
		                                             int BankNameId,
		                                             int LedgerType,
		                                             int LoginId,
		                                             int FromNumber,
		                                             int ToNumber,
		                                             string FromReceiptDate,
		                                             string ToReceiptDate,
		                                             string FromEnterDate,
		                                             string ToEnterDate,
		                                             int AddedById)
		{
			string Result = "";
			string Method = "EAReceiptScreenEventAdd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" EXECUTE PROCEDURE EAReceiptScreenEventAdd(" +
					"   @CompanyId, @MasterEventId, @BranchId, @ManagerId, @BankNameId, @LedgerType, @LoginId," +
					"   @FromNumber, @ToNumber, @FromReceiptDate, @ToReceiptDate," +
					"   @FromEnterDate, @ToEnterDate, @AddedById)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("MasterEventId", MasterEventId));
				Params.Add(new FbParameterItem("BranchId", BranchId));
				Params.Add(new FbParameterItem("ManagerId", ManagerId));
				Params.Add(new FbParameterItem("BankNameId", BankNameId));
				Params.Add(new FbParameterItem("LedgerType", LedgerType));
				Params.Add(new FbParameterItem("LoginId", LoginId));
				Params.Add(new FbParameterItem("FromNumber", FromNumber));
				Params.Add(new FbParameterItem("ToNumber", ToNumber));
				Params.Add(new FbParameterItem("FromReceiptDate", FromReceiptDate));
				Params.Add(new FbParameterItem("ToReceiptDate", ToReceiptDate));
				Params.Add(new FbParameterItem("FromEnterDate", FromEnterDate));
				Params.Add(new FbParameterItem("ToEnterDate", ToEnterDate));
				Params.Add(new FbParameterItem("AddedById", AddedById));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
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
		
		public static string LoadGatherReceiptsRequestItem(ActionBase ActionBase,
		                                                ref GatherReceiptsRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGatherReceiptsRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PastEventId");
				TempItem.PastEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptOptionsId");
				TempItem.ReceiptOptionsIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string EAReceiptScreenGroup2(string ConnectionString,
		                                           Int64 CompanyId,
		                                           int ReceiptEventId,
		                                           int ClientId,
		                                           int PastEventId,
		                                           int SecondOption,
		                                           ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAReceiptScreenGroup2";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT OutputLine FROM EAReceiptScreenGroup2(@CompanyId, @ReceiptEventId, @LoginId, @PastEventId, @SecondOption)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("ReceiptEventId", ReceiptEventId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("PastEventId", PastEventId));
				Params.Add(new FbParameterItem("SecondOption", SecondOption));
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
					//string Path = "$.SystemSettings.PrintGroups.sqlprintgroupremove";
					string Path = "$.SQLStatement";
					QueryText = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the SqlStatement entry";
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						QueryText = QueryText.Replace("@", "'");
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (!string.IsNullOrEmpty(QueryText))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, Method, 0);
						tbl =
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, QueryText, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					}
					if (string.IsNullOrEmpty(Result))
					{
						string Value = "";
						foreach (DataRow Row in tbl.Rows)
						{
							Object Obj = Row[0];
							Value = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
							if (!string.IsNullOrEmpty(Value)) QueryText = Value;
						}
						
						if (!string.IsNullOrEmpty(QueryText)) 
						{
							QueryText = QueryText.Replace("\"", "'");
							WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, Method, 0);
							tbl =
								UsefulDBMethods.Helpers.GetFirstDataTableEx(
									ConnectionString, QueryText, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
							if (string.IsNullOrEmpty(Result))
							{
								Entries.Append(",");
								Result = JsonUtilities.BuildJsonArrayFromDataTable(tbl, "GridEntries", ref Entries);
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
		
		public static string LoadCreateBankingDlgRequestItem(ActionBase ActionBase,
		                                                     ref CreateOptions.CreateBankingDlgRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadCreateBankingDlgRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "CategoryOnlyId");
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
		
		public static string EAReceiptScreenChangeEvent(string ConnectionString,
		                                                Int64 CompanyId,
		                                                int MasterEventId,
		                                                int ComboNumberId,
		                                                int ComboId)
		{
			string Result = "";
			string Method = "EAReceiptScreenChangeEvent";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "EXECUTE PROCEDURE EAReceiptScreenChangeEvent(@CompanyId, @MasterEventId, @ComboNumberId, @ComboId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("MasterEventId", MasterEventId));
				Params.Add(new FbParameterItem("ComboNumberId", ComboNumberId));
				Params.Add(new FbParameterItem("ComboId", ComboId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
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
		
		public static string EAReceiptScreenGroupAddBanking(string ConnectionString,
		                                                    Int64 CompanyId,
		                                                    int MasterEventId,
		                                                    int AddedById,
		                                                    int CategoryOnlyId,
		                                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAReceiptScreenGroupAddBanking";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				string QueryText = "SELECT OutputLine FROM EAReceiptScreenGroupAddBanking(@CompanyId, @MasterEventId, @AddedById, @CategoryOnlyId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("MasterEventId", MasterEventId));
				Params.Add(new FbParameterItem("AddedById", AddedById));
				Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result) && (tbl != null))
				{
					foreach(DataRow Row in tbl.Rows)
					{
						Object Obj = Row["OUTPUTLINE"];
						Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
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
		
		public static string LoadAddBankingRequestItem(ActionBase ActionBase,
		                                                     ref CreateOptions.AddBankingRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAddBankingRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "CategoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Description");
				TempItem.Description = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EABankImportFileScreenGroup(string ConnectionString,
		                                                 Int64 CompanyId,
		                                                 Int64 MasterEventId,
		                                                 int LoginId,
		                                                 int CategoryDefaultId,
		                                                 string FileName,		                                                 
		                                                 int BankNameId,
		                                                 int StartingReceiptNumber,
		                                                 int ReceiptSetId,
		                                                 int PageNumber,
		                                                 string BankStatementDate,
		                                                 ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EABankImportFileScreenGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT OUTPUTLINE FROM EABankImportFileScreenGroup(" +
					"   @CompanyId, @MasterEventId, @LoginId, @CategoryDefaultId, @FileName, @BankNameId," +
					"   @StartingReceiptNumber, @ReceiptSetId, @PageNumber, @BankStatementDate)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("MasterEventId", MasterEventId));
				Params.Add(new FbParameterItem("LoginId", LoginId));
				Params.Add(new FbParameterItem("CategoryDefaultId", CategoryDefaultId));
				Params.Add(new FbParameterItem("FileName", FileName));
				Params.Add(new FbParameterItem("BankNameId", BankNameId));
				Params.Add(new FbParameterItem("StartingReceiptNumber", StartingReceiptNumber));
				Params.Add(new FbParameterItem("ReceiptSetId", ReceiptSetId));
				Params.Add(new FbParameterItem("PageNumber", PageNumber));
				Params.Add(new FbParameterItem("BankStatementDate", BankStatementDate));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result) && (tbl != null))
				{
					foreach(DataRow Row in tbl.Rows)
					{
						Object Obj = Row["OUTPUTLINE"];
						Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
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
		
		public static string LoadImportBankingFileDlgRequestItem(ActionBase ActionBase,
		                                                         ref CreateOptions.ImportBankingFileDlgRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadImportBankingFileDlgRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankImportTypeId");
				TempItem.BankImportTypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

				Obj = ActionBase.GetCollectionValue("Data", "BankNameId");
				TempItem.BankNameIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptSetId");
				TempItem.ReceiptSetIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "StartingReceiptNumber");
				TempItem.StartingReceiptNumberValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PageNumber");
				TempItem.PageNumberValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankStmtDate");
				TempItem.BankStmtDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FileName");
				TempItem.FileName = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string LoadImportBankingFileRequestItem(ActionBase ActionBase,
		                                                      ref CreateOptions.ImportBankingFileRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadImportBankingFileDlgRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankImportTypeId");
				TempItem.BankImportTypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

				Obj = ActionBase.GetCollectionValue("Data", "BankNameId");
				TempItem.BankNameIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptSetId");
				TempItem.ReceiptSetIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "StartingReceiptNumber");
				TempItem.StartingReceiptNumberValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PageNumber");
				TempItem.PageNumberValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankStmtDate");
				TempItem.BankStmtDate = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EABankRec(string ConnectionString,
		                               Int64 CompanyId,
		                               int BankImportId)
		{
			string Result = "";
			string Method = "EABankRec";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "EXECUTE PROCEDURE EABankRec(@CompanyId, @BankImportId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("BankImportId", BankImportId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
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
		
		public static string LoadCreateDirectDebitListRequestItem(ActionBase ActionBase,
		                                                      ref CreateOptions.CreateDirectDebitListRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadCreateDirectDebitListRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankNameId");
				TempItem.BankNameIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

				Obj = ActionBase.GetCollectionValue("Data", "ManagerId");
				TempItem.ManagerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "TransactionType");
				TempItem.TransactionTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DebitTypeId");
				TempItem.DebitTypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "MultiBankFile");
				TempItem.MultiBankFile = (Obj == null) ? "" : Convert.ToString(Obj);				
				Obj = ActionBase.GetCollectionValue("Data", "DebitRunCode1");
				TempItem.DebitRunCode1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DebitRunCode2");
				TempItem.DebitRunCode2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DebitRunCode3");
				TempItem.DebitRunCode3 = (Obj == null) ? "" : Convert.ToString(Obj);

				Obj = ActionBase.GetCollectionValue("Data", "ClearanceDays");
				TempItem.ClearanceDaysValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DueFrom");
				TempItem.DueFrom = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DueTo");
				TempItem.DueTo = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ExcludeVacating");
				TempItem.ExcludeVacating = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "VacatingFrom");
				TempItem.VacatingFrom = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "VacatingTo");
				TempItem.VacatingTo = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DirectDebitId");
				TempItem.DirectDebitIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BranchId");
				TempItem.BranchIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "NextReceiptNumber");
                TempItem.NextReceiptNumberValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EADirectDebitOKToCreate(string ConnectionString,
		                                             Int64 CompanyId,
		                                             Int64 ClientId,
		                                             int MasterEventId,
		                                             int DirectDebitId,
		                                             ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EADirectDebitOKToCreate";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					"SELECT Status, DirectDebitId, DirectDebitSql, DirectDebitGroupSql FROM EADIRECTDEBITOKTOCREATE(" +
					"@CompanyId, @ClientId, @MasterEventId, @DirectDebitId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("ClientId", ClientId));
				Params.Add(new FbParameterItem("MasterEventId", MasterEventId));
				Params.Add(new FbParameterItem("DirectDebitId", DirectDebitId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
				}
				
				object Obj = null;
				string Status = "";
				string DirectDebitGroupSql = "";
				if (string.IsNullOrEmpty(Result) && (tbl != null))
				{
					Obj = tbl.Rows[0]["Status"];
					Status = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = tbl.Rows[0]["DirectDebitGroupSql"];
					DirectDebitGroupSql = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
				
				tbl = null;
				if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(DirectDebitGroupSql))
				{
					DirectDebitGroupSql = DirectDebitGroupSql.Replace("\"", "'");
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, DirectDebitGroupSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);					
					if (string.IsNullOrEmpty(Result) && (tbl != null))
					{
						foreach(DataRow Row in tbl.Rows)
						{
							Obj = Row["OUTPUTLINE"];
							Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
						}
					}
				}
				
				if (Entries.Length > 0) Entries.Append(",");
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("Status", Status, false));
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
		
		public static string EADirectDebitOKToCreateWithDirectDebitSql(string ConnectionString,
		                                                               Int64 CompanyId,
		                                                               Int64 ClientId,
		                                                               int MasterEventId,
		                                                               int DirectDebitId,
		                                                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EADirectDebitOKToCreate";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					"SELECT Status, DirectDebitId, DirectDebitSql, DirectDebitGroupSql FROM EADIRECTDEBITOKTOCREATE(" +
					"@CompanyId, @ClientId, @MasterEventId, @DirectDebitId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("ClientId", ClientId));
				Params.Add(new FbParameterItem("MasterEventId", MasterEventId));
				Params.Add(new FbParameterItem("DirectDebitId", DirectDebitId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
				}
				
				object Obj = null;
				string Status = "";
				string DirectDebitSql = "";
				string DirectDebitGroupSql = "";
				if (string.IsNullOrEmpty(Result) && (tbl != null))
				{
					Obj = tbl.Rows[0]["Status"];
					Status = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = tbl.Rows[0]["DirectDebitGroupSql"];
					DirectDebitGroupSql = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = tbl.Rows[0]["DirectDebitSql"];
					DirectDebitSql = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
				
				tbl = null;
				if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(DirectDebitGroupSql))
				{
					DirectDebitGroupSql = DirectDebitGroupSql.Replace("\"", "'");
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, DirectDebitGroupSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);					
					if (string.IsNullOrEmpty(Result) && (tbl != null))
					{
						foreach(DataRow Row in tbl.Rows)
						{
							Obj = Row["OUTPUTLINE"];
							Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
						}
					}
				}
				
				if (Entries.Length > 0) Entries.Append(",");
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("DirectDebitSql", DirectDebitSql));
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("Status", Status, false));
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
		
		public static string DirectDebitList_ProcessUpdateSql(string UpdateSql,
		                                                      string ConnectionString,
		                                                      CreateOptions.CreateDirectDebitListRequestItem RequestItem,
		                                                      Int64 ClientId)
		{
			string Result = "";
			string Method = "DirectDebitList_ProcessUpdateSql";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = UpdateSql;
				//Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("ManagerId", RequestItem.ManagerId));
				Params.Add(new FbParameterItem("BranchId", RequestItem.BranchId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("BankId", RequestItem.BankNameId));
				Params.Add(new FbParameterItem("MultiBankFile", RequestItem.MultiBankFile));
				Params.Add(new FbParameterItem("TransactionTypeId", RequestItem.TransactionType));
				Params.Add(new FbParameterItem("DebitRunCode1", RequestItem.DebitRunCode1));
				Params.Add(new FbParameterItem("DebitRunCode2", RequestItem.DebitRunCode2));
				Params.Add(new FbParameterItem("DebitRunCode3", RequestItem.DebitRunCode3));
				Params.Add(new FbParameterItem("DebitCollection", RequestItem.DebitTypeId));
				Params.Add(new FbParameterItem("ClearanceDays", RequestItem.ClearanceDays));
				Params.Add(new FbParameterItem("DueFrom", RequestItem.DueFrom));
				Params.Add(new FbParameterItem("DueTo", RequestItem.DueTo));
				Params.Add(new FbParameterItem("ExcludingVacating", RequestItem.ExcludeVacating));
				Params.Add(new FbParameterItem("VacateFrom", RequestItem.VacatingFrom));
				Params.Add(new FbParameterItem("VacateTo", RequestItem.VacatingTo));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
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

        public static string DirectDebitList_ProcessUpdateSql2(string UpdateSql,
                                                               string ConnectionString,
                                                               CreateOptions.CreateDirectDebitListRequestItem RequestItem,
                                                               Int64 ClientId)
        {
            string Result = "";
            string Method = "DirectDebitList_ProcessUpdateSql2";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = UpdateSql;
                //Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("NextReceiptNumber", RequestItem.NextReceiptNumber));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

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

        public static string DirectDebitList_ProcessDirectDebitSql(string ConnectionString,
		                                                           string DirectDebitSql)
		{
			string Result = "";
			string Method = "DirectDebitList_ProcessDirectDebitSql";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = DirectDebitSql.Replace("@", "'");
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
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
		
		public static string LoadEventProcessOptions_OnChangeRequestItem(ActionBase ActionBase,
		                                                                 ref EventProcessOptions.EventProcessOptions_OnChangeRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadEventProcessOptions_OnChangeRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PastEventId");
				TempItem.PastEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ItemId");
				TempItem.ItemIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string LoadReceiptScreenChargePopupRequestItem(ActionBase ActionBase,
		                                                             ref ReceiptScreenChargePopupRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "ID");
				TempItem.IDValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PayingAmount");
				TempItem.PayingAmountValue= (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAReceiptScreenChargePopup(string ConnectionString,
		                                                int ID, 
		                                                double PayingAmount,
		                                                ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAReceiptScreenChargePopup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT OutputLine FROM EAReceiptScreenChargePopup(@ID, @PayingAmount)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("ID", ID));
				Params.Add(new FbParameterItem("PayingAmount", PayingAmount));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl == null)) Result = "Error calling query in " + Method;
				}
				
				object Obj = null;
				if (string.IsNullOrEmpty(Result) && (tbl != null))
				{
					foreach(DataRow Row in tbl.Rows)
					{
						Obj = Row["OutputLine"];
						Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
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
		
		public static string FindScreenForReceiptType(string ConnectionString,
		                                              Int64 CompanyId,
		                                              int TypeId,
		                                              int ReceiptType,
		                                              string WhatToFind,
		                                              string IncludeInactives,
		                                              ref StringBuilder Entries)
		{
			string Result = "";
			string Method = GetPrefix() + "FindScreenForReceiptType";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT SqlStatement FROM EAFindScreenSql(@CompanyId, @TypeId, @TableToSearch, @WhatToFind, @IncludeInactives)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TypeId", TypeId));
				string TableToSearch = "TRANSACTION";
				if (ReceiptType == 1) TableToSearch = "LEDGER";
				Params.Add(new FbParameterItem("TableToSearch", TableToSearch));
				Params.Add(new FbParameterItem("WhatToFind", WhatToFind));
				Params.Add(new FbParameterItem("IncludeInactives", IncludeInactives));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl == null)) Result = "Error calling query in " + Method;
				}
				
				QueryText = "";
				object Obj = null;
				if (string.IsNullOrEmpty(Result))
				{
					foreach(DataRow Row in tbl.Rows)
					{
						Obj = Row["SqlStatement"];
						QueryText = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
					if (string.IsNullOrEmpty(QueryText)) Result = "Cannot determine the QueryText in " + Method;
				}
				
				tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl == null)) Result = "Error calling the second query in " + Method;
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Result = JsonUtilities.BuildJsonArrayFromDataTable(tbl, "GridEntries", ref Entries);
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
		
		public static string LoadSetupFindDialogRequestItem(ActionBase ActionBase,
		                                                     ref SetupFindDialogRequestItem TempItem)
		{
			string Result = "";
			string Method = GetPrefix() + "LoadSetupFindDialogRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptType");
				TempItem.ReceiptTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FindText");
				TempItem.FindText = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "IncludeInactives");
				TempItem.IncludeInactives = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "TypeId");
				TempItem.TypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string LoadCheckOKToReceiptRequestItem(ActionBase ActionBase,
		                                                     ref CheckOKToReceiptRequestItem TempItem)
		{
			string Result = "";
			string Method = GetPrefix() + "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptType");
				TempItem.ReceiptTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FindId");
				TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string ValidateReceiptEntries(string ReceiptDetail,
		                                            string Payee,
		                                            string TakenBy,
		                                            int ReceiptNo,
		                                            int ReceiptType,
		                                            int BankAccountId,
		                                            int SubLedgerId,
		                                            int RevenueCodeId)
		{
			string Result = "";
			string Method = "ValidateEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Errors = new StringBuilder();
				if (ReceiptDetail.Trim() == "") Errors.AppendLine("Invalid or empty Receipt Detail value");
				if (Payee.Trim() == "") Errors.AppendLine("Invalid or empty Payee value");
				if (TakenBy.Trim() == "") Errors.AppendLine("Invalid or emtpy Taken By value");
				if (ReceiptNo < 0) Errors.AppendLine("Invalid Receipt Number value");
				if (ReceiptType == 0)
				{
					if (BankAccountId == 0) Errors.AppendLine("Please select a Bank Account entry");
				}
				else
				{
					if (SubLedgerId == 0) Errors.AppendLine("Please select a Sub Ledger entry");
					if (RevenueCodeId == 0) Errors.Append("Please select a Revenue entry");
				}
				
				Result = Errors.ToString();
				
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

        public static string LoadDisplayGridReportRequestItem(ActionBase ActionBase,
                                                              ref DisplayGridReportRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadDisplayGridReportRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                GatherReceiptsRequestItem Item = new GatherReceiptsRequestItem();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = Item.Load(ActionBase);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.MasterEventIdValue = Item.MasterEventIdValue;
                    TempItem.ReceiptOptionsIdValue = Item.ReceiptOptionsIdValue;
                    TempItem.PastEventIdValue = Item.PastEventIdValue;

                    Object Obj = ActionBase.GetCollectionValue("Data", "UniqueId");
                    TempItem.UniqueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string CopyRequestObject(DisplayGridReportRequestItem From,
                                               ref GatherReceiptsRequestItem To)
        {
            string Result = "";
            string Method = "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.MasterEventIdValue = From.MasterEventIdValue;
                To.PastEventIdValue = From.PastEventIdValue;
                To.ReceiptOptionsIdValue = From.ReceiptOptionsIdValue;
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

        public static string DisplayGridReport(string ConnectionString,
                                               int CompanyId,
                                               int ClientId,
                                               DisplayGridReportRequestItem Request,
                                               ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "DisplayGridReport";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string ReportSource = "";
                if (string.IsNullOrEmpty(Result))
                {
                    ReportSource = GetReportSource(ConnectionString, CompanyId, ClientId, Request, out Result);
                }

                string OutputFileName = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Int64 NewId =
                        global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
                            ConnectionString, CompanyId, ClientId,
                            Request.UniqueId, ReportSource, out Result, out OutputFileName);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    if (Entries.Length > 0) Entries.Append(",");
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false));
                }
                else
                {
                    Result = ActionBase.LogOnProcessingError(Method, Result, "");
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After DisplayGridReport Result: " + Result, Method, 0);
            }

            return Result;
        }

        private static string GetReportSource(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
                                              DisplayGridReportRequestItem Request,
                                              out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";
            string Method = "GetReportSource";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder Entries = new StringBuilder();
                GatherReceiptsRequestItem Item = new GatherReceiptsRequestItem();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = CopyRequestObject(Request, ref Item);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        EAReceiptScreenGroup2(
                            ConnectionString, CompanyId,  Item.MasterEventId, ClientId, Item.PastEventId, Item.ReceiptOptionsId, ref Entries);
                }

                string Value = "";
                JObject JSONSource = null;
                if (string.IsNullOrEmpty(Result))
                {
                    Value =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        JsonUtilities.EndJson();
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";

                    if (string.IsNullOrEmpty(Result))
                    {
                        JSONSource = JObject.Parse(Value);
                        if (JSONSource == null) Result = "Cannot parse the Group source";
                        Value = "";
                    }
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    string JPath = @"$.GridEntries[?(@.ID == '" + Request.UniqueId + "')]";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, Method, 0);
                    JToken Record = JSONSource.SelectToken(JPath);
                    if (Record == null)
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", Method, 0);
                    else
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), Method, 0);
                        Result = (string)Record["ZL3_B" + Request.ButtonId];
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After GetReportSource Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string ProcessEmail(string ConnectionString,
                                          int CompanyId,
                                          int ClientId,
                                          GatherReceiptsRequestItem Request,
                                          ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "ProcessEmail";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Int64 NewDebtEventId = 0;
                if (string.IsNullOrEmpty(Result))
                {
                    NewDebtEventId =
                        UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
                            ConnectionString, "I_DEV_ID_GEN", out Result);
                    if (string.IsNullOrEmpty(Result) && (NewDebtEventId == 0)) Result = "Error generating the Debt Event ID";
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "EXECUTE PROCEDURE EAReceiptsEmail(@CompanyId, @MasterEventId, @DebtEventId, @LoginId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("MasterEventId", Request.MasterEventId));
                    Params.Add(new FbParameterItem("DebtEventId", NewDebtEventId));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                QueryText = "";
                Params.Clear();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        " select I_ASTR_LEDGERBULKID from eastatementrun, eadebtevent" +
                        " where I_ASTR_CLOSEACCOUNTRUNID = I_DEV_STATEMENTRUNID" +
                        "   and I_ASTATE_COMPANYCLIENTID=@CompanyId" +
                        "   and I_DEV_COMPANYCLIENTID=I_ASTATE_COMPANYCLIENTID" +
                        "   and I_DEV_ID=I_ASTR_DEBTEVENTID" +
                        "   and I_DEV_ID=@DebtEventId";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("DebtEventId", NewDebtEventId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                DataTable tbl = null;
                object Obj = null;
                string StrValue = "";
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["I_ASTR_LEDGERBULKID"];
                            StrValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("LedgerBulkId", StrValue, false));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ProcessEmail Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string LoadDeleteGridEntryRequestItem(ActionBase ActionBase,
                                                            ref DeleteGridEntryRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadDeleteGridEntryRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                GatherReceiptsRequestItem Item = new GatherReceiptsRequestItem();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = Item.Load(ActionBase);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.MasterEventIdValue = Item.MasterEventIdValue;
                    TempItem.ReceiptOptionsIdValue = Item.ReceiptOptionsIdValue;
                    TempItem.PastEventIdValue = Item.PastEventIdValue;

                    Object Obj = ActionBase.GetCollectionValue("Data", "UniqueId");
                    TempItem.UniqueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string RemoveGridEntry(string ConnectionString,
                                             Int64 CompanyId,
                                             int ClientId,
                                             DeleteGridEntryRequestItem Request,
                                             ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "RemoveGridEntry";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();
            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        EAReceiptScreenGroup2(
                            ConnectionString, CompanyId, Request.MasterEventId, ClientId,
                            Request.PastEventId, Request.ReceiptOptionsId, ref TempEntries);
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

                string ReverseString = "";
                if (string.IsNullOrEmpty(Result))
                {
                    string JPath = @"$.GridEntries[?(@.ID == '" + Request.UniqueId + "')]";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, Method, 0);
                    JToken Record = JSONSource.SelectToken(JPath);
                    if (Record == null)
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", Method, 0);
                    else
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), Method, 0);
                        ReverseString = (string)Record["ZC20_REVERSE_STRING"];
                        WindowsEventLoggingUtilities.WriteToInfoEvent("ReverseString: " + ReverseString, Method, 0);
                        if (string.IsNullOrEmpty(ReverseString)) Result = "Cannot determine the Reverse Query";
                    }
                }

                if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(ReverseString))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, ReverseString);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        EAReceiptScreenGroup2(
                            ConnectionString, CompanyId, Request.MasterEventId, ClientId,
                            Request.PastEventId, Request.ReceiptOptionsId, ref Entries);
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
