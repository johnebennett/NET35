using System;
using System.Collections.Generic;
using System.Data;
using CommonUtilities;
namespace UsefulDBMethods
{
 public class CompanyFolderSettings
 {
public static string DocsLocation(string ConnectionString,
		                                  Int64 CompanyClientId,
		                                  out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "DocsLocation";
			
			try 
			{
				string QueryText = "SELECT CO_DocsLocation FROM EACompanySettings WHERE I_Settings_CompanyId = @CompanyClientId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				DataTable tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, 
						UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
				//Console.WriteLine("After GetFirstDataTable");
				if (string.IsNullOrEmpty(Result))
				{
					object Obj = tbl.Rows[0]["CO_DocsLocation"];
					Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
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

        public static string ImportStorageLocation(string ConnectionString,
                                                   Int64 CompanyClientId,
                                                   out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";
            string Method = "DocsLocation";

            try
            {
                string QueryText = "SELECT CO_ImportStorage FROM EACompanySettings WHERE I_Settings_CompanyId = @CompanyClientId";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
                ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                DataTable tbl =
                    UsefulDBMethods.Helpers.GetFirstDataTableEx(
                        ConnectionString, QueryText, Params, 0,
                        UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
                //Console.WriteLine("After GetFirstDataTable");
                if (string.IsNullOrEmpty(Result))
                {
                    object Obj = tbl.Rows[0]["CO_ImportStorage"];
                    Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                }
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

        public static string TestRPOLocation(string ConnectionString,
		                                     Int64 CompanyClientId,
		                                     out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "TestRPOLocation";
			
			try 
			{
				string QueryText = "SELECT CO_TestRPOLocation FROM EACompanySettings WHERE I_Settings_CompanyId = @CompanyClientId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				DataTable tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, 
						UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
				//Console.WriteLine("After GetFirstDataTable");
				if (string.IsNullOrEmpty(Result))
				{
					object Obj = tbl.Rows[0]["CO_TestRPOLocation"];
					Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
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

        public static string EAOfficeServiceLocation(string ConnectionString,
                                                     Int64 CompanyClientId,
                                                     out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";
            string Method = "EAOfficeServiceLocation";

            try
            {
                string QueryText = "SELECT CO_ServiceFolderLocation FROM EACompanySettings WHERE I_Settings_CompanyId = @CompanyClientId";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
                ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                DataTable tbl =
                    UsefulDBMethods.Helpers.GetFirstDataTableEx(
                        ConnectionString, QueryText, Params, 0,
                        UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
                //Console.WriteLine("After GetFirstDataTable");
                if (string.IsNullOrEmpty(Result))
                {
                    object Obj = tbl.Rows[0]["CO_ServiceFolderLocation"];
                    Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                }
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

        
 }
}
