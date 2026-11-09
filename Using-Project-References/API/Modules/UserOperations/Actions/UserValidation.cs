/*
 * Created by SharpDevelop.
 * User: John
 * Date: 16/10/2018
 * Time: 11:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UsefulDBMethods;
using BCrypt.Net;

namespace Actions.UserOperations
{
	public class UserValidationItem
	{
		public int Result { get; set; }
		public string FullName { get; set; }
		public string AfterHours { get; set; }
		public string ReturnMessage { get; set; }
		public int CompanyId { get; set; }
		public string ExternalUser { get; set; }
		public string CompanyName { get; set; }
		public string CompanyABN { get; set; }
		public string CompanyAddress1 { get; set; }
		public string CompanyAddress2 { get; set; }
        public string Permission { get; set; }
        public string ChangePwd { get; set; }
        public string Use2FA { get; set;  }
        public string SessionToken { get; set; }
        public string IsAuditor { get; set; }

        public UserValidationItem()
		{
			this.Result = 0;
			this.FullName = "";
			this.AfterHours = "N";
			this.ReturnMessage = "";
			this.CompanyId = 0;
			this.ExternalUser = "N";
			this.CompanyName = "";
			this.CompanyABN = "";
			this.CompanyAddress1 = "";
			this.CompanyAddress2 = "";
            this.Permission = "";
            this.ChangePwd = "N";
            this.Use2FA = "N";
            this.SessionToken = "";
            this.IsAuditor = "N";
        }

        public static UserValidationItem LoadFromTable(DataTable Source, 
		                                        out string ErrorMsg)
		{
			ErrorMsg = "";
			Object obj = null;
			UserValidationItem result = null;
			
			if (Source != null)
			{
				result = new UserValidationItem();
				DataRow dr = Source.Rows[0];
				if (dr != null)
				{
					//obj = dr["Result"];
					obj = dr["EAOClientId"];
					result.Result = ((obj == null) || (obj == DBNull.Value)) ? -1 : Convert.ToInt32(obj);

					//if (result.Result < 0) ErrorMsg += "Missing or invalid Result Value" + Environment.NewLine;
					
					obj = dr["ClientFullName"];
					result.FullName = ((obj != null) && (obj != DBNull.Value)) ? Convert.ToString(obj) : "";
					//if (string.IsNullOrEmpty(result.FullName)) ErrorMsg += "Missing or invalid FullName value";							  
					
					obj = dr["AfterHour"];
					result.AfterHours = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);
					//if (string.IsNullOrEmpty(result.AfterHours)) ErrorMsg += "Missing or invalid AfterHours Value" + Environment.NewLine;
					
					/*
					obj = dr["ReturnMessage"];
					result.ReturnMessage = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);
					*/
					
					obj = dr["CompanyClientId"];
					result.CompanyId = ((obj == null) || (obj == DBNull.Value)) ? -1 : Convert.ToInt32(obj);

					obj = dr["ExternalUser"];
					result.ExternalUser = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);

					obj = dr["Company_Name"];
					result.CompanyName = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);

					obj = dr["Company_ABN"];
					result.CompanyABN = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);

					obj = dr["Company_Address1"];
					result.CompanyAddress1 = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);

					obj = dr["Company_Address2"];
					result.CompanyAddress2 = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);

                    obj = dr["Permission"];
                    result.Permission = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);

                    obj = dr["ChangePwd"];
                    result.ChangePwd = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);

                    obj = dr["Use2FA"];
                    result.Use2FA = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);

                    obj = dr["SessionToken"];
                    result.SessionToken = ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);

                    obj = dr["IsAuditor"];
                    result.IsAuditor = ((obj == null) || (obj == DBNull.Value)) ? "N" : Convert.ToString(obj);
                }
            }
			return result;
		}
	}
	/// <summary>
	/// Description of UserValidation.
	/// </summary>
	public class UserValidation : ActionBase
	{
		/*
		public UserValidation()
		{
		}
		*/
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "UserValidation - Execute";
			string Result = "";
			ErrorMsg = "";
			
			UserValidation inst = new UserValidation();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try
			{
				Result = inst.ExecuteCommon(out ErrorMsg);
			}
			finally
			{
				inst.LogOnLeavingMethod(Method);
			}			
			
			//Result = inst.ValidateUser(MasterId, CommonItem, DataObject, out ErrorMsg);
			
			return Result;
		}
		
		private string ExecuteCommon(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Method = "ExecuteCommon";
			LogOnInsideMethod(Method);
			
			string Result = "";
			try 
			{
				if (CommonUtilities.StringUtilities.SameText(this.CommonItem.Action, "ValidateUser"))
				    Result = ValidateUser(out ErrorMsg);
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
		
        /*
        private string ValidateUser(out string ErrorMsg)
		{
			ErrorMsg = "";
			string MethodName = "ValidateUser";
			string Result = "";
			
			LogOnInsideMethod(MethodName);
			try
			{
				StringBuilder sb = new StringBuilder();
                string Msg = "";
				if (!string.IsNullOrEmpty(FbConnectionString))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + FbConnectionString, "ValidateUser", MasterId);
					string Password = "";
					Password = (string)(DataObject["password"]);
					
					//string QueryText = "SELECT * FROM ValidateWebUserEx('{0}', '{1}', '{2}')";
					string QueryText = "SELECT * FROM ealogintodatabase('{0}', '{1}', '{2}')";
					QueryText = string.Format(QueryText, CommonItem.CompanyId, CommonItem.ClientId, Password);
					WindowsEventLoggingUtilities.WriteToErrorEvent("QueryText:" + Environment.NewLine + QueryText, "EAOfficeAPI", MasterId);
                    Msg =
                        "CompanyId: " + CommonItem.CompanyId + Environment.NewLine +
                        "CompanyId: " + CommonItem.CompanyIdValue + Environment.NewLine +
                        "ClientId: " + CommonItem.ClientId + Environment.NewLine +
                        "ClientId: " + CommonItem.ClientIdValue;
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Params:" + Environment.NewLine + Msg, "EAOfficeAPI", MasterId);
                    System.Data.DataSet ds =
						CommonUtilities.FirebirdUtilities.GetDataSetFromQuery(
							FbConnectionString, QueryText, out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg =
							"Error Executing Query" + Environment.NewLine +
							ErrorMsg + Environment.NewLine +
							"QueryText:" + Environment.NewLine +
							QueryText;
						Result = LogOnErrorSelectingData(MethodName, ErrorMsg, QueryText);
						WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "EAOfficeAPI", MasterId);
						return "";
					}
					else
					{
						if (ds.Tables.Count == 0)
						{
							ErrorMsg = "No tables in the dataset to process";
							LogOnErrorSelectingData(MethodName, ErrorMsg, QueryText);
							return "";
						}
						else if (ds.Tables[0].Rows.Count == 0)
						{
							ErrorMsg = "No rows in the table to process";
							LogOnErrorSelectingData(MethodName, ErrorMsg, QueryText);
							return "";
						}
						else
						{
							UserValidationItem ValidationItem = UserValidationItem.LoadFromTable(ds.Tables[0], out ErrorMsg);
							if (!string.IsNullOrEmpty(ErrorMsg))
							{
								WindowsEventLoggingUtilities.WriteToErrorEvent("ValidationItem Result: " + ErrorMsg, "EAOfficeAPI", MasterId);
								LogOnErrorValidatingData(MethodName, ErrorMsg, QueryText);
								return "";
							}
							
							//sb.AppendLine(JsonUtilities.StartJsonObject("User"));
							sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("Result", ValidationItem.Result));
							sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("FullName", ValidationItem.FullName));
							sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("AfterHours", ValidationItem.AfterHours));
							sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("ReturnMessage", ValidationItem.ReturnMessage));
							sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("ExternalUser", ValidationItem.ExternalUser));
							sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyId", ValidationItem.CompanyId));
							sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyName", ValidationItem.CompanyName));
							sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyABN", ValidationItem.CompanyABN));
							sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyAddress1", ValidationItem.CompanyAddress1));
							sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyAddress2", ValidationItem.CompanyAddress2));
                            sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("Permission", ValidationItem.Permission, false));
                            //sb.AppendLine(JsonUtilities.EndJsonObject()); // end of User

                            string DataAsJson = sb.ToString();
							
							Result = LogResponse(MethodName, DataAsJson);
						}
					}
				}
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
		*/

        private string ValidateUser(out string ErrorMsg)
        {
            ErrorMsg = "";
            string MethodName = "ValidateUser";
            string Result = "";
            object Obj = null;

            LogOnInsideMethod(MethodName);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                StringBuilder sb = new StringBuilder();
                UserValidationItem ValidationItem = null;
                string Password = "";
                string Msg = "";
                if (!string.IsNullOrEmpty(FbConnectionString))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + FbConnectionString, "ValidateUser", MasterId);
                    Password = (string)(DataObject["password"]);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Password: " + Password, "ValidateUser", MasterId);
                    Msg =
                        "CompanyId: " + CommonItem.CompanyId + Environment.NewLine +
                        "ClientId: " + CommonItem.ClientId;
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Params:" + Environment.NewLine + Msg, "EAOfficeAPI", MasterId);

                    System.Data.DataSet ds = null;
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        //string QueryText = "SELECT * FROM ValidateWebUserEx('{0}', '{1}', '{2}')";
                        QueryText = "SELECT * FROM ealogintodatabase('{0}', '{1}', '{2}')";
                        WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, "EAOfficeAPI", MasterId);
                        QueryText = string.Format(QueryText, CommonItem.CompanyId, CommonItem.ClientId, Password);
                        WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, "EAOfficeAPI", MasterId);
                        //System.Data.DataSet ds =
                        ds =
                            CommonUtilities.FirebirdUtilities.GetDataSetFromQuery(
                                FbConnectionString, QueryText, out ErrorMsg);
                    }

                    if (!string.IsNullOrEmpty(ErrorMsg))
                    {
                        ErrorMsg =
                            "Error Executing Query" + Environment.NewLine +
                            ErrorMsg + Environment.NewLine +
                            "QueryText:" + Environment.NewLine +
                            QueryText;
                        Result = LogOnErrorSelectingData(MethodName, ErrorMsg, QueryText);
                        WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "EAOfficeAPI", MasterId);
                        return "";
                    }
                    else
                    {
                        if (ds.Tables.Count == 0)
                        {
                            ErrorMsg = "No tables in the dataset to process";
                            LogOnErrorSelectingData(MethodName, ErrorMsg, QueryText);
                            return "";
                        }
                        else if (ds.Tables[0].Rows.Count == 0)
                        {
                            ErrorMsg = "No rows in the table to process";
                            LogOnErrorSelectingData(MethodName, ErrorMsg, QueryText);
                            return "";
                        }
                        else
                        {
                            //UserValidationItem ValidationItem = UserValidationItem.LoadFromTable(ds.Tables[0], out ErrorMsg);
                            ValidationItem = UserValidationItem.LoadFromTable(ds.Tables[0], out ErrorMsg);
                            if (!string.IsNullOrEmpty(ErrorMsg))
                            {
                                WindowsEventLoggingUtilities.WriteToErrorEvent("ValidationItem Result: " + ErrorMsg, "EAOfficeAPI", MasterId);
                                LogOnErrorValidatingData(MethodName, ErrorMsg, QueryText);
                                return "";
                            }
                            else
                            {

                                Obj = (ds.Tables[0].Rows[0]["EAOClientId"]);
                                int LocalClientId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                                Obj = (ds.Tables[0].Rows[0]["COMPANYCLIENTID"]);
                                int LocalCompanyId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                                string AuthCode = (string)(DataObject["AuthCode"]);

                                if (string.IsNullOrEmpty(ErrorMsg) && (ValidationItem.Use2FA == "Y"))
                                {
                                    ErrorMsg = CommonMethods.VerifyCode(FbConnectionString, LocalCompanyId, LocalClientId, AuthCode);
                                    WindowsEventLoggingUtilities.WriteToInfoEvent("After VerifyCode, Result: " + ErrorMsg, "UserValidation - 2FA", MasterId);
                                    if (!string.IsNullOrEmpty(ErrorMsg))
                                    {
                                        Result = LogOnErrorValidatingData(MethodName, ErrorMsg, "");
                                        //return "";
                                    }
                                }

                                if (string.IsNullOrEmpty(ErrorMsg) && (ValidationItem.Use2FA == "N") && (AuthCode != ""))
                                {
                                    ErrorMsg = CommonMethods.VerifyCode(FbConnectionString, LocalCompanyId, LocalClientId, AuthCode);
                                    WindowsEventLoggingUtilities.WriteToInfoEvent("Trying to use an Auth Code when not applicable, Result: " + ErrorMsg, "UserValidation - 2FA", MasterId);
                                    if (!string.IsNullOrEmpty(ErrorMsg))
                                    {
                                        Result = LogOnErrorValidatingData(MethodName, ErrorMsg, "");
                                        //return "";
                                    }
                                }

                                if (string.IsNullOrEmpty(ErrorMsg))
                                {
                                    ValidationItem.SessionToken = "";
                                    /*
                                    if (string.IsNullOrEmpty(ErrorMsg))
                                    {
                                        // Create a new GUID as a session token for the current login attempt
                                        ValidationItem.SessionToken = Guid.NewGuid().ToString();
                                        QueryText =
                                            " UPDATE EALogin SET ALOGIN_SESSIONTOKEN = @SessionToken" +
                                            " WHERE I_ALogin_CompanyId = @CompanyId AND I_ALogin_ClientId = @ClientId";
                                        Params.Clear();
                                        Params.Add(new FbParameterItem("SessionToken", ValidationItem.SessionToken));
                                        Params.Add(new FbParameterItem("CompanyId", LocalCompanyId));
                                        Params.Add(new FbParameterItem("ClientId", LocalClientId));
                                        ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, MethodName, 0);
                                        if (string.IsNullOrEmpty(ErrorMsg))
                                        {
                                            ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
                                        }
                                    }
                                    */

                                    //sb.AppendLine(JsonUtilities.StartJsonObject("User"));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("Result", ValidationItem.Result));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("FullName", ValidationItem.FullName));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("AfterHours", ValidationItem.AfterHours));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("ReturnMessage", ValidationItem.ReturnMessage));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("ExternalUser", ValidationItem.ExternalUser));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyId", ValidationItem.CompanyId));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyName", ValidationItem.CompanyName));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyABN", ValidationItem.CompanyABN));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyAddress1", ValidationItem.CompanyAddress1));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("CompanyAddress2", ValidationItem.CompanyAddress2));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("Permission", ValidationItem.Permission));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("ChangePwd", ValidationItem.ChangePwd));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("SessionToken", ValidationItem.SessionToken));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("Use2FA", ValidationItem.Use2FA));
                                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("IsAuditor", ValidationItem.IsAuditor, false));
                                    //sb.AppendLine(JsonUtilities.EndJsonObject()); // end of User

                                    string DataAsJson = sb.ToString();

                                    Result = LogResponse(MethodName, DataAsJson);
                                }
                            }
                        }
                    }

                    int CompanyIdValue = 0;
                    int ClientIdValue = 0;
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        CompanyIdValue = ValidationItem.CompanyId;
                        ClientIdValue = ValidationItem.Result;
                        //WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + FbConnectionString, "ValidateUser", MasterId);
                        Msg =
                            "CompanyId: " + CommonItem.CompanyId + Environment.NewLine +
                            "CompanyId: " + CompanyIdValue + Environment.NewLine +
                            "ClientId: " + CommonItem.ClientId + Environment.NewLine +
                            "ClientId: " + ClientIdValue;
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Params:" + Environment.NewLine + Msg, "EAOfficeAPI", MasterId);
                    }

                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        QueryText =
                            "SELECT ALOGIN_PASSWORD FROM EALogin WHERE I_ALOGIN_COMPANYID = @CompanyId AND I_ALOGIN_CLIENTID = @ClientId";
                        Params.Add(new FbParameterItem("CompanyId", CompanyIdValue));
                        Params.Add(new FbParameterItem("ClientId", ClientIdValue));
                        ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, MethodName, 0);
                    }

                    DataTable tbl = null;
                    //object Obj = null;
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
                        bool bResult = BCrypt.Net.BCrypt.Verify(Password, StoredPwd, false, HashType.SHA384);
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
