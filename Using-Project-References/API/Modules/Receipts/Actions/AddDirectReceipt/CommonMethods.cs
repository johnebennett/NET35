/*
 * Created by SharpDevelop.
 * User: John
 * Date: 01/05/2020
 * Time: 3:13 PM
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
namespace Actions.Receipts.AddDirectReceipt
{
    /// <summary>
    /// Description of CommonMethods.
    /// </summary>
    public class CommonMethods
    {
        public static string GetPrefix()
        {
            return "AddDirectReceipt.";
        }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref AddDirectReceipt.SetupPageEntriesRequestItem TempItem)
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

                Obj = ActionBase.GetCollectionValue("Data", "FindText");
                TempItem.FindText = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "FindId");
                TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
                TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "BankId");
                TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "RevenueId");
                TempItem.RevenueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "GST");
                TempItem.GST = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReceiptAmount");
                TempItem.ReceiptAmount = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReceiptGSTAmount");
                TempItem.ReceiptGSTAmount = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReceiptNo");
                TempItem.ReceiptNoValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReceiptDate");
                TempItem.ReceiptDate = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReceiptClearance");
                TempItem.ReceiptClearance = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Detail");
                TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Payee");
                TempItem.Payee = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TakenBy");
                TempItem.TakenBy = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ClientName");
                TempItem.ClientName = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Address1");
                TempItem.Address1 = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Address2");
                TempItem.Address2 = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Address3");
                TempItem.Address3 = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Address4");
                TempItem.Address4 = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "PaidOwnerDirect");
                TempItem.PaidOwnerDirect = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "PageNo");
                TempItem.PageNoValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "XImportId");
                TempItem.XImportIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string EAReceiptScreenDirectAdd(string ConnectionString,
                                                      Int64 CompanyId,
                                                      Int64 ClientId,
                                                      AddDirectReceipt.SetupPageEntriesRequestItem Item,
                                                      ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetPrefix() + "EAReceiptScreenDirectAdd";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText =
                    "SELECT OutputLine FROM EAReceiptScreenDirectAdd(" +
                    "  @CompanyId, @ReceiptEventId, @LoginId, @ReceiptType, @FindText, @FindId," +
                    "  @SubLedgerId, @BankId, @RevenueId, @GST, @ReceiptAmount, @ReceiptGSTAmount," +
                    "  @ReceiptNo, @ReceiptDate, @ReceiptClearance, @Detail, @Payee, @TakenBy, @ClientName," +
                    "  @Address1, @Address2, @Address3, @Address4, @PaidOwnerDirect, @PageNo, @XImportId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyId", CompanyId));
                Params.Add(new FbParameterItem("ReceiptEventId", Item.MasterEventId));
                Params.Add(new FbParameterItem("LoginId", ClientId));
                Params.Add(new FbParameterItem("ReceiptType", Item.ReceiptType));
                Params.Add(new FbParameterItem("FindText", Item.FindText));
                Params.Add(new FbParameterItem("FindId", Item.FindId));
                Params.Add(new FbParameterItem("SubLedgerId", Item.SubLedgerId));
                Params.Add(new FbParameterItem("BankId", Item.BankId));
                Params.Add(new FbParameterItem("RevenueId", Item.RevenueId));
                Params.Add(new FbParameterItem("GST", Item.GST));
                Params.Add(new FbParameterItem("ReceiptAmount", Item.ReceiptAmount));
                Params.Add(new FbParameterItem("ReceiptGSTAmount", Item.ReceiptGSTAmount));
                Params.Add(new FbParameterItem("ReceiptNo", Item.ReceiptNo));
                Params.Add(new FbParameterItem("ReceiptDate", Item.ReceiptDate));
                Params.Add(new FbParameterItem("ReceiptClearance", Item.ReceiptClearance));
                Params.Add(new FbParameterItem("Detail", Item.Detail));
                Params.Add(new FbParameterItem("Payee", Item.Payee));
                Params.Add(new FbParameterItem("TakenBy", Item.TakenBy));
                Params.Add(new FbParameterItem("ClientName", Item.ClientName));
                Params.Add(new FbParameterItem("Address1", Item.Address1));
                Params.Add(new FbParameterItem("Address2", Item.Address2));
                Params.Add(new FbParameterItem("Address3", Item.Address3));
                Params.Add(new FbParameterItem("Address4", Item.Address4));
                Params.Add(new FbParameterItem("PaidOwnerDirect", Item.PaidOwnerDirect));
                Params.Add(new FbParameterItem("PageNo", Item.PageNo));
                Params.Add(new FbParameterItem("XImportId", Item.XImportId));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

                DataTable tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result) && (tbl == null)) Result = "Error calling query in " + Method;
                }

                if (string.IsNullOrEmpty(Result) && (tbl != null))
                {
                    if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
                    foreach (DataRow Row in tbl.Rows)
                    {
                        Object Obj = Row["OUTPUTLINE"];
                        Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
                    }
                }

                string GridSql = "";
                if (string.IsNullOrEmpty(Result))
                {
                    GridSql = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.GridSQL", out Result);
                    //if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(GridSql)) Result = "Cannot find the Grid SQL value";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("GridSQL:" + Environment.NewLine + GridSql, Method, 0);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Result:" + Result, Method, 0);
                }

                tbl = null;
                if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(GridSql))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("NOT Building GridEntries", Method, 0);
                    if (Entries.Length > 0) Entries.Append(",");
                    Entries.Append(JsonUtilities.StartJsonArray("GridEntries"));
                    Entries.Append(JsonUtilities.EndJsonArray());
                }
                else if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(GridSql))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, GridSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result) && (tbl == null)) Result = "Error calling query in " + Method;

                    if (string.IsNullOrEmpty(Result))
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Building GridEntries", Method, 0);
                        if (Entries.Length > 0) Entries.Append(",");
                        Result = JsonUtilities.BuildJsonArrayFromDataTable(tbl, "GridEntries", ref Entries);
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

        /*
		public static string EAFindScreenSql(string ConnectionString,
		                                     Int64 CompanyId,
		                                     int ReceiptType,
		                                     string WhatToFind,
		                                     string IncludeInactives,
		                                     ref StringBuilder Entries)
		{
			string Result = "";
			string Method = GetPrefix() + "EAFindScreenSql";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT SqlStatement FROM EAFindScreenSql(@CompanyId, @ReceiptType, @TableToSearch, @WhatToFind, @IncludeInactives)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("ReceiptType", ReceiptType));
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
		*/

        public static string EABankAddReceiptCheck(string ConnectionString,
                                                   Int64 CompanyId,
                                                   int FindId,
                                                   out string OKToReceipt,
                                                   out string OKToReceiptMessage)
        {
            string Result = "";
            string Method = GetPrefix() + "EABankAddReceiptCheck";
            OKToReceipt = "N";
            OKToReceiptMessage = "No Value Returned";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "SELECT OKToReceipt, OkToReceiptMessage FROM EABankAddReceiptCheck(@CompanyId, @FindId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyId", CompanyId));
                Params.Add(new FbParameterItem("FindId", FindId));
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
                    foreach (DataRow Row in tbl.Rows)
                    {
                        Obj = Row["OKToReceipt"];
                        OKToReceipt = (Obj == DBNull.Value) ? OKToReceipt : Convert.ToString(Obj);
                        Obj = Row["OKToReceiptMessage"];
                        OKToReceiptMessage = (Obj == DBNull.Value) ? OKToReceiptMessage : Convert.ToString(Obj);
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

        public static string EAReceiptDirectPrepare(string ConnectionString,
                                                    Int64 CompanyId,
                                                    int ReceiptEventId,
                                                    int FindId,
                                                    Int64 AddedById)
        {
            string Result = "";
            string Method = GetPrefix() + "EAReceiptDirectPrepare_Zero";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Result = EAReceiptDirectPrepare(ConnectionString, CompanyId, ReceiptEventId, FindId, "0.00", AddedById);
                /*
				string QueryText = "EXECUTE PROCEDURE EAReceiptDirectPrepare(@CompanyId, @ReceiptEventId, @FindId, 0, 0, 0, @AddedById)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("ReceiptEventId", ReceiptEventId));
				Params.Add(new FbParameterItem("FindId", FindId));
				Params.Add(new FbParameterItem("AddedById", AddedById));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
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

        public static string EAReceiptDirectPrepare(string ConnectionString,
                                                    Int64 CompanyId,
                                                    int ReceiptEventId,
                                                    int FindId,
                                                    string ReceiptAmount,
                                                    Int64 AddedById)
        {
            string Result = "";
            string Method = GetPrefix() + "EAReceiptDirectPrepare";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "EXECUTE PROCEDURE EAReceiptDirectPrepare(@CompanyId, @ReceiptEventId, @FindId, @ReceiptAmount, 0, 0, @AddedById)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyId", CompanyId));
                Params.Add(new FbParameterItem("ReceiptEventId", ReceiptEventId));
                Params.Add(new FbParameterItem("FindId", FindId));
                Params.Add(new FbParameterItem("ReceiptAmount", ReceiptAmount));
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

        public static string EAReceiptDirectTemp(string ConnectionString,
                                                 int ReceiptEventId)
        {
            string Result = "";
            string Method = GetPrefix() + "EAReceiptDirectTemp";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "DELETE FROM EAReceiptDirectTemp WHERE ReceiptEventId = @ReceiptEventId";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("ReceiptEventId", ReceiptEventId));
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

        public static string LoadChangeIncludeStatusRequestItem(ActionBase ActionBase,
                                                                ref AddDirectReceipt.ChangeIncludeStatusRequestItem TempItem)
        {
            string Result = "";
            string Method = GetPrefix() + "LoadChangeIncludeStatusRequestItem";
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
                Obj = ActionBase.GetCollectionValue("Data", "FindId");
                TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
                TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "BankId");
                TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "RevenueId");
                TempItem.RevenueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "GST");
                TempItem.GST = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReceiptAmount");
                TempItem.ReceiptAmount = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReceiptGSTAmount");
                TempItem.ReceiptGSTAmount = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReceiptNo");
                TempItem.ReceiptNoValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReceiptDate");
                TempItem.ReceiptDate = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReceiptClearance");
                TempItem.ReceiptClearance = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Detail");
                TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Payee");
                TempItem.Payee = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TakenBy");
                TempItem.TakenBy = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ClientName");
                TempItem.ClientName = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Address1");
                TempItem.Address1 = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Address2");
                TempItem.Address2 = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Address3");
                TempItem.Address3 = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Address4");
                TempItem.Address4 = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "PaidOwnerDirect");
                TempItem.PaidOwnerDirect = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "PageNo");
                TempItem.PageNoValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "XImportId");
                TempItem.XImportIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "UniqueId");
                TempItem.UniqueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string DisplayReceiptMessage(string ConnectionString,
                                                      Int64 CompanyId,
                                                      Int64 ClientId,
                                                      AddDirectReceipt.SetupPageEntriesRequestItem Item,
                                                      ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetPrefix() + "EAReceiptScreenDirectAdd";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText =
                    "SELECT OutputLine FROM EAReceiptScreenDirectAdd(" +
                    "  @CompanyId, @ReceiptEventId, @LoginId, @ReceiptType, @FindText, @FindId," +
                    "  @SubLedgerId, @BankId, @RevenueId, @GST, @ReceiptAmount, @ReceiptGSTAmount," +
                    "  @ReceiptNo, @ReceiptDate, @ReceiptClearance, @Detail, @Payee, @TakenBy, @ClientName," +
                    "  @Address1, @Address2, @Address3, @Address4, @PaidOwnerDirect, @PageNo, @XImportId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyId", CompanyId));
                Params.Add(new FbParameterItem("ReceiptEventId", Item.MasterEventId));
                Params.Add(new FbParameterItem("LoginId", ClientId));
                Params.Add(new FbParameterItem("ReceiptType", Item.ReceiptType));
                Params.Add(new FbParameterItem("FindText", Item.FindText));
                Params.Add(new FbParameterItem("FindId", Item.FindId));
                Params.Add(new FbParameterItem("SubLedgerId", Item.SubLedgerId));
                Params.Add(new FbParameterItem("BankId", Item.BankId));
                Params.Add(new FbParameterItem("RevenueId", Item.RevenueId));
                Params.Add(new FbParameterItem("GST", Item.GST));
                Params.Add(new FbParameterItem("ReceiptAmount", Item.ReceiptAmount));
                Params.Add(new FbParameterItem("ReceiptGSTAmount", Item.ReceiptGSTAmount));
                Params.Add(new FbParameterItem("ReceiptNo", Item.ReceiptNo));
                Params.Add(new FbParameterItem("ReceiptDate", Item.ReceiptDate));
                Params.Add(new FbParameterItem("ReceiptClearance", Item.ReceiptClearance));
                Params.Add(new FbParameterItem("Detail", Item.Detail));
                Params.Add(new FbParameterItem("Payee", Item.Payee));
                Params.Add(new FbParameterItem("TakenBy", Item.TakenBy));
                Params.Add(new FbParameterItem("ClientName", Item.ClientName));
                Params.Add(new FbParameterItem("Address1", Item.Address1));
                Params.Add(new FbParameterItem("Address2", Item.Address2));
                Params.Add(new FbParameterItem("Address3", Item.Address3));
                Params.Add(new FbParameterItem("Address4", Item.Address4));
                Params.Add(new FbParameterItem("PaidOwnerDirect", Item.PaidOwnerDirect));
                Params.Add(new FbParameterItem("PageNo", Item.PageNo));
                Params.Add(new FbParameterItem("XImportId", Item.XImportId));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

                DataTable tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result) && (tbl == null)) Result = "Error calling query in " + Method;
                }

                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result) && (tbl != null))
                {
                    if ((TempEntries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
                    foreach (DataRow Row in tbl.Rows)
                    {
                        Object Obj = Row["OUTPUTLINE"];
                        TempEntries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
                    }
                }

                string ReportSource = "";
                if (string.IsNullOrEmpty(Result))
                {
                    ReportSource = JsonUtilities.GetJsonPathValueStr(TempEntries.ToString(), "$.PDFB1Value", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(ReportSource)) Result = "Cannot find the Report Source";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Result:" + Result, Method, 0);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("ReportSource:" + ReportSource, Method, 0);
                }

                string OutputFileName = "";
                if (string.IsNullOrEmpty(Result))
                {
                    long UniqueId = (long)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
                    Int64 NewId =
                        global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
                            ConnectionString, CompanyId, ClientId,
                            UniqueId, ReportSource, out Result, out OutputFileName);
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
            }

            return Result;
        }
    }
}
