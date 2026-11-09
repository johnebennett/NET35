using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Portal.PortalEmailDlg
{
    public class RemoveEmailAttachment : ActionBase
    {
        private string FModuleName = "Portal.PortalEmailDlg";

        private string FRunId = "";
        private int FRunIdValue = 0;
        private string FRecordId = "";
        private int FRecordIdValue = 0;

        public static string Execute(int MasterId,
                                     RequestCommonItem CommonItem,
                                     JObject DataObject,
                                     APISettingsItem SettingsItem,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            RemoveEmailAttachment inst = new RemoveEmailAttachment();
            if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
                FRunId = (string)DataObject["runId"];
                FRunIdValue = StringUtilities.StrToIntDef(FRunId, -1);
                FRecordId = (string)DataObject["recordId"];
                FRecordIdValue = StringUtilities.StrToIntDef(FRecordId, -1);
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
                if (string.IsNullOrEmpty(Result) && (FRunIdValue <= 0)) Result = "Invalid RunId Value provided";
                if (string.IsNullOrEmpty(Result) && (FRecordIdValue <= 0)) Result = "Invalid Record ID Value provided";

                if (!string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Validation Failed: " + Result, FModuleName, MasterId);
                    Result =
                        JsonUtilities.StartJson() +
                        JsonUtilities.AddPropertyNameAndValue("Error", "Validation Failed", false) +
                        JsonUtilities.EndJson();
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

                Result = CommonMethods.RemoveAnEmailAttachment(this, FRecordIdValue);

                if (!string.IsNullOrEmpty(Result))
                {
                    Result = LogOnProcessingError(Method, Result, JsonUtilities.AddPropertyNameAndValue("Error", Result, false)); ;
                }
                else
                {
                    StringBuilder Entries = new StringBuilder();
                    Entries.AppendLine(JsonUtilities.StartJson());
                    Result = CommonMethods.BuildEmailAttachmentList(FbConnectionString, FRunIdValue, ref Entries);
                    Entries.AppendLine(JsonUtilities.EndJson());

                    if (!string.IsNullOrEmpty(Result))
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent(
                            "After BuildEmailAttachmentList Result: " + Result, FModuleName, MasterId);
                        Result = LogOnProcessingError(Method, Result, "");
                    }
                    else
                    {
                        Result = Entries.ToString();
                        WindowsEventLoggingUtilities.WriteToInfoEvent(
                            "After BuildEmailAttachmentList Entries: " + Environment.NewLine + Entries.ToString(), 
                            FModuleName, MasterId);
                    }
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
