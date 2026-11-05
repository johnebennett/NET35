/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/01/2019
 * Time: 12:34 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;

using CommonUtilities;
using System.IO;
using System.Collections.Generic;

namespace UsefulDBMethods
{
	/// <summary>
	/// Helper methods for generators
	/// </summary>
	public class GeneratorHelpers
	{
		
		public GeneratorHelpers()
		{
		}
		
		public static int GetNextGeneratorId(string ConnectionString,
		                                     string GeneratorName,
		                                     int CompanyId,
		                                     int ClientId,
		                                     out string ErrorMsg)
		{
			return GetNextGeneratorId(ConnectionString, GeneratorName, out ErrorMsg);
			/*
			ErrorMsg = "";
			int Result = 0;
			DataTable dt = null;			
			
			//string QueryText = "SELECT Gen_ID({0}, 1) AS GenId FROM EALOGIN WHERE I_ALOGIN_CompanyId = {1} AND I_ALOGIN_CLIENTID = {2}";
			//QueryText = string.Format(QueryText, GeneratorName, CompanyId, ClientId);
			string QueryText = "SELECT FIRST 1 Gen_ID({0}, 1) AS GenId FROM EACLIENT";
			QueryText = string.Format(QueryText, GeneratorName);
			//WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, "GetNextGeneratorId", -1);
			try 
			{
				dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
				if (dt == null)
				{
					ErrorMsg = 
						"Error retrieving the Generator Id for the Filter Event Record" + Environment.NewLine +
						"QueryText" + Environment.NewLine + QueryText;
				}
				else
				{
					// get the value from the table
					object obj = dt.Rows[0]["GenId"];
					Result = (obj == DBNull.Value) ? 0  : Convert.ToInt32(obj);
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
			*/
		}
		
		public static int GetNextGeneratorId(string ConnectionString,
		                                     string GeneratorName,
		                                     out string ErrorMsg)
		{
			ErrorMsg = "";
			int Result = 0;
			DataTable dt = null;

            /*
			string QueryText = "SELECT Gen_ID({0}, 1) AS GenId FROM EALOGIN WHERE I_ALOGIN_CompanyId = {1} AND I_ALOGIN_CLIENTID = {2}";
			QueryText = string.Format(QueryText, GeneratorName, CompanyId, ClientId);
			*/
            //WriteToOutput("ConnectionString: " + ConnectionString);
			string QueryText = "SELECT FIRST 1 Gen_ID({0}, 1) AS GenId FROM EACLIENT";
			QueryText = string.Format(QueryText, GeneratorName);
            //WriteToOutput("QueryText: " + QueryText);
            //WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, "GetNextGeneratorId", -1);
            try
            {
				dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
				if (dt == null)
				{
					ErrorMsg = 
						"Error retrieving the Generator Id for the Filter Event Record" + Environment.NewLine +
						"QueryText" + Environment.NewLine + QueryText;
				}
				else
				{
					// get the value from the table
					object obj = dt.Rows[0]["GenId"];
					Result = (obj == DBNull.Value) ? 0  : Convert.ToInt32(obj);
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}

        public static void WriteToOutput(string LineText)
        {
            string TodayStr = DateTime.Today.ToString("yyyyMMdd");
            string FileName = "GeneratorHelpers_" +  TodayStr + ".txt";
            string FWorkingDir = Directory.GetCurrentDirectory();
            if (!FWorkingDir.EndsWith(Path.DirectorySeparatorChar.ToString())) FWorkingDir += Path.DirectorySeparatorChar;
            //Console.WriteLine("WorkingDir: " + FWorkingDir);

            FileName = FWorkingDir + FileName;
            using (StreamWriter writer = new StreamWriter(FileName, true))
            {
                writer.WriteLine(LineText);
                Console.WriteLine(LineText);
            }
        }

        public static void WriteParamsToOutput(string QueryText,
                                         List<FbParameterItem> Params)
        {
            /*
            WriteToOutput("QueryText:" + Environment.NewLine + QueryText);
            foreach (FbParameterItem Item in Params)
            {
                WriteToOutput(Item.ParameterName + ": " + Convert.ToString(Item.ParameterValue));
            }
            */
            string Msg = QueryText;
            if (Params != null)
            {
                foreach (FbParameterItem Param in Params)
                {
                    Msg += Environment.NewLine;
                    Msg = Msg + Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue);
                }
            }

            WriteToOutput(Msg);
        }
    }
}
