/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 3/12/2024
 * Time: 1:21 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Text;

namespace ReverseProxy.Actions
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public static class CommonMethods
	{
        
        public static string GetExecutingAssemblyPath()
        {
        	string 
        		Result =
        			new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
        	Result = Path.GetDirectoryName(Result);
        	string SepChar = Path.DirectorySeparatorChar.ToString();
        	if (!Result.EndsWith(SepChar)) Result += SepChar;
        	return Result;
        }

        public static void WriteToOutput(string LineText)
        {
            //string WorkingDir = Directory.GetCurrentDirectory();
            string WorkingDir = GetExecutingAssemblyPath();
            string SepChar = Path.DirectorySeparatorChar.ToString();
            if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
            string DateStr = DateTime.Today.ToString("yyyy-MM-dd");
            string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";
            
            Console.WriteLine(TimeStr + LineText);
        }
        
        public static void WriteToOutputLog(string LineText)
        {
        	//string UniqueId = new Guid().ToString();
        	string UniqueId = DateTime.Today.ToString("yyyy-MM-dd");
        	WriteToOutputLog(UniqueId, LineText);
        }
        
        public static void WriteToOutputLog(string UniqueId, string LineText)
        {
			string FileName = "ReverseProxy_" + UniqueId + ".txt";
			string WorkingDir = GetExecutingAssemblyPath();
			FileName = WorkingDir + FileName;
			string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " ";
			using (StreamWriter writer = new StreamWriter(FileName, true))
			{
				writer.WriteLine(TimeStr + LineText);
				//Console.WriteLine(TimeStr + LineText);
			}
        }
        
        public static void WriteToReferrerLog(string LineText)
        {
        	//string UniqueId = new Guid().ToString();
        	string UniqueId = DateTime.Today.ToString("yyyy-MM-dd");
        	WriteToReferrerLog(UniqueId, LineText);
        }
        
        public static void WriteToReferrerLog(string UniqueId, string LineText)
        {
			string FileName = "Referrer_ReverseProxy_" + UniqueId + ".txt";
			string WorkingDir = GetExecutingAssemblyPath();
			FileName = WorkingDir + FileName;
			string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " ";
			using (StreamWriter writer = new StreamWriter(FileName, true))
			{
				writer.WriteLine(TimeStr + LineText);
				//Console.WriteLine(TimeStr + LineText);
			}
        }

        public static void WriteToErrorOutput(string LineText)
        {
            string DateStr = DateTime.Today.ToString("yyyy-MM-dd");
            string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";
            string WorkingDir = GetExecutingAssemblyPath();
            string SepChar = Path.DirectorySeparatorChar.ToString();
            if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;

            string FileName = "ReverseProxy_Errors_" + DateStr + ".txt";
            FileName = WorkingDir + FileName;
            using (StreamWriter writer = new StreamWriter(FileName, true))
            {
                writer.WriteLine(TimeStr + LineText);
            }
        }
	}
}
