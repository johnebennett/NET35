using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Diagnostics;
using System.Xml;
using System.IO;

using UsefulDBMethods;
using CommonUtilities;
using EAOfficeAPI;
using OtpNet;

namespace Actions.Portal
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "Portal."; }

        public static string LoadValidateUserRequestItem(ActionBase ActionBase,
                                                         ref ValidateUserRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadValidateUserRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "Password");
                TempItem.Password = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AuthCode");
                TempItem.AuthCode = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string VerifyCode(string ConnectionString,
                                        int CompanyId,
                                        int ClientId,
                                        string AuthCode)
        {
            string Result = "";
            string Method = "VerifyCode";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string Secret = GetOur2FASecret(ConnectionString);
                string UserUniqueKey = GetUserUniqueKey(ConnectionString, CompanyId, ClientId);
                string OtpEntry = @"otpauth://totp/GOATPM?secret=" + Secret + UserUniqueKey;
                byte[] securityKey = Base32Encoding.ToBytes(Secret + UserUniqueKey);
                DateTime CurrentTime = DateTime.UtcNow;
                OtpNet.TimeCorrection correction = new TimeCorrection(CurrentTime);
                OtpNet.Totp topt =
                    new OtpNet.Totp(
                        securityKey, mode: OtpNet.OtpHashMode.Sha1, step: 30, totpSize: 6, timeCorrection: correction);
                long LongVal = 0;
                VerificationWindow window = new VerificationWindow(previous: 1, future: 1);
                bool bResult = topt.VerifyTotp(CurrentTime, AuthCode, out LongVal, window);
                Result = (bResult) ? "" : "Invalid Authentication Code";
                /*
                if (Entries.Length > 0) Entries.Append(",");
                Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("Result", (bResult) ? "Valid" : "Invalid", false));
                */
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After VerifyCode Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        private static string GetOur2FASecret(string ConnectionString)
        {
            string Result = "";
            string Method = GetModuleName() + "GetOur2FASecret";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "SELECT EAOffice_2FASecret FROM EAOfficeSettings";
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);

                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["EAOffice_2FASecret"];
                            Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        }
                    }
                }

                if (string.IsNullOrEmpty(Result)) Result = "GOATPM";
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After GetOur2FASecret Result: " + Result, Method, 0);
            }

            if (string.IsNullOrEmpty(Result)) Result = "GOATPM";
            return Result;
        }

        private static string GetUserUniqueKey(string ConnectionString,
                                               Int64 CompanyId,
                                               Int64 ClientId)
        {
            string Result = "";
            string Method = GetModuleName() + "GetUserUniqueKey";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText =
                    " SELECT ALogin_Use2FAKey FROM EALogin" +
                    " WHERE I_ALogin_CompanyId = @CompanyId AND I_ALogin_ClientId = @ClientId";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyId", CompanyId));
                Params.Add(new FbParameterItem("ClientId", ClientId));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["ALogin_Use2FAKey"];
                            Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After GetUserUniqueKey Result: " + Result, Method, 0);
            }

            return Result;
        }
        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "RelationId");
                TempItem.RelationIdValue = (Obj == null) ? "0" : Convert.ToString(Obj);
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

        public static string SetupPageEntries(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
                                              SetupPageEntriesRequestItem Request,
                                              ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "SetupPageEntries";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "SELECT OutputLine FROM EAPortalDisplayList(@CompanyId, @RelationId, @LoginId)";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("RelationId", Request.RelationId));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                }

                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["OUTPUTLINE"];
                            Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupPageEntries Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string LoadDisplayGridReportRequestItem(ActionBase ActionBase,
                                                           ref DisplayGridReportRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadDisplayGridReportRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "RelationId");
                TempItem.RelationIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "UniqueId");
                TempItem.UniqueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ButtonId");
                TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string LoadGetCompanyLogoRequestItem(ActionBase ActionBase,
                                                           ref GetCompanyLogoRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadGetCompanyLogoRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "ForCompanyId");
                TempItem.ForCompanyIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
        
        public static string GetCompanyLoginLogo(string SourceFolder,
                                                 int CompanyId,                                                 
                                                 out string ErrorMsg)
        {
        	string Result = "";
        	ErrorMsg = "";
        	if (string.IsNullOrEmpty(ErrorMsg))
        	{
        		if (!Directory.Exists(SourceFolder))
        			ErrorMsg = "The folder: " + SourceFolder + " does not exist";
        	}
        	
        	if (string.IsNullOrEmpty(ErrorMsg))
        	{
        		if (!SourceFolder.EndsWith(Path.DirectorySeparatorChar.ToString()))
        			SourceFolder += Path.DirectorySeparatorChar;
        		SourceFolder += CompanyId.ToString();
        		if (!SourceFolder.EndsWith(Path.DirectorySeparatorChar.ToString()))
        			SourceFolder += Path.DirectorySeparatorChar;
        	}

        	if (string.IsNullOrEmpty(ErrorMsg))
        	{
        		if (!Directory.Exists(SourceFolder))
        			ErrorMsg = "The folder: " + SourceFolder + " does not exist";
        	}
        	
        	if (string.IsNullOrEmpty(ErrorMsg))
        	{
	        	Result = SourceFolder + "LoginLogo.jpg";
	        	if (!File.Exists(Result))
	        	{
	        		Result = SourceFolder + "LoginLogo.jpeg";
	        		if (!File.Exists(Result))
	        		{
	        			Result = SourceFolder + "LoginLogo.png";
	        			if (!File.Exists(Result))
	        			{
	        				ErrorMsg = "Login Logo Not Found";
	        				ErrorMsg = Result;
	        			}
	        		}
	        	}
        	}
        	
        	if (string.IsNullOrEmpty(ErrorMsg))
        	{
        		byte[] imgBytes = File.ReadAllBytes(Result);
        		Result = Convert.ToBase64String(imgBytes);
        	}
        	
        	if (!string.IsNullOrEmpty(ErrorMsg))
        	{
        		Result = "";
        	}

        	return Result;
        }
    }
}
