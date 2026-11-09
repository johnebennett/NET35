/*
 * Created by SharpDevelop.
 * User: John
 * Date: 03/07/2019
 * Time: 10:38 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using System.Text;
using System.IO;
using System.Windows.Forms.VisualStyles;
using CommonUtilities;

namespace MakeItemClasses
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private StringBuilder FileContent = new StringBuilder();
		private FileSettingsItem settingsItem = new FileSettingsItem();
		private int IndentLevel = 0;
		private string WorkingFileName = "";
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void BtnExitClick(object sender, EventArgs e)
		{
			Close();
		}
		
		void MnuMain_File_ExitClick(object sender, EventArgs e)
		{
			Close();
		}
		
		void MnuMain_File_OpenClick(object sender, EventArgs e)
		{
			txtFileContent.Clear();
			WorkingFileName = "";
			/* Open the file dialog and get the JSON file to process */
			string CurrentDir = Directory.GetCurrentDirectory();
			if (!CurrentDir.EndsWith(Path.PathSeparator.ToString()))
			{
				CurrentDir += "\\";
			}
			CurrentDir += "Files";
			dlgOpenFile.InitialDirectory = CurrentDir;
			if (dlgOpenFile.ShowDialog() == DialogResult.OK)
			{
				WorkingFileName = dlgOpenFile.FileName;
				BtnGenerateContentClick(sender, e);
			}
		}
		
		
		private void WriteLine(string LineText)
		{
			//txtFileContent.AppendText(Indents(IndentLevel) + LineText + Environment.NewLine);
			txtFileContent.AppendText(LineText + Environment.NewLine);
		}

		void BtnGenerateContentClick(object sender, EventArgs e)
		{
			txtFileContent.Clear();
				string Result = settingsItem.LoadFromFile(dlgOpenFile.FileName);
				/*
				using (StreamReader reader = new StreamReader(dlgOpenFile.FileName))
				{
					FileContent.Append(reader.ReadToEnd());
					WriteLine("File content read");
					ProcessFileContent();
				}
				*/
				if (!string.IsNullOrEmpty(Result))
				{
					MessageBox.Show(Result);					
				}
				else
				{
					List<string> Content = settingsItem.GenerateContent(out Result);
					if (!string.IsNullOrEmpty(Result))
					{
						MessageBox.Show(Result);
					}
					else
					{
						foreach (string Line in Content)
						{
							WriteLine(Line);
						}
					}
				}
			/*
			txtFileContent.Clear();
			
			string ErrorMsg = "";
			List<string> Content = settingsItem.GenerateContent(out ErrorMsg);
			
			BuildContentHeader();
			BuildUsingSection();
			StartNamespaceSection();
			BuildFileContent();
			EndNamespaceSection();
			*/
		}
		
		void BtnSaveToFileClick(object sender, EventArgs e)
		{
			if (File.Exists(settingsItem.OutputFileName))
			{				
				DialogResult res =
					MessageBox.Show(
						"The file: " + settingsItem.OutputFileName + " already exists." + Environment.NewLine + 
						"Do you want to overwrite the existing file?",
						"Overwrite File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (res != DialogResult.Yes) return;
			}
			
			/*
			if (string.IsNullOrEmpty(settingsItem.OutputFileName))
			{
				MessageBox.Show("The OutputFileName value has not been set");
				return;
			}
			*/
			
			string Result = settingsItem.SaveToFile();
			if (!string.IsNullOrEmpty(Result)) MessageBox.Show(Result);
			else MessageBox.Show("File saved successfully");
		}

		/*
		private void ProcessFileContent()
		{
			string Result = settingsItem.Load(FileContent.ToString());
			if (string.IsNullOrEmpty(Result))
			{
				string Msg = 
					"Author: " + settingsItem.Author + Environment.NewLine +
					"FileName: " + settingsItem.FileName + Environment.NewLine +
					"Properties.Count: " + Convert.ToString(settingsItem.Properties.Count) + Environment.NewLine +
					"Includes.Count: " + Convert.ToString(settingsItem.Includes.Count);
				WriteLine(Msg);
			}
			else
			{
				MessageBox.Show("Error: " + Result);
			}
		}
		
		void BtnGenerateContentClick(object sender, EventArgs e)
		{
			txtFileContent.Clear();
			
			string ErrorMsg = "";
			List<string> Content = settingsItem.GenerateContent(out ErrorMsg);
			
			BuildContentHeader();
			BuildUsingSection();
			StartNamespaceSection();
			BuildFileContent();
			EndNamespaceSection();
		}
		
		private void WriteLine(string LineText)
		{
			txtFileContent.AppendText(Indents(IndentLevel) + LineText + Environment.NewLine);
		}
		
		private void BuildContentHeader()
		{
			WriteLine("/*");
			WriteLine(" * Created by Make Item Classes.");
			WriteLine(" * Author: " + settingsItem.Author);
			WriteLine(" * Date: " + DateTime.Now.ToString("dd-MM-yyyy"));
			WriteLine(" * Time: " + DateTime.Now.ToString("t"));
			WriteLine("");
			WriteLine(" * SourceFile: " + dlgOpenFile.FileName);
			WriteLine("*//*");
		}
		
		private void BuildUsingSection()
		{
			foreach (string Value in settingsItem.Includes)
			{
				WriteLine("using " + Value + ";");
			}
			WriteLine("");
		}
		
		private void StartNamespaceSection()
		{
			if (!string.IsNullOrEmpty(settingsItem.Namespace))
			{
				WriteLine("namespace " + settingsItem.Namespace);
				WriteLine("{");
				IndentLevel = 1;
			}
		}
		
		private void EndNamespaceSection()
		{
			if (!string.IsNullOrEmpty(settingsItem.Namespace))
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
			WriteLine("public class " + settingsItem.ClassName);
			WriteLine("{");
				
			BuildProperties();
			BuildClearMethod();
			BuildValidateMethod();
			BuildMethodStubs();
		}
		
		private void BuildProperties()
		{
			IndentLevel++;
			
			WriteLine("#region Properties - Getters and Setters");
			
			foreach (PropertiesItem Item in settingsItem.Properties)
			{
				if (StringUtilities.SameText(Item.DataType, "int")) BuildIntProperty(Item);
				else if (StringUtilities.SameText(Item.DataType, "string")) BuildStringProperty(Item);
			}
			
			WriteLine("#endregion");
			WriteLine("");
			IndentLevel--;
		}
		
		private void BuildIntProperty(PropertiesItem Item)
		{
			WriteLine("private string F" + Item.Name + "Value = \"\";");
			WriteLine("public int " + Item.Name + " { get; set; }");
			WriteLine("public string " + Item.Name + "Value");
			WriteLine("{");
			IndentLevel++;
			WriteLine("get { return F" + Item.Name + "; }");
			WriteLine("set");
			WriteLine("{");
			IndentLevel++;
			WriteLine("F" + Item.Name + "Value = value;");
			WriteLine(Item.Name + " = StringUtilities.StrToIntDef(F" + Item.Name + "Value, -1);");
			IndentLevel--;
			WriteLine("}");
			IndentLevel--;				
			WriteLine("}");
			WriteLine("");
		}
		
		private void BuildStringProperty(PropertiesItem Item)
		{
			WriteLine("public string " + Item.Name + " { get; set; }");
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
			foreach (PropertiesItem Item in settingsItem.Properties)
			{
				if (StringUtilities.SameText(Item.Validate, "Y"))
				{
					Str = Item.Name;
					if (StringUtilities.SameText(Item.DataType, "int")) Str += "Value";
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
			
			WriteLine("pubic string Validate()");
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

			foreach (PropertiesItem Item in settingsItem.Properties)
			{
				if (StringUtilities.SameText(Item.Validate, "Y"))
				{
					WriteLine("if (string.IsNullOrEmpty(Result))");
					WriteLine("{");
					IndentLevel++;
					WriteLine("Key = \"" + Item.Name + "\";");
					
					if (StringUtilities.SameText(Item.DataType, "int"))
					{
						WriteLine("if (string.IsNullOrEmpty(" + Item.Name + "Value)) Result = string.Format(BaseFormat, Key, NotProvided);");
						WriteLine("else if (" + Item.Name + " < 0)                   Result = string.Format(BaseFormat, Key, Invalid);");
					}
					else if (StringUtilities.SameText(Item.DataType, "string"))
					{						
						WriteLine("if (string.IsNullOrEmpty(" + Item.Name + "Value)) Result = string.Format(BaseFormat, Key, NotProvided + \" or \" + Invalid);");
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
			WriteLine("Ex.InnerException + Environment.NewLine +");
			WriteLine("Ex.Message + Environment.NewLine +");
			WriteLine("Ex.StackTrace;");
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
		
		private void BuildMethodStubs()
		{
			IndentLevel++;
			foreach (MethodStubItem Item in settingsItem.MethodStubs)
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
		}*/
	}
}
