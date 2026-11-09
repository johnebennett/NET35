/*
 * Created by SharpDevelop.
 * User: John
 * Date: 27/07/2020
 * Time: 4:22 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;

namespace Actions.Independent.AddEditBankAccountDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "AddEditBankAccountDlg"; }
		
		public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "AddEditMode");
				TempItem.AddEditModeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankNameId");
				TempItem.BankNameIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
			string Method = "SetupPageEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				int NewId = 0;
				if (string.IsNullOrEmpty(Result) && (Request.BankNameId == 0))
				{
					NewId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							ConnectionString, "I_ABANK_ID_GEN", CompanyId, ClientId, out Result);					
					if (string.IsNullOrEmpty(Result))
					{
						Result = EABankNameCreate(ConnectionString, CompanyId, NewId);
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						Request.BankNameId = NewId;
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("BankNameId", Request.BankNameId, false));
				}
				
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						"SELECT OutputLine FROM EABankNameMaintain(" +
						"@CompanyId, @BankNameId, @AddEditMode,'','','','','','','','','','','','','','','','', '','')";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("BankNameId", Request.BankNameId));
					Params.Add(new FbParameterItem("AddEditMode", Request.AddEditMode));
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
		
		public static string EABankNameCreate(string ConnectionString,
		                                      int CompanyId,
		                                      int NewBankNameId)
		{
			string Result = "";
			string Method = "EABankNameCreate";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "EXECUTE PROCEDURE EABankNameCreate(@CompanyId, @NewId)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("NewId", NewBankNameId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EABankNameCreate Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string DeleteBankAccount(string ConnectionString,
		                                       int CompanyId,
		                                       int ClientId,
		                                       SetupPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "DeleteBankAccount";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.DeleteSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update budget query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
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
		
		public static string LoadAllPageEntriesRequestItem(ActionBase ActionBase,
		                                                   ref AllPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAllPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = Item.Load(ActionBase);					
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.AddEditModeValue = Item.AddEditModeValue;
					TempItem.BankNameIdValue = Item.BankNameIdValue;
					
					Object Obj = ActionBase.GetCollectionValue("Data", "BankName");
					TempItem.BankName = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "BranchName");
					TempItem.BranchName = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "BSB");
					TempItem.BSB = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountNumber");
					TempItem.AccountNumber = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Description");
					TempItem.Description = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ShortBankName");
					TempItem.ShortBankName = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ForResidentialLedgers");
					TempItem.ForResidentialLedgers = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ForCommercialLedgers");
					TempItem.ForCommercialLedgers = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ForSalesLedgers");
					TempItem.ForSalesLedgers = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ForSecurityDepositLedgers");
					TempItem.ForSecurityDepositLedgers = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ForStrataLedgers");
					TempItem.ForStrataLedgers = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ForManagementLedgers");
					TempItem.ForManagementLedgers = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "CompanyName");
					TempItem.CompanyName = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "CompanyAddress");
					TempItem.CompanyAddress = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "EffectiveOfficer");
					TempItem.EffectiveOfficer = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ExcludeEOM");
					TempItem.ExcludeEOM = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "NSWUniqueTrustId");
					TempItem.NSWUniqueTrustId = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "DirectEntryUserNumber");
					TempItem.DirectEntryUserNumber = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Inactive");
					TempItem.Inactive = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string CopyRequestObject(AllPageEntriesRequestItem From,
		                                       ref SetupPageEntriesRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.AddEditModeValue = From.AddEditModeValue;
                To.BankNameIdValue = From.BankNameIdValue;
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string SaveBankAccount(string ConnectionString,
		                                     int CompanyId,
		                                     int ClientId,
		                                     AllPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "DeleteBankAccount";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = CopyRequestObject(Request, ref Item);
				}
				
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref TempEntries);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update budget query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("bankname", Request.BankName));
					Params.Add(new FbParameterItem("BRANCHNAME", Request.BranchName));
					Params.Add(new FbParameterItem("BSB", Request.BSB));
					Params.Add(new FbParameterItem("ACCOUNTNUMBER", Request.AccountNumber));
					Params.Add(new FbParameterItem("DESCRIPTION", Request.Description));
					Params.Add(new FbParameterItem("ShortBankName", Request.ShortBankName));
					Params.Add(new FbParameterItem("RESCHECK", Request.ForResidentialLedgers));
					Params.Add(new FbParameterItem("COMCHECK", Request.ForCommercialLedgers));
					Params.Add(new FbParameterItem("SECCHECK", Request.ForSecurityDepositLedgers));
					Params.Add(new FbParameterItem("SALCHECK", Request.ForSalesLedgers));
					Params.Add(new FbParameterItem("STRCHECK", Request.ForStrataLedgers));
					Params.Add(new FbParameterItem("MANCHECK", Request.ForManagementLedgers));
					Params.Add(new FbParameterItem("COMPANYADDRESS", Request.CompanyAddress));
					Params.Add(new FbParameterItem("COMPANYNAME", Request.CompanyName));
					Params.Add(new FbParameterItem("EFFECTIVEOFFICER", Request.EffectiveOfficer));
					Params.Add(new FbParameterItem("EXCLUDEFROMEOM", Request.ExcludeEOM));
					Params.Add(new FbParameterItem("NSWID", Request.NSWUniqueTrustId));
					Params.Add(new FbParameterItem("DIRECTENTRYUSERNUMBER", Request.DirectEntryUserNumber));
					Params.Add(new FbParameterItem("INACTIVE", Request.Inactive));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
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
	}
}
