/*
 * Created by SharpDevelop.
 * User: John
 * Date: 08/08/2020
 * Time: 2:57 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.ToDo.Job
{
    /// <summary>
    /// Description of CommonMethods.
    /// </summary>
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "ToDo.Job."; }

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
                Obj = ActionBase.GetCollectionValue("Data", "TaskType");
                TempItem.TaskTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ServiceId");
                TempItem.ServiceIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "NotesId");
                TempItem.NotesIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "OptionId");
                TempItem.OptionIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
                int NewId = 0;
                int NewTaskId = 0;
                if (string.IsNullOrEmpty(Result) && (Request.TaskId == 0))
                {
                    NewId = UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(ConnectionString, "i_atask_id_gen", out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        Result = EATaskAddEditCreate(ConnectionString, CompanyId, ClientId, Request.PortfolioId, NewId, out NewTaskId);
                    }

                    if (string.IsNullOrEmpty(Result))
                    {
                        Request.TaskId = NewTaskId;
                    }
                }

                /*
                if (string.IsNullOrEmpty(Result) && (Request.TaskId > 0))
                {
                    if (Entries.Length > 0) Entries.Append(",");
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("TaskId", Request.TaskId, false));
                }
                */

                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        "SELECT OutputLine FROM EATASKADDEDITSTART(" +
                        "@CompanyId, @TransactionId, @LoginId, @TaskId, @TaskType, @ServiceId, @NotesId, @OptionId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    Params.Add(new FbParameterItem("TaskId", Request.TaskId));
                    Params.Add(new FbParameterItem("TaskType", Request.TaskType));
                    Params.Add(new FbParameterItem("ServiceId", Request.ServiceId));
                    Params.Add(new FbParameterItem("NotesId", Request.NotesId));
                    Params.Add(new FbParameterItem("OptionId", Request.OptionId));
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
                            Obj = Row["OUTPUTLINE"];
                            QueryText = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        }
                    }
                }

                tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
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
                string NoteSql = "";
                string MaintenanceNoteSql = "";
                string Notes = "";
                //string MaintenanceNotes = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Str =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        JsonUtilities.EndJson();
                    NoteSql = JsonUtilities.GetJsonPathValueStr(Str, "$.NotesSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(NoteSql)) Result = "Cannot find the Notes entry";
                    if (string.IsNullOrEmpty(Result))
                    {
                        MaintenanceNoteSql = JsonUtilities.GetJsonPathValueStr(Str, "$.MaintenanceNotesSQL", out Result);
                        if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(MaintenanceNoteSql)) Result = "Cannot find the Maintenance Notes entry";
                    }
                }

                tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, NoteSql, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
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

                /*
                tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, MaintenanceNoteSql, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row[0];
                            if (Obj != DBNull.Value)
                            {
                                byte[] byteArray = (byte[])Obj;
                                MaintenanceNotes = System.Text.Encoding.Default.GetString(byteArray);

                            }
                        }
                        if (Entries.Length > 0) Entries.Append(",");
                        Entries.Append(JsonUtilities.AddPropertyNameAndValue("MaintenanceNotes", MaintenanceNotes, false));
                    }
                }
                */

                if (string.IsNullOrEmpty(Result))
                {
                    Result = GetMaintenanceNoteText(ConnectionString, MaintenanceNoteSql, ref Entries);
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

        public static string EATaskAddEditCreate(string ConnectionString,
                                                 int CompanyId,
                                                 int ClientId,
                                                 int TransactionId,
                                                 int NewId,
                                                 out int NewTaskId)
        {
            string Result = "";
            string Method = "EATaskAddEditCreate";
            NewTaskId = 0;
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "EXECUTE PROCEDURE EATaskAddEditCreate(@CompanyId, @TransactionId, @NewId, @LoginId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", TransactionId));
                    Params.Add(new FbParameterItem("NewId", NewId));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "SELECT FIRST 1 I_ATASK_TASKID from EATASK WHERE I_ATASK_ID = @NewId";
                    Params.Clear();
                    Params.Add(new FbParameterItem("NewId", NewId));
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
                            Obj = Row["I_ATASK_TASKID"];
                            NewTaskId = ((Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj));
                        }
                        WindowsEventLoggingUtilities.WriteToInfoEvent("NewTaskId: " + NewTaskId, GetModuleName(), 0);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After EATaskAddEditCreate Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string LoadAllPageEntriesRequestItem(ActionBase ActionBase,
                                                           ref AllPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadAllPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

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
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.TaskIdValue = Item.TaskIdValue;
                    TempItem.TaskTypeValue = Item.TaskTypeValue;
                    TempItem.ServiceIdValue = Item.ServiceIdValue;
                    TempItem.NotesIdValue = Item.NotesIdValue;
                    TempItem.OptionIdValue = Item.OptionIdValue;

                    string DefaultDateStr = "01/01/1900";
                    Object Obj = ActionBase.GetCollectionValue("Data", "Property");
                    TempItem.Property = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Client");
                    TempItem.Client = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Owner");
                    TempItem.Owner = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Date");
                    TempItem.Date = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "AssignedTo");
                    TempItem.AssignedToValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Insurer");
                    TempItem.Insurer = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Policy");
                    TempItem.Policy = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "PolicyNumber");
                    TempItem.PolicyNumber = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "RenewalDate");
                    TempItem.RenewalDate = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "Status");
                    TempItem.StatusValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Priority");
                    TempItem.PriorityValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Periodic");
                    TempItem.Periodic = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Advance");
                    TempItem.AdvanceValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Subject");
                    TempItem.Subject = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Notes");
                    TempItem.Notes = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "Tradesperson");
                    TempItem.TradespersonValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Quoting1");
                    TempItem.Quoting1Value = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Quoting2");
                    TempItem.Quoting2Value = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Quoting3");
                    TempItem.Quoting3Value = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "MaintenanceNotes");
                    TempItem.MaintenanceNotes = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string CopyRequestObject(AllPageEntriesRequestItem From,
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
                To.TaskIdValue = From.TaskIdValue;
                To.TaskTypeValue = From.TaskTypeValue;
                To.ServiceIdValue = From.ServiceIdValue;
                To.NotesIdValue = From.NotesIdValue;
                To.OptionIdValue = From.OptionIdValue;
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

        public static string CopyRequestObject(UpdateNoteRequestItem From,
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
                To.TaskIdValue = From.TaskIdValue;
                To.TaskTypeValue = From.TaskTypeValue;
                To.ServiceIdValue = From.ServiceIdValue;
                To.NotesIdValue = From.NotesIdValue;
                To.OptionIdValue = From.OptionIdValue;
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

        public static string CopyRequestObject(DisplayGridReportRequestItem From,
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
                To.TaskIdValue = From.TaskIdValue;
                To.TaskTypeValue = From.TaskTypeValue;
                To.ServiceIdValue = From.ServiceIdValue;
                To.NotesIdValue = From.NotesIdValue;
                To.OptionIdValue = From.OptionIdValue;
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

        public static string SaveChanges(string ConnectionString,
                                         Int64 CompanyId,
                                         Int64 ClientId,
                                         AllPageEntriesRequestItem RequestItem)
        {
            string Result = "";
            string Method = "SaveChanges";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = CopyRequestObject(RequestItem, ref Item);
                }

                StringBuilder Entries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, (int)CompanyId, (int)ClientId, Item, ref Entries);
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

                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Save query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = ExecuteSaveSql(ConnectionString, RequestItem, QueryText);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second Save query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = ExecuteSaveSql2(ConnectionString, RequestItem, QueryText);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateNotesSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update note query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = ExecuteUpdateNote(ConnectionString, RequestItem, QueryText);
                }

                /* DON'T Save the Maintenance Notes here */
                /*
                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateMaintenanceNotesSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update maintenance note query";
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Result = ExecuteUpdateMaintenanceNote(ConnectionString, RequestItem, QueryText);
                }
                */

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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After SaveChanges Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string ExecuteSaveSql(string ConnectionString,
                                            AllPageEntriesRequestItem Request,
                                            string QueryText)
        {
            string Result = "";
            string Method = "ExecuteSaveSql";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("notesid", Request.NotesId));
                    Params.Add(new FbParameterItem("option1", Request.OptionId));
                    Params.Add(new FbParameterItem("t1", Request.Tradesperson));
                    Params.Add(new FbParameterItem("q1", Request.Quoting1));
                    Params.Add(new FbParameterItem("q2", Request.Quoting2));
                    Params.Add(new FbParameterItem("q3", Request.Quoting3));
                    Params.Add(new FbParameterItem("B_ATASK_PERIODICAL", Request.Periodic));

                    Params.Add(new FbParameterItem("I_ATASK_ADVANCEBY", Request.Advance));
                    Params.Add(new FbParameterItem("TASKDATE", Request.Date));
                    Params.Add(new FbParameterItem("TASKSUBJECT", Request.Subject));
                    Params.Add(new FbParameterItem("PRIORITYONE", Request.Priority));
                    Params.Add(new FbParameterItem("TASKSTATUS", Request.Status));
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
            }

            return Result;
        }

        public static string ExecuteSaveSql2(string ConnectionString,
                                             AllPageEntriesRequestItem Request,
                                             string QueryText)
        {
            string Result = "";
            string Method = "ExecuteSaveSql2";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("AssignedToId", Request.AssignedTo));
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
            }

            return Result;
        }

        public static string ExecuteUpdateNote(string ConnectionString,
                                               AllPageEntriesRequestItem Request,
                                               string QueryText)
        {
            string Result = "";
            string Method = "ExecuteUpdateNote";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(Request.Notes);
                    Params.Add(new FbParameterItem("notepad", bytes));
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
            }

            return Result;
        }

        public static string ExecuteUpdateMaintenanceNote(string ConnectionString,
                                                          string NoteText,
                                                          string QueryText)
        {
            string Result = "";
            string Method = "ExecuteUpdateMaintenanceNote";
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

        public static string LoadUpdateNoteRequestItem(ActionBase ActionBase,
                                                       ref UpdateNoteRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadAllPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

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
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.TaskIdValue = Item.TaskIdValue;
                    TempItem.TaskTypeValue = Item.TaskTypeValue;
                    TempItem.ServiceIdValue = Item.ServiceIdValue;
                    TempItem.NotesIdValue = Item.NotesIdValue;
                    TempItem.OptionIdValue = Item.OptionIdValue;

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

        public static string UpdateNote(string ConnectionString,
                                        int CompanyId,
                                        int ClientId,
                                        UpdateNoteRequestItem Request)
        {
            string Result = "";
            string Method = "UpdateNote";
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
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateMaintenanceNotesSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update maintenance note query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = ExecuteUpdateMaintenanceNote(ConnectionString, Request.NoteText, QueryText);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateMaintenanceNotesSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(QueryText))
                    {
                        Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
                    }
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

        public static string GetMaintenanceNoteText(string ConnectionString,
                                                    string QueryText,
                                                    ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "GetMaintenanceNoteText";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                DataTable tbl = null;
                object Obj = null;
                string NoteText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, GetModuleName(), 0);
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row[0];
                            if (Obj != DBNull.Value)
                            {
                                byte[] byteArray = (byte[])Obj;
                                NoteText = System.Text.Encoding.Default.GetString(byteArray);
                            }
                        }
                        if (Entries.Length > 0) Entries.Append(",");
                        Entries.Append(JsonUtilities.AddPropertyNameAndValue("MaintenanceNotes", NoteText, false));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After GetMaintenanceNoteText Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string NotesChanged(string ConnectionString,
                                          int CompanyId,
                                          int ClientId,
                                          SetupPageEntriesRequestItem Request,
                                          ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "NotesChanged";
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

                string Str = "";
                string NotesQuery = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Str =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();
                    NotesQuery = JsonUtilities.GetJsonPathValueStr(Str, "$.MaintenanceNotesSQL", out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = GetMaintenanceNoteText(ConnectionString, NotesQuery, ref Entries);
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

        public static string Archive(string ConnectionString,
                                     int CompanyId,
                                     int ClientId,
                                     SetupPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = "Archive";
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

                string Str = "";
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Str =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();
                    QueryText = JsonUtilities.GetJsonPathValueStr(Str, "$.ArchiveSQL", out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After Archive Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string Recall(string ConnectionString,
                                    int CompanyId,
                                    int ClientId,
                                    SetupPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = "Recall";
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

                string Str = "";
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Str =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();
                    QueryText = JsonUtilities.GetJsonPathValueStr(Str, "$.RecallSQL", out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After Recall Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string Complete(string ConnectionString,
                                      int CompanyId,
                                      int ClientId,
                                      SetupPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + ".Complete";
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

                string Str = "";
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Str =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();
                    QueryText = JsonUtilities.GetJsonPathValueStr(Str, "$.CompleteSQL", out Result);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + QueryText, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After Complete Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string DeleteTask(string ConnectionString,
                                        int CompanyId,
                                        int ClientId,
                                        SetupPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = "DeleteTask";
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

                string Str = "";
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Str =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();
                    QueryText = JsonUtilities.GetJsonPathValueStr(Str, "$.DeleteCommand", out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After DeleteTask Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string DisplayPdfReport(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
                                              SetupPageEntriesRequestItem Request,
                                              ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "DisplayPdfReport";
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

                string ReportSource = "";
                if (string.IsNullOrEmpty(Result))
                {
                    ReportSource = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.B1Value", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(ReportSource)) Result = "Cannot determine the Report value";
                }

                string OutputFileName = "";
                if (string.IsNullOrEmpty(Result))
                {
                    long UniqueId = (long)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After DisplayPdfReport Result: " + Result, Method, 0);
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
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = Item.Load(ActionBase);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.TaskIdValue = Item.TaskIdValue;
                    TempItem.TaskTypeValue = Item.TaskTypeValue;
                    TempItem.ServiceIdValue = Item.ServiceIdValue;
                    TempItem.NotesIdValue = Item.NotesIdValue;
                    TempItem.OptionIdValue = Item.OptionIdValue;

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
                        CommonUtilities.SystemReportUtilities.Execute(
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
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = CopyRequestObject(Request, ref Item);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref Entries);
                }

                string Str =
                    JsonUtilities.StartJson() +
                    Entries.ToString() +
                    JsonUtilities.EndJson();

                WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, Method, 0);

                JObject JObj = null;
                try
                {
                    if (Entries.Length > 0)
                    {
                        JObj = JObject.Parse(Str);
                    }
                }
                catch (Exception Ex)
                {
                    ErrorMsg =
                        Method + Environment.NewLine +
                        Ex.Message + Environment.NewLine +
                        Ex.StackTrace;
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    string JPath = @"$.gridEntries[?(@.I_ID == '" + Request.UniqueId + "')]";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, Method, 0);
                    JToken Record = JObj.SelectToken(JPath);
                    if (Record == null)
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", Method, 0);
                    else
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), Method, 0);
                        Result = (string)Record["ZL20_B" + Request.ButtonId];
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
    }
}
