/*
 * Created by SharpDevelop.
 * User: John
 * Date: 04/04/2020
 * Time: 6:43 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using Actions.SettingsOperations;

namespace Actions.SettingsOperations
{
	/// <summary>
	/// Description of SaveCommercialSettings.
	/// </summary>
	public class SaveCommercialSettings : ActionBase
	{
		private string FModuleName = "SettingsOperations";
		private SaveCommercialSettingsRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SaveCommercialSettings inst = new SaveCommercialSettings();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
				DataCollection = CommonMethods.MakeCollectionFromJObject(DataObject, out Result);
				FRequestItem = new SaveCommercialSettingsRequestItem();
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
				if (string.IsNullOrEmpty(Result))
				{
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				//Result = CommonMethods.SaveResidentialBonds(FbConnectionString, CommonItem.CompanyIdValue, FRequestItem);				
				NameValueCollection NewCollection = CommonMethods.MakeCollectionFromJObject(DataObject, out Result);
				foreach (string key in NewCollection)
				{
				    // Overwrite any entry already there
				    WindowsEventLoggingUtilities.WriteToInfoEvent(key + ": " + NewCollection[key], FModuleName, MasterId);
				    this.DataCollection[key] = NewCollection[key];
				    WindowsEventLoggingUtilities.WriteToInfoEvent(key + ": " + this.DataCollection[key], FModuleName, MasterId);				    
				}
				//DataCollection = CommonMethods.MakeCollectionFromJObject(DataObject, out Result);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = CommercialSettings.SaveSettings((ActionBase)this, FRequestItem);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = LogResponse(Method, "");
				}
				else 
				{
					Result = LogOnProcessingError(Method, Result, "");
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
	}
}
