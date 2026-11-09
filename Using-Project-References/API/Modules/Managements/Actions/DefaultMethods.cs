/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 12/10/2018
 * Time: 12:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;


//namespace Module.Managements.Actions
namespace Actions.Managements
{
	/// <summary>
	/// Methods that do not belong to a specific Action 
	/// </summary>
	public class DefaultMethods : ActionBase
	{
		/*
		public DefaultMethods()
		{
		}
		*/
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "DefaultMethods - Execute";
			string Result = "";
			ErrorMsg = "";

			DefaultMethods inst = new DefaultMethods();
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
			
			//Result = inst.ValidateUser(MasterId, CommonItem, DataObject, out ErrorMsg);
			
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
				    Result = CommonComboEntries(out ErrorMsg);
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
		
		private string CommonComboEntries(out string ErrorMsg)
		{
			string Method = "CommonComboEntries";
			ErrorMsg = "";
			string Result = "";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside Actions.Managements.CommonComboEntries()", "Managements", 0);
				Result = CommonDBMethods.Common.CommonComboEntries(this.CommonItem.CompanyIdValue, this.CommonItem.ClientIdValue, CommonUtilities.StringUtilities.StrToIntDef((string)DataObject["type"], -1), out ErrorMsg);
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
