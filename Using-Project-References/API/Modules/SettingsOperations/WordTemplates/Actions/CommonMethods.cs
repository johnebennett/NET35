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
namespace Actions.SettingsOperations.WordTemplates
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "SettingsOperations.WordTemplates."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "DisplayCategoryId");
                TempItem.DisplayCategoryIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "OptionsId");
                TempItem.OptionsIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TemplateId");
                TempItem.TemplateIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "FileOptionsId");
                TempItem.FileOptionsIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DisplayCategory2Id");
                TempItem.DisplayCategory2IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
                    QueryText =
                        "SELECT OutputLine FROM EAWordTemplatesAddEditGroup(" +
                        "@CompanyId, @DisplayCategoryId, @OptionsId, @TemplateId, @FileOptions, @DisplayCategory2Id)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("DisplayCategoryId", Request.DisplayCategoryId));
                    Params.Add(new FbParameterItem("OptionsId", Request.OptionsId));
                    Params.Add(new FbParameterItem("TemplateId", Request.TemplateId));
                    Params.Add(new FbParameterItem("FileOptions", Request.FileOptionsId));
                    Params.Add(new FbParameterItem("DisplayCategory2Id", Request.DisplayCategory2Id));
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

                /*
                string Str = "";
                string NoteSql = "";
                string BodyText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Str =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        JsonUtilities.EndJson();
                    NoteSql = JsonUtilities.GetJsonPathValueStr(Str, "$.GetNoteSQL", out Result);
                    //if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(NoteSql)) Result = "Cannot find the Notes entry";
                    if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(NoteSql))
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("NoteSql:" + Environment.NewLine + NoteSql, Method, 0);
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
                                    BodyText = System.Text.Encoding.Default.GetString(byteArray);

                                }
                            }
                            if (Entries.Length > 0) Entries.Append(",");
                            Entries.Append(JsonUtilities.AddPropertyNameAndValue("BodyText", BodyText, false));
                        }
                    }
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupPageEntries Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string LoadCreateEntryRequestItem(ActionBase ActionBase,
                                                        ref CreateEntryRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadCreateEntryRequestItem";
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

                    TempItem.DisplayCategoryIdValue = Item.DisplayCategoryIdValue;
                    TempItem.OptionsIdValue = Item.OptionsIdValue;
                    TempItem.TemplateIdValue = Item.TemplateIdValue;
                    TempItem.FileOptionsIdValue = Item.FileOptionsIdValue;
                    TempItem.DisplayCategory2IdValue = Item.DisplayCategory2IdValue;

                    Object Obj = ActionBase.GetCollectionValue("Data", "TemplateName");
                    TempItem.TemplateName = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string CopyRequestObject(CreateEntryRequestItem From,
                                                ref SetupPageEntriesRequestItem To)
        {
            string Result = "";
            string Method = GetModuleName() + "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.DisplayCategoryIdValue = From.DisplayCategoryIdValue;
                To.OptionsIdValue = From.OptionsIdValue;
                To.TemplateIdValue = From.TemplateIdValue;
                To.FileOptionsIdValue = From.FileOptionsIdValue;
                To.DisplayCategory2IdValue = From.DisplayCategory2IdValue;
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

        public static string CopyRequestObject(UpdateEntryRequestItem From,
                                                ref SetupPageEntriesRequestItem To)
        {
            string Result = "";
            string Method = GetModuleName() + "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.DisplayCategoryIdValue = From.DisplayCategoryIdValue;
                To.OptionsIdValue = From.OptionsIdValue;
                To.TemplateIdValue = From.TemplateIdValue;
                To.FileOptionsIdValue = From.FileOptionsIdValue;
                To.DisplayCategory2IdValue = From.DisplayCategory2IdValue;
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

        public static string CopyRequestObject(UploadTemplateRequestItem From,
                                                ref SetupPageEntriesRequestItem To)
        {
            string Result = "";
            string Method = GetModuleName() + "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.DisplayCategoryIdValue = From.DisplayCategoryIdValue;
                To.OptionsIdValue = From.OptionsIdValue;
                To.TemplateIdValue = From.TemplateIdValue;
                To.FileOptionsIdValue = From.FileOptionsIdValue;
                To.DisplayCategory2IdValue = From.DisplayCategory2IdValue;
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

        public static string CreateEntry(string ConnectionString,
                                         int CompanyId,
                                         int ClientId,
                                         CreateEntryRequestItem Request,
                                         ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "CreateEntry";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            List<FbParameterItem> Params = new List<FbParameterItem>();

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem SetupItem = new SetupPageEntriesRequestItem();
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    if (string.IsNullOrEmpty(Result))
                    {
                        Result = CopyRequestObject(Request, ref SetupItem);
                    }
                    if (string.IsNullOrEmpty(Result))
                    {
                        Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, SetupItem, ref TempEntries);
                    }
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
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateSQL1", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Create query";
                }

                Int64 NewWordGenId = 0;
                if (string.IsNullOrEmpty(Result))
                {
                    Result = GetNewWordGenId(ConnectionString, QueryText, out NewWordGenId);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second Create query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Params.Clear();
                    Params.Add(new FbParameterItem("wordid", NewWordGenId));
                    Params.Add(new FbParameterItem("createname", Request.TemplateName));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                    if (string.IsNullOrEmpty(Result))
                        Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                Int64 NewStorageGenId = 0;
                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateSQL3", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third Create query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = GetNewStorageGenId(ConnectionString, QueryText, out NewStorageGenId);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateSQL4", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the fourth Create query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
                    QueryText = QueryText.Replace("++", "\\");
                    Params.Clear();
                    Params.Add(new FbParameterItem("STORAGEID", NewStorageGenId));
                    Params.Add(new FbParameterItem("wordid", NewWordGenId));
                    //Params.Add(new FbParameterItem("filename", Request.TemplateId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                    if (string.IsNullOrEmpty(Result))
                        Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateSQL5", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the fifth Create query";
                }

                string CommandLine = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Clear();
                    Params.Add(new FbParameterItem("StorageId", NewStorageGenId));
                    Result = GetCommandLineFromQuery(ConnectionString, QueryText, Params, out CommandLine);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = ExecuteCommandLine(ConnectionString, CompanyId, CommandLine);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, SetupItem, ref Entries);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
                    UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, SetupItem, ref Entries);
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

        public static string GetNewWordGenId(string ConnectionString,
                                            string QueryText,
                                            out Int64 NewWordGenId)
        {
            NewWordGenId = 0;
            string Result = "";
            string Method = GetModuleName() + "GetNewWordGenId";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row[0];
                            NewWordGenId =  (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
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

        public static string GetNewStorageGenId(string ConnectionString,
                                                string QueryText,
                                                out Int64 NewStorageGenId)
        {
            NewStorageGenId = 0;
            string Result = "";
            string Method = GetModuleName() + "GetNewStorageGenId";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row[0];
                            NewStorageGenId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
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

        public static string GetCommandLineFromQuery(string ConnectionString, 
                                                     string QueryText,
                                                     List<FbParameterItem> Params,
                                                     out string CommandLine)
        {
            CommandLine = "";
            string Result = "";
            string Method = GetModuleName() + "GetCommandLineFromQuery";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
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
                            Obj = Row[0];
                            CommandLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
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

        public static string ExecuteCommandLine(string ConnectionString,
                                                Int64 CompanyId, 
                                                string CommandLine)
        {
            string Result = "";
            string Method = GetModuleName() + "ExecuteCommandLine";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string TestRPOLocation = global::UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(ConnectionString, CompanyId, out Result);
                if (string.IsNullOrEmpty(Result))
                {
                    Result = UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(CommandLine, TestRPOLocation, 1000);
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

        public static string DownloadDocument(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
                                              SetupPageEntriesRequestItem Request,
                                              ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "DownloadDocument";
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
                    ReportSource = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.DownLoadFileSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(ReportSource)) Result = "Cannot find the Report Source";
                }

                string OutputFileName = "";
                if (string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("ReportSource: " + ReportSource, Method, 0);
                    long UniqueId = (long)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
                    Int64 NewId =
                        global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
                            ConnectionString, CompanyId, ClientId,
                            UniqueId, ReportSource, out Result, out OutputFileName);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName: " + OutputFileName, Method, 0);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After DownloadDocument Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string LoadUpdateEntryRequestItem(ActionBase ActionBase,
                                                        ref UpdateEntryRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadUpdateEntryRequestItem";
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

                    TempItem.DisplayCategoryIdValue = Item.DisplayCategoryIdValue;
                    TempItem.OptionsIdValue = Item.OptionsIdValue;
                    TempItem.TemplateIdValue = Item.TemplateIdValue;
                    TempItem.FileOptionsIdValue = Item.FileOptionsIdValue;
                    TempItem.DisplayCategory2IdValue = Item.DisplayCategory2IdValue;

                    Object Obj = ActionBase.GetCollectionValue("Data", "TemplateName");
                    TempItem.TemplateName = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Hide");
                    TempItem.Hide = (Obj == null) ? "" : Convert.ToString(Obj);
                    WindowsEventLoggingUtilities.WriteToInfoEvent(
                        "After LoadUpdateEntryRequestItem, DisplayCategory2IdValue: " + TempItem.DisplayCategory2IdValue,
                        "Look Here John", 0);
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

        public static string UpdateEntry(string ConnectionString,
                                         int CompanyId,
                                         int ClientId,
                                         UpdateEntryRequestItem Request,
                                         ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "UpdateEntry";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem SetupItem = new SetupPageEntriesRequestItem();
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    if (string.IsNullOrEmpty(Result))
                    {
                        Result = CopyRequestObject(Request, ref SetupItem);
                    }
                    if (string.IsNullOrEmpty(Result))
                    {
                        Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, SetupItem, ref TempEntries);
                    }
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
                    //Params.Add(new FbParameterItem("templatecategory", Request.DisplayCategoryId));
                    Params.Add(new FbParameterItem("templatecategory", Request.DisplayCategory2Id));
                    Params.Add(new FbParameterItem("TemplateName", Request.TemplateName));
                    Params.Add(new FbParameterItem("hide", Request.Hide));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
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
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, SetupItem, ref Entries);
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

        public static string LoadUploadTemplateRequestItem(ActionBase ActionBase,
                                                        ref UploadTemplateRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadUploadTemplateRequestItem";
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

                    TempItem.DisplayCategoryIdValue = Item.DisplayCategoryIdValue;
                    TempItem.OptionsIdValue = Item.OptionsIdValue;
                    TempItem.TemplateIdValue = Item.TemplateIdValue;
                    TempItem.FileOptionsIdValue = Item.FileOptionsIdValue;
                    TempItem.DisplayCategory2IdValue = Item.DisplayCategory2IdValue;

                    Object Obj = ActionBase.GetCollectionValue("Data", "FileSize");
                    TempItem.FileSizeValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string ProcessUploadFilesList(string ConnectionString,
                                                    int CompanyId,
                                                    int ClientId,
                                                    UploadTemplateRequestItem Request,
                                                    List<string> FileList)
        {
            string Result = "";
            string Method = GetModuleName() + "ProcessUploadFilesList";
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

                if (string.IsNullOrEmpty((Result)))
                {
                    StringBuilder Entries = new StringBuilder();
                    string Value = "";
                    JObject JSONSource = null;
                    string QueryText = "";
                    Int64 NewId = 0;
                    string CommandLine = "";
                    string OutputFileName = "";
                    foreach (string FileItem in FileList)
                    {
                        Entries.Length = 0;
                        /*
                        Item.FileName = FileItem;
                        Request.FileName = FileItem;
                        */
                        long FileSize = new System.IO.FileInfo(FileItem).Length;
                        Request.FileSize = FileSize;

                        if (string.IsNullOrEmpty(Result))
                            Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref Entries);

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

                        if (string.IsNullOrEmpty(Result))
                        {
                            NewId = 0;
                            if (string.IsNullOrEmpty(Result))
                            {
                                QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpLoadSQL1", out Result);
                                if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Upload query";
                            }

                            if (string.IsNullOrEmpty(Result)) Result = ProcessUploadSql1(ConnectionString, QueryText, out NewId);
                            if (string.IsNullOrEmpty(Result) && (NewId == 0)) Result = "Invalid value for NewId";
                        }

                        if (string.IsNullOrEmpty(Result))
                        {

                            if (string.IsNullOrEmpty(Result))
                            {
                                QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpLoadSQL2", out Result);
                                if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second Upload query";
                            }

                            if (string.IsNullOrEmpty(Result))
                                Result = ProcessUploadSql2(ConnectionString, QueryText, NewId, FileItem);
                        }

                        if (string.IsNullOrEmpty(Result))
                        {

                            QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpLoadSQL3", out Result);
                            if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third Update query";
                        }

                        CommandLine = "";
                        if (string.IsNullOrEmpty(Result))
                        {
                            Result = ProcessUploadSql3(ConnectionString, CompanyId, QueryText, NewId, out CommandLine);
                        }

                        CommandLine = "";
                        if (string.IsNullOrEmpty(Result))
                        {

                            CommandLine = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpLoadSQL4", out Result);
                            if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the fourth Update query";
                        }

                        if (string.IsNullOrEmpty(Result))
                        {
                            Result = ProcessUploadSql4(ConnectionString, CompanyId, CommandLine);
                        }

                        if (string.IsNullOrEmpty(Result))
                        {
                            File.Delete(FileItem);
                            if (File.Exists(FileItem)) Result = "Did not delete the OutputFileName";
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }

            return Result;
        }

        private static string ProcessUploadSql1(string ConnectionString,
                                                string QueryText,
                                                out Int64 NewId)
        {
            string Result = "";
            string Method = GetModuleName() + "ProcessUploadSql1";
            NewId = 0;
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl = UsefulDBMethods.Helpers.GetFirstDataTableEx(ConnectionString, QueryText, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row[0];
                            NewId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
                        }
                        if (NewId == 0) Result = "Invalid generation of Record Id";
                        WindowsEventLoggingUtilities.WriteToInfoEvent("NewStorageId: " + NewId, Method, 0);
                    }
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }

            return Result;
        }

        private static string ProcessUploadSql2(string ConnectionString,
                                                string QueryText,
                                                Int64 NewId,
                                                string FileName)
        {
            string Result = "";
            string Method = GetModuleName() + "ProcessUploadSql2";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("StorageId", NewId));
                    Params.Add(new FbParameterItem("FileName", FileName));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
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

            return Result;
        }

        private static string ProcessUploadSql3(string ConnectionString,
                                               Int64 CompanyId,
                                               string QueryText,
                                               Int64 NewId,
                                               out string CommandLine)
        {
            string Result = "";
            string Method = GetModuleName() + "ProcessUploadSql3";
            CommandLine = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string TestRPOLocation = "";
                if (string.IsNullOrEmpty(Result))
                {
                    TestRPOLocation = global::UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(ConnectionString, CompanyId, out Result);
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("StorageId", NewId));
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
                            Obj = Row["STORAGE_COMMANDLINE"];
                            CommandLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        }
                    }
                }

                if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(CommandLine))
                    Result = "Cannot determine the CommandLine to process";

                WindowsEventLoggingUtilities.WriteToInfoEvent("ComamndLine: " + CommandLine, Method, 0);

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(
                            CommandLine, TestRPOLocation, 1000);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }

            return Result;
        }

        /* Result = ProcessUploadSql4(ConnectionString, CompanyId, CommandLine); */
        private static string ProcessUploadSql4(string ConnectionString,
                                               Int64 CompanyId,
                                               string CommandLine)
        {
            string Result = "";
            string Method = GetModuleName() + "ProcessUploadSql4";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string TestRPOLocation = "";
                if (string.IsNullOrEmpty(Result))
                {
                    TestRPOLocation = global::UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(ConnectionString, CompanyId, out Result);
                }

                WindowsEventLoggingUtilities.WriteToInfoEvent("ComamndLine: " + CommandLine, Method, 0);

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(
                            CommandLine, TestRPOLocation, 1000);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }

            return Result;
        }
    }
}
