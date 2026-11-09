using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.PaymentOperations
{
    public class DisplayPaymentsGridReport : ActionBase
    {
        private string FModuleName = "PaymentOperations";
        private DisplayPaymentsGridReportRequestItem FRequestItem;

        public static string Execute(int MasterId,
                                     APISettingsItem SettingsItem,
                                     string Request,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            DisplayPaymentsGridReport inst = new DisplayPaymentsGridReport();
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
                FRequestItem = new DisplayPaymentsGridReportRequestItem();
                Result = FRequestItem.Load((ActionBase)this, DataObject);
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
                /*
                StatementRunItem RunItem = new StatementRunItem();
                ErrorMsg = RunItem.Load((EAOfficeAPI.ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RunId);

                string ReportSource = "";
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ReportSource = GetReportSource(RunItem, out ErrorMsg);
                }

                string OutputFileName = "";
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Int64 NewId =
                        global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
                            FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
                            FRequestItem.PassedId, ReportSource, out ErrorMsg, out OutputFileName);
                }
                */

                StringBuilder Entries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        CommonMethods.DisplayPaymentsGridReport(
                            FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
                            FRequestItem, ref Entries);

                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Result =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        //JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false) +
                        JsonUtilities.EndJson();
                }
                else
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
    }
}
