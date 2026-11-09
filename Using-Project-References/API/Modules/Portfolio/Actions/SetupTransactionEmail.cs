/*
 * Created by SharpDevelop.
 * User: John
 * Date: 05/08/2020
 * Time: 6:21 AM
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
using UsefulDBMethods;
using EAOfficeAPI;
namespace Actions.Portfolio
{
	/// <summary>
	/// Description of SetupTransactionEmail.
	/// </summary>
	public class SetupTransactionEmail : ActionBase
	{
		private string FModuleName = "Portfolio";
		private string  FPortfolioIdValue = "";
		private int FPortfolioId = 0;
		private string FTaskIdValue = "";
		private int FTaskId = 0;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SetupTransactionEmail inst = new SetupTransactionEmail();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				this.FPortfolioIdValue = (string)GetCollectionValue("Data", "PortfolioId");
				this.FTaskIdValue = (string)GetCollectionValue("Data", "TaskId");
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method); 
			try 
			{
                int Dummy = 0;
                bool IsValid = int.TryParse(FPortfolioIdValue, out Dummy);
                FPortfolioId = (IsValid) ? Dummy : 0;
                if (!IsValid) Result = "PortfolioId is Not Provided or Invalid";
                if (string.IsNullOrEmpty(Result))
                {
		            IsValid = int.TryParse(FTaskIdValue, out Dummy);
		            FTaskId = (IsValid) ? Dummy : 0;
		            if (!IsValid) Result = "TaskId is Not Provided or Invalid";
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
		
		private string Output()
		{
			string Method = "Output";
			string Result = "";
			//string ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				// We shouldn't get to this point without a PortfolioId so fail if we do
				if (FPortfolioId == 0)
				{
					Result = "Invalid or missing PortfolioId - terminating";
				}
				
				int FilterEventId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					FilterEventId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(FbConnectionString, "ea_filtereventid_gen", out Result);
				}				
				
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "EXECUTE PROCEDURE EAEmailTransactionLoader(@CompanyId, @Login, @FilterEventId, @B1Value, @TransactionId, @TaskId)";
					Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
					Params.Add(new FbParameterItem("Login", CommonItem.ClientIdValue));
					Params.Add(new FbParameterItem("FilterEventId", FilterEventId));
					Params.Add(new FbParameterItem("B1Value", ""));
					Params.Add(new FbParameterItem("TransactionId", FPortfolioId));
					Params.Add(new FbParameterItem("TaskId", FTaskId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
				
				QueryText = "";
				Params.Clear();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						"SELECT FIRST 1 EA_TRANSACTIONTYPEID from eafilterevent where ea_filtereventid = @FilterEventId";
					Params.Add(new FbParameterItem("FilterEventId", FilterEventId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, 0);
				}
				
				DataTable tbl = null;
				object Obj = null;
				int TransactionTypeId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["EA_TRANSACTIONTYPEID"];
							TransactionTypeId = (Obj == DBNull.Value) ? 0: Convert.ToInt32(Obj);
						}
					}
				}
				
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("FilterEventId", FilterEventId));
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("TransactionTypeId", TransactionTypeId, false));
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				}
				else
				{
					Result = LogOnProcessingError(Method, Result, "");
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