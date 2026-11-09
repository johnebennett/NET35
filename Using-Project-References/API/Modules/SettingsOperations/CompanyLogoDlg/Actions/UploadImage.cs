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
namespace Actions.SettingsOperations.CompanyLogoDlg
{
    public class UploadImage : ActionBase
    {
        private string FModuleName = "Portfolio.AddPropertyImagesDlg";
        private UploadImageRequestItem FRequestItem;
        private List<string> FFileList = new List<string>();
        private int FFileListCount = 0;
        private string FBaseDir = "";

        public static string Execute(int MasterId,
                                     APISettingsItem SettingsItem,
                                     string Request,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            UploadImage inst = new UploadImage();
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
                FRequestItem = new UploadImageRequestItem();
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
                string RootDir = "";
                string SourceFolder = "";

                if (string.IsNullOrEmpty(Result))
                {
                    DocsLocation =
                        global::UsefulDBMethods.CompanyFolderSettings.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
                    if (!DocsLocation.EndsWith(DirectorySepChar)) DocsLocation += DirectorySepChar;
                    SourceFolder = DocsLocation + CommonItem.CompanyId + DirectorySepChar;
                    RootDir = Directory.GetParent(DocsLocation).FullName;
                    RootDir = Directory.GetParent(RootDir).FullName;
                    if (!RootDir.EndsWith(DirectorySepChar)) RootDir += DirectorySepChar;
                    WindowsEventLoggingUtilities.WriteToInfoEvent(
                        "RootDir: " + RootDir + Environment.NewLine + "SourceDir: " + SourceFolder, FModuleName, MasterId);
                    FBaseDir = RootDir;
                    if (!FBaseDir.EndsWith(DirectorySepChar)) FBaseDir += DirectorySepChar;
                    FBaseDir += CommonItem.CompanyId;
                    if (!FBaseDir.EndsWith(DirectorySepChar)) FBaseDir += DirectorySepChar;
                    /*
                    FBaseDir = Directory.GetParent(FBaseDir).FullName;
                    if (!FBaseDir.EndsWith(DirectorySepChar)) FBaseDir += DirectorySepChar;
                    */
                    WindowsEventLoggingUtilities.WriteToInfoEvent("BaseDir: " + FBaseDir, FModuleName, MasterId);
                }

                JArray FilesObject = (JArray)DataObject["files"];
                if (FilesObject == null)
                {
                    Result = "FilesObject IS NULL";
                }

                string CurrentFileName = "";
                string FileNamePrefix =
                            CommonItem.CompanyId + "_" + CommonItem.ClientId + "_" + FRequestItem.Source + "_";
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
                                FFileList.Add(SourceFolder + CurrentFileName);
                            }
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
                        CommonMethods.UploadImage(
                            FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
                            FRequestItem, FFileList);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    /*
                    Result =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        JsonUtilities.EndJson();
                    */
                    Result = LogResponse(Method, "");
                }
                else
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