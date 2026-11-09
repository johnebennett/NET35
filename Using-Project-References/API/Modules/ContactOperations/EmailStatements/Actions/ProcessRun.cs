using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.ContactOperations.EmailStatementsEx
{
    public class ProcessRun : ActionBase
    {
        private string FModuleName = "ContactOperations.EmailStatements";
        private ProcessRunRequestItem FRequestItem;

        public static string Execute(int MasterId,
                                     APISettingsItem SettingsItem,
                                     string Request,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            ProcessRun inst = new ProcessRun();
            //if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
                UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
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
                FRequestItem = new ProcessRunRequestItem();
                Result = FRequestItem.Load(this);
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
                Result = FRequestItem.Validate();
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
            string ErrorMsg = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                Result = BuildOutput(out ErrorMsg);
                if (!string.IsNullOrEmpty(ErrorMsg))
                {
                    Result = LogOnProcessingError(Method, ErrorMsg, "");
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

        private string BuildOutput(out string ErrorMsg)
        {
            ErrorMsg = "";
            string Result = "";
            string Method = "BuildOutput";

            LogOnInsideMethod(Method);
            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);

                string TodayStr = MiscUtilities.DateTimeToYYYYMMDD(DateTime.Now);
                ErrorMsg =
                    UsefulDBMethods.EventHelpers.ExecuteEventRunCreate(
                        FbConnectionString, FRequestItem.EventRunId, CommonItem.CompanyIdValue, 0, 0, 0, CommonItem.ClientIdValue, 0, "", "", "",
                        FRequestItem.EmailOption, "N", "N", FRequestItem.TemplateId, FRequestItem.Subject, FRequestItem.CCValue, FRequestItem.BCCValue,
                        FRequestItem.ExclusionsValue, FRequestItem.EventRunSource, "N", 0, FRequestItem.ReportId, FRequestItem.RunId, 0, 0, TodayStr, TodayStr, "", 0);

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    string QueryText =
                        "UPDATE EAEventRun SET " +
                        "EVA_FromEmail = @FromId" +
                        " WHERE EVA_ID = @RunId";
                    List<FbParameterItem> Params = new List<FbParameterItem>();
                    Params.Add(new FbParameterItem("@FromId", FRequestItem.From));
                    Params.Add(new FbParameterItem("@RunId", FRequestItem.EventRunId));

                    UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, 0);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", FModuleName, 0);
                    ErrorMsg = CommonUtilities.FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", FModuleName, 0);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + ErrorMsg, FModuleName, 0);
                }

                // When the TemplateId == 0 we assume that the email body is in HTML format already
                /*
                if ((string.IsNullOrEmpty(ErrorMsg)) && (FRequestItem.TemplateId == 0))
                {
                    ErrorMsg =
                        CommonMethods.UpdateEmailBodyForEventRunId(
                            FbConnectionString, FRequestItem.EventRunId, FRequestItem.EmailBody);
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        ErrorMsg =
                            CommonUtilities.UtilityApps.HtmlToRtf(
                                    "EAEventRun", "EVA_EmailBody", "EVA_ID", FRequestItem.EventRunIdValue,
                                    "EAEventRun", "EVA_EmailBody", "EVA_ID", FRequestItem.EventRunIdValue);
                    }
                }
                */

                // EventRunCreate populates the email body from the AEM_EM_Message field which is the RTF version
                // Now, regardless the TemplateId value, we want the email body to be in HTML format which is what
                // the FRequestItem.EmailBody format contains
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg =
                        CommonMethods.UpdateEmailBodyForEventRunId(
                            FbConnectionString, FRequestItem.EventRunId, FRequestItem.EmailBody);
                    //if (string.IsNullOrEmpty(ErrorMsg)) ErrorMsg = "Abandon";
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg =
                        CommonMethods.UpdateSignatureIdForEventRunId(
                        FbConnectionString, FRequestItem.EventRunId, FRequestItem.SignatureId);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg =
                        Actions.ContactOperations.CommonMethods.ExecuteEventRunProcess(
                            FbConnectionString, FRequestItem.EventRunId, CommonItem.CompanyIdValue);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling ExecuteEventRunProcess", FModuleName, MasterId);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    // Call the EmailProcessor to handle the sending of the emails
                    string WorkingDir = "";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling GetCurrentDirectory", FModuleName, MasterId);
                    //WorkingDir = Directory.GetCurrentDirectory();
                    WorkingDir = AppDomain.CurrentDomain.BaseDirectory;
                    string SepChar = Path.DirectorySeparatorChar.ToString();
                    if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling GetCurrentDirectory", FModuleName, MasterId);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("WorkingDir " + WorkingDir, FModuleName, MasterId);
                    try
                    {
                        /*
                    	Process runApp = new Process();
                        runApp.StartInfo.FileName = WorkingDir + "EmailProcessorHtml";
                        runApp.StartInfo.Arguments = FRequestItem.EventRunIdValue;
                        runApp.Start();
                        */
                       Result = CommonMethods.SetNewEmailStatus(FbConnectionString, FRequestItem.EventRunId.ToString());

                    }
                    catch (Exception Ex)
                    {
                        Result = LogOnException(Method, Ex, "");
                    }
                    finally
                    {
                        //Directory.SetCurrentDirectory(WorkingDir);
                    }

                    if (string.IsNullOrEmpty(Result))
                    {
                        StringBuilder DataAsJson = new StringBuilder();
                        DataAsJson.Append(JsonUtilities.StartJson());
                        DataAsJson.Append(JsonUtilities.StartJsonObject("result"));
                        DataAsJson.Append(JsonUtilities.AddPropertyNameAndValue("RunId", FRequestItem.RunIdValue));
                        DataAsJson.Append(JsonUtilities.AddPropertyNameAndValue("Status", "Success"));
                        DataAsJson.Append(JsonUtilities.EndJsonObject());
                        DataAsJson.Append(JsonUtilities.EndJson());
                        Result = LogResponse(Method, DataAsJson.ToString());
                    }
                }

                StringBuilder entries = new StringBuilder();
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    entries.AppendLine(JsonUtilities.StartJson());
                    entries.AppendLine(JsonUtilities.EndJson());
                    Result = entries.ToString();
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
    }
}
