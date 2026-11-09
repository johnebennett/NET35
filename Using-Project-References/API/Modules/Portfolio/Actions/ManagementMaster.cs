/*
 * Created by SharpDevelop.
 * User: John
 * Date: 04/06/2020
 * Time: 5:44 PM
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
	/// Description of ManagementMaster.
	/// </summary>
	public class ManagementMaster : ActionBase
	{
		private string FModuleName = "Portfolio";
		private string FCategory = "";
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ManagementMaster inst = new ManagementMaster();
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
				return ErrorMsg;
			
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
				this.FCategory = (string)GetCollectionValue("Data", "Category");
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
				Result = ValidationChecks.ErrorIfEmptyString(FCategory, "Category");
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
			string Method = "ManagementMaster.Output";
			string Result = "";
			string ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Category: " + FCategory, FModuleName, MasterId);
				StringBuilder Entries = new StringBuilder();
				switch (FCategory)
				{
					case "SetupPageEntries":
						Result = Management.SetupPageEntries.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "Option1Changed":
						Result = Management.Option1Changed.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "Option2Changed":
						Result = Management.Option2Changed.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "NotesChanged":
						Result = Management.NotesChanged.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "SaveChanges":
						Result = Management.SaveChanges.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "DisplayGridReport":
						Result = Management.DisplayGridReport.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ChangeActiveStatus":
						Result = Management.ChangeActiveStatus.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "UpdateNote":
						Result = Creditor.UpdateNote.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
                    case "DisplayGridLink":
                        Result = Management.DisplayGridLink.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
                        break;
                    default: break;
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
