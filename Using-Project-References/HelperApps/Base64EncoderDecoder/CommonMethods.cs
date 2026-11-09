using System;
using System.IO;
/*
using System.Data;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
*/
using FirebirdSql.Data.FirebirdClient;

namespace Base64EncoderDecoder
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "IPX_LoadAndSendSnapshot."; }

        public static void WriteToOutput(string Cmd, string LineText)
        {
            string WorkingDir = Directory.GetCurrentDirectory();
            string SepChar = Path.DirectorySeparatorChar.ToString();
            if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
            string DateStr = DateTime.Today.ToString("yyyy-MM-dd");
            string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";

            string FileName = "Base64EncoderDecoder_" + Cmd + "_" + DateStr + ".txt";
            FileName = WorkingDir + FileName;
            /*
            using (StreamWriter writer = new StreamWriter(FileName, true))
            {
                writer.WriteLine(TimeStr + LineText);
            }
            */
        }

        public static void WriteToErrorOutput(string LineText)
        {
            string DateStr = DateTime.Today.ToString("yyyy-MM-dd");
            string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";
            string WorkingDir = Directory.GetCurrentDirectory();
            string SepChar = Path.DirectorySeparatorChar.ToString();
            if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;

            //string FileName = "LoadAndSendSnapshot" + FRunId + ".txt";
            string FileName = "Base64EncoderDecoder_Errors_" + DateStr + ".txt";
            FileName = WorkingDir + FileName;
            /*
            using (StreamWriter writer = new StreamWriter(FileName, true))
            {
                writer.WriteLine(TimeStr + LineText);
            }
            */
        }

        public static void DisplayUsage()
        {
            Console.WriteLine("Base64EncoderDecoder");
            Console.WriteLine("Usage:");
            Console.WriteLine("Action: Encode or Decode");
            Console.WriteLine("Type: File-To-File (F2F), File-To-Database (F2D), Database-To-File (D2F), Database-To-Database (D2D)");
            Console.WriteLine("File-To- File");
            Console.WriteLine("=============");
            Console.WriteLine("/SrcFile - Source File");
            Console.WriteLine("/DestFile - Destination File");
        }

		public static string TestDatabaseConnection(string ConnectionString)
		{
			string result = "";
			using (FbConnection fbConnection = new FbConnection(ConnectionString))
			{
				try
				{
					fbConnection.Open();
				}
				catch (System.Exception ex)
				{
					result = ex.Message;
				}
			}
			return result;
		}
    }
}
