/*
 * Created by SharpDevelop.
 * User: John
 * Date: 17/05/2019
 * Time: 5:58 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using System.IO;
/*
using Word = Microsoft.Office.Interop.Word;
using WordApp = Microsoft.Office.Interop.Word._Application;
using WordDoc = Microsoft.Office.Interop.Word._Document;
*/

/// <summary>
/// Description of TMSWordHelpers.
/// </summary>
public class TMSWordHelpers
{
	
	private string FLastErrorMessage = "";
	private const int wdExportFormatPDF     = 17;
	private const int wdFormatFilteredHTML  = 10;
	private const int wdFormatHTML          = 8;
	private const int wdAlertsAll           = -1;
	private const int wdAlertsMessageBox    = -2;
	private const int wdAlertsNone          = 0;
	
	private Object oMissing                 = System.Reflection.Missing.Value;
	private Object oTrue                    = true;
	private Object oFalse                   = false;
	
	private const string WORD_APPLICATION   = "Word.Application";
	private const string MIN_WORD_VERSION   = "12";
	
	//private Microsoft.Office.Interop.Word.Application FWordApplication;
	
	public TMSWordHelpers()
	{
		FWordApplication = new Microsoft.Office.Interop.Word.Application();			
		FWordApplication.Visible = false;
		FWordApplication.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone;
	}
	
	~TMSWordHelpers()
	{
		try 
		{
			if (FWordApplication != null)
			{
				Object SaveChanges = (bool)false;
				((WordApp)FWordApplication).Quit(ref oFalse, ref oMissing, ref oMissing);
			}
		}
		catch //(Exception Ex)
		{
			// what to do here?
			//Console.WriteLine(Ex.Message + Environment.NewLine + Ex.StackTrace);
		}
	}
	
	public string ErrorDescription(int ErrorNumber)
	{
		string Result = "Unknown Error Description";
		if (ErrorNumber >= 0) Result = Convert.ToString(ErrorNumber) + " - OK";
		else if (ErrorNumber <= -900) Result = BuildUnknownErrorStr(ErrorNumber);
		else
		{
			switch (ErrorNumber)
			{
				case -001: Result = "Word is not available"; break;
				case -002: Result = "Word version needs to be 2007 or above"; break;
				case -003: Result = "Empty Text returned"; break;
			}
		}
		return Result;
	}
	
	public string BuildUnknownErrorStr(int ErrorNumber)
	{
		string Result = "";
		switch (ErrorNumber)
		{
			case -999: Result = "Word Version - "; break;
			case -998: Result = "WordDocToPDF - "; break;
			case -997: Result = "TextDocToPDF - "; break;
			case -996: Result = "WordDocToText - "; break;
			case -995: Result = "WordDocToTextStr - "; break;
			case -994: Result = "StringToPDFDoc - "; break;
			case -993: Result = "InternalWordDocToText - "; break;
			case -992: Result = "RTFToHTMLDoc - "; break;
			case -991: Result = "CheckWordVersion - "; break;
		}
		Result = Result + FLastErrorMessage;
		return Result;
	}
	
	public string WordVersion()
	{
		return FWordApplication.Version;
	}
	
	private int CheckWordVersion()
	{
		string sWordVersion = "";
		int Result = 0;
		try 
		{
			sWordVersion = FWordApplication.Version;
			Result = (int)Convert.ToDouble(sWordVersion);
		}
		catch (Exception Ex)
		{
			FLastErrorMessage =
				"Exception in CheckWordVerion:" + Environment.NewLine +
				Ex.Message + "." + Ex.StackTrace;
			WriteToLog(FLastErrorMessage);
			Result = -991;
		}
		
		return Result;
	}
	
	public int WordDocToPDF(string WordDocumentFileName,
	                        string PDFDocumentFileName)
	{
		int Result = CheckWordVersion();
		if (Result < 12) Result = -002;
		else             Result = 0;
		
		if (Result < 0) return Result;
		
		string WordDoc = WordDocumentFileName;
		string PDFDoc = PDFDocumentFileName;
		//Microsoft.Office.Interop.Word.Document Doc = null;
		Word.Document Doc = null;
		Object oFile = WordDoc;
		Object oPDFFile = PDFDoc;
		
		try 
		{
			Doc = 
				FWordApplication.Documents.Open(
					ref oFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,
					ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing);
			//Doc = FWordApplication.Documents.Open(FileName: ref oFile);
		}
		catch //(Exception Ex)
		{
			Result = -992;
			FLastErrorMessage = "WordDocToPDF - Error opening the file: " + WordDoc;
		}
		
		try 
		{
			if ((Result >= 0) && (Doc != null))
			{
				Doc.ExportAsFixedFormat(PDFDoc, Word.WdExportFormat.wdExportFormatPDF);
			}
		}
		catch (Exception Ex)
		{
			FLastErrorMessage =
				"Exception in WordDocToPDF:" + Environment.NewLine +
				Ex.Message + "." + Ex.StackTrace;
			Result = -998;
		}
		
		return Result;
	}
	
