/*
 * Created by SharpDevelop.
 * User: John
 * Date: 12/06/2020
 * Time: 2:14 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

//using Actions.Portfolio.BankingDlg;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
namespace Actions.Portfolio.VacateDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string RemoveChargeTableEntry(string ConnectionString,
		                                            int CompanyId,
		                                            int TransactionId)
		{
			string Result = "";
			string Method = "SaveManagementEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			List<FbParameterItem> Params = new List<FbParameterItem>();
			try 
			{
				string QueryText =
					" DELETE FROM  eachargetovacate where I_ECV_COMPANYCLIENTID=@CompanyId" +
					" and I_ECV_TRANSACTIONID=@TransactionId and B_ECV_POSTED='N' AND B_ECV_REVERSED='N'";
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
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
		
		public static string EATransactionVacateValidate(string ConnectionString,
		                                                 int CompanyId,
		                                                 int ClientId,
		                                                 int TransactionId,
		                                                 out string OutputMessage,
		                                                 out int OutputId,
		                                                 out string ShowVacateForm,
		                                                 out string ShowUnvacateForm,
		                                                 out string OutputVacateDate,
		                                                 out string InsertSql,
		                                                 out string SelectSql)
		{
			string Result = "";
			string Method = "EATransactionVacateValidate";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			OutputMessage = "";
			OutputId = 0;
			ShowVacateForm = "";
			ShowUnvacateForm = "";
			InsertSql = "";
			SelectSql = "";
			OutputVacateDate = "";

			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			List<FbParameterItem> Params = new List<FbParameterItem>();
			try 
			{
				string QueryText = "SELECT * FROM EATransactionVacateValidate(@CompanyId, @TransactionId, @LoginId)";
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);				
				
				object Obj = null;
				DateTime TheDateTime = DateTime.Now;
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{							
							Obj = Row["InsertSql"];
							InsertSql = (Obj == DBNull.Value) ? "NULL" : Convert.ToString(Obj);							
							//WindowsEventLoggingUtilities.WriteToInfoEvent("InsertSql: " + InsertSql, Method, 0);
							
							Obj = Row["SelectSql"];
							SelectSql = (Obj == DBNull.Value) ? "NULL" : Convert.ToString(Obj);
							//WindowsEventLoggingUtilities.WriteToInfoEvent("SelectSql: " + SelectSql, Method, 0);

							Obj = Row["OutputMessage"];
							OutputMessage = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
							//WindowsEventLoggingUtilities.WriteToInfoEvent("OutputMessage: " + OutputMessage, Method, 0);
							
							Obj = Row["OutputId"];
							OutputId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
							//WindowsEventLoggingUtilities.WriteToInfoEvent("OutputId: " + OutputId, Method, 0);
							
							Obj = Row["ShowVacateForm"];
							ShowVacateForm = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
							//WindowsEventLoggingUtilities.WriteToInfoEvent("ShowVacateForm: " + ShowVacateForm, Method, 0);
							
							Obj = Row["ShowUnVacateForm"];
							ShowUnvacateForm = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
							//WindowsEventLoggingUtilities.WriteToInfoEvent("ShowUnvacateForm: " + ShowUnvacateForm, Method, 0);
							
							Obj = Row["OutputVacateDate"];
							TheDateTime  = (Obj == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(Obj);
							WindowsEventLoggingUtilities.WriteToInfoEvent("TheDateTime: " + TheDateTime, Method, 0);
							OutputVacateDate = TheDateTime.ToString("dd/MM/yyyy");
							//WindowsEventLoggingUtilities.WriteToInfoEvent("OutputVacateDate: " + OutputVacateDate, Method, 0);
						}
					}
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
		
		public static string ShowVacateForm(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    int TransactionId,
		                                    int OutputId,
		                                    string InsertSql,
		                                    string SelectSql,
		                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "ShowVacateForm";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

			ActionBase ActionBase = new ActionBase();			
			ActionBase.LogOnInsideMethod(Method);
			List<FbParameterItem> Params = new List<FbParameterItem>();
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					//WindowsEventLoggingUtilities.WriteToInfoEvent("InsertSql:" + Environment.NewLine + InsertSql, Method, 0);
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(InsertSql, null, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						InsertSql = InsertSql.Replace("\"", "'");
						Result = FirebirdUtilities.DBExecute(ConnectionString, InsertSql);
					}
				}
				
				object Obj = null;
				DateTime TheDateTime = DateTime.Now;
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					//WindowsEventLoggingUtilities.WriteToInfoEvent("SelectSql:" + Environment.NewLine + SelectSql, Method, 0);
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(SelectSql, null, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						tbl =
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, SelectSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
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
					
					/*
					if (Entries.Length > 0)
					{
						string Str = Entries.ToString();
						JObject jobj = JObject.Parse(Str);
						
					}
					*/
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
		
		public static string ShowUnVacateForm(string ConnectionString,
		                                      int CompanyId,
		                                      int ClientId,
		                                      int TransactionId,
		                                      int OutputId,
		                                      string InsertSql,
		                                      ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "ShowUnVacateForm";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

			ActionBase ActionBase = new ActionBase();			
			ActionBase.LogOnInsideMethod(Method);
			List<FbParameterItem> Params = new List<FbParameterItem>();
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("InsertSql:" + Environment.NewLine + InsertSql, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, InsertSql);
				}
				
				/*
				if (string.IsNullOrEmpty(Result))
				{				
					if (Entries.Length > 0) Entries.Append(",");
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("Status", "Success", false));
				}
				*/
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
		
		public static string LoadSaveChangesRequestItem(ActionBase ActionBase,
		                                                ref VacateDlg.SaveChangesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSaveChangesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
