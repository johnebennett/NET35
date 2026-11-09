/*
 * Created by SharpDevelop.
 * User: John
 * Date: 12/06/2020
 * Time: 1:43 PM
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
namespace Actions.Portfolio.VacateDlg
{
	/// <summary>
	/// Description of SetupPageEntries.
	/// </summary>
	public class SetupPageEntries : ActionBase
	{
		private string FModuleName = "Portfolio";
		private Management.ManagementPageRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SetupPageEntries inst = new SetupPageEntries();
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
				FRequestItem = new Management.ManagementPageRequestItem();
				Result = FRequestItem.Load((ActionBase)this);
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
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						Management.CommonMethods.SaveManagementEntries(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.RemoveChargeTableEntry(FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId);
				}
				
				string OutputMessage = "";
				int OutputId = 0;
				string ShowVacateForm = "";
				string ShowUnvacateForm = "";
				string OutputVacateDate = "";
				string InsertSql = "";
				string SelectSql = "";				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EATransactionVacateValidate(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.PortfolioId,
							out OutputMessage, out OutputId, out ShowVacateForm, out ShowUnvacateForm, out OutputVacateDate,
							out InsertSql, out SelectSql);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg) && (ShowVacateForm == "N") && (ShowUnvacateForm == "N"))
				{
					ErrorMsg = OutputMessage;
				}
				
				if (string.IsNullOrEmpty(ErrorMsg) && (ShowVacateForm == "Y"))
				{
					//if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(OutputMessage)) ErrorMsg = OutputMessage;
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg =
							CommonMethods.ShowVacateForm(
								FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
								FRequestItem.PortfolioId, OutputId, InsertSql, SelectSql, ref Entries);
					}
				}
				else if (string.IsNullOrEmpty(ErrorMsg) && (ShowUnvacateForm == "Y"))
				{
					ErrorMsg =
						CommonMethods.ShowUnVacateForm(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
							FRequestItem.PortfolioId, OutputId, InsertSql, ref Entries);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Entries.Length = 0;
						Entries.Append(JsonUtilities.AddPropertyNameAndValue("UndoVacateStatus", "Success", false));
						
						/*
						ErrorMsg =
							Management.CommonMethods.EAResComSalesGroup(
								FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
								FRequestItem.PortfolioId, FRequestItem.NotesId, FRequestItem.Option1Id, FRequestItem.Option2Id, ref Entries);
						*/
					}
				}
				else
				{
					//ErrorMsg = "Invalid Values: Vacate ->" + ShowVacateForm + "<- UnVacate ->" + ShowUnvacateForm + "<-";
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					/*
					ErrorMsg =
						Management.CommonMethods.EAResComSalesGroup(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
							FRequestItem.PortfolioId, FRequestItem.NotesId, FRequestItem.Option1Id, FRequestItem.Option2Id, ref Entries);
					*/
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
			}
			return Result;
		}
	}
}