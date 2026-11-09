using System;
using System.Data;
using System.Runtime.Hosting;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.ContactOperations.SMSContact
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "ContactOperations.SMSContact."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
                TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TaskId");
                TempItem.TaskIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TransTypeId");
                TempItem.TransTypeIdValue= (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "FilterEventId");
                TempItem.FilterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
                TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "TaskTypeId");
                TempItem.TaskTypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "WhoTo1");
                TempItem.WhoTo1Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "WhoTo2");
                TempItem.WhoTo2Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TemplateId");
                TempItem.TemplateIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TaskStatusId");
                TempItem.TaskStatusIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                int NewId = 0;
                if (string.IsNullOrEmpty(Result) && (Request.MasterEventId == 0))
                {
                    NewId = UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(ConnectionString, "I_SMS_ID_GEN", out Result);
                    Request.MasterEventId = NewId;
                    WindowsEventLoggingUtilities.WriteToInfoEvent("NewId: " + NewId.ToString(), Method, 0);
                }

                if (string.IsNullOrEmpty(Result) && (Request.MasterEventId > 0))
                {
                    if (Entries.Length > 0) Entries.Append(",");
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("MasterEventId", Request.MasterEventId, false));
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = EASMSScreenPrepare(ConnectionString, CompanyId, ClientId, Request);
                }

                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        "SELECT SqlStatement FROM EASMSScreenStart(" +
                        "@CompanyId, @TransactionId, @TaskId, @TransactionTypeId, @LoginId, @SMSEventId, @FilterEventId, " +
                        "@EmailTemplateId, @WhoTo1, @WhoTo2, @TaskTypeId, @TaskStatusId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
                    Params.Add(new FbParameterItem("TaskId", Request.TaskId));
                    Params.Add(new FbParameterItem("TransactionTypeId", Request.TransTypeId));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    Params.Add(new FbParameterItem("SMSEventId", Request.MasterEventId));
                    Params.Add(new FbParameterItem("FilterEventId", Request.FilterEventId));
                    Params.Add(new FbParameterItem("EmailTemplateId", Request.TemplateId));
                    Params.Add(new FbParameterItem("WhoTo1", Request.WhoTo1));
                    Params.Add(new FbParameterItem("WhoTo2", Request.WhoTo2));
                    Params.Add(new FbParameterItem("TaskTypeId", Request.TaskTypeId));
                    Params.Add(new FbParameterItem("TaskStatusId", Request.TaskStatusId));
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
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["SqlStatement"];
                            QueryText = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        }
                    }
                }

                tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
                    WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, Method, 0);
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

                string Str = "";
                string GetTemplateBlobSQL = "";
                string SMSBody = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Str =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        JsonUtilities.EndJson();
                    GetTemplateBlobSQL = JsonUtilities.GetJsonPathValueStr(Str, "$.GetTemplateBlobSQL", out Result);
                }

                tbl = null;
                if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(GetTemplateBlobSQL))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, GetTemplateBlobSQL, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row[0];
                            if (Obj != DBNull.Value)
                            {
                                byte[] byteArray = (byte[])Obj;
                                SMSBody = System.Text.Encoding.Default.GetString(byteArray);

                            }
                        }
                    }
                }
                if (Entries.Length > 0) Entries.Append(",");
                Entries.Append(JsonUtilities.AddPropertyNameAndValue("SMSBody", SMSBody, false));
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

        public static string EASMSScreenPrepare(string ConnectionString,
                                                int CompanyId,
                                                int ClientId,
                                                SetupPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "EASMSScreenPrepare";
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
                        "EXECUTE PROCEDURE EASMSScreenPrepare(" +
                        "@CompanyId, @TransactionId, @TaskId, @TransactionTypeId, @LoginId, @SMSEventId, @FilterEventId, " +
                        "@EmailTemplateId, @WhoTo1, @WhoTo2, @TaskTypeId, @TaskStatusId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
                    Params.Add(new FbParameterItem("TaskId", Request.TaskId));
                    Params.Add(new FbParameterItem("TransactionTypeId", Request.TransTypeId));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    Params.Add(new FbParameterItem("SMSEventId", Request.MasterEventId));
                    Params.Add(new FbParameterItem("FilterEventId", Request.FilterEventId));
                    Params.Add(new FbParameterItem("EmailTemplateId", Request.TemplateId));
                    Params.Add(new FbParameterItem("WhoTo1", Request.WhoTo1));
                    Params.Add(new FbParameterItem("WhoTo2", Request.WhoTo2));
                    Params.Add(new FbParameterItem("TaskTypeId", Request.TaskTypeId));
                    Params.Add(new FbParameterItem("TaskStatusId", Request.TaskStatusId));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After EASMSScreenPrepare Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string GatherRecipients(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
                                              SetupPageEntriesRequestItem Request,
                                              ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "GatherRecipients";
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
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PrepareSQL1", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Prepare query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PrepareSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Prepare query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After GatherRecipients Result: " + Result, Method, 0);
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
                if (string.IsNullOrEmpty(Result))
                {
                    Result = Item.Load(ActionBase);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.MasterEventIdValue = Item.MasterEventIdValue;
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.TaskIdValue = Item.TaskIdValue;
                    TempItem.TransTypeIdValue = Item.TransTypeIdValue;
                    TempItem.FilterEventIdValue = Item.FilterEventIdValue;
                    TempItem.TaskTypeIdValue = Item.TaskTypeIdValue;
                    TempItem.WhoTo1Value = Item.WhoTo1Value;
                    TempItem.WhoTo2Value = Item.WhoTo2Value;
                    TempItem.TemplateIdValue = Item.TemplateIdValue;
                    TempItem.TaskStatusIdValue = Item.TaskStatusIdValue;
                }

                Object Obj = ActionBase.GetCollectionValue("Data", "UniqueId");
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
                To.MasterEventIdValue = From.MasterEventIdValue;
                To.PortfolioIdValue = From.PortfolioIdValue;
                To.TaskIdValue = From.TaskIdValue;
                To.TransTypeIdValue = From.TransTypeIdValue;
                To.FilterEventIdValue = From.FilterEventIdValue;
                To.TaskTypeIdValue = From.TaskTypeIdValue;
                To.WhoTo1Value = From.WhoTo1Value;
                To.WhoTo2Value = From.WhoTo2Value;
                To.TemplateIdValue = From.TemplateIdValue;
                To.TaskStatusIdValue = From.TaskStatusIdValue;
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

        private static string CopyRequestObject(AllPageEntriesRequestItem From,
                                                ref SetupPageEntriesRequestItem To)
        {
            string Result = "";
            string Method = "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.MasterEventIdValue = From.MasterEventIdValue;
                To.PortfolioIdValue = From.PortfolioIdValue;
                To.TaskIdValue = From.TaskIdValue;
                To.TransTypeIdValue = From.TransTypeIdValue;
                To.FilterEventIdValue = From.FilterEventIdValue;
                To.TaskTypeIdValue = From.TaskTypeIdValue;
                To.WhoTo1Value = From.WhoTo1Value;
                To.WhoTo2Value = From.WhoTo2Value;
                To.TemplateIdValue = From.TemplateIdValue;
                To.TaskStatusIdValue = From.TaskStatusIdValue;
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
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.RemoveSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Remove query";
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("Id", Request.UniqueId));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After RemoveGridEntry Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string LoadAllPageEntriesRequestItem(ActionBase ActionBase,
                                                           ref AllPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadAllPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

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
                    TempItem.MasterEventIdValue = Item.MasterEventIdValue;
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.TaskIdValue = Item.TaskIdValue;
                    TempItem.TransTypeIdValue = Item.TransTypeIdValue;
                    TempItem.FilterEventIdValue = Item.FilterEventIdValue;
                    TempItem.TaskTypeIdValue = Item.TaskTypeIdValue;
                    TempItem.WhoTo1Value = Item.WhoTo1Value;
                    TempItem.WhoTo2Value = Item.WhoTo2Value;
                    TempItem.TemplateIdValue = Item.TemplateIdValue;
                    TempItem.TaskStatusIdValue = Item.TaskStatusIdValue;
                }

                Object Obj = ActionBase.GetCollectionValue("Data", "SMSBody");
                TempItem.SMSBody = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string ProcessRun(string ConnectionString,
                                        int CompanyId,
                                        int ClientId,
                                        AllPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "ProcessRun";
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
                        if (JSONSource == null)
                        { 
                            Result = "Cannot parse the Group source";
                            WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, 0);
                        }
                        Value = "";
                    }
                }

                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second Save query";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("SaveSQL2: " + QueryText, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling ProcessSaveSql2", Method, 0);
                    Result = ProcessSaveSql2(ConnectionString, CompanyId, ClientId, QueryText, Request);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling ProcessSaveSql2", Method, 0);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Post query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
                }

                string EventRunId = "";
                if (string.IsNullOrEmpty(Result))
                {
                    EventRunId = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.EventRunID", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(EventRunId)) Result = "Cannot find the EventRun ID";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = ProcessSMSMessages(EventRunId);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ProcessRun Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string ProcessSaveSql2(string ConnectionString,
                                             int CompanyId,
                                             int ClientId,
                                             string QueryText,
                                             AllPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "ProcessSaveSql2";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(Request.SMSBody);
                    Params.Add(new FbParameterItem("BlobText", bytes));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ProcessSaveSql2 Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string ProcessSMSMessages(string EventRunId)
        {
            string Result = "";
            string Method = GetModuleName() + "ProcessSMSMessages";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string WorkingDir = "";
                if (string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling GetCurrentDirectory", Method, 0);
                    //WorkingDir = Directory.GetCurrentDirectory();
                    WorkingDir = CommonUtilities.FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
                    string SepChar = Path.DirectorySeparatorChar.ToString();
                    if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling GetCurrentDirectory", Method, 0);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("WorkingDir " + WorkingDir, Method, 0);
                    try
                    {
                        Process runApp = new Process();
                        runApp.StartInfo.FileName = WorkingDir + "EmailProcessor";
                        runApp.StartInfo.Arguments = EventRunId;
                        runApp.Start();
                    }
                    catch (Exception Ex)
                    {
                        Result = ActionBase.LogOnException(Method, Ex, "");
                    }
                    finally
                    {
                        Directory.SetCurrentDirectory(WorkingDir);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ProcessSMSMessages Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
