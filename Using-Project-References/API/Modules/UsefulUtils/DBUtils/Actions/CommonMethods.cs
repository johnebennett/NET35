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
namespace Actions.UsefulUtils.DBUtils
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "UsefulUtils.DBUtils"; }

        public static string LoadRunQueryRequestItem(ActionBase ActionBase,
                                                     ref RunQueryRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadRunQueryRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "Source");
                TempItem.Source = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "UseParams");
                TempItem.UseParams = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Output");
                TempItem.Output = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string RunQuery(string ConnectionString,
                                      RunQueryRequestItem Request,
                                      ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "RunQuery";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();
            ActionBase.LogOnInsideMethod(Method);
            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                string QueryText = File.ReadAllText(Request.Source);

                if (StringUtilities.SameText(Request.UseParams, "Y"))
                {
                    if (string.IsNullOrEmpty(Result))
                    {
                        QueryText =
                            "SELECT OutputSql FROM OCEAccrualsLedgerStart(" +
                            "@CompanyId, @TransactionId, @SubAccountId, @DateFrom, @DateTo, @ChartId, @LoginId, @AddJnl)";
                        /*
                        Params.Add(new FbParameterItem("CompanyId", CompanyId));
                        Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
                        Params.Add(new FbParameterItem("SubAccountId", Request.SubAccountId));
                        Params.Add(new FbParameterItem("DateFrom", Request.DateFrom));
                        Params.Add(new FbParameterItem("DateTo", Request.DateTo));
                        Params.Add(new FbParameterItem("ChartId", Request.ChartId));
                        Params.Add(new FbParameterItem("LoginId", ClientId));
                        Params.Add(new FbParameterItem("AddJnl", Request.AddJnl));
                        Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
                        */                       
                    }
                }

                DataTable tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);

                    if (string.IsNullOrEmpty(Result))
                    {
                        if (StringUtilities.SameText(Request.Output, "JSON"))
                        {
                            Result = BuildJsonArrayFromDataTable(tbl, "Result", ref Entries);
                        }
                        else if (StringUtilities.SameText(Request.Output, "HtmlTable"))
                        {
                            Result = ConvertDataTableToHTML(tbl, ref Entries);
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                if (StringUtilities.SameText(Request.Output, "HtmlTable"))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After RunQuery Result: HtmlTable Output", Method, 0);
                }
                else
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After RunQuery Result: " + Result, Method, 0);
                }
            }

            return Result;
        }

        /// <summary>
        /// BuildJsonArrayFromDataTable is used to generate JSON entries
        /// that can be used in the web page for grid displays
        /// </summary>
        /// <param name="tblSource">A DataTable containing the values to generate the content from.
        /// This table can be empty</param>
        /// <param name="ArrayName">The name that will be used for the JSON array of values </param>
        /// <param name="Entries">The StringBuilder that will be used to contain the generated content</param>
        /// <returns></returns>
        public static string BuildJsonArrayFromDataTable(DataTable tblSource,
                                                         string ArrayName,
                                                         ref StringBuilder Entries)
        {
            string Result = "";
            if (tblSource == null)
            {
                Result = "Source Table is NULL";
            }

            if (!string.IsNullOrEmpty(Result)) return Result;

            string Str = "";
            string ColName = "";
            string ColValue = "";
            List<string> RowList = new List<string>();
            List<string> StrList = new List<string>();
            object Obj = null;
            try
            {
                Entries.AppendLine(JsonUtilities.StartJsonArray(ArrayName));
                foreach (DataRow ThisRow in tblSource.Rows)
                {
                    StrList.Clear();
                    foreach (DataColumn ThisCol in tblSource.Columns)
                    {
                        ColName = ThisCol.ColumnName;
                        Obj = ThisRow[ColName];
                        if (ThisCol.DataType == typeof(System.Byte[]))
                        {
                            byte[] toBytes = (byte[])Obj;
                            ColValue = System.Text.Encoding.Default.GetString(toBytes);
                            //ColValue = "This is a byte array";
                        }
                        else
                        {
                            ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        }
                        StrList.Add(JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
                    }
                    Str =
                        JsonUtilities.StartJsonObject("") +
                        string.Join(",", StrList.ToArray()) +
                        JsonUtilities.EndJsonObject();
                    RowList.Add(Str);
                }
                Entries.Append(string.Join(",", RowList.ToArray()));
                Entries.AppendLine(JsonUtilities.EndJsonArray());
            }
            catch (Exception Ex)
            {
                Result =
                    Ex.Message + Environment.NewLine +
                    Ex.InnerException + Environment.NewLine +
                    Ex.StackTrace;
            }

            return Result;
        }

        public static string ConvertDataTableToHTML(DataTable dt, ref StringBuilder Entries)
        {
            Entries.AppendLine("<!DOCTYPE html>");
            Entries.AppendLine("<html>");
            Entries.AppendLine("<head>");
            Entries.AppendLine("<style>");
            Entries.AppendLine(@"table {font-family: arial, sans-serif; border-collapse: collapse; width: 100%; }");
            Entries.AppendLine(@"td {vertical-align: top;}");
            Entries.AppendLine(@"td, th {border: 1px solid #dddddd; text-align: left; padding: 8px;}");
            Entries.AppendLine(@"tr:nth-child(even) {background-color: #dddddd;}");
            Entries.AppendLine("</style>");
            Entries.AppendLine("</head>");
            Entries.AppendLine("<body>");

            Entries.AppendLine("<table border='1'>");
            //add header row
            Entries.Append("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
                Entries.Append("<td>" + dt.Columns[i].ColumnName + "</td>");
            Entries.AppendLine("</tr>");

            /*
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            */


            string ColName = "";
            string ColValue = "";
            object Obj = null;

            foreach (DataRow ThisRow in dt.Rows)
            {
                Entries.AppendLine("<tr>");
                foreach (DataColumn ThisCol in dt.Columns)
                {
                    ColName = ThisCol.ColumnName;
                    Obj = ThisRow[ColName];
                    if (ThisCol.DataType == typeof(System.Byte[]))
                    {
                        byte[] toBytes = (byte[])Obj;
                        ColValue = System.Text.Encoding.Default.GetString(toBytes);
                        //ColValue = "This is a byte array";
                    }
                    else
                    {
                        ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    }
                    Entries.Append("<td>" + ColValue + "</td>");
                }
                Entries.AppendLine("</tr>");
            }
            Entries.Append("</table>");
            Entries.AppendLine("</body>");
            Entries.AppendLine("</html>");
            return "";
        }
    }
}
