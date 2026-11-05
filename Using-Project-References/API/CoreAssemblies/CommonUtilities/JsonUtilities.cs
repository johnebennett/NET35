/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 18/04/2017
 * Time: 12:25 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace CommonUtilities
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class JsonUtilities
	{
		public static string MissingTag(string TagName)
		{
			return JsonUtilities.WriteJSONResult("Missing " + TagName + " Tag");
		}
		
		public static string BuildInvalidRequestType()
		{
			return JsonUtilities.WriteJSONResult("Invalid Request Type");
		}

		public static string MethodNotImplemented()
		{
			return JsonUtilities.WriteJSONResult("Method not implemented.");
		}

		public static string WriteJSONResult(string aValue)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter textWriter = new StringWriter(stringBuilder);
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(textWriter))
			{
				jsonTextWriter.Formatting = Formatting.Indented;
				jsonTextWriter.WriteStartObject();
				jsonTextWriter.WritePropertyName("Result");
				jsonTextWriter.WriteValue(aValue);
				jsonTextWriter.WriteEndObject();
			}
			return stringBuilder.ToString();
		}

		public static string WriteJSONResultWithEmbeddedObject(string aValue)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter textWriter = new StringWriter(stringBuilder);
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(textWriter))
			{
				jsonTextWriter.Formatting = Formatting.Indented;
				jsonTextWriter.WriteStartObject();
					jsonTextWriter.WritePropertyName("Result");
					jsonTextWriter.WriteStartObject();
					jsonTextWriter.WriteEndObject();
				jsonTextWriter.WriteEndObject();
			}
			string lResultJsonStr = stringBuilder.ToString();
			
			JObject lResultJsonObj = GetJSONObject(lResultJsonStr);
			
			JObject lOverrideObj = GetJSONObject(aValue);
			
			//Setup object as Result object
			if (lOverrideObj != null)
			{
				JObject lResultSectionObj = lResultJsonObj["Result"] as JObject;
				if (lResultSectionObj != null)
				{
					lResultJsonObj["Result"] = lOverrideObj;  // Set the Result object as the Data Object
				}
			}				
			
			return lResultJsonObj.ToString();
		}

		
		public static JObject GetJSONObject(string JSONString)
		{
			return JsonConvert.DeserializeObject<JObject>(JSONString);	
		}		
		
		public static void AddPropertyNameAndValue(JsonTextWriter Writer,
		                                           string PropertyName,
		                                           string Value)
		{
			Writer.WritePropertyName(PropertyName);
			//Writer.WriteValue(Value);
			Writer.WriteValue(CleanForJSON(Value));
		}			

		public static string StartJson()
		{
			return "{";
		}
		
		public static string EndJson()
		{
			return EndJsonObject();
		}
		
		public static string AddPropertyNameAndValue(string PropertyName, string PropertyValue)
		{
			return AddPropertyNameAndValue(PropertyName, PropertyValue, true);
		}
		
		public static string AddPropertyNameAndValue(string PropertyName, string PropertyValue, bool AppendComma)
		{
			string result = 
				//"\"" + PropertyName + "\": " +
				//"\"" + PropertyValue + "\"";
				"\"" + CleanForJSON(PropertyName) + "\": " +
				"\"" + CleanForJSON(PropertyValue) + "\"";
			
			if (AppendComma) result += ",";
			
			return result;
		}
		
		public static string AddPropertyNameAndValue(string PropertyName, object PropertyValue)
		{
			return AddPropertyNameAndValue(PropertyName, PropertyValue, true);
		}
		
		public static string AddPropertyNameAndValue(string PropertyName, object PropertyValue, bool AppendComma)
		{
			string StrValue = CleanForJSON(Convert.ToString(PropertyValue));
			
			//string result = "\"" + PropertyName + "\": " + StrValue;
			string result = "\"" + CleanForJSON(PropertyName) + "\": " + StrValue;
			
			if (AppendComma) result += ",";
			
			return result;
		}
		
		public static string StartJsonObject(string PropertyName)
		{
			string result = "";
			
			if (!string.IsNullOrEmpty(PropertyName)) result += BuildPropertyName(PropertyName) + ":";
			
			result += StartJson();
			
			return result;
		}
		
		public static string EndJsonObject()
		{
			return "}";
		}
		
		public static string StartJsonArray(string ArrayName)
		{
			string result = "";
			
			if (!string.IsNullOrEmpty(ArrayName)) 
				result += BuildPropertyName(ArrayName) + ":";
			
			result += "[";
			
			return result;
		}
		
		public static string EndJsonArray()
		{
			return "]";
		}
		
		private static string BuildPropertyName(string PropertyName)
		{
			string result = "";
			
			if (!string.IsNullOrEmpty(PropertyName))
			{
				//result = "\"" + PropertyName + "\"";
				result = "\"" + CleanForJSON(PropertyName) + "\"";
			}
			
			return result;
		}
		
		public static bool PopulateListFromArray(JObject DataObject, string ArrayName, ref List<string> StrList)
		{
			JArray array = (JArray)DataObject[ArrayName];
			bool result = (array != null);
			
			if (result)
			{
				for (int Index = 0; Index < array.Count; Index++)
				{
					StrList.Add((string)array[Index]);
				}
			}
			
			return result;
		}		
		
		public static string EscapeJSONString_JsonConvert(string aValue)
		{
			string lResult = "";
			
			lResult = JsonConvert.ToString(aValue);				// convert value to json string (also escaped)
			lResult = lResult.Substring(1, lResult.Length - 2);	// remove extra double quotes surrounding the string
			
			return lResult;
		}

		public static string CleanForJSON(string s)
		{
			//if (s == null || s.Length == 0) { return ""; }
			if (string.IsNullOrEmpty(s)) return "";
			
			char         c = '\0';
		    int          i;
		    int          len = s.Length;
		    StringBuilder sb = new StringBuilder(len + 4);
		    String       t;
		
		    for (i = 0; i < len; i += 1) 
		    {
		    	c = s[i];
		    	switch (c) 
		    	{
		    		case '\\':
		            case '"':
		    			sb.Append('\\');
		                sb.Append(c);
		                break;
		            case '/':
		            	sb.Append('\\');
		                sb.Append(c);
		                break;
		            case '\b':
		            	sb.Append("\\b");
		                break;
		            case '\t':
		            	sb.Append("\\t");
		                break;
		            case '\n':
		                sb.Append("\\n");
		                break;
		            case '\f':
		            	sb.Append("\\f");
		                break;
		            case '\r':
		                sb.Append("\\r");
		                break;
		            default:
		                if (c < ' ') 
		                {
		                	t = "000" + String.Format("X", c);
		                	sb.Append("\\u" + t.Substring(t.Length - 4));
		                }
		                else
		                {
		                	sb.Append(c);
		                }
		                break;
		             
		        }
		    }
		    return sb.ToString();
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
						ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
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
		
		/// <summary>
		/// For a given JObject, find the JToken object with the
		/// given Json Path.
		/// </summary>
		/// <param name="Source">A JObject object representing some valid JSON</param>
		/// <param name="Path">The Path to search inside the Source</param>
		/// <param name="ErrorMsg">If there is a error or exception processing
		/// the request, it is returned in this parameter</param>
		/// <returns>A JToken object representing the Path.
		/// This object could be a single value or an object representing, for example,
		/// an array item.
		/// </returns>
		public static JToken GetJsonPathValue(JObject Source, string Path, out string ErrorMsg)
		{
			//string Method = "GetJsonPathValueStr Source=JObject 2";
			//WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			ErrorMsg = "";
			JToken Result = null;
			try
			{
				Result = Source.SelectToken(Path);
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message + Environment.NewLine + Ex.InnerException;

			}
			return Result;
		}
		
		/// <summary>
		/// For a given string that can be converted to a JObject, 
		/// find the JToken object with the given Json Path.
		/// </summary>
		/// <param name="Source">
		/// A string, either with or without leading braces
		/// that represents some JSON the can be converted to a JObject object
		/// </param>
		/// <param name="Path">The Path to search inside the Source</param>
		/// <param name="ErrorMsg">If there is a error or exception processing
		/// the request, it is returned in this parameter</param>
		/// <returns>A JToken object representing the Path.
		/// This object could be a single value or an object representing, for example,
		/// an array item.
		/// </returns>
		public static JToken GetJsonPathValue(string Source, string Path, out string ErrorMsg)
		{
			//string Method = "GetJsonPathValueStr Source=string 2";
			//WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			ErrorMsg = "";
			Source = Source.Trim();
			//WindowsEventLoggingUtilities.WriteToInfoEvent("Source Before" + Source, Method, 0);
			if (!Source.StartsWith("{")) Source = StartJson() + Source + EndJson();
			//WindowsEventLoggingUtilities.WriteToInfoEvent("Source After" + Source, Method, 0);
			JObject SourceObj = JObject.Parse(Source);
			
			return GetJsonPathValue(SourceObj, Path, out ErrorMsg);
		}
		
		/// <summary>
		/// For a given JObject, find the string value with the
		/// given Json Path.
		/// </summary>
		/// <param name="Source">A JObject object representing some valid JSON</param>
		/// <param name="Path">The Path to search inside the Source</param>
		/// <param name="ErrorMsg">If there is a error or exception processing
		/// the request, it is returned in this parameter</param>
		/// <returns>A JToken object representing the Path.
		/// This object could be a single value or an object representing, for example,
		/// an array item.
		/// </returns>
		public static string GetJsonPathValueStr(JObject Source, string Path, out string ErrorMsg)
		{
			//string Method = "GetJsonPathValueStr Source=JObject";
			//WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			ErrorMsg = "";
			string Result = "";
			JToken token = GetJsonPathValue(Source, Path, out ErrorMsg);
			if (string.IsNullOrEmpty(ErrorMsg)) Result = (string)token;
			return Result;
		}
		
		/// <summary>
		/// For a given string that can be converted to a JObject, 
		/// find the string value for the given Json Path.
		/// </summary>
		/// <param name="Source">
		/// A string, either with or without leading braces
		/// that represents some JSON the can be converted to a JObject object
		/// </param>
		/// <param name="Path">The Path to search inside the Source</param>
		/// <param name="ErrorMsg">If there is a error or exception processing
		/// the request, it is returned in this parameter</param>
		/// <returns>A JToken object representing the Path.
		/// This object could be a single value or an object representing, for example,
		/// an array item.
		/// </returns>
		public static string GetJsonPathValueStr(string Source, string Path, out string ErrorMsg)
		{
			//string Method = "GetJsonPathValueStr Source=string";
			//WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			ErrorMsg = "";
			string Result = "";
			JToken token = GetJsonPathValue(Source, Path, out ErrorMsg);
			if (string.IsNullOrEmpty(ErrorMsg)) Result = (string)token;
			return Result;
		}

        public static JObject ParseJSONSource(string Source, out string ErrorMsg)
        {
            ErrorMsg = "";
            JObject result = null;
            Source = Source.Trim();
            bool flag = !Source.StartsWith("{");
            if (flag)
            {
                Source = JsonUtilities.StartJson() + Source + JsonUtilities.EndJson();
            }
            try
            {
                result = JObject.Parse(Source);
            }
            catch (Exception ex)
            {
                result = null;
                ErrorMsg = "Error parsing the JSON Source." + Environment.NewLine + ex.Message;
            }
            return result;
        }
    }
}