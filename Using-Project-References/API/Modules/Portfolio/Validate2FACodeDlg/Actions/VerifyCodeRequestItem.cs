/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 03-03-2021
 * Time: 1:18 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/Validate2FACodeDlg/Actions/Templates/VerifyCodeRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.Validate2FACodeDlg
{
    public class VerifyCodeRequestItem
    {
        #region Properties - Getters and Setters
        public string Code { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            Code = "";
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
                    Key = "Code";
                    if (string.IsNullOrEmpty(Code)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(VerifyCodeRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.Code = Source.Code;
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
            
                    VerifyCodeRequestItem TempItem = new VerifyCodeRequestItem();
                    //Result = CommonMethods.LoadVerifyCodeRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadVerifyCodeRequestItem(ActionBase, ref TempItem);
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
