/*
 * Created by SharpDevelop.
 * User: John
 * Date: 30/01/2020
 * Time: 3:45 PM
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
	/// Description of OwnersCertificateDialogContent.
	/// </summary>
	public class OwnersCertificateDialogContent : ActionBase
	{
		private string FModuleName = "Portfolio";
		private  OwnersCertificateDialogContentRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			OwnersCertificateDialogContent inst = new OwnersCertificateDialogContent();
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
				FRequestItem = new OwnersCertificateDialogContentRequestItem();
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
				
				StringBuilder Entries = new StringBuilder();
				int NewId = 0;
				
				if (FRequestItem.RecordId == 0)
				{
					NewId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							FbConnectionString, "I_OC_ID_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if ((FRequestItem.RecordId == 0) && (NewId > 0))
					{
						string TodayStr = DateTime.Now.ToString("dd/MM/yyyy");
						ErrorMsg =
							CommonMethods.EAOCCertificateAdd(
								FbConnectionString, NewId, CommonItem.CompanyIdValue, FRequestItem.PortfolioId,
								"", "", "", "", "", "", "", TodayStr, TodayStr, "");
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if (FRequestItem.RecordId == 0) FRequestItem.RecordId = NewId;
				}
				
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("RecordId", FRequestItem.RecordId));
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("PortfolioId", FRequestItem.PortfolioId));
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("CategoryOnlyId", FRequestItem.CategoryOnlyId));
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EAOCCertificateGroup(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RecordId, FRequestItem.CategoryOnlyId, 
							CommonItem.ClientIdValue, ref Entries);
				}
				
				string BlobSql = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					BlobSql = GetSqlToGetBlob(Entries, out ErrorMsg);
				}
				
				string BlobText = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					BlobText = GetBlobTextFromQuery(BlobSql, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(BlobText))
				{
					Entries.Append("," + BlobText);
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
		
		private string GetSqlToGetBlob(StringBuilder Entries, out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetSqlToGetBlob";
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
					WindowsEventLoggingUtilities.WriteToInfoEvent("Before getting sqltogetblob", FModuleName, MasterId);
					Result = (string)jobj["sqltogetblob"];
					WindowsEventLoggingUtilities.WriteToInfoEvent("After getting sqltogetblob", FModuleName, MasterId);
					if (string.IsNullOrEmpty(Result))
					{
						ErrorMsg = "Missing SqlToGetBlob in Entries";
					}					
					WindowsEventLoggingUtilities.WriteToInfoEvent("sqltogetblob: " + Result, FModuleName, MasterId);
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
		
		private string GetBlobTextFromQuery(string BlobSql, out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetBlobTextFromQuery";
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				DataTable tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, BlobSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if (tbl.Rows.Count > 0)
					{
						object obj = tbl.Rows[0][0];
						if (obj != DBNull.Value)
						{
							byte[] bytes = (byte[])obj;
							Result = Encoding.Default.GetString(bytes);
						}
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = JsonUtilities.AddPropertyNameAndValue("NoteContent", Result, false);
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