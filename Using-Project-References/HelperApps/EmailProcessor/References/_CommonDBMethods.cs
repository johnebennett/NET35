using System;
using CommonUtilities;
using EAOfficeAPI;
using System.Data;
using System.Text;

namespace CommonDBMethods
{
	public class Common
	{
		public static string ConnectionString()
		{
			string ErrorMsg = "";
			APISettingsItem Item = APISettingsFile.GetAPISettings(APIAppType.Service, out ErrorMsg);
			
			//return CommonUtilities.FirebirdUtilities.BuildConnectionString("SYSDBA", "!icraft!", Item.DBName, Item.DBServer, true);
			string connectionString = CommonUtilities.FirebirdUtilities.BuildConnectionString("EAOFFICEUSER", "!bcdeaoffice!", Item.DBName, Item.DBServer, true);
			connectionString = connectionString.Replace("user id", "User");
			connectionString += ";Role=rdb$admin;";
			return connectionString;
		}
		
		public static string ConnectionString(APISettingsItem Source)
		{
			//return CommonUtilities.FirebirdUtilities.BuildConnectionString("SYSDBA", "!icraft!", Source.DBName, Source.DBServer, true);
			string connectionString = CommonUtilities.FirebirdUtilities.BuildConnectionString("EAOFFICEUSER", "!bcdeaoffice!", Source.DBName, Source.DBServer, true);
			connectionString = connectionString.Replace("user id", "User");
			connectionString += ";Role=rdb$admin;";
			return connectionString;
		}
		
		public static string CommonComboEntries(int CompanyId,
		                                        int ClientId,
		                                        int ComboType,
		                                        out string ErrorMsg)
		{
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside EAOfficeAPI.CommonDBMethods.Common.CommonComboEntries()", "EAOfficeAPI", 0);
			string Result = "";
			APISettingsItem Item = APISettingsFile.GetAPISettings(APIAppType.Service, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Error gathering the API Settings", "EAOfficeAPI", 0);
				return ErrorMsg;
			}
			
			StringBuilder sb = new StringBuilder();
			string connectionString = 
				CommonUtilities.FirebirdUtilities.BuildConnectionString("EAOFFICEUSER", "!bcdeaoffice!", Item.DBName, Item.DBServer, true);
			if (!string.IsNullOrEmpty(connectionString))
			{
				connectionString = connectionString.Replace("user id", "User");
				connectionString += ";Role=rdb$admin;";
				WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + connectionString, "EAOfficeAPI", 0);
				//string QueryText = "SELECT * FROM CommonComboEntries(0, 0, 0)";
				string QueryText = "SELECT * FROM CommonComboEntriesEx({0}, {1}, {2})";
				QueryText = string.Format(QueryText, CompanyId, ClientId, ComboType);
				System.Data.DataSet ds = 
					CommonUtilities.FirebirdUtilities.GetDataSetFromQuery(connectionString, QueryText, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						"Error Executing Query" + Environment.NewLine +
						ErrorMsg + Environment.NewLine +
						"QueryText:" + Environment.NewLine +
						QueryText;
					WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, "EAOfficeAPI", 0);
					return ErrorMsg;
				}
				else
				{
					if (ds.Tables.Count == 0)
					{
						ErrorMsg = "No tables in the dataset to process";
						return Result;
					}
					else
					{
						foreach (DataRow row in ds.Tables[0].Rows)
						{
							sb.AppendLine((row[0] == DBNull.Value) ? "" : Convert.ToString(row[0]));
						}
						Result = sb.ToString();
					}
				}
			}
			
			return Result;
		}
	}
}