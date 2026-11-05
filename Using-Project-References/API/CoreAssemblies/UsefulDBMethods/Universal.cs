/*
 * Created by SharpDevelop.
 * User: John
 * Date: 03/08/2020
 * Time: 7:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Collections.Generic;

using CommonUtilities;

namespace UsefulDBMethods
{
	/// <summary>
	/// Description of Universal.
	/// </summary>
	public class Universal
	{
		public Universal()
		{
		}
		
		public static string TransactionGetDisplayType(string ConnectionString,
		                                               int CompanyId,
		                                               int TransactionId,
		                                               out string DisplayType)
		{
			DisplayType = "";
			string Result = "";
			DataTable dt = null;
            string Method = "TransactionGetDisplayType";

            string QueryText = "SELECT OutputValue FROM EATransactionGetDisplayType(@CompanyId, @TransactionId)";
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TransactionId", TransactionId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    dt =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result) && (dt == null))
                        Result =
                            "Error retrieving the TransactionGetDisplayType result:" + Environment.NewLine +
                            "QueryText" + Environment.NewLine + QueryText;
                }


                if (string.IsNullOrEmpty(Result))
                {
                    object obj = dt.Rows[0]["OutputValue"];
                    DisplayType = (obj == DBNull.Value) ? "" : Convert.ToString(obj);
                }
                /*
                dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
				if (dt == null)
				{
					Result = 
						"Error retrieving the TransactionGetDisplayType result:" + Environment.NewLine +
						"QueryText" + Environment.NewLine + QueryText;
				}
				else
				{
					// get the value from the table
					object obj = dt.Rows[0]["OutputValue"];
					DisplayType = (obj == DBNull.Value) ? ""  : Convert.ToString(obj);
				}
				*/
            }
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		
		public static string RelationGetDisplayType(string ConnectionString,
		                                               int CompanyId,
		                                               int RelationId,
		                                               out string DisplayType,
		                                               out int DisplayId)
		{
			DisplayType = "";
			DisplayId = 0;
			string Result = "";
			DataTable dt = null;			
			
			string QueryText = "SELECT OutputValue, OutputId FROM EARelationGetDisplayType(@CompanyId, @RelationId)";
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("RelationId", RelationId));
				dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
				if (dt == null)
				{
					Result = 
						"Error retrieving the TransactionGetDisplayType result:" + Environment.NewLine +
						"QueryText" + Environment.NewLine + QueryText;
				}
				else
				{
					// get the value from the table
					object obj = dt.Rows[0]["OutputValue"];
					DisplayType = (obj == DBNull.Value) ? ""  : Convert.ToString(obj);
					obj = dt.Rows[0]["OutputId"];
					DisplayId = (obj == DBNull.Value) ? 0: Convert.ToInt32(obj);
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
	}
}
