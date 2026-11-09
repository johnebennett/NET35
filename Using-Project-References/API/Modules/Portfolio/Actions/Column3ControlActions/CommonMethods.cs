/*
 * Created by SharpDevelop.
 * User: John
 * Date: 14/07/2020
 * Time: 10:44 AM
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
namespace Actions.Portfolio.Column3ControlActions
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "RelationsAddDlg"; }
		
		#region AddBondLedger		
		public static string LoadAddBondLedgerRequestItem(ActionBase ActionBase,
		                                                  ref AddBondLedgerRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAddBondLedgerRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NotesId");
				TempItem.NotesIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string AddBondLedger(string ConnectionString,
		                                   int CompanyId,
		                                   int ClientId,
		                                   int TransactionId)
		{
			string Result = "";
			string Method = "AddBondLedger";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "execute procedure EATRANSACTIONBONDLEDGERADD(@CompanyId, @TransactionId, @ClientId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", TransactionId));
					Params.Add(new FbParameterItem("ClientId", ClientId));
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
        #endregion

        #region AddHoldingLedger       
        public static string LoadAddHoldingLedgerRequestItem(ActionBase ActionBase,
                                                          ref AddHoldingLedgerRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadAddHoldingLedgerRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
                TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "NotesId");
                TempItem.NotesIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string AddHoldingLedger(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
                                              int TransactionId)
        {
            string Result = "";
            string Method = "AddHoldingLedger";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "execute procedure EATRANSACTIONBONDLEDGERADD(@CompanyId, @TransactionId, @ClientId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", TransactionId));
                    Params.Add(new FbParameterItem("ClientId", ClientId));
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
        #endregion
    }
}
