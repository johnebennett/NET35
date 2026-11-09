using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
using SW.Phoenix.Server.Integration.ClientDL.ClientSide;
using SW.Phoenix.Server.Integration.Models;
namespace IPX_App
{
    public class Processor
    {
        public Processor()
        {
        }

        private string FUrl = "https://stage.ipropertyexpress.com/integration/";
        //private string FClientKey = "8d17ef42-cd46-4916-8087-39d65c29518c";

        private string FCmd = "";
        private string FDirection = "";
        private string FRunId = "";
        private string FPlugin = "IPX_App";
        private string FCurrentCompanyId = "";
        private EAOfficeAPI.APISettingsItem FSettingsItem = null;
        private RequestCommonItem FCommonItem = new RequestCommonItem();
        private string FConnectionString = "";
        private string FWorkingDir = "";
        private string FDirSepChar = Path.DirectorySeparatorChar.ToString();
        private JObject FJsonSource = null;
        private StringBuilder DownloadSuccess = new StringBuilder();
        private StringBuilder DownloadFail = new StringBuilder();

        public void Execute(string RunId, string Direction, string Cmd)
        {
            string Result = "";
            CommonMethods.WriteToOutput(Cmd, "Start Processing");
            FDirection = Direction;
            FCmd = Cmd;
            FRunId = RunId;

            if (string.IsNullOrEmpty(Result))
            {
                FSettingsItem = EAOfficeAPI.APISettingsFile.GetAPISettings(EAOfficeAPI.APIAppType.Service, out Result);
                FSettingsItem.DebugLogging = true;
                FCommonItem.Plugin = FPlugin;
                FWorkingDir = Directory.GetCurrentDirectory();
                if (!FWorkingDir.EndsWith(FDirSepChar)) FWorkingDir += FDirSepChar;
                FConnectionString = CommonDBMethods.Common.ConnectionString(FSettingsItem);
                if (string.IsNullOrEmpty(FConnectionString))
                    Result = "Missing or empty ConnectionString";
            }

            if (string.IsNullOrEmpty(Result))
            {
                FJsonSource = LoadJSONConfigSource(FWorkingDir, out Result);
            }

            if (!string.IsNullOrEmpty(Result))
            {
                CommonMethods.WriteToErrorOutput("Processing: " + Cmd);
                CommonMethods.WriteToErrorOutput(Result);
                CommonMethods.AddLoggingEntry(FConnectionString, RunId, "-1", Direction, "Error", Result);
            }
            else
            {
                ProcessCommand();
            }

            CommonMethods.AddLoggingEntry(FConnectionString, RunId, "-1", Direction, "Finished", Result);
        }

        private void ProcessCommand()
        {
            string Result = "";
            if (FCmd == "ALL")
            {
                string QueryText = "SELECT DISTINCT I_CO_CompanyClientId AS CompanyId FROM EACompany WHERE B_CO_Active = 'Y'";
                if (string.IsNullOrEmpty(Result))
                {
                    DataTable tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            FConnectionString, QueryText, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                    Object Obj = null;
                    FCurrentCompanyId = "";
                    foreach (DataRow Row in tbl.Rows)
                    {
                        Obj = Row["CompanyId"];
                        FCurrentCompanyId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        if (FCurrentCompanyId != "") Result = ProcessSingleCompany(FRunId, FDirection, FCurrentCompanyId);
                        if (!string.IsNullOrEmpty(Result))
                        {
                            CommonMethods.WriteToErrorOutput(Result);
                            CommonMethods.WriteToOutput(FCurrentCompanyId, Result);
                            CommonMethods.AddLoggingEntry(FConnectionString, FRunId, FCurrentCompanyId, FDirection, "Error", Result);
                        }
                    }
                }
            }
            else
            {
                FCurrentCompanyId = FCmd;
                Result = ProcessSingleCompany(FRunId, FDirection, FCurrentCompanyId);
            }
        }

