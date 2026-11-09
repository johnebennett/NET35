/*
 * Created by SharpDevelop.
 * User: John
 * Date: 07/07/2020
 * Time: 10:30 AM
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
namespace Actions.Portfolio.Stocklist
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
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

				Obj = ActionBase.GetCollectionValue("Data", "Text1");
				TempItem.Text1 = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Text2");
				TempItem.Text2 = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Text3");
				TempItem.Text3 = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Text4");
				TempItem.Text4 = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Text5");
				TempItem.Text5 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Text6");
				TempItem.Text6 = (Obj == null) ? "" : Convert.ToString(Obj);				

				Obj = ActionBase.GetCollectionValue("Data", "Text7");
				TempItem.Text7 = (Obj == null) ? "" : Convert.ToString(Obj);				
				Obj = ActionBase.GetCollectionValue("Data", "Text8");
				TempItem.Text8 = (Obj == null) ? "" : Convert.ToString(Obj);				

				Obj = ActionBase.GetCollectionValue("Data", "Amount1");
				TempItem.Amount1Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Amount2");
				TempItem.Amount2Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Amount3");
				TempItem.Amount3Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Amount4");
                TempItem.Amount4Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Amount5");
                TempItem.Amount5Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Amount6");
                TempItem.Amount6Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Chk1");
				TempItem.Chk1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Combo2Id");
				TempItem.Combo2IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
						" DATE1, DATE2, Date3, Date4, Date5, " +
						" AMOUNT1, AMOUNT2, AMOUNT3, " +
                        " AMOUNT4, AMOUNT5, AMOUNT6, " +
						" CBX1, " +
						" DROPDOWN2," +
						" TEXT1, TEXT2, TEXT3, TEXT4, TEXT5, Text6, Text7, Text8)" +
						" VALUES (" +
						" @I_TRANEVENT_ID, @COMPANYCLIENTID, @TRANSACTIONID, @LOGINID," +
						" @TRANTYPEID, @PROPERTYREF, @CLIENTREF, @MANAGERID, @BRANCHID," +
						" @DATE1, @DATE2, @Date3, @Date4, @Date5," +
						" @AMOUNT1, @AMOUNT2, @AMOUNT3, " +
                        " @AMOUNT4, @AMOUNT5, @AMOUNT6, " +
                        " @CBX1, " +
						" @DROPDOWN2," +
						" @TEXT1, @TEXT2, @TEXT3, @TEXT4, @TEXT5, @Text6, @Text7, @Text8)";
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
					
					Params.Add(new FbParameterItem("AMOUNT1", RequestItem.Amount1));
					Params.Add(new FbParameterItem("AMOUNT2", RequestItem.Amount2));
					Params.Add(new FbParameterItem("AMOUNT3", RequestItem.Amount3));
                    Params.Add(new FbParameterItem("AMOUNT4", RequestItem.Amount4));
                    Params.Add(new FbParameterItem("AMOUNT5", RequestItem.Amount5));
                    Params.Add(new FbParameterItem("AMOUNT6", RequestItem.Amount6));

					Params.Add(new FbParameterItem("CBX1", RequestItem.Chk1));
					
					Params.Add(new FbParameterItem("DROPDOWN2", RequestItem.Combo2Id));

					Params.Add(new FbParameterItem("TEXT1", RequestItem.Text1));
					Params.Add(new FbParameterItem("TEXT2", RequestItem.Text2));
					Params.Add(new FbParameterItem("TEXT3", RequestItem.Text3));
					Params.Add(new FbParameterItem("TEXT4", RequestItem.Text4));
					Params.Add(new FbParameterItem("TEXT5", RequestItem.Text5));
					Params.Add(new FbParameterItem("TEXT6", RequestItem.Text6));
					Params.Add(new FbParameterItem("TEXT7", RequestItem.Text7));
					Params.Add(new FbParameterItem("TEXT8", RequestItem.Text8));
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SaveChanges Result: " + Result, Method, 0);
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

	                TempItem.Text1 = Item.Text1;
	                TempItem.Text2 = Item.Text2;
	                TempItem.Text3 = Item.Text3;
	                TempItem.Text4 = Item.Text4;
	                TempItem.Text5 = Item.Text5;
	                TempItem.Text6 = Item.Text6;
	                TempItem.Text7 = Item.Text7;
	                TempItem.Text8 = Item.Text8;
	                
	                TempItem.Amount1Value = Item.Amount1Value;
	                TempItem.Amount2Value = Item.Amount2Value;
	                TempItem.Amount3Value = Item.Amount3Value;
                    TempItem.Amount4Value = Item.Amount4Value;
                    TempItem.Amount5Value = Item.Amount5Value;
                    TempItem.Amount6Value = Item.Amount6Value;

                    TempItem.Chk1 = Item.Chk1;
	                
	                TempItem.Combo2IdValue = Item.Combo2IdValue;
	                
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
		
		public static string NotesChanged(string ConnectionString,
		                                  int CompanyId,
		                                  int ClientId,
		                                  AllPageEntriesRequestItem Request,
		                                  ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "NotesChanged";
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After NotesChanged Result: " + Result, Method, 0);
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
	}
}
