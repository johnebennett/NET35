/*
 * Created by SharpDevelop.
 * User: John
 * Date: 13/07/2020
 * Time: 8:35 PM
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
namespace Actions.Portfolio.Creditor
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "Portfolio.Creditor"; }
		
		public static string LoadAllPageEntriesRequestItem(ActionBase ActionBase,
		                                                   ref AllPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAllPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string DefaultDateStr = "01/01/1900";
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NotesId");
				TempItem.NotesIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Option1Id");
				TempItem.Option1IdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Option2Id");
				TempItem.Option2IdValue = (Obj == null) ? "" : Convert.ToString(Obj); 

				Obj = ActionBase.GetCollectionValue("Data", "TypeId");
				TempItem.TypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Header4Value");
				TempItem.Header4Value = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "BranchId");
				TempItem.BranchIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Property");
				TempItem.Property = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Header7Value");
				TempItem.Header7Value = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ManagerId");
				TempItem.ManagerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Header3Value");
				TempItem.Header3Value = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Header8Value");
				TempItem.Header8Value = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "NoteText");
				TempItem.NoteText= (Obj == null) ? "" : Convert.ToString(Obj); 

				/*
				Obj = ActionBase.GetCollectionValue("Data", "Date1");
				TempItem.Date1 = (Obj == null) ? "" : Convert.ToString(Obj);
				if (string.IsNullOrEmpty(TempItem.Date1)) TempItem.Date1 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date2");
				TempItem.Date2 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date2)) TempItem.Date2 = DefaultDateStr;				
				Obj = ActionBase.GetCollectionValue("Data", "Date3");
				TempItem.Date3 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date3)) TempItem.Date3 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date4");
				TempItem.Date4 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date4)) TempItem.Date4 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date5");
				TempItem.Date5 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date5)) TempItem.Date5 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date6");
				TempItem.Date6 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date6)) TempItem.Date6 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date7");
				TempItem.Date7 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date7)) TempItem.Date7 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date8");
				TempItem.Date8 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date8)) TempItem.Date8 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date9");
				TempItem.Date9 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date9)) TempItem.Date9 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date10");
				TempItem.Date10 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date10)) TempItem.Date10 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date11");
				TempItem.Date11 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date11)) TempItem.Date11 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date12");
				TempItem.Date12 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date12)) TempItem.Date12 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date13");
				TempItem.Date13 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date13)) TempItem.Date13 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date14");
				TempItem.Date14 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date14)) TempItem.Date14 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date15");
				TempItem.Date15 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date15)) TempItem.Date15 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date16");
				TempItem.Date16 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date16)) TempItem.Date16 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date17");
				TempItem.Date17 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date17)) TempItem.Date17 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date18");
				TempItem.Date18 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date18)) TempItem.Date18 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date19");
				TempItem.Date19 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date19)) TempItem.Date19 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date20");
				TempItem.Date20 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date20)) TempItem.Date20 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date21");
				TempItem.Date21 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date21)) TempItem.Date21 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date22");
				TempItem.Date22 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date22)) TempItem.Date22 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date23");
				TempItem.Date23 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date23)) TempItem.Date23 = DefaultDateStr;
				Obj = ActionBase.GetCollectionValue("Data", "Date24");
				TempItem.Date24 = (Obj == null) ? "" : Convert.ToString(Obj); 
				if (string.IsNullOrEmpty(TempItem.Date24)) TempItem.Date24 = DefaultDateStr;
				*/
				Obj = ActionBase.GetCollectionValue("Data", "Date1");
				TempItem.Date1 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Date2");
				TempItem.Date2 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date3");
				TempItem.Date3 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date4");
				TempItem.Date4 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date5");
				TempItem.Date5 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date6");
				TempItem.Date6 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date7");
				TempItem.Date7 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date8");
				TempItem.Date8 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date9");
				TempItem.Date9 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date10");
				TempItem.Date10 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date11");
				TempItem.Date11 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date12");
				TempItem.Date12 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date13");
				TempItem.Date13 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date14");
				TempItem.Date14 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date15");
				TempItem.Date15 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date16");
				TempItem.Date16 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date17");
				TempItem.Date17 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date18");
				TempItem.Date18 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date19");
				TempItem.Date19 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date20");
				TempItem.Date20 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date21");
				TempItem.Date21 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date22");
				TempItem.Date22 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date23");
				TempItem.Date23 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Date24");
				TempItem.Date24 = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj); 

				Obj = ActionBase.GetCollectionValue("Data", "Text1");
				TempItem.Text1 = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Text2");
				TempItem.Text2 = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Text3");
				TempItem.Text3 = (Obj == null) ? "" : Convert.ToString(Obj); 

				Obj = ActionBase.GetCollectionValue("Data", "Chk1");
				TempItem.Chk1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Chk2");
				TempItem.Chk2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Chk3");
				TempItem.Chk3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Chk4");
				TempItem.Chk4 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Chk5");
				TempItem.Chk5 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Chk6");
				TempItem.Chk6 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Chk7");
				TempItem.Chk7 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Chk8");
				TempItem.Chk8 = (Obj == null) ? "" : Convert.ToString(Obj);

				Obj = ActionBase.GetCollectionValue("Data", "Combo1Id");
				TempItem.Combo1IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Combo2Id");
				TempItem.Combo2IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Combo3Id");
				TempItem.Combo3IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Combo4Id");
                TempItem.Combo4IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string SaveChanges(string ConnectionString,
		                                 Int64 CompanyId,
		                                 Int64 ClientId,
		                                 AllPageEntriesRequestItem RequestItem)
		{
			string Result = "";
			string Method = "SaveChanges";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				int NewId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					NewId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							ConnectionString, "I_TRANEVENT_ID_GEN", (int)CompanyId, (int)ClientId, out Result);
					WindowsEventLoggingUtilities.WriteToInfoEvent("NewId: " + NewId.ToString(), Method, 0);
				}
				
				string QueryText = "";
				DateTime dtNow = DateTime.Now;
				string TodayStr = dtNow.ToString("dd/MM/yyyy");
				string TimeStr = dtNow.ToString("hh:mm:ff tt");
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Building Query", Method, 0);
					QueryText =
						" INSERT INTO EATransactionEventSave(" +
						"  I_TRANEVENT_ID, COMPANYCLIENTID, TRANSACTIONID, LOGINID," +
						" TRANTYPEID, PROPERTYREF, CLIENTREF, MANAGERID, BRANCHID," +
						" DATE1, DATE2, Date3, Date4, Date5, Date6, Date7, Date8, Date9, Date10, " +
						" DATE11, DATE12, Date13, Date14, Date15, Date16, Date17, Date18, Date19, Date20, " +
						" DATE21, DATE22, Date23, Date24, " +
						" CBX1, CBX2, CBX3, CBX4, CBX5, CBX6, CBX7, CBX8," +
						" DROPDOWN1, DROPDOWN2, DROPDOWN3, DROPDOWN4," +
						" TEXT1, TEXT2, TEXT3)" +
						" VALUES (" +
						" @I_TRANEVENT_ID, @COMPANYCLIENTID, @TRANSACTIONID, @LOGINID," +
						" @TRANTYPEID, @PROPERTYREF, @CLIENTREF, @MANAGERID, @BRANCHID," +
						" @DATE1, @DATE2, @Date3, @Date4, @Date5, @Date6, @Date7, @Date8, @Date9, @Date10, " +
						" @DATE11, @DATE12, @Date13, @Date14, @Date15, @Date16, @Date17, @Date18, @Date19, @Date20, " +
						" @DATE21, @DATE22, @Date23, @Date24, " +
						" @CBX1, @CBX2, @CBX3, @CBX4, @CBX5, @CBX6, @CBX7, @CBX8," +
						" @DROPDOWN1, @DROPDOWN2, @DROPDOWN3, @DROPDOWN4, " +
						" @TEXT1, @TEXT2, @TEXT3)";
					Params.Add(new FbParameterItem("I_TRANEVENT_ID", NewId));
					Params.Add(new FbParameterItem("COMPANYCLIENTID", CompanyId));
					Params.Add(new FbParameterItem("TRANSACTIONID", RequestItem.PortfolioId));
					Params.Add(new FbParameterItem("LOGINID", ClientId));
					
					Params.Add(new FbParameterItem("TRANTYPEID", RequestItem.TypeId));
					Params.Add(new FbParameterItem("PROPERTYREF", RequestItem.Property));
					Params.Add(new FbParameterItem("CLIENTREF", RequestItem.Header3Value));
					Params.Add(new FbParameterItem("MANAGERID", RequestItem.ManagerId));
					Params.Add(new FbParameterItem("BRANCHID", RequestItem.BranchId));
					
					Params.Add(new FbParameterItem("DATE1", RequestItem.Date1));
					Params.Add(new FbParameterItem("DATE2", RequestItem.Date2));
					Params.Add(new FbParameterItem("DATE3", RequestItem.Date3));
					Params.Add(new FbParameterItem("DATE4", RequestItem.Date4));
					Params.Add(new FbParameterItem("DATE5", RequestItem.Date5));
					Params.Add(new FbParameterItem("DATE6", RequestItem.Date6));
					Params.Add(new FbParameterItem("DATE7", RequestItem.Date7));
					Params.Add(new FbParameterItem("DATE8", RequestItem.Date8));
					Params.Add(new FbParameterItem("DATE9", RequestItem.Date9));
					Params.Add(new FbParameterItem("DATE10", RequestItem.Date10));
					Params.Add(new FbParameterItem("DATE11", RequestItem.Date11));
					Params.Add(new FbParameterItem("DATE12", RequestItem.Date12));
					Params.Add(new FbParameterItem("DATE13", RequestItem.Date13));
					Params.Add(new FbParameterItem("DATE14", RequestItem.Date14));
					Params.Add(new FbParameterItem("DATE15", RequestItem.Date15));
					Params.Add(new FbParameterItem("DATE16", RequestItem.Date16));
					Params.Add(new FbParameterItem("DATE17", RequestItem.Date17));
					Params.Add(new FbParameterItem("DATE18", RequestItem.Date18));
					Params.Add(new FbParameterItem("DATE19", RequestItem.Date19));
					Params.Add(new FbParameterItem("DATE20", RequestItem.Date20));
					Params.Add(new FbParameterItem("DATE21", RequestItem.Date21));
					Params.Add(new FbParameterItem("DATE22", RequestItem.Date22));
					Params.Add(new FbParameterItem("DATE23", RequestItem.Date23));
					Params.Add(new FbParameterItem("DATE24", RequestItem.Date24));

					Params.Add(new FbParameterItem("CBX1", RequestItem.Chk1));
					Params.Add(new FbParameterItem("CBX2", RequestItem.Chk2));
					Params.Add(new FbParameterItem("CBX3", RequestItem.Chk3));
					Params.Add(new FbParameterItem("CBX4", RequestItem.Chk4));
					Params.Add(new FbParameterItem("CBX5", RequestItem.Chk5));
					Params.Add(new FbParameterItem("CBX6", RequestItem.Chk6));
					Params.Add(new FbParameterItem("CBX7", RequestItem.Chk7));
					Params.Add(new FbParameterItem("CBX8", RequestItem.Chk8));
					
					Params.Add(new FbParameterItem("DROPDOWN1", RequestItem.Combo1Id));
					Params.Add(new FbParameterItem("DROPDOWN2", RequestItem.Combo2Id));
					Params.Add(new FbParameterItem("DROPDOWN3", RequestItem.Combo3Id));
                    Params.Add(new FbParameterItem("DROPDOWN4", RequestItem.Combo4Id));

                    Params.Add(new FbParameterItem("TEXT1", RequestItem.Text1));
					Params.Add(new FbParameterItem("TEXT2", RequestItem.Text2));
					Params.Add(new FbParameterItem("TEXT3", RequestItem.Text3));
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				EAResComSalesGroupUpdateNoteRequestItem UpdateItem = new EAResComSalesGroupUpdateNoteRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					EAResComSalesGroupUpdateNoteRequestItem Item = new EAResComSalesGroupUpdateNoteRequestItem();
					Result = CopyRequestObject(RequestItem, ref UpdateItem);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = Actions.Portfolio.CommonMethods.UpdateNote(ConnectionString, (int)CompanyId, (int)ClientId, UpdateItem);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SaveChanges Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string ChangeActiveStatus(string ConnectionString,
		                                        int CompanyId,
		                                        int ClientId,
		                                        AllPageEntriesRequestItem Request,
		                                        ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "ChangeActiveStatus";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveChanges(ConnectionString, CompanyId, ClientId, Request);
				}
				
				StringBuilder TempEntries = new StringBuilder();				
				if (string.IsNullOrEmpty(Result))
				{
					Result =
						Actions.Portfolio.CommonMethods.EAResComSalesGroup(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.NotesId, Request.Option1Id, Request.Option2Id, ref TempEntries);
				}
				
				string InActiveMessage = "";
				string InActiveSql = "";
				string Source = TempEntries.ToString();
				string Path = "";
				if (string.IsNullOrEmpty(Result))
				{
					Path = "$.InActiveMessage";
					InActiveMessage = JsonUtilities.GetJsonPathValueStr(Source, Path, out Result);
					//if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(InActiveMessage)) ErrorMsg = "Cannot find the Message entry";
					if (string.IsNullOrEmpty(Result))
					{
						Path = "$.InActiveSQL";
						InActiveSql = JsonUtilities.GetJsonPathValueStr(Source, Path, out Result);
						//if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(InActiveSql)) ErrorMsg = "Cannot find the Query entry";						
					}
				}
				
				if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(InActiveMessage)) Result = InActiveMessage;
				if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(InActiveSql)) Result = "Cannot find the Query entry";
				
				if (string.IsNullOrEmpty(Result))
				{
					InActiveSql = InActiveSql.Replace("__", "'");
					Result = FirebirdUtilities.DBExecute(ConnectionString, InActiveSql);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result =
						Actions.Portfolio.CommonMethods.EAResComSalesGroup(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.NotesId, Request.Option1Id, Request.Option2Id, ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After ChangeActiveStatus Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string Option1Changed(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    AllPageEntriesRequestItem Request,
		                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "Option1Changed";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveChanges(ConnectionString, CompanyId, ClientId, Request);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result =
						Actions.Portfolio.CommonMethods.EAResComSalesGroup(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.NotesId, Request.Option1Id, Request.Option2Id, ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After Option1Changed Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string Option2Changed(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    AllPageEntriesRequestItem Request,
		                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "Option2Changed";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveChanges(ConnectionString, CompanyId, ClientId, Request);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result =
						Actions.Portfolio.CommonMethods.EAResComSalesGroup(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.NotesId, Request.Option1Id, Request.Option2Id, ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After Option2Changed Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadDisplayGridReportRequestItem(ActionBase ActionBase,
		                                                      ref DisplayGridReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDisplayGridReportRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				AllPageEntriesRequestItem Item = new AllPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
	                TempItem.PortfolioIdValue = Item.PortfolioIdValue;
	                TempItem.NotesIdValue = Item.NotesIdValue;
	                TempItem.Option1IdValue = Item.Option1IdValue;
	                TempItem.Option2IdValue = Item.Option2IdValue;
	                TempItem.TypeIdValue = Item.TypeIdValue;
	                TempItem.Header4Value = Item.Header4Value;
	                TempItem.BranchIdValue = Item.BranchIdValue;
	                TempItem.Property = Item.Property;
	                TempItem.Header7Value = Item.Header7Value;
	                TempItem.ManagerIdValue = Item.ManagerIdValue;
	                TempItem.Header3Value = Item.Header3Value;
	                TempItem.Header8Value = Item.Header8Value;

	                TempItem.NoteText = Item.NoteText;

	                TempItem.Date1 = Item.Date1;
	                TempItem.Date2 = Item.Date2;
	                TempItem.Date3 = Item.Date3;
	                TempItem.Date4 = Item.Date4;
	                TempItem.Date5 = Item.Date5;
	                TempItem.Date6 = Item.Date6;
	                TempItem.Date7 = Item.Date7;
	                TempItem.Date8 = Item.Date8;
	                TempItem.Date9 = Item.Date9;
	                TempItem.Date10 = Item.Date10;
	                TempItem.Date11 = Item.Date11;
	                TempItem.Date12 = Item.Date12;
	                TempItem.Date13 = Item.Date13;
	                TempItem.Date14 = Item.Date14;
	                TempItem.Date15 = Item.Date15;
	                TempItem.Date16 = Item.Date16;
	                TempItem.Date17 = Item.Date17;
	                TempItem.Date18 = Item.Date18;
	                TempItem.Date19 = Item.Date19;
	                TempItem.Date20 = Item.Date20;
	                TempItem.Date21 = Item.Date21;	                
	                TempItem.Date22 = Item.Date22;
	                TempItem.Date23 = Item.Date23;
	                TempItem.Date24 = Item.Date24;

	                TempItem.Text1 = Item.Text1;
	                TempItem.Text2 = Item.Text2;
	                TempItem.Text3 = Item.Text3;
	                
	                TempItem.Chk1 = Item.Chk1;
	                TempItem.Chk2 = Item.Chk2;
	                TempItem.Chk3 = Item.Chk3;
	                TempItem.Chk4 = Item.Chk4;
	                TempItem.Chk5 = Item.Chk5;
	                TempItem.Chk6 = Item.Chk6;
	                TempItem.Chk7 = Item.Chk7;
	                TempItem.Chk8 = Item.Chk8;
	                
	                TempItem.Combo1IdValue = Item.Combo1IdValue;
	                TempItem.Combo2IdValue = Item.Combo2IdValue;
	                TempItem.Combo3IdValue = Item.Combo3IdValue;
	                
              
					Object Obj = ActionBase.GetCollectionValue("Data", "UniqueId");
					TempItem.UniqueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ButtonId");
					TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string DisplayGridReport(string ConnectionString,
		                                       int CompanyId,
		                                       int ClientId,
		                                       AllPageEntriesRequestItem Request,
		                                       int UniqueId,
		                                       int BtnId,
		                                       ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "DisplayGridReport";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveChanges(ConnectionString, CompanyId, ClientId, Request);
				}
				
				string ReportSource = "";
				if (string.IsNullOrEmpty(Result))
				{
					ReportSource = GetReportSource(ConnectionString, CompanyId, ClientId, Request, UniqueId, BtnId, out Result);
				}
				
				string OutputFileName = "";
				if (string.IsNullOrEmpty(Result))
				{
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After DisplayGridReport Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		private static string GetReportSource(string ConnectionString,
		                                      int CompanyId,
		                                      int ClientId,
		                                      AllPageEntriesRequestItem Request,
		                                      int UniqueId,
		                                      int ButtonId,
		                                      out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetReportSource";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						Actions.Portfolio.CommonMethods.EAResComSalesGroup(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.NotesId, Request.Option1Id, Request.Option2Id, ref Entries);
				}
				
				string Str =
					JsonUtilities.StartJson() +
					Entries.ToString() +
					JsonUtilities.EndJson();
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, Method, 0);
					
				JObject JObj = null;
				try 
				{
					if (Entries.Length > 0)
					{
						JObj = JObject.Parse(Str);
					}				
				}
				catch (Exception Ex)
				{
					ErrorMsg =
						Method + Environment.NewLine +
						Ex.Message + Environment.NewLine +
						Ex.StackTrace;					
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					string JPath = @"$.grid[?(@.I_ID == '" + UniqueId + "')]";
					WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, Method, 0);
					JToken Record = JObj.SelectToken(JPath);
					if (Record == null)
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", Method, 0);
					else
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), Method, 0);
						Result = (string)Record["ZL20_B" + ButtonId];				
						WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After GetReportSource Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		/*
		public static string LoadUpdateNoteRequestItem(ActionBase ActionBase,
		                                               ref UpdateNoteRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadUpdateNoteRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				EAResComSalesGroupRequestItem Item = new EAResComSalesGroupRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
	                TempItem.PortfolioIdValue = Item.PortfolioIdValue;
	                TempItem.NotesIdValue = Item.NotesIdValue;
	                TempItem.Option1IdValue = Item.Option1IdValue;
	                TempItem.Option2IdValue = Item.Option2IdValue;

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
		
		public static string CopyRequestObject(UpdateNoteRequestItem From,
		                                       ref EAResComSalesGroupRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.NotesIdValue = From.NotesIdValue;
                To.Option1IdValue = From.Option1IdValue;
                To.Option2IdValue = From.Option2IdValue;
                To.PortfolioIdValue = From.PortfolioIdValue;
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
		*/
		
		public static string CopyRequestObject(AllPageEntriesRequestItem From,
		                                       ref EAResComSalesGroupUpdateNoteRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.NotesIdValue = From.NotesIdValue;
                To.Option1IdValue = From.Option1IdValue;
                To.Option2IdValue = From.Option2IdValue;
                To.PortfolioIdValue = From.PortfolioIdValue;
                To.NoteText = From.NoteText;
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
	}
}
