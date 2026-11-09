/*
 * Created by SharpDevelop.
 * User: DELL 990
 * Date: 05/09/2019
 * Time: 10:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using CommonUtilities;


namespace EmailProcessor.Actions
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetEventRunEmailBody(string ConnectionString,
		                                          Int64 RunId,
		                                          out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetEventRunEmailBody";
			string QueryText = "SELECT EVA_EmailBody FROM EAEventRun WHERE EVA_ID = @RunId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			DataTable tbl = null;
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					Console.WriteLine("Inside " + Method + " ErrorMsg: " + ErrorMsg);
				}
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				object Obj = tbl.Rows[0]["EVA_EmailBody"];
				byte[] toBytes = (byte[])Obj;
				Result = System.Text.Encoding.Default.GetString(toBytes);
			}

			return Result;
		}
	}
}