	public int WordDocToText(string WordDocumentFileName,
	                           string TextDocumentFileName)
	{
		int Result = 0;
		string DocContents = "";
		Result = InternalWordDocToText(WordDocumentFileName, out DocContents);
		if (Result < 0) return Result;
		
		if (string.IsNullOrEmpty(DocContents))
		{
			Result = -003;
			return Result;
		}
		
		try
		{
			File.WriteAllText(TextDocumentFileName, DocContents);
			Result = 0;
		}
		catch (Exception Ex)
		{
			FLastErrorMessage =
				"Exception in WordDocToText:" + Environment.NewLine +
				Ex.Message + "." + Ex.StackTrace;
			Result = -996;
		}
		
		return Result;
	}

	private int InternalWordDocToText(string WordDoc,
	                                  out string DocContents)
	{
		DocContents = "";
		int Result = CheckWordVersion();

		if (Result < 12) Result = -002;
		else             Result = 0;
		
		if (Result < 0) return Result;

		Word.Document Doc = null;
		Object oFile = WordDoc;
		
		try 
		{
			Doc = 
				FWordApplication.Documents.Open(
					ref oFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,
					ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing);
			//Doc = FWordApplication.Documents.Open(FileName: ref oFile);
		}
		catch //(Exception Ex)
		{
			Result = -992;
			FLastErrorMessage = "InternalWordDocToText - Error opening the file: " + WordDoc;
		}
		
		if (Result < 0) return Result;
		
		try
		{
			try
			{
				Object oStart = 0;
				DocContents = Doc.Range(ref oStart, ref oMissing).Text;
		      	//DocContents = ReplaceControlCharacters(DocContents);
			}
		    finally
		    {		    	 
		    	((WordDoc)Doc).Close(ref oFalse, ref oMissing, ref oMissing);
		    	//Doc := Unassigned;
		    }
		}
		catch (Exception Ex)
		{
			FLastErrorMessage =
		        "Exception in InternalWordDocToText:" + Environment.NewLine +
		      	Ex.Message + "." + Ex.StackTrace;
		      Result = -993;
		}
		
		return Result;
	}
	
	public string ReplaceControlCharacters(string Source)
	{
		int Count = 31;
		string Result = Source;
		for (int Index = 0; Index < Count; Index++)
		{
			Result = Result.Replace((char)(Index), '.');
		}
		
		return Result;
	}
	
