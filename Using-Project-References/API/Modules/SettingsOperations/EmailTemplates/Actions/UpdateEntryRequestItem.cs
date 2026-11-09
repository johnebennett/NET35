/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 03-09-2020
 * Time: 4:17 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/EmailTemplates/Actions/Templates/UpdateEntryRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations.EmailTemplates
{
    public class UpdateEntryRequestItem
    {
        #region Properties - Getters and Setters
        private string FCategoryIdValue = "";
        public bool CategoryIdIsValid = false;
        public int CategoryId { get; set; }
        public string CategoryIdValue
        {
            get { return FCategoryIdValue; }
            set
            {
                FCategoryIdValue = value;
                int Dummy = 0;
                CategoryIdIsValid = int.TryParse(FCategoryIdValue, out Dummy);
                CategoryId = (CategoryIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOptionIdValue = "";
        public bool OptionIdIsValid = false;
        public int OptionId { get; set; }
        public string OptionIdValue
        {
            get { return FOptionIdValue; }
            set
            {
                FOptionIdValue = value;
                int Dummy = 0;
                OptionIdIsValid = int.TryParse(FOptionIdValue, out Dummy);
                OptionId = (OptionIdIsValid) ? Dummy : 0;
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
        
        private string FTemplateCategoryIdValue = "";
        public bool TemplateCategoryIdIsValid = false;
        public int TemplateCategoryId { get; set; }
        public string TemplateCategoryIdValue
        {
            get { return FTemplateCategoryIdValue; }
            set
            {
                FTemplateCategoryIdValue = value;
                int Dummy = 0;
                TemplateCategoryIdIsValid = int.TryParse(FTemplateCategoryIdValue, out Dummy);
                TemplateCategoryId = (TemplateCategoryIdIsValid) ? Dummy : 0;
            }
        }
        
        public string TemplateName { get; set; }
        
        public string Subject { get; set; }
        
        public string Hide { get; set; }
        
        public string BodyContent { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            CategoryIdValue = "";
            OptionIdValue = "";
            TemplateIdValue = "";
            TemplateCategoryIdValue = "";
            TemplateName = "";
            Subject = "";
            Hide = "";
            BodyContent = "";
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
                    Key = "CategoryId";
                    if (!CategoryIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OptionId";
                    if (!OptionIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TemplateId";
                    if (!TemplateIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TemplateCategoryId";
                    if (!TemplateCategoryIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TemplateName";
                    if (string.IsNullOrEmpty(TemplateName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Subject";
                    if (string.IsNullOrEmpty(Subject)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Hide";
                    if (string.IsNullOrEmpty(Hide)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BodyContent";
                    if (string.IsNullOrEmpty(BodyContent)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UpdateEntryRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.CategoryIdValue = Source.CategoryIdValue;
                this.OptionIdValue = Source.OptionIdValue;
                this.TemplateIdValue = Source.TemplateIdValue;
                this.TemplateCategoryIdValue = Source.TemplateCategoryIdValue;
                this.TemplateName = Source.TemplateName;
                this.Subject = Source.Subject;
                this.Hide = Source.Hide;
                this.BodyContent = Source.BodyContent;
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
            
                    UpdateEntryRequestItem TempItem = new UpdateEntryRequestItem();
                    //Result = CommonMethods.LoadUpdateEntryRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadUpdateEntryRequestItem(ActionBase, ref TempItem);
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
