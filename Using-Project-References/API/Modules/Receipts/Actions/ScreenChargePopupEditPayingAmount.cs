/*
 * Created by SharpDevelop.
 * User: John
 * Date: 07/05/2020
 * Time: 6:42 PM
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

namespace Actions.Receipts
{
	/// <summary>
	/// Description of ScreenChargePopupEditPayingAmount.
	/// </summary>
	public class ScreenChargePopupEditPayingAmount : ActionBase
	{
		private string FModuleName = "Receipts";		
		private ReceiptScreenChargePopupRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "ReceiptScreenChargePopup - Execute";
			string Result = "";
			ErrorMsg = "";

			ScreenChargePopupEditPayingAmount inst = new ScreenChargePopupEditPayingAmount();
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
				
				string ApplySQL = "";
				string StrMinValue = "";
				string StrMaxValue = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ApplySQL = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.ApplySQL", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(ApplySQL)) ErrorMsg = "Cannot find the Apply entry";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						StrMinValue = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.MinimumPayingValue", out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(StrMinValue)) ErrorMsg = "Cannot find the MinValue entry";
					}
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						StrMaxValue = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.MaximumPayingValue", out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(StrMaxValue)) ErrorMsg = "Cannot find the ManValue entry";
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					double MinValue = Convert.ToDouble(StrMinValue);
					double MaxValue = Convert.ToDouble(StrMaxValue);
					if ((FRequestItem.PayingAmount < MinValue) || (FRequestItem.PayingAmount > MaxValue))
					{
						ErrorMsg = "The value entered is not within the acceptable payable range";
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("ApplySQL:" + Environment.NewLine + ApplySQL, Method, 0);
					List<FbParameterItem> Params = new List<FbParameterItem>();
					Params.Add(new FbParameterItem("PayingAmount", FRequestItem.PayingAmount));
					ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(ApplySQL, Params, FModuleName, MasterId);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, ApplySQL, Params, 0);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogResponse(Method, "");
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
