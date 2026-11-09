/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 14-06-2021
 * Time: 5:02 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portal/Actions/Templates/ValidateUserRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Portal
{
    public class ValidateUserRequestItem
    {
        #region Properties - Getters and Setters
        public string Password { get; set; }
        
        public string AuthCode { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            Password = "";
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
                    Key = "Password";
                    if (string.IsNullOrEmpty(Password)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(ValidateUserRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.Password = Source.Password;
                this.AuthCode = Source.AuthCode;
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
            
                    ValidateUserRequestItem TempItem = new ValidateUserRequestItem();
                    //Result = CommonMethods.LoadValidateUserRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadValidateUserRequestItem(ActionBase, ref TempItem);
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
