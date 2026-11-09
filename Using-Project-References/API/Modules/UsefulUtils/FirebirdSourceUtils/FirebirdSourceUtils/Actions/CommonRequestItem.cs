/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 17-09-2021
 * Time: 4:15 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/UsefulUtils/FirebirdSourceUtils/FirebirdSourceUtils/Actions/Templates/CommonRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.UsefulUtils.FirebirdSourceUtils
{
    public class CommonRequestItem
    {
        #region Properties - Getters and Setters
        public string Action { get; set; }
        
        public string SourceObject { get; set; }
        
        public string SourceName { get; set; }
        
        public string OutputFileName { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            Action = "";
            SourceObject = "";
            OutputFileName = "";
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
                    Key = "Action";
                    if (string.IsNullOrEmpty(Action)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SourceObject";
                    if (string.IsNullOrEmpty(SourceObject)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OutputFileName";
                    if (string.IsNullOrEmpty(OutputFileName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(CommonRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.Action = Source.Action;
                this.SourceObject = Source.SourceObject;
                this.SourceName = Source.SourceName;
                this.OutputFileName = Source.OutputFileName;
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
            
                    CommonRequestItem TempItem = new CommonRequestItem();
                    //Result = CommonMethods.LoadCommonRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadCommonRequestItem(ActionBase, ref TempItem);
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
