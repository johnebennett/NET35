/*
 * Created by SharpDevelop.
 * User: John
 * Date: 02/07/2019
 * Time: 6:48 PM
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
using CommonDBMethods;

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of PaymentSearchGridRequestItem.
    /// </summary>
	public class PaymentSearchGridRequestItem
	{
		#region Properties - Getters and Setters

		private string FRunIdValue = "";
		public int RunId { get; set; }			
		public string RunIdValue
		{
			get { return FRunIdValue; }
			set
			{
				FRunIdValue = value;
				RunId = StringUtilities.StrToIntDef(FRunIdValue, -1);				
			}
		}
		#endregion
		
		#region Load from DataObject
		public string Load(JObject DataObject)
		{
			string Result = "";
			try 
			{
				RunIdValue = (string)DataObject["id"];
			}
			catch (Exception Ex)
			{
				Result =
					Ex.InnerException + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			
			return Result;
		}
		#endregion
		
		#region Clear
		private void Clear()
		{
			RunIdValue = "0";
		}
		#endregion
		
		#region Validate
		public string Validate()
		{
			string Result = "";
			try 
			{
				string BaseFormat = "{0} is {1}";
				string NotProvided = "Not Provided";
				string Invalid = "Invalid";				
				string Key = "";
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "RunId";
					if (string.IsNullOrEmpty(RunIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (RunId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
			}
			catch (Exception Ex)
			{
				Result =
					Ex.InnerException + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			
			return Result;
		}
		#endregion
	}
}
