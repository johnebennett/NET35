using System;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;

namespace RunNextNBackup
{
    public class JsonUtilities
    {
        public JsonUtilities()
        {
        }
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
            string jSONString = stringBuilder.ToString();
            JObject jSONObject = GetJSONObject(jSONString);
            JObject jSONObject2 = GetJSONObject(aValue);
            if (jSONObject2 != null)
            {
                JObject jObject = jSONObject["Result"] as JObject;
                if (jObject != null)
                {
                    jSONObject["Result"] = jSONObject2;
                }
            }
            return jSONObject.ToString();
        }

        public static JObject GetJSONObject(string JSONString)
        {
            return JsonConvert.DeserializeObject<JObject>(JSONString);
        }

        public static void AddPropertyNameAndValue(JsonTextWriter Writer, string PropertyName, string Value)
        {
            Writer.WritePropertyName(PropertyName);
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
            string text = "\"" + CleanForJSON(PropertyName) + "\": \"" + CleanForJSON(PropertyValue) + "\"";
            if (AppendComma)
            {
                text += ",";
            }
            return text;
        }

        public static string AddPropertyNameAndValue(string PropertyName, object PropertyValue)
        {
            return AddPropertyNameAndValue(PropertyName, PropertyValue, true);
        }

        public static string AddPropertyNameAndValue(string PropertyName, object PropertyValue, bool AppendComma)
        {
            string str = CleanForJSON(Convert.ToString(PropertyValue));
            string text = "\"" + CleanForJSON(PropertyName) + "\": " + str;
            if (AppendComma)
            {
                text += ",";
            }
            return text;
        }

        public static string StartJsonObject(string PropertyName)
        {
            string str = "";
            if (!string.IsNullOrEmpty(PropertyName))
            {
                str = str + BuildPropertyName(PropertyName) + ":";
            }
            return str + StartJson();
        }

        public static string EndJsonObject()
        {
            return "}";
        }

        public static string StartJsonArray(string ArrayName)
        {
            string str = "";
            if (!string.IsNullOrEmpty(ArrayName))
            {
                str = str + BuildPropertyName(ArrayName) + ":";
            }
            return str + "[";
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
            JArray jArray = (JArray)DataObject[ArrayName];
            bool flag = jArray != null;
            if (flag)
            {
                for (int i = 0; i < jArray.Count; i = checked(i + 1))
                {
                    StrList.Add((string)jArray[i]);
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
                                break;
                            case '/':
                                stringBuilder.Append('\\');
                                stringBuilder.Append(c);
                                break;
                            case '\b':
                                stringBuilder.Append("\\b");
                                break;
                            case '\t':
                                stringBuilder.Append("\\t");
                                break;
                            case '\n':
                                stringBuilder.Append("\\n");
                                break;
                            case '\f':
                                stringBuilder.Append("\\f");
                                break;
                            case '\r':
                                stringBuilder.Append("\\r");
                                break;
                            default:
                                if (c < ' ')
                                {
                                    string text = "000" + string.Format("X", unchecked((object)c));
                                    stringBuilder.Append("\\u" + text.Substring(text.Length - 4));
                                }
                                else
                                {
                                    stringBuilder.Append(c);
                                }
                                break;
                        }
                    }
                    return stringBuilder.ToString();
                }
                return "";
            }
        }

        public static string BuildJsonArrayFromDataTable(DataTable tblSource, string ArrayName, ref StringBuilder Entries)
        {
            //IL_00bf: Unknown result type (might be due to invalid IL or missing references)
            //IL_00c4: Invalid comparison between O and Unknown
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
                            text4 = ((obj == (object)DBNull.Value) ? "" : Convert.ToString(obj));
                            list2.Add(AddPropertyNameAndValue(text3, text4, false));
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
                result = Source.SelectToken(Path);
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
            JObject jObject = null;
            Source = Source.Trim();
            if (!Source.StartsWith("{"))
            {
                Source = StartJson() + Source + EndJson();
            }
            try
            {
                jObject = JObject.Parse(Source);
            }
            catch (Exception ex)
            {
                jObject = null;
                ErrorMsg = "Error parsing the JSON Source." + Environment.NewLine + ex.Message;
            }
            return jObject;
        }
    }
}
