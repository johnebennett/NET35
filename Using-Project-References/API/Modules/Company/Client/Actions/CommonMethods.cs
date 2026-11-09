/*
 * Created by SharpDevelop.
 * User: John
 * Date: 31/07/2020
 * Time: 11:05 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.Company.Client
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "Portfolio.Client"; }
		
		public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "IsAdd");
				TempItem.IsAdd = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ClientId");
				TempItem.ClientIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PropertyId");
				TempItem.PropertyIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ClientType");
				TempItem.ClientTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string SetupPageEntries(string ConnectionString,
		                                      int CompanyId,
		                                      int ClientId,
		                                      SetupPageEntriesRequestItem Request,
		                                      ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "SetupPageEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				int NewId = 0;
				if (string.IsNullOrEmpty(Result) && (Request.ClientId == 0))
				{
					int NewClientId = 0;
					NewId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							ConnectionString, "I_ACLIENT_ID_GEN", CompanyId, ClientId, out Result);					
					if (string.IsNullOrEmpty(Result))
					{
						Result = EAClientAdd(ConnectionString, CompanyId, NewId, Request.PropertyId, out NewClientId);
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						Request.ClientId = NewClientId;
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("ClientId", Request.ClientId, false));
				}
				
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						"SELECT OutputLine FROM EAClientAddEditGroup(@CompanyId, @ClientId, @ClientType)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("ClientId", Request.ClientId));
					Params.Add(new FbParameterItem("ClientType", Request.ClientType));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				DataTable tbl = null;
				object Obj = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["OUTPUTLINE"];
							Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
						}
					}
				}				
				
				string Str = "";
				string NoteSql = "";
				string Notes = "";
				if (string.IsNullOrEmpty(Result))
				{
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					NoteSql = JsonUtilities.GetJsonPathValueStr(Str, "$.GetNoteSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(NoteSql)) Result = "Cannot find the Notes entry";
					if (string.IsNullOrEmpty(Result))
					{
						NoteSql = NoteSql.Replace("__", "'");
						WindowsEventLoggingUtilities.WriteToInfoEvent("NoteSql:" + Environment.NewLine + NoteSql, GetModuleName(), 0);
						tbl =
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, NoteSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
						if (string.IsNullOrEmpty(Result))
						{
							foreach (DataRow Row in tbl.Rows)
							{
								Obj = Row[0];
								if (Obj != DBNull.Value)
								{
									byte[] byteArray = (byte[])Obj;
									Notes = System.Text.Encoding.Default.GetString(byteArray);
            					}
							}
							if (Entries.Length > 0) Entries.Append(",");
							Entries.Append(JsonUtilities.AddPropertyNameAndValue("NotesText", Notes, false));
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupPageEntries Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string EAClientAdd(string ConnectionString,
		                                 int CompanyId,
		                                 int NewId,
		                                 int PropertyId,
		                                 out int NewClientId)
		{
			string Result = "";
			string Method = "EAClientAdd";
			NewClientId = 0;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "EXECUTE PROCEDURE EAClientAdd(@CompanyId, @NewId, @PropertyId)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("NewId", NewId));
					Params.Add(new FbParameterItem("PropertyId", PropertyId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "SELECT FIRST 1 I_ACLIENT_CLIENTID from EAClient where I_ACLIENT_ID = @NewId";
					Params.Clear();
					Params.Add(new FbParameterItem("NewId", NewId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				DataTable tbl = null;
				object Obj = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["I_ACLIENT_CLIENTID"];
							NewClientId = ((Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj));
						}
					}
				}				
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EAClientAdd Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string LoadAllPageEntriesRequestItem(ActionBase ActionBase,
		                                                   ref AllPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAllPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = Item.Load(ActionBase);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.IsAdd = Item.IsAdd;
					TempItem.ClientIdValue = Item.ClientIdValue;
					TempItem.PropertyIdValue = Item.PropertyIdValue;
					TempItem.ClientTypeValue = Item.ClientTypeValue;
					
					Object Obj = ActionBase.GetCollectionValue("Data", "SurnameCompany");
					TempItem.SurnameCompany = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Title");
					TempItem.Title = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ACN");
					TempItem.ACN = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ABN");
					TempItem.ABN = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Contact");
					TempItem.Contact = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Name");
					TempItem.Name = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Attention");
					TempItem.Attention = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Position");
					TempItem.Position = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Department");
					TempItem.Department = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "TradingAs");
					TempItem.TradingAs = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "DearFormal");
					TempItem.DearFormal = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "DearInformal");
					TempItem.DearInformal = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ClientAddress1");
					TempItem.ClientAddress1 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ClientAddress2");
					TempItem.ClientAddress2 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ClientAddress3");
					TempItem.ClientAddress3 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Suburb");
					TempItem.Suburb = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "State");
					TempItem.State = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PostCode");
					TempItem.PostCode = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Country");
					TempItem.Country = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "Home");
					TempItem.Home = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Mobile1");
					TempItem.Mobile1 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Mobile2");
					TempItem.Mobile2 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Mobile3");
					TempItem.Mobile3 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Business1");
					TempItem.Business1 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Business2");
					TempItem.Business2 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Business3");
					TempItem.Business3 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Fax");
					TempItem.Fax = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Email1");
					TempItem.Email1 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Email2");
					TempItem.Email2 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Email3");
					TempItem.Email3 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountsEmail1");
					TempItem.AccountsEmail1 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountsEmail2");
					TempItem.AccountsEmail2 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountsEmail3");
					TempItem.AccountsEmail3 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountsEmail4");
					TempItem.AccountsEmail4 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountsEmail5");
					TempItem.AccountsEmail5 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountsEmail6");
					TempItem.AccountsEmail6 = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "EMailStmt");
					TempItem.EMailStmt = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PrintStmt");
					TempItem.PrintStmt = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "eMailNotice");
					TempItem.eMailNotice = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PrintNotice");
					TempItem.PrintNotice = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PostalAddress1");
					TempItem.PostalAddress1 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PostalAddress2");
					TempItem.PostalAddress2 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PostalAddress3");
					TempItem.PostalAddress3 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PostalAddress4");
					TempItem.PostalAddress4 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PostalAddress5");
					TempItem.PostalAddress5 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PostalAddress6");
					TempItem.PostalAddress6 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PostalAddress7");
					TempItem.PostalAddress7 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PostalAddress8");
					TempItem.PostalAddress8 = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "NoteText");
					TempItem.NoteText = (Obj == null) ? "" : Convert.ToString(Obj);
				}
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
		
		public static string CopyRequestObject(AllPageEntriesRequestItem From,
		                                       ref SetupPageEntriesRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.IsAdd = From.IsAdd;
                To.ClientIdValue = From.ClientIdValue;
                To.PropertyIdValue = From.PropertyIdValue;
                To.ClientTypeValue = From.ClientTypeValue;
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string CopyRequestObject(UpdateNoteRequestItem From,
		                                       ref SetupPageEntriesRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.IsAdd = From.IsAdd;
                To.ClientIdValue = From.ClientIdValue;
                To.PropertyIdValue = From.PropertyIdValue;
                To.ClientTypeValue = From.ClientTypeValue;
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string SaveChanges(string ConnectionString,
		                                 int CompanyId,
		                                 int ClientId,
		                                 AllPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "SaveChanges";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = CopyRequestObject(Request, ref Item);
				}
				
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref TempEntries);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateClientSQL1", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first update query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = ExecuteUpdateClientSql1(ConnectionString, Request, ClientId, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateClientSQL2", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second update query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = ExecuteUpdateClientSql2(ConnectionString, Request, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateClientSQL3", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third update query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = ExecuteUpdateClientSql3(ConnectionString, Request, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateNoteSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update note query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveNoteText(ConnectionString, QueryText, Request.NoteText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SaveChanges Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string ExecuteUpdateClientSql1(string ConnectionString,
		                                             AllPageEntriesRequestItem Request,
                                                     int ClientId,
		                                             string QueryText)
		{
			string Result = "";
			string Method = "ExecuteUpdateClientSql1";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("I_ACLIENT_CLIENTORCOMPANY", Request.ClientType));
					Params.Add(new FbParameterItem("ACLIENT_LASTNAME", Request.SurnameCompany));
					Params.Add(new FbParameterItem("ACLIENT_TITLE", Request.Title));
					Params.Add(new FbParameterItem("ACLIENT_CONTACT", Request.Contact));
					Params.Add(new FbParameterItem("ACLIENT_ACN", Request.ACN));
					Params.Add(new FbParameterItem("ACLIENT_ABN", Request.ABN));
					Params.Add(new FbParameterItem("NAME", Request.Name));
					
					Params.Add(new FbParameterItem("ACLIENT_ATTENTION", Request.Attention));
					Params.Add(new FbParameterItem("ACLIENT_POSITION", Request.Position));
					Params.Add(new FbParameterItem("ACLIENT_DEPARTMENT", Request.Department));
					Params.Add(new FbParameterItem("ACLIENT_TRADINGAS", Request.TradingAs));
					Params.Add(new FbParameterItem("ACLIENT_DEARFORMAL", Request.DearFormal));
					Params.Add(new FbParameterItem("ACLIENT_DEARINFORMAL", Request.DearInformal));
					Params.Add(new FbParameterItem("ACLIENT_ADDRESS1", Request.ClientAddress1));
					Params.Add(new FbParameterItem("ACLIENT_ADDRESS2", Request.ClientAddress2));
					Params.Add(new FbParameterItem("ACLIENT_ADDRESS3", Request.ClientAddress3));
					Params.Add(new FbParameterItem("ACLIENT_SUBURB", Request.Suburb));
					Params.Add(new FbParameterItem("ACLIENT_STATE", Request.State));
					Params.Add(new FbParameterItem("ACLIENT_POSTCODE", Request.PostCode));
					Params.Add(new FbParameterItem("ACLIENT_COUNTRY", Request.Country));
                    Params.Add(new FbParameterItem("loginid", ClientId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
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
		
		public static string ExecuteUpdateClientSql2(string ConnectionString,
		                                             AllPageEntriesRequestItem Request,
		                                             string QueryText)
		{
			string Result = "";
			string Method = "ExecuteUpdateClientSql2";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("ACLIENT_PHONEHOME", Request.Home));
					Params.Add(new FbParameterItem("ACLIENT_PHONEFAX", Request.Fax));
					Params.Add(new FbParameterItem("ACLIENT_PHONEBUSINESS1", Request.Business1));
					Params.Add(new FbParameterItem("ACLIENT_PHONEBUSINESS2", Request.Business2));
					Params.Add(new FbParameterItem("ACLIENT_PHONEBUSINESS3", Request.Business3));
					Params.Add(new FbParameterItem("ACLIENT_PHONEMOBILE1", Request.Mobile1));
					Params.Add(new FbParameterItem("ACLIENT_PHONEMOBILE2", Request.Mobile2));
					Params.Add(new FbParameterItem("ACLIENT_PHONEMOBILE3", Request.Mobile3));
					Params.Add(new FbParameterItem("ACLIENT_PHONEEMAIL1", Request.Email1));
					Params.Add(new FbParameterItem("ACLIENT_PHONEEMAIL2", Request.Email2));
					Params.Add(new FbParameterItem("ACLIENT_PHONEEMAIL3", Request.Email3));
					Params.Add(new FbParameterItem("ACLIENT_ACCOUNTSEMAIL1", Request.AccountsEmail1));
					Params.Add(new FbParameterItem("ACLIENT_ACCOUNTSEMAIL2", Request.AccountsEmail2));
					Params.Add(new FbParameterItem("ACLIENT_ACCOUNTSEMAIL3", Request.AccountsEmail3));
					Params.Add(new FbParameterItem("ACLIENT_ACCOUNTSEMAIL4", Request.AccountsEmail4));
					Params.Add(new FbParameterItem("ACLIENT_ACCOUNTSEMAIL5", Request.AccountsEmail5));
					Params.Add(new FbParameterItem("ACLIENT_ACCOUNTSEMAIL6", Request.AccountsEmail6));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
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
		
		public static string ExecuteUpdateClientSql3(string ConnectionString,
		                                             AllPageEntriesRequestItem Request,
		                                             string QueryText)
		{
			string Result = "";
			string Method = "ExecuteUpdateClientSql3";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("B_ACLIENT_EMAILSTATEMENT", Request.EMailStmt));
					Params.Add(new FbParameterItem("B_ACLIENT_PRINTSTATEMENT", Request.PrintStmt));
					Params.Add(new FbParameterItem("B_ACLIENT_EMAILNOTICE", Request.eMailNotice));
					Params.Add(new FbParameterItem("B_ACLIENT_PRINTNOTICE", Request.PrintNotice));
					Params.Add(new FbParameterItem("ACLIENT_DEBTORNOTICELINE1", Request.PostalAddress1));
					Params.Add(new FbParameterItem("ACLIENT_DEBTORNOTICELINE2", Request.PostalAddress2));
					Params.Add(new FbParameterItem("ACLIENT_DEBTORNOTICELINE3", Request.PostalAddress3));
					Params.Add(new FbParameterItem("ACLIENT_DEBTORNOTICELINE4", Request.PostalAddress4));
					Params.Add(new FbParameterItem("ACLIENT_DEBTORNOTICELINE5", Request.PostalAddress5));
					Params.Add(new FbParameterItem("ACLIENT_DEBTORNOTICELINE6", Request.PostalAddress6));
					Params.Add(new FbParameterItem("ACLIENT_DEBTORNOTICELINE7", Request.PostalAddress7));
					Params.Add(new FbParameterItem("ACLIENT_DEBTORNOTICELINE8", Request.PostalAddress8));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
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
		
		public static string SaveNoteText(string ConnectionString,
		                                  string QueryText,
		                                  string NoteText)
		{
			string Result = "";
			string Method = "SaveNoteText";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					byte[] bytes = Encoding.ASCII.GetBytes(NoteText);
					Params.Add(new FbParameterItem("notepad", bytes));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
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
		
		public static string LoadUpdateNoteRequestItem(ActionBase ActionBase,
		                                               ref UpdateNoteRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadUpdateNoteRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = Item.Load(ActionBase);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.IsAdd = Item.IsAdd;
					TempItem.ClientIdValue = Item.ClientIdValue;
					TempItem.PropertyIdValue = Item.PropertyIdValue;
					TempItem.ClientTypeValue = Item.ClientTypeValue;
					
					Object Obj = ActionBase.GetCollectionValue("Data", "NoteText");
					TempItem.NoteText = (Obj == null) ? "" : Convert.ToString(Obj);
				}
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
		
		public static string UpdateNote(string ConnectionString,
		                                int CompanyId,
		                                int ClientId,
		                                UpdateNoteRequestItem Request)
		{
			string Result = "";
			string Method = "UpdateNote";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = CopyRequestObject(Request, ref Item);
				}
				
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref TempEntries);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateNoteSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update note query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveNoteText(ConnectionString, QueryText, Request.NoteText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateNote Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string ClientCalculate(string ConnectionString,
		                                     int CompanyId,
		                                     int ClientId,
		                                     AllPageEntriesRequestItem Request,
		                                     ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "ClientCalculate";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveChanges(ConnectionString, CompanyId, ClientId, Request);
				}
				
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = CopyRequestObject(Request, ref Item);
				}
				
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref TempEntries);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.ClientCalculateSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first update query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After ClientCalculate Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string PostalCalculate(string ConnectionString,
		                                     int CompanyId,
		                                     int ClientId,
		                                     AllPageEntriesRequestItem Request,
		                                     ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "PostalCalculate";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveChanges(ConnectionString, CompanyId, ClientId, Request);
				}
				
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = CopyRequestObject(Request, ref Item);
				}
				
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref TempEntries);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.ClientPostalCalculateSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first update query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, GetModuleName(), 0);
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After PostalCalculate Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string DeleteClient(string ConnectionString,
		                                  int CompanyId,
		                                  int ClientId,
		                                  SetupPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "DeleteClient";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.DeleteSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first update query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}				
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After DeleteClient Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}

        public static string DisplayPdfReport(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
                                              SetupPageEntriesRequestItem Request,
                                              ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "DisplayPdfReport";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);
                }

                string Value = "";
                JObject JSONSource = null;
                if (string.IsNullOrEmpty(Result))
                {
                    Value =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";

                    if (string.IsNullOrEmpty(Result))
                    {
                        JSONSource = JObject.Parse(Value);
                        if (JSONSource == null) Result = "Cannot parse the Group source";
                        Value = "";
                    }
                }

                string ReportSource = "";
                if (string.IsNullOrEmpty(Result))
                {
                    ReportSource = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.B1Value", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(ReportSource)) Result = "Cannot determine the Report value";
                }

                string OutputFileName = "";
                if (string.IsNullOrEmpty(Result))
                {
                    long UniqueId = (long)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
                    Int64 NewId =
                        global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
                            ConnectionString, CompanyId, ClientId,
                            UniqueId, ReportSource, out Result, out OutputFileName);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    if (Entries.Length > 0) Entries.Append(",");
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false));
                }
                else
                {
                    Result = ActionBase.LogOnProcessingError(Method, Result, "");
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After DisplayPdfReport Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
