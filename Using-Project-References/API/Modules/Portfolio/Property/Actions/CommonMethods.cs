/*
 * Created by SharpDevelop.
 * User: John
 * Date: 28/07/2020
 * Time: 3:29 PM
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
namespace Actions.Portfolio.Property
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "Portfolio.Property"; }
		
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
				Obj = ActionBase.GetCollectionValue("Data", "MgmtId");
				TempItem.MgmtIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NotesId");
				TempItem.NotesIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
				if (string.IsNullOrEmpty(Result) && (Request.MgmtId == 0))
				{
					int MgmtId = 0;
					NewId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							ConnectionString, "i_propertyid_gen", CompanyId, ClientId, out Result);					
					if (string.IsNullOrEmpty(Result))
					{
						Result = EAPropertyAdd(ConnectionString, CompanyId, NewId, out MgmtId);
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						Request.MgmtId = MgmtId;
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("MgmtId", Request.MgmtId, false));
				}
				
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						"SELECT OutputLine FROM EAPropertyAddEditGroup(@CompanyId, @PropertyId, @NotesId)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("PropertyId", Request.MgmtId));
					Params.Add(new FbParameterItem("NotesId", Request.NotesId));
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
					WindowsEventLoggingUtilities.WriteToInfoEvent("Start - OwnerInstructions", GetModuleName(), 0);
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					NoteSql = JsonUtilities.GetJsonPathValueStr(Str, "$.SQLtoGetNote", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(NoteSql)) Result = "Cannot find the Notes entry";
					if (string.IsNullOrEmpty(Result))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("OwnerNoteSql:" + Environment.NewLine + NoteSql, GetModuleName(), 0);
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
		
		public static string EAPropertyAdd(string ConnectionString,
		                                   int CompanyId,
		                                   int NewId,
		                                   out int MgmtId)
		{
			string Result = "";
			string Method = "EAPropertyAdd";
			MgmtId = 0;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "EXECUTE PROCEDURE EAPropertyAdd(@CompanyId, @NewId)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("NewId", NewId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "SELECT FIRST 1 I_PROPERTY_PROPERTYID from eaproperty where I_PROPERTY_ID= @NewId";
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
							Obj = Row["I_PROPERTY_PROPERTYID"];
							MgmtId = ((Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EAPropertyAdd Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string LoadAllPageEntriesRequestItem(ActionBase ActionBase,
		                                                   ref AllPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
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
					TempItem.MgmtIdValue = Item.MgmtIdValue;
					TempItem.IsAdd = Item.IsAdd;
					TempItem.NotesIdValue = Item.NotesIdValue;
					
					Object Obj = ActionBase.GetCollectionValue("Data", "BldgName");
					TempItem.BldgName = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "SmokeAlarm");
					TempItem.SmokeAlarm = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "SmokeAlarmDate");
					TempItem.SmokeAlarmDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Bedrooms");
					TempItem.BedroomsValue = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "LevelNo");
					TempItem.LevelNo = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "LevelExtra");
					TempItem.LevelExtra = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Heater");
					TempItem.Heater = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "HeaterDate");
					TempItem.HeaterDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Bathrooms");
					TempItem.BathroomsValue = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "UnitNo");
					TempItem.UnitNo = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "UnitExtra");
					TempItem.UnitExtra = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AirCon");
					TempItem.AirCon = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AirconServiceDate");
					TempItem.AirconServiceDate = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "StreetNo");
					TempItem.StreetNo = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StreetNoExtra");
					TempItem.StreetNoExtra = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PoolCert");
					TempItem.PoolCert = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PoolCertDate");
					TempItem.PoolCertDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "CarSpaces");
					TempItem.CarSpacesValue = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "StreetPOBox");
					TempItem.StreetPOBox = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "FireService");
					TempItem.FireService = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "FireServiceDate");
					TempItem.FireServiceDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Garage");
					TempItem.GarageValue = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "Suburb");
					TempItem.Suburb = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Gas");
					TempItem.Gas = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "GasDate");
					TempItem.GasDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "CarPort");
					TempItem.CarPortValue = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "State");
					TempItem.State = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Electrical");
					TempItem.Electrical = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ElectricalDate");
					TempItem.ElectricalDate = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "PostCode");
					TempItem.PostCode = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Flue");
					TempItem.Flue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "FlueDate");
					TempItem.FlueDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "BldgArea");
					TempItem.BldgAreaValue = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "Grease");
					TempItem.Grease = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "GreaseDate");
					TempItem.GreaseDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "LandArea");
					TempItem.LandAreaValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AuthorityDate");
					TempItem.AuthorityDate = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "HotPlate");
					TempItem.HotPlate = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "PropertyType");
					TempItem.PropertyTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "NoteText");
					TempItem.NoteText = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "OwnerToAttendInspection");
                    TempItem.OwnerToAttendInspection = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "Council");
                    TempItem.Council = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "SearchKey");
                    TempItem.SearchKey = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "SmokeComment");
                    TempItem.SmokeComment = (Obj == null) ? "" : Convert.ToString(Obj);
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
                To.MgmtIdValue = From.MgmtIdValue;
                To.NotesIdValue = From.NotesIdValue;
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
                To.MgmtIdValue = From.MgmtIdValue;
                To.NotesIdValue = From.NotesIdValue;
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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL1", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first post query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = ExecutePostSql1(ConnectionString, Request, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL2", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second post query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = ExecutePostSql2(ConnectionString, Request, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL3", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third post query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = ExecutePostSql3(ConnectionString, Request, QueryText);
				}

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL4", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Fourth post query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = ExecutePostSql4(ConnectionString, Request, QueryText);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL5", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the fifth post query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = ExecutePostSql5(ConnectionString, Request, QueryText);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL6", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the fifth post query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = ExecutePostSql6(ConnectionString, Request, QueryText);
                }

                if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SQLtoUpdateNote", out Result);
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
		
		public static string ExecutePostSql1(string ConnectionString,
		                                     AllPageEntriesRequestItem Request,
		                                     string QueryText)
		{
			string Result = "";
			string Method = "ExecutePostSql1";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("PROPERTY_BUILDINGNAME", Request.BldgName));
					Params.Add(new FbParameterItem("PROPERTY_FLOOR", Request.LevelNo));
					Params.Add(new FbParameterItem("PROPERTY_FLOORDETAIL", Request.LevelExtra));
					Params.Add(new FbParameterItem("PROPERTY_UNIT", Request.UnitNo));
					Params.Add(new FbParameterItem("PROPERTY_UNITDETAIL", Request.UnitExtra));
					Params.Add(new FbParameterItem("PROPERTY_NUMBER", Request.StreetNo));
					Params.Add(new FbParameterItem("PROPERTY_NUMBERDETAIL", Request.StreetNoExtra));
					Params.Add(new FbParameterItem("PROPERTY_STREET", Request.StreetPOBox));
					Params.Add(new FbParameterItem("PROPERTY_SUBURB", Request.Suburb));
					Params.Add(new FbParameterItem("PROPERTY_STATE", Request.State));
					Params.Add(new FbParameterItem("PROPERTY_POSTCODE", Request.PostCode));
					Params.Add(new FbParameterItem("I_PROPERTY_NOOFBEDROOM", Request.Bedrooms));
					Params.Add(new FbParameterItem("I_PROPERTY_NOOFBATHROOM", Request.Bathrooms));
					Params.Add(new FbParameterItem("I_PROPERTY_NOOFCARSPACES", Request.CarSpaces));
					Params.Add(new FbParameterItem("I_PROPERTY_GARAGE", Request.Garage));
					Params.Add(new FbParameterItem("I_PROPERTY_CARPORT", Request.CarPort));
					Params.Add(new FbParameterItem("M_PROPERTY_BUILDAREAM2", Request.BldgArea));
					Params.Add(new FbParameterItem("M_PROPERTY_LANDAREAM2", Request.LandArea));
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
		
		public static string ExecutePostSql2(string ConnectionString,
		                                     AllPageEntriesRequestItem Request,
		                                     string QueryText)
		{
			string Result = "";
			string Method = "ExecutePostSql2";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("B_PROPERTY_SMOKEALARM", Request.SmokeAlarm));
					Params.Add(new FbParameterItem("D_PROPERTY_SMOKEDATE", Request.SmokeAlarmDate));
					Params.Add(new FbParameterItem("B_PROPERTY_HEATER", Request.Heater));
					Params.Add(new FbParameterItem("D_PROPERTY_HEATERDATE", Request.HeaterDate));
					Params.Add(new FbParameterItem("B_PROPERTY_AIRCONDITIONSERVICE", Request.AirCon));
					Params.Add(new FbParameterItem("D_PROPERTY_AIRCONDITIONINGDATE", Request.AirconServiceDate));
					Params.Add(new FbParameterItem("B_PROPERTY_POOLCERTIFICATE", Request.PoolCert));
					Params.Add(new FbParameterItem("D_PROPERTY_POOLCERTIFICATEDATE", Request.PoolCertDate));
					Params.Add(new FbParameterItem("B_PROPERTY_FIRESERVICE", Request.FireService));
					Params.Add(new FbParameterItem("D_PROPERTY_FIRESERVICEDATE", Request.FireServiceDate));
                    Params.Add(new FbParameterItem("B_PROPERTY_OWNERTOATTENDINSP", Request.OwnerToAttendInspection));
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
		
		public static string ExecutePostSql3(string ConnectionString,
		                                     AllPageEntriesRequestItem Request,
		                                     string QueryText)
		{
			string Result = "";
			string Method = "ExecutePostSql3";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("I_PROPERTY_PROPERTYTYPE", Request.PropertyType));
					Params.Add(new FbParameterItem("B_PROPERTY_GAS", Request.Gas));
					Params.Add(new FbParameterItem("D_PROPERTY_GAS", Request.GasDate));
					Params.Add(new FbParameterItem("B_PROPERTY_ELECTRICAL", Request.Electrical));
					Params.Add(new FbParameterItem("D_PROPERTY_ELECTRICAL", Request.ElectricalDate));
					Params.Add(new FbParameterItem("B_PROPERTY_FLUE", Request.Flue));
					Params.Add(new FbParameterItem("D_PROPERTY_FLUEDATE", Request.FlueDate));
					Params.Add(new FbParameterItem("B_PROPERTY_GREASE", Request.Grease));
					Params.Add(new FbParameterItem("D_PROPERTY_GREASEDATE", Request.GreaseDate));
					Params.Add(new FbParameterItem("B_PROPERTY_HOTPLATE", Request.HotPlate));
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

        public static string ExecutePostSql4(string ConnectionString,
                                             AllPageEntriesRequestItem Request,
                                             string QueryText)
        {
            string Result = "";
            string Method = "ExecutePostSql4";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("PROPERTY_MUNICIPALITY", Request.Council));
                    Params.Add(new FbParameterItem("PROPERTY_SEARCHKEY", Request.SearchKey));
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

        public static string ExecutePostSql5(string ConnectionString,
                                             AllPageEntriesRequestItem Request,
                                             string QueryText)
        {
            string Result = "";
            string Method = "ExecutePostSql5";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("PROPERTY_SMOKECOMMENT", Request.SmokeComment));
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

        public static string ExecutePostSql6(string ConnectionString,
                                             AllPageEntriesRequestItem Request,
                                             string QueryText)
        {
            string Result = "";
            string Method = "ExecutePostSql6";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                	Params.Add(new FbParameterItem("I_PROPERTY_PROPERTYTYPE", Request.PropertyType));
                    Params.Add(new FbParameterItem("D_PROPERTY_MANAGEMENTSTART", Request.AuthorityDate));
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
					Params.Add(new FbParameterItem("notes", bytes));
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
					TempItem.MgmtIdValue = Item.MgmtIdValue;
					TempItem.NotesIdValue = Item.NotesIdValue;
					
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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SQLtoUpdateNote", out Result);
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
		
		public static string GetNoteText(string ConnectionString,
		                                 int CompanyId,
		                                 int ClientId,
		                                 SetupPageEntriesRequestItem Request,
		                                 ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "GetNoteText";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("MgmtId", Request.MgmtId, false));
				}
				
				string Str = "";
				string Notes = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Str =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					Notes = JsonUtilities.GetJsonPathValueStr(Str, "$.NotesText", out Result);
				}
			
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("NoteText:" + Environment.NewLine + Notes, GetModuleName(), 0);
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("NotesText", Notes, false));
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After GetNoteText Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
		
		public static string DeleteProperty(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    SetupPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "DeleteProperty";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "DELETE FROM EAProperty WHERE I_PROPERTY_PROPERTYID = @PropertyId AND I_PROPERTY_COMPANYID = @CompanyId";
					Params.Add(new FbParameterItem("PropertyId", Request.MgmtId));
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After DeleteProperty Result: " + Result, GetModuleName(), 0);
			}
			
			return Result;
		}
	}
}