	public int RTFDocToHTML(string RTFDocumentFileName,
	                        string HTMLDocumentFileName)
	{
		/*
		Doc: OleVariant;
		IntVersion: Integer;
		*/
		
		int Result = CheckWordVersion();

		if (Result < 12) Result = -002;
		else             Result = 0;
		
		if (Result < 0) return Result;
		
		Word.Document Doc = null;
		string RTFDoc = RTFDocumentFileName;
		Object oFile = RTFDoc;
		string HTMLDoc = HTMLDocumentFileName;
		Object oHTMLDoc = HTMLDoc;
		bool HasMethodResult = false;
		Object oHtmlFormat = wdFormatHTML;
		
		try
		{
			try
			{
				/*
				Doc =
					FWordApplication.Documents.Open(
						ref oFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,
						ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing,  ref oMissing);
				*/
				Doc = FWordApplication.Documents.Open(FileName: ref oFile);
				// This only works for Word 2007 or greater - version 12 or above
		        WriteToLog("Processing RTFDocToHTML");
		        HasMethodResult = HasMethod(Doc, "SaveAs2");
		        if (HasMethodResult) Console.WriteLine("SaveAs2 method exists");
				else                 Console.WriteLine("SaveAs2 method does not exist");
				if (HasMethodResult)
				{
					/*
					((WordDoc)Doc).SaveAs2(ref oHTMLDoc, ref oHtmlFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
					                       ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, 
					                       ref oMissing, ref oMissing);
					*/
					//((WordDoc)Doc).SaveAs2(FileName: ref oHTMLDoc, FileFormat: ref oHtmlFormat);
					((WordDoc)Doc).SaveAs(FileName: ref oHTMLDoc, FileFormat: ref oHtmlFormat);
				}
				else
				{
					((WordDoc)Doc).SaveAs(ref oHTMLDoc, ref oHtmlFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
					                      ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
					//((WordDoc)Doc).SaveAs(FileName: ref oHTMLDoc, FileFormat: ref oHtmlFormat);
		        	WriteToLog("Processing RTFDocToHTML Complete");
				}
			}
			catch (Exception Ex)
			{
				/*
		        // Trap error and just log
		        on E: Exception do
		        begin
		          WriteToLog('Exception closing Word Document: ' + WordDoc + #13#10 +
		            '  ' + E.ClassName + ' - ' + E.Message);
		        end;
		      end;
		
		      Doc := Unassigned;
		    end;
		    */
		   		WriteToLog("Exception closing Word Document: " + RTFDoc + Environment.NewLine +
		                     Ex.Message + "." + Ex.StackTrace);
			}
			finally
			{
				WriteToLog("RTFDocToHTML - Closing Document");
				((WordDoc)Doc).Close();
				WriteToLog("RTFDocToHTML - Closed");
			}
		}
	    catch (Exception Ex)
	    {
	    	FLastErrorMessage =
	    		"Exception in RTFDocToHTML:" + Environment.NewLine +
	    		Ex.Message + "." + Ex.StackTrace;
	    	Result = -992;
	    }
	    
	    return Result;
	}
	
	private bool HasMethod(Object Source,
	                       string MethodName)
	{
		bool Result = false;
		var type = Source.GetType();
		Console.WriteLine("HasMethod: " + type.ToString());
		Result = (type.GetMethod(MethodName) != null);
		/*
		OleStr: WideString;
		iDispId: Integer;
		Result := (IDispatch(Obj) <> nil);
		if (Result = False) then Exit;
		
		OleStr := StringToOleStr(MethodName);
		Result :=
		(IDispatch(Obj).GetIDsOfNames(
		   GUID_NULL, @OleStr, 1, LOCALE_USER_DEFAULT, @iDispId) = S_OK);
		Result := Result and (iDispId > 0);
		*/
		return Result;
	}
	
	public int RTFDocToHTMLFiltered(string RTFDocumentFileName,
	                                string HTMLDocumentFileName)
	{
		int Result = CheckWordVersion();

		if (Result < 12) Result = -002;
		else             Result = 0;
		
		if (Result < 0) return Result;
		
		Word.Document Doc = null;
		string RTFDoc = RTFDocumentFileName;
		Object oFile = RTFDoc;
		string HTMLDoc = HTMLDocumentFileName;
		Object oHTMLDoc = HTMLDoc;
		bool HasMethodResult = false;
		Object oHtmlFormat = wdFormatFilteredHTML;
		
		try
		{
			try
			{
				if (!File.Exists(RTFDoc))
				{
					Result = -992;
					FLastErrorMessage = 
						"RTFDocToHTMLFiltered - The file: " + RTFDoc + " does not exist.";
					WriteToLog(FLastErrorMessage);
					return Result;
				}
				
				try
				{
					WriteToLog("RTFDocToHTMLFiltered - Before calling Documents.Open");
					Doc = FWordApplication.Documents.Open(FileName: ref oFile);
					WriteToLog("RTFDocToHTMLFiltered - After calling Documents.Open");
				}
				catch
				{
					Result = -992;
					FLastErrorMessage = 
						"RTFDocToHTMLFiltered - Error opening the file: " + RTFDoc;
					WriteToLog(FLastErrorMessage);
					return Result;
				}
				
				WriteToLog("Processing RTFDocToHTMLFiltered");
				HasMethodResult = HasMethod(Doc, "SaveAs2");
				if (HasMethodResult) Console.WriteLine("SaveAs2 method exists");
				else                 Console.WriteLine("SaveAs2 method does not exist");
				WriteToLog("RTFDocToHTMLFiltered - Before calling Doc.SaveXXX");
				//if (HasMethodResult) Doc.SaveAs2(FileName: ref oHTMLDoc, FileFormat: ref oHtmlFormat);
				if (HasMethodResult) Doc.SaveAs(FileName: ref oHTMLDoc, FileFormat: ref oHtmlFormat, InsertLineBreaks: oFalse);
				else                 Doc.SaveAs(FileName: ref oHTMLDoc, FileFormat: ref oHtmlFormat, InsertLineBreaks: oFalse);
				WriteToLog("RTFDocToHTMLFiltered - After calling Doc.SaveXXX");
				WriteToLog("Processing RTFDocToHTMLFiltered Complete");
			}
			catch (Exception Ex)
			{
				// Trap error and just log as HTML should have been created
				// With client Buxton Bentleigh this would periodically raise exception due to Word auto recovery
				WriteToLog("RTFDocToHTMLFiltered - Exception closing Word Document: " + Environment.NewLine + RTFDoc + Environment.NewLine + 
				           "  " + Ex.Message + " - " + Ex.StackTrace);
		   		Console.WriteLine(
					"Exception closing Word Document: " + RTFDoc + Environment.NewLine +
					Ex.Message + "." + Ex.StackTrace);
			}
			finally
			{
				((WordDoc)Doc).Close(SaveChanges: ref oFalse);   // When closing dont save changes
			}
		}
		catch (Exception Ex)
		{
			FLastErrorMessage =
				"Exception in RTFDocToHTMLFiltered:" + Environment.NewLine +
				Ex.Message + "." + Ex.StackTrace;
			WriteToLog(FLastErrorMessage);
			Result = -992;
			return Result;
		}
		
		return Result;
	}
	
	public int RTFDocToPDF(string RTFDocumentFileName,
	                       string PDFDocumentFileName)
	{
		// As far as Word is concerned there is no difference between a .doc
		// or a .rtf file - it all works the same.
		int Result = WordDocToPDF(RTFDocumentFileName, PDFDocumentFileName);
		return Result;
	}

	public int StringToPDFDoc(string SourceText,
	                          string PDFFileName)
	{
		int Result = CheckWordVersion();

		if (Result < 12) Result = -002;
		else             Result = 0;
		
		if (Result < 0) return Result;
		
		Word.Document Doc = null;
		try
		{
			Doc = FWordApplication.Documents.Add();
			FWordApplication.Selection.TypeText(SourceText);
			((WordDoc)Doc).ExportAsFixedFormat(PDFFileName, Word.WdExportFormat.wdExportFormatPDF);
			Result = 0;
		}
		catch (Exception Ex)			
		{
			FLastErrorMessage =
				"Exception in StringToPDFDoc:" + Environment.NewLine +
				Ex.Message + "." + Ex.StackTrace;
			WriteToLog(FLastErrorMessage);
			Result = -994;
		}
		
		return Result;
	}

	public int TextDocToPDF(string TextDocumentFileName,
	                       string PDFDocumentFileName)
	{
		int Result = CheckWordVersion();

		if (Result < 12) Result = -002;
		else             Result = 0;
		
		if (Result < 0) return Result;
		
		string FileContents = File.ReadAllText(TextDocumentFileName);		
		Word.Document Doc = null;
		
		try
		{
			Doc = FWordApplication.Documents.Add();
			FWordApplication.Selection.TypeText(FileContents);
			((WordDoc)Doc).ExportAsFixedFormat(PDFDocumentFileName, Word.WdExportFormat.wdExportFormatPDF);
			Result = 0;
		}
		catch (Exception Ex)
		{
			FLastErrorMessage =
				"Exception in TextDocToPDF:" + Environment.NewLine +
				Ex.Message + "." + Ex.StackTrace;
			WriteToLog(FLastErrorMessage);
			Result = -997;
		}
		
		return Result;
	}
	
	public int WordDocToTextStr(string WordDocumentFileName,
	                            out string DocumentContents)
	{
		int Result = 0;
		DocumentContents = "";
		try
		{
			Result = InternalWordDocToText(WordDocumentFileName, out DocumentContents);
		}
		catch (Exception Ex)
		{
			FLastErrorMessage = 
				"Exception: " + Environment.NewLine +
				Ex.Message + "." + Ex.StackTrace;
			Result = -995;
		}
		return Result;
	}

	private void WriteToLog(string LogMessage)
	{
		string FileName = 
			"WordHelpers_" + CommonUtilities.MiscUtilities.DateTimeToYYYYMMDD(DateTime.Now) + ".Log";
		string Dir = Directory.GetCurrentDirectory();
		if (!Dir.EndsWith("\\")) Dir += "\\";
		Dir += @"Logs\";
		if (!Directory.Exists(Dir)) Directory.CreateDirectory(Dir);
		FileName = Dir + FileName;
		using (var tw = new StreamWriter(FileName, true))
		{
			tw.WriteLine(LogMessage);
		}
		/*
  F: TextFile;
  FileName: String;
begin
  try
    FileName :=
      IncludeTrailingPathDelimiter(ExtractFilePath(ParamStr(0))) +
      'WordHelpers_' + FormatDateTime('YYYYMMDD', NOW) + '.Log';
    AssignFile(F, FileName);
    try
      if (FileExists(FileName) = True) then Append(F)
                                       else Rewrite(F);
      WriteLn(F, FormatDateTime('HH:MM:SS', NOW) + ' - ' + LogMessage);
    finally
      CloseFile(F);
    end;
  except
    // trap any errors that may occur when logging
  end;
  */
	}

	public int GetWordAddins(ref List<string> StrList)
	{
		int Result = CheckWordVersion();

		if (Result < 12) Result = -002;
		else             Result = 0;
		
		if (Result < 0) return Result;
		
		//(WordApp)FWordApplication.AddIns CurrentAddins = null;

		try
		{
			StrList.Clear();
			StrList.Add("AddIns");
			StrList.Add("------");
			//var TheAddIns = FWordApplication.AddIns;
			Microsoft.Office.Interop.Word.AddIns TheAddIns = FWordApplication.AddIns;
			int Count = TheAddIns.Count;
			StrList.Add("Total AddIns Count: " + Convert.ToString(Count));
			StrList.Add("");
			foreach (Microsoft.Office.Interop.Word.AddIn AddIn in FWordApplication.AddIns)
			{
				StrList.Add(AddIn.Name);
				StrList.Add(AddIn.Path);
			}
			StrList.Add("");
			StrList.Add("COMAddIns");
			StrList.Add("------");
			
			/*
			var TheComAddins = FWordApplication.COMAddIns;
			StrList.Add("Total Count: " + Count.ToString());
			StrList.Add("");
			*/
			
			//foreach (var AddIn in FWordApplication.COMAddIns)
				/*
			for (int Index = 1; Index < FWordApplication.COMAddIns.Count; Index++)
			{
				StrList.Add(AddIn.D .Item(Index).Description);
        StrList.Add(AddIns.Item(Index).progID);
        StrList.Add(AddIns.Item(Index).Guid);
        StrList.Add('');       
       
			}
			FLastErrorMessage = Count.ToString();
				 */
        }
		catch (Exception Ex)
		{
			FLastErrorMessage =
				"Exception in GetWordAddins:" + Environment.NewLine +
				Ex.Message + "." + Ex.StackTrace;
			WriteToLog(FLastErrorMessage);
			Result = -992;
			
		}
		return Result;
	}

	public int GetSystemValues(ref List<string> StrList)
	{
  		int Result = CheckWordVersion();

		if (Result < 12) Result = -002;
		else             Result = 0;
		
		if (Result < 0) return Result;
		
		Microsoft.Office.Interop.Word.System SystemObj = null;
		
		try
		{
			SystemObj = FWordApplication.System;
			StrList.Add("Free Disk Space: " + SystemObj.FreeDiskSpace.ToString());
			StrList.Add("Horizontal Resolution: " + SystemObj.HorizontalResolution.ToString());
			StrList.Add("Vertical Resolution: " + SystemObj.VerticalResolution.ToString());
			StrList.Add("Language Designation: " + SystemObj.LanguageDesignation);
			string Value = (SystemObj.MathCoprocessorInstalled == true) ? "Yes" : "No";
			StrList.Add("Math Co-processor Installed : " + Value);
			StrList.Add("Operating System: " + SystemObj.OperatingSystem);
			//StrList.Add("Private Profile String: " + SystemObj.PrivateProfileString);
			//StrList.Add('Profile String: ' + SystemObj.ProfileString);
			StrList.Add("Version: " + SystemObj.Version);
		}
		catch (Exception Ex)
		{
			FLastErrorMessage =
				"Exception in GetSystemValues:" + Environment.NewLine +
				Ex.Message + "." + Ex.StackTrace;
			WriteToLog(FLastErrorMessage);
			Result = -992;
		}
		return Result;
	}
}