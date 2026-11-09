/*
 * Created by SharpDevelop.
 * User: John
 * Date: 07/02/2020
 * Time: 6:26 PM
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

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of UpdateOCCertificateValues.
	/// </summary>
	public class UpdateOCCertificateValues : ActionBase
	{
		private string FModuleName = "Portfolio";
		private  UpdateOCCertificateRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			UpdateOCCertificateValues inst = new UpdateOCCertificateValues();
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
				FRequestItem = new UpdateOCCertificateRequestItem();
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
				Result = FRequestItem.Validate();
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
			LogOnInsideMethod(Method);
			try 
			{
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
					WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, Method, MasterId);
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
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Method = "BuildOutput";
			string Result = "";
			
			LogOnInsideMethod(Method);
			try
			{			
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				if (FRequestItem.RecordId == 0)
				{
					ErrorMsg = "Invalid RecordId provided";
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EAOCCertificateAdd(
							FbConnectionString, FRequestItem.RecordId, CommonItem.CompanyIdValue, FRequestItem.PortfolioId,
							FRequestItem.Address, FRequestItem.InPresenceOf, FRequestItem.LotNo, FRequestItem.PlanNo, 
							FRequestItem.PostalAddress, FRequestItem.Applicant, FRequestItem.DeliveryAddress, 
							FRequestItem.OCDate, FRequestItem.IssuedDate, FRequestItem.EntityName);
				}
				
				
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EAOCCertificateGroup(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RecordId, FRequestItem.CategoryOnlyId, 
							CommonItem.ClientIdValue, ref Entries);
				}
				
				string UpdateSql = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					UpdateSql = GetUpdateBlobSql(Entries, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					UpdateSql = UpdateSql.Replace("_XXX_", "@NoteContent");
					UpdateSql = UpdateSql.Replace("\"", "'");
					byte[] bytes = Encoding.ASCII.GetBytes(FRequestItem.NoteContent);
					List<FbParameterItem> Params = new List<FbParameterItem>();
					Params.Add(new FbParameterItem("NoteContent", bytes));
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, UpdateSql, Params, 0);
				}

				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						//Entries.ToString() +
						JsonUtilities.AddPropertyNameAndValue("Result", "Updated Successfully", false) +
						JsonUtilities.EndJson();
				}
				else
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
			
			return Result;
		}
		
		private string GetUpdateBlobSql(StringBuilder Entries, out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetUpdateBlobSql";
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string JsonText =
					JsonUtilities.StartJson() +
					Entries.ToString() +
					JsonUtilities.EndJson();
				JObject jobj = JsonConvert.DeserializeObject<JObject>(JsonText);
				if (jobj == null)
				{
					ErrorMsg = "Error deserializing the Request";
				}
				else
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Before getting sqltext", FModuleName, MasterId);
					Result = (string)jobj["update_blob"];
					WindowsEventLoggingUtilities.WriteToInfoEvent("After getting sqltext", FModuleName, MasterId);
					if (string.IsNullOrEmpty(Result))
					{
						ErrorMsg = "Missing sqltext in Entries";
					}					
					WindowsEventLoggingUtilities.WriteToInfoEvent("sqltext: " + Result, FModuleName, MasterId);
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