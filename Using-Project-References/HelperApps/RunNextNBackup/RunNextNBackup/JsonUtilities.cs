using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RunNextNBackup;

public class JsonUtilities
{
	public static string MissingTag(string TagName)
	{
		return WriteJSONResult("Missing " + TagName + " Tag");
	}

	public static string BuildInvalidRequestType()
	{
		return WriteJSONResult("Invalid Request Type");
	}

	public static string MethodNotImplemented()
	{
		return WriteJSONResult("Method not implemented.");
	}

	public static string WriteJSONResult(string aValue)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder);
		JsonTextWriter val = new JsonTextWriter((TextWriter)(object)stringWriter);
		try
		{
			((JsonWriter)val).Formatting = (Formatting)1;
			((JsonWriter)val).WriteStartObject();
			((JsonWriter)val).WritePropertyName("Result");
			((JsonWriter)val).WriteValue(aValue);
			((JsonWriter)val).WriteEndObject();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return stringBuilder.ToString();
	}

	public static string WriteJSONResultWithEmbeddedObject(string aValue)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder);
		JsonTextWriter val = new JsonTextWriter((TextWriter)(object)stringWriter);
		try
		{
			((JsonWriter)val).Formatting = (Formatting)1;
			((JsonWriter)val).WriteStartObject();
			((JsonWriter)val).WritePropertyName("Result");
			((JsonWriter)val).WriteStartObject();
			((JsonWriter)val).WriteEndObject();
			((JsonWriter)val).WriteEndObject();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		string jSONString = stringBuilder.ToString();
		JObject jSONObject = GetJSONObject(jSONString);
		JObject jSONObject2 = GetJSONObject(aValue);
		if (jSONObject2 != null)
		{
			JToken obj = jSONObject["Result"];
			JObject val2 = (JObject)(object)((obj is JObject) ? obj : null);
			if (val2 != null)
			{
				jSONObject["Result"] = (JToken)(object)jSONObject2;
			}
		}
		return ((object)jSONObject).ToString();
	}

	public static JObject GetJSONObject(string JSONString)
	{
		return JsonConvert.DeserializeObject<JObject>(JSONString);
	}

	public static void AddPropertyNameAndValue(JsonTextWriter Writer, string PropertyName, string Value)
	{
		((JsonWriter)Writer).WritePropertyName(PropertyName);
		((JsonWriter)Writer).WriteValue(CleanForJSON(Value));
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
		return AddPropertyNameAndValue(PropertyName, PropertyValue, AppendComma: true);
	}

	public static string AddPropertyNameAndValue(string PropertyName, string PropertyValue, bool AppendComma)
	{
		string text = "\"" + CleanForJSON(PropertyName) + "\": \"" + CleanForJSON(PropertyValue) + "\"";
		if (AppendComma)
		{
			text += ",";
		}
		return text;
	}

	public static string AddPropertyNameAndValue(string PropertyName, object PropertyValue)
	{
		return AddPropertyNameAndValue(PropertyName, PropertyValue, AppendComma: true);
	}

	public static string AddPropertyNameAndValue(string PropertyName, object PropertyValue, bool AppendComma)
	{
		string text = CleanForJSON(Convert.ToString(PropertyValue));
		string text2 = "\"" + CleanForJSON(PropertyName) + "\": " + text;
		if (AppendComma)
		{
			text2 += ",";
		}
		return text2;
	}

	public static string StartJsonObject(string PropertyName)
	{
		string text = "";
		if (!string.IsNullOrEmpty(PropertyName))
		{
			text = text + BuildPropertyName(PropertyName) + ":";
		}
		return text + StartJson();
	}

	public static string EndJsonObject()
	{
		return "}";
	}

	public static string StartJsonArray(string ArrayName)
	{
		string text = "";
		if (!string.IsNullOrEmpty(ArrayName))
		{
			text = text + BuildPropertyName(ArrayName) + ":";
		}
		return text + "[";
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
			result = "\"" + CleanForJSON(PropertyName) + "\"";
		}
		return result;
	}

	public static bool PopulateListFromArray(JObject DataObject, string ArrayName, ref List<string> StrList)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		JArray val = (JArray)DataObject[ArrayName];
		bool flag = val != null;
		if (flag)
		{
			for (int i = 0; i < ((JContainer)val).Count; i = checked(i + 1))
			{
				StrList.Add((string)val[i]);
			}
		}
		return flag;
	}

	public static string EscapeJSONString_JsonConvert(string aValue)
	{
		string text = "";
		text = JsonConvert.ToString(aValue);
		return text.Substring(1, checked(text.Length - 2));
	}

	public static string CleanForJSON(string s)
	{
		checked
		{
			if (!string.IsNullOrEmpty(s))
			{
				char c = '\0';
				int length = s.Length;
				StringBuilder stringBuilder = new StringBuilder(length + 4);
				for (int i = 0; i < length; i++)
				{
					c = s[i];
					switch (c)
					{
					case '"':
					case '\\':
						stringBuilder.Append('\\');
						stringBuilder.Append(c);
						continue;
					case '/':
						stringBuilder.Append('\\');
						stringBuilder.Append(c);
						continue;
					case '\b':
						stringBuilder.Append("\\b");
						continue;
					case '\t':
						stringBuilder.Append("\\t");
						continue;
					case '\n':
						stringBuilder.Append("\\n");
						continue;
					case '\f':
						stringBuilder.Append("\\f");
						continue;
					case '\r':
						stringBuilder.Append("\\r");
						continue;
					}
					if (c < ' ')
					{
						string text = "000" + string.Format("X", c);
						stringBuilder.Append("\\u" + text.Substring(text.Length - 4));
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				return stringBuilder.ToString();
			}
			return "";
		}
	}

	public static string BuildJsonArrayFromDataTable(DataTable tblSource, string ArrayName, ref StringBuilder Entries)
	{
		string text = "";
		if (tblSource == null)
		{
			text = "Source Table is NULL";
		}
		if (string.IsNullOrEmpty(text))
		{
			string text2 = "";
			string text3 = "";
			string text4 = "";
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			object obj = null;
			try
			{
				Entries.AppendLine(StartJsonArray(ArrayName));
				foreach (DataRow row in tblSource.Rows)
				{
					list2.Clear();
					foreach (DataColumn column in tblSource.Columns)
					{
						text3 = column.ColumnName;
						obj = row[text3];
						text4 = ((obj == DBNull.Value) ? "" : Convert.ToString(obj));
						list2.Add(AddPropertyNameAndValue(text3, text4, AppendComma: false));
					}
					text2 = StartJsonObject("") + string.Join(",", list2.ToArray()) + EndJsonObject();
					list.Add(text2);
				}
				Entries.Append(string.Join(",", list.ToArray()));
				Entries.AppendLine(EndJsonArray());
			}
			catch (Exception ex)
			{
				text = ex.Message + Environment.NewLine + ex.InnerException.ToString() + Environment.NewLine + ex.StackTrace;
			}
			return text;
		}
		return text;
	}

	public static JToken GetJsonPathValue(JObject Source, string Path, out string ErrorMsg)
	{
		ErrorMsg = "";
		JToken result = null;
		try
		{
			result = ((JToken)Source).SelectToken(Path);
		}
		catch (Exception ex)
		{
			ErrorMsg = ex.Message + Environment.NewLine + ex.InnerException.ToString();
		}
		return result;
	}

	public static JToken GetJsonPathValue(string Source, string Path, out string ErrorMsg)
	{
		ErrorMsg = "";
		Source = Source.Trim();
		if (!Source.StartsWith("{"))
		{
			Source = StartJson() + Source + EndJson();
		}
		JObject source = JObject.Parse(Source);
		return GetJsonPathValue(source, Path, out ErrorMsg);
	}

	public static string GetJsonPathValueStr(JObject Source, string Path, out string ErrorMsg)
	{
		ErrorMsg = "";
		string result = "";
		JToken jsonPathValue = GetJsonPathValue(Source, Path, out ErrorMsg);
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			result = (string)jsonPathValue;
		}
		return result;
	}

	public static string GetJsonPathValueStr(string Source, string Path, out string ErrorMsg)
	{
		ErrorMsg = "";
		string result = "";
		JToken jsonPathValue = GetJsonPathValue(Source, Path, out ErrorMsg);
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			result = (string)jsonPathValue;
		}
		return result;
	}

	public static JObject ParseJSONSource(string Source, out string ErrorMsg)
	{
		ErrorMsg = "";
		JObject val = null;
		Source = Source.Trim();
		if (!Source.StartsWith("{"))
		{
			Source = StartJson() + Source + EndJson();
		}
		try
		{
			val = JObject.Parse(Source);
		}
		catch (Exception ex)
		{
			val = null;
			ErrorMsg = "Error parsing the JSON Source." + Environment.NewLine + ex.Message;
		}
		return val;
	}
}
