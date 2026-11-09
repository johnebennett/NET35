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
namespace Actions.Portal.PortalEmailDlg
{
    public class ProcessRun : ActionBase
    {
        private string FModuleName = "Portal.PortalEmailDlg";
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
                Result = FRequestItem.Load((ActionBase)this);
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
                StringBuilder Entries = new StringBuilder();
                Result =
                    CommonMethods.UpdateEventRunRecord(
                        this.FbConnectionString,
                        FRequestItem.RunId, 0, FRequestItem.WhoTo1, FRequestItem.WhoTo2,
                        "Y", "Y", FRequestItem.EmailOption,
                        FRequestItem.TemplateId, FRequestItem.Subject, FRequestItem.EmailBody,
                        FRequestItem.From, FRequestItem.CC, FRequestItem.BCC, 0,
                        "N", 0);

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        CommonMethods.UpdateEmailBodyForEventRunId(
                            FbConnectionString, FRequestItem.RunId, FRequestItem.EmailBody);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        CommonMethods.UpdateSignatureIdForEventRunId(
                        FbConnectionString, FRequestItem.RunId, FRequestItem.SignatureId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        CommonMethods.ExecuteEventRunProcess(
                            FbConnectionString, FRequestItem.RunId, CommonItem.CompanyIdValue);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling ExecuteEventRunProcess", FModuleName, MasterId);
                }

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
                    runApp.StartInfo.Arguments = FRequestItem.RunIdValue;
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
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
            }
            return Result;
        }
    }
}