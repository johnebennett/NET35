/*
 * Created by SharpDevelop.
 * User: John
 * Date: 25/09/2019
 * Time: 11:15 AM
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

namespace Actions.ComboBoxEntries
{
	/// <summary>
	/// Description of CommonFilteredLookups.
	/// </summary>
	public class CommonFilteredLookups : ActionBase
	{
		private string FModuleName = "ComboBoxEntries";
		private CommonFilteredLookupsItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			CommonFilteredLookups inst = new CommonFilteredLookups();
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
				FRequestItem = new CommonFilteredLookupsItem();
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
				StringBuilder Entries = new StringBuilder();
				switch (FRequestItem.DlgType)
				{
					case "PortfolioContacts":
						ErrorMsg = 
							CommonMethods.BuildOutput_PortfolioContacts(
								FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, FRequestItem.CategoryOnlyId, ref Entries);
						break;
					case "PortfolioStorage":
						ErrorMsg = 
							CommonMethods.BuildOutput_PortfolioStorage(
								FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, FRequestItem.CategoryOnlyId, ref Entries);
						break;
					case "PortfolioTasks":
						ErrorMsg = 
							CommonMethods.BuildOutput_PortfolioTasks(
								FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, FRequestItem.CategoryOnlyId, ref Entries);
						break;
					case "PortfolioNotes":
						ErrorMsg = 
							CommonMethods.BuildOutput_PortfolioNotes(
								FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.PortfolioId, 
								FRequestItem.CategoryOnlyId, ref Entries);
						break;
					case "PortfolioFinancials":
						ErrorMsg = 
							CommonMethods.BuildOutput_PortfolioFinancials(
								FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, FRequestItem.CategoryOnlyId, ref Entries);								
						break;
					default:
						ErrorMsg = "Unknown dialog type provided";
						break;
				}
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
			return Result;
		}
	}
}
