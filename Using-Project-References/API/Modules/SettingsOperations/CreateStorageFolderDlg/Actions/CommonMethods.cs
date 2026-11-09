using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.SettingsOperations.CreateStorageFolderDlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "SettingsOperations.CreateStorageFolderDlg."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "Name");
                TempItem.Name = (Obj == null) ? "" : Convert.ToString(Obj);
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
                        "SELECT OutputLine FROM EATypesStorageAddOnlyGroup(@CompanyId, @FolderName)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("FolderName", Request.Name));
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

        public static string SaveEntry(string ConnectionString,
                                       int CompanyId,
                                       int ClientId,
                                       SetupPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "SaveEntry";
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

                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.savesql", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Save query";
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("TaskTypeName", Request.Name));
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
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After SaveEntry Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
