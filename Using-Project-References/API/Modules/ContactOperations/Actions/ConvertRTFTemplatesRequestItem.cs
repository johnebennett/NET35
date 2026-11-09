/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 10-10-2020
 * Time: 10:47 AM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/ContactOperations/Actions/Templates/ConvertRTFTemplatesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations
{
    public class ConvertRTFTemplatesRequestItem
    {
        #region Properties - Getters and Setters
        private string FCompanyIdValue = "";
        public bool CompanyIdIsValid = false;
        public int CompanyId { get; set; }
        public string CompanyIdValue
        {
            get { return FCompanyIdValue; }
            set
            {
                FCompanyIdValue = value;
                int Dummy = 0;
                CompanyIdIsValid = int.TryParse(FCompanyIdValue, out Dummy);
                CompanyId = (CompanyIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            CompanyIdValue = "";
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
                    Key = "CompanyId";
                    if (!CompanyIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(ConvertRTFTemplatesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.CompanyIdValue = Source.CompanyIdValue;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
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
            
                    ConvertRTFTemplatesRequestItem TempItem = new ConvertRTFTemplatesRequestItem();
                    //Result = CommonMethods.LoadConvertRTFTemplatesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadConvertRTFTemplatesRequestItem(ActionBase, ref TempItem);
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
