/*
 * Created by SharpDevelop.
 * User: John
 * Date: 02/07/2019
 * Time: 9:43 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of StatementRunItem.
	/// </summary>
	public class StatementRunItem
	{
		private string FModuleName = "PaymentOperations";
		private int FMasterId = 0;
		#region constructor
		public StatementRunItem()
		{
			IdValue = "";
			CloseAccountRunIdValue = "";
			StatementPreview = "N";
			StatementRunType = "";
		}
		#endregion
		
		#region Properties - Getters and Setters
		
		private string FIdValue = "";
		public int Id { get; set; }
		public string IdValue
		{
			get { return FIdValue; }
			set 
			{
				FIdValue = value;
				Id = StringUtilities.StrToIntDef(FIdValue, -1);
			}
		}
		
		private string FCloseAccountRunIdValue = "";
		public int CloseAccountRunId { get; set; }			
		public string CloseAccountRunIdValue
		{
			get { return FCloseAccountRunIdValue; }
			set
			{
				FCloseAccountRunIdValue = value;
				CloseAccountRunId = StringUtilities.StrToIntDef(FCloseAccountRunIdValue, -1);				
			}
		}
		
		public string StatementPreview { get; set; }
		public string StatementRunType { get; set; }
		public string BPA { get; set; }
		public string DirectEntryDiskCreated { get; set; }
		public string MultiDirectRun { get; set; }
		
		private string FPaymentEventIdValue = "";
		public int PaymentEventId { get; set; }
		public string PaymentEventIdValue
		{
			get { return FPaymentEventIdValue; }
			set
			{
				FPaymentEventIdValue = value;
				PaymentEventId = StringUtilities.StrToIntDef(FPaymentEventIdValue, -1);
			}
		}
		
		private string FAccountTypeValue = "";
		public int AccountType { get; set; }
		public string AccountTypeValue
		{
			get { return FAccountTypeValue; }
			set
			{
				FAccountTypeValue = value;
				AccountType = StringUtilities.StrToIntDef(FAccountTypeValue, -1);
			}
		}
		#endregion
		
		public void Clear()
		{
			IdValue = "";
			CloseAccountRunIdValue = "";
			StatementPreview = "N";
			StatementRunType = "";
			BPA = "";
			DirectEntryDiskCreated = "";
			MultiDirectRun = "";
			PaymentEventIdValue = "";
			AccountTypeValue = "";
		}
		
		public string Load(string ConnectionString, Int64 CompanyId, Int64 CloseAccountRunId)
		{
			string Result = "";
			
			try
			{
				string QueryText =
					" SELECT select * from EASTATEMENTRUNVALUES(@CompanyId, @CloseAccountRunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("CloseAccountRunId", CloseAccountRunId));
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, FMasterId);
				
				foreach (FbParameterItem Param in Params)
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(
						Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue), FModuleName, FMasterId);
				}
	
				DataTable dt = null;
				try 
				{
					dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
					if (dt == null)
					{
						Result = "No tables in the dataset to process";
						return Result;
					}				
				}					
				catch (Exception Ex)
				{
					//Result = LogOnException(Method, Ex, "");
					Result = 
						Ex.Message + Environment.NewLine +
						Ex.InnerException + Environment.NewLine +
						Ex.StackTrace;
					return Result;
				}
				
				object Obj = null;
				Clear();
				foreach (DataRow Row in dt.Rows)
				{
					Obj = Row["I_AState_Id"];
					IdValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["I_Astr_CloseAccountRunId"];
					CloseAccountRunIdValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["B_ASTR_StatementPreview"];
					StatementPreview = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["ASTR_Type"];
					StatementRunType = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["B_ASTR_BPA"];
					BPA = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["ASTR_DirectEntryDiskCreated"];
					DirectEntryDiskCreated = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);

					Obj = Row["B_ASTR_MultiDirectRun"];
					MultiDirectRun = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["I_ASTR_PaymentEventId"];
					PaymentEventIdValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["I_ASTR_AccountType"];
					AccountTypeValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
				Result = Validate();
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
					Key = "IdValue";
					if (string.IsNullOrEmpty(IdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (Id < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "CloseAccountRunId";
					if (string.IsNullOrEmpty(CloseAccountRunIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (CloseAccountRunId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "StatementPreview";
					if (string.IsNullOrEmpty(StatementPreview)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "StatementRunType";
					if (string.IsNullOrEmpty(StatementRunType)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
				}
				
				// BPA can be NULL in the table
				// DirectEntryDiskCreated can be null in the table
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "AccountType";
					if (string.IsNullOrEmpty(AccountTypeValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (AccountType < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "PaymentEventId";
					if (string.IsNullOrEmpty(PaymentEventIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (PaymentEventId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "MultiDirectRun";
					if (string.IsNullOrEmpty(MultiDirectRun)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
	}
}
