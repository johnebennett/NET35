using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using System.IO;
using System.Diagnostics;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations.EmailStatementsEx
{
    public class EmailTemplateOnChange : ActionBase
    {
        private string FModuleName = "ContactOperations";

        private string FTemplateId = "";
        private int FTemplateIdValue = 0;

        public static string Execute(int MasterId,
                                     RequestCommonItem CommonItem,
                                     JObject DataObject,
                                     APISettingsItem SettingsItem,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            EmailTemplateOnChange inst = new EmailTemplateOnChange();
            if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
                return "";

            inst.LogOnInsideMethod(Method);
            try
            {
                Result = inst.Init();
                if (!string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
                    return "";
                }

                Result = inst.Validate();
                if (!string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
                    return "";
                }

                Result = inst.Output();
            }
            finally
            {
                inst.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        private string Init()
        {
            string Method = "Init";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                FTemplateId = (string)DataObject["templateId"];
                FTemplateIdValue = StringUtilities.StrToIntDef(FTemplateId, -1);
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }
            return Result;
        }

        private string Validate()
        {
            string Method = "Validate";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                if (string.IsNullOrEmpty(Result) && (FTemplateIdValue <= 0)) Result = "Invalid TemplateId Value provided";

                if (!string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Validation Failed: " + Result, FModuleName, MasterId);
                    Result =
                        JsonUtilities.StartJson() +
                        JsonUtilities.AddPropertyNameAndValue("Error", "Validation Failed", false) +
                        JsonUtilities.EndJson();
                }
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }
            return Result;
        }

        private string Output()
        {
            string Method = "Output";
            string Result = "";
            string ErrorMsg = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                Result = BuildOutput(out ErrorMsg);
                if (!string.IsNullOrEmpty(ErrorMsg))
                {
                    Result = LogOnProcessingError(Method, ErrorMsg, "");
                }
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }
            WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
            return Result;
        }

        /*
        private string BuildOutput(out string ErrorMsg)
        {
            ErrorMsg = "";
            string Result = "";
            string Method = "BuildOutput";
            string Value = "";
            
            string OutputLine = "";
            string SubjectLine = "";
            string EmailBody = "";
            List<string> Entries = new List<string>();
            List<string> Values = new List<string>();
            StringBuilder sb = new StringBuilder();
            Object Obj = null;
            
            LogOnInsideMethod(Method);
            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
                
                string QueryText = "SELECT OutputLine, SubjectLine, EmailBody FROM EMAIL_TEMPLATE_ONCHANGE_ITEMS(@CompanyId, @TemplateId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
                Params.Add(new FbParameterItem("@TemplateId", FTemplateIdValue));
                
                DataTable dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    foreach(DataRow ThisRow in dt.Rows)
                    {
                        Obj = ThisRow["OutputLine"];
                        OutputLine = ((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
                        if (!string.IsNullOrEmpty(OutputLine)) Entries.Add(OutputLine);
                        else
                        {
                            Obj = ThisRow["SubjectLine"];
                            SubjectLine = ((Obj == DBNull.Value) ? "" : Convert.ToString(Obj)); 
                            Obj = ThisRow["EmailBody"];
                            EmailBody = ((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
                            Value =
                                JsonUtilities.StartJsonObject("") +
                                JsonUtilities.AddPropertyNameAndValue("subjectLine", SubjectLine) +
                                JsonUtilities.AddPropertyNameAndValue("emailBody", EmailBody, false) +
                                JsonUtilities.EndJsonObject();
                            Values.Add(Value);
                        }
                    }
                }
                
                sb.Append(JsonUtilities.StartJson());
                sb.Append(JsonUtilities.StartJsonArray("Items"));
                sb.Append(string.Join(",", Entries.ToArray()));
                sb.Append(JsonUtilities.EndJsonArray());
                sb.Append(",");
                sb.Append(JsonUtilities.StartJsonArray("Values"));
                sb.Append(string.Join(",", Values.ToArray()));
                sb.Append(JsonUtilities.EndJsonArray());
                sb.Append(JsonUtilities.EndJson());
                Result = sb.ToString();
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }
            
            return Result;
        }
        */

        private string BuildOutput(out string ErrorMsg)
        {
            ErrorMsg = "";
            string Result = "";
            string Method = "BuildOutput";
            string Value = "";

            string OutputLine = "";
            string SubjectLine = "";
            string EmailBody = "";
            List<string> Entries = new List<string>();
            List<string> Values = new List<string>();
            StringBuilder sb = new StringBuilder();
            Object Obj = null;

            LogOnInsideMethod(Method);
            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);

                //string QueryText = "SELECT OutputLine, SubjectLine, EmailBody FROM EMAIL_TEMPLATE_ONCHANGE_ITEMS(@CompanyId, @TemplateId)";
                string QueryText = "SELECT OutputLine, SubjectLine FROM EMAIL_TEMPLATE_ONCHANGE_ITEMS(@CompanyId, @TemplateId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
                Params.Add(new FbParameterItem("@TemplateId", FTemplateIdValue));

                DataTable dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    foreach (DataRow ThisRow in dt.Rows)
                    {
                        Obj = ThisRow["OutputLine"];
                        OutputLine = ((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
                        if (!string.IsNullOrEmpty(OutputLine)) Entries.Add(OutputLine);
                        else
                        {
                            Obj = ThisRow["SubjectLine"];
                            SubjectLine = ((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
                            //                          Obj = ThisRow["EmailBody"];
                            //                          EmailBody = ((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
                            EmailBody = GetEmailBodyAsHtml(out ErrorMsg); ;
                            Value =
                                JsonUtilities.StartJsonObject("") +
                                JsonUtilities.AddPropertyNameAndValue("subjectLine", SubjectLine) +
                                JsonUtilities.AddPropertyNameAndValue("emailBody", EmailBody, false) +
                                JsonUtilities.EndJsonObject();
                            Values.Add(Value);
                        }
                    }
                }

                sb.Append(JsonUtilities.StartJson());
                sb.Append(JsonUtilities.StartJsonArray("Items"));
                sb.Append(string.Join(",", Entries.ToArray()));
                sb.Append(JsonUtilities.EndJsonArray());
                sb.Append(",");
                sb.Append(JsonUtilities.StartJsonArray("Values"));
                sb.Append(string.Join(",", Values.ToArray()));
                sb.Append(JsonUtilities.EndJsonArray());
                sb.Append(JsonUtilities.EndJson());
                Result = sb.ToString();
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }

            return Result;
        }

        private string GetEmailBodyAsHtml(out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";
            string Method = "GetEmailBodyAsHtml";
            string Args = "";
            string Dummy = "";
            string DocsLocation = "";
            string HtmlFileName = "";
            string SepChar = Path.DirectorySeparatorChar.ToString();

            LogOnInsideMethod(Method);
            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Inside GetEmailBodyAsHtml", FModuleName, MasterId);

                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        "SELECT AEM_BlobHtml FROM EAEmailTemplates WHERE I_AEM_CompanyClientId = @CompanyId AND I_AEM_ID = @TemplateId";
                    Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyId));
                    Params.Add(new FbParameterItem("TemplateId", FTemplateId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, MasterId);
                }

                DataTable tbl = null;
                object Obj = null;
                byte[] BlobBytes = new byte[0];
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);

                    if (string.IsNullOrEmpty(Result))
                    {
                        Obj = tbl.Rows[0]["AEM_BlobHtml"];
                        if (Obj != DBNull.Value) BlobBytes = (byte[])Obj;
                        Result = System.Text.Encoding.Default.GetString(BlobBytes);
                        if (string.IsNullOrEmpty(Result)) 
                            WindowsEventLoggingUtilities.WriteToInfoEvent("HtmlBlob Result: " + Result, Method, 0);
                    }
                }
                if (Result.Length > 0) return Result;

                global::UsefulDBMethods.Reporting.ReportUtilities reportUtils = new global::UsefulDBMethods.Reporting.ReportUtilities();
                reportUtils.GetCompanySettingsItems(FbConnectionString, CommonItem.CompanyIdValue, out Dummy, out DocsLocation, out ErrorMsg);
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    if (!DocsLocation.EndsWith(SepChar)) DocsLocation += SepChar;
                    HtmlFileName = DocsLocation + "{0}";
                    List<string> FileNameParts = new List<string>();
                    FileNameParts.Add(CommonItem.CompanyId);
                    FileNameParts.Add(CommonItem.ClientId);
                    FileNameParts.Add(FTemplateId);
                    HtmlFileName = string.Format(HtmlFileName, string.Join("_", FileNameParts.ToArray()));
                    HtmlFileName += ".html";

                    // Call the EmailProcessor to handle the sending of the emails
                    string WorkingDir = "";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling GetCurrentDirectory", FModuleName, MasterId);
                    //WorkingDir = Directory.GetCurrentDirectory();
                    WorkingDir = CommonUtilities.FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
                    if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling GetCurrentDirectory", FModuleName, MasterId);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("WorkingDir " + WorkingDir, FModuleName, MasterId);
                    try
                    {
                        Process runApp = new Process();
                        runApp.StartInfo.FileName = WorkingDir + "EAOfficeUtilities";
                        List<string> ArgsList = new List<string>();
                        ArgsList.Add("cmd=GetEmailTemplateTextAsHtml");
                        ArgsList.Add("EmailTemplateId=" + FTemplateId);
                        ArgsList.Add("CCID=" + CommonItem.CompanyId);
                        ArgsList.Add("ClientId=" + CommonItem.ClientId);
                        Args = string.Join(" ", ArgsList.ToArray());
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Command: " + runApp.StartInfo.FileName + " " + Args, FModuleName, MasterId);
                        runApp.StartInfo.Arguments = Args;
                        runApp.Start();
                        const int tries = 60;
                        for (int Index = 0; Index < tries; Index++)
                        {
                            if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
                        }
                        if (runApp.HasExited)
                        {
                            Result = GetFileContent(HtmlFileName, out ErrorMsg);
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorMsg = LogOnException(Method, Ex, "");
                    }
                    finally
                    {
                        Directory.SetCurrentDirectory(WorkingDir);
                    }

                }

            }
            catch (Exception Ex)
            {
                ErrorMsg = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }

            return Result;
        }

        private string GetFileContent(string FileName,
                                      out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";
            string Method = "GetFileContent";

            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("GetFileContent - FileName: " + FileName, FModuleName, MasterId);
                Result = File.ReadAllText(FileName);
            }
            catch (Exception Ex)
            {
                ErrorMsg = LogOnException(Method, Ex, "");
            }
            return Result;
        }

    }
}
