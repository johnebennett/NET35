/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 05-09-2020
 * Time: 5:03 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/WordTemplates/Actions/Templates/UploadTemplateRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations.WordTemplates
{
    public class UploadTemplateRequestItem
    {
        #region Properties - Getters and Setters
        private string FDisplayCategoryIdValue = "";
        public bool DisplayCategoryIdIsValid = false;
        public int DisplayCategoryId { get; set; }
        public string DisplayCategoryIdValue
        {
            get { return FDisplayCategoryIdValue; }
            set
            {
                FDisplayCategoryIdValue = value;
                int Dummy = 0;
                DisplayCategoryIdIsValid = int.TryParse(FDisplayCategoryIdValue, out Dummy);
                DisplayCategoryId = (DisplayCategoryIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOptionsIdValue = "";
        public bool OptionsIdIsValid = false;
        public int OptionsId { get; set; }
        public string OptionsIdValue
        {
            get { return FOptionsIdValue; }
            set
            {
                FOptionsIdValue = value;
                int Dummy = 0;
                OptionsIdIsValid = int.TryParse(FOptionsIdValue, out Dummy);
                OptionsId = (OptionsIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FTemplateIdValue = "";
        public bool TemplateIdIsValid = false;
        public int TemplateId { get; set; }
        public string TemplateIdValue
        {
            get { return FTemplateIdValue; }
            set
            {
                FTemplateIdValue = value;
                int Dummy = 0;
                TemplateIdIsValid = int.TryParse(FTemplateIdValue, out Dummy);
                TemplateId = (TemplateIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FFileOptionsIdValue = "";
        public bool FileOptionsIdIsValid = false;
        public int FileOptionsId { get; set; }
        public string FileOptionsIdValue
        {
            get { return FFileOptionsIdValue; }
            set
            {
                FFileOptionsIdValue = value;
                int Dummy = 0;
                FileOptionsIdIsValid = int.TryParse(FFileOptionsIdValue, out Dummy);
                FileOptionsId = (FileOptionsIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FDisplayCategory2IdValue = "";
        public bool DisplayCategory2IdIsValid = false;
        public int DisplayCategory2Id { get; set; }
        public string DisplayCategory2IdValue
        {
            get { return FDisplayCategory2IdValue; }
            set
            {
                FDisplayCategory2IdValue = value;
                int Dummy = 0;
                DisplayCategory2IdIsValid = int.TryParse(FDisplayCategory2IdValue, out Dummy);
                DisplayCategory2Id = (DisplayCategory2IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FFileSizeValue = "";
        public bool FileSizeIsValid = false;
        public long FileSize { get; set; }
        public string FileSizeValue
        {
            get { return FFileSizeValue; }
            set
            {
                FFileSizeValue = value;
                long Dummy = 0;
                FileSizeIsValid = long.TryParse(FFileSizeValue, out Dummy);
                FileSize = (FileSizeIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            DisplayCategoryIdValue = "";
            OptionsIdValue = "";
            TemplateIdValue = "";
            FileOptionsIdValue = "";
            DisplayCategory2IdValue = "";
            FileSizeValue = "";
        }
        #endregion
        
        #region Validate
        public string Validate()
        {
            string Result = "";
            try
            {
                string BaseFormat = "{0} is {1}";
                string NotProvided = "Not Provided";
                string Invalid = "Invalid";
                string Key = "";
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DisplayCategoryId";
                    if (!DisplayCategoryIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OptionsId";
                    if (!OptionsIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TemplateId";
                    if (!TemplateIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FileOptionsId";
                    if (!FileOptionsIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DisplayCategory2Id";
                    if (!DisplayCategory2IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FileSize";
                }
                
            }
            catch (Exception Ex)
            {
                Result =
                    Ex.Message + Environment.NewLine +
                    Ex.InnerException;
                }
            
            return Result;
        }
        #endregion
        
        #region Update
        public string Update(UploadTemplateRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.DisplayCategoryIdValue = Source.DisplayCategoryIdValue;
                this.OptionsIdValue = Source.OptionsIdValue;
                this.TemplateIdValue = Source.TemplateIdValue;
                this.FileOptionsIdValue = Source.FileOptionsIdValue;
                this.DisplayCategory2IdValue = Source.DisplayCategory2IdValue;
                this.FileSize = Source.FileSize;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
        //public string Load(ActionBase ActionBase, JObject DataObject)
        public string Load(ActionBase ActionBase)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                //string Method = ClassName + ".LoadFromDataObject";
                string Method = ClassName + ".LoadFromActionBase";
            
                ActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    UploadTemplateRequestItem TempItem = new UploadTemplateRequestItem();
                    //Result = CommonMethods.LoadUploadTemplateRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadUploadTemplateRequestItem(ActionBase, ref TempItem);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = ActionBase.LogOnProcessingError(Method, Result, "");
                    }
                    else
                    {
                        Result = Update(TempItem);
                    }
                }
                catch (Exception Ex)
                {
                    Result = ActionBase.LogOnException(Method, Ex, "");
                    return Result;
                }
                finally
                {
                    ActionBase.LogOnLeavingMethod(Method);
                }
            
                return Result;
        }
        #endregion
        
    }
}
