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
namespace Actions.Portal.PortalEmailDlg
{
    /// <summary>
    /// Class that contains all of the form data passed from the 
    /// Contact\Email page
    /// </summary>
    /// 
    public class ContactEmailFormData
    {
        public string RunId { get; set; }
        public int RunIdValue { get; set; }
        public string Task { get; set; }
        public int TaskValue { get; set; }
        public string WhoTo1 { get; set; }
        public int WhoTo1Value { get; set; }
        public string WhoTo2 { get; set; }
        public int WhoTo2Value { get; set; }
        public string DistinctClientsOnly { get; set; }
        public string AddClientsAsExcluded { get; set; }
        public string EmailOption { get; set; }
        public int EmailOptionValue { get; set; }
        public string EmailTemplateId { get; set; }
        public int EmailTemplateIdValue { get; set; }

        public string Subject { get; set; }
        public string EmailBody { get; set; }

        public string FromId { get; set; }
        public int FromIdValue { get; set; }
        public string CCId { get; set; }
        public int CCIdValue { get; set; }
        public string BccId { get; set; }
        public int BccIdValue { get; set; }
        public string PrintToId { get; set; }
        public int PrintToIdValue { get; set; }
        public string ReportEventId { get; set; }
        public int ReportEventIdValue { get; set; }
        public string TestEmail { get; set; }
        public string EmailSignatureId { get; set; }
        public int EmailSignatureIdValue { get; set; }

        private string FModuleName = "ContactEmailFormData";

        public string Load(JObject Source)
        {
            string Result = "";
            try
            {
                RunId = (string)Source["runId"];
                RunIdValue = StringUtilities.StrToIntDef(RunId, -1);
                Task = (string)Source["task"];
                TaskValue = StringUtilities.StrToIntDef(Task, -1);
                WhoTo1 = (string)Source["whoTo1"];
                WhoTo1Value = StringUtilities.StrToIntDef(WhoTo1, -1);
                WhoTo2 = (string)Source["whoTo2"];
                WhoTo2Value = StringUtilities.StrToIntDef(WhoTo2, -1);
                DistinctClientsOnly = (string)Source["distinct"];
                AddClientsAsExcluded = (string)Source["excluded"];
                EmailOption = (string)Source["emailOption"];
                EmailOptionValue = StringUtilities.StrToIntDef(EmailOption, -1);
                EmailTemplateId = (string)Source["templateId"];
                EmailTemplateIdValue = StringUtilities.StrToIntDef(EmailTemplateId, -1);
                Subject = (string)Source["subject"];
                EmailBody = (string)Source["emailBody"];
                FromId = (string)Source["from"];
                FromIdValue = StringUtilities.StrToIntDef(FromId, -1);
                CCId = (string)Source["cc"];
                CCIdValue = StringUtilities.StrToIntDef(CCId, -1);
                BccId = (string)Source["bcc"];
                BccIdValue = StringUtilities.StrToIntDef(BccId, -1);
                PrintToId = (string)Source["print"];
                PrintToIdValue = StringUtilities.StrToIntDef(PrintToId, -1);
                ReportEventId = (string)Source["reportEventId"];
                ReportEventIdValue = StringUtilities.StrToIntDef(ReportEventId, -1);
                TestEmail = (string)Source["testEmail"];
                EmailSignatureId = (string)Source["emailSignatureId"];
                EmailSignatureIdValue = StringUtilities.StrToIntDef(EmailSignatureId, -1);
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
            }

            return Result;
        }

