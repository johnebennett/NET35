using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Portal.PortalEmailDlg
{
    public class AddEmailUploadAttachments : ActionBase
    {
        private string FModuleName = "Portal.PortalEmailDlg";

        private string FRunId = "";
        private int FRunIdValue = 0;
        private string FFileName = "";
        private List<string> FFileList = new List<string>();
        private int FFileListCount = 0;

        public static string Execute(int MasterId,
                                     RequestCommonItem CommonItem,
                                     JObject DataObject,
                                     APISettingsItem SettingsItem,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            AddEmailUploadAttachments inst = new AddEmailUploadAttachments();
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
                FRunId = (string)DataObject["runId"];
                FRunIdValue = StringUtilities.StrToIntDef(FRunId, -1);
                //FFileName = (string)DataObject["fileName"];
                JArray FilesObject = (JArray)DataObject["files"];
                if (FilesObject == null)
                {
                    Result = "FilesObject IS NULL";
                }

                string Str = "";
                string FileContent = "";
                string BaseDir = FolderHelpers.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
                if (string.IsNullOrEmpty(Result))
                {
                    if (BaseDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    {
                        BaseDir = BaseDir.Remove(BaseDir.Length - 1);
                    }
                    BaseDir = Directory.GetParent(BaseDir).FullName;
                    if (!BaseDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                        BaseDir += Path.DirectorySeparatorChar.ToString();
                    BaseDir = BaseDir + "Uploads" + Path.DirectorySeparatorChar.ToString();
                    WindowsEventLoggingUtilities.WriteToInfoEvent("BaseDir: " + BaseDir, FModuleName, MasterId);
                }
                if (string.IsNullOrEmpty(Result))
                {
                    FFileListCount = FilesObject.Count;
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Files Count: " + FFileListCount, FModuleName, MasterId);

                    if (FilesObject.Count > 0)
                    {
                        foreach (JObject Item in FilesObject)
                        {
                            FFileName = CommonItem.CompanyId + "_" + CommonItem.ClientId + "_";
                            FFileName += (string)Item["FileName"];
                            FFileName = BaseDir + FFileName;
                            FileContent = (string)Item["Content"];
                            if (FileContent.Contains(","))
                            {
                                FileContent = FileContent.Substring(FileContent.IndexOf(",") + 1);
                            }
                            Str =
                                "FileName: " + FFileName + Environment.NewLine +
                                "FileSize: " + (string)Item["FileSize"] + Environment.NewLine +
                                "LastModified: " + (string)Item["LastModified"] + Environment.NewLine +
                                "Content: " + FileContent;
                            //WindowsEventLoggingUtilities.WriteToInfoEvent(Str, FModuleName, MasterId);
                            Result = SaveFileContent(FFileName, FileContent);
                            if (!string.IsNullOrEmpty(Result))
                            {
                                break;
                            }
                            FFileList.Add(FFileName);
                        }
                    }
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

        private string SaveFileContent(string FileName, string FileContent)
        {
            string Method = "SaveFileContent";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Processing File: " + FileName, FModuleName, MasterId);
                File.WriteAllBytes(FileName, Convert.FromBase64String(FileContent));
                if (!File.Exists(FileName) || (File.Exists(FFileName) && new FileInfo(FileName).Length == 0))
                {
                    Result = "File " + FileName + " does not exist or its length is zero";
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

        private string Validate()
        {
            string Method = "Validate";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                if (string.IsNullOrEmpty(Result) && (FRunIdValue < 0)) Result = "Invalid RunId provided";

                if (string.IsNullOrEmpty(Result) && (FFileList.Count == 0))
                {
                    Result = "There are no files to process";
                }
                else if (string.IsNullOrEmpty(Result) && (FFileList.Count != FFileListCount))
                {
                    Result = "FFileList.Count ({0}) != FFileListCount {1}";
                    Result = string.Format(Result, FFileList.Count, FFileListCount);
                }

                // validate that all the items in the FFileList exist
                if (string.IsNullOrEmpty(Result))
                {
                    foreach (string FileName in FFileList)
                    {
                        if (!File.Exists(FileName))
                        {
                            Result = "The file: " + FileName + " does not exist";
                            break;
                        }
                    }
                }

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

        private string BuildOutput(out string ErrorMsg)
        {
            ErrorMsg = "";
            string Result = "";
            string Method = "BuildOutput";

            List<string> ErrorList = new List<string>();

            LogOnInsideMethod(Method);
            try
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);

                StringBuilder entries = new StringBuilder();
                entries.AppendLine(JsonUtilities.StartJson());

                /*
                 * Convert FileName to base64
                 * Get next EAEMAILATTACHMENT_GEN value
                 * insert record for the above gen value
                 * get list of records for the provided runId
                */

                Byte[] bytes = null;
                string fileContents = "";
                int NextId = 0;

                foreach (string FileItem in FFileList)
                {
                    Result = "";
                    bytes = File.ReadAllBytes(FileItem);
                    fileContents = Convert.ToBase64String(bytes);
                    NextId =
                        UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
                            FbConnectionString, "EAEMAILATTACHMENT_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out Result);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = LogOnProcessingError(Method, Result, JsonUtilities.AddPropertyNameAndValue("fileName", FileItem, false));
                    }

                    if (string.IsNullOrEmpty(Result))
                    {
                        Result = AddEmailAttachment(FbConnectionString, NextId, FRunIdValue, FileItem, fileContents);
                        if (!string.IsNullOrEmpty(Result))
                        {
                            Result = LogOnProcessingError(Method, Result, JsonUtilities.AddPropertyNameAndValue("fileName", FileItem, false));
                        }
                    }

                    if (!string.IsNullOrEmpty(Result))
                    {
                        ErrorList.Add(Result);
                    }
                }

                if (ErrorList.Count != 0)
                {
                    entries.AppendLine(JsonUtilities.StartJsonArray("Errors"));
                    entries.Append(string.Join(",", ErrorList.ToArray()));
                    entries.AppendLine(JsonUtilities.EndJsonArray());
                }
                else
                {
                    //StringBuilder entries = new StringBuilder();
                    Result = CommonMethods.BuildEmailAttachmentList(FbConnectionString, FRunIdValue, ref entries);
                }

                entries.AppendLine(JsonUtilities.EndJson());
                Result = entries.ToString();
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

        private string AddEmailAttachment(string ConnectionString,
                                          int NewId,
                                          int RunId,
                                          string FileName,
                                          string Base64FileContents)
        {
            string Result = "";

            string QueryText =
                "INSERT INTO EAEmailAttachments(EA_ID, EA_CompanyClientId, EA_EventRunId, EA_AttachmentName, EA_FileExtension, EA_Source, EA_Base64) " +
                "VALUES (@Id, @CompanyClientId, @EventRunId, @AttachmentName, @FileExtension, @Source, @Base64Content)";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("@Id", NewId));

            Params.Add(new FbParameterItem("@CompanyClientId", CommonItem.CompanyIdValue));
            Params.Add(new FbParameterItem("@EventRunId", RunId));

            string AttachmentName = Path.GetFileNameWithoutExtension(FileName);
            Params.Add(new FbParameterItem("@AttachmentName", AttachmentName));

            string FileExtension = Path.GetExtension(FileName);
            Params.Add(new FbParameterItem("@FileExtension", FileExtension));

            Params.Add(new FbParameterItem("@Source", "Contact-Email"));

            var byteArray = Convert.FromBase64String(Base64FileContents);
            Params.Add(new FbParameterItem("@Base64Content", byteArray));

            WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", FModuleName, MasterId);
            Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", FModuleName, MasterId);
            WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);

            return Result;
        }
    }
}
