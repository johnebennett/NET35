/*
 * Created by SharpDevelop.
 * User: John
 * Date: 05/01/2019
 * Time: 6:45 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation
{
	/// <summary>
	/// UpdateBankAccountItemItem represents an item being used
	/// to update a BankAccountItem in the database	/// 
	/// </summary>
	public class UpdateBankAccountItemItem
	{
		private string FClassName = "UpdateBankAccountItemItem";
		
		public string BankId { get; set; }
		public string Active { get; set; }
		public string General { get; set; }
		public string IBD { get; set; }
		public string Residential { get; set; }
		public string Commercial { get; set; }
		public string Strata { get; set; }
		public string Sales { get; set; }
		public string Management { get; set; }
		public string Bond { get; set; }

		private ActionParameterStatus FBankIdStatus = ActionParameterStatus.OK;
		private ActionParameterStatus FActiveStatus = ActionParameterStatus.OK;
		private ActionParameterStatus FGeneralStatus = ActionParameterStatus.OK;
		private ActionParameterStatus FIBDStatus = ActionParameterStatus.OK;
		private ActionParameterStatus FResidentialStatus = ActionParameterStatus.OK;
		private ActionParameterStatus FCommercialStatus = ActionParameterStatus.OK;
		private ActionParameterStatus FStrataStatus = ActionParameterStatus.OK;
		private ActionParameterStatus FSalesStatus = ActionParameterStatus.OK;
		private ActionParameterStatus FManagementStatus = ActionParameterStatus.OK;
		private ActionParameterStatus FBondStatus = ActionParameterStatus.OK;

		public ActionParameterStatus BankIdStatus { get {return FBankIdStatus; } }
		public ActionParameterStatus ActiveStatus { get { return FActiveStatus; } }
		public ActionParameterStatus GeneralStatus { get { return FGeneralStatus; } }
		public ActionParameterStatus IBDStatus { get { return FIBDStatus; } }
		public ActionParameterStatus ResidentialStatus { get {return FResidentialStatus; } }
		public ActionParameterStatus CommercialStatus { get { return FCommercialStatus; } }
		public ActionParameterStatus StrataStatus { get { return FStrataStatus; } }
		public ActionParameterStatus SalesStatus { get { return FSalesStatus; } }
		public ActionParameterStatus ManagementStatus { get { return FManagementStatus; } }
		public ActionParameterStatus BondStatus { get { return FBondStatus; } }
		
		public UpdateBankAccountItemItem()
		{
			this.FClassName = this.GetType().ToString();
			this.BankId = "";
			this.Active = "";
			this.General = "";
			this.IBD = "";
			this.Residential = "";
			this.Commercial = "";
			this.Strata = "";
			this.Sales = "";
			this.Management = "";
			this.Bond = "";

			this.FBankIdStatus = ActionParameterStatus.OK;
			this.FActiveStatus = ActionParameterStatus.OK;
			this.FGeneralStatus = ActionParameterStatus.OK;
			this.FIBDStatus = ActionParameterStatus.OK;
			this.FResidentialStatus = ActionParameterStatus.OK;
			this.FCommercialStatus = ActionParameterStatus.OK;
			this.FStrataStatus = ActionParameterStatus.OK;
			this.FSalesStatus = ActionParameterStatus.OK;
			this.FManagementStatus = ActionParameterStatus.OK;
			this.FBondStatus = ActionParameterStatus.OK;
		}
		
		public void LoadFromRequest(JObject DataObject, out string ErrorMsg)
		{
			string Method = "LoadFromRequest";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FClassName, 0);
			ErrorMsg = "";
			try
			{
				string Key = "";				
				string Value = "";
				Object obj = null;
				int DummyInt =  0;
				
				try 
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Building BankId", FClassName, 0);
					Key = "bankId";
					obj = DataObject[Key];
					BankId = "";
					//FBankIdStatus = ActionParameterStatus.OK;
					FBankIdStatus = ActionParameterStatus.OK;
					if (obj == null)
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("BankId IS NULL", FClassName, 0);
						FBankIdStatus = ActionParameterStatus.NotProvided;
					}
					else 
					{
						BankId = (string)DataObject[Key];
						if (CommonUtilities.StringUtilities.SameText(BankId, "") ||
						    !int.TryParse(BankId,  out DummyInt))
						{
							FBankIdStatus = ActionParameterStatus.Invalid;
						}
					}
					
					WindowsEventLoggingUtilities.WriteToInfoEvent("Building ActiveFlag", FClassName, 0);
					Key = "active";
					obj = DataObject[Key];
					Active = "";
					FActiveStatus = ActionParameterStatus.OK;
					if (obj == null) FActiveStatus = ActionParameterStatus.NotProvided;
					else 
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Before setting Active Value", FClassName, 0);
						//Active = (string)obj;
						Active = (string)DataObject[Key];
						WindowsEventLoggingUtilities.WriteToInfoEvent("Active = " + Active, FClassName, 0);
						if (CommonUtilities.StringUtilities.SameText(Active, "") ||
						    !(CommonUtilities.StringUtilities.SameText(Active, "Y") ||
						      CommonUtilities.StringUtilities.SameText(Active, "N")))
						{
							FActiveStatus = ActionParameterStatus.Invalid;
						}
					}
					
					WindowsEventLoggingUtilities.WriteToInfoEvent("Building GeneralFlag", FClassName, 0);
					Key = "general";
					obj = DataObject[Key];
					General = "";
					FGeneralStatus = ActionParameterStatus.OK;
					if (obj == null) FGeneralStatus = ActionParameterStatus.NotProvided;
					else 
					{
						General = (string)DataObject[Key];
						if (CommonUtilities.StringUtilities.SameText(General, "") ||
						    !(CommonUtilities.StringUtilities.SameText(General, "Y") ||
						      CommonUtilities.StringUtilities.SameText(General, "N")))
						{
							FGeneralStatus = ActionParameterStatus.Invalid;
						}
					}
					
					Key = "ibd";
					obj = DataObject[Key];
					IBD = "";
					FIBDStatus = ActionParameterStatus.OK;
					if (obj == null) FIBDStatus = ActionParameterStatus.NotProvided;
					else 
					{
						IBD = (string)DataObject[Key];
						if (CommonUtilities.StringUtilities.SameText(IBD, "") ||
						    !(CommonUtilities.StringUtilities.SameText(IBD, "Y") ||
						      CommonUtilities.StringUtilities.SameText(IBD, "N")))
						{
							FIBDStatus = ActionParameterStatus.Invalid;
						}
					}
					
					Key = "residential";
					obj = DataObject[Key];
					Residential = "";
					FResidentialStatus = ActionParameterStatus.OK;
					if (obj == null) FResidentialStatus = ActionParameterStatus.NotProvided;
					else 
					{
						Residential = (string)DataObject[Key];
						if (CommonUtilities.StringUtilities.SameText(Residential, "") ||
						    !(CommonUtilities.StringUtilities.SameText(Residential, "Y") ||
						      CommonUtilities.StringUtilities.SameText(Residential, "N")))
						{
							FResidentialStatus = ActionParameterStatus.Invalid;
						}
					}
					
					Key = "commercial";
					obj = DataObject[Key];
					Commercial = "";
					FCommercialStatus = ActionParameterStatus.OK;
					if (obj == null) FCommercialStatus = ActionParameterStatus.NotProvided;
					else 
					{
						Commercial = (string)DataObject[Key];
						Value = Commercial;
						if (CommonUtilities.StringUtilities.SameText(Value, "") ||
						    !(CommonUtilities.StringUtilities.SameText(Value, "Y") ||
						      CommonUtilities.StringUtilities.SameText(Value, "N")))
						{
							FCommercialStatus = ActionParameterStatus.Invalid;
						}
					}
					
					Key = "strata";
					obj = DataObject[Key];
					Strata = "";
					FStrataStatus = ActionParameterStatus.OK;
					if (obj == null) FStrataStatus = ActionParameterStatus.NotProvided;
					else 
					{
						Strata = (string)DataObject[Key];
						Value = Strata;
						if (CommonUtilities.StringUtilities.SameText(Value, "") ||
						    !(CommonUtilities.StringUtilities.SameText(Value, "Y") ||
						      CommonUtilities.StringUtilities.SameText(Value, "N")))
						{
							FStrataStatus = ActionParameterStatus.Invalid;
						}
					}
					
					Key = "sales";
					obj = DataObject[Key];
					Sales = "";
					FSalesStatus = ActionParameterStatus.OK;
					if (obj == null) FSalesStatus = ActionParameterStatus.NotProvided;
					else 
					{
						Sales = (string)DataObject[Key];
						Value = Sales;
						if (CommonUtilities.StringUtilities.SameText(Value, "") ||
						    !(CommonUtilities.StringUtilities.SameText(Value, "Y") ||
						      CommonUtilities.StringUtilities.SameText(Value, "N")))
						{
							FSalesStatus = ActionParameterStatus.Invalid;
						}
					}
					
					Key = "management";
					obj = DataObject[Key];
					Management  = "";
					FManagementStatus = ActionParameterStatus.OK;
					if (obj == null) FManagementStatus = ActionParameterStatus.NotProvided;
					else 
					{
						Management = (string)DataObject[Key];
						Value = Management;
						if (CommonUtilities.StringUtilities.SameText(Value, "") ||
						    !(CommonUtilities.StringUtilities.SameText(Value, "Y") ||
						      CommonUtilities.StringUtilities.SameText(Value, "N")))
						{
							FManagementStatus = ActionParameterStatus.Invalid;
						}
					}
					
					Key = "bond";
					obj = DataObject[Key];
					Bond  = "";
					FBondStatus = ActionParameterStatus.OK;
					if (obj == null) FBondStatus = ActionParameterStatus.NotProvided;
					else 
					{
						Bond = (string)DataObject[Key];
						Value = Bond;
						if (CommonUtilities.StringUtilities.SameText(Value, "") ||
						    !(CommonUtilities.StringUtilities.SameText(Value, "Y") ||
						      CommonUtilities.StringUtilities.SameText(Value, "N")))
						{
							FBondStatus = ActionParameterStatus.Invalid;
						}
					}
					WindowsEventLoggingUtilities.WriteToInfoEvent("Finished Building BondFlag", FClassName, 0);
				}
				catch (Exception Ex)
				{
					ErrorMsg = Ex.Message;
				}				
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message;
			}
		}
		
		public void Validate(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Key = "";
			string BaseFormat = "{0} is {1}";
			string NotProvided = "Not Provided";
			string Invalid = "Invalid";
			
			try 
			{
				Key = "BankIdStatus";
				if (BankIdStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (BankIdStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}

				Key = "ActiveStatus";
				if (ActiveStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (ActiveStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}

				Key = "GeneralStatus";
				if (GeneralStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (GeneralStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}

				Key = "IBDStatus";
				if (IBDStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (IBDStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}

				Key = "ResidentialStatus";
				if (ResidentialStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (ResidentialStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}

				Key = "CommercialStatus";
				if (CommercialStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (CommercialStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}

				Key = "StrataStatus";
				if (StrataStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (StrataStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}

				Key = "SalesStatus";
				if (SalesStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (SalesStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}

				Key = "ManagementStatus";
				if (ManagementStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (ManagementStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}

				Key = "BondStatus";
				if (BondStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (BondStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message;
			}
		}
	}
	
	/// <summary>
	/// Description of BankAccountList.
	/// </summary>
	public class UpdateBankAccountItem : ActionBase
	{
		private string FModuleName = "Reconcilliation";
		private UpdateBankAccountItemItem WorkItem = new UpdateBankAccountItemItem();
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "DefaultMethods - Execute";
			string Result = "";
			ErrorMsg = "";

			UpdateBankAccountItem inst = new UpdateBankAccountItem();
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
				WorkItem.LoadFromRequest(DataObject, out Result);
				//if (string.IsNullOrEmpty(Result)) AItem.Validate(out Result);
				
				/*
				FActive = (string)DataObject["active"];
				FGeneral = (string)DataObject["general"];
				FIBD = (string)DataObject["ibd"];
				FResidential = (string)DataObject["residential"];
				FCommercial = (string)DataObject["commercial"];
				FStrata = (string)DataObject["strata"];
				FSales = (string)DataObject["sales"];
				FManagement = (string)DataObject["management"];
				FBond = (string)DataObject["bond"];
				
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
				WorkItem.Validate(out Result);
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				//Result = "{\"message\": \"hello\"}";
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
	}
}
