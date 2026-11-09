/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 29/12/2025
 * Time: 1:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using CommonUtilities;
using EAOfficeAPI;
using System.Collections.Generic;
using System.IO;

namespace IPXOvernightProcessor.Actions
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
			APISettingsItem ApiSettings;
			using (new ConsoleSilencer())
			{
				ApiSettings = APISettingsFile.GetAPISettings(APIAppType.Service, out ErrorMsg);				
			}
			
			/*
			Console.WriteLine("DB Name: " + ApiSettings.DBName);
			Console.WriteLine("DB Server: " + ApiSettings.DBServer);
			Console.WriteLine("DB UserName: " + ApiSettings.DBUserName);
			Console.WriteLine("DB Password: " + ApiSettings.DBPassword);
			Console.WriteLine("Error Msg: " + ErrorMsg);
			*/
			
			try 
			{
				Result = 
					FirebirdUtilities.BuildConnectionString(
						ApiSettings.DBUserName, ApiSettings.DBPassword, ApiSettings.DBName, ApiSettings.DBServer, true);
				Result = Result.Replace("user id", "User");
				Result += ";Role=rdb$admin;";
				Console.WriteLine("Result: " + Result);
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message + Environment.NewLine + Ex.StackTrace;
				Console.WriteLine(ErrorMsg);
			}
			return Result;
		}
		
		public static void WriteToOutput(string LineText)
		{			
			Console.WriteLine(LineText);
			//WriteToOutput(LineText, "Y");
		}

		public static void WriteToOutput(string LineText, string IsDebug)
		{
			if (CommonUtilities.StringUtilities.SameText(IsDebug, "Y"))
				WriteToOutput(LineText);
		}

		public static void WriteToErrorOutput(string LineText)
		{
			string CurrentDate = DateTime.Today.ToString("yyyy-MM-dd");
			string CurrentDir = GetExecutingAssemblyPath();
			string CurrentFileName = "IPXOvernightProcessor_Errors_" + CurrentDate + ".txt";
			using (StreamWriter streamWriter = new StreamWriter(CurrentDir, true))
			{
				streamWriter.WriteLine(CurrentFileName + LineText);
			}
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
