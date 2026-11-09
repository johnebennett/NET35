/*
 * Created by SharpDevelop.
 * User: John
 * Date: 26/06/2019
 * Time: 4:13 PM
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
using CommonDBMethods;

namespace Actions.SettingsOperations
{
	#region UserSettingsItem
	public class UserSettingsItem
	{
		#region Properties - Getters and Setters
		public string Section { get; set; }

		private string FOfficeIdValue = "";
		public string OfficeIdValue
		{
			get { return FOfficeIdValue; }
			set
			{
				FOfficeIdValue = value;
				OfficeId = StringUtilities.StrToIntDef(FOfficeIdValue, -1);				
			}
		}
		public int OfficeId { get; set; }			
		
		private string FManagerIdValue = "";
		public int ManagerId { get; set; }
		public string ManagerIdValue
		{
			get { return FManagerIdValue; }
			set
			{
				FManagerIdValue = value; 
				ManagerId = StringUtilities.StrToIntDef(FManagerIdValue, -1);
			}
		}
		
		private string FBankNameIdValue = "";
		public int BankNameId { get; set; }
		public string BankNameIdValue
		{
			get { return FBankNameIdValue; }
			set
			{
				FBankNameIdValue = value;
				BankNameId = StringUtilities.StrToIntDef(FBankNameIdValue, -1);
			}
		}
		
		private string FLedgerTypeIdValue = "";
		public int LedgerTypeId { get; set; }
		public string LedgerTypeIdValue
		{
			get { return FLedgerTypeIdValue; }
			set
			{
				FLedgerTypeIdValue = value;
				LedgerTypeId = StringUtilities.StrToIntDef(FLedgerTypeIdValue, -1);
			}
		}
		
		private string FPaymentTypeIdValue = "";
		public int PaymentTypeId { get; set; }
		public string PaymentTypeIdValue
		{
			get { return FPaymentTypeIdValue; }
			set
			{
				FPaymentTypeIdValue = value;
				PaymentTypeId = StringUtilities.StrToIntDef(FPaymentTypeIdValue, -1);
			}
		}
		
		private string FPortfolioTypeIdValue = "";
		public int PortfolioTypeId { get; set; }
		public string PortfolioTypeIdValue
		{
			get { return FPortfolioTypeIdValue; }
			set
			{
				FPortfolioTypeIdValue = value;
				PortfolioTypeId = StringUtilities.StrToIntDef(FPortfolioTypeIdValue, -1);
			}
		}
		
		private string FDateRangeIdValue = "";
		public int DateRangeId { get; set; }
		public string DateRangeIdValue
		{
			get { return FDateRangeIdValue; }
			set
			{
				FDateRangeIdValue = value;
				DateRangeId = StringUtilities.StrToIntDef(FDateRangeIdValue, -1);
			}
		}
		#endregion
		
		#region Load from DataObject
		public string Load(JObject DataObject)
		{
			string Method = "Load";
			string ClassName = this.GetType().ToString();						
			string Result = "";
			
			UserSettingsItem TempItem = new UserSettingsItem();
			
			Result = CommonMethods.LoadUserSettingsItem(DataObject, ref TempItem);
			if (string.IsNullOrEmpty(Result)) 
			{
				Result = Update(TempItem);
			}

			/*
			string Result = "";
			try 
			{
				Section = (string)DataObject["section"];
				OfficeIdValue = (string)DataObject["officeId"];
				ManagerIdValue = (string)DataObject["managerId"];
				BankNameIdValue = (string)DataObject["bankNameId"];
				LedgerTypeIdValue = (string)DataObject["ledgerTypeId"];
				PaymentTypeIdValue = (string)DataObject["paymentTypeId"];
				PortfolioTypeIdValue = (string)DataObject["portfolioTypes"];
				DateRangeIdValue = (string)DataObject["dateRangeId"];				
			}
			catch (Exception Ex)
			{
				Result =
					Ex.InnerException + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			*/
			
			return Result;
		}
		#endregion
		
		#region Load for CompanyId and ClientId
		public string Load(string ConnectionString,
		                   Int64 CompanyId,
		                   Int64 ClientId,
		                   string Section)
		{
			string Method = "Load";
			string ClassName = this.GetType().ToString();						
			string Result = "";
			
			UserSettingsItem TempItem = new UserSettingsItem();
			
			Result = CommonMethods.LoadUserSettingsItem(ConnectionString, CompanyId, ClientId, Section, ref TempItem);
			if (string.IsNullOrEmpty(Result)) 
			{
				Result = Update(TempItem);
			}
			
			/*
			string QueryText =
				" SELECT" +
				"   I_ALogin_PortfolioBranchId AS OfficeId," +
				"   I_ALogin_PortfolioManagerId AS ManagerId," +
				"   I_ALogin_BankNameId AS BankNameId," +
				"   I_ALogin_LedgerTypeId AS LedgerTypeId," +
				"   I_ALogin_PaymentType AS PaymentTypeId," +
				"   I_ALogin_PortfolioPortfolioId AS PortfolioTypeId," +
				"   I_ALogin_PaymentDateOption AS DateRangeId" +
				" FROM EALogin" +
				" WHERE I_ALogin_CompanyId = @CompanyId" +
				"   AND I_ALogin_ClientId = @ClientId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@ClientId", ClientId));
			
			ActionBase actionBase = new ActionBase();
			actionBase.DebugMode = true;
			actionBase.LogOnInsideMethod(Method);

			DataTable dt = null;
			try 
			{
				dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
				if (dt == null)
				{
					Result = actionBase.LogOnProcessingError(Method, "No tables in the dataset to process", "");
					return Result;
				}
				
				Clear();
				
				if (dt.Rows.Count > 0)
				{
					DataRow Row = dt.Rows[0]; // there should only be 1 row but just in case only use the first row
					Object Obj = Row["OfficeId"];
					OfficeIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);					
					Obj = Row["ManagerId"];
					ManagerIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);					
					Obj = Row["BankNameId"];
					BankNameIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);					
					Obj = Row["LedgerTypeId"];
					LedgerTypeIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
					Obj = Row["PaymentTypeId"];
					PaymentTypeIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
					Obj = Row["PortfolioTypeId"];
					PortfolioTypeIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
					Obj = Row["DateRangeId"];
					DateRangeIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
				}
			}					
			catch (Exception Ex)
			{
				Result = actionBase.LogOnException(ClassName + "." + Method, Ex, "");
				return Result;
			}
			finally
			{
				actionBase.LogOnLeavingMethod(Method);
			}
			*/
			
			return Result;
		}
		#endregion
		
		#region Clear
		private void Clear()
		{
			OfficeIdValue = "0";
			ManagerIdValue = "0";
			BankNameIdValue = "0";
			LedgerTypeIdValue = "0";
			PaymentTypeIdValue = "0";
			PortfolioTypeIdValue = "0";
			DateRangeIdValue = "0";
		}
		#endregion
		
		#region Validate
		public string Validate()
		{
			string Result = "";
			try 
			{
				string BaseFormat = "{0} is {1}";
				string NotProvided = "Not Provided";
				string Invalid = "Invalid";				
				string Key = "";
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "Section";
					if (string.IsNullOrEmpty(Section)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "OfficeId";
					if (string.IsNullOrEmpty(OfficeIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (OfficeId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "ManagerId";
					if (string.IsNullOrEmpty(ManagerIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (ManagerId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "BankNameId";
					if (string.IsNullOrEmpty(BankNameIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (BankNameId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "LedgerTypeId";
					if (string.IsNullOrEmpty(LedgerTypeIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (LedgerTypeId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "PaymentTypeId";
					if (string.IsNullOrEmpty(PaymentTypeIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (PaymentTypeId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "PortfolioTypeId";
					if (string.IsNullOrEmpty(PortfolioTypeIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (PortfolioTypeId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "DateRangeId";
					if (string.IsNullOrEmpty(DateRangeIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (DateRangeId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
			}
			catch (Exception Ex)
			{
				Result =
					Ex.InnerException + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			
			return Result;
		}
		#endregion
		
		#region Save
		public string Save(string ConnectionString,
		                   Int64 CompanyId,
		                   Int64 ClientId)
		{
			string Result = CommonMethods.SaveUserSettingsItem(ConnectionString, CompanyId, ClientId, this);
			/*
			string QueryText =
				" UPDATE EALogin" +
				"    SET I_ALOGIN_PORTFOLIOBRANCHID = @OfficeId," +
				"        I_ALOGIN_PORTFOLIOMANAGERID = @ManagerId," +
				"        I_ALOGIN_PORTFOLIOPORTFOLIOID = @PortfolioTypeId," +
				"        I_ALOGIN_BANKNAMEID = @BankNameId," +
				"        I_ALOGIN_LEDGERTYPEID = @LedgerTypeId," +
				"        I_ALogin_PaymentType = @PaymentTypeId," +
				"        I_ALogin_PaymentDateOption = @DateRangeId" +
				" WHERE I_ALogin_CompanyId = @CompanyId" +
				"   AND I_ALogin_ClientId = @ClientId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@OfficeId", OfficeId));
			Params.Add(new FbParameterItem("@ManagerId", ManagerId));
			Params.Add(new FbParameterItem("@PortfolioTypeId", PortfolioTypeId));
			Params.Add(new FbParameterItem("@BankNameId", BankNameId));
			Params.Add(new FbParameterItem("@LedgerTypeId", LedgerTypeId));
			Params.Add(new FbParameterItem("@PaymentTypeId", PaymentTypeId));
			Params.Add(new FbParameterItem("@DateRangeId", DateRangeId));
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@ClientId", ClientId));
			Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			*/
			return Result;
		}
		#endregion
		
		public string Update(UserSettingsItem Source)
		{
            string Result = "";
            
            try
            {
            	this.Section = Source.Section;
				this.BankNameIdValue = Source.BankNameIdValue;
				this.DateRangeIdValue = Source.DateRangeIdValue;
				this.LedgerTypeIdValue = Source.LedgerTypeIdValue;
				this.ManagerIdValue = Source.ManagerIdValue;
				this.OfficeIdValue = Source.OfficeIdValue;
				this.PaymentTypeIdValue = Source.PaymentTypeIdValue;
				this.PortfolioTypeIdValue = Source.PortfolioTypeIdValue;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
		}
	}
	#endregion // SavePageEntries_UserSettingsItem
}
