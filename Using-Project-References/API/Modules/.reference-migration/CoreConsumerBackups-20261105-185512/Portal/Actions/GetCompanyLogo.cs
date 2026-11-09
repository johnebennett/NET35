/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 12/06/2024
 * Time: 5:02 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
using System.IO;

namespace Actions.Portal
{
    public class GetCompanyLogo : ActionBase
    {
        private string FModuleName = "Portal";
        private GetCompanyLogoRequestItem FRequestItem;

        public static string Execute(int MasterId,
                                     APISettingsItem SettingsItem,
                                     string Request,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            GetCompanyLogo inst = new GetCompanyLogo();
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
                FRequestItem = new GetCompanyLogoRequestItem();
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
                     * Check the login status using EAPortalLogin and Id 989869
                     * If successful it will return some fields including the companyId and ClientId
                     * Using these values and the provided password - verify the password is valid
                     * If this passes validate the AuthCode the same way as the UserValidation of the UserOperations API method does
                    */
                    Result = DoGetCompanyLogo(out ErrorMsg);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Result =
                        JsonUtilities.StartJson() +
                    	JsonUtilities.AddPropertyNameAndValue("Logo", Result, false) +
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
            }
            return Result;
        }

        private string DoGetCompanyLogo(out string ErrorMsg)
        {
            ErrorMsg = "";
            string MethodName = "DoGetCompanyLogo";
            string Result = "";

            LogOnInsideMethod(MethodName);
            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent(
            		"Inside Portal-GetCompanyLogo", CommonMethods.GetModuleName(), MasterId);
                List<FbParameterItem> Params = new List<FbParameterItem>();
                StringBuilder sb = new StringBuilder();
                string DocsLocation = string.Empty;
                if (!string.IsNullOrEmpty(FbConnectionString))
                {
	            	Result =
	            		CommonUtilities.FolderHelpers.DocsLocation(
	            			FbConnectionString, CommonItem.CompanyIdValue, out ErrorMsg);
	            	if (string.IsNullOrEmpty(Result))
	            		ErrorMsg = "Docs Directory not provided";
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                    	if (Result.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    		Result += Path.DirectorySeparatorChar;
                    	Result += "Logo" + Path.DirectorySeparatorChar;
                    	if (!Directory.Exists(Result))
                    		ErrorMsg = "Logo Directory does not exist";
                    }
                }
                
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                	Result = 
                		CommonMethods.GetCompanyLoginLogo(
                			Result, CommonItem.CompanyIdValue,  out ErrorMsg);
                }                
            }
            catch (Exception Ex)
            {
                ErrorMsg = LogOnException(MethodName, Ex, "");
            }
            finally
            {
            	if (!string.IsNullOrEmpty(ErrorMsg))
            	{
            		Result = "";
            	}
                LogOnLeavingMethod(MethodName);
            }

            return Result;
        }
    }
}