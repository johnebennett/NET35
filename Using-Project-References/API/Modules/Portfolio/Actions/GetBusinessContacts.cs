/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/09/2019
 * Time: 10:59 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of GetBusinessContacts.
	/// </summary>
	public class GetBusinessContacts : ActionBase
	{
		private string FModuleName = "Portfolio";
		private BusinessContactsItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			GetBusinessContacts inst = new GetBusinessContacts();
			//if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				FRequestItem = new BusinessContactsItem();
				Result = FRequestItem.Load(this, DataObject);
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
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try
			{
				StringBuilder Entries = new StringBuilder();
				ErrorMsg = BuildEntries(ref Entries);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				}
				else
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
			
			return Result;
		}
		
		private string BuildEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM EATRANSACTIONPHONESGROUP(@CompanyId, @PortfolioId, @CategoryOnlyId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("@PortfolioId", FRequestItem.PortfolioId));
			Params.Add(new FbParameterItem("CategoryOnlyId", FRequestItem.CategoryOnlyId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

			DataTable tbl = null;
			if (string.IsNullOrEmpty(Result))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				foreach (DataRow Row in tbl.Rows)
				{
					Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));				}
			}
			
			return Result;
		}
		
		private string BuildComboEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string QueryText = 
				" SELECT CategoryName as Name, CategoryId as \"Value\"" +
				" FROM EATRANSACTIONPHONESGROUP(@CompanyId, @PortfolioId, 0, 0)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("@PortfolioId", FRequestItem.PortfolioId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

			DataTable tbl = null;
			if (string.IsNullOrEmpty(Result))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = JsonUtilities.BuildJsonArrayFromDataTable(tbl, "comboEntries", ref Entries);
			}
			
			return Result;
		}
		
		private string BuildGridEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string QueryText = 
				" SELECT" +
                " I_ID, ZL20_Relation 15, ZL20_ContactType AS ZL10_Contact_Type," +
				" ZL20_ContactName AS ZL20_Contact_Name, ZL30_ContactDetails AS ZL30_Contact_Details," +
				" ZL30_ContactExtra AS ZL30_Contact_Extra, ZL30_Contact" + 
				" FROM EATRANSACTIONPHONESDROPDOWN(@CompanyId, @PortfolioId, 1, @CategoryOnlyId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("@PortfolioId", FRequestItem.PortfolioId));
			Params.Add(new FbParameterItem("@CategoryOnlyId", FRequestItem.CategoryOnlyId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

			DataTable tbl = null;
			if (string.IsNullOrEmpty(Result))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = JsonUtilities.BuildJsonArrayFromDataTable(tbl, "gridEntries", ref Entries);
			}
			
			return Result;
		}
	}
}
