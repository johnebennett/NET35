using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;

namespace Actions.Portal
{
    public class ValidateUser : ActionBase
    {
        private string FModuleName = "Portal";
        private ValidateUserRequestItem FRequestItem;

        public static string Execute(int MasterId,
                                     APISettingsItem SettingsItem,
                                     string Request,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            ValidateUser inst = new ValidateUser();
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
                FRequestItem = new ValidateUserRequestItem();
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
                    ErrorMsg =
                        CommonMethods.SetupPageEntries(
                            FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
                            FRequestItem, ref Entries);
                    */
                    /*
                     * Check the login status using EAPortalLogin and Id 989869
                     * If successful it will return some fields including the companyId and ClientId
                     * Using these values and the provided password - verify the password is valid
                     * If this passes validate the AuthCode the same way as the UserValidation of the UserOperations API method does
                    */
                    Result = DoValidateUser(out ErrorMsg);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    /*
                    Result =
                        JsonUtilities.StartJson() +
                        //Entries.ToString() +
                        Result +
                        JsonUtilities.EndJson();
                   */                       
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

        private string DoValidateUser(out string ErrorMsg)
        {
            ErrorMsg = "";
            string MethodName = "ValidateUser";
            string Result = "";
            object Obj = null;

            LogOnInsideMethod(MethodName);
            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Inside Portal-ValidateUser", CommonMethods.GetModuleName(), MasterId);
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                StringBuilder sb = new StringBuilder();
                //UserValidationItem ValidationItem = null;
                string Password = "";
                string Msg = "";
                if (!string.IsNullOrEmpty(FbConnectionString))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + FbConnectionString, "Portal-ValidateUser", MasterId);
                    Password = (string)(DataObject["password"]);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Password: " + Password, "ValidateUser", MasterId);
                    Msg =
                        "CompanyId: " + CommonItem.CompanyId + Environment.NewLine +
                        "ClientId: " + CommonItem.ClientId;
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Params:" + Environment.NewLine + Msg, "EAOfficeAPI", MasterId);

                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        //string QueryText = "SELECT * FROM ValidateWebUserEx('{0}', '{1}', '{2}')";
                        QueryText = "SELECT * FROM eaportallogin(@ID)";
                        Params.Clear();
                        //Params.Add(new FbParameterItem("ID", FRequestItem.ID));
                        Params.Add(new FbParameterItem("ID", CommonItem.ClientId));
                        ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, CommonMethods.GetModuleName(), 0);
                    }

                    DataTable tbl = null;
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        tbl =
                            UsefulDBMethods.Helpers.GetFirstDataTableEx(
                                FbConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
                    }

