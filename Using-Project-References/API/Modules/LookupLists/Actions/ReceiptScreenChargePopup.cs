/*
 * Created by SharpDevelop.
 * User: John
 * Date: 07/05/2020
 * Time: 5:29 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.LookupLists
{
	/// <summary>
	/// Description of ReceiptScreenChargePopup.
	/// </summary>
	public class ReceiptScreenChargePopup : ActionBase
	{
		private string FModuleName = "LookupLists";		
		private ReceiptScreenChargePopupRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "ReceiptScreenChargePopup - Execute";
			string Result = "";
			ErrorMsg = "";

			ReceiptScreenChargePopup inst = new ReceiptScreenChargePopup();
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try 
			{
				Result = inst.Init();
				if (!string.IsNullOrEmpty(Result))
					return "";
				
				Result = inst.Validate();
				if (!string.IsNullOrEmpty(Result))
					return Result;
				
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
			
			LogOnInsideMethod(Method);
			try 
			{
				FRequestItem = new ReceiptScreenChargePopupRequestItem();
				Result = FRequestItem.Load((ActionBase)this);
			}
			catch (Exception ex) 
			{
				Result = LogOnException(Method, ex, "");
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
			
			LogOnInsideMethod(Method);
			try 
			{
				Result = FRequestItem.Validate();
			}
			catch (Exception ex)
			{
				Result = LogOnException(Method, ex, "");
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EAReceiptScreenChargePopup(
							FbConnectionString, FRequestItem.ID, FRequestItem.PayingAmount, ref Entries);
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
			catch (Exception ex)
			{
				Result = LogOnException(Method, ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
	}
}
