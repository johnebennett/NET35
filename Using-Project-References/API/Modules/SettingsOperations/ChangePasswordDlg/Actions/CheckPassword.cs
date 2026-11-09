using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.SettingsOperations.ChangePasswordDlg
{
    public class CheckPassword : ActionBase
    {
        private string FModuleName = "SettingsOperations.ChangePasswordDlg";
        private ChangePasswordRequestItem FRequestItem;

        public static string Execute(int MasterId,
                                     APISettingsItem SettingsItem,
                                     string Request,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            CheckPassword inst = new CheckPassword();
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
                FRequestItem = new ChangePasswordRequestItem();
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
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    /*
            string hashed = BCrypt.Net.BCrypt.HashPassword(RequestItem.Password, BCrypt.Net.BCrypt.GenerateSalt(12));
            WindowsEventLoggingUtilities.WriteToInfoEvent("Hashed Password of: " + RequestItem.Password + " is " + hashed, Method, 0);
                    */
                    ErrorMsg = CommonMethods.CompareOldPasswordValues(FbConnectionString, CommonItem.CompanyIdValue, FRequestItem);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Result = LogResponse(Method, "");
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
            }
            return Result;
        }
    }
}