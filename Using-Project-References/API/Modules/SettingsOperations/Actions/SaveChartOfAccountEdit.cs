/*
 * Created by SharpDevelop.
 * User: John
 * Date: 08/04/2020
 * Time: 12:07 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using Actions.SettingsOperations;

namespace Actions.SettingsOperations
{
	/// <summary>
	/// Description of SaveChartOfAccountEdit.
	/// </summary>
	public class SaveChartOfAccountEdit : ActionBase
	{
		private string FModuleName = "SettingsOperations";
		private SaveChartOfAccountEditRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SaveChartOfAccountEdit inst = new SaveChartOfAccountEdit();
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
				DataCollection = CommonMethods.MakeCollectionFromJObject(DataObject, out Result);
				if (string.IsNullOrEmpty(Result))
				{
					FRequestItem = new SaveChartOfAccountEditRequestItem();
					Result = FRequestItem.Load((ActionBase)this);
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
		
		private string Validate()
		{
			string Method = "Validate";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = FRequestItem.Validate();
				if (string.IsNullOrEmpty(Result))
				{
					Result = ValidateSaveChartOfAccountSettings();
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
		
		/// <summary>
		/// This is the minimum set of values that are required to
		/// save a record.
		/// Add any additional values here as required
		/// </summary>
		/// <returns>
		/// Returns an empty string if all is OK, otherwise
		/// returns an appropriate message if an entry is not required 
		/// or is invalid
		/// </returns>
		private string ValidateSaveChartOfAccountSettings()
		{
			string Result = "";
			string Method = "ValidateSaveChartOfAccountSettings";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			LogOnInsideMethod(Method);
			try
			{
				string ValueStr = "";
				if (string.IsNullOrEmpty(Result))
				{
					Result = ValidationChecks.ErrorIfZeroInteger(FRequestItem.RecordId, "Record ID");
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if ((FRequestItem.DlgMode < 0) || (FRequestItem.DlgMode > 1))
						Result = ValidationChecks.ErrorIfEmptyString("", "Dialog Mode");
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					ValueStr = FRequestItem.Description;
					if (StringUtilities.SameText(ValueStr, "ENTER DESCRIPTION")) ValueStr = "";
					Result = ValidationChecks.ErrorIfEmptyString(ValueStr, "Description");
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = ValidationChecks.ErrorIfZeroInteger(FRequestItem.Order2Id, "Order2 ID");
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = ValidationChecks.ErrorIfZeroInteger(FRequestItem.Order3Id, "Order3 ID");
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveSettings();
				}
				
				if (!string.IsNullOrEmpty(Result))
				{
					Result = LogOnProcessingError(Method, Result, "");
				}
				else
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
			return Result;
		}
		
		public string SaveSettings()
		{
			string Result = "";
			string Method = "SaveSettings";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, MasterId);
					
			LogOnInsideMethod(Method);
			try
			{
				// DlgMode == 0 - Add mode, DlgMode == 1 - Edit mode
				if (string.IsNullOrEmpty(Result) && (FRequestItem.DlgMode == 0))
				{
					Result = 
						CommonMethods.EAChartCodeValidate(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RecordId, FRequestItem.Account);
				}
				
				string UpdateScript = "";				
				if (string.IsNullOrEmpty(Result))
				{
					UpdateScript = GetUpdateScriptForRecordId(out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = ExecuteUpdateScript(UpdateScript);
				}
				
				if (string.IsNullOrEmpty(Result) && (FRequestItem.DlgMode == 0))
				{
					string QueryText = "UPDATE EAChart SET AChart_AccountNumber = @AccountNumber WHERE I_ACHART_ID = @RecordId";
					List<FbParameterItem> Params = new List<FbParameterItem>();
					Params.Add(new FbParameterItem("AccountNumber", FRequestItem.Account));
					Params.Add(new FbParameterItem("RecordId", FRequestItem.RecordId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, 0);
					
					if (string.IsNullOrEmpty(Result))
						Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
				
				/*
				string Description = "";
				if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(UpdateScript))
				{
					Description = CollectionUtilities.GetCollectionValue(DataCollection, "Description", "");
					UpdateScript = UpdateScript.Replace("__ENTER DESCRIPTION__", "@DESCRIPTION");
					UpdateScript = UpdateScript.Replace("@ENTER DESCRIPTION@", "@DESCRIPTION");
					List<FbParameterItem> Params = new List<FbParameterItem>();
					Params.Add(new FbParameterItem("Descrption", Description));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateScript, Params, Method, 0);
					
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(FbConnectionString, UpdateScript, Params, 0);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = UpdateCheckBoxValues();
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
		
		public string GetUpdateScriptForRecordId(out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetUpdateScriptForRecordId";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				StringBuilder gatherItems = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						ChartOfAccountSettings.GatherChartOfAccountItems(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RecordId, FRequestItem.DlgMode, ref gatherItems);
				}
				
				string Str = "";
				JObject JObj = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Str =
						JsonUtilities.StartJson() +
						gatherItems.ToString() +
						JsonUtilities.EndJson();
					WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, Method, MasterId);
				}
				
				if (gatherItems.Length > 0)
				{
					JObj = JObject.Parse(Str);
				}				
			
				if (string.IsNullOrEmpty(ErrorMsg) && (JObj != null))
				{
					Result = (string)JObj["UpdateSQL"];
					WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
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
		
		public string UpdateCheckBoxValues()
		{
			string Result = "";
			string Method = "UpdateCheckBoxValues";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				List<string> CheckBoxList = new List<string>();
				List<FbParameterItem> Params = new List<FbParameterItem>();
				NameValueCollection Coll = DataCollection;
				
				int RecordId = 0;
				string Key = "RecordId";
				string Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
				Result = ValidationChecks.ErrorIfEmptyString(Value, Key);
				if (string.IsNullOrEmpty(Result))
				{
					RecordId = StringUtilities.StrToIntDef(Value, 0);					
					Result = ValidationChecks.ErrorIfZeroInteger(RecordId, Key);
				}
				
				if (string.IsNullOrEmpty(Result))
				{				
					Key = "Hide";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value))
					{
						CheckBoxList.Add("B_ACHART_HIDE= @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
					
					Key = "Advertising";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value))
					{
						CheckBoxList.Add("B_ACHART_ADVERTISING = @" +Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
					
					Key = "CommissionOnRental";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value))
					{
						CheckBoxList.Add("B_ACHART_COMMISSION = @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
	
					Key = "TenancySchedule";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value)) 
					{
						CheckBoxList.Add("B_ACHART_TENANCYSCHEDULE = @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
	
					Key = "Outgoings";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value)) 
					{
						CheckBoxList.Add("B_ACHART_OUTGOINGS = @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
	
					Key = "NotOnBAS";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value))
					{
						CheckBoxList.Add("B_ACHART_NOTONBAS = @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
	
					Key = "SecurityDeposit";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value))
					{
						CheckBoxList.Add("B_ACHART_SECURITYDEPOSIT = @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
	
					Key = "CommissionOnSale";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value))
					{
						CheckBoxList.Add("B_ACHART_COMMISSIONONSALE = @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
	
					Key = "Insurance";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value))
					{
						CheckBoxList.Add("B_ACHART_INSURANCE = @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
	
					/*
					Key = "PenaltyInterest";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value))
					{
						CheckBoxList.Add("???? = @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
					*/
	
					Key = "DoNoChargeInterest";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value))
					{
						CheckBoxList.Add("B_ACHART_DONOTCHARGEINTEREST = @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}
	
					Key = "GroupOnStatement";
					Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
					if (!string.IsNullOrEmpty(Value))
					{
						CheckBoxList.Add("B_ACHART_GROUPONSTATEMENT = @" + Key);
						Params.Add(new FbParameterItem(Key, Value));
					}

                    /*
                    Key = "Compliance";
                    Value = CollectionUtilities.GetCollectionValue(Coll, Key, "");
                    WindowsEventLoggingUtilities.WriteToInfoEvent("CheckBoxList: ComplianceValue: " + Value, Method, MasterId);
                    if (!string.IsNullOrEmpty(Value))
                    {
                        CheckBoxList.Add("B_ACHART_COMPLIANCE = @" + Key);
                        Params.Add(new FbParameterItem(Key, Value));
                    }
                    */
                }

                WindowsEventLoggingUtilities.WriteToInfoEvent("CheckBoxList: " + string.Join(", ", CheckBoxList.ToArray()), Method, MasterId);

                string QueryText = "";
				if (string.IsNullOrEmpty(Result) && (CheckBoxList.Count > 0))
				{
					if (CheckBoxList.Count != Params.Count)
					{
						Result = "Invalid Param count in " + Method;
					}
					else
					{
						QueryText =
							" UPDATE EAChart SET" + Environment.NewLine +
							string.Join(", ", CheckBoxList.ToArray()) + Environment.NewLine +
							" WHERE I_ACHART_ID = @RecordId";
						Params.Add(new FbParameterItem("RecordId", RecordId));
					}
				}
				
				if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(QueryText))
				{
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, MasterId);
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
					}
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
		
		private string ExecuteUpdateScript(string QueryText)
		{
			string Result = "";
			string Method = "ExecuteUpdateScript";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				// Make the list of parameters contained in the QueryText script
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Result = BuildUpdateScriptParams(QueryText, ref Params);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
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
		
		private string BuildUpdateScriptParams(string QueryText,
		                                       ref List<FbParameterItem> Params)
		{
			string Result = "";
			string Method = "BuildUpdateScriptParams";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string Key = "@B_ACHART_HIDE";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Hide));
				
				Key = "@B_ACHART_ADVERTISING";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Advertising));
				
				Key = "@B_ACHART_COMMISSION";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.CommissionOnRental));
				
				Key = "@B_ACHART_TENANCYSCHEDULE"; 
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.TenancySchedule));
				
				Key = "@B_ACHART_OUTGOINGS"; 
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Outgoings));
				
				Key = "@B_ACHART_NOTONBAS";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.NotOnBAS));
				
				Key = "@B_ACHART_SECURITYDEPOSIT";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.SecurityDeposit));
				
				Key = "@B_ACHART_COMMISSIONONSALE";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.CommissionOnSale));
				
				Key = "@B_ACHART_INSURANCE";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Insurance));
				
				Key = "@B_ACHART_DONOTCHARGEINTEREST";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.DoNotChargeInterest));
				
				Key = "@B_ACHART_PENALTYCHARTCODE";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.PenaltyInterest));
				
				Key = "@B_ACHART_GROUPONSTATEMENT";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.GroupOnStatement));
				
				Key = "@I_ACHART_GROUP3ID";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Order3Id));
				
				Key = "@I_ACHART_GROUP2ID";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Order2Id));
				
				Key = "@ACHART_ACCOUNTNUMBERDESCRIPTION";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Description));
				
				Key = "@I_ACHART_GROUP2ORDER";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Order2));
				
				Key = "@I_ACHART_GROUP3ORDER";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Order3));
				
				Key = "@I_ACHART_ACCOUNTNUMBERORDER";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.NumberOrder));
				
				Key = "@UNIQUEID";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.RecordId));
				
				Key = "@B_ACHART_INVOICEHANDLING";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.InvoiceHandling));
				
				Key = "@B_ACHART_MAINTENANCE";
				if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Maintenance));

                Key = "@B_ACHART_COMPLIANCE";
                if (QueryText.Contains(Key)) Params.Add(new FbParameterItem(Key, FRequestItem.Compliance));
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
