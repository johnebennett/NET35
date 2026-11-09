using System;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.SettingsOperations.SignatureDlg
{
    public class UploadSignature : ActionBase
    {
        private string FModuleName = "SettingsOperations.SignatureDlg";
        private SetupPageEntriesRequestItem FRequestItem;
        private List<string> FSrcFileList = new List<string>();
        private List<string> FDestFileList = new List<string>();
        private int FFileListCount = 0;
        private string FBaseDir = "";
        private string FSignatureSource = "";
        private string FSourceDir = "";
        private string FDestDir = "";

        public static string Execute(int MasterId,
                                     APISettingsItem SettingsItem,
                                     string Request,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            UploadSignature inst = new UploadSignature();
            if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
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
                UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
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
                FRequestItem = new SetupPageEntriesRequestItem();
                Result = FRequestItem.Load((ActionBase)this);

                if (string.IsNullOrEmpty(Result))
                {
                    Result = InitFilesItems();
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

        private string InitFilesItems()
        {
            string Result = "";
            string Method = "InitFilesItems";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);

            LogOnInsideMethod(Method);
            try
            {
                string DirectorySepChar = Path.DirectorySeparatorChar.ToString();
                string DocsLocation = "";
                string SourceFolder = "";

                if (string.IsNullOrEmpty(Result))
                {
                    if (FRequestItem.SignatureSource == 1) FSignatureSource = "Word";
                    else if (FRequestItem.SignatureSource == 2) FSignatureSource = "Email";
                    else FSignatureSource = "Unknown";
                    FSignatureSource += "_Signature";

                    FDestDir =
                        CommonUtilities.FolderHelpers.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
                    if (!FDestDir.EndsWith(DirectorySepChar)) DocsLocation += DirectorySepChar;
                    WindowsEventLoggingUtilities.WriteToInfoEvent("DestDir: " + FDestDir, Method, 0);
                    FSourceDir = Directory.GetParent(FDestDir).FullName;
                    FSourceDir = Directory.GetParent(FSourceDir).FullName;
                    if (!FSourceDir.EndsWith(DirectorySepChar)) FSourceDir += DirectorySepChar;
                    FSourceDir = FSourceDir + "Uploads" + DirectorySepChar;
                    WindowsEventLoggingUtilities.WriteToInfoEvent(
                        "SourceDir: " + FSourceDir + Environment.NewLine + "DestDir: " + FDestDir, FModuleName, MasterId);
                }

                JArray FilesObject = (JArray)DataObject["files"];
                if (FilesObject == null)
                {
                    Result = "FilesObject IS NULL";
                }

                string CurrentFileName = "";
                string FileNamePrefix =
                            CommonItem.CompanyId + "_" + CommonItem.ClientId + "_" + FSignatureSource + "_";
                if (string.IsNullOrEmpty(Result))
                {
                    FFileListCount = FilesObject.Count;

                    if (FilesObject.Count > 0)
                    {
                        foreach (JObject Item in FilesObject)
                        {
                            if ((Item != null) && (Item["file"] != null))
                            {
                                CurrentFileName = (string)Item["file"];
                                if (CurrentFileName.StartsWith("Uploads\\")) CurrentFileName = CurrentFileName.Replace("Uploads\\", "");
                                CurrentFileName = FileNamePrefix + CurrentFileName;
                                WindowsEventLoggingUtilities.WriteToInfoEvent("FileName: " + SourceFolder + CurrentFileName, Method, 0);
                                FSrcFileList.Add(FSourceDir + CurrentFileName);
                                FDestFileList.Add(FDestDir + CurrentFileName);
                                File.Copy(FSourceDir + CurrentFileName, FDestDir + CurrentFileName, true);
                                Result = 
                                    (File.Exists(FDestDir + CurrentFileName)) ? "" : "File: " + FDestDir + CurrentFileName + " does not exist";
                                if (!string.IsNullOrEmpty(Result)) break;
                                //if (File.Exists(FSourceDir + CurrentFileName)) File.Delete(FSourceDir + CurrentFileName);
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(Result))
                    if (FDestFileList.Count != FSrcFileList.Count) Result = "Source and Destination file list is different";
                if (string.IsNullOrEmpty(Result))
                    if (FDestFileList.Count != FFileListCount) Result = "Destination file list does not match input list";
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
                Result = FRequestItem.Validate();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = ValidateFilesItems();
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

        private string ValidateFilesItems()
        {
            string Result = "";
            string Method = "ValidateFilesItems";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);

            LogOnInsideMethod(Method);
            try
            {
                if (string.IsNullOrEmpty(Result))
                {
                }
                if (string.IsNullOrEmpty(Result) && (FSrcFileList.Count == 0))
                {
                    Result = "There are no files to process";
                }
                else if (string.IsNullOrEmpty(Result) && (FSrcFileList.Count != FFileListCount))
                {
                    Result = "FFileList.Count ({0}) != FFileListCount {1}";
                    Result = string.Format(Result, FSrcFileList.Count, FFileListCount);
                }

                // validate that all the items in the FFileList exist
                if (string.IsNullOrEmpty(Result))
                {
                    foreach (string FileName in FSrcFileList)
                    {
                        if (!File.Exists(FileName))
                        {
                            Result = "The file: " + FileName + " does not exist";
                            break;
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

        private string Output()
        {
            string Method = "Output";
            string Result = "";
            string ErrorMsg = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                StringBuilder Entries = new StringBuilder();
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg =
                        CommonMethods.UploadSignature(
                            FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
                            FRequestItem, FSignatureSource, FDestDir, FSrcFileList, FDestFileList);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Result = LogResponse(Method, "");
                }

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
                WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
            }
            return Result;
        }
    }
}