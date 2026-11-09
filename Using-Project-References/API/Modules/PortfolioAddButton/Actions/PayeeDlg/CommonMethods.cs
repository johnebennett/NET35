/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/06/2020
 * Time: 11:43 AM
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
using UsefulDBMethods;
namespace Actions.PortfolioAddButton.PayeeDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Option1Id");
				TempItem.Option1IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				//WindowsEventLoggingUtilities.WriteToInfoEvent("Option1IdValue: " + TempItem.Option1IdValue, Method, 0);
				Obj = ActionBase.GetCollectionValue("Data", "Option2Id");
				TempItem.Option2IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				//WindowsEventLoggingUtilities.WriteToInfoEvent("Option2IdValue: " + TempItem.Option2IdValue, Method, 0);
				Obj = ActionBase.GetCollectionValue("Data", "GridId");
				TempItem.GridIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "TempId");
				TempItem.TempId = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAPayeeTempAdd(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    int TransactionId,
		                                    int FindId,
		                                    string FindText,
		                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAPayeeTempAdd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT OutputLine FROM EAPayeeTempAdd(@CompanyId, @TransactionId, @LoginId, @FindId, @FindText)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("FindId", FindId));
				Params.Add(new FbParameterItem("FindText", FindText));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
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
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EAPayeeTempAdd Result: " + Result, Method, 0);
			}
			
			return Result;
		}		
		
		public static string LoadGetPayeeDetailsRequestItem(ActionBase ActionBase,
		                                                     ref GetPayeeDetailsRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGetPayeeDetailsRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.PortfolioIdValue = Item.PortfolioIdValue;
	                TempItem.Option1IdValue = Item.Option1IdValue;
	                TempItem.Option2IdValue = Item.Option2IdValue;
	                TempItem.GridIdValue = Item.GridIdValue;
	                
					Object Obj = ActionBase.GetCollectionValue("Data", "FindId");
					TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "FindText");
					TempItem.FindText = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string LoadAddPayeeRequestItem(ActionBase ActionBase,
		                                             ref AddPayeeRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAddPayeeRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.PortfolioIdValue = Item.PortfolioIdValue;
	                TempItem.Option1IdValue = Item.Option1IdValue;
	                TempItem.Option2IdValue = Item.Option2IdValue;
	                TempItem.GridIdValue = Item.GridIdValue;
	                
					Object Obj = ActionBase.GetCollectionValue("Data", "FindId");
					TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "FindText");
					TempItem.FindText = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string AddPayee(string ConnectionString,
		                              int CompanyId,
		                              int ClientId,
		                              AddPayeeRequestItem Request)
		{
			string Result = "";
			string Method = "AddPayee";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result = 
					EAPayeeTempAdd(
						ConnectionString, CompanyId, ClientId, 
						Request.PortfolioId, Request.FindId, Request.FindText, ref Entries);
				
				string Source = "";
				string PostSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					PostSql = JsonUtilities.GetJsonPathValueStr(Source, "$.PostSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(PostSql)) Result = "Cannot find the Save entry";					
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, PostSql);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After AddPayee Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string EAPayeeTempGroup(string ConnectionString,
		                                      int CompanyId,
		                                      int ClientId,
		                                      int TransactionId,
		                                      int GridId,
		                                      string TempId,
		                                      ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAPayeeTempGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				WindowsEventLoggingUtilities.WriteToInfoEvent("TempId: " + TempId, Method, 0);
				if (TempId == "0")
				{
					if (string.IsNullOrEmpty(Result))
					{
						QueryText = "EXECUTE PROCEDURE EAPayeeTempCreate(@CompanyId, @TransactionId, @LoginId)";
						Params.Clear();
						Params.Add(new FbParameterItem("CompanyId", CompanyId));
						Params.Add(new FbParameterItem("TransactionId", TransactionId));
						Params.Add(new FbParameterItem("LoginId", ClientId));
						Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "SELECT OutputLine FROM EAPayeeTempGroup(@CompanyId, @TransactionId, @LoginId, @ClientId)";
					Params.Clear();
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", TransactionId));
					Params.Add(new FbParameterItem("LoginId", ClientId));
					Params.Add(new FbParameterItem("ClientId", GridId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
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
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EAPayeeTempGroup Result: " + Result, Method, 0);
			}
			
			return Result;
		}		
		
		public static string LoadUpdateRecordRequestItem(ActionBase ActionBase,
		                                                 ref UpdateRecordRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadUpdateRecordRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.PortfolioIdValue = Item.PortfolioIdValue;
	                TempItem.Option1IdValue = Item.Option1IdValue;
	                TempItem.Option2IdValue = Item.Option2IdValue;
	                TempItem.GridIdValue = Item.GridIdValue;
	                TempItem.TempId = Item.TempId;
	                
					Object Obj = ActionBase.GetCollectionValue("Data", "PayeeId");
					TempItem.PayeeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StmtName");
					TempItem.StmtName = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "Address1");
					TempItem.Address1 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Address2");
					TempItem.Address2 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Address3");
					TempItem.Address3 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Address4");
					TempItem.Address4 = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "DirectEntry");
					TempItem.DirectEntry = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "BSB");
					TempItem.BSB = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountNumber");
					TempItem.AccountNumber = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountDetail");
					TempItem.AccountDetail = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "RemoveWhenPosted");
					TempItem.RemoveWhenPosted = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Percentage");
					TempItem.PercentageValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string UpdateRecord(string ConnectionString,
		                                  int CompanyId,
		                                  int ClientId,
		                                  UpdateRecordRequestItem Request,
		                                  ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "UpdateRecord";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Temp = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EAPayeeTempGroup(
							ConnectionString, CompanyId, ClientId, Request.PortfolioId, 
							Request.GridId, Request.TempId, ref Temp);
				}
				
				string Source = "";
				string UpdateSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Temp.ToString() +
						JsonUtilities.EndJson();
					UpdateSql = JsonUtilities.GetJsonPathValueStr(Source, "$.UpdateSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(UpdateSql)) Result = "Cannot find the Update entry";
					WindowsEventLoggingUtilities.WriteToInfoEvent("UpdateSql:" + Environment.NewLine + UpdateSql, Method, 0);
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("bsb", Request.BSB));
					Params.Add(new FbParameterItem("accountnumber", Request.AccountNumber));
					Params.Add(new FbParameterItem("ACCOUNTDETAIL", Request.AccountDetail));
					Params.Add(new FbParameterItem("percent", Request.Percentage));
					Params.Add(new FbParameterItem("directentry", Request.DirectEntry));
					Params.Add(new FbParameterItem("remove", Request.RemoveWhenPosted));
					Params.Add(new FbParameterItem("statementname", Request.StmtName));
					Params.Add(new FbParameterItem("statementaddress1", Request.Address1));
					Params.Add(new FbParameterItem("statementaddress2", Request.Address2));
					Params.Add(new FbParameterItem("statementaddress3", Request.Address3));
					Params.Add(new FbParameterItem("statementaddress4", Request.Address4));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql, Params, Method, 0);
					
					if (string.IsNullOrEmpty(Result))
						Result = FirebirdUtilities.DBExecute(ConnectionString, UpdateSql, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EAPayeeTempGroup(
							ConnectionString, CompanyId, ClientId, Request.PortfolioId, 
							Request.GridId, Request.TempId, ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After AddPayee Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string PostRecord(string ConnectionString,
		                                int CompanyId,
		                                int ClientId,
		                                SetupPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "PostRecord";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Temp = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EAPayeeTempGroup(
							ConnectionString, CompanyId, ClientId, Request.PortfolioId, 
							Request.GridId, Request.TempId, ref Temp);
				}
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Temp.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.PostValidateSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Validate entry";
					WindowsEventLoggingUtilities.WriteToInfoEvent("PostValidateSql:" + Environment.NewLine + QueryText, Method, 0);

					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.PostSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Post entry";
					WindowsEventLoggingUtilities.WriteToInfoEvent("PostSql:" + Environment.NewLine + QueryText, Method, 0);

					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
					}
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After AddPayee Result: " + Result, Method, 0);
			}
			
			return Result;
		}
	}
}
