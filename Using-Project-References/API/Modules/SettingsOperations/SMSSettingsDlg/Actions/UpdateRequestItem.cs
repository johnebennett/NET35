/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 05-01-2021
 * Time: 4:59 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/SMSSettingsDlg/Actions/Templates/UpdateRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations.SMSSettingsDlg
{
    public class UpdateRequestItem
    {
        #region Properties - Getters and Setters
        private string FSMSSupplierIdValue = "";
        public bool SMSSupplierIdIsValid = false;
        public int SMSSupplierId { get; set; }
        public string SMSSupplierIdValue
        {
            get { return FSMSSupplierIdValue; }
            set
            {
                FSMSSupplierIdValue = value;
                int Dummy = 0;
                SMSSupplierIdIsValid = int.TryParse(FSMSSupplierIdValue, out Dummy);
                SMSSupplierId = (SMSSupplierIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FMaxSMSLengthValue = "";
        public bool MaxSMSLengthIsValid = false;
        public int MaxSMSLength { get; set; }
        public string MaxSMSLengthValue
        {
            get { return FMaxSMSLengthValue; }
            set
            {
                FMaxSMSLengthValue = value;
                int Dummy = 0;
                MaxSMSLengthIsValid = int.TryParse(FMaxSMSLengthValue, out Dummy);
                MaxSMSLength = (MaxSMSLengthIsValid) ? Dummy : 0;
            }
        }
        
        public string DefaultToEmail { get; set; }
        
        public string DefaultFromEmail { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            SMSSupplierIdValue = "";
            MaxSMSLengthValue = "";
            DefaultToEmail = "";
            DefaultFromEmail = "";
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
                    Key = "SMSSupplierId";
                    if (!SMSSupplierIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MaxSMSLength";
                    if (!MaxSMSLengthIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DefaultToEmail";
                    if (string.IsNullOrEmpty(DefaultToEmail)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DefaultFromEmail";
                    if (string.IsNullOrEmpty(DefaultFromEmail)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UpdateRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.SMSSupplierIdValue = Source.SMSSupplierIdValue;
                this.MaxSMSLengthValue = Source.MaxSMSLengthValue;
                this.DefaultToEmail = Source.DefaultToEmail;
                this.DefaultFromEmail = Source.DefaultFromEmail;
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
            
                    UpdateRequestItem TempItem = new UpdateRequestItem();
                    //Result = CommonMethods.LoadUpdateRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadUpdateRequestItem(ActionBase, ref TempItem);
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
