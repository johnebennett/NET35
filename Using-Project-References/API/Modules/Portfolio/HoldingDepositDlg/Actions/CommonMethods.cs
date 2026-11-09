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
namespace Actions.Portfolio.HoldingDepositDlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "Portfolio.HoldingDepositDlg"; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
                TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "FoundId");
                TempItem.FoundIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DepositAmount");
                TempItem.DepositAmount = (Obj == null) ? "" : Convert.ToString(Obj);
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
            string Method = "SetupPageEntries";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        "SELECT OutputLine FROM EAHoldingDepositGroup(@CompanyId, @TransactionId, @FoundId, @DepositAmount, @LoginId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
                    Params.Add(new FbParameterItem("FoundId", Request.FoundId));
                    Params.Add(new FbParameterItem("DepositAmount", Request.DepositAmount));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
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
                        Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FoundId", Request.FoundIdValue, false));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupPageEntries Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string Post(string ConnectionString,
                                  int CompanyId,
                                  int ClientId,
                                  SetupPageEntriesRequestItem Request,
                                  ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "Post";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);
                }

                string Source = "";
                JObject JSONSource = null;
                if (string.IsNullOrEmpty(Result))
                {
                    Source =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();
                    JSONSource = JObject.Parse(Source);
                    if (JSONSource == null) Result = "Cannot parse the Group source";
                    Source = "";
                }

                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the post query";
                    if (string.IsNullOrEmpty(Result))
                        Result = ExecutePostSQL(ConnectionString, QueryText, Request.DepositAmount);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref Entries);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After Post Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string ExecutePostSQL(string ConnectionString,
                                            string QueryText,
                                            string DepositAmount)
        {
            string Result = "";
            string Method = "ExecutePostSQL";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
                    Params.Add(new FbParameterItem("DEPOSITAMOUNT", DepositAmount));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ExecutePostSQL Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }
    }
}
