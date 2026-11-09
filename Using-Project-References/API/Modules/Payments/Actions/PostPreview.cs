/*
 * Created by SharpDevelop.
 * User: John
 * Date: 07/08/2019
 * Time: 1:56 PM
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

namespace Actions.PaymentOperations
{
    /// <summary>
    /// Description of PostPreview.
    /// </summary>
    public class PostPreview : ActionBase
    {
        private string FModuleName = "PaymentOperations";

        private PostPreviewRequestItem FRequestItem;

        public static string Execute(int MasterId,
                                     RequestCommonItem CommonItem,
                                     JObject DataObject,
                                     APISettingsItem SettingsItem,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            PostPreview inst = new PostPreview();
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
                UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
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
                FRequestItem = new PostPreviewRequestItem();
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

        /// <summary>
        /// Creates the output for the process
        /// </summary>
        /// <returns>
        /// The resulting JSON value will contain:
        /// A SUCCESS message if all went well,
        /// An ERROR message if there was a problem during the process
        /// OR a list of "Errors" in the JSON Array
        /// </returns>
        private string Output()
        {
            string Method = "Output";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                string ErrorMsg = "";
                Result = BuildOutput(out ErrorMsg);
                if (!string.IsNullOrEmpty(ErrorMsg))
                {
                    Result = LogOnProcessingError(Method, Result, "");
                }
                else if (string.IsNullOrEmpty(Result))
                {
                    Result = LogResponse(Method, "");
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
            string Result = "";
            string Method = "BuildOutput";
            ErrorMsg = "";

            LogOnInsideMethod(Method);
            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);

                ErrorMsg = UsefulDBMethods.EventHelpers.SetEndOfMonthInProgress(FbConnectionString, CommonItem.CompanyIdValue, "S");

                //call EAStatementRunUpdateChqNumbers

                if (string.IsNullOrEmpty(ErrorMsg)) ErrorMsg = EAStatementRunUpdateChqNumbers();
                if (string.IsNullOrEmpty(ErrorMsg)) ErrorMsg = EAStatementPostList();
                if (string.IsNullOrEmpty(ErrorMsg)) ErrorMsg = EAStatementValidate();

                // check the validate results
                /*
SELECT * FROM EASTATEMENTVALIDATEPOST WHERE ASVP_OK='N'
WHERE I_ASVP_COMPANYCLIENTID=? AND I_ASVP_RUNID=?
RUNID,SUMMARYID,OWNER,REASON

                */
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Result = CheckValidateResults("Errors", out ErrorMsg);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result =
                            JsonUtilities.StartJson() +
                            Result +
                            JsonUtilities.EndJson();
                        return Result;
                    }
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg = EAStatementPostProcess();
                }
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {

                UsefulDBMethods.EventHelpers.SetEndOfMonthInProgress(FbConnectionString, CommonItem.CompanyIdValue, "N");
                LogOnLeavingMethod(Method);
            }

            return Result;
        }

        private string EAStatementRunUpdateChqNumbers()
        {
            string Result = "";
            string QueryText = "EXECUTE PROCEDURE EAStatementRunUpdateChqNumbers(@CompanyClientId, @RunId, @ChequeNumber)";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
            Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
            Params.Add(new FbParameterItem("ChequeNumber", FRequestItem.ChqNumber));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
            if (string.IsNullOrEmpty(Result))
                Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);

            return Result;
        }

        private string EAStatementPostList()
		{
			string Result = "";
			string QueryText = "EXECUTE PROCEDURE EAStatementPostList(@CompanyClientId, @RunId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			if (string.IsNullOrEmpty(Result))
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			
			return Result;
		}
		
		private string EAStatementValidate()
		{
			string Result = "";
			string QueryText = "EXECUTE PROCEDURE eaStatementValidate(@CompanyClientId, @RunId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			if (string.IsNullOrEmpty(Result))
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			
			return Result;
		}
		
		/// <summary>
		/// Builds a JSON list of reasons the validation failed
		/// </summary>
		/// <param name="ArrayName">The name of the JSON array created</param>
		/// <param name="ErrorMsg"></param>
		/// <returns>
		/// An empty string if there are no validation error or
		/// a JSON array of reasons
		/// </returns>
		private string CheckValidateResults(string ArrayName,
		                                    out string ErrorMsg)
		{
			string Result = "";
			string QueryText = 
				" SELECT" +
				"   I_ASVP_RunId AS I_Run_Id," +
				"   I_ASVP_SummaryId AS I_Summary_Id," +
				"   ASVP_AccountOwner AS Account_Owner," +
				"   ASVP_ReasonMessage AS Reason_Message" +
				" FROM EASTATEMENTVALIDATEPOST" +
				" WHERE ASVP_OK='N' AND I_ASVP_COMPANYCLIENTID = @CompanyClientId AND I_ASVP_RUNID = @RunId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
			ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			try 
			{
				DataTable tbl = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, 
							UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				}
				
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg) && (tbl.Rows.Count > 0))
				{
					ErrorMsg = CommonMethods.BuildJsonArrayFromDataTable(tbl, ArrayName, ref Entries);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = Entries.ToString();
				}
			}
			catch (Exception Ex)
			{
				Result =
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace;
			}
			
			return Result;
		}
		
		private string EAStatementPostProcess()
		{
			string Result = "";
			string QueryText = "EXECUTE PROCEDURE EAStatementPostProcess(@CompanyClientId, @RunId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			if (string.IsNullOrEmpty(Result))
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			
			return Result;
		}
    }
}