                    if (!string.IsNullOrEmpty(ErrorMsg))
                    {
                        ErrorMsg =
                            "Error Executing Query" + Environment.NewLine +
                            ErrorMsg + Environment.NewLine +
                            "QueryText:" + Environment.NewLine +
                            QueryText;
                        Result = LogOnErrorSelectingData(MethodName, ErrorMsg, QueryText);
                        WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, CommonMethods.GetModuleName(), MasterId);
                        return "";
                    }

                    string ResultMessage = "";
                    int LocalClientId = 0;
                    int LocalCompanyId = 0;
                    string Use2FA = "N";
                    string AuthCode = FRequestItem.AuthCode;
                    string SessionToken = "";
                    string StrValue = "";

                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        if (tbl == null)
                        {
                            ErrorMsg = "No table to process";
                            LogOnErrorSelectingData(MethodName, ErrorMsg, QueryText);
                            return "";
                        }
                        else if (tbl.Rows.Count == 0)
                        {
                            ErrorMsg = "No Row to process";
                            LogOnErrorSelectingData(MethodName, ErrorMsg, QueryText);
                            return "";
                        }
                        else
                        {
                            DataRow Row = tbl.Rows[0];
                            Obj = Row["ResultMessage"];
                            ResultMessage = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            Obj = Row["CompanyClientId"];
                            LocalCompanyId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                            Obj = Row["ClientId"];
                            LocalClientId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                            Obj = Row["Use2FA"];
                            Use2FA = (Obj == DBNull.Value) ? "N" : Convert.ToString(Obj);

                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("Result", LocalClientId));

                            Obj = Row["ClientFullName"];
                            StrValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("FullName", StrValue));

                            //sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("AfterHours", ValidationItem.AfterHours));

                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("ReturnMessage", ResultMessage));

                            //sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("ExternalUser", ValidationItem.ExternalUser));

                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyId", LocalCompanyId));

                            Obj = Row["Company_Name"];
                            StrValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyName", StrValue));

                            Obj = Row["Company_ABN"];
                            StrValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyABN", StrValue));

                            Obj = Row["Company_Address1"];
                            StrValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyAddress1", StrValue));

                            Obj = Row["Company_Address2"];
                            StrValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyAddress2", StrValue));
                            //sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("Permission", ValidationItem.Permission));
                            //sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("ChangePwd", ValidationItem.ChangePwd));
                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("SessionToken", SessionToken));
                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("Use2FA", Use2FA, false));
                            //sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("IsAuditor", ValidationItem.IsAuditor, false));
                            //sb.AppendLine(JsonUtilities.EndJsonObject()); // end of User

                            //string DataAsJson = sb.ToString();

                            //Result = LogResponse(MethodName, DataAsJson);

                            if (ResultMessage.ToLower() != "successful")
                            {
                                ErrorMsg = "Unsuccessful login attempt for the provided ID.";
                                Result = LogOnAuthorisationError(MethodName, ErrorMsg, "");
                            }


                            if (string.IsNullOrEmpty(ErrorMsg) && (Use2FA == "Y"))
                            {
                                ErrorMsg = CommonMethods.VerifyCode(FbConnectionString, LocalCompanyId, LocalClientId, AuthCode);
                                WindowsEventLoggingUtilities.WriteToInfoEvent("After VerifyCode, Result: " + ErrorMsg, "UserValidation - 2FA", MasterId);
                                if (!string.IsNullOrEmpty(ErrorMsg))
                                {
                                    Result = LogOnErrorValidatingData(MethodName, ErrorMsg, "");
                                    //return "";
                                }
                            }

                            if (string.IsNullOrEmpty(ErrorMsg) && (Use2FA == "N") && (AuthCode != ""))
                            {
                                ErrorMsg = CommonMethods.VerifyCode(FbConnectionString, LocalCompanyId, LocalClientId, AuthCode);
                                WindowsEventLoggingUtilities.WriteToInfoEvent(
                                    "Trying to use an Auth Code when not applicable, Result: " + ErrorMsg, 
                                    "UserValidation - 2FA", MasterId);
                                if (!string.IsNullOrEmpty(ErrorMsg))
                                {
                                    Result = LogOnErrorValidatingData(MethodName, ErrorMsg, "");
                                    //return "";
                                }
                            }

                            if (string.IsNullOrEmpty(ErrorMsg))
                            {
                                string DataAsJson = sb.ToString();
                                Result = LogResponse(MethodName, DataAsJson);
                            }
                        }
                    }

                    //int CompanyIdValue = 0;
                    //int ClientIdValue = 0;
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        Msg =
                            "CompanyId: " + CommonItem.CompanyId + Environment.NewLine +
                            "CompanyId: " + LocalCompanyId + Environment.NewLine +
                            "ClientId: " + CommonItem.ClientId + Environment.NewLine +
                            "ClientId: " + LocalClientId;
                        WindowsEventLoggingUtilities.WriteToInfoEvent(
                            "Params:" + Environment.NewLine + Msg, CommonMethods.GetModuleName(), MasterId);
                    }

                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        QueryText =
                            "SELECT ALOGIN_PASSWORD FROM EALogin WHERE I_ALOGIN_COMPANYID = @CompanyId AND I_ALOGIN_CLIENTID = @ClientId";
                        Params.Clear();
                        Params.Add(new FbParameterItem("CompanyId", LocalCompanyId));
                        Params.Add(new FbParameterItem("ClientId", LocalClientId));
                        ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, MethodName, 0);
                    }

                    string StoredPwd = "";
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Pulling Password", MethodName, 0);
                        tbl =
                            UsefulDBMethods.Helpers.GetFirstDataTableEx(
                                FbConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
                        if (string.IsNullOrEmpty(ErrorMsg))
                        {
                            foreach (DataRow Row in tbl.Rows)
                            {
                                Obj = Row["ALogin_Password"];
                                StoredPwd = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                                WindowsEventLoggingUtilities.WriteToInfoEvent("Stored Password: " + StoredPwd, MethodName, 0);
                            }
                        }
                        else
                        {
                            WindowsEventLoggingUtilities.WriteToInfoEvent("Error Msg: " + ErrorMsg, MethodName, 0);
                        }
                    }

                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        if (StoredPwd.ToLower() == Password.ToLower())
                        {
                            ErrorMsg = "Invalid password type";
                            Result = LogOnException(MethodName, ErrorMsg, "");
                        }
                    }

                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        bool bResult = BCrypt.Net.BCrypt.Verify(Password, StoredPwd);
                        if (!bResult)
                        {
                            ErrorMsg = "Invalid password provided for the given user.";
                            Result = LogOnException(MethodName, ErrorMsg, "");
                        }
                    }
                }
            }
            catch (BCrypt.Net.SaltParseException)
            {
                Result = "SaltParseException - Check password failed";
                //Result = ActionBase.LogOnException(Method, Result, "");
            }
            catch (Exception Ex)
            {
                Result = LogOnException(MethodName, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(MethodName);
            }

            return Result;
        }
    }
}