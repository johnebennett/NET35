/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 6/02/2024
 * Time: 9:14 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using CommonUtilities;
using EAOfficeAPI;
using System.Collections.Generic;

namespace EmailQueueManager.Actions
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public static string ConnectionString(out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			APISettingsItem ApiSettings = APISettingsFile.GetAPISettings(APIAppType.Service, out ErrorMsg);
			/*
			Console.WriteLine("DB Name: " + ApiSettings.DBName);
			Console.WriteLine("DB Server: " + ApiSettings.DBServer);
			Console.WriteLine("DB UserName: " + ApiSettings.DBUserName);
			Console.WriteLine("DB Password: " + ApiSettings.DBPassword);
			Console.WriteLine("Error Msg: " + ErrorMsg);
			*/
			try 
			{
				Result = FirebirdUtilities.BuildConnectionString(ApiSettings.DBUserName, ApiSettings.DBPassword, ApiSettings.DBName, ApiSettings.DBServer, true);
				Result = Result.Replace("user id", "User");
				Result += ";Role=rdb$admin;";
				//Console.WriteLine("Result: " + Result);
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message + Environment.NewLine + Ex.StackTrace;
				Console.WriteLine(ErrorMsg);
			}
			return Result;
		}
		
		public static string SetStartedStatus(string ConnectionString,
		                                      Int64 RunId)
		{
			string Result = "";
			try 
			{
				string QueryText =
					"UPDATE EAEventRun SET EMAIL_STATUS = 'STARTED' WHERE EVA_ID = @EvaId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EvaId", RunId));
				WriteParamsToOutput(QueryText, Params);
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);				
			}
			//catch (Exception Ex)
			catch(Exception Ex)
			{
				Result = SetErrorStatus(ConnectionString, RunId);
			}
			return Result;
		}
		
		
		public static string SetErrorStatus(string ConnectionString,
		                                    Int64 RunId)
		{
			string Result = "";
			try 
			{
				string QueryText =
					"UPDATE EAEventRun SET EMAIL_STATUS = 'Queue_Errors' WHERE EVA_ID = @EvaId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EvaId", RunId));
				WriteParamsToOutput(QueryText, Params);
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			return Result;
		}
		
		public static void WriteToOutput(string LineText)
		{			
			Console.WriteLine(LineText);
		}
		
		public static void WriteToOutput(string LineText, string IsDebug)
		{
			if (CommonUtilities.StringUtilities.SameText(IsDebug, "Y"))
				WriteToOutput(LineText);
		}
		
		public static void WriteParamsToOutput(string QueryText,
		                                       List<FbParameterItem> Params)
		{
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
        
        public static string GetExecutingAssemblyPath()
        {
        	string 
        		Result =
        			new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
        	Result = System.IO.Path.GetDirectoryName(Result);
        	string SepChar = System.IO.Path.DirectorySeparatorChar.ToString();
        	if (!Result.EndsWith(SepChar)) Result += SepChar;
        	return Result;
        }
	}
}
