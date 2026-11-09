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
namespace Actions.Portfolio.AddStorageDlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "Portfolio.AddStorageDlg"; }

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
                Obj = ActionBase.GetCollectionValue("Data", "StorageType");
                TempItem.StorageTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "FileName");
                TempItem.FileName = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "ItemType");
                TempItem.ItemTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TextValue");
                TempItem.TextValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
                        "SELECT OutputLine FROM EAStorageAddGroup(@CompanyId, @TransactionId, @StorageType, @FileName, " +
                        "@LoginId, @ItemType, @Description, @TextValue)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
                    Params.Add(new FbParameterItem("StorageType", Request.StorageType));
                    Params.Add(new FbParameterItem("FileName", Request.FileName));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    Params.Add(new FbParameterItem("ItemType", Request.ItemType));
                    Params.Add(new FbParameterItem("TextValue", Request.TextValue));
                    Params.Add(new FbParameterItem("Description", Request.Description));
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

        public static string LoadUploadFilesRequestItem(ActionBase ActionBase,
                                                        ref UploadFilesRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadUploadFilesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                Result = Item.Load(ActionBase);
                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.StorageTypeValue = Item.StorageTypeValue;
                    TempItem.FileName = Item.FileName;

                    Object Obj = ActionBase.GetCollectionValue("Data", "destination");
                    TempItem.Destination = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "splitchar");
                    TempItem.SplitChar = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "useLast");
                    TempItem.UseLastValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Source");
                    TempItem.Source = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "FileSize");
                    TempItem.FileSizeValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "ItemType");
                    TempItem.ItemTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string CopyRequestObject(UploadFilesRequestItem From,
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
                To.StorageTypeValue = From.StorageTypeValue;
                To.FileName = From.FileName;
                To.ItemTypeValue = From.ItemTypeValue;
                To.TextValue = From.TextValue;
                To.Description = From.Description;
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

        public static string ProcessUploadFilesList(string ConnectionString,
                                                    int CompanyId,
                                                    int ClientId,
                                                    UploadFilesRequestItem Request,
                                                    List<string> FileList)
        {
            string Result = "";
            string Method = "ProcessUploadFilesList";
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
                    int NewId = 0;
                    string CommandLine = "";
                    string OutputFileName = "";
                    foreach (string FileItem in FileList)
                    {
                        Entries.Length = 0;
                        Item.FileName = FileItem;
                        Request.FileName = FileItem;
                        long FileSize = new System.IO.FileInfo(Request.FileName).Length;
                        Request.FileSize = FileSize;

                        /* Files are already in the drop locaion 
                        if (string.IsNullOrEmpty(Result))
                        {
                            Result = CopyFilesToDropLocation(CompanyId, ClientId, ref Request);
                        }
                        */

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
                                QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL1", out Result);
                                if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first save query";
                                if (string.IsNullOrEmpty(Result))
                                    WindowsEventLoggingUtilities.WriteToInfoEvent(
                                        "SaveSQL1: " + Environment.NewLine + QueryText, GetModuleName(), 0);
                            }

                            if (string.IsNullOrEmpty(Result)) Result = ProcessSaveSql1(ConnectionString, QueryText, out NewId);
                            if (string.IsNullOrEmpty(Result) && (NewId == 0)) Result = "Invalid value for NewId";
                        }

                        if (string.IsNullOrEmpty(Result))
                        {

                            if (string.IsNullOrEmpty(Result))
                            {
                                QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL2", out Result);
                                if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second save query";
                                if (string.IsNullOrEmpty(Result))
                                    WindowsEventLoggingUtilities.WriteToInfoEvent(
                                        "SaveSQL2: " + Environment.NewLine + QueryText, GetModuleName(), 0);
                            }

                            if (string.IsNullOrEmpty(Result))
                                Result =
                                ProcessSaveSql2(ConnectionString, CompanyId, ClientId, QueryText, Request, NewId, out OutputFileName);
                        }


                        if (string.IsNullOrEmpty(Result))
                        {

                            if (string.IsNullOrEmpty(Result))
                            {
                                QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL3", out Result);
                                if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third save query";
                                if (string.IsNullOrEmpty(Result))
                                    WindowsEventLoggingUtilities.WriteToInfoEvent(
                                        "SaveSQL3: " + Environment.NewLine + QueryText, GetModuleName(), 0);
                            }
                        }

                        CommandLine = "";
                        if (string.IsNullOrEmpty(Result))
                        {
                            Result = ProcessSaveSql3(ConnectionString, CompanyId, QueryText, NewId, out CommandLine);
                        }

                        if (string.IsNullOrEmpty(Result))
                        {
                            WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName: " + OutputFileName, Method, 0);
                            File.Delete(OutputFileName);
                            if (File.Exists(OutputFileName)) Result = "Did not delete the OutputFileName";
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

        private static string ProcessSaveSql1(string ConnectionString,
                                              string QueryText,
                                              out int NewId)
        {
            string Result = "";
            string Method = GetModuleName() + ".ProcessSaveSql1";
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
                            Obj = Row["ID"];
                            NewId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                        }
                        if (NewId == 0) Result = "Invalid generation of Record Id";
                    }
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }

            return Result;
        }

        private static string ProcessSaveSql2(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
                                              string QueryText,
                                              UploadFilesRequestItem Request,
                                              int NewId,
                                              out string OutputFileName)
        {
            string Result = "";
            string Method = GetModuleName() + ".ProcessSaveSql2";
            OutputFileName = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string FileName = "";
                string FileNamePrefix = "";
                //string OutputFileName = "";
                string OutputDir = "";
                string DirectorySepChar = Path.DirectorySeparatorChar.ToString();
                if (string.IsNullOrEmpty(Result))
                {
                    FileName = Request.FileName;
                    FileNamePrefix =
                        CompanyId.ToString() + "_" +
                        ClientId.ToString() + "_" +
                        Request.Source + "_";
                    OutputFileName = Request.FileName;
                    OutputFileName = Path.GetFileName(OutputFileName);
                    if (OutputFileName.StartsWith(FileNamePrefix)) OutputFileName = OutputFileName.Replace(FileNamePrefix, "");
                    OutputDir = Request.Destination;
                    OutputDir = OutputDir.Replace("@", "\\");
                    if (!OutputDir.EndsWith(DirectorySepChar)) OutputDir += DirectorySepChar;
                    OutputFileName = OutputDir + OutputFileName;
                    WindowsEventLoggingUtilities.WriteToInfoEvent(
                        "FileName: " + FileName + Environment.NewLine +
                        "OutputFileName: " + OutputFileName, Method, 0);
                    File.Move(FileName, OutputFileName);
                    if (!File.Exists(OutputFileName)) Result = "Failed to rename the file before inserting";
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("ID", NewId));
                    Params.Add(new FbParameterItem("FileName", OutputFileName));
                    Params.Add(new FbParameterItem("FileSize", Request.FileSize));
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

            return Result;
        }

        private static string ProcessSaveSql3(string ConnectionString,
                                              int CompanyId,
                                              string QueryText,
                                              int NewId,
                                              out string CommandLine)
        {
            string Result = "";
            string Method = GetModuleName() + ".ProcessSaveSql3";
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
                    Params.Add(new FbParameterItem("ID", NewId));
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

        public static string LoadRemoveGridEntryRequestItem(ActionBase ActionBase,
                                                            ref RemoveGridEntryRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadRemoveGridEntryRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {

                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                Result = Item.Load(ActionBase);
                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.StorageTypeValue = Item.StorageTypeValue;
                    TempItem.FileName = Item.FileName;

                    Object Obj = ActionBase.GetCollectionValue("Data", "Option1Id");
                    TempItem.Option1Value = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "GridId");
                    TempItem.GridIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
                                             RemoveGridEntryRequestItem Request)
        {
            string Result = "";
            string Method = "RemoveGridEntry";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder Entries = new StringBuilder();
                Result =
                    Portfolio.CommonMethods.EAResComSalesGroup(
                        ConnectionString, CompanyId, ClientId, Request.PortfolioId, 0, Request.Option1, Request.StorageType,
                        ref Entries);

                string Source = "";
                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Source =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        JsonUtilities.EndJson();
                }

                JObject JSONSource = null;
                if (string.IsNullOrEmpty(Result))
                {
                    JSONSource = JObject.Parse(Source);
                    if (JSONSource == null) Result = "Cannot parse the Group source";
                    Source = "";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    string JPath = @"$.grid[?(@.I_ID == '" + Request.GridId + "')]";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, Method, 0);
                    JToken Record = JSONSource.SelectToken(JPath);
                    if (Record == null)
                    {
                        Result = "Record IS NULL";
                        WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, 0);
                    }
                    else
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), Method, 0);
                        QueryText = (string)Record["ZC5_Show_TrashSQL"];
                        if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText))
                        {
                            Result = "Cannot find the Trash entry";
                            WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
                        }
                    }
                }


                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
                    WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + QueryText, Method, 0);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After RemoveGridEntry Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string AddTextString(string ConnectionString,
                                           int CompanyId,
                                           int ClientId,
                                           SetupPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = "AddTextString";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder Entries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref Entries);

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
                Int64 NewId = 0;
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL1", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first save query";
                    if (string.IsNullOrEmpty(Result))
                        WindowsEventLoggingUtilities.WriteToInfoEvent(
                            "SaveSQL1: " + Environment.NewLine + QueryText, GetModuleName(), 0);

                    if (string.IsNullOrEmpty(Result)) Result = Process_AddTextString_SaveSql1(ConnectionString, QueryText, out NewId);
                    if (string.IsNullOrEmpty(Result) && (NewId == 0)) Result = "Invalid value for NewId";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second save query";
                    if (string.IsNullOrEmpty(Result))
                        WindowsEventLoggingUtilities.WriteToInfoEvent(
                            "SaveSQL2: " + Environment.NewLine + QueryText, GetModuleName(), 0);

                    if (string.IsNullOrEmpty(Result))
                        Result =
                            Process_AddTextString_SaveSql2(ConnectionString, QueryText, Request, NewId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL3", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third save query";
                    if (string.IsNullOrEmpty(Result))
                        WindowsEventLoggingUtilities.WriteToInfoEvent(
                            "SaveSQL3: " + Environment.NewLine + QueryText, GetModuleName(), 0);
                    if (string.IsNullOrEmpty(Result))
                    {
                        Result = Process_AddTextString_SaveSql3(ConnectionString, CompanyId, QueryText, NewId);
                    }
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }

            return Result;
        }

        private static string Process_AddTextString_SaveSql1(string ConnectionString,
                                                             string QueryText,
                                                             out Int64 NewId)
        {
            string Result = "";
            string Method = GetModuleName() + ".Process_AddTextString_SaveSql1";
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
                            Obj = Row["ID"];
                            NewId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
                            WindowsEventLoggingUtilities.WriteToInfoEvent("NewId: " + NewId, Method, 0);
                        }
                    }

                    if (NewId == 0) Result = "Invalid generation of Record Id";
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }

            return Result;
        }

        private static string Process_AddTextString_SaveSql2(string ConnectionString,
                                                             string QueryText,
                                                             SetupPageEntriesRequestItem Request,
                                                             Int64 NewId)
        {
            string Result = "";
            string Method = GetModuleName() + ".Process_AddTextString_SaveSql2";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("ID", NewId));
                    Params.Add(new FbParameterItem("UrlText", Request.TextValue));
                    Params.Add(new FbParameterItem("UrlDescription", Request.Description));
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
            return Result;
        }

        private static string Process_AddTextString_SaveSql3(string ConnectionString,
                                                             int CompanyId,
                                                             string QueryText,
                                                             Int64 NewId)
        {
            string Result = "";
            string Method = GetModuleName() + ".Process_AddTextString_SaveSql3";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
            string CommandLine = "";

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
                    Params.Add(new FbParameterItem("ID", NewId));
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
    }
}