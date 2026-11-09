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
namespace Actions.Portfolio.SplitsDlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "Portfolio.SplitsDlg"; }

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
                Obj = ActionBase.GetCollectionValue("Data", "MultiStrataNumber");
                TempItem.MultiStrataNumberValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "IncludedList");
                TempItem.IncludedListValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
                        "SELECT OutputLine FROM EAMultiCommGroup(@CompanyId, @TransactionId, @MultiStrataNumber, @IncludedList)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
                    Params.Add(new FbParameterItem("MultiStrataNumber", Request.MultiStrataNumber));
                    Params.Add(new FbParameterItem("IncludedList", Request.IncludedList));
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

        public static string LoadUpdateMultiNameRequestItem(ActionBase ActionBase,
                                                            ref UpdateMultiNameRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadUpdateMultiNameRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                Result = Item.Load(ActionBase);
                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.MultiStrataNumberValue = Item.MultiStrataNumberValue;
                    TempItem.IncludedListValue = Item.IncludedListValue;

                    Object Obj = ActionBase.GetCollectionValue("Data", "MultiName1");
                    TempItem.MultiName1 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MultiName2");
                    TempItem.MultiName2 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MultiName3");
                    TempItem.MultiName3 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MultiName4");
                    TempItem.MultiName4 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MultiName5");
                    TempItem.MultiName5 = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "MultiName6");
                    TempItem.MultiName6 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MultiName7");
                    TempItem.MultiName7 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MultiName8");
                    TempItem.MultiName8 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MultiName9");
                    TempItem.MultiName9 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MultiName10");
                    TempItem.MultiName10 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MultiName11");
                    TempItem.MultiName11 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MultiName12");
                    TempItem.MultiName12 = (Obj == null) ? "" : Convert.ToString(Obj);
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

        private static string CopyRequestObject(UpdateMultiNameRequestItem From,
                                                ref SetupPageEntriesRequestItem To)
        {
            string Result = "";
            string Method = "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.PortfolioIdValue = From.PortfolioIdValue;
                To.MultiStrataNumberValue = From.MultiStrataNumberValue;
                To.IncludedListValue = From.IncludedListValue;
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

        public static string UpdateMultiName(string ConnectionString,
                                             int CompanyId,
                                             int ClientId,
                                             UpdateMultiNameRequestItem Request)
        {
            string Result = "";
            string Method = "UpdateMultiName";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

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

                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(TempEntries.ToString(), "$.UpdateMultiNameSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update query";
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("strata1", Request.MultiName1));
                    Params.Add(new FbParameterItem("strata2", Request.MultiName2));
                    Params.Add(new FbParameterItem("strata3", Request.MultiName3));
                    Params.Add(new FbParameterItem("strata4", Request.MultiName4));
                    Params.Add(new FbParameterItem("strata5", Request.MultiName5));
                    Params.Add(new FbParameterItem("strata6", Request.MultiName6));
                    Params.Add(new FbParameterItem("strata7", Request.MultiName7));
                    Params.Add(new FbParameterItem("strata8", Request.MultiName8));
                    Params.Add(new FbParameterItem("strata9", Request.MultiName9));
                    Params.Add(new FbParameterItem("strata10", Request.MultiName10));
                    Params.Add(new FbParameterItem("strata11", Request.MultiName11));
                    Params.Add(new FbParameterItem("strata12", Request.MultiName12));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateMultiName Result: " + Result, Method, 0);
            }

            return Result;
        }

        private static string CopyRequestObject(UpdateDetailsRequestItem From,
                                                ref SetupPageEntriesRequestItem To)
        {
            string Result = "";
            string Method = "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.PortfolioIdValue = From.PortfolioIdValue;
                To.MultiStrataNumberValue = From.MultiStrataNumberValue;
                To.IncludedListValue = From.IncludedListValue;
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

        public static string LoadUpdateDetailsRequestItem(ActionBase ActionBase,
                                                          ref UpdateDetailsRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadUpdateDetailsRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                Result = Item.Load(ActionBase);
                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.MultiStrataNumberValue = Item.MultiStrataNumberValue;
                    TempItem.IncludedListValue = Item.IncludedListValue;

                    Object Obj = ActionBase.GetCollectionValue("Data", "Percentage");
                    TempItem.PercentageValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string UpdateDetails(string ConnectionString,
                                           int CompanyId,
                                           int ClientId,
                                           UpdateDetailsRequestItem Request,
                                           ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "UpdateDetails";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

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

                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(TempEntries.ToString(), "$.UpdateSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update query";
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
                    Params.Add(new FbParameterItem("liability", Request.Percentage));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
                    UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateDetails Result: " + Result, Method, 0);
            }

            return Result;
        }

        private static string CopyRequestObject(RemoveGridEntryRequestItem From,
                                                ref SetupPageEntriesRequestItem To)
        {
            string Result = "";
            string Method = "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.PortfolioIdValue = From.PortfolioIdValue;
                To.MultiStrataNumberValue = From.MultiStrataNumberValue;
                To.IncludedListValue = From.IncludedListValue;
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

        public static string LoadRemoveGridEntryRequestItem(ActionBase ActionBase,
                                                            ref RemoveGridEntryRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadRemoveGridEntryRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                Result = Item.Load(ActionBase);
                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.MultiStrataNumberValue = Item.MultiStrataNumberValue;
                    TempItem.IncludedListValue = Item.IncludedListValue;

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
                                            int CompanyId,
                                            int ClientId,
                                            RemoveGridEntryRequestItem Request,
                                            ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "RemoveGridEntry";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

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

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        ExecuteRemoveSql(
                            ConnectionString, JSONSource, Request.UniqueId);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After RemoveGridEntry Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string ExecuteRemoveSql(string ConnectionString,
                                              //int CompanyId,
                                              //int ClientId,
                                              JObject JSONSource,
                                              int UniqueId)
        {
            string Result = "";
            string Method = "ExeecuteRemoveSql";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    string JPath = @"$.gridEntries[?(@.I_ID == '" + UniqueId + "')]";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, Method, 0);
                    JToken Record = JSONSource.SelectToken(JPath);
                    if (Record == null)
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", Method, 0);
                    else
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), Method, 0);
                        QueryText = (string)Record["ZL100_RemoveSQL"];
                        WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + QueryText, Method, 0);
                    }

                    if (string.IsNullOrEmpty(Result) && (string.IsNullOrEmpty(QueryText))) Result = "Cannot determine the Remove Sql";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ExeecuteRemoveSql Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
