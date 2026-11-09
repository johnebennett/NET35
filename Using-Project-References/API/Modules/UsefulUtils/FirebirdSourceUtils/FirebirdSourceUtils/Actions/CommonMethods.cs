using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
using StrUtlils = CommonUtilities.StringUtilities;
namespace Actions.UsefulUtils.FirebirdSourceUtils
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "UsefulUtils.FirebirdSourceUtils"; }

        public static string LoadCommonRequestItem(ActionBase ActionBase,
                                                   ref CommonRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadCommonRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "Action");
                TempItem.Action = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "SourceObject");
                TempItem.SourceObject = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "SourceName");
                TempItem.SourceName = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "OutputFileName");
                TempItem.OutputFileName = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static void WriteToOutput(string OutputFileName, string LineText)
        {
            string DirectorySeparator = Path.DirectorySeparatorChar.ToString();
            string CurrentDir = CommonUtilities.FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
            CurrentDir += (CurrentDir.EndsWith(DirectorySeparator, StringComparison.CurrentCulture)) ? "" : DirectorySeparator;

            //string TodayStr = DateTime.Today.ToString("yyyyMMdd");
            //string FileName = "GeneratorHelpers_" + TodayStr + ".txt";
            //string FWorkingDir = Directory.GetCurrentDirectory();
            //if (!FWorkingDir.EndsWith(Path.DirectorySeparatorChar.ToString())) FWorkingDir += Path.DirectorySeparatorChar;
            //Console.WriteLine("WorkingDir: " + FWorkingDir);

            using (StreamWriter writer = new StreamWriter(OutputFileName, true))
            {
                writer.WriteLine(LineText);
                //Console.WriteLine(LineText);
            }
        }

        public static string GetStoredProcList(string ConnectionString,
                                               CommonRequestItem Request,
                                               ref List<string> ProcList)
        {
            string Result = "";
            string Method = GetModuleName() + ".GetStoredProcList";
            List<FbParameterItem> Params = new List<FbParameterItem>();

            string QueryText = "SELECT RDB$PROCEDURE_NAME FROM RDB$PROCEDURES ";
            if (! string.IsNullOrEmpty(Request.SourceName))
            {
                QueryText += " WHERE RDB$PROCEDURE_NAME = @ProcedureName ";
                Params.Add(new FbParameterItem("ProcedureName", Request.SourceName));
            }

            QueryText += " ORDER BY RDB$PROCEDURE_ID";
            DataTable tbl = null;
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

            if (Params.Count > 0)
            {
                tbl =
                    UsefulDBMethods.Helpers.GetFirstDataTableEx(
                    ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
            }
            else
            {
                tbl =
                    UsefulDBMethods.Helpers.GetFirstDataTableEx(
                    ConnectionString, QueryText, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
            }

            Object Obj = null;
            string ProcedureName = "";
            if (string.IsNullOrEmpty(Result))
            {
                foreach (DataRow Row in tbl.Rows)
                {
                    Obj = Row["rdb$procedure_name"];
                    ProcedureName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    ProcList.Add(ProcedureName);
                }
            }

            return Result;
        }

        public static string GetStoredProcSource(string ConnectionString,
                                                 CommonRequestItem Request)
        {
            string Result = "";
            string ErrorMsg = "";

            string Method = GetModuleName() + ".GetStoredProcSource";
            string QueryText = "";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            DataTable tbl = null;
            List<string> ProcList = new List<string>();

            if (string.IsNullOrEmpty(Result))
            {
                Result = GetStoredProcList(ConnectionString, Request, ref ProcList);
            }

            if (string.IsNullOrEmpty(Result))
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Stored Proc Count: " + ProcList.Count, Method, 0);
            }

            Object Obj = null;
            string ProcedureSource = "";
            //string FieldName = "";
            string ParamName = "";
            int FieldLength = 0;
            int BlobType = 0;
            string FieldType = "";
            int ParameterType = 0;
            int InputCount = 0;
            int OutputCount = 0;
            string ParamItem = "";
            List<string> InputParams = new List<string>();
            List<string> OutputParams = new List<string>();
            string OutputLine = "";
            if (string.IsNullOrEmpty(Result)) 
            {
                foreach (string ProcedureName in ProcList)
                {
                    ProcedureSource = "";
                    //FieldName = "";
                    InputParams.Clear();
                    OutputParams.Clear();
                    Params.Clear();

                    QueryText =
                        "SELECT" +
                        "  a.RDB$PROCEDURE_SOURCE, a.RDB$PROCEDURE_INPUTS," +
                        "  a.RDB$PROCEDURE_OUTPUTS, b.rdb$procedure_name, b.rdb$parameter_name," +
                        "  b.rdb$parameter_type, c.rdb$field_name, c.rdb$field_length," +
                        "  case c.rdb$field_type" +
                        "    when 007 then 'SMALLINT'" +
                        "    when 008 then 'INTEGER'" +
                        "    when 010 then 'FLOAT'" +
                        "    when 012 then 'DATE'" +
                        "    when 013 then 'TIME'" +
                        "    when 014 then 'CHAR'" +
                        "    when 016 then 'BIGINT'" +
                        "    when 027 then 'DOUBLE PRECISION'" +
                        "    when 035 then 'TIMESTAMP'" +
                        "    when 037 then 'VARCHAR'" +
                        "    when 261 then 'BLOB'" +
                        "    else 'Unknown'" +
                        "  end as FieldType," +
                        "  c.rdb$field_sub_type" +
                        //" FROM rdb$procedures a, rdb$procedure_parameters b, rdb$fields c" +
                        " FROM RDB$PROCEDURES a " +
                        "   LEFT JOIN RDB$PROCEDURE_PARAMETERS b ON " +
                        "     b.RDB$PROCEDURE_NAME = a.RDB$PROCEDURE_NAME " +
                        "   LEFT JOIN RDB$FIELDS c ON " +
                        "     c.RDB$FIELD_NAME = b.RDB$FIELD_SOURCE" +
                        " WHERE a.RDB$PROCEDURE_NAME = @ProcedureName ";
                    /*
                         +
                        "  AND b.rdb$procedure_name = a.RDB$PROCEDURE_NAME" +
                        "  AND c.rdb$field_name = b.rdb$field_source";
                    */                       

                    Params.Add(new FbParameterItem("ProcedureName", ProcedureName));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);

                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["rdb$procedure_source"];
                            ProcedureSource = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            Obj = Row["RDB$PROCEDURE_INPUTS"];
                            InputCount = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                            Obj = Row["RDB$PROCEDURE_OUTPUTS"];
                            OutputCount = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                            Obj = Row["rdb$parameter_type"];
                            ParameterType = (Obj == DBNull.Value) ? -1 : Convert.ToInt32(Obj);
                            //Obj = Row["rdb$field_name"];
                            //FieldName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            Obj = Row["RDB$PARAMETER_NAME"];
                            ParamName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            Obj = Row["FieldType"];
                            FieldType = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            FieldType = FieldType.Trim();
                            Obj = Row["rdb$field_length"];
                            FieldLength = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                            Obj = Row["rdb$field_sub_type"];
                            BlobType = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                            ParamName = ParamName.Trim();
                            ParamItem = MakeParamItem(ParamName, FieldType, FieldLength, BlobType);
                            if (ParameterType == 0)
                            {
                                InputParams.Add(ParamItem);
                            }
                            else if (ParameterType == 1)
                            {
                                OutputParams.Add(ParamItem);
                            }
                        }

                        OutputLine = "CREATE OR ALTER PROCEDURE " + ProcedureName;
                        if (InputParams.Count > 0) OutputLine += "(";
                        WriteToOutput(Request.OutputFileName, OutputLine);
                        /*
                        foreach (string AParamItem in InputParams)
                        {
                            WriteToOutput(Request.OutputFileName, AParamItem);
                        }
                        if (InputParams.Count > 0)
                        {
                            WriteToOutput(Request.OutputFileName, ")");
                        }
                        */

                        if (InputParams.Count > 0)
                        {
                            //WriteToOutput(Request.OutputFileName, "(");
                            WriteToOutput(Request.OutputFileName, string.Join("," + Environment.NewLine, InputParams.ToArray()));
                            WriteToOutput(Request.OutputFileName, ")");
                        }

                        /*
                        OutputLine = "";
                        if (OutputParams.Count > 0)
                        {
                            OutputLine += "RETURNS (";
                            WriteToOutput(Request.OutputFileName, OutputLine);
                        }
                        foreach (string AParamItem in OutputParams)
                        {
                            WriteToOutput(Request.OutputFileName, AParamItem);
                        }
                        if (OutputParams.Count > 0) WriteToOutput(Request.OutputFileName, ")");
                        */

                        if (OutputParams.Count > 0)
                        {
                            WriteToOutput(Request.OutputFileName, "RETURNS (");
                            WriteToOutput(Request.OutputFileName, string.Join("," + Environment.NewLine, OutputParams.ToArray()));
                            WriteToOutput(Request.OutputFileName, ")");
                        }

                        WriteToOutput(Request.OutputFileName, "AS");
                        WriteToOutput(Request.OutputFileName, ProcedureSource);
                        WriteToOutput(Request.OutputFileName, "");

                    }
                }
            }

            return Result;
        }

        public static string MakeParamItem(string ParamName, string FieldType, int FieldLength, int BlobType)
        {
            string Result = ParamName + " " + FieldType;

            if (StrUtlils.SameText(FieldType, "VARCHAR") || StrUtlils.SameText(FieldType, "CHAR"))
            {
                Result = Result + "(" + FieldLength + ")";
            }
            else if (StrUtlils.SameText(FieldType, "BLOB"))
            {
                Result = Result + " SUB_TYPE BINARY SEGMENT_SIZE " + FieldLength;
            }

            return Result;
        }
    }
}
