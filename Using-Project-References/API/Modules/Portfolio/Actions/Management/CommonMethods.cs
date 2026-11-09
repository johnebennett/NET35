/*
 * Created by SharpDevelop.
 * User: John
 * Date: 04/06/2020
 * Time: 1:14 PM
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
namespace Actions.Portfolio.Management
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		/*
		public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
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
				}
				
//				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
//				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
//				Obj = ActionBase.GetCollectionValue("Data", "NotesId");
//				TempItem.NotesIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
//				Obj = ActionBase.GetCollectionValue("Data", "Option1Id");
//				TempItem.Option1IdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
//				Obj = ActionBase.GetCollectionValue("Data", "Option2Id");
//				TempItem.Option2IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAResComSalesGroup(string ConnectionString,
		                                        Int64 CompanyId,
		                                        Int64 ClientId,
		                                        int TransactionId,
		                                        int NotesId,
		                                        int Option1,
		                                        int Option2,
		                                        ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAResComSalesGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (Entries.Length > 0) Entries.Append(",");
				
				string QueryText = 
					"SELECT OutputLine FROM EAResComSalesGroup(@CompanyId, @TransactionId, @LoginId, @NotesId, @Option1, @Option2)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("NotesId", NotesId));
				Params.Add(new FbParameterItem("Option1", Option1));
				Params.Add(new FbParameterItem("Option2", Option2));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				object Obj = null;
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						if (Entries.Length > 0) Entries.Append(",");
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
					WindowsEventLoggingUtilities.WriteToInfoEvent("Start - OwnerInstructions", Method, 0);
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					WindowsEventLoggingUtilities.WriteToInfoEvent("Before", Method, 0);
					NoteSql = JsonUtilities.GetJsonPathValueStr(Str, "$.NoteSQL", out Result);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After", Method, 0);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(NoteSql)) Result = "Cannot find the Notes entry";
					if (string.IsNullOrEmpty(Result))
					{
						NoteSql = NoteSql.Replace("__", "'");
						WindowsEventLoggingUtilities.WriteToInfoEvent("OwnerNoteSql:" + Environment.NewLine + NoteSql, Method, 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EAResComSalesGroup Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		*/
		
		public static string LoadManagementPageRequestItem(ActionBase ActionBase,
		                                                   ref ManagementPageRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadManagementPageRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string DefaultDateStr = "01/01/1900";
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "TypeId");
				TempItem.TypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BranchId");
				TempItem.BranchIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ManagerId");
				TempItem.ManagerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Property");
				TempItem.Property = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Header3Value");
				TempItem.Header3Value = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Header4Value");
				TempItem.Header4Value = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Header7Value");
				TempItem.Header7Value = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Header8Value");
				TempItem.Header8Value = (Obj == null) ? "" : Convert.ToString(Obj);
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
				Obj = ActionBase.GetCollectionValue("Data", "Combo1Id");
				TempItem.Combo1IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Combo2Id");
				TempItem.Combo2IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Int4");
				TempItem.Int4Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Int5");
				TempItem.Int5Value = (Obj == null) ? "" : Convert.ToString(Obj);
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
				Obj = ActionBase.GetCollectionValue("Data", "Chk2");
				TempItem.Chk2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Chk3");
				TempItem.Chk3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Chk4");
				TempItem.Chk4 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NotesId");
				TempItem.NotesIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "NoteText");
				TempItem.NoteText= (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Option1Id");
				TempItem.Option1IdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Option2Id");
				TempItem.Option2IdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string SaveManagementEntries(string ConnectionString,
		                                           Int64 CompanyId,
		                                           Int64 ClientId,
		                                           ManagementPageRequestItem RequestItem)
		{
			string Result = "";
			string Method = "SaveManagementEntries";
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
						"  I_TRANEVENT_ID, DATEADDED, TIMEADDED, COMPANYCLIENTID, TRANSACTIONID, LOGINID," +
						" TRANTYPEID, PROPERTYREF, CLIENTREF, MANAGERID, BRANCHID," +
						" DATE1, DATE2, DATE3, DATE4, DATE5, DATE6, DATE7, DATE8, DATE9, DATE10," +
						" AMOUNT1, AMOUNT2, AMOUNT3, AMOUNT4, AMOUNT5, AMOUNT6," +
						" INTEGER4, INTEGER5," +
						" TEXT1, TEXT2, TEXT3, TEXT4, TEXT5," +
						" CBX1, CBX2, CBX3, CBX4," +
						" DROPDOWN1, DROPDOWN2)" +
						"VALUES (" +
						" @I_TRANEVENT_ID, @DATEADDED, @TIMEADDED, @COMPANYCLIENTID, @TRANSACTIONID, @LOGINID," +
						" @TRANTYPEID, @PROPERTYREF, @CLIENTREF, @MANAGERID, @BRANCHID," +
						" @DATE1, @DATE2, @DATE3, @DATE4, @DATE5, @DATE6, @DATE7, @DATE8, @DATE9, @DATE10," +
						" @AMOUNT1, @AMOUNT2, @AMOUNT3, @Amount4, @Amount5, @Amount6," +
						" @INTEGER4, @INTEGER5," +
						" @TEXT1, @TEXT2, @TEXT3, @TEXT4, @TEXT5," +
						" @CBX1, @CBX2, @CBX3, @CBX4," +
						" @DROPDOWN1, @DROPDOWN2)";
					Params.Add(new FbParameterItem("I_TRANEVENT_ID", NewId));
					Params.Add(new FbParameterItem("DATEADDED", TodayStr));
					Params.Add(new FbParameterItem("TIMEADDED", TimeStr));
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
					Params.Add(new FbParameterItem("AMOUNT1", RequestItem.Amount1));
					Params.Add(new FbParameterItem("AMOUNT2", RequestItem.Amount2));
					Params.Add(new FbParameterItem("AMOUNT3", RequestItem.Amount3));
                    Params.Add(new FbParameterItem("AMOUNT4", RequestItem.Amount4));
                    Params.Add(new FbParameterItem("AMOUNT5", RequestItem.Amount5));
                    Params.Add(new FbParameterItem("AMOUNT6", RequestItem.Amount6));
                    Params.Add(new FbParameterItem("INTEGER4", RequestItem.Int4));
					Params.Add(new FbParameterItem("INTEGER5", RequestItem.Int5));					
					Params.Add(new FbParameterItem("TEXT1", RequestItem.Text1));
					Params.Add(new FbParameterItem("TEXT2", RequestItem.Text2));
					Params.Add(new FbParameterItem("TEXT3", RequestItem.Text3));
					Params.Add(new FbParameterItem("TEXT4", RequestItem.Text4));
					Params.Add(new FbParameterItem("TEXT5", RequestItem.Text5));
					Params.Add(new FbParameterItem("CBX1", RequestItem.Chk1));
					Params.Add(new FbParameterItem("CBX2", RequestItem.Chk2));
					Params.Add(new FbParameterItem("CBX3", RequestItem.Chk3));
					Params.Add(new FbParameterItem("CBX4", RequestItem.Chk4));
					Params.Add(new FbParameterItem("DROPDOWN1", RequestItem.Combo1Id));
					Params.Add(new FbParameterItem("DROPDOWN2", RequestItem.Combo2Id));
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
			}
			
			return Result;
		}
		
		/*
		public static string SaveManagementEntries(string ConnectionString,
		                                           Int64 CompanyId,
		                                           Int64 ClientId,
		                                           string UpdateSql,
		                                           string UpdateSql1,
		                                           string UpdateSql2,
		                                           string UpdateSql3,
		                                           ManagementPageRequestItem RequestItem)
		{
			string Result = "";
			string Method = "SaveManagementEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					if (!string.IsNullOrEmpty(UpdateSql))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Before UpdateSql", Method, 0);
						Result = SaveManagementEntriesUpdateSql(ConnectionString, UpdateSql, RequestItem);
						WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateSql", Method, 0);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (!string.IsNullOrEmpty(UpdateSql1))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Before UpdateSql1", Method, 0);
						Result = SaveManagementEntriesUpdateSql1(ConnectionString, UpdateSql1, RequestItem);
						WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateSql1", Method, 0);
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
			}
			
			return Result;
		}
		
		public static string SaveManagementEntriesUpdateSql(string ConnectionString,
		                                                    string UpdateSql,
		                                                    ManagementPageRequestItem RequestItem)
		{
			string Result = "";
			string Method = "SaveManagementEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("PropertyRef", RequestItem.Property));
				Params.Add(new FbParameterItem("Header3Value", RequestItem.Header3Value));
				Params.Add(new FbParameterItem("BranchId", RequestItem.BranchId));
				Params.Add(new FbParameterItem("ManagerId", RequestItem.ManagerId));
				Params.Add(new FbParameterItem("BUTYPEID ", RequestItem.TypeId));				
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, UpdateSql, Params, 0);
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
		
		public static string SaveManagementEntriesUpdateSql1(string ConnectionString,
		                                                     string UpdateSql,
		                                                     ManagementPageRequestItem RequestItem)
		{
			string Result = "";
			string Method = "SaveManagementEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("PropertyRef", RequestItem.Property));
				Params.Add(new FbParameterItem("Header3Value", RequestItem.Header3Value));
				Params.Add(new FbParameterItem("BranchId", RequestItem.BranchId));
				Params.Add(new FbParameterItem("ManagerId", RequestItem.ManagerId));
				Params.Add(new FbParameterItem("BUTYPEID ", RequestItem.TypeId));				
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, UpdateSql, Params, 0);
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
		*/
		
		public static string LoadDisplayGridReportRequestItem(ActionBase ActionBase,
		                                                   ref DisplayGridReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDisplayGridReportRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				ManagementPageRequestItem Item = new ManagementPageRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{					
	                TempItem.PortfolioIdValue = Item.PortfolioIdValue;
	                TempItem.TypeIdValue = Item.TypeIdValue;
	                TempItem.BranchIdValue = Item.BranchIdValue;
	                TempItem.ManagerIdValue = Item.ManagerIdValue;
	                TempItem.Property = Item.Property;
	                TempItem.Header3Value = Item.Header3Value;
	                TempItem.Header4Value = Item.Header4Value;
	                TempItem.Header7Value = Item.Header7Value;
	                TempItem.Header8Value = Item.Header8Value;
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
	                TempItem.Text1 = Item.Text1;
	                TempItem.Text2 = Item.Text2;
	                TempItem.Text3 = Item.Text3;
	                TempItem.Text4 = Item.Text4;
	                TempItem.Text5 = Item.Text5;
	                TempItem.Combo1IdValue = Item.Combo1IdValue;
	                TempItem.Combo2IdValue = Item.Combo2IdValue;
	                TempItem.Int4Value = Item.Int4Value;
	                TempItem.Int5Value = Item.Int5Value;
	                TempItem.Amount1Value = Item.Amount1Value;
	                TempItem.Amount2Value = Item.Amount2Value;
	                TempItem.Amount3Value = Item.Amount3Value;
                    TempItem.Amount4Value = Item.Amount4Value;
                    TempItem.Amount5Value = Item.Amount5Value;
                    TempItem.Amount6Value = Item.Amount6Value;
                    TempItem.Chk1 = Item.Chk1;
	                TempItem.Chk2 = Item.Chk2;
	                TempItem.Chk3 = Item.Chk3;
	                TempItem.Chk4 = Item.Chk4;
	                TempItem.NotesIdValue = Item.NotesIdValue;
	                TempItem.NoteText = Item.NoteText;
	                TempItem.Option1IdValue = Item.Option1IdValue;
	                TempItem.Option2IdValue = Item.Option2IdValue;
				}
				Object Obj = ActionBase.GetCollectionValue("Data", "UniqueId");
				TempItem.UniqueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ButtonId");
				TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string LoadDisplayGridLinkRequestItem(ActionBase ActionBase,
                                                           ref DisplayGridLinkRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadDisplayGridLinkRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                ManagementPageRequestItem Item = new ManagementPageRequestItem();
                Result = Item.Load(ActionBase);
                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.TypeIdValue = Item.TypeIdValue;
                    TempItem.BranchIdValue = Item.BranchIdValue;
                    TempItem.ManagerIdValue = Item.ManagerIdValue;
                    TempItem.Property = Item.Property;
                    TempItem.Header3Value = Item.Header3Value;
                    TempItem.Header4Value = Item.Header4Value;
                    TempItem.Header7Value = Item.Header7Value;
                    TempItem.Header8Value = Item.Header8Value;
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
                    TempItem.Text1 = Item.Text1;
                    TempItem.Text2 = Item.Text2;
                    TempItem.Text3 = Item.Text3;
                    TempItem.Text4 = Item.Text4;
                    TempItem.Text5 = Item.Text5;
                    TempItem.Combo1IdValue = Item.Combo1IdValue;
                    TempItem.Combo2IdValue = Item.Combo2IdValue;
                    TempItem.Int4Value = Item.Int4Value;
                    TempItem.Int5Value = Item.Int5Value;
                    TempItem.Amount1Value = Item.Amount1Value;
                    TempItem.Amount2Value = Item.Amount2Value;
                    TempItem.Amount3Value = Item.Amount3Value;
                    TempItem.Amount4Value = Item.Amount4Value;
                    TempItem.Amount5Value = Item.Amount5Value;
                    TempItem.Amount6Value = Item.Amount6Value;
                    TempItem.Chk1 = Item.Chk1;
                    TempItem.Chk2 = Item.Chk2;
                    TempItem.Chk3 = Item.Chk3;
                    TempItem.Chk4 = Item.Chk4;
                    TempItem.NotesIdValue = Item.NotesIdValue;
                    TempItem.NoteText = Item.NoteText;
                    TempItem.Option1IdValue = Item.Option1IdValue;
                    TempItem.Option2IdValue = Item.Option2IdValue;
                }
                Object Obj = ActionBase.GetCollectionValue("Data", "UniqueId");
                TempItem.UniqueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "LinkId");
                TempItem.LinkIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
