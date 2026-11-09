/*
 * Created by SharpDevelop.
 * User: John
 * Date: 28/12/2019
 * Time: 10:45 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.LookupLists
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}

		public static string GetPrefix()
		{
			return "LookupLists.";
		}
		
		public static string LoadTransactionLookupRequestItem(ActionBase ActionBase,
		                                                      ref TransactionLookupRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadTransactionLookupRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "TBA");
				TempItem.TBAValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "CreditorAccId");
				TempItem.CreditorAccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "SearchText");
				TempItem.SearchText = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "SearchType");
				TempItem.SearchTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "SearchStreet");
				TempItem.SearchStreetValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "SearchClient");
				TempItem.SearchClientValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "SearchOwner");
				TempItem.SearchOwnerValue = (Obj == null) ? "" : Convert.ToString(Obj);
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
	}
}
