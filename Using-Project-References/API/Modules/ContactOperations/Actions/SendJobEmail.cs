using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using UsefulDBMethods;
using EAOfficeAPI;
namespace Actions.ContactOperations
{
    public class SendJobEmailClass
    {
        public int RunId { get; set; }
        public int Task { get; set; }
        public int WhoTo1{ get; set; }
        public int WhoTo2 { get; set; }
        public string DistinctClientsOnly { get; set; }
        public string AddClientsAsExcluded { get; set; }
        public int EmailOption { get; set; }
        public int EmailTemplateId { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public int FromId { get; set; }
        public int CCId { get; set; }
        public int BccId { get; set; }
        public int PrintToId { get; set; }
        public string TestEmail { get; set; }
        public int EmailSignatureId { get; set; }
    }

    public class SendJobEmail : ActionBase
    {
        private string FModuleName = "ContactOperations";

        private string FPortfolioIdValue = "";
        private int FPortfolioId = 0;
        private string FTaskIdValue = "";
        private int FTaskId = 0;

        private string FPortfolioType = "";
        private int FPortfolioTypeValue = 0;
        private string FReportEventId = "";
        private int FReportEventIdValue = 0;
        private int FFilterEventId = 0;
        private int FTransactionTypeId = 0;
        private int FEventRunId = 0;
        private string FTransactionIdValue = "";
        private int FTransactionId = 0;

        private StringBuilder FInitEmailItems = new StringBuilder();
        private JObject FInitEmailItemsSource = null;
        //private SendJobEmailClass FSendJobEmailClass = new SendJobEmailClass();
        private ContactEmailFormData FContactEmailFormData = new ContactEmailFormData();

        public static string Execute(int MasterId,
                                     APISettingsItem SettingsItem,
                                     string Request,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            SendJobEmail inst = new SendJobEmail();
            if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
                return "";

            inst.LogOnInsideMethod(Method);
            try
            {
                Result = inst.Init();
                if (!string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
                    return "";
                }

                Result = inst.Validate();
                if (!string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
                    return "";
                }

                Result = inst.Output();
            }
            finally
            {
                inst.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        private string Init()
        {
            string Method = "Init";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                /*
                FPortfolioType = (string)DataObject["portfolioTypeId"];
                FPortfolioTypeValue = StringUtilities.StrToIntDef(FPortfolioType, -1);
                FReportEventId = (string)DataObject["reportEventId"];
                FReportEventIdValue = StringUtilities.StrToIntDef(FReportEventId, -1);
                */               

                FPortfolioIdValue = (string)DataObject["PortfolioId"];
                FPortfolioId = StringUtilities.StrToIntDef(FPortfolioIdValue, -1);
                FTaskIdValue = (string)DataObject["TaskId"];
                FTaskId = StringUtilities.StrToIntDef(FTaskIdValue, -1);
                FTransactionIdValue = (string)DataObject["TransactionId"];
                FTransactionId = StringUtilities.StrToIntDef(FTransactionIdValue, -1);
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }
            return Result;
        }

        private string Validate()
        {
            string Method = "Validate";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                int Dummy = 0;
                bool IsValid = int.TryParse(FPortfolioIdValue, out Dummy);
                FPortfolioId = (IsValid) ? Dummy : 0;
                if (!IsValid) Result = "PortfolioId is Not Provided or Invalid";
                if (string.IsNullOrEmpty(Result))
                {
                    IsValid = int.TryParse(FTaskIdValue, out Dummy);
                    FTaskId = (IsValid) ? Dummy : 0;
                    if (!IsValid) Result = "TaskId is Not Provided or Invalid";
                }
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }
            return Result;
        }

        private string Output()
        {
            string Method = "Output";
            string Result = "";
            //string ErrorMsg = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                if (string.IsNullOrEmpty(Result))
                {
                    FFilterEventId =
                        UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(FbConnectionString, "ea_filtereventid_gen", out Result);
                    if (string.IsNullOrEmpty(Result) && (FFilterEventId == 0)) Result = "Error generating the Filter Event ID";
                }

                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "EXECUTE PROCEDURE EAEmailTransactionLoader(@CompanyId, @Login, @FilterEventId, @B1Value, @TransactionId, @TaskId)";
                    Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
                    Params.Add(new FbParameterItem("Login", CommonItem.ClientIdValue));
                    Params.Add(new FbParameterItem("FilterEventId", FFilterEventId));
                    Params.Add(new FbParameterItem("B1Value", ""));
                    Params.Add(new FbParameterItem("TransactionId", FPortfolioId));
                    Params.Add(new FbParameterItem("TaskId", FTaskId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
                }

                QueryText = "";
                Params.Clear();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        "SELECT FIRST 1 EA_TRANSACTIONTYPEID from eafilterevent where ea_filtereventid = @FilterEventId";
                    Params.Add(new FbParameterItem("FilterEventId", FFilterEventId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, 0);
                }

                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            FbConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["EA_TRANSACTIONTYPEID"];
                            FTransactionTypeId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                        }
                    }
                }

