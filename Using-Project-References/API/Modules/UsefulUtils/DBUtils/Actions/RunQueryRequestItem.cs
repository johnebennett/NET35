/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 29-06-2021
 * Time: 6:56 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/UsefulUtils/DBUtils/Actions/Templates/RunQueryRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.UsefulUtils.DBUtils
{
    public class RunQueryRequestItem
    {
        #region Properties - Getters and Setters
        public string Source { get; set; }
        
        public string UseParams { get; set; }
        
        public string Output { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            Source = "";
            UseParams = "";
            Output = "";
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
                    Key = "Source";
                    if (string.IsNullOrEmpty(Source)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "UseParams";
                    if (string.IsNullOrEmpty(UseParams)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Output";
                    if (string.IsNullOrEmpty(Output)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(RunQueryRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.Source = Source.Source;
                this.UseParams = Source.UseParams;
                this.Output = Source.Output;
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
            
                    RunQueryRequestItem TempItem = new RunQueryRequestItem();
                    //Result = CommonMethods.LoadRunQueryRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadRunQueryRequestItem(ActionBase, ref TempItem);
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
