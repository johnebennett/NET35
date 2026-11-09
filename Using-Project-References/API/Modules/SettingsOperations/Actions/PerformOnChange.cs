/*
 * Created by SharpDevelop.
 * User: John
 * Date: 03/04/2020
 * Time: 6:59 PM
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

namespace Actions.SettingsOperations
{
	/// <summary>
	/// Description of PerformOnChange.
	/// </summary>
	public class PerformOnChange : ActionBase
	{
		private string FModuleName = "SettingsOperations";
		
		private string FSettingType = "";
		private string FChangeAction = "";
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			PerformOnChange inst = new PerformOnChange();
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
				object Obj = this.GetCollectionValue("Data", "SettingType");
				FSettingType = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = this.GetCollectionValue("Data", "ChangeAction");
				FChangeAction = (Obj == null) ? "" : Convert.ToString(Obj);
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
			string Key = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				string BaseFormat = "{0} is {1}";
				string NotProvided = "Not Provided";
				//string Invalid = "Invalid";				
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "SettingType";
					if (string.IsNullOrEmpty(FSettingType)) Result = string.Format(BaseFormat, Key, NotProvided);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "ChangeAction";
					if (string.IsNullOrEmpty(FChangeAction)) Result = string.Format(BaseFormat, Key, NotProvided);
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
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				StringBuilder Entries = new StringBuilder();
				switch (FSettingType)
				{
					case "Commercial": 
						ErrorMsg = CommercialSettings.OnChange((ActionBase)this, this.DataCollection, ref Entries);
						break;
					case "ChartOfAccounts": 
						ErrorMsg = ChartOfAccountSettings.OnChange((ActionBase)this, this.DataCollection, ref Entries);
						break;
					case "SystemSettings": 
						ErrorMsg = SystemSettings.OnChange((ActionBase)this, this.DataCollection, ref Entries);
						break;
					default: ErrorMsg = "Invalid or Missing Setting Type value"; break;
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
				/*
				entries.AppendLine(JsonUtilities.StartJson());
					
				ErrorMsg = BuildOutputEntries(ref entries);				
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				entries.AppendLine(JsonUtilities.EndJson());
				Result = entries.ToString();
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
	}
}
