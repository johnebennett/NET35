using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.Receipts.AdjustReceiptDlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "Receipts.AdjustReceiptDlg."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = null;
                Obj = ActionBase.GetCollectionValue("Data", "GridId");
                TempItem.GridIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ChargeId");
                TempItem.ChargeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "FirstTimeOnly");
                TempItem.FirstTimeOnly = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string SetupPageEntries(string ConnectionString,
                                      int CompanyId,
                                      int ClientId,
                                      SetupPageEntriesRequestItem Request,
                                      ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "SetupPageEntries";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    if (Request.FirstTimeOnly == "Y")
                    {
                        QueryText = "update ximport set EBI_ACTIONEXTRATEST=EBI_ACTIONEXTRA where ximportid = @GridId";
                        Params.Clear();
                        Params.Add(new FbParameterItem("GridId", Request.GridId));
                        Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                        if (string.IsNullOrEmpty(Result))
                        {
                            Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                        }
                    }
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "select OutputSql from eareceiptsimportpartpaidsql(@CompanyId, @GridId, @LoginId, @ChargeId)";
                    Params.Clear();
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("GridId", Request.GridId));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    Params.Add(new FbParameterItem("ChargeId", Request.ChargeId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["OutputSql"];
                            QueryText = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        }
                    }
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText))
                        Result = "Cannot find the first required query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["OUTPUTLINE"];
                            Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupPageEntries Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string LoadUpdateEntryRequestItem(ActionBase ActionBase,
                                                        ref UpdateEntryRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadUpdateEntryRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = Item.Load(ActionBase);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.GridIdValue = Item.GridIdValue;
                    TempItem.ChargeIdValue = Item.ChargeIdValue;
                    TempItem.FirstTimeOnly = Item.FirstTimeOnly;
                }

                Object Obj = null;
                Obj = ActionBase.GetCollectionValue("Data", "SpentAmount");
                TempItem.SpentAmount = (Obj == null) ? "" : Convert.ToString(Obj);
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

        private static string CopyRequestObject(UpdateEntryRequestItem From,
                                                ref SetupPageEntriesRequestItem To)
        {
            string Result = "";
            string Method = "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.GridIdValue = From.GridIdValue;
                To.ChargeIdValue = From.ChargeIdValue;
                To.FirstTimeOnly = From.FirstTimeOnly;
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string UpdateEntry(string ConnectionString,
                                         int CompanyId,
                                         int ClientId,
                                         UpdateEntryRequestItem Request,
                                         ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "UpdateEntry";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();

                if (string.IsNullOrEmpty(Result))
                {
                    Result = CopyRequestObject(Request, ref Item);
                }

                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref TempEntries);
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
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Update query";
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
                    Params.Add(new FbParameterItem("Spend", Request.SpentAmount));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref Entries);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateEntry Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string PostEntry(string ConnectionString,
                                       int CompanyId,
                                       int ClientId,
                                       SetupPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "PostEntry";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);
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
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Post query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After PostEntry Result: " + Result, Method, 0);
            }

            return Result;
        }
		
		public static string DisplayReport(string ConnectionString,
		                                   int CompanyId,
		                                   int ClientId,
		                                   SetupPageEntriesRequestItem Request,
		                                   ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "DisplayReport";
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
				long UniqueId = (long)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
				if (string.IsNullOrEmpty(Result))
				{
					Int64 NewId =
						CommonUtilities.SystemReportUtilities.Execute(
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After DisplayReport Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		private static string GetReportSource(string ConnectionString,
		                                      int CompanyId,
		                                      int ClientId,
		                                      SetupPageEntriesRequestItem Request,
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
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref Entries);
				}
				
				string Str =
					JsonUtilities.StartJson() +
					Entries.ToString() +
					JsonUtilities.EndJson();
				WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, Method, 0);
					
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = JsonUtilities.GetJsonPathValueStr(Str, "$.PDFB1Value", out ErrorMsg);
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
    }
}
