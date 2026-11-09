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
namespace Actions.SettingsOperations.SignatureDlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "SettingsOperations.SignatureDlg."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "ClientId");
                TempItem.ClientIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "SignatureSource");
                TempItem.SignatureSourceValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "FileName");
                TempItem.FileName = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "FileSize");
                TempItem.FileSizeValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Source");
                TempItem.Source = (Obj == null) ? "" : Convert.ToString(Obj);
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
                    if (Request.SignatureSource == 1) // WordSignature
                        QueryText = "SELECT OutputLine FROM EASIGNATUREWORDADD(@CompanyId, @ClientId, @FileName, @LoginId)";
                    else if (Request.SignatureSource == 2) // EmailSignature
                        QueryText = "SELECT OutputLine FROM EASIGNATUREEMAILADD(@CompanyId, @ClientId, @FileName, @LoginId)";

                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("ClientId", Request.ClientId));
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

        public static string UploadSignature(string ConnectionString,
                                             int CompanyId,
                                             int ClientId,
                                             SetupPageEntriesRequestItem Request,
                                             string SignatureSource,
                                             string DestDir,
                                             List<string> SrcFileList,
                                             List<string> DestFileList)
        {
            string Result = "";
            string Method = "UploadSignature";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                if (string.IsNullOrEmpty((Result)))
                {
                    StringBuilder Entries = new StringBuilder();
                    string Value = "";
                    JObject JSONSource = null;
                    string QueryText = "";
                    int NewId = 0;
                    foreach (string FileItem in DestFileList)
                    {
                        Entries.Length = 0;
                        Request.FileName = FileItem;
                        Request.FileName = FileItem;
                        long FileSize = new System.IO.FileInfo(Request.FileName).Length;
                        Request.FileSize = FileSize;

                        if (string.IsNullOrEmpty(Result))
                            Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref Entries);

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
                                ProcessSaveSql2(
                                    ConnectionString, CompanyId, ClientId, QueryText, Request,
                                    SignatureSource, DestDir, NewId, FileItem);
                        }

                        if (string.IsNullOrEmpty(Result))
                        {
                            Result = ProcessUploadCommandLine(ConnectionString, CompanyId, NewId);
                        }

                        if (string.IsNullOrEmpty(Result))
                        {
                            WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName: " + FileItem, Method, 0);
                            File.Delete(FileItem);
                            if (File.Exists(FileItem)) Result = "Did not delete the FileItem";
                        }
                    }
                }

                foreach (string FileItem in DestFileList)
                {
                    if (File.Exists(FileItem)) File.Delete(FileItem);
                }

                foreach (string FileItem in SrcFileList)
                {
                    if (File.Exists(FileItem)) File.Delete(FileItem);
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
                                              SetupPageEntriesRequestItem Request,
                                              string SignatureSource,
                                              string Destination,
                                              int NewId,
                                              string FileItem)
        {
            string Result = "";
            string Method = GetModuleName() + ".ProcessSaveSql2";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("\"", "'");
                    QueryText = QueryText.Replace("__", "'");
                    Params.Add(new FbParameterItem("ID", NewId));
                    //Params.Add(new FbParameterItem("FileName", OutputFileName));
                    Params.Add(new FbParameterItem("FileName", FileItem));
                    //Params.Add(new FbParameterItem("FileSize", Request.FileSize));
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

        private static string ProcessUploadCommandLine(string ConnectionString,
                                                       int CompanyId, 
                                                       int NewId)
        {
            string Result = "";
            string Method = GetModuleName() + ".ProcessUploadCommandLine";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string TestRPOLocation = "";
                if (string.IsNullOrEmpty(Result))
                {
                    if (string.IsNullOrEmpty(Result))
                    {
                        TestRPOLocation = global::UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(ConnectionString, CompanyId, out Result);
                    }
                }
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "SELECT Storage_CommandLine AS CmdLine FROM EAStorageAdd WHERE I_Storage_ID = @NewId";
                    Params.Add(new FbParameterItem("NewId", NewId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                string CommandLine = "";
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
                            Obj = Row["CmdLine"];
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