        public string Validate()
        {
            string Result = "";
            string Method = "Validate";
            try
            {
                if (string.IsNullOrEmpty(Result) && (RunIdValue < 0)) Result = "Invalid RunId provided";
                if (string.IsNullOrEmpty(Result) && (TaskValue < 0)) Result = "Invalid Task Value provided";
                if (string.IsNullOrEmpty(Result) && (WhoTo1Value < 0)) Result = "Invalid WhotTo1 provided";
                if (string.IsNullOrEmpty(Result) && (WhoTo2Value < 0)) Result = "Invalid WhotTo2 provided";
                if (string.IsNullOrEmpty(Result) && (WhoTo2Value < 0)) Result = "Invalid WhotTo2 provided";

                if (string.IsNullOrEmpty(Result) && (EmailOptionValue < 0)) Result = "Invalid EmailOption Value provided";
                if (string.IsNullOrEmpty(Result) && (EmailTemplateIdValue < 0)) Result = "Invalid EmailTemplateId Value provided";
                if (string.IsNullOrEmpty(Result) && (FromIdValue < 0)) Result = "Invalid FromId Value provided";
                if (string.IsNullOrEmpty(Result) && (CCIdValue < 0)) Result = "Invalid CCId Value provided";
                if (string.IsNullOrEmpty(Result) && (BccIdValue < 0)) Result = "Invalid BccId Value provided";
                if (string.IsNullOrEmpty(Result) && (PrintToIdValue < 0)) Result = "Invalid PrintToId Value provided";
                if (string.IsNullOrEmpty(Result) && (EmailSignatureIdValue < 0)) Result = "Invalid Email Signature Id Value provided";

                if (string.IsNullOrEmpty(Result) &&
                    !((StringUtilities.SameText(DistinctClientsOnly, "Y")) || (StringUtilities.SameText(DistinctClientsOnly, "N")))
                   ) Result = "Invalid DistinctClientsOnly Value provided";
                if (string.IsNullOrEmpty(Result) &&
                    !(StringUtilities.SameText(AddClientsAsExcluded, "Y") || StringUtilities.SameText(AddClientsAsExcluded, "N"))
                   ) Result = "Invalid DistinctClientsOnly Value provided";
                if (string.IsNullOrEmpty(Result) &&
                    !(StringUtilities.SameText(TestEmail, "Y") || StringUtilities.SameText(TestEmail, "N"))
                   ) Result = "Invalid Test Email Value provided";

                if (!string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Validation Failed: " + Result, FModuleName, 0);
                    Result =
                        JsonUtilities.StartJson() +
                        JsonUtilities.AddPropertyNameAndValue("Error", "Validation Failed", false) +
                        JsonUtilities.EndJson();
                }
            }
            catch (Exception Ex)
            {
                Result = new ActionBase().LogOnException(FModuleName + "." + Method, Ex, "");
            }
            return Result;
        }
    }

    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "Portal.PortalEmailDlg"; }

        public static string LoadSetupTransactionEmailRequestItem(ActionBase ActionBase,
                                                          ref SetupTransactionEmailRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSetupTransactionEmailRequestItem";
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "RelationId");
                TempItem.RelationIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
            }
            catch (Exception Ex)
            {
                Result =
                    Method + Environment.NewLine +
                    Ex.Message + Environment.NewLine +
                    Ex.StackTrace;
            }
            return Result;
        }

        public static string SetupTransactionEmail(string ConnectionString,
                                                   int CompanyId,
                                                   int ClientId,
                                                   SetupTransactionEmailRequestItem Request,
                                                   ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "SetupTransactionEmail";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                DataTable tbl = null;
                object Obj = null;
                List<FbParameterItem> Params = new List<FbParameterItem>();

                int TransactionId = 0;
                string QueryText = "SELECT I_ARE_BusinessId FROM EARelation WHERE I_ARE_ID = @RelationId";
                Params.Clear();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("RelationId", Request.RelationId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["I_ARE_BusinessId"];
                            TransactionId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                        }
                    }
                }

                int FilterEventId = 0;
                if (string.IsNullOrEmpty(Result))
                {
                    FilterEventId =
                        UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(ConnectionString, "ea_filtereventid_gen", out Result);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "EXECUTE PROCEDURE EAEmailTransactionLoader(@CompanyId, @Login, @FilterEventId, @B1Value, @TransactionId, @TaskId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("Login", ClientId));
                    Params.Add(new FbParameterItem("FilterEventId", FilterEventId));
                    Params.Add(new FbParameterItem("B1Value", ""));
                    Params.Add(new FbParameterItem("TransactionId", TransactionId));
                    Params.Add(new FbParameterItem("TaskId", 0));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                QueryText = "";
                Params.Clear();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        "SELECT FIRST 1 EA_TRANSACTIONTYPEID from eafilterevent where ea_filtereventid = @FilterEventId";
                    Params.Add(new FbParameterItem("FilterEventId", FilterEventId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                }

                int TransactionTypeId = 0;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["EA_TRANSACTIONTYPEID"];
                            TransactionTypeId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                        }
                    }
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Entries.Append(JsonUtilities.AddPropertyNameAndValue("FilterEventId", FilterEventId));
                    Entries.Append(JsonUtilities.AddPropertyNameAndValue("TransactionTypeId", TransactionTypeId, false));
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        ContactOperations_InitEmailItems(
                            ConnectionString, CompanyId, ClientId, FilterEventId, TransactionTypeId, ref Entries);
                }

                int EventRunId = 0;
                if (string.IsNullOrEmpty(Result))
                {
                    // create an eventrun record                
                    EventRunId =
                        UsefulDBMethods.EventHelpers.AddEmailEvent(
                            ConnectionString, CompanyId, FilterEventId, "Contact-Email",
                            ClientId, out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Entries.Append(",");
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("EventRunId", EventRunId, false));
                }

                if (string.IsNullOrEmpty(Result))
                {
                    // create the xemail_eventrunId_ table
                    Result = CreateXEmailTableForRunId(ConnectionString, EventRunId);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupTransactionEmail Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string ContactOperations_InitEmailItems(string ConnectionString,
                                                              int CompanyId,
                                                              int ClientId,
                                                              int ReportEventId,
                                                              int PortfolioTypeId,
                                                              ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "ContactOperations_InitEmailItems";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                DataTable tbl = null;
                List<FbParameterItem> Params = new List<FbParameterItem>();

                string QueryText = "SELECT * FROM Init_Email_Items(@CompanyId, @ClientId, @PortfolioType)";
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("@CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("@ClientId", ClientId));
                    Params.Add(new FbParameterItem("@PortfolioType", PortfolioTypeId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.AppendLine(",");

                        foreach (DataRow Row in tbl.Rows)
                        {
                            Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ContactOperations_InitEmailItems Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string UpdateEventRunRecord(string ConnectionString,
                                                  int RunId,
                                                  int TaskId,
                                                  int WhoTo1Id,
                                                  int WhoTo2Id,
                                                  string DistinctClientsOnly,
                                                  string AddClientsAsExcluded,
                                                  int EmailOption,
                                                  int EmailTemplateId,
                                                  string Subject,
                                                  string EmailBody,
                                                  int FromId,
                                                  int CCId,
                                                  int BccId,
                                                  int PrintToId,
                                                  string TestEmail,
                                                  int EmailSignatureId)
        {
            string Result = "";
            string Msg =
                "Inside UpdateEventRunRecord" + Environment.NewLine +
                "TemplateId: " + EmailTemplateId.ToString() + Environment.NewLine +
                "Subject: " + Subject + Environment.NewLine +
                "EmailBody: " + EmailBody;
            WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, "ContactOperations - UpdateEventRunRecord", 0);

            if (string.IsNullOrEmpty(Result))
            {
                string QueryText =
                    "UPDATE EAEventRun SET " +
                    "I_EVA_TaskType = @TaskType," +
                    "EVA_WHO1 = @WhoTo1," +
                    "EVA_WHO2 = @WhoTo2," +
                    "B_EVA_DistinctClients = @DistinctClientsOnly," +
                    "B_EVA_AddAsExcluded = @AddClientsAsExcluded," +
                    "I_EVA_EmailOption = @EmailOption," +
                    "I_EVA_EmailTemplateId = @EmailTemplateId," +
                    "EVA_SubjectLine = @EmailSubject," +
                    "EVA_EmailBody = @EmailBody," +
                    "EVA_FromEmail = @FromId," +
                    "EVA_CCTo = @CCId," +
                    "EVA_BCCTo = @BccId," +
                    "EVA_ExclusionTo = @PrintToId," +
                    "B_EVA_TestEmail = @TestEmail," +
                    "I_EVA_SignatureId = @EmailSignatureId" +
                    " WHERE EVA_ID = @RunId";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("@TaskType", TaskId));

                Params.Add(new FbParameterItem("@WhoTo1", WhoTo1Id));
                Params.Add(new FbParameterItem("@WhoTo2", WhoTo2Id));
                Params.Add(new FbParameterItem("@DistinctClientsOnly", DistinctClientsOnly));
                Params.Add(new FbParameterItem("@AddClientsAsExcluded", AddClientsAsExcluded));
                Params.Add(new FbParameterItem("@EmailOption", EmailOption));
                Params.Add(new FbParameterItem("@EmailTemplateId", EmailTemplateId));
                Params.Add(new FbParameterItem("@EmailSubject", Subject));

                /*
                byte[] EmailBodyArray = new byte[0];
                if (ContentIsValidHtml(EmailBody))
                {
                    EmailBodyArray = Encoding.UTF8.GetBytes(HtmlToRTF(EmailBody));
                }
                else
                {
                    EmailBodyArray = Encoding.UTF8.GetBytes(HtmlToRTF(EmailBody));
                }
                */
                byte[] EmailBodyArray = Encoding.UTF8.GetBytes(EmailBody);
                //byte[] EmailBodyArray = Encoding.UTF8.GetBytes(HtmlToRTF(EmailBody));

                Params.Add(new FbParameterItem("@EmailBody", EmailBodyArray));
                Params.Add(new FbParameterItem("@FromId", FromId));
                Params.Add(new FbParameterItem("@CCId", CCId));
                Params.Add(new FbParameterItem("@BccId", BccId));
                Params.Add(new FbParameterItem("@PrintToId", PrintToId));
                Params.Add(new FbParameterItem("@RunId", RunId));

                Params.Add(new FbParameterItem("@TestEmail", TestEmail));
                Params.Add(new FbParameterItem("@EmailSignatureId", EmailSignatureId));
                UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "ContactOperations", 0);

                WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", "UpdateEventRunRecord", 0);
                Result = CommonUtilities.FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", "UpdateEventRunRecord", 0);
                WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, "UpdateEventRunRecord", 0);
            }

            if (string.IsNullOrEmpty(Result) && (EmailTemplateId == 0))
            {
                // The content of the email body should be HTML.
                // Convert it to RTF and update the EVA_EmailBody field with the converted text
                Result =
                    CommonUtilities.UtilityApps.HtmlToRtf(
                        "EAEventRun", "EVA_EmailBody", "EVA_ID", RunId,
                        "EAEventRun", "EVA_EmailBody", "EVA_ID", RunId);
            }

            return Result;
        }

        public static string BuildXEmailRecipientList(string ConnectionString,
                                               int RunId,
                                               ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "BuildXEmailRecipientList";
            List<string> RowList = new List<string>();
            List<string> StrList = new List<string>();
            string ColName = "";
            string ColValue = "";
            string Str = "";
            string QueryText = "SELECT * FROM XEMAIL" + RunId;
            WindowsEventLoggingUtilities.WriteToErrorEvent("QueryText: " + QueryText, Method, 0);

            Entries.Append(CommonUtilities.JsonUtilities.StartJsonArray("Items"));
            DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
            if (dt == null)
            {
                Result = "No data table returned";
                WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
            }
            /*
            else if (dt.Rows.Count == 0)
            {
                Result = "No record returned for the query";
                WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
            }
            */
            else if (dt.Rows.Count > 0)
            {
                RowList.Clear();
                object Obj = null;
                Str = "";
                foreach (DataRow ThisRow in dt.Rows)
                {
                    StrList.Clear();
                    foreach (DataColumn ThisCol in dt.Columns)
                    {
                        ColName = ThisCol.ColumnName;
                        Obj = ThisRow[ColName];
                        ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        StrList.Add(JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
                    }
                    Str =
                        JsonUtilities.StartJsonObject("") +
                        string.Join(",", StrList.ToArray()) +
                        JsonUtilities.EndJsonObject();
                    RowList.Add(Str);
                }

                Entries.Append(string.Join(",", RowList.ToArray()));
            }
            Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());

            return Result;
        }

        public static string CreateXEmailTableForRunId(string ConnectionString,
                                                       int RunId)
        {
            string Result = "";
            string Method = "CreateXEmailTableForRunId";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, "ContactOperations", 0);
            WindowsEventLoggingUtilities.WriteToInfoEvent("RunId " + RunId.ToString(), "ContactOperations", 0);

            string ModuleName = GetModuleName();

            try
            {
                string QueryText = "EXECUTE PROCEDURE CREATE_WORKING_XEMAIL_CLIENTS(@RunId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("RunId", RunId));
                WindowsEventLoggingUtilities.WriteToInfoEvent("Before creating XEMAIL Table", "ContactOperations", 0);
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After creating XEMAIL Table", "ContactOperations", 0);
                WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, "ContactOperations", 0);
                if (!string.IsNullOrEmpty(Result))
                {
                    Result = new ActionBase().LogOnErrorSelectingData(Method, Result, QueryText);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    // check to see if the table exists
                    // Table should be named XEMAIL_RunId_
                    QueryText = "SELECT * FROM XEMAIL" + Convert.ToString(RunId);
                    DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
                    if (dt == null) Result = "No table returned.";
                }
            }
            catch (Exception Ex)
            {
                Result = new ActionBase().LogOnException(Method, Ex, "");
            }

            return Result;
        }

        public static string LoadProcessRunRequestItem(ActionBase ActionBase,
                                                       ref ProcessRunRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadProcessRunRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = null;
                Obj = ActionBase.GetCollectionValue("Data", "RunId");
                TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                /*
                Obj = ActionBase.GetCollectionValue("Data", "EventRunId");
                TempItem.EventRunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                */               
                Obj = ActionBase.GetCollectionValue("Data", "Task");
                TempItem.TaskValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "WhoTo1");
                TempItem.WhoTo1Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "WhoTo2");
                TempItem.WhoTo2Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "EmailOption");
                TempItem.EmailOptionValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "TemplateId");
                TempItem.TemplateIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ReportEventId");
                TempItem.ReportEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Subject");
                TempItem.Subject = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "EmailBody");
                TempItem.EmailBody = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "From");
                TempItem.FromValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "CC");
                TempItem.CCValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "BCC");
                TempItem.BCCValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Print");
                TempItem.PrintToValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "EmailSignatureId");
                TempItem.SignatureIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                /*
                Obj = ActionBase.GetCollectionValue("Data", "EventRunSource");
                TempItem.EventRunSource = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Source");
                TempItem.Source = (Obj == null) ? "" : Convert.ToString(Obj);
                */               
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

        public static string UpdateEmailBodyForEventRunId(string ConnectionString,
                                                          Int64 EventRunId,
                                                          string EmailBody)
        {
            string Result = "";
            string Method = "UpdateEmailBodyForEventRunId";
            string QueryText = "UPDATE EAEventRun SET EVA_EmailBody = @EmailBody WHERE EVA_ID = @EventRunId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            byte[] EmailBodyArray = Encoding.ASCII.GetBytes(EmailBody);
            Params.Add(new FbParameterItem("EmailBody", EmailBodyArray));
            Params.Add(new FbParameterItem("EventRunId", EventRunId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

            try
            {
                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
            }
            catch (Exception Ex)
            {
                Result =
                    Method + Environment.NewLine +
                    Ex.Message + Environment.NewLine +
                    Ex.StackTrace;
            }

            return Result;
        }

        public static string UpdateSignatureIdForEventRunId(string ConnectionString,
                                                            Int64 EventRunId,
                                                            int SignatureId)
        {
            string Result = "";
            string Method = "UpdateSignatureIdForEventRunId";
            string QueryText = "UPDATE EAEventRun SET I_EVA_SignatureId = @SignatureId WHERE EVA_ID = @EventRunId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("SignatureId", SignatureId));
            Params.Add(new FbParameterItem("EventRunId", EventRunId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

            try
            {
                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
            }
            catch (Exception Ex)
            {
                Result =
                    Method + Environment.NewLine +
                    Ex.Message + Environment.NewLine +
                    Ex.StackTrace;
            }

            return Result;
        }

        public static string ExecuteEventRunProcess(string ConnectionString,
                                                    int EventRunId,
                                                    int CompanyClientId)
        {
            string Result = "";
            string Method = "ExecuteEventRunProcess";

            string QueryText = "EXECUTE PROCEDURE EventRunProcess(@RunId, @CompanyClientId)";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("@RunId", EventRunId));
            Params.Add(new FbParameterItem("@CompanyClientId", CompanyClientId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

            if (string.IsNullOrEmpty(Result))
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Before EXECUTE PROCEDURE EVENTRUNPROCESS", Method, 0);
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After EXECUTE PROCEDURE EVENTRUNPROCESS", Method, 0);
                WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
            }

            if (!string.IsNullOrEmpty(Result))
            {
                Result = new ActionBase().LogOnProcessingError(Method, Result, "");
            }

            return Result;
        }

        public static string BuildEmailAttachmentList(string ConnectionString,
                                                      Int64 RunId,
                                                      ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "BuildEmailAttachmentList";
            List<string> RowList = new List<string>();
            List<string> StrList = new List<string>();
            string ColName = "";
            string ColValue = "";
            string Str = "";
            string QueryText =
                //"SELECT EAEmailAttachments.*, character_length(EAEmailAttachments.EA_Base64) AS FileLength " +
                //"FROM EAEmailAttachments WHERE EA_EventRunId = @RunId";
                " SELECT " +
                "   EAEmailAttachments.EA_ID," +
                "   EAEmailAttachments.EA_AttachmentName || EAEmailAttachments.EA_FileExtension AS FileName," +
                "   character_length(EAEmailAttachments.EA_BASE64)  AS FileLength" +
                " FROM EAEmailAttachments WHERE EA_EventRunId = @RunId";

            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("@RunId", RunId));

            //WindowsEventLoggingUtilities.WriteToErrorEvent("QueryText: " + QueryText, Method, 0);
            UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

            Entries.Append(CommonUtilities.JsonUtilities.StartJsonArray("Items"));
            //DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
            DataTable dt =
                UsefulDBMethods.Helpers.GetFirstDataTableEx(
                    ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
            /*
            FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
            if (dt == null)
            {
                Result = "No data table returned";
                WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
            }
            /*
            else if (dt.Rows.Count == 0)
            {
                Result = "No record returned for the query";
                WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
            }
            else if (dt.Rows.Count > 0)         
            */
            if (string.IsNullOrEmpty(Result))
            {
                RowList.Clear();
                object Obj = null;
                Str = "";
                foreach (DataRow ThisRow in dt.Rows)
                {
                    StrList.Clear();
                    foreach (DataColumn ThisCol in dt.Columns)
                    {
                        ColName = ThisCol.ColumnName;
                        Obj = ThisRow[ColName];
                        ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        StrList.Add(JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
                    }
                    Str =
                        JsonUtilities.StartJsonObject("") +
                        string.Join(",", StrList.ToArray()) +
                        JsonUtilities.EndJsonObject();
                    RowList.Add(Str);
                }

                Entries.Append(string.Join(",", RowList.ToArray()));
            }
            Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());

            return Result;
        }

        public static string RemoveAnEmailAttachment(ActionBase ActionBase,
                                                     int RecordId)
        {
            string Result = "";
            string Method = "RemoveAnEmailAttachment";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "DELETE FROM EAEmailAttachments WHERE EA_ID = @RecordId";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("@RecordId", RecordId));
                UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);

                WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", Method, ActionBase.MasterId);
                Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", Method, ActionBase.MasterId);
                WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, ActionBase.MasterId);
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
    }
}
