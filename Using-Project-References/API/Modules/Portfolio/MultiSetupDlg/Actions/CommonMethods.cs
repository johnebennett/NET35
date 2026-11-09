/*
 * Created by SharpDevelop.
 * User: John
 * Date: 25/07/2020
 * Time: 12:33 PM
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
namespace Actions.Portfolio.MultiSetupDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "MultiSetupDlg"; }
		
		public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "MultiOCListId");
				TempItem.MultiOCListIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "LotOwnerListId");
				TempItem.LotOwnerListIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "SELECT OutputLine FROM EAMultiOCGroup(@CompanyId, @TransactionId, @MultiOCListId, @LotOwnerListId)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
					Params.Add(new FbParameterItem("MultiOCListId", Request.MultiOCListId));
					Params.Add(new FbParameterItem("LotOwnerListId", Request.LotOwnerListId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
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
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupPageEntries Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadUpdateMultiNameRequestItem(ActionBase ActionBase,
		                                                    ref UpdateMultiNameRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadUpdateMultiNameRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
		            TempItem.PortfolioIdValue = Item.PortfolioIdValue;
		            TempItem.MultiOCListIdValue = Item.MultiOCListIdValue;
		            TempItem.LotOwnerListIdValue = Item.LotOwnerListIdValue;
					
					Object Obj = ActionBase.GetCollectionValue("Data", "StrataName1");
					TempItem.StrataName1 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StrataName2");
					TempItem.StrataName2 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StrataName3");
					TempItem.StrataName3 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StrataName4");
					TempItem.StrataName4 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StrataName5");
					TempItem.StrataName5 = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "StrataName6");
                    TempItem.StrataName6 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "StrataName7");
                    TempItem.StrataName7 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "StrataName8");
                    TempItem.StrataName8 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "StrataName9");
                    TempItem.StrataName9 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "StrataName10");
                    TempItem.StrataName10 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "StrataName11");
                    TempItem.StrataName11 = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "StrataName12");
                    TempItem.StrataName12 = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		private static string CopyRequestObject(UpdateMultiNameRequestItem From,
		                                        ref SetupPageEntriesRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.PortfolioIdValue = From.PortfolioIdValue;
                To.LotOwnerListIdValue = From.LotOwnerListIdValue;
                To.MultiOCListIdValue = From.MultiOCListIdValue;
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		private static string CopyRequestObject(UpdateDetailsRequestItem From,
		                                        ref SetupPageEntriesRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.PortfolioIdValue = From.PortfolioIdValue;
                To.LotOwnerListIdValue = From.LotOwnerListIdValue;
                To.MultiOCListIdValue = From.MultiOCListIdValue;
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		private static string CopyRequestObject(RemoveGridEntryRequestItem From,
		                                        ref SetupPageEntriesRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.PortfolioIdValue = From.PortfolioIdValue;
                To.LotOwnerListIdValue = From.LotOwnerListIdValue;
                To.MultiOCListIdValue = From.MultiOCListIdValue;
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string UpdateMultiName(string ConnectionString,
		                                     int CompanyId,
		                                     int ClientId,
		                                     UpdateMultiNameRequestItem Request)
		{
			string Result = "";
			string Method = "UpdateMultiName";
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
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(TempEntries.ToString(), "$.UpdateMultiNameSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();				
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("strata1", Request.StrataName1));
					Params.Add(new FbParameterItem("strata2", Request.StrataName2));
					Params.Add(new FbParameterItem("strata3", Request.StrataName3));
					Params.Add(new FbParameterItem("strata4", Request.StrataName4));
					Params.Add(new FbParameterItem("strata5", Request.StrataName5));
                    Params.Add(new FbParameterItem("strata6", Request.StrataName6));
                    Params.Add(new FbParameterItem("strata7", Request.StrataName7));
                    Params.Add(new FbParameterItem("strata8", Request.StrataName8));
                    Params.Add(new FbParameterItem("strata9", Request.StrataName9));
                    Params.Add(new FbParameterItem("strata10", Request.StrataName10));
                    Params.Add(new FbParameterItem("strata11", Request.StrataName11));
                    Params.Add(new FbParameterItem("strata12", Request.StrataName12));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateMultiName Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadUpdateDetailsRequestItem(ActionBase ActionBase,
		                                                  ref UpdateDetailsRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadUpdateDetailsRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
		            TempItem.PortfolioIdValue = Item.PortfolioIdValue;
		            TempItem.MultiOCListIdValue = Item.MultiOCListIdValue;
		            TempItem.LotOwnerListIdValue = Item.LotOwnerListIdValue;
					
					Object Obj = ActionBase.GetCollectionValue("Data", "Liability");
					TempItem.LiabilityValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string UpdateDetails(string ConnectionString,
		                                   int CompanyId,
		                                   int ClientId,
		                                   UpdateDetailsRequestItem Request,
		                                   ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "UpdateDetails";
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
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(TempEntries.ToString(), "$.UpdateSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the update query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					Params.Add(new FbParameterItem("liability", Request.Liability));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateDetails Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadRemoveGridEntryRequestItem(ActionBase ActionBase,
		                                                    ref RemoveGridEntryRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadRemoveGridEntryRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
		            TempItem.PortfolioIdValue = Item.PortfolioIdValue;
		            TempItem.MultiOCListIdValue = Item.MultiOCListIdValue;
		            TempItem.LotOwnerListIdValue = Item.LotOwnerListIdValue;
					
					Object Obj = ActionBase.GetCollectionValue("Data", "UniqueId");
					TempItem.UniqueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string RemoveGridEntry(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    RemoveGridEntryRequestItem Request,
		                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "RemoveGridEntry";
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
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						ExecuteRemoveSql(
							ConnectionString, JSONSource, Request.UniqueId);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After RemoveGridEntry Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string ExecuteRemoveSql(string ConnectionString,
		                                      //int CompanyId,
		                                      //int ClientId,
		                                      JObject JSONSource,
		                                      int UniqueId)
		{
			string Result = "";
			string Method = "ExeecuteRemoveSql";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					string JPath = @"$.gridEntries[?(@.I_ID == '" + UniqueId + "')]";
					WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, Method, 0);
					JToken Record = JSONSource.SelectToken(JPath);
					if (Record == null)
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", Method, 0);
					else
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), Method, 0);
						QueryText = (string)Record["ZL100_RemoveSQL"];				
						WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + QueryText, Method, 0);
					}
					
					if (string.IsNullOrEmpty(Result) && (string.IsNullOrEmpty(QueryText))) Result = "Cannot determine the Remove Sql";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After ExeecuteRemoveSql Result: " + Result, Method, 0);
			}
			
			return Result;
		}
	}
}
