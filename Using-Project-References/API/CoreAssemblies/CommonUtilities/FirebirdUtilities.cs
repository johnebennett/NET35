/*
 * Created by SharpDevelop.
 * User: John Bennet
 * Date: 29/05/2017
 * Time: 9:31 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 
 /* Derived from FirebirdUtilies for web application and modified to use in RPO assemblies
  * 
  * 
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Text;
//using CommonUtilities;

namespace CommonUtilities
{
	public enum DBFieldType
	{
		ftString,
		ftSmallInt,
		ftInteger,
		ftFloat,
		ftCurrency,
		ftDate,
		ftTime,
		ftDateTime,
		ftUnknown
	}

	public class FbParameterItem
	{
		public string ParameterName = "";
		public object ParameterValue = null;

        public FbParameterItem(string ParameterName, object ParameterValue)
        {
            this.ParameterName = ParameterName;
            this.ParameterValue = ParameterValue;
        }
	}

	/// <summary>
	/// Description of FirebirdUtilities
	/// </summary>
	
	public class FirebirdUtilities
	{

		public FirebirdUtilities()
		{
		}		
		
		public static bool TableExists(string connectionString, string TableName)
		{
			bool result = false;
			int Total = 0;
			
			using (FbConnection conn = new FbConnection(connectionString))
			{
				conn.Open();

				using (FbTransaction trans = conn.BeginTransaction())
				{
					using (FbCommand cmd = new FbCommand())
					{
						cmd.Connection = conn;
						cmd.Transaction = trans;
						string QueryText =
							"SELECT COUNT(RDB$RELATION_NAME) AS Total FROM RDB$RELATIONS " +
							"WHERE RDB$RELATION_NAME = @RelationName";
						cmd.CommandText = QueryText;
						cmd.Parameters.AddWithValue("@RelationName", TableName.ToUpper());
						FbDataReader reader = cmd.ExecuteReader();
						if (reader.HasRows == false)
						{
							result = false;
						}

						while (reader.Read())
						{
							Total = Convert.ToInt32(reader["Total"]);
							if (Total > 0) { result = true; }
						}
					} // FbCommand
					trans.Commit();
				} // FbTransaction			
				conn.Close();
			} // FbConnection
			
			return result;
		}

		public static bool FieldExists(string connectionString,
		                               string TableName,
									   string FieldName)
		{
			bool result = false;
			TableName = TableName.ToUpper();
			FieldName = FieldName.ToUpper();
			int Total = 0;
			string QueryText =
				"SELECT COUNT(f.RDB$FIELD_NAME) AS Total " +
				"FROM RDB$RELATION_FIELDS f " +
				"  INNER JOIN RDB$RELATIONS r ON " +
				"    f.RDB$RELATION_NAME = r.RDB$RELATION_NAME " +
				"    AND r.RDB$VIEW_BLR IS NULL " +
				"    AND (r.RDB$SYSTEM_FLAG IS NULL OR r.RDB$SYSTEM_FLAG = 0) " +
				"WHERE f.RDB$RELATION_NAME = @RelationName " +
				"  AND f.RDB$FIELD_NAME = @FieldName";
			using (FbConnection conn = new FbConnection(connectionString))
			{
				conn.Open();

				using (FbTransaction trans = conn.BeginTransaction())
				{
					using (FbCommand cmd = new FbCommand())
					{
						cmd.Connection = conn;
						cmd.Transaction = trans;
						cmd.CommandText = QueryText;
						cmd.Parameters.AddWithValue("@RelationName", TableName.ToUpper());
						cmd.Parameters.AddWithValue("@FieldName", FieldName.ToUpper());
						FbDataReader reader = cmd.ExecuteReader();
						if (reader.HasRows == false)
						{
							result = false;
						}

						while (reader.Read())
						{
							Total = Convert.ToInt32(reader["Total"]);
							if (Total > 0) { result = true; }
						}
					} // FbCommand
					trans.Commit();
				} // FbTransaction			
				conn.Close();
			} // FbConnection
			return result;
		}

		public static string AddMissingField(string connectionString,
											 string TableName,
										     string FieldName,
										     string FieldType)
		{
			return AddMissingField(connectionString, TableName, FieldName, FieldType, 0);
		}

		public static string AddMissingField(string connectionString,
											 string TableName,
										     string FieldName,
										     DBFieldType FieldType)
		{
			return AddMissingField(connectionString, TableName, FieldName, FieldType, 0);
		}

		public static string AddMissingField(string connectionString,
											 string TableName,
										     string FieldName,
										     DBFieldType FieldType,
										     int VarCharFieldSize)
		{
			if (FieldExists(connectionString, TableName, FieldName)) return "";

			string result = "";
            TableName = TableName.ToUpper();
            FieldName = FieldName.ToUpper();
            string QueryText = "ALTER TABLE " + TableName + " ADD " + FieldName;
			if (FieldType == DBFieldType.ftUnknown)
			{
				result = "Unhandled FieldType in AddMissingField";
			}
			else
			{
				switch (FieldType)
				{
					case DBFieldType.ftString: QueryText += " VARCHAR(" + VarCharFieldSize.ToString() + ")"; break;
					case DBFieldType.ftSmallInt: QueryText += " SmallInt"; break;
					case DBFieldType.ftInteger: QueryText += " Integer"; break;
					case DBFieldType.ftFloat: QueryText += " Float"; break;
					case DBFieldType.ftCurrency: QueryText += " Double Precision"; break;
					case DBFieldType.ftDate:
					case DBFieldType.ftTime:
					case DBFieldType.ftDateTime: QueryText += " Date"; break;
				}
				if (FieldType == DBFieldType.ftString)
				{
					QueryText += " VARCHAR(" + VarCharFieldSize.ToString() + ")";
				}
			}

			result = DBExecute(connectionString, QueryText);

			/*
			string ConnectionString =
				Utilities.FirebirdUtilities.FirebirdUtilities.BuildConfigConnectionString();
			using (FbConnection conn = new FbConnection(ConnectionString))
			{
				conn.Open();

				using (FbTransaction trans = conn.BeginTransaction())
				{
					using (FbCommand cmd = new FbCommand())
					{
						cmd.Connection = conn;
						cmd.Transaction = trans;
						string cmdText = QueryText;
						//FbDataReader reader = cmd.ExecuteReader();
						try
						{
							cmd.ExecuteNonQuery();
							result = "";

						}
						catch (Exception ex)
						{
							result = ex.Message;
						}
					} // FbCommand
					trans.Commit();
				} // FbTransaction			
				conn.Close();
			} // FbConnection
			*/

			return result;
		}

		public static string AddMissingField(string connectionString,
										   string TableName,
										   string FieldName,
										   string FieldType,
										   int VarCharFieldSize)
		{
			FieldType = FieldType.ToUpper();
			DBFieldType fieldType = GetDBFieldTypeFromString(FieldType);

			return AddMissingField(connectionString, TableName, FieldName, fieldType, VarCharFieldSize);
		}

		public static DBFieldType GetDBFieldTypeFromString(string FieldType)
		{
			DBFieldType result = DBFieldType.ftUnknown;
			
				 if (StringUtilities.SameText(FieldType, "ftString"))   result = DBFieldType.ftString;
			else if (StringUtilities.SameText(FieldType, "ftSmallInt")) result = DBFieldType.ftSmallInt;
			else if (StringUtilities.SameText(FieldType, "ftInteger"))  result = DBFieldType.ftInteger;
			else if (StringUtilities.SameText(FieldType, "ftFloat"))    result = DBFieldType.ftFloat;
			else if (StringUtilities.SameText(FieldType, "ftCurrency")) result = DBFieldType.ftCurrency;
			else if (StringUtilities.SameText(FieldType, "ftDate"))     result = DBFieldType.ftDate;
			else if (StringUtilities.SameText(FieldType, "ftTime"))     result = DBFieldType.ftTime;
			else if (StringUtilities.SameText(FieldType, "ftDateTime")) result = DBFieldType.ftDateTime;
		
			return result;
		}

		public static string BuildConfigConnectionString()
		{
			return "";
			
/*			return
				BuildConnectionString(
					WebConfigurationManager.AppSettings["UserId"],
					WebConfigurationManager.AppSettings["Password"],
					WebConfigurationManager.AppSettings["DBFileName"],
					WebConfigurationManager.AppSettings["DataSource"]);
*/					
		}
		
		public static string BuildConnectionString(string UserId,
												   string Password,
												   string DBFileName,
												   string DataSource)
		{		
			FbConnectionStringBuilder cs = new FbConnectionStringBuilder();
			cs.UserID = UserId;
			cs.Password = Password;
			cs.Database = DBFileName;
			cs.DataSource = DataSource;
			
			return cs.ToString();
		}
		
		public static string BuildConnectionString(string UserId,
												   string Password,
												   string DBFileName,
												   string DataSource,
												   bool Pooling)
		{		
			FbConnectionStringBuilder cs = new FbConnectionStringBuilder();
			cs.UserID = UserId;
			cs.Password = Password;
			cs.Database = DBFileName;
			cs.DataSource = DataSource;
			cs.Pooling = Pooling;
			
			return cs.ToString();
		}		

        public static DataTable GetFirstDataTable(string connectionString, string QueryText)
        {
            return GetFirstDataTable(connectionString, QueryText, null, 0);
        }

        public static DataTable GetFirstDataTable(string connectionString, string QueryText, List<FbParameterItem> Parameters, int timeoutSeconds)
        {
            DataTable result = null;
            DataSet ds = GetDataSet(connectionString, QueryText, Parameters, timeoutSeconds);
            if (ds != null) 
            {
                if (ds.Tables.Count > 0)
                {
                    result = ds.Tables[0];
                }
            }

            return result;
        }

        public static DataSet GetDataSet(string connectionString, string QueryText)
        {
            return GetDataSet(connectionString, QueryText, null, 0);
        }

        public static DataSet GetDataSet(string connectionString, string QueryText, List<FbParameterItem> Parameters, int timeoutSeconds)
        {
            System.Data.DataSet result = new DataSet();

            /*
            string ConnectionString =
                Utilities.FirebirdUtilities.FirebirdUtilities.BuildConfigConnectionString();
            */
            using (FbConnection conn = new FbConnection(connectionString))
            {
                conn.Open();
                try
                {
                	using (FbTransaction trans = conn.BeginTransaction())
                	{
                		using (FbCommand cmd = new FbCommand(QueryText, conn, trans))
                		{
                			cmd.CommandTimeout = timeoutSeconds;
                			if (Parameters != null)
                			{
                				for (int Index = 0; Index < Parameters.Count; Index++)
                				{
                					cmd.Parameters.AddWithValue(Parameters[Index].ParameterName,
                					                            Parameters[Index].ParameterValue);
                				}
                			}
                			using (FbDataAdapter da = new FbDataAdapter(cmd))
                			{
                				da.Fill(result);
                			}
                		}
                	}
                }
                finally
                {
                	conn.Close();
                }
            }

            return result;
        }

        /*
        public static FbDataReader GetDataReader(string QueryText,
                                                 List<FbParameterItem> Parameters)
        {
            FbDataReader result = null;
            string ConnectionString =
                Utilities.FirebirdUtilities.FirebirdUtilities.BuildConfigConnectionString();
            using (FbConnection conn = new FbConnection(ConnectionString))
            {
                conn.Open();

                using (FbTransaction trans = conn.BeginTransaction())
                {
                    using (FbCommand cmd = new FbCommand())
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = trans;
                        string cmdText = QueryText;
                        cmd.CommandText = cmdText;
                        if (Parameters != null)
                        {
                            for (int Index = 0; Index < Parameters.Count; Index++)
                            {
                                cmd.Parameters.AddWithValue(Parameters[Index].ParameterName,
                                                            Parameters[Index].ParameterValue);
                            }
                        }
                        result = cmd.ExecuteReader();
                    } // FbCommand
                    trans.Commit();
                } // FbTransaction			
                conn.Close();
            } // FbConnection

            return result;
        }

		public static FbDataReader GetDataReader(string QueryText)
		{
            return GetDataReader(QueryText, null);
		}
        */

		public static string DBExecute(string connectionString, string QueryText, List<FbParameterItem> Parameters, int timeoutSeconds)
		{
			string result = "";

			using (FbConnection conn = new FbConnection(connectionString))
			{
				conn.Open();
				try 
				{
					using (FbTransaction trans = conn.BeginTransaction())
					{
						using (FbCommand cmd = new FbCommand())
						{
							cmd.CommandTimeout = timeoutSeconds;
							cmd.Connection = conn;
							cmd.Transaction = trans;
							string cmdText = QueryText;
							cmd.CommandText = cmdText;
							if (Parameters != null)
							{
								for (int Index = 0; Index < Parameters.Count; Index++)
								{
									cmd.Parameters.AddWithValue(Parameters[Index].ParameterName,
									                            Parameters[Index].ParameterValue);
								}
							}
							try
							{
								cmd.ExecuteNonQuery();
								result = "";
								trans.Commit();

							}
							catch (Exception ex)
							{
								trans.Rollback();
								//result = ex.ToString();
								result = PrepareExceptionError("Error executing query", ex, "", QueryText);
							}
						} // FbCommand
					} // FbTransaction
				} 
				finally 
				{
					conn.Close();
				}
			} // FbConnection

			return result;
		}

		public static string DBExecute(string connectionString, string QueryText)
		{
			return DBExecute(connectionString, QueryText, null, 0);
		}

        public static string AddTableFieldErrorMsg(string TableName, string FieldName)
        {
            return 
                "Error adding the " + FieldName + 
                " field to the " + TableName + " table.";
        }

        public static void AddMissingField(string connectionString,
        								   string TableName,
                                           string FieldName,
                                           DBFieldType FieldType,
                                           StringBuilder Errors)
        {
            AddMissingField(connectionString, TableName, FieldName, FieldType, 0, Errors);
        }

        public static void AddMissingField(string connectionString,
        	 							   string TableName,
                                           string FieldName,
                                           DBFieldType FieldType,
                                           int VarCharFieldSize,
                                           StringBuilder Errors)
        {
            string Str = AddMissingField(connectionString, TableName, FieldName, FieldType, VarCharFieldSize);
            if (Str != "")
            {
                Errors.AppendLine(AddTableFieldErrorMsg(TableName, FieldName));
            }
        }
        
        private static string PrepareExceptionError(string aMainMessage, Exception aException, string aCategory, string aSQLText)
        {
        	string lMessage = aMainMessage;
        	if (!string.IsNullOrEmpty(aCategory))
        	{
        		lMessage = string.Concat(new string[]{lMessage, Environment.NewLine, "  (Category: ", aCategory, ")"});
        	}
        	if (aException != null)
        	{
        		lMessage = string.Concat(new string[]{lMessage, Environment.NewLine, Environment.NewLine, "  ", aException.Message, Environment.NewLine, Environment.NewLine});
        	}
        	if (!string.IsNullOrEmpty(aSQLText))
        	{
        		lMessage = string.Concat(new string[]{lMessage, Environment.NewLine, Environment.NewLine, "Query:", Environment.NewLine, "  ", aSQLText});
        	}
        	
        	return lMessage;
        }
        
        public static bool TestDBConnection(string connectionString, out string aErrorMessage)
        {
        	aErrorMessage = "";
        	bool lResult = false;
        	try
        	{
        		using (FbConnection lConnection = new FbConnection(connectionString))
        		{
        			lConnection.Open();
        			try
        			{
        				lResult = true;
        			}
        			finally
        			{
        				lConnection.Close();
        			}
        		}
        	}
        	catch (Exception ex)
        	{
        		aErrorMessage = "Unable to connect to database: " + Environment.NewLine + ex.Message;
        	}
        	return lResult;
        }
        
		public static DataSet GetDataSetFromQuery(string connectionString, string queryText, out string errorMessage)
		{
			errorMessage = "";
			
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new Exception("Unable to get data from query." + Environment.NewLine + "No database connection specified");
			}
			
			if ((queryText == null) || (queryText == ""))
			{
				throw new Exception("Unable to perform command." + Environment.NewLine + "No database query specified");
			}
			
			try 
			{
				return FirebirdUtilities.GetDataSet(connectionString, queryText);
			} 
			catch 
			{
				throw;
			}
		}
		
		public static object GetFieldValueFromFirstRecordOfQuery(string connectionString, string queryText, string aFieldName)
		{			
			object result = null;
			
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new Exception("Unable to get value from db query." + Environment.NewLine + "No database connection specified");
			}
			
			if (string.IsNullOrEmpty(queryText))
			{
				throw new Exception("Unable to get value from db query." + Environment.NewLine + "No database query specified");
			}
			
			if (string.IsNullOrEmpty(aFieldName))
			{
				throw new Exception("Unable to get value from db query." + Environment.NewLine + "No table field specified");
			}
			
			try 
			{
				using (DataSet queryData = FirebirdUtilities.GetDataSet(connectionString, queryText))
				{
					if (queryData.Tables[0].Rows.Count > 0)
					{
						DataRow row = queryData.Tables[0].Rows[0];
						
						result = row[aFieldName];
					}
				}
					
				return result;
			} 
			catch 
			{
				throw;
			}
		}		
		
		public static string GetFirebirdYNResponseFromBool(bool value)
		{
			if (value)
				return "Y";
			else
				return "N";				
		}        
        
        public static string GetDBTableValueAsString(DataSet aDataSet, int aTableInd, int aRowInd, string aFieldName)
        {
        	string lResult = "";
        		
			object lValue = aDataSet.Tables[aTableInd].Rows[aRowInd][aFieldName];
					
			if ((lValue != null) && (lValue != DBNull.Value))
				lResult = (string)lValue;
			
			return lResult;
        }
        
        public static int GetDBTableValueAsInt(DataSet aDataSet, int aTableInd, int aRowInd, string aFieldName)
        {
        	int lResult = 0;
        		
			object lValue = aDataSet.Tables[aTableInd].Rows[aRowInd][aFieldName];
					
			if ((lValue != null) && (lValue != DBNull.Value))
				lResult = (int)lValue;
			
			return lResult;
        }
        
        public static Int64 GetDBTableValueAsInt64(DataSet aDataSet, int aTableInd, int aRowInd, string aFieldName)
        {
        	Int64 lResult = 0;
        		
			object lValue = aDataSet.Tables[aTableInd].Rows[aRowInd][aFieldName];
					
			if ((lValue != null) && (lValue != DBNull.Value))
				lResult = (Int64)lValue;
			
			return lResult;
        }
        
        public static string YYYYMMDDToDbDateString(string Source,
                                                    string Separator)
        {
			string Result = "";
			
			if (Source.Length == 8)
			{
				Result = 
					Source.Substring(4, 2) + Separator +
					Source.Substring(6, 2) + Separator +
					Source.Substring(0, 4);
			}
			
			return Result;
		}
		
		public static string YYYYMMDDToDbDateString(string Source)
		{
			return YYYYMMDDToDbDateString(Source, "/");
		}
		
		public static string DateTimeToDbDateString(DateTime Source,
		                                            string Separator)
		{
			return Source.ToString("MM" + Separator + "dd" + Separator + "yyyy");		
		}
		
		public static string DateTimeToDbDateString(DateTime Source)
		{
			return DateTimeToDbDateString(Source, "/");
		}		
	}		
}