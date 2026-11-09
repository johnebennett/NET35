/*
 * Created by SharpDevelop.
 * User: John
 * Date: 03/07/2019
 * Time: 12:15 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonUtilities;

namespace MakeItemClasses
{
	public class PropertiesItem
	{
		public string Name { get; set; }
		public string Validate { get; set; }
		public string DataType { get; set; }
	}
	
	public class MethodStubItem
	{
		public string Region { get; set; }
		public string Name { get; set; }
		public List<string> FLines;
		public List<string> Lines
		{
			get { return FLines; }
			set { FLines = value; }
		}
		
		public MethodStubItem()
		{
			Region = "";
			Name = "";
			FLines = new List<string>();
		}
	}
	
	/// <summary>
	/// Description of FileSettingsItem.
	/// </summary>
	public class FileSettingsItem
	{
		private int IndentLevel = 0;
		private List<string> Content = new List<string>();
		private string FSourceFileName = "";
		
		#region Constructor
		public FileSettingsItem()
		{
			FSourceFileName = "";
			FileName = "";
			Namespace = "";
			Author = "";
			OutputFileName = "";
			FProperties = new List<PropertiesItem>();
			FIncludes = new List<string>();
			FMethodStubs = new List<MethodStubItem>();
		}
		#endregion
		
		#region Properties
		public string FileName { get; set; }
		public string Namespace { get; set; }
		public string Author { get; set; }
		public string ClassName { get; set; }
		public string OutputFileName { get; set; }
		
		private List<PropertiesItem> FProperties;
		public List<PropertiesItem> Properties 
		{
			get { return FProperties; }
			set { FProperties = value; }
		}

		private List<string> FIncludes;
		public List<string> Includes 
		{
			get { return FIncludes; }
			set { FIncludes = value; }
		}
		
		private List<MethodStubItem> FMethodStubs;
		public List<MethodStubItem> MethodStubs
		{
			get { return FMethodStubs; }
			set { FMethodStubs = value; }
		}
		#endregion
		
		#region LoadFromFile
		public string LoadFromFile(string SourceFileName)
		{
			FSourceFileName = SourceFileName;
			
			string Result = "";
			try 
			{
				using (StreamReader reader = new StreamReader(SourceFileName))
				{
					Result = Load(reader.ReadToEnd());
				}
			}
			catch (Exception Ex)
			{
				Result = 
					Ex.Message + Environment.NewLine + 
					Ex.InnerException + Environment.NewLine + 
					Ex.InnerException;
			}
			
			return Result;
		}
		#endregion
		
		#region Load from Json String
		public string Load(string JsonSource)
		{
			Content.Clear();			
			FProperties.Clear();
			FIncludes.Clear();
			FMethodStubs.Clear();
			
			string ErrorMsg = "";
			JsonSource = PreProcessJsonSource(JsonSource, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg)) return ErrorMsg;

			JObject jObject = JsonConvert.DeserializeObject<JObject>(JsonSource);
			if (jObject == null) return "Error deserializing the JsonSource";
			
			FileName = (string)jObject["fileName"];
			Namespace = (string)jObject["namespace"];
			Author = (string)jObject["author"];
			ClassName = (string)jObject["className"];
			
			JArray Props = (JArray)jObject["properties"];
			if ((Props != null) && (Props.Count > 0))
			{
				foreach (JObject Item in Props)
				{
					if (Item != null)
					{
						PropertiesItem aProp = new PropertiesItem();
						aProp.Name = (string)Item["name"];
						aProp.Validate = (string)Item["validate"];
						aProp.DataType = (string)Item["dataType"];
						FProperties.Add(aProp);
					}
				}
			}
			
			JArray includes = (JArray)jObject["includes"];
			if ((includes != null) && (includes.Count > 0))
			{
				foreach (string Item in includes)
				{
					FIncludes.Add(Item);
				}
			}
			
			JArray AMethodStubs = (JArray)jObject["methodStubs"];
			if ((AMethodStubs != null) && (AMethodStubs.Count > 0))
			{
				foreach (JObject Stub in AMethodStubs)
				{
					if (Stub != null)
					{
						MethodStubItem Item = new MethodStubItem();
						Item.Region = (string)Stub["region"];
						Item.Name = (string)Stub["name"];
						
						JArray StubContent = (JArray)Stub["content"];
						if ((StubContent != null) && (StubContent.Count > 0))
						{
							foreach (string Line in StubContent)
							{
								Item.Lines.Add(Line);
							}
						}
						
						FMethodStubs.Add(Item);
					}
				}
			}
			
			OutputFileName = Path.GetDirectoryName(FSourceFileName);
			if (!OutputFileName.EndsWith("\\")) OutputFileName += "\\";
			OutputFileName += FileName + ".cs";

			return "";
		}
		#endregion
		
		private string PreProcessJsonSource(string Source, out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = Source;
			
			// Replace the appropriate strings in the source code
			JObject jObject = JsonConvert.DeserializeObject<JObject>(Source);
			if (jObject == null) ErrorMsg = "Error deserializing the JsonSource in PreProcessJsonSource";
			
			if (!string.IsNullOrEmpty(ErrorMsg)) return "";
			
			string Str = (string)jObject["className"];
			Result = Result.Replace("__ClassName__", Str);
			
			return Result;
		}
		
		public List<string> GenerateContent(out string ErrorMsg)
		{
			ErrorMsg = "";
			Content.Clear();
			
			BuildContentHeader();
			BuildUsingSection();
			StartNamespaceSection();
			BuildFileContent();
			EndNamespaceSection();
			
			return Content;
		}
		
		
		private void WriteLine(string LineText)
		{
			Content.Add(Indents(IndentLevel) + LineText);
		}
		
		private void BuildContentHeader()
		{
			WriteLine("/*");
			WriteLine(" * Created by Make Item Classes.");
			WriteLine(" * Author: " + Author);
			WriteLine(" * Date: " + DateTime.Now.ToString("dd-MM-yyyy"));
			WriteLine(" * Time: " + DateTime.Now.ToString("t"));
			WriteLine("");
			WriteLine(" * SourceFile: " + FSourceFileName);
			WriteLine("*/");
		}
		
		private void BuildUsingSection()
		{
			foreach (string Value in Includes)
			{
				WriteLine("using " + Value + ";");
			}
			WriteLine("");
		}
		
		private void StartNamespaceSection()
		{
			if (!string.IsNullOrEmpty(Namespace))
			{
				WriteLine("namespace " + Namespace);
				WriteLine("{");
				IndentLevel = 1;
			}
		}
		
		private void EndNamespaceSection()
		{
			if (!string.IsNullOrEmpty(Namespace))
			{
				IndentLevel--;
				WriteLine("}");
			}
		}
		
		private void BuildFileContent()
		{
			StartClass();
			EndClass();
		}
		
		private string Indents(int Level)
		{
			string Result = "";
			
			for (int Index = 0; Index < Level; Index++)
			{
				Result += "    "; // 4 spaces per level
			}
			
			return Result;
		}
		
		private void StartClass()
		{
			WriteLine("public class " + ClassName);
			WriteLine("{");
				
			BuildProperties();
			BuildClearMethod();			
			BuildValidateMethod();
			BuildUpdateMethod();
			BuildMethodStubs();
		}
		
		private void BuildProperties()
		{
			IndentLevel++;
			
			WriteLine("#region Properties - Getters and Setters");
			
			foreach (PropertiesItem Item in Properties)
			{
                if (StringUtilities.SameText(Item.DataType, "int")) BuildIntProperty(Item);
                else if (StringUtilities.SameText(Item.DataType, "string")) BuildStringProperty(Item);
                else if (StringUtilities.SameText(Item.DataType, "double")) BuildDoubleProperty(Item);
                else if (StringUtilities.SameText(Item.DataType, "long")) BuildLongProperty(Item);
			}
			
			WriteLine("#endregion");
			WriteLine("");
			IndentLevel--;
		}
		
		private void BuildStringProperty(PropertiesItem Item)
		{
			WriteLine("public string " + Item.Name + " { get; set; }");
			WriteLine("");
		}
		
		private void BuildIntProperty(PropertiesItem Item)
		{
			WriteLine("private string F" + Item.Name + "Value = \"\";");
			WriteLine("public bool " + Item.Name + "IsValid = false;");
			WriteLine("public int " + Item.Name + " { get; set; }");
			WriteLine("public string " + Item.Name + "Value");
			WriteLine("{");
			IndentLevel++;
			WriteLine("get { return F" + Item.Name + "Value; }");
			WriteLine("set");
			WriteLine("{");
			IndentLevel++;
			WriteLine("F" + Item.Name + "Value = value;");
			//WriteLine(Item.Name + " = StringUtilities.StrToIntDef(F" + Item.Name + "Value, -1);");
			WriteLine("int Dummy = 0;");
			WriteLine(Item.Name + "IsValid = int.TryParse(F" + Item.Name + "Value, out Dummy);");
			WriteLine(Item.Name + " = (" + Item.Name + "IsValid) ? Dummy : 0;");
			IndentLevel--;
			WriteLine("}");
			IndentLevel--;				
			WriteLine("}");
			WriteLine("");
		}
		
		private void BuildDoubleProperty(PropertiesItem Item)
		{
			WriteLine("private string F" + Item.Name + "Value = \"\";");
			WriteLine("public bool " + Item.Name + "IsValid = false;");
			WriteLine("public double " + Item.Name + " { get; set; }");
			WriteLine("public string " + Item.Name + "Value");
			WriteLine("{");
			IndentLevel++;
			WriteLine("get { return F" + Item.Name + "Value; }");
			WriteLine("set");
			WriteLine("{");
			IndentLevel++;
			WriteLine("F" + Item.Name + "Value = value;");
			WriteLine("double Dummy = 0.0;");
			WriteLine(Item.Name + "IsValid = Double.TryParse(F" + Item.Name + "Value, out Dummy);");
			WriteLine(Item.Name + " = (" + Item.Name + "IsValid) ? Dummy : 0;");
			IndentLevel--;
			WriteLine("}");
			IndentLevel--;				
			WriteLine("}");
			WriteLine("");
		}

        private void BuildLongProperty(PropertiesItem Item)
        {
            WriteLine("private string F" + Item.Name + "Value = \"\";");
            WriteLine("public bool " + Item.Name + "IsValid = false;");
            WriteLine("public long " + Item.Name + " { get; set; }");
            WriteLine("public string " + Item.Name + "Value");
            WriteLine("{");
            IndentLevel++;
            WriteLine("get { return F" + Item.Name + "Value; }");
            WriteLine("set");
            WriteLine("{");
            IndentLevel++;
            WriteLine("F" + Item.Name + "Value = value;");
            WriteLine("long Dummy = 0;");
            WriteLine(Item.Name + "IsValid = long.TryParse(F" + Item.Name + "Value, out Dummy);");
            WriteLine(Item.Name + " = (" + Item.Name + "IsValid) ? Dummy : 0;");
            IndentLevel--;
            WriteLine("}");
            IndentLevel--;
            WriteLine("}");
            WriteLine("");
        }

        private void BuildClearMethod()
		{
			IndentLevel++;
			
			WriteLine("#region Clear");
			
			WriteLine("public void Clear()");
			WriteLine("{");
			
			IndentLevel++;
			
			string Str = "";
			string DataType = "";
			foreach (PropertiesItem Item in Properties)
			{
				Str = Item.Name;
				DataType = Item.DataType;
				if ((StringUtilities.SameText(Item.Validate, "Y")) || 
				    (StringUtilities.SameText(DataType, "double")))
				{
					if (StringUtilities.SameText(DataType, "int")) Str += "Value";
					else if (StringUtilities.SameText(DataType, "double")) Str += "Value";
					Str += " = \"\";";
					WriteLine(Str);
				}
			}
			
			IndentLevel--;
			
			WriteLine("}");
			
			WriteLine("#endregion");
			WriteLine("");
			
			IndentLevel--;
		}
		
		private void BuildValidateMethod()
		{
			IndentLevel++;
			
			WriteLine("#region Validate");
			
			WriteLine("public string Validate()");
			WriteLine("{");
			
			IndentLevel++;
			
			WriteLine("string Result = \"\";");
			WriteLine("try");
			WriteLine("{");
			IndentLevel++;
			WriteLine("string BaseFormat = \"{0} is {1}\";");
			WriteLine("string NotProvided = \"Not Provided\";");
			WriteLine("string Invalid = \"Invalid\";");
			WriteLine("string Key = \"\";");
			WriteLine("");

			foreach (PropertiesItem Item in Properties)
			{
				if (StringUtilities.SameText(Item.Validate, "Y"))
				{
					WriteLine("if (string.IsNullOrEmpty(Result))");
					WriteLine("{");
					IndentLevel++;
					WriteLine("Key = \"" + Item.Name + "\";");
					
					if (StringUtilities.SameText(Item.DataType, "string"))
					{						
						WriteLine("if (string.IsNullOrEmpty(" + Item.Name + ")) Result = string.Format(BaseFormat, Key, NotProvided + \" or \" + Invalid);");
					}
					else if ((StringUtilities.SameText(Item.DataType, "int")) || (StringUtilities.SameText(Item.DataType, "double")))
					{
						WriteLine("if (!" + Item.Name + "IsValid) Result = string.Format(BaseFormat, Key, NotProvided + \" or \" + Invalid);");
					}
					IndentLevel--;
					WriteLine("}");
					WriteLine("");					
				}
			}
			
			IndentLevel--;
			WriteLine("}");
			WriteLine("catch (Exception Ex)");
			WriteLine("{");
			IndentLevel++;
			WriteLine("Result =");
			IndentLevel++;
			WriteLine("Ex.Message + Environment.NewLine +");
			WriteLine("Ex.InnerException;");
			IndentLevel--;
			WriteLine("}");
			IndentLevel--;
			WriteLine("");
			WriteLine("return Result;");			
			
			IndentLevel--;
			
			WriteLine("}");
			
			WriteLine("#endregion");
			WriteLine("");
			
			IndentLevel--;
		}
		
		private void BuildUpdateMethod()
		{
			IndentLevel++;
			WriteLine("#region Update");
			
			WriteLine("public string Update(" + ClassName + " Source)");
			WriteLine("{");
			IndentLevel++;
			WriteLine("string Result = \"\";");
			WriteLine("");
			WriteLine("try");
			WriteLine("{");
			IndentLevel++;
			string Line = "";
			string ValueStr = "";
			foreach (PropertiesItem Item in Properties)
			{
				ValueStr = "";
				if ((StringUtilities.SameText(Item.DataType, "int")) || (StringUtilities.SameText(Item.DataType, "double")))
				{
					ValueStr = "Value";
				}
				Line = "this." + Item.Name + ValueStr + " = Source." + Item.Name + ValueStr + ";";
				WriteLine(Line);				
			}
			IndentLevel--;
			WriteLine("}");
			WriteLine("catch (Exception Ex)");
			WriteLine("{");
			IndentLevel++;
			WriteLine("Result = Ex.Message + Environment.NewLine + Ex.InnerException;");
			IndentLevel--;
			WriteLine("}");
			WriteLine("");
			WriteLine("return Result;");
			
			IndentLevel--;
			WriteLine("}");
			WriteLine("#endregion");
			WriteLine("");
			IndentLevel--;
		}
		
		private void BuildMethodStubs()
		{
			IndentLevel++;
			foreach (MethodStubItem Item in MethodStubs)
			{
				WriteLine("#region " + Item.Region);
				foreach (string Line in Item.Lines)
				{
					if (StringUtilities.SameText(Line, "{")) IndentLevel++;
					else if (StringUtilities.SameText(Line, "}")) IndentLevel--;
					WriteLine(Line);
				}
				WriteLine("#endregion");
				WriteLine("");
			}
			IndentLevel--;
		}
		
		private void EndClass()
		{
			WriteLine("}");
		}
		
		public string SaveToFile()
		{
			string Result = "";
			
			if (string.IsNullOrEmpty(OutputFileName)) 
			{
				Result = "The OutputFileName value has not been set";
				return Result;
			}
			
			try 
			{
				using (TextWriter writer = new StreamWriter(OutputFileName))
				{
					foreach (string s in Content)
					{
						writer.WriteLine(s);
					}
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.InnerException + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
	}
}
