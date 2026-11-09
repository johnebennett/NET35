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
namespace Actions.SettingsOperations.CompanyLogoDlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "SettingsOperations.CompanyLogoDlg."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "FileName");
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
                        "SELECT OutputLine FROM EALOGOADDGROUP(@CompanyId, @FileName, @LoginId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("FileName", Request.FileName));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupPageEntries Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string LoadUploadImageRequestItem(ActionBase ActionBase,
                                                        ref UploadImageRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadUploadImageRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                Result = Item.Load(ActionBase);
                if (string.IsNullOrEmpty(Result))
                {
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

        public static string CopyRequestObject(UploadImageRequestItem From,
                                               ref SetupPageEntriesRequestItem To)
        {
            string Result = "";
            string Method = "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                To.FileName = From.FileName;
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

        public static string UploadImage(string ConnectionString,
                                         int CompanyId,
                                         int ClientId,
                                         UploadImageRequestItem Request,
                                         List<string> FileList)
        {
            string Result = "";
            string Method = "UploadImage";
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
                                ActionBase.LogResponse(Method, Entries.ToString());
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
                            string Str =
                                "OutputFileName: " + OutputFileName + Environment.NewLine;
                            if (File.Exists(OutputFileName))
                            {
                                Str += "File Exists";
                            }
                            else
                            {
                                Str += "File does not Exist";
                            }

                            WindowsEventLoggingUtilities.WriteToInfoEvent(Str, Method, 0);
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
                                              UploadImageRequestItem Request,
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
                    QueryText = QueryText.Replace("\"", "'");
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
                    TestRPOLocation = CommonUtilities.FolderHelpers.TestRPOLocation(ConnectionString, CompanyId, out Result);
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
