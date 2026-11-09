using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.ToDo.Tasks
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "ToDo.Tasks."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "BranchId");
                TempItem.BranchIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ManagerId");
                TempItem.ManagerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TransTypeId");
                TempItem.TransTypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TaskTypeId");
                TempItem.TaskTypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TaskStatusId");
                TempItem.TaskStatusIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DateType");
                TempItem.DateTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "DateFrom");
                TempItem.DateFrom = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DateTo");
                TempItem.DateTo = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "OrderBy");
                TempItem.OrderByValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "CompletedType");
                TempItem.CompletedTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AssignedTo1");
                TempItem.AssignedTo1Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AssignedTo2");
                TempItem.AssignedTo2Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TradespersonId");
                TempItem.TradespersonIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AssignedTo3");
                TempItem.AssignedTo3Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Options");
                TempItem.OptionsValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
                        "SELECT OutputLine FROM EATaskScreenGroup(" +
                        "@CompanyId, @BranchId, @ManagerId, @TransTypeId, @TaskTypeId, @TaskStatusId," +
                        "@DateType, @DateFrom, @DateTo, @Options, @OrderBy, @CompletedType," +
                        "@AssignedTo1, @AssignedTo2, @AssignedTo3, @TradespersonId, @LoginId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("BranchId", Request.BranchId));
                    Params.Add(new FbParameterItem("ManagerId", Request.ManagerId));
                    Params.Add(new FbParameterItem("TransTypeId", Request.TransTypeId));
                    Params.Add(new FbParameterItem("TaskTypeId", Request.TaskTypeId));
                    Params.Add(new FbParameterItem("TaskStatusId", Request.TaskStatusId));
                    Params.Add(new FbParameterItem("DateType", Request.DateType));
                    Params.Add(new FbParameterItem("DateFrom", Request.DateFrom));
                    Params.Add(new FbParameterItem("DateTo", Request.DateTo));
                    Params.Add(new FbParameterItem("Options", Request.Options));
                    Params.Add(new FbParameterItem("OrderBy", Request.OrderBy));
                    Params.Add(new FbParameterItem("CompletedType", Request.CompletedType));
                    Params.Add(new FbParameterItem("AssignedTo1", Request.AssignedTo1));
                    Params.Add(new FbParameterItem("AssignedTo2", Request.AssignedTo2));
                    Params.Add(new FbParameterItem("AssignedTo3", Request.AssignedTo3));
                    Params.Add(new FbParameterItem("TradespersonId", Request.TradespersonId));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                }

                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
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
                        global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
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
                    TempItem.BranchIdValue = Item.BranchIdValue;
                    TempItem.ManagerIdValue = Item.ManagerIdValue;
                    TempItem.TransTypeIdValue = Item.TransTypeIdValue;
                    TempItem.TaskTypeIdValue = Item.TaskTypeIdValue;
                    TempItem.TaskStatusIdValue = Item.TaskStatusIdValue;
                    TempItem.DateTypeValue = Item.DateTypeValue;
                    TempItem.DateFrom = Item.DateFrom;
                    TempItem.DateTo = Item.DateTo;
                    TempItem.OrderByValue = Item.OrderByValue;
                    TempItem.CompletedTypeValue = Item.CompletedTypeValue;
                    TempItem.AssignedTo1Value = Item.AssignedTo1Value;
                    TempItem.AssignedTo2Value = Item.AssignedTo2Value;
                    TempItem.TradespersonIdValue = Item.TradespersonIdValue;
                    TempItem.AssignedTo3Value = Item.AssignedTo3Value;
                    TempItem.OptionsValue = Item.OptionsValue;

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
                To.BranchIdValue = From.BranchIdValue;
                To.ManagerIdValue = From.ManagerIdValue;
                To.TransTypeIdValue = From.TransTypeIdValue;
                To.TaskTypeIdValue = From.TaskTypeIdValue;
                To.TaskStatusIdValue = From.TaskStatusIdValue;
                To.DateTypeValue = From.DateTypeValue;
                To.DateFrom = From.DateFrom;
                To.DateTo = From.DateTo;
                To.OrderByValue = From.OrderByValue;
                To.CompletedTypeValue = From.CompletedTypeValue;
                To.AssignedTo1Value = From.AssignedTo1Value;
                To.AssignedTo2Value = From.AssignedTo2Value;
                To.TradespersonIdValue = From.TradespersonIdValue;
                To.AssignedTo3Value = From.AssignedTo3Value;
                To.OptionsValue = From.OptionsValue;
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
                    string JPath = @"$.gridEntries[?(@.ZC5_ID == '" + Request.UniqueId + "')]";
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
