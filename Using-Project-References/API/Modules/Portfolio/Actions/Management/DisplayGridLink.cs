using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
namespace Actions.Portfolio.Management
{
    public class DisplayGridLink : ActionBase
    {
        private string FModuleName = "Portfolio";
        private DisplayGridLinkRequestItem FRequestItem;

        public static string Execute(int MasterId,
                                     APISettingsItem SettingsItem,
                                     string Request,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            DisplayGridLink inst = new DisplayGridLink();
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
                FRequestItem = new DisplayGridLinkRequestItem();
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
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ManagementPageRequestItem TempItem = new ManagementPageRequestItem();
                    ErrorMsg = TempItem.Load(this);
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        ErrorMsg =
                            CommonMethods.SaveManagementEntries(
                                FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, TempItem);
                    }
                }

                string LinkValue = "";
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    LinkValue = GetLinkValue(out ErrorMsg);
                }

                if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(LinkValue))
                {
                    Result =
                        JsonUtilities.StartJson() +
                        JsonUtilities.AddPropertyNameAndValue("Link", LinkValue, false) +
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

        private string GetLinkValue(out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";
            string Method = "GetLinkValue";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);

            LogOnInsideMethod(Method);
            try
            {
                StringBuilder Entries = new StringBuilder();

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg =
                        Actions.Portfolio.CommonMethods.EAResComSalesGroup(
                            FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
                            FRequestItem.PortfolioId, FRequestItem.NotesId, FRequestItem.Option1Id, FRequestItem.Option2Id, ref Entries);
                }

                string Str =
                    JsonUtilities.StartJson() +
                    Entries.ToString() +
                    JsonUtilities.EndJson();

                WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, FModuleName, MasterId);

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
                    string JPath = @"$.grid[?(@.I_ID == '" + FRequestItem.UniqueIdValue + "')]";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, FModuleName, MasterId);
                    JToken Record = JObj.SelectToken(JPath);
                    if (Record == null)
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", FModuleName, MasterId);
                    else
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), FModuleName, MasterId);
                        Result = (string)Record["ZL20_L" + FRequestItem.LinkIdValue];
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
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