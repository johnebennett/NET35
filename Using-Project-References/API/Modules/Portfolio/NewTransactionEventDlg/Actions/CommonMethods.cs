/*
 * Created by SharpDevelop.
 * User: John
 * Date: 18/07/2020
 * Time: 4:46 PM
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
namespace Actions.Portfolio.NewTransactionEventDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "NewTransactionEventDlg"; }
		
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
				Obj = ActionBase.GetCollectionValue("Data", "ButtonNumber");
				TempItem.ButtonNumberValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "TransTypeId");
				TempItem.TransTypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FindId");
				TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FindText");
				TempItem.FindText = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NeededId");
				TempItem.NeededIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "OptionalId");
				TempItem.OptionalIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "LedgerFindId");
				TempItem.LedgerFindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankAccountId");
				TempItem.BankAccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EATransactionNewEventCreate(string ConnectionString,
		                                                 int CompanyId,
		                                                 int ClientId,
		                                                 int ButtonNumber, 
		                                                 int TransTypeId,
		                                                 int PortfolioId,
		                                                 int MasterEventId)
		{
			string Result = "";
			string Method = "EATransactionNewEventCreate";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						"EXECUTE PROCEDURE EATransactionNewEventCreate(" +
						" @CompanyId, @ClientId, @ButtonNumber, @TransTypeId, @PortfolioId, @MasterEventId)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("ClientId", ClientId));
					Params.Add(new FbParameterItem("ButtonNumber", ButtonNumber));
					Params.Add(new FbParameterItem("TransTypeId", TransTypeId));
					Params.Add(new FbParameterItem("PortfolioId", PortfolioId));
					Params.Add(new FbParameterItem("MasterEventId", MasterEventId));					
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EATransactionNewEventCreate Result: " + Result, Method, 0);
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
				if (string.IsNullOrEmpty(Result) && (Request.MasterEventId == 0))
				{
					Request.MasterEventId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							ConnectionString, "I_TAEV_ID_GEN", CompanyId, ClientId, out Result);					
					if (string.IsNullOrEmpty(Result))
					{
						Result = 
							EATransactionNewEventCreate(
								ConnectionString, CompanyId, ClientId, Request.ButtonNumber, Request.TransTypeId, Request.PortfolioId, Request.MasterEventId);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("MasterEventId", Request.MasterEventId, false));
				}
				
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						"SELECT OutputLine from EATRANSACTIONNEWEVENTGROUP(" +
						"@CompanyId, @MasterEventId, @findid, @findtext, @NEEDEDID, @OPTIONALID, @LEDGERFINDID, @LEDGERBANKAccountId)";

					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("MasterEventId", Request.MasterEventId));
					Params.Add(new FbParameterItem("findid", Request.FindId));
					Params.Add(new FbParameterItem("findtext", Request.FindText));
					Params.Add(new FbParameterItem("NEEDEDID", Request.NeededId));
					Params.Add(new FbParameterItem("OPTIONALID", Request.OptionalId));
					Params.Add(new FbParameterItem("LEDGERFINDID", Request.LedgerFindId));
					Params.Add(new FbParameterItem("LEDGERBANKAccountId", Request.BankAccountId));
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
		
		/*		
		public static string OnComboChange(string ConnectionString,
		                                   int CompanyId,
		                                   int ClientId,
		                                   SetupPageEntriesRequestItem Request,
		                                   ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnComboChange";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result =
						SetupPageEntries(
							ConnectionString, CompanyId, ClientId, Request, ref TempEntries);
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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.GetSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the query";
				}
				
				DataTable tbl = null;
				object Obj = null;
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("MasterEventId", Request.MasterEventId, false));
					
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						if ((tbl.Rows.Count > 0) && (Entries.Length > 0)) Entries.Append(",");
						foreach (DataRow row in tbl.Rows)
						{
							Obj = row["OutputLine"];
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
		*/
		
		public static string AfterFind(string ConnectionString,
		                               int CompanyId,
		                               int ClientId,
		                               SetupPageEntriesRequestItem Request,
		                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "AfterFind";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result =
						SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);
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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.UpdateSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Update query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					QueryText = QueryText.Replace("@findid", Request.FindIdValue);
					//Params.Add(new FbParameterItem("findid", Request.FindId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
				}

				if (string.IsNullOrEmpty(Result))
				{
					Result =
						SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After AfterFind Result: " + Result, Method, 0);
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
	                TempItem.ButtonNumberValue = Item.ButtonNumberValue;
	                TempItem.TransTypeIdValue = Item.TransTypeIdValue;
	                TempItem.MasterEventIdValue = Item.MasterEventIdValue;
	                TempItem.FindIdValue = Item.FindIdValue;
	                TempItem.FindText = Item.FindText;
	                TempItem.NeededIdValue = Item.NeededIdValue;
	                TempItem.OptionalIdValue = Item.OptionalIdValue;
	                TempItem.LedgerFindIdValue = Item.LedgerFindIdValue;
	                TempItem.BankAccountIdValue = Item.BankAccountIdValue;
	
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
                To.ButtonNumberValue = From.ButtonNumberValue;
                To.TransTypeIdValue = From.TransTypeIdValue;
                To.MasterEventIdValue = From.MasterEventIdValue;
                To.FindIdValue = From.FindIdValue;
                To.FindText = From.FindText;
                To.NeededIdValue = From.NeededIdValue;
                To.OptionalIdValue = From.OptionalIdValue;
                To.LedgerFindIdValue = From.LedgerFindIdValue;
                To.BankAccountIdValue = From.BankAccountIdValue;
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
		
		private static string CopyRequestObject(AllPageEntriesRequestItem From,
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
                To.ButtonNumberValue = From.ButtonNumberValue;
                To.TransTypeIdValue = From.TransTypeIdValue;
                To.MasterEventIdValue = From.MasterEventIdValue;
                To.FindIdValue = From.FindIdValue;
                To.FindText = From.FindText;
                To.NeededIdValue = From.NeededIdValue;
                To.OptionalIdValue = From.OptionalIdValue;
                To.LedgerFindIdValue = From.LedgerFindIdValue;
                To.BankAccountIdValue = From.BankAccountIdValue;
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

        private static string CopyRequestObject(CreateLedgerRequestItem From,
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
                To.ButtonNumberValue = From.ButtonNumberValue;
                To.TransTypeIdValue = From.TransTypeIdValue;
                To.MasterEventIdValue = From.MasterEventIdValue;
                To.FindIdValue = From.FindIdValue;
                To.FindText = From.FindText;
                To.NeededIdValue = From.NeededIdValue;
                To.OptionalIdValue = From.OptionalIdValue;
                To.LedgerFindIdValue = From.LedgerFindIdValue;
                To.BankAccountIdValue = From.BankAccountIdValue;
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
				StringBuilder TempEntries = new StringBuilder();
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = CopyRequestObject(Request, ref Item);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result =
						SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref TempEntries);
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
							ConnectionString, /*CompanyId, ClientId,*/ JSONSource, Request.UniqueId);
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
					string JPath = @"$.gridEntries[?(@.ZR6_ID == '" + UniqueId + "')]";
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
		
		public static string LoadAllPageEntriesRequestItem(ActionBase ActionBase,
		                                                   ref AllPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAllPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
	                TempItem.PortfolioIdValue = Item.PortfolioIdValue;
	                TempItem.ButtonNumberValue = Item.ButtonNumberValue;
	                TempItem.TransTypeIdValue = Item.TransTypeIdValue;
	                TempItem.MasterEventIdValue = Item.MasterEventIdValue;
	                TempItem.FindIdValue = Item.FindIdValue;
	                TempItem.FindText = Item.FindText;
	                TempItem.NeededIdValue = Item.NeededIdValue;
	                TempItem.OptionalIdValue = Item.OptionalIdValue;
	                TempItem.LedgerFindIdValue = Item.LedgerFindIdValue;
	                TempItem.BankAccountIdValue = Item.BankAccountIdValue;
					
					string DefaultDateStr = "01/01/1900";
					Object Obj = ActionBase.GetCollectionValue("Data", "SaleDate");
					TempItem.SaleDate = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "SalePrice");
					TempItem.SalePriceValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "SaleDeposit");
					TempItem.SaleDepositValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "SaleCommission");
					TempItem.SaleCommissionValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "SaleAdvertising");
					TempItem.SaleAdvertisingValue = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "CommissionRate");
					TempItem.CommissionRateValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StatementFee");
					TempItem.StatementFeeValue = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "GSTRegistered");
					TempItem.GSTRegistered = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "LedgerABN");
					TempItem.LedgerABN = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "RentGross");
					TempItem.RentGrossValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "RentGST");
					TempItem.RentGSTValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "RentPaidFrom");
					TempItem.RentPaidFrom = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "RentCreditAmount");
					TempItem.RentCreditAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "RentPaidToInclusive");
					TempItem.RentPaidToInclusive = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "LettingFee");
					TempItem.LettingFeeValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "TenancyAdvertising");
					TempItem.TenancyAdvertisingValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "RentFrequency");
					TempItem.RentFrequencyValue = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "ShowPreparationFee");
					TempItem.ShowPreparationFee = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PreparationLandlordFee");
					TempItem.PreparationLandlordFeeValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PreparationTenantFee");
					TempItem.PreparationTenantFeeValue = (Obj == null) ? "" : Convert.ToString(Obj);
					
					Obj = ActionBase.GetCollectionValue("Data", "BondAmount");
					TempItem.BondAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "NoLots");
                    TempItem.NoLotsValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "FirstLotNo");
                    TempItem.FirstLotNoValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "FirstUnitNo");
                    TempItem.FirstUnitNoValue = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "LeaseStart");
                    TempItem.LeaseStart = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "LeaseExpiry");
                    TempItem.LeaseExpiry = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj);
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
		
		public static string Post(string ConnectionString,
		                          int CompanyId,
		                          int ClientId,
		                          AllPageEntriesRequestItem Request,
		                          ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "Post";
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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Post query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("saledate", Request.SaleDate));
					Params.Add(new FbParameterItem("saleprice", Request.SalePrice));
					Params.Add(new FbParameterItem("saledeposit", Request.SaleDeposit));
					Params.Add(new FbParameterItem("salecommission", Request.SaleCommission));
					Params.Add(new FbParameterItem("saleadvertising", Request.SaleAdvertising));
					
					Params.Add(new FbParameterItem("commissionrate", Request.CommissionRate));
					Params.Add(new FbParameterItem("statementfee", Request.StatementFee));
					
					Params.Add(new FbParameterItem("gstregistered", Request.GSTRegistered));
					Params.Add(new FbParameterItem("abn", Request.LedgerABN));
					Params.Add(new FbParameterItem("rentgross", Request.RentGross));
					Params.Add(new FbParameterItem("rentgst", Request.RentGST));
					Params.Add(new FbParameterItem("rentpaidto", Request.RentPaidFrom));
					Params.Add(new FbParameterItem("rentcredit", Request.RentCreditAmount));
					Params.Add(new FbParameterItem("rentfrequency", Request.RentFrequency));
					Params.Add(new FbParameterItem("rentinclusive", Request.RentPaidToInclusive));
					Params.Add(new FbParameterItem("rentlettingfee", Request.LettingFee));
					Params.Add(new FbParameterItem("rentadvertising", Request.TenancyAdvertising));
					
					Params.Add(new FbParameterItem("landlordpreparationfee", Request.PreparationLandlordFee));
					Params.Add(new FbParameterItem("tenantpreparationfee", Request.PreparationTenantFee));
					Params.Add(new FbParameterItem("showpreparationfee", Request.ShowPreparationFee));
					Params.Add(new FbParameterItem("bondamount", Request.BondAmount));
                    Params.Add(new FbParameterItem("NoLots", Request.NoLots));
                    Params.Add(new FbParameterItem("StartingLotNo", Request.FirstLotNo));
                    Params.Add(new FbParameterItem("StartingUnitNo", Request.FirstUnitNo));
                    Params.Add(new FbParameterItem("leasestart", Request.LeaseStart));
                    Params.Add(new FbParameterItem("leaseexpiry", Request.LeaseExpiry));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL2", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second Post query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("PostSQL2:" + Environment.NewLine + QueryText, GetModuleName(), 0);
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL3", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Third Post query";
				}
				
				DataTable tbl = null;
				object Obj = null;
				string NewId = "";
				string NewType = "";
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["NEWTRANSACTIONID"];
							NewId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
							Obj = Row["ScreenType"];
							NewType = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						}
					}
					
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(NewId)) Result = "No New ID value returned";
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(NewType)) Result = "No New Type value returned";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("NewId: " + NewId + Environment.NewLine + "NewType: " + NewType, GetModuleName(), 0);
					if (Entries.Length > 0) Entries.Append(",");
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("NewId", NewId, true));
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("NewType", NewType, false));
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

        public static string LoadCreateLedgerRequestItem(ActionBase ActionBase,
                                                            ref CreateLedgerRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadCreateLedgerRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                Result = Item.Load(ActionBase);
                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.PortfolioIdValue = Item.PortfolioIdValue;
                    TempItem.ButtonNumberValue = Item.ButtonNumberValue;
                    TempItem.TransTypeIdValue = Item.TransTypeIdValue;
                    TempItem.MasterEventIdValue = Item.MasterEventIdValue;
                    TempItem.FindIdValue = Item.FindIdValue;
                    TempItem.FindText = Item.FindText;
                    TempItem.NeededIdValue = Item.NeededIdValue;
                    TempItem.OptionalIdValue = Item.OptionalIdValue;
                    TempItem.LedgerFindIdValue = Item.LedgerFindIdValue;
                    TempItem.BankAccountIdValue = Item.BankAccountIdValue;

                    Object Obj = ActionBase.GetCollectionValue("Data", "ABN");
                    TempItem.ABN = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "GSTRegistered");
                    TempItem.GSTRegistered = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "CommissionRate");
                    TempItem.CommissionRateValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "StmtFee");
                    TempItem.StmtFeeValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string AddLedger(string ConnectionString,
                                       int CompanyId,
                                       int ClientId,
                                       CreateLedgerRequestItem Request,
                                       ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "AddLedger";
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
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AddLedgerSQL1", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first AddLedger query";
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("ABN", Request.ABN));
                    Params.Add(new FbParameterItem("GST", Request.GSTRegistered));
                    Params.Add(new FbParameterItem("COMMISSIONRATE", Request.CommissionRate));
                    Params.Add(new FbParameterItem("STATEMENTFEE", Request.StmtFee));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AddLedgerSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second AddLedger query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, GetModuleName(), 0);
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

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AddLedgerSQL3", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third AddLedger query";
                }

                DataTable tbl = null;
                object Obj = null;
                string LedgerId = "";
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["I_ICE_LEDGERID"];
                            LedgerId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        }

                        if (LedgerId == "") Result = "Cannot determine the new Ledger Id";
                        WindowsEventLoggingUtilities.WriteToInfoEvent("New LedgerId: " + LedgerId, Method, 0);
                    }
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Item.LedgerFindIdValue = LedgerId;
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
            }

            return Result;
        }
    }
}
