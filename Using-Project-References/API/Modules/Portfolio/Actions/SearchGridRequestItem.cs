/*
 * Created by SharpDevelop.
 * User: John
 * Date: 01/07/2019
 * Time: 9:28 AM
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

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of SearchGridRequestItem.
	/// </summary>
	public class SearchGridRequestItem
	{
		#region Properties - Getters and Setters

		private string FOfficeIdValue = "";
		public int OfficeId { get; set; }			
		public string OfficeIdValue
		{
			get { return FOfficeIdValue; }
			set
			{
				FOfficeIdValue = value;
				OfficeId = StringUtilities.StrToIntDef(FOfficeIdValue, -1);				
			}
		}

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

		private string FPortfolioIdValue = "";
		public int PortfolioId { get; set; }			
		public string PortfolioIdValue
		{
			get { return FPortfolioIdValue; }
			set
			{
				FPortfolioIdValue = value;
				PortfolioId = StringUtilities.StrToIntDef(FPortfolioIdValue, -1);				
			}
		}
		#endregion
		
		#region Load from DataObject
		public string Load(JObject DataObject)
		{
			string Result = "";
			try 
			{
				OfficeIdValue = (string)DataObject["officeId"];
				ManagerIdValue = (string)DataObject["managerId"];
				PortfolioIdValue = (string)DataObject["portfolioId"];
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
		
		#region Load for CompanyId and ClientId
		public string Load(string ConnectionString,
		                   Int64 CompanyId,
		                   Int64 ClientId)
		{
			string Result = "";
			string Method = "Load";
			string ClassName = this.GetType().ToString();
			
			string QueryText =
				" SELECT" +
				"   I_ALogin_PortfolioBranchId AS OfficeId," +
				"   I_ALogin_PortfolioManagerId AS ManagerId," +
				"   I_ALogin_PortfolioPortfolioId AS PortfolioTypeId" +
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
					Obj = Row["PortfolioTypeId"];
					PortfolioIdValue = (Obj == DBNull.Value) ? "33" : Convert.ToString(Obj);
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
			
			return Result;
		}
		#endregion
		
		#region Clear
		private void Clear()
		{
			OfficeIdValue = "0";
			ManagerIdValue = "0";
			PortfolioIdValue = "33";
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
					Key = "PortfolioTypeId";
					if (string.IsNullOrEmpty(PortfolioIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (PortfolioId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
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
	}
}