        private string ProcessSingleCompany(string RunId, string Direction, string CompanyId)
        {
            string Result = "";
            CommonMethods.WriteToOutput(CompanyId, "Processing CompanyId: " + CompanyId);
            CommonMethods.AddLoggingEntry(FConnectionString, FRunId, CompanyId, FDirection, "Information", "Processing CompanyId: " + CompanyId);

            string DocsLocation = "";
            string RootLocation = "";
            string ClientKey = "";
            if (string.IsNullOrEmpty(Result))
            {
                Result = 
                    SetupCompanyRelatedItems(
                        FConnectionString, CompanyId, out DocsLocation, out RootLocation, out ClientKey);
                if (string.IsNullOrEmpty(Result) && (string.IsNullOrEmpty(ClientKey) || (ClientKey == "N")))
                {
                    Result = "Company: " + CompanyId + " cannot access the IPX system";
                    WindowsEventLoggingUtilities.WriteToErrorEvent(Result, FPlugin, 0);
                    WindowsEventLoggingUtilities.WriteToInfoEvent(Result, FPlugin, 0);
                    new ActionBase().LogOnProcessingError(FPlugin, Result, "");
                    CommonMethods.AddLoggingEntry(FConnectionString, FRunId, CompanyId, FDirection, "Error", Result);
                }
            }

            if (string.IsNullOrEmpty(Result))
            {
                if (Direction == "UPLOAD")
                {
                    CommonMethods.AddLoggingEntry(
                        FConnectionString, FRunId, CompanyId, FDirection, "Information", "Before LoadAndSendSnapshot");
                    Result = LoadAndSendSnapshot(FUrl, ClientKey, CompanyId, FJsonSource);
                    CommonMethods.AddLoggingEntry(
                        FConnectionString, FRunId, CompanyId, FDirection, "Information", "After LoadAndSendSnapshot");
                }
                else if (Direction == "DOWNLOAD")
                {
                    DownloadSuccess.Length = 0;
                    DownloadFail.Length = 0;
                    CommonMethods.AddLoggingEntry(
                        FConnectionString, FRunId, CompanyId, FDirection, "Information", "Before RunDownloadService");
                    Result = RunDownloadService(FUrl, ClientKey, CompanyId);
                    CommonMethods.AddLoggingEntry(
                        FConnectionString, FRunId, CompanyId, FDirection, "Information", "After RunDownloadService");
                }
            }

            if (!string.IsNullOrEmpty(Result))
            {
                CommonMethods.WriteToErrorOutput(Result);
                CommonMethods.WriteToOutput(CompanyId, Result);
                CommonMethods.AddLoggingEntry(FConnectionString, FRunId, CompanyId, FDirection, "Error", Result);
            }

            return Result;
        }

        private string SetupCompanyRelatedItems(string ConnectionString,
                                                string CompanyId,
                                                out string DocsLocation,
                                                out string RootLocation,
                                                out string ClientKey)
        {
            string Result = "";
            string Method = "SetupCompanyRelatedItems";
            DocsLocation = "";
            RootLocation = "";
            ClientKey = "";

            int CompanyIdValue = CommonUtilities.StringUtilities.StrToIntDef(CompanyId, 0);
            if (string.IsNullOrEmpty(Result))
            {
                DocsLocation = CommonUtilities.FolderHelpers.DocsLocation(FConnectionString, CompanyIdValue, out Result);
                CommonMethods.WriteToOutput(CompanyId, "DocsLocation: " + DocsLocation);
            }

            if (string.IsNullOrEmpty(Result))
            {
                RootLocation = DocsLocation;
                if (RootLocation.EndsWith(FDirSepChar))
                {
                    RootLocation = Directory.GetParent(RootLocation).FullName;
                }
                RootLocation = Directory.GetParent(RootLocation).FullName;
                if (!RootLocation.EndsWith(FDirSepChar)) RootLocation += FDirSepChar;
                CommonMethods.WriteToOutput(CompanyId, "Root Location: " + RootLocation);
            }

            string QueryText = "";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            if (string.IsNullOrEmpty(Result))
            {
                QueryText = "SELECT CO_IPX_CLIENTKEY AS ClientKey FROM EACompany WHERE I_CO_CompanyClientId = @CompanyId";
                Params.Add(new FbParameterItem("CompanyId", CompanyId));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            }

            DataTable tbl = null;
            Object Obj = null;
            if (string.IsNullOrEmpty(Result))
            {
                tbl =
                    UsefulDBMethods.Helpers.GetFirstDataTableEx(
                        ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                if (string.IsNullOrEmpty(Result))
                {
                    foreach (DataRow Row in tbl.Rows)
                    {
                        Obj = Row["ClientKey"];
                        ClientKey = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    }
                }
            }

            return Result;
        }

        private JObject LoadJSONConfigSource(string WorkingDirectory, out string ErrorMsg)
        {
            JObject Result = null;
            ErrorMsg = "";

            string ConfigFileName = WorkingDirectory + "IPX_Config.json";
            string ConfigContents = File.ReadAllText(ConfigFileName);
            if (string.IsNullOrEmpty(ConfigContents)) ErrorMsg = "Cannot read the contents of the config file";

            if (string.IsNullOrEmpty(ErrorMsg))
            {
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Result = JObject.Parse(ConfigContents);
                    if (Result == null) ErrorMsg = "Cannot parse the Group source";
                }
            }

            return Result;
        }

