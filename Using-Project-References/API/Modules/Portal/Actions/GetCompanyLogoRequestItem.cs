/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 12-06-2024
 * Time: 5:20 PM

 * SourceFile: E:\Source\EAOffice\EAOfficeAPI\Modules\Portal\Actions\Templates\GetCompanyLogoRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Portal
{
    public class GetCompanyLogoRequestItem
    {
        #region Properties - Getters and Setters
        private string FForCompanyIdValue = "";
        public bool ForCompanyIdIsValid = false;
        public int ForCompanyId { get; set; }
        public string ForCompanyIdValue
        {
            get { return FForCompanyIdValue; }
            set
            {
                FForCompanyIdValue = value;
                int Dummy = 0;
                ForCompanyIdIsValid = int.TryParse(FForCompanyIdValue, out Dummy);
                ForCompanyId = (ForCompanyIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            ForCompanyIdValue = "";
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
                    Key = "ForCompanyId";
                    if (!ForCompanyIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(GetCompanyLogoRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.ForCompanyIdValue = Source.ForCompanyIdValue;
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
            
                    GetCompanyLogoRequestItem TempItem = new GetCompanyLogoRequestItem();
                    //Result = CommonMethods.LoadGetCompanyLogoRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadGetCompanyLogoRequestItem(ActionBase, ref TempItem);
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
