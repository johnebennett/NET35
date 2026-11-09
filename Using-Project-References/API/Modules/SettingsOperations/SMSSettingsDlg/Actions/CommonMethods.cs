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
namespace Actions.SettingsOperations.SMSSettingsDlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "SettingsOperations.SMSSettingsDlg."; }

        public static string SetupPageEntries(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
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
                        "SELECT OutputLine FROM EASMSSETTINGS(@CompanyId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
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

        public static string LoadUpdateRequestItem(ActionBase ActionBase,
                                                   ref UpdateRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadUpdateRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "SMSSupplierId");
                TempItem.SMSSupplierIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "MaxSMSLength");
                TempItem.MaxSMSLengthValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DefaultToEmail");
                TempItem.DefaultToEmail = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DefaultFromEmail");
                TempItem.DefaultFromEmail = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string Update(string ConnectionString,
                                    int CompanyId,
                                    int ClientId,
                                    UpdateRequestItem Request,
                                    ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "Update";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        SetupPageEntries(ConnectionString, CompanyId, ClientId, ref TempEntries);
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
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateSQL1", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Update query";
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("I_CO_SMSSUPPLIER", Request.SMSSupplierId));
                    Params.Add(new FbParameterItem("I_CO_SMSLENGTH", Request.MaxSMSLength));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second Update query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Params.Clear();
                    Params.Add(new FbParameterItem("CO_SMSTOEMAIL", Request.DefaultToEmail));
                    Params.Add(new FbParameterItem("CO_SMSFROMEMAIL", Request.DefaultFromEmail));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, ref Entries);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After Update Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
