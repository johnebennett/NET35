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
namespace Actions.SettingsOperations.LedgerMarkUnmark
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "SettingsOperations.LedgerMarkUnmark."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "CallStart");
                TempItem.CallStart = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TransactionId");
                TempItem.TransactionIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "FindText");
                TempItem.FindText = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
                TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "OptionsId");
                TempItem.OptionsIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
                    if (Request.CallStart == "Y")
                    {
                        QueryText = "execute procedure EAMARKUNMARKSTART(@CompanyId, @LoginId)";
                        Params.Clear();
                        Params.Add(new FbParameterItem("CompanyId", CompanyId));
                        Params.Add(new FbParameterItem("LoginId", ClientId));
                        Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                        if (string.IsNullOrEmpty(Result))
                        {
                            Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                        }
                        if (string.IsNullOrEmpty(Result)) Request.CallStart = "N";
                    }
                }

                if (string.IsNullOrEmpty(Result))
                {

                }

                if (string.IsNullOrEmpty(Result))
                {
                    if (Entries.Length > 0) Entries.Append(",");
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("CallStart", Request.CallStart, false ));
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        "SELECT OutputLine FROM EAMarkUnmarkGroup(" +
                        "@CompanyId, @TransactionId, @FindText, @SubLedgerId, @OptionsId, @LoginId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", Request.TransactionId));
                    Params.Add(new FbParameterItem("FindText", Request.FindText));
                    Params.Add(new FbParameterItem("SubLedgerId", Request.SubLedgerId));
                    Params.Add(new FbParameterItem("OptionsId", Request.OptionsId));
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

        public static string LoadRemoveGridEntryRequestItem(ActionBase ActionBase,
                                                            ref RemoveGridEntryRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadRemoveGridEntryRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                Result = Item.Load(ActionBase);
                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.CallStart = Item.CallStart;
                    TempItem.TransactionIdValue = Item.TransactionIdValue;
                    TempItem.FindText = Item.FindText;
                    TempItem.SubLedgerIdValue = Item.SubLedgerIdValue;
                    TempItem.OptionsIdValue = Item.OptionsIdValue;

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

        private static string CopyRequestObject(RemoveGridEntryRequestItem From,
                                                ref SetupPageEntriesRequestItem To)
        {
            string Result = "";
            string Method = GetModuleName() + "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.CallStart = From.CallStart;
                To.TransactionIdValue = From.TransactionIdValue;
                To.FindText = From.FindText;
                To.SubLedgerIdValue = From.SubLedgerIdValue;
                To.OptionsIdValue = From.OptionsIdValue;
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

        public static string RemoveGridEntry(string ConnectionString,
                                             int CompanyId,
                                             int ClientId,
                                             RemoveGridEntryRequestItem Request,
                                             ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "RemoveGridEntry";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder TempEntries = new StringBuilder();
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = CopyRequestObject(Request, ref Item);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref TempEntries);
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
                    string JPath = @"$.gridEntries[?(@.ZC5_ID == '" + Request.UniqueId + "')]";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, Method, 0);
                    JToken Record = JSONSource.SelectToken(JPath);
                    if (Record == null)
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", Method, 0);
                    else
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), Method, 0);
                        QueryText = (string)Record["ZL20_SelectSQL"];
                        WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + QueryText, Method, 0);
                    }

                    if (string.IsNullOrEmpty(Result) && (string.IsNullOrEmpty(QueryText))) Result = "Cannot determine the Select Sql";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
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

        public static string Post(string ConnectionString,
                                  int CompanyId,
                                  int ClientId,
                                  SetupPageEntriesRequestItem Request,
                                  ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "Post";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);
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

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("SubLedgerId", Request.SubLedgerId));
                    Params.Add(new FbParameterItem("Options", Request.OptionsId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Request.CallStart = "Y";
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After Post Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
