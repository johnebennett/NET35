/*
 * Created by SharpDevelop.
 * User: John
 * Date: 10/01/2019
 * Time: 11:07 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ComboBoxEntries
{
	/// <summary>
	/// Description of CommonComboEntries.
	/// </summary>
	public class CommonComboEntries : ActionBase
	{
		
		private string FModuleName = "ComboBoxEntries";
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "DefaultMethods - Execute";
			string Result = "";
			ErrorMsg = "";

			CommonComboEntries inst = new CommonComboEntries();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try
			{
				Result = inst.ExecuteCommon(out ErrorMsg);
			}
			finally
			{
				inst.LogOnLeavingMethod(Method);
			}			

			return Result;
		}
		
		private string ExecuteCommon(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Method = "ExecuteCommon";
			LogOnInsideMethod(Method);
			
			string Result = "";
			try 
			{
				if (CommonUtilities.StringUtilities.SameText(this.CommonItem.Action, "CommonComboEntries"))
				    Result = GetCommonComboEntries(out ErrorMsg);
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
		
		private string GetCommonComboEntries(out string ErrorMsg)
		{
			string Method = "CommonComboEntries";
			ErrorMsg = "";
			string Result = "";
			int Value = CommonUtilities.StringUtilities.StrToIntDef((string)DataObject["type"], -1);
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside Actions.CommonComboEntries.GetCommonComboEntries()", FModuleName, 0);
				Result = CommonDBMethods.Common.CommonComboEntries(this.CommonItem.CompanyIdValue, this.CommonItem.ClientIdValue, Value, out ErrorMsg);
				LogResponse(Method, Result);
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
		
		public string GetCommonComboEntries(ComboBoxEntryTypes ComboType,
		                                    out string ErrorMsg)
		{
			string Method = "CommonComboEntries";
			ErrorMsg = "";
			string Result = "";
			int Value = (int)ComboType;
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside Actions.CommonComboEntries.GetCommonComboEntries()", FModuleName, 0);
				Result = CommonDBMethods.Common.CommonComboEntries(this.CommonItem.CompanyIdValue, this.CommonItem.ClientIdValue, Value, out ErrorMsg);
				LogResponse(Method, Result);
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