                QueryText = "";
                Params.Clear();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "SELECT * FROM Init_Email_Items(@CompanyId, @ClientId, @PortfolioType, @TransactionId)";
                    Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
                    Params.Add(new FbParameterItem("@ClientId", CommonItem.ClientIdValue));
                    Params.Add(new FbParameterItem("@PortfolioType", FPortfolioTypeValue));
					Params.Add(new FbParameterItem("@TransactionId", FTransactionIdValue));                    
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
                }

                DataTable dt = null;
                if (string.IsNullOrEmpty(Result))
                {
                    try
                    {
                        dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);
                        if (dt == null)
                        {
                            Result = "No tables in the dataset to process";
                            return Result;
                        }
                    }
                    catch (Exception Ex)
                    {
                        Result = LogOnException(Method, Ex, "");
                        return Result;
                    }
                }

                FInitEmailItems.AppendLine(JsonUtilities.StartJson());
                if (string.IsNullOrEmpty(Result))
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        FInitEmailItems.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
                    }
                }
                FInitEmailItems.AppendLine(JsonUtilities.EndJson());

                if (string.IsNullOrEmpty(Result))
                {
                    FInitEmailItemsSource = JObject.Parse(FInitEmailItems.ToString());
                    if (FInitEmailItemsSource == null) Result = "Cannot parse the Group source";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = PopulateSendJobEmailClass(FInitEmailItemsSource, ref FContactEmailFormData);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling AddEmailEvent", FModuleName, 0);
                    FEventRunId =
                        UsefulDBMethods.EventHelpers.AddEmailEvent(
                            this.FbConnectionString, CommonItem.CompanyIdValue, FReportEventIdValue, "Contact-Email",
                            CommonItem.ClientIdValue, out Result);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling AddEmailEvent", FModuleName, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    if (FPortfolioTypeValue > 200)
                    {
                        QueryText = "UPDATE EAEVENTRUN set EAEVENTRUN.I_EVA_TASKID = @TaskId where eva_id = @EventRunId";
                        Params.Clear();
                        Params.Add(new FbParameterItem("TaskId", FTaskId));
                        Params.Add(new FbParameterItem("EventRunId", FEventRunId));
                        Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

                        if (string.IsNullOrEmpty(Result))
                        {
                            Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
                        }
                    }
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = CommonMethods.CreateXEmailTableForRunId(this.FbConnectionString, FEventRunId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        CommonMethods.UpdateEventRunRecord(
                            this.FbConnectionString,
                            FContactEmailFormData.RunIdValue,
                            FContactEmailFormData.TaskValue,
                            FContactEmailFormData.WhoTo1Value,
                            FContactEmailFormData.WhoTo2Value,
                            FContactEmailFormData.DistinctClientsOnly,
                            FContactEmailFormData.AddClientsAsExcluded,
                            FContactEmailFormData.EmailOptionValue,
                            FContactEmailFormData.EmailTemplateIdValue,
                            FContactEmailFormData.Subject,
                            FContactEmailFormData.EmailBody,
                            FContactEmailFormData.FromIdValue,
                            FContactEmailFormData.CCIdValue,
                            FContactEmailFormData.BccIdValue,
                            FContactEmailFormData.PrintToIdValue,
                            FContactEmailFormData.TestEmail,
                            FContactEmailFormData.EmailSignatureIdValue); 
                }

                if (string.IsNullOrEmpty(Result))
                {
                    // Not Required ??????
                    //Result = ExecuteEventEmailSql(FSendJobEmailClass.RunId);
                }

                StringBuilder Entries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = CommonMethods.BuildXEmailRecipientList(FbConnectionString, FContactEmailFormData.RunIdValue, ref Entries);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        CommonMethods.UpdateEventRunRecord(
                            this.FbConnectionString,
                            FContactEmailFormData.RunIdValue, FContactEmailFormData.TaskValue, FContactEmailFormData.WhoTo1Value, FContactEmailFormData.WhoTo2Value,
                            FContactEmailFormData.DistinctClientsOnly, FContactEmailFormData.AddClientsAsExcluded, FContactEmailFormData.EmailOptionValue,
                            FContactEmailFormData.EmailTemplateIdValue, FContactEmailFormData.Subject, FContactEmailFormData.EmailBody,
                            FContactEmailFormData.FromIdValue, FContactEmailFormData.CCIdValue, FContactEmailFormData.BccIdValue, FContactEmailFormData.PrintToIdValue,
                            FContactEmailFormData.TestEmail, FContactEmailFormData.EmailSignatureIdValue);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        EmailStatements.CommonMethods.UpdateEmailBodyForEventRunId(
                            FbConnectionString, FContactEmailFormData.RunIdValue, FContactEmailFormData.EmailBody);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        CommonMethods.UpdateSignatureIdForEventRunId(
                        FbConnectionString, FContactEmailFormData.RunIdValue, FContactEmailFormData.EmailSignatureIdValue);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        CommonMethods.ExecuteEventRunProcess(
                            FbConnectionString, FContactEmailFormData.RunIdValue, CommonItem.CompanyIdValue);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling ExecuteEventRunProcess", FModuleName, MasterId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    // Call the EmailProcessor to handle the sending of the emails
                    string WorkingDir = "";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling GetCurrentDirectory", FModuleName, MasterId);
                    //WorkingDir = Directory.GetCurrentDirectory();
                    WorkingDir = CommonUtilities.FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
                    string SepChar = Path.DirectorySeparatorChar.ToString();
                    if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling GetCurrentDirectory", FModuleName, MasterId);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("WorkingDir " + WorkingDir, FModuleName, MasterId);
                    try
                    {
                        Process runApp = new Process();
                        //runApp.StartInfo.FileName = WorkingDir + "EmailProcessor";
                        runApp.StartInfo.FileName = WorkingDir + "EmailProcessorHtml";
                        runApp.StartInfo.Arguments = FContactEmailFormData.RunId;
                        //runApp.StartInfo.Verb = "runas";
                        //runApp.StartInfo.UseShellExecute = true;
                        runApp.Start();
                    }
                    catch (Exception Ex)
                    {
                        Result = LogOnException(Method, Ex, "");
                    }
                    finally
                    {
                        Directory.SetCurrentDirectory(WorkingDir);
                    }
                }

                if (string.IsNullOrEmpty(Result))
                {
                    StringBuilder DataAsJson = new StringBuilder();
                    DataAsJson.Append(JsonUtilities.StartJson());
                    DataAsJson.Append(JsonUtilities.StartJsonObject("result"));
                    DataAsJson.Append(JsonUtilities.AddPropertyNameAndValue("RunId", FContactEmailFormData.RunId));
                    DataAsJson.Append(JsonUtilities.AddPropertyNameAndValue("Status", "Success"));
                    DataAsJson.Append(JsonUtilities.EndJsonObject());
                    DataAsJson.Append(JsonUtilities.EndJson());
                    Result = LogResponse(Method, DataAsJson.ToString());
                }
                else if (!string.IsNullOrEmpty(Result))
                {
                    Result = LogOnProcessingError(Method, Result, "");
                }
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }
            WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
            return Result;
        }

        private string PopulateSendJobEmailClass(JObject InitEmailItemsSource, ref ContactEmailFormData ContactEmailFormData)
        {
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside PopulateSendJobEmailClass", FModuleName, 0);
            return Result;
        }

        /*
        private string BuildOutput(out string ErrorMsg)
        {
            ErrorMsg = "";
            string Result = "";
            string Method = "BuildOutput";
            int EventRunId = 0;

            LogOnInsideMethod(Method);
            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);

                StringBuilder entries = new StringBuilder();
                entries.AppendLine(JsonUtilities.StartJson());

                ErrorMsg = BuildOutputEntries(ref entries);
                if (!string.IsNullOrEmpty(ErrorMsg)) return "";

                // create an eventrun record                
                EventRunId =
                    UsefulDBMethods.EventHelpers.AddEmailEvent(
                        this.FbConnectionString, CommonItem.CompanyIdValue, FReportEventIdValue, "Contact-Email",
                        CommonItem.ClientIdValue, out ErrorMsg);
                if (!string.IsNullOrEmpty(ErrorMsg)) return "";

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    if (FPortfolioTypeValue > 200)
                    {
                        string QueryText = "UPDATE EAEVENTRUN set EAEVENTRUN.I_EVA_TASKID = @TaskId where eva_id = @EventRunId";
                        List<FbParameterItem> Params = new List<FbParameterItem>();
                        Params.Add(new FbParameterItem("TaskId", FPortfolioTypeValue));
                        Params.Add(new FbParameterItem("EventRunId", EventRunId));
                        ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

                        if (string.IsNullOrEmpty(ErrorMsg))
                        {
                            ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(ErrorMsg)) return "";

                // pass the eventrunId in the results set
                entries.Append(",");
                entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("EventRunId", EventRunId, false));

                // create the xemail_eventrunId_ table
                ErrorMsg = CommonMethods.CreateXEmailTableForRunId(this.FbConnectionString, EventRunId);
                if (!string.IsNullOrEmpty(ErrorMsg)) return "";

                entries.AppendLine(JsonUtilities.EndJson());
                Result = entries.ToString();
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }

            return Result;
        }

        private string BuildOutputEntries(ref StringBuilder Entries)
        {
            string Method = "BuildOutputEntries";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);

            LogOnInsideMethod(Method);
            try
            {
                string QueryText = "SELECT * FROM Init_Email_Items(@CompanyId, @ClientId, @PortfolioType)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
                Params.Add(new FbParameterItem("@ClientId", CommonItem.ClientIdValue));
                Params.Add(new FbParameterItem("@PortfolioType", FPortfolioTypeValue));
                //WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

                DataTable dt = null;
                try
                {
                    dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);
                    if (dt == null)
                    {
                        Result = "No tables in the dataset to process";
                        return Result;
                    }
                }
                catch (Exception Ex)
                {
                    Result = LogOnException(Method, Ex, "");
                    return Result;
                }

                foreach (DataRow dataRow in dt.Rows)
                {
                    Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
                }
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }

            return Result;
        }
        */
    }
}
