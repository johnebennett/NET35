using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
namespace IPX_App
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

            string FileName = "LoadAndSendSnapshot_" + Cmd + "_" + DateStr + ".txt";
            FileName = WorkingDir + FileName;
            using (StreamWriter writer = new StreamWriter(FileName, true))
            {
                writer.WriteLine(TimeStr + LineText);
            }
        }

        public static void WriteToErrorOutput(string LineText)
        {
            string DateStr = DateTime.Today.ToString("yyyy-MM-dd");
            string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";
            string WorkingDir = Directory.GetCurrentDirectory();
            string SepChar = Path.DirectorySeparatorChar.ToString();
            if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;

            //string FileName = "LoadAndSendSnapshot" + FRunId + ".txt";
            string FileName = "LoadAndSendSnapshot_Errors_" + DateStr + ".txt";
            FileName = WorkingDir + FileName;
            using (StreamWriter writer = new StreamWriter(FileName, true))
            {
                writer.WriteLine(TimeStr + LineText);
            }
        }

        public static void AddLoggingEntry(string ConnectionString,
                                           string RunId,
                                           string CompanyId,
                                           string Direction,
                                           string TaskId,
                                           string Url,
                                           string EntryType,
                                           string Content,
                                           string TransactionId,
                                           string InspectionKey,
                                           string ScheduledDate)
        {
            AddLoggingEntry(
                ConnectionString,
                Convert.ToInt64(RunId),
                CompanyId,
                Direction,
                Convert.ToInt64(TaskId),
                Url,
                EntryType,
                Content,
                Convert.ToInt64(TransactionId),
                InspectionKey,
                ScheduledDate);

        }

        public static void AddLoggingEntry(string ConnectionString,
                                           string RunId,
                                           string CompanyId,
                                           string Direction,
                                           string EntryType,
                                           string Content)
        {
            AddLoggingEntry(
                ConnectionString,
                Convert.ToInt64(RunId),
                CompanyId,
                Direction,
                0, // TaskId
                "", // Url
                EntryType,
                Content,
                0, // TransactionId
                "", // InspectionKey
                "01/01/1900" // ScheduledDate
                );

        }

        public static void AddLoggingEntry(string ConnectionString,
                                           Int64 RunId,
                                           string CompanyId,
                                           string Direction,
                                           Int64 TaskId,
                                           string Url,
                                           string EntryType,
                                           string Content,
                                           Int64 TransactionId,
                                           string InspectionKey,
                                           string ScheduledDate)
        {
            string Method = GetModuleName() + "AddLoggingEntry";

            ActionBase ActionBase = new ActionBase();
            ActionBase.LogOnInsideMethod(Method);

            try
            {
                string ErrorMsg = "";

                int NewId = 
                    UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
                        ConnectionString, "I_IPXLOGGING_ID_GEN", out ErrorMsg);
                WindowsEventLoggingUtilities.WriteToInfoEvent("NewId: " + NewId, Method, (int)RunId);

                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    QueryText = "INSERT INTO EAIPXLOGGING (I_IPX_ID) VALUES (@NewId)";
                    Params.Add(new FbParameterItem("NewId", NewId));
                    ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, (int)RunId);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    QueryText =
                        " UPDATE EAIPXLOGGING" +
                        "    SET I_IPX_RunNumber = @RunId," +
                        "        I_IPX_CompanyClientId = @CompanyId," +
                        "        IPX_Direction = @Direction," +
                        "        I_IPX_TaskId = @TaskId," +
                        "        IPX_URL = @Url," +
                        "        IPX_EntryType = @EntryType," +
                        "        IPX_BLOB = @Content," +
                        "        I_IPX_TransactionId = @TransactionId," +
                        "        IPX_KEY = @InspectionKey," +
                        "        D_IPX_ScheduledDate = @ScheduledDate" +
                        " WHERE I_IPX_ID = @NewId";
                    Params.Clear();
                    Params.Add(new FbParameterItem("RunId", RunId));
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("Direction", Direction));
                    Params.Add(new FbParameterItem("TaskId", TaskId));
                    Params.Add(new FbParameterItem("Url", Url));
                    Params.Add(new FbParameterItem("EntryType", EntryType));
                    byte[] bytes = Encoding.ASCII.GetBytes(Content);
                    Params.Add(new FbParameterItem("Content", bytes));
                    Params.Add(new FbParameterItem("TransactionId", TransactionId));
                    Params.Add(new FbParameterItem("InspectionKey", InspectionKey));
                    Params.Add(new FbParameterItem("ScheduledDate", ScheduledDate));
                    Params.Add(new FbParameterItem("NewId", NewId));
                    ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, (int)RunId);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                if (!string.IsNullOrEmpty(ErrorMsg))
                {
                    WindowsEventLoggingUtilities.WriteToErrorEvent(
                        "Error in AddLoggingEntry" + Environment.NewLine + ErrorMsg, "IPX_APP", 0);
                }
            }
            catch (Exception Ex)
            {
                WindowsEventLoggingUtilities.WriteToErrorEvent(
                    "Error in AddLoggingEntry" + Environment.NewLine + Ex.Message, "IPX_APP", 0);
            }
        }

        public static string BuildQueryParamsInfo(string QueryText, List<FbParameterItem> Params)
        {
            string Result = "QueryText:" + Environment.NewLine + QueryText;

            try
            {
                if (Params != null)
                {
                    foreach (FbParameterItem Param in Params)
                    {
                        Result += Environment.NewLine;
                        Result = Result + Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue);
                    }
                }

            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }

            return Result;
        }
    }
}
