using System;
using System.Data;
using System.Runtime.Hosting;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations.EmailStatementsEx
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "ContactOperations.EmailStatementsEx"; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase AActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadSetupPageEntriesRequestItem";
            object Obj = null;

            AActionBase.LogOnInsideMethod(Method);
            try
            {
                Obj = AActionBase.GetCollectionValue("Data", "runId");
                TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
            }
            catch (Exception Ex)
            {
                Result = AActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                AActionBase.LogOnLeavingMethod(Method);
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
                if (string.IsNullOrEmpty(Result))
                {
                    Result = InitContactEmailStatements(ConnectionString, CompanyId, ClientId, Request.RunId, ref Entries);
                }

                int EventRunId = 0;
                if (string.IsNullOrEmpty(Result))
                {
                    // create an eventrun record                
                    EventRunId =
                        UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
                            ConnectionString, "EVA_ID_GEN", CompanyId, ClientId, out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Entries.Append(",");
                    Entries.Append(JsonUtilities.AddPropertyNameAndValue("EventRunId", EventRunId, false));

                    string TodayStr = MiscUtilities.DateTimeToYYYYMMDD(DateTime.Now);
                    Result =
                        UsefulDBMethods.EventHelpers.ExecuteEventRunCreate(
                            ConnectionString, EventRunId, CompanyId, 0, 0, 0, ClientId, 0, "", "", "", 0, "N", "N",
                            0, "", "", "", "", "", //"Contact-EmailStatements", 
                            "N", // forMyself
                            0, // ForMyselfPdfOrXml
                            0, Request.RunId, 0, 0, TodayStr, TodayStr, "", 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    // get the entries for the recipients for the grid display
                    if (Entries.Length > 0) Entries.Append(",");
                    Result = GetEventRunShowClientsAll(ConnectionString, EventRunId, ref Entries);
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

        private static string InitContactEmailStatements(string ConnectionString,
                                                         int CompanyId, 
                                                         int ClientId,
                                                         Int64 RunId,
                                                         ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "InitContactEmailStatements";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "SELECT * FROM Init_Contact_EmailStmts(@CompanyId, @ClientId, @RunId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("@CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("@ClientId", ClientId));
                    Params.Add(new FbParameterItem("@RunId", RunId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                DataTable tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    foreach (DataRow dataRow in tbl.Rows)
                    {
                        Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
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

        private static String GetEventRunShowClientsAll(string ConnectionString,
                                                        Int64 EventRunId,
                                                        ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "GetEventRunShowClientsAll";

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText =
                    " SELECT" +
                    "   Z20_Client AS ZL20_Client," +
                    "   Z20_Property AS ZL20_Property," +
                    "   ZR10_Payment AS M_Payment," +
                    "   I_Client_Id," +
                    "   I_Id" +
                    " FROM EventRunShowClientsAll(@EventRunId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("@EventRunId", EventRunId));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

                DataTable tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = JsonUtilities.BuildJsonArrayFromDataTable(tbl, "gridEntries", ref Entries);
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

        public static string LoadProcessRunRequestItem(ActionBase ActionBase,
                                                       ref ProcessRunRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadStatementCancelPreviewRequestItem";
            object Obj = null;
            try
            {
                Obj = ActionBase.GetCollectionValue("Data", "runId");
                TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "eventRunId");
                TempItem.EventRunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "emailOption");
                TempItem.EmailOptionValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "templateId");
                TempItem.TemplateIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "reportId");
                TempItem.ReportIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "subject");
                TempItem.Subject = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "emailBody");
                TempItem.EmailBody = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "from");
                TempItem.FromValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "cc");
                TempItem.CCValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "bcc");
                TempItem.BCCValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "print");
                TempItem.ExclusionsValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "emailSignatureId");
                TempItem.SignatureIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "eventRunSource");
                TempItem.EventRunSource = (Obj == null) ? "" : Convert.ToString(Obj);

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
            WindowsEventLoggingUtilities.WriteToInfoEvent("EmailBodyText" + Environment.NewLine + EmailBody, Method, 0);

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

        public static string LoadUpdateHtmlTemplateRequestItem(ActionBase ActionBase,
                                                               ref UpdateHtmlTemplateRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadRunIdRequestItem";
            object Obj = null;
            try
            {
                Obj = ActionBase.GetCollectionValue("Data", "templateId");
                TempItem.TemplateIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "emailBody");
                TempItem.EmailBody = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string UpdateHtmlTemplate(string ConnectionString,
                                                int CompanyId,
                                                int ClientId,
                                                UpdateHtmlTemplateRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "UpdateHtmlTemplate";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    byte[] EmailBodyArray = Encoding.ASCII.GetBytes(Request.EmailBody);
                    QueryText = 
                        "UPDATE EAEmailTemplates SET AEM_BlobHtml = @BlobHtml WHERE I_AEM_CompanyClientId = @CompanyId AND I_AEM_ID = @TemplateId";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TemplateId", Request.TemplateId));
                    Params.Add(new FbParameterItem("BlobHtml", EmailBodyArray));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                string TestRPOLocation = "";
                if (string.IsNullOrEmpty(Result))
                {
                    TestRPOLocation = CommonUtilities.FolderHelpers.TestRPOLocation(ConnectionString, CompanyId, out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    string CommandLine = "GOATIMPORT /HTMLTOTEXT:" + Request.TemplateId;
                    Result =
                        UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(
                            CommandLine, TestRPOLocation, 1000);
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
		
		public static string SetNewEmailStatus(string ConnectionString,
		                                       string RunId)
		{
			string Result = "";
			string Method = "SetNewEmailStatus";
			string QueryText = "UPDATE EAEventRun SET EMAIL_STATUS = 'NEW' WHERE EVA_ID = @RunId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
			if (!string.IsNullOrEmpty(Result))
			{
				Result = new ActionBase().LogOnErrorSelectingData(Method, Result, QueryText);
			}
				
			return Result;
		}
    }
}
