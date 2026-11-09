/*
 * Created by SharpDevelop.
 * User: John
 * Date: 06/07/2020
 * Time: 2:36 PM
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
namespace Actions.Portfolio
{
	/// <summary>
	/// Description of PortfolioMgmtCommonMethods.
	/// </summary>
	public class PortfolioMgmtCommonMethods
	{
		public PortfolioMgmtCommonMethods()
		{
		}
		
		public static string LoadEAResComSalesGroupRequestItem(ActionBase ActionBase,
		                                                       ref EAResComSalesGroupRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadEAResComSalesGroupRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
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
	}
}
