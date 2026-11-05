using System;
using CommonUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EAOfficeAPI
{
	public class RequestCommonItem
	{
		private string FCompanyId = "";
		private int FComanyIdValue = 0;
		
		private string FClientId = "";
		private int FClientIdValue = 0;
		
		private string FDebug = "";
		private bool FDebugValue = false;
		
		public string Plugin { get; set; }
		public string Version { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		//public string Controller { get; set; }
		public string Action { get; set; }
		//public string Method { get; set; }
		public string Command { get; set; }
		public string CompanyId 
		{ 
			get { return FCompanyId; }
			set
			{
				FCompanyId = value;
				SetCompanyIdValue(value);
			}
		}
		public int CompanyIdValue { get { return FComanyIdValue; } }
		
		public string ClientId
		{
			get { return FClientId; }
			set
			{
				FClientId = value;
				SetClientIdValue(value);
			}
		}
		public int ClientIdValue { get { return FClientIdValue; } }
		
		public string Debug
		{
			get { return FDebug; }
			set
			{
				FDebug = value;
				SetDebugValue(value);
			}
		}
		
		public bool DebugValue { get { return FDebugValue; } }

		private void SetCompanyIdValue(string Value)
		{
			FComanyIdValue = CommonUtilities.StringUtilities.StrToIntDef(Value, -1);
		}
		
		private void SetClientIdValue(string Value)
		{
			FClientIdValue = CommonUtilities.StringUtilities.StrToIntDef(Value, -1);
		}
		
		private void SetDebugValue(string Value)
		{
			FDebugValue =
				((CommonUtilities.StringUtilities.SameText(Value, "Y")) ||
			     (CommonUtilities.StringUtilities.SameText(Value, "YES")) ||
			     (CommonUtilities.StringUtilities.SameText(Value, "TRUE")) ||
			     (CommonUtilities.StringUtilities.SameText(Value, "1")) ||
			     (CommonUtilities.StringUtilities.StrToIntDef(Value, -1) > 0));
		}
		
		private string FMethod = "";
		
		public string Method 
		{ 
			get 
			{ 
				return FMethod; 
			}
			set 
			{ 
				FMethod = value; 
				this.Action = value; 
			}
		}
	}
	
	public class RequestCommonProcessor
	{
		public static RequestCommonItem LoadFromRequest(string Request,
		                                                out string ErrorMsg)
		{
			ErrorMsg = "";
			RequestCommonItem Result = null;
			JObject jobj = JsonConvert.DeserializeObject<JObject>(Request);
			if (jobj == null)
			{
				ErrorMsg = "Error deserializing the Request";
			}
			else
			{
				JObject obj = (jobj["common"] as JObject);
				if (obj == null)
				{
					ErrorMsg = "Missing common element";
				}
				else
				{
					string Str = "";
					try 
					{
						Str = JsonConvert.SerializeObject(obj, Formatting.Indented);
						try 
						{
							Result = JsonConvert.DeserializeObject<RequestCommonItem>(Str);
						} 
						catch (Exception ExDeserialize) 
						{
							ErrorMsg =
								ExDeserialize.Message + Environment.NewLine +
								ExDeserialize.StackTrace;
						}
					} 
					catch (Exception ExSerialize) 
					{
						ErrorMsg =
							ExSerialize.Message + Environment.NewLine +
							ExSerialize.StackTrace;
					}
				}
			}
			
			return Result;
		}
		
		/// <summary>
		/// What members of the Item object are mandatory
		/// </summary>
		/// <param name="Item"></param>
		/// <returns>
		/// A string containing all the members that are
		/// mandatory but not correctly formatted or provided
		/// </returns>
		public static string ValidateCommonItem(RequestCommonItem Item)
		{
			/*
			 * What should the Item contain that is mandatory?
			*/
			
			string Result = "";
			/*
				public string Plugin { get; set; }
				public string Version { get; set; }
				public string UserName { get; set; }
				public string Password { get; set; }
				public string Controller { get; set; }
				public string Action { get; set; }
				public string Method { get; set; }
				public string Command { get; set; }
				public string CompanyId 
				public int CompanyIdValue { get { return FComanyIdValue; } }		
				public string ClientId
				public int ClientIdValue { get { return FClientIdValue; } }		
				public string Debug
			*/
			
			if (string.IsNullOrEmpty(Item.Plugin))
				Result += "Invalid or empty Plugin value provided." + Environment.NewLine;
			if (string.IsNullOrEmpty(Item.UserName))
				Result += "Invalid or empty UserName value provided." + Environment.NewLine;
			if (string.IsNullOrEmpty(Item.Password))
				Result += "Invalid or empty Password value provided." + Environment.NewLine;
			/*
			if (string.IsNullOrEmpty(Item.Controller))
				Result += "Invalid or empty Controller value provided." + Environment.NewLine;
			*/
			if (string.IsNullOrEmpty(Item.Action))
				Result += "Invalid or empty Action value provided." + Environment.NewLine;
			if (string.IsNullOrEmpty(Item.CompanyId) ||
			    (Item.CompanyIdValue < 0))
				Result += "Invalid or empty CompanyId value provided." + Environment.NewLine;
			if (string.IsNullOrEmpty(Item.ClientId) ||
			    (Item.ClientIdValue < 0))
				Result += "Invalid or empty ClientId value provided." + Environment.NewLine;			
			
			return Result;
		}
	}
}