        private string GetConfigSectionQuery(JObject ConfigSource,
                                             string CompanyId,
                                             string Section,
                                             out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";

            try
            {
                string Path = "$." + Section;
                JArray ResultArray = JsonUtilities.GetJsonPathValue(ConfigSource, Path, out Result) as JArray;
                if (string.IsNullOrEmpty(ErrorMsg) && (ResultArray == null)) ErrorMsg = "No " + Section + " content available";

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    for (int Index = 0; Index < ResultArray.Count; Index++)
                    {
                        Result = Result + ResultArray[Index];
                    }
                }

                if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(Result))
                    ErrorMsg = "Cannot find the " + Section + " query";

                if (string.IsNullOrEmpty((ErrorMsg)))
                {
                    Result = string.Format(Result, CompanyId);
                    CommonMethods.WriteToOutput(CompanyId, Section + Environment.NewLine + Result);
                }
            }
            catch (Exception Ex)
            {
                ErrorMsg = Ex.Message;
            }

            return Result;
        }

        private string LoadAndSendSnapshot(string Url, 
                                           string ClientKey, 
                                           string CompanyId, 
                                           JObject JsonSource)
        {
            string Result = "";
            var intg = new IPXRPOfficeIntegration();

            CommonMethods.WriteToOutput(CompanyId, "Sending Snapshot data....");
            try
            {
                if (string.IsNullOrEmpty(Result))
                {
                    CommonMethods.WriteToOutput(CompanyId, "GetManagers");
                    intg.SQL_Managers = GetConfigSectionQuery(FJsonSource, CompanyId, "Managers", out Result); ;
                }

                if (string.IsNullOrEmpty(Result))
                {
                    CommonMethods.WriteToOutput(CompanyId, "Properties");
                    intg.SQL_Properties = GetConfigSectionQuery(FJsonSource, CompanyId, "Properties", out Result); ;
                }

                if (string.IsNullOrEmpty(Result))
                {
                    CommonMethods.WriteToOutput(CompanyId, "Owners");
                    intg.SQL_Owners = GetConfigSectionQuery(FJsonSource, CompanyId, "Owners", out Result); ;
                }

                if (string.IsNullOrEmpty(Result))
                {
                    CommonMethods.WriteToOutput(CompanyId, "Tenants");
                    intg.SQL_Tenants = GetConfigSectionQuery(FJsonSource, CompanyId, "Tenants", out Result); ;
                }

                if (string.IsNullOrEmpty(Result))
                {
                    CommonMethods.WriteToOutput(CompanyId, "Inspections");
                    intg.SQL_Inspections = GetConfigSectionQuery(FJsonSource, CompanyId, "Inspections", out Result); ;
                }

                if (string.IsNullOrEmpty(Result))
                {
                    CommonMethods.WriteToOutput(CompanyId, "Branches");
                    intg.SQL_Branches = GetConfigSectionQuery(FJsonSource, CompanyId, "Branches", out Result); ;
                }

                if (string.IsNullOrEmpty(Result))
                {
                    CommonMethods.AddLoggingEntry(
                        FConnectionString, FRunId, CompanyId, FDirection, "Information", "All Queries created");

                    CommonMethods.WriteToOutput(CompanyId, "LoadAndSendSnapshot");
                    Result = intg.LoadAndSendSnapshot(FUrl, ClientKey, FConnectionString, null);
                }
            }
            catch (Exception Ex)
            {
                StringBuilder values = new StringBuilder();
                foreach (var Key in Ex.Data.Keys)
                {
                    values.AppendLine(Key.GetType().ToString());
                }

                Result =
                    Ex.Message + Environment.NewLine +
                    Ex.Source + Environment.NewLine +
                    values.ToString() + Environment.NewLine +
                    Ex.InnerException + Environment.NewLine +
                    Ex.TargetSite + Environment.NewLine +
                    Ex.StackTrace;
                CommonMethods.WriteToOutput(CompanyId, "*****************************");
                CommonMethods.WriteToOutput(CompanyId, Result);
                CommonMethods.WriteToOutput(CompanyId, "*****************************");
            }
            return Result;
        }

        private string RunDownloadService(string Url,
                                          string ClientKey,
                                          string CompanyId)
        {
            string Result = "";

            try
            {
                DownloadSuccess.Length = 0;
                DownloadFail.Length = 0;
                FCurrentCompanyId = CompanyId;
                var intg = new IPXRPOfficeIntegration();

                CommonMethods.AddLoggingEntry(
                    FConnectionString, FRunId, FCurrentCompanyId, FDirection, "Information",
                    "Before calling RunDownloadService");
                Result = intg.RunDownloadService(Url, ClientKey, AfterDownload, PayloadReceived);
                CommonMethods.AddLoggingEntry(
                    FConnectionString, FRunId, FCurrentCompanyId, FDirection, "Information",
                    "After calling RunDownloadService");

                CommonMethods.WriteToOutput(CompanyId, "DownloadSuccess");
                CommonMethods.WriteToOutput(CompanyId, DownloadSuccess.ToString());
                CommonMethods.WriteToOutput(CompanyId, "DownloadFail");
                CommonMethods.WriteToOutput(CompanyId, DownloadFail.ToString());

                CommonMethods.AddLoggingEntry(
                    FConnectionString, FRunId, FCurrentCompanyId, FDirection, "Information",
                    "DownloadSuccess:" + Environment.NewLine + DownloadSuccess.ToString());
                CommonMethods.AddLoggingEntry(
                    FConnectionString, FRunId, FCurrentCompanyId, FDirection, "Information",
                    "DownloadFail:" + Environment.NewLine + DownloadFail.ToString());

            }
            catch (Exception Ex)
            {
                Result = Ex.Message;
            }
            return Result;
        }

        private void AfterDownload(string inspectionKey, int? BizId, DateTime scheduleDate)
        {
            //CommonMethods.WriteToOutput(string.Format("InspectionKey: {0}", inspectionKey));
            Console.WriteLine("InspectionKey: {0}", inspectionKey);
            if (DownloadSuccess.Length > 0) DownloadSuccess.Append(",");
            DownloadSuccess.Append(JsonUtilities.StartJson());
            DownloadSuccess.Append(JsonUtilities.AddPropertyNameAndValue("InspectionKey", inspectionKey));
            DownloadSuccess.Append(JsonUtilities.AddPropertyNameAndValue("TransactionId", BizId));
            DownloadSuccess.Append(JsonUtilities.AddPropertyNameAndValue("ScheduleDate", scheduleDate, false));
            DownloadSuccess.Append(JsonUtilities.EndJson());

            string Msg = string.Format("TaskID: {0}, BizId: {1}", inspectionKey, BizId);
            CommonMethods.AddLoggingEntry(
              FConnectionString, FRunId, FCurrentCompanyId, FDirection, "AfterDownload", Msg);

            string Result = MarkJobAsCompleted(inspectionKey, (int)BizId);

            //var hasError = true;
            var hasError = (!string.IsNullOrEmpty(Result));

            // if any detected, throw ApplicationException to the caller. 
            // Stops further processing
            if (hasError)
            {
                //var ErrorMsg = "database is not ready";
                //var error = string.Format("TaskID: {0}, BizId: {1} | Error Details: {2}", inspectionKey, BizId, ErrorMsg);
                var error = string.Format("TaskID: {0}, BizId: {1} | Error Details: {2}", inspectionKey, BizId, Result);

                throw new ApplicationException(error);
            }
        }

        private string MarkJobAsCompleted(string JobId, int BizId)
        {
            string Result = "";
            string Method = "MarkJobAsCompleted";

            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        " update eatask set B_ATASK_COMPLETED='Y'" +
                        " where I_ATASK_businessid = @BusinessId" +
                        "   and I_ATASK_TASKID= @JobId" +
                        "   and I_ATASK_COMPANYCLIENTID=@CompanyId";
                    Params.Add(new FbParameterItem("BusinessId", BizId));
                    Params.Add(new FbParameterItem("JobId", JobId));
                    Params.Add(new FbParameterItem("CompanyId", FCurrentCompanyId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, Convert.ToInt32(FRunId));
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(FConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FConnectionString, Convert.ToInt64(FCurrentCompanyId));
                }
            }
            catch (Exception Ex)
            {
                Result = Ex.Message;
            }

            return Result;
        }

        private void PayloadReceived(DLReportInfoPayload payload)
        {
            string Result = "";
            string Method = "PayloadReceived";
            //// to John:  please use this method to save the link to your system 
            Console.WriteLine("Inspection Express Report Key: {0}", payload.InspectionExpressReportKey);
            Console.WriteLine("RPO Inspection ID: {0}", payload.InspectionIntegrationKey);
            Console.WriteLine("RPO Property ID: {0}", payload.PropInegrationId);
            Console.WriteLine("LocalScheduleDate: {0}", payload.LocalScheduleDate);
            Console.WriteLine("Total Files: {0}", payload.Files.Count);

            string Msg =
                "Inspection Express Report Key: {0}" + Environment.NewLine +
                "RPO Inspection ID: {1}" + Environment.NewLine +
                "Transaction ID: {2}" + Environment.NewLine +
                "LocalScheduleDate: {3}" + Environment.NewLine +
                "Total Files: {4}";
            Msg =
                string.Format(
                    Msg, payload.InspectionExpressReportKey, payload.InspectionIntegrationKey,
                    payload.PropInegrationId, payload.LocalScheduleDate, payload.Files.Count);


            CommonMethods.AddLoggingEntry(
                FConnectionString, FRunId, FCurrentCompanyId, FDirection, "Information", Msg);

            string QueryText = "";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            for (var i = 0; i < payload.Files.Count; i++)
            {
                var fileInfo = payload.Files[i];
                Msg = "File [{0}].Name: {1} => File[{2}].Url: {3}";
                Msg = string.Format(Msg, i, fileInfo.FileName, i, fileInfo.URL);
                CommonMethods.AddLoggingEntry(
                    FConnectionString, FRunId, FCurrentCompanyId, FDirection, "Information", Msg);
                Console.WriteLine("  File [{0}].Name: {1} ", i, fileInfo.FileName);
                Console.WriteLine("  File [{0}].Url : {1} ", i, fileInfo.URL);

                QueryText =
                    " UPDATE EATask SET ATask_Url = @ReportUrl" +
                    " WHERE I_ATASK_PropertyId = @PropertyId" +
                    "   and I_ATASK_TASKID= @JobId" +
                    "   and I_ATASK_COMPANYCLIENTID= @CompanyId";
                Params.Clear();
                Params.Add(new FbParameterItem("ReportUrl", fileInfo.URL));
                Params.Add(new FbParameterItem("PropertyId", payload.PropInegrationId));
                Params.Add(new FbParameterItem("JobId", payload.InspectionIntegrationKey));
                Params.Add(new FbParameterItem("CompanyId", FCurrentCompanyId));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

                if (string.IsNullOrEmpty(Result))
                {
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, Convert.ToInt32(FRunId));
                }

                CommonMethods.AddLoggingEntry(
                    FConnectionString, FRunId, FCurrentCompanyId, FDirection, "Information",
                    "Before Updating Report Url");

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(FConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    CommonMethods.AddLoggingEntry(
                        FConnectionString, FRunId, FCurrentCompanyId, FDirection, "Information",
                        "After Updating Report Url");
                }
                else
                {
                    CommonMethods.AddLoggingEntry(
                        FConnectionString, FRunId, FCurrentCompanyId, FDirection, "Error", Result);
                }
            }
        }

    }
}
