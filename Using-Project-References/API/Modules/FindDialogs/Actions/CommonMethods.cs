/*
 * Created by SharpDevelop.
 * User: John
 * Date: 11/07/2020
 * Time: 3:28 PM
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
namespace Actions.FindDialogs
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
			
		public static string EAFindScreenSql(string ConnectionString,
		                                     int CompanyId,
		                                     int ClientId,
		                                     int TypeId,
		                                     string TableToSearch,
		                                     string FindText,
		                                     string IncludeInactives,
		                                     ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAFindScreenSql";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT SqlStatement FROM EAFindScreenSql(@CompanyId, @TypeId, @TableToSearch, @FindText, @IncludeInactives)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TypeId", TypeId));
				Params.Add(new FbParameterItem("TableToSearch", TableToSearch));
				Params.Add(new FbParameterItem("FindText", FindText));
				Params.Add(new FbParameterItem("IncludeInactives", IncludeInactives));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				object Obj = null;
				string SqlStatement = "";
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow row in tbl.Rows)
						{
							Obj = row["SqlStatement"];
							SqlStatement = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						}
					}
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SqlStatement)) Result = "Cannot find the Query value";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					SqlStatement = SqlStatement.Replace("__", "'");
					WindowsEventLoggingUtilities.WriteToInfoEvent("SqlStatement:" + Environment.NewLine + SqlStatement, Method, 0);
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, SqlStatement, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
						JsonUtilities.BuildJsonArrayFromDataTable(tbl, "GridEntries", ref Entries);
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
}
}
