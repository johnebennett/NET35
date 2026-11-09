using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
using OtpNet;
namespace Actions.UserOperations
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "UserOperations."; }

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
                    new OtpNet.Totp(securityKey, mode: OtpNet.OtpHashMode.Sha1, step: 30, totpSize: 6, timeCorrection: correction);
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
    }
}
