/*
 * Created by SharpDevelop.
 * User: John
 * Date: 20/06/2019
 * Time: 12:10 PM
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
	/// Description of GetSearchGridResults.
	/// </summary>
	public class GetSearchGridResults : ActionBase
	{
		private string FModuleName = "PaymentOperations";
		
		/*
		private string FDisplayTypeValue = "";
		private int FDisplayType = 0;
		private string FStatementRunIdValue = "";
		private int FStatementRunId = 0;
		private string FOfficeIdValue = "";
		private int FOfficeId = 0;
		private string FManagerIdValue = "";
		private int FManagerId = 0;
		private string FLedgerTypeIdValue = "";
		private int FLedgerTypeId = 0;
		private string FBankNameIdValue = "";
		private int FBankNameId = 0;
		private string FPaymentTypeIdValue = "";
		private int FPaymentTypeId = 0;
		private string FLedger = "";
		private string FDateRangeIdValue = "";
		private int FDateRangeId = 0;
		
		private int FLedgerId = 0;
		*/
		
		private SearchGridRequestItem FSearchGridRequestItem = new SearchGridRequestItem();
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			GetSearchGridResults inst = new GetSearchGridResults();
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
				Result = FSearchGridRequestItem.Load(this, DataObject);
                /*
				if (string.IsNullOrEmpty(Result))
				{
					string SettingsSection = "userDefaults";
					string UseDefaults = (string)DataObject["useDefaults"];
					if (StringUtilities.SameText(UseDefaults, "Y"))
					{
						Actions.SettingsOperations.UserSettingsItem userSettings = 
							new Actions.SettingsOperations.UserSettingsItem();
						userSettings.Section = SettingsSection;
						Result = userSettings.Load((ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, SettingsSection);
						if (string.IsNullOrEmpty(Result))
						{
							//FSearchGridRequestItem.ManagerIdValue = userSettings.ManagerIdValue;
							//FSearchGridRequestItem.OfficeIdValue = userSettings.OfficeIdValue;
							
							//FSearchGridRequestItem.BankNameIdValue = userSettings.BankNameIdValue;
							//FSearchGridRequestItem.DateRangeIdValue = userSettings.DateRangeIdValue;
							//FSearchGridRequestItem.LedgerTypeIdValue = userSettings.LedgerTypeIdValue;
							//FSearchGridRequestItem.PaymentTypeIdValue = userSettings.PaymentTypeIdValue;
						}
						
						if (string.IsNullOrEmpty(Result))
						{
							SettingsSection = "paymentDefaults";
							Actions.SettingsOperations.PaymentSettingsItem paymentSettings =
								new Actions.SettingsOperations.PaymentSettingsItem();
							paymentSettings.Section = SettingsSection;
							Result = paymentSettings.Load((ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue, SettingsSection);
							if (string.IsNullOrEmpty(Result))
							{
								//FSearchGridRequestItem.BankNameIdValue = paymentSettings.BankNameIdValue;
								//FSearchGridRequestItem.DateRangeIdValue = paymentSettings.DateRangeIdValue; // Keep this one only
								//FSearchGridRequestItem.LedgerTypeIdValue = paymentSettings.LedgerTypeIdValue;
								//FSearchGridRequestItem.PaymentTypeIdValue = paymentSettings.PaymentTypeIdValue;
							}
						}						
					}
				}
				*/
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
				// Ledger can be empty
				Result = FSearchGridRequestItem.Validate();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
			return Result;
		}
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
					
				ErrorMsg = BuildOutputEntries(ref entries);				
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				entries.AppendLine(JsonUtilities.EndJson());
				Result = entries.ToString();
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
		
		private string BuildOutputEntries(ref StringBuilder Entries)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
                /*
				string QueryText =
					" SELECT " +
					"   RUNID as Run_ID," +
					"   ZL15_Type AS ZL13_Run_Type," +
					"   ZL15_AddedBy," +
					"   ZL20_LEDGER_TYPES_PAID AS ZL10_Ledger_Type" +
					"   ,ZC10_DATE AS D_Date" +
					"   ,(select OUTPUTVALUE from formatcurrencyforlist(ZR10_AMOUNT)) as M_Total" +
					"   ,I_PAYMENTS" +
					"   ,I_Directs" +
					"   ,I_Cheques" +
					"   ,ZC8_Archived" +
                    "   ,ShowEdit" +
                    "   ,ShowTrash" +
                    "   ,ZL3_B1" +
                    "   ,ZL3_B2" +
                    "   ,ZL3_B3" +
                    " FROM EAPAYMENTLIST (" +
					"   @DisplayType," + // 0, - Display List, 1 - Display Statement Run ID entries
					"   @StatementRunId," + // 0, - Statement RUNID  0  is all
					"   @CompanyClientId," +
					"   @BankId," + // BANKID ,      0  is all
					"   @BranchId," + // BRANCHID ,    0  is all
					"   @ManagerId," + // MANAGERID,    0  is all
					"   @LedgerTypeId," + // LEDGERTYPEID, 0  is all
					"   @LedgerId," + // LEDGERID,     0  is all
					"   @PaymentTypeId," + // PAYMENTTYPE ) '' is all, ACCEPTS 'PAYMENT,STATEMENT,PREVIEW,ADVANCE,BPAY,MULTI'
					"   @DateRangeId" +
					" )" +
					" ORDER BY RunId DESC";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("DisplayType", FSearchGridRequestItem.DisplayType));
				Params.Add(new FbParameterItem("StatementRunId", FSearchGridRequestItem.StatementRunId));
				Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("BankId", FSearchGridRequestItem.BankNameId));
				Params.Add(new FbParameterItem("BranchId", FSearchGridRequestItem.OfficeId));
				Params.Add(new FbParameterItem("ManagerId", FSearchGridRequestItem.ManagerId));
				Params.Add(new FbParameterItem("LedgerTypeId", FSearchGridRequestItem.LedgerTypeId));
				Params.Add(new FbParameterItem("LedgerId", FSearchGridRequestItem.LedgerId));
				Params.Add(new FbParameterItem("PaymentTypeId", FSearchGridRequestItem.PaymentTypeId));
				Params.Add(new FbParameterItem("DateRangeId", FSearchGridRequestItem.DateRangeId));
                UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

				DataTable dt = null;
				try 
				{
					dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);
					if (dt == null)
					{
						Result = "No tables in the dataset to process";
						Result = LoggingEntry(Method, Result, "");
						return Result;
					}
				}					
				catch (Exception Ex)
				{
					Result = LogOnException(Method, Ex, "");
					return Result;
				}
				
				string Str = "";
				string ColName = "";
				string ColValue = "";
				List<string> RowList = new List<string>();
				List<string> StrList = new List<string>();
				object Obj = null;
				Entries.AppendLine(JsonUtilities.StartJsonArray("Items"));
				foreach (DataRow ThisRow in dt.Rows)
				{
					StrList.Clear();
					foreach (DataColumn ThisCol in dt.Columns)
					{
						ColName = ThisCol.ColumnName;
						Obj = ThisRow[ColName];
						ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						StrList.Add(JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
					}
					Str =
						JsonUtilities.StartJsonObject("") +
						string.Join(",", StrList.ToArray()) +
						JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				Entries.Append(string.Join(",", RowList.ToArray()));
				Entries.AppendLine(JsonUtilities.EndJsonArray());
				*/

                Result =
                    CommonMethods.GetSearchGridResults(
                        FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
                        FSearchGridRequestItem, ref Entries);
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
