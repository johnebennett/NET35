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
namespace Actions.Portfolio.ADLFormsDialog
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "Portfolio.ADLFormsDialog"; }

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
                Obj = ActionBase.GetCollectionValue("Data", "ClientListId");
                TempItem.ClientListIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
                        "SELECT OutputLine FROM EAADLIconGroup(@CompanyId, @TransactionId, @ClientId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
                    Params.Add(new FbParameterItem("ClientId", Request.ClientListId));
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

        public static string PrepareDocument(string ConnectionString,
                                             int CompanyId,
                                             int ClientId,
                                             SetupPageEntriesRequestItem Request,
                                             ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "PrepareDocument";
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
                Int32 StorageId = 0;
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PrepareSQL1", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first query";
                    if (string.IsNullOrEmpty(Result))
                        Result = ExecutePrepareSQL1(ConnectionString, QueryText, out StorageId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PrepareSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second query";
                    if (string.IsNullOrEmpty(Result))
                        Result = ExecutePrepareSQL2(ConnectionString, QueryText, StorageId);
                }

                string CommandLine = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PrepareSQL3", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third query";
                    if (string.IsNullOrEmpty(Result))
                        Result = ExecutePrepareSQL3(ConnectionString, QueryText, StorageId, out CommandLine);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(CommandLine))
                        Result = "Cannot find the command to generate ADL Content";
                }

                string TestRPOLocation = "";
                if (string.IsNullOrEmpty(Result))
                {
                    TestRPOLocation = CommonUtilities.FolderHelpers.TestRPOLocation(ConnectionString, CompanyId, out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(TestRPOLocation))
                        Result = "Cannot find the command start location";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(CommandLine, TestRPOLocation, 1000);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PrepareSQL4", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the fourth query";
                    if (string.IsNullOrEmpty(Result))
                        Result = ExecutePrepareSQL4(ConnectionString, QueryText, CompanyId, ClientId, StorageId, ref Entries);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After PrepareDocument Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string ExecutePrepareSQL1(string ConnectionString,
                                                string QueryText,
                                                out int GenId)
        {
            string Result = "";
            string Method = "ExecutePrepareSQL1";
            GenId = 0;
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
                            Obj = Row["GEN_ID"];
                            GenId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ExecutePrepareSQL1 Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string ExecutePrepareSQL2(string ConnectionString,
                                                string QueryText,
                                                int StorageId)
        {
            string Result = "";
            string Method = "ExecutePrepareSQL2";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
                    Params.Add(new FbParameterItem("STORAGEID", StorageId));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ExecutePrepareSQL2 Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string ExecutePrepareSQL3(string ConnectionString,
                                                string QueryText,
                                                int StorageId,
                                                out string CommandLine)
        {
            string Result = "";
            string Method = "ExecutePrepareSQL3";
            CommandLine = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
                    Params.Add(new FbParameterItem("STORAGEID", StorageId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                }

                if (string.IsNullOrEmpty(Result))
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
                                Obj = Row["STORAGE_COMMANDLINE"];
                                CommandLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            }
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ExecutePrepareSQL3 Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string ExecutePrepareSQL4(string ConnectionString,
                                                string QueryText,
                                                int CompanyId, 
                                                int ClientId,
                                                int StorageId,
                                                ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "ExecutePrepareSQL4";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string DocsLocation = "";
                if (string.IsNullOrEmpty(Result))
                {
                    DocsLocation = CommonUtilities.FolderHelpers.DocsLocation(ConnectionString, CompanyId, out Result);
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("STORAGEID", StorageId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                }

                string FileName = "";
                string FileNameContent = "";
                if (string.IsNullOrEmpty(Result))
                {
                    DataTable tbl = null;
                    object Obj = null;
                    if (string.IsNullOrEmpty(Result))
                    {
                        tbl =
                            UsefulDBMethods.Helpers.GetFirstDataTableEx(
                                ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                        if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
                        if (string.IsNullOrEmpty(Result))
                        {
                            foreach (DataRow Row in tbl.Rows)
                            {
                                Obj = Row["EATAKE_FileNameNew"];
                                FileName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                                Obj = Row["EATAKE_BASE64"];
                                //Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
                                byte[] bytes = (byte[])Obj;
                                FileNameContent = Encoding.Default.GetString(bytes);
                            }

                            if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(FileName))
                            {
                                Result = "Cannot generate the output filename";
                            }

                            if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(FileNameContent))
                            {
                                Result = "No content to save to the output file";
                            }

                            string OutputFileName = "";
                            if (string.IsNullOrEmpty(Result))
                            {
                                OutputFileName = DocsLocation + FileName;
                                File.WriteAllBytes(OutputFileName, Convert.FromBase64String(FileNameContent));
                                if (!File.Exists(OutputFileName)) Result = "The output file does not exist";
                            }

                            if (string.IsNullOrEmpty(Result))
                            {
                                Entries.AppendLine(JsonUtilities.StartJsonObject("OutputFile"));
                                Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("file", OutputFileName));
                                Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileName", @"\Docs\" + FileName, false));
                                Entries.AppendLine(JsonUtilities.EndJsonObject());
                            }
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ExecutePrepareSQL4 Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }
    }
}
