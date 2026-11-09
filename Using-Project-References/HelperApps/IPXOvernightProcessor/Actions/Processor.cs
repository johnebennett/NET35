using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CommonUtilities;
using UsefulDBMethods;
using System.IO;
using System.Diagnostics;

namespace IPXOvernightProcessor.Actions
{
    public class Processor
    {
        public string Execute(string ConnectionString,
                              ConfigOptions config)
        {
            CommonMethods.WriteToOutput("Inside Processor.Execute", config.ShowDebugMessages);
            string Result = "";

            DataTable ClientsToProcess = GetClientsToProcess(ConnectionString, config);
            if (ClientsToProcess == null)
            {
                Result = "No Clients to process";
                CommonMethods.WriteToOutput(
                    string.Format(
                        "There are {0} record(s) to process.", ClientsToProcess.Rows.Count), config.ShowDebugMessages);
            }
            else
            {
                Result = "";
                Object Obj;
                Int64 CompanyClientId = 0;
                string ClientKey = "";
                string Direction = "";
                for (int Index = 1; Index <= 2; Index++)
                {
                    if (Index == 1) Direction = "UP"; else Direction = "DOWN";
                    
                    foreach (DataRow Row in ClientsToProcess.Rows)
                    {
                        Obj = Row["CompanyClientId"];
                        CompanyClientId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
                        Obj = Row["ClientKey"];
                        ClientKey = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        if ((CompanyClientId > 0) && 
                            (!string.IsNullOrEmpty(ClientKey)) && 
                            ((config.ClientToProcess == 0) || (CompanyClientId == config.ClientToProcess ))
                           )
                        //if ((CompanyClientId > 0) && (!string.IsNullOrEmpty(ClientKey)))
                        {
                            Result = ProcessSingleRequest(ConnectionString, CompanyClientId, Direction, config);
                            if (!string.IsNullOrEmpty(Result))
                            {
                                CommonMethods.WriteToErrorOutput(Result);
                            }
                        }
                    }
                }
            }
            
            return Result;
        }

        private DataTable GetClientsToProcess(string ConnectionString, ConfigOptions config)
        {
            DataTable Result = null;
            string ErrorMsg = "";

            string QueryText = config.QueryText;
            /*
                "SELECT EACompany.I_CO_COMPANYCLIENTID AS CompanyClientId, " +
                "CO_IPX_CLIENTKEY AS ClientKey " +
                "FROM EACompany " +
                "WHERE EACompany.B_CO_IPX_LIVE = 'Y' " +
                "  AND EACompany.CO_IPX_CLIENTKEY <> '' " +
                "  AND EACompany.B_CO_ACTIVE = 'Y' " +
                "ORDER BY I_CO_COMPANYCLIENTID";
            */
            if (string.IsNullOrEmpty(ErrorMsg))
            {
                CommonMethods.WriteToOutput("QueryText:" + Environment.NewLine + QueryText, config.ShowDebugMessages);            
                Result = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
            }

            return Result;
        }

        private string ProcessSingleRequest(string ConnectionString,
                                            Int64 CompanyClientId,
                                            string Direction,
                                            ConfigOptions config)
        {
            string Result = "";

            Int64 RunId = UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(ConnectionString, "I_IPXLOGGING_ID_GEN", out Result); 
            //CommonMethods.WriteToOutput(string.Format("RunId: {0}", RunId), ShowDebugMessages);
            string CommandLine = "";
            CommandLine = string.Format("{0} /RunId:{1} /Dir:{2} /Id:{3}", config.TargetAppName, RunId, Direction, CompanyClientId);
            string CommandLineArgs = string.Format("/RunId:{0} /Dir:{1} /Id:{2}", RunId, Direction, CompanyClientId);
            CommonMethods.WriteToOutput("Command Line: " + CommandLine, config.ShowDebugMessages);
            //CommonMethods.WriteToOutput("Command Line Args: " + CommandLineArgs, config.ShowDebugMessages);
            try
            {
                if (config.ShowCommandLinesOnly.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    Process runApp = new Process();
                    runApp.StartInfo.FileName = config.TargetAppName;
                    runApp.StartInfo.Arguments = CommandLineArgs;
                    runApp.Start();
                    int Tries = config.HasExitedTries; // 5 minutes
                    for (int Index = 0; Index < Tries; Index++)
                    {
                        if (!runApp.HasExited) System.Threading.Thread.Sleep(config.SleepTimeMilliseconds);
                    }
                    if (!runApp.HasExited)
                    {
                        Result = "Application did not finish in time.";
                    }
                }
            }
            catch (System.Exception ex)
            {
                 Result = ex.Message;
            }
            return Result;
        }
    }
}