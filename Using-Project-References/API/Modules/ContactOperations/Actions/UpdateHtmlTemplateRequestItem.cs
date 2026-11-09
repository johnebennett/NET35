/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 17-10-2020
 * Time: 6:11 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/ContactOperations/Actions/Templates/UpdateHtmlTemplateRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations
{
    public class UpdateHtmlTemplateRequestItem
    {
        #region Properties - Getters and Setters
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
        
        public string EmailBody { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            TemplateIdValue = "";
            EmailBody = "";
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
                    Key = "TemplateId";
                    if (!TemplateIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EmailBody";
                    if (string.IsNullOrEmpty(EmailBody)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UpdateHtmlTemplateRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.TemplateIdValue = Source.TemplateIdValue;
                this.EmailBody = Source.EmailBody;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
        //public string Load(ActionBase AActionBase, JObject DataObject)
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
            
                    UpdateHtmlTemplateRequestItem TempItem = new UpdateHtmlTemplateRequestItem();
                    //Result = CommonMethods.LoadUpdateHtmlTemplateRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadUpdateHtmlTemplateRequestItem(ActionBase, ref TempItem);
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
