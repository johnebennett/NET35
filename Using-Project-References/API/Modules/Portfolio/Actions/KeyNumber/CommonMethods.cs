/*
 * Created by SharpDevelop.
 * User: John
 * Date: 09/06/2020
 * Time: 7:11 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Diagnostics;

using CommonUtilities;
using EAOfficeAPI;
namespace Actions.Portfolio.KeyNumber
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
				Obj = ActionBase.GetCollectionValue("Data", "KeyNumber");
				TempItem.KeyNumberValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "KeyNumber2");
                TempItem.KeyNumber2Value = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "KeyNumber3");
                TempItem.KeyNumber3Value = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EATransactionKeys(string ConnectionString,
		                                       int CompanyId,
		                                       int PortfolioId,
		                                       int KeyNumber,
                                               int KeyNumber2,
                                               int KeyNumber3,
                                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EATransactionKeys";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (Entries.Length > 0) Entries.Append(",");
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("PortfolioId", PortfolioId, false));
				string QueryText = 
                    "SELECT OutputLine FROM EATransactionKeys(@CompanyId, @TransactionId, @KeyNumber, @KeyNumber2, @KeyNumber3)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", PortfolioId));
				Params.Add(new FbParameterItem("KeyNumber", KeyNumber));
                Params.Add(new FbParameterItem("KeyNumber2", KeyNumber2));
                Params.Add(new FbParameterItem("KeyNumber3", KeyNumber3));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				object Obj = null;
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EATransactionKeys Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadDisplayPdfReportRequestItem(ActionBase ActionBase, 
		                                                     ref DisplayPdfReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.PortfolioIdValue = Item.PortfolioIdValue;
					TempItem.KeyNumberValue = Item.KeyNumberValue;
                    TempItem.KeyNumber2Value = Item.KeyNumber2Value;
                    TempItem.KeyNumber3Value = Item.KeyNumber3Value;
                }

                Object Obj = ActionBase.GetCollectionValue("Data", "ReportId");
				TempItem.ReportIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string LoadUpdateKeysRequestItem(ActionBase ActionBase,
		                                               ref UpdateKeysRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadUpdateKeysRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.PortfolioIdValue = Item.PortfolioIdValue;
					TempItem.KeyNumberValue = Item.KeyNumberValue;
                    TempItem.KeyNumber2Value = Item.KeyNumber2Value;
                    TempItem.KeyNumber3Value = Item.KeyNumber3Value;
                }

                Object Obj = ActionBase.GetCollectionValue("Data", "Comment");
				TempItem.Comment = (Obj == null) ? "" : Convert.ToString(Obj);
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
