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
using BCrypt;
using BCrypt.Net;
using System.Web.Security;
using System.Security.Cryptography;
namespace Actions.SettingsOperations.ChangePasswordDlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "SettingsOperations.ChangePasswordDlg."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadAllPageDataRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = null;
                Obj = ActionBase.GetCollectionValue("Data", "ClientId");
                TempItem.ClientIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
                                              ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "SetupPageEntries";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "SELECT * FROM EAGetLoginDetailsForClient(@CompanyId, @ClientId)";
                    Params.Clear();
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("ClientId", ClientId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                DataTable tbl = null;
                object Obj = null;
                string Value = "";
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
                            Obj = Row["LoginValue"];
                            Value = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("LoginValue", Value));
                            Obj = Row["ClientName"];
                            Value = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("ClientName", Value, false));
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

        public static string LoadChangePasswordRequestItem(ActionBase ActionBase,
                                                             ref ChangePasswordRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadAllPageDataRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                Result = Item.Load(ActionBase);

                Object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.ClientIdValue = Item.ClientIdValue;

                    Obj = ActionBase.GetCollectionValue("Data", "OldPassword");
                    TempItem.OldPassword = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "NewPassword");
                    TempItem.NewPassword = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "ConfirmPassword");
                    TempItem.ConfirmPassword= (Obj == null) ? "" : Convert.ToString(Obj);
                }
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

        public static string ChangePassword(string ConnectionString,
                                            int CompanyId,
                                            ChangePasswordRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "ChangePassword";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                // get hashed copy of the OldPassword value passed
                if (string.IsNullOrEmpty(Result))
                {
                    Result = CompareOldPasswordValues(ConnectionString, CompanyId, Request);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = UpdateUserPassword(ConnectionString, CompanyId, Request);
                }
                string HashedOldPwd = BCrypt.Net.BCrypt.HashPassword(Request.OldPassword, BCrypt.Net.BCrypt.GenerateSalt(12));
                WindowsEventLoggingUtilities.WriteToInfoEvent("Hashed Password of: " + Request.OldPassword + " is " + HashedOldPwd, Method, 0);

                // check hashed OldPassword to the one stored in the database
                // return error message if they don't match

                // hash the NewPassword value and store it in the database
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

        public static string CompareOldPasswordValues(string ConnectionString,
                                                      int CompanyId,
                                                      ChangePasswordRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "CompareOldPasswordValues";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                // get hashed copy of the OldPassword value passed
                /*
                string HashedOldPwd = "";
                if (string.IsNullOrEmpty(Result))
                {
                    HashedOldPwd = BCrypt.Net.BCrypt.HashPassword(Request.OldPassword, BCrypt.Net.BCrypt.GenerateSalt(12));
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Hashed Password of: " + Request.OldPassword + " is " + HashedOldPwd, Method, 0);
                    if (string.IsNullOrEmpty(HashedOldPwd)) Result = "Invalid hashed password";
                }
                */

                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = 
                        "SELECT ALOGIN_PASSWORD FROM EALogin WHERE I_ALOGIN_COMPANYID = @CompanyId AND I_ALOGIN_CLIENTID = @ClientId";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("ClientId", Request.ClientId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                DataTable tbl = null;
                object Obj = null;
                string StoredPwd = "";
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["ALogin_Password"];
                            StoredPwd = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            WindowsEventLoggingUtilities.WriteToInfoEvent("Stored Password: " + StoredPwd, Method, 0);
                        }
                    }
                }

                if (StoredPwd.ToLower() == Request.OldPassword.ToLower())
                {
                    Result = "Invalid password type";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    bool bResult = BCrypt.Net.BCrypt.Verify(Request.OldPassword, StoredPwd);
                    if (!bResult) Result = "Invalid password provided for the given user.";
                }
            }
            catch (BCrypt.Net.SaltParseException)
            {
                Result = "SaltParseException - Check password failed";
                //Result = ActionBase.LogOnException(Method, Result, "");
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

        public static string UpdateUserPassword(string ConnectionString,
                                                int CompanyId,
                                                ChangePasswordRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "UpdateUserPassword";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                // get hashed copy of the NewPassword value passed
                string HashedNewPwd = "";
                if (string.IsNullOrEmpty(Result))
                {
                    HashedNewPwd = BCrypt.Net.BCrypt.HashPassword(Request.NewPassword, BCrypt.Net.BCrypt.GenerateSalt(12));
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Hashed Password of: " + Request.NewPassword + " is " + HashedNewPwd, Method, 0);
                    if (string.IsNullOrEmpty(HashedNewPwd)) Result = "Invalid hashed password";
                }

                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        "UPDATE EALogin " +
                        "   SET ALOGIN_PASSWORD = @NewPassword," +
                        "       ALogin_Keep = @KeepPassword," +
                        "       B_ALogin_ChangePassword = 'N'" +
                        " WHERE I_ALOGIN_COMPANYID = @CompanyId AND I_ALOGIN_CLIENTID = @ClientId";
                    Params.Add(new FbParameterItem("NewPassword", HashedNewPwd));
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("ClientId", Request.ClientId));
                    Params.Add(new FbParameterItem("KeepPassword", Request.NewPassword));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
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

        public static string ResetPassword(string ConnectionString,
                                           int CompanyId,
                                           int ClientId,
                                           SetupPageEntriesRequestItem RequestItem,
                                           ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "ResetPassword";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string TempPwd = "";
                if (string.IsNullOrEmpty(Result))
                {
                    string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_!*#%$&(){}[]";

                    RandomNumberGenerator rng = RandomNumberGenerator.Create();
                    int PwdLength = 8;

                    StringBuilder passwordBuilder = new StringBuilder();
                    for (int i = 0; i < PwdLength; ++i)
                    {
                        int index = NextValue(rng, alphabet.Length);
                        passwordBuilder.Append(alphabet[index]);
                    }
                    TempPwd = passwordBuilder.ToString();
                    WindowsEventLoggingUtilities.WriteToInfoEvent("New Password: " + TempPwd, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = UpdatePasswordValues(ConnectionString, CompanyId, TempPwd, RequestItem);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    if (Entries.Length > 0) Entries.Append(",");
                    Entries.Append(JsonUtilities.AddPropertyNameAndValue("NewPassword", TempPwd, false));
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ResetPassword Result: " + Result, Method, 0);
            }

            return Result;
        }

        private static int NextValue(RandomNumberGenerator rng, int maxValueExclusive)
        {
            byte[] bytes = new byte[1];
            do
            {
                rng.GetBytes(bytes);
            } while (bytes[0] >= maxValueExclusive);
            return bytes[0];
        }

        public static string UpdatePasswordValues(string ConnectionString,
                                                  int CompanyId,
                                                  string NewPassword,
                                                  SetupPageEntriesRequestItem RequestItem)
        {
            string Result = "";
            string Method = GetModuleName() + "UpdatePasswordValues";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string HashedPwd = BCrypt.Net.BCrypt.HashPassword(NewPassword, BCrypt.Net.BCrypt.GenerateSalt(12));
                WindowsEventLoggingUtilities.WriteToInfoEvent("Hashed Password of: " + NewPassword + " is " + HashedPwd, Method, 0);
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        " UPDATE EALogin" +
                        "    SET ALogin_Password = @HashedPwd," +
                        "        ALogin_Keep = @NewPassword," +
                        "        B_ALOGIN_CHANGEPASSWORD = 'Y'" +
                        " WHERE I_ALogin_CompanyId = @CompanyId" +
                        "   AND I_ALogin_ClientID = @ClientId";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("ClientId", RequestItem.ClientId));
                    Params.Add(new FbParameterItem("HashedPwd", HashedPwd));
                    Params.Add(new FbParameterItem("NewPassword", NewPassword));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ResetPassword Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string ConvertPasswords(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
                                              SetupPageEntriesRequestItem RequestItem)
        {
            string Result = "";
            string Method = GetModuleName() + "ConvertPasswords";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        " SELECT I_ALOGIN_ID, ALOGIN_USERNAME, ALOGIN_PASSWORD" +
                        " FROM EALogin" +
                        " WHERE I_ALogin_CompanyId = @CompanyId" +
                        "   AND ALogin_Password IS NOT NULL" +
                        "   AND ALogin_Password<> ''" +
                        "   AND substring(ALOGIN_PASSWORD from 1 for 7) <> '$2a$12$'" +
                        " ORDER BY I_ALOGIN_ID";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                }

                DataTable tbl = null;
                object Obj = null;
                int LoginId = 0;
                string UserName = "";
                string Password = "";
                string NewPassword = "";
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        QueryText =
                            "UPDATE EALogin SET ALogin_Password = @NewPassword WHERE I_ALogin_CompanyId = @CompanyId AND I_ALogin_ID = @LoginId";
                        foreach (DataRow Row in tbl.Rows)
                        {
                            if (string.IsNullOrEmpty(Result))
                            {
                                Obj = Row["I_ALogin_ID"];
                                LoginId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                                Obj = Row["ALogin_UserName"];
                                UserName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                                Obj = Row["ALogin_Password"];
                                Password = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);

                                NewPassword = BCrypt.Net.BCrypt.HashPassword(Password, BCrypt.Net.BCrypt.GenerateSalt(12));

                                Params.Clear();
                                Params.Add(new FbParameterItem("CompanyId", CompanyId));
                                Params.Add(new FbParameterItem("NewPassword", NewPassword));
                                Params.Add(new FbParameterItem("LoginId", LoginId));
                                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                                if (string.IsNullOrEmpty(Result))
                                {
                                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                                }
                            }
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ResetPassword Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
