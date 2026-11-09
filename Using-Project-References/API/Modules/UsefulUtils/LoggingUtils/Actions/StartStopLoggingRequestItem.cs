/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 28-06-2021
 * Time: 10:47 AM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/UsefulUtils/LoggingUtils/Actions/Templates/StartStopLogging.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.UsefulUtils.LoggingUtils
{
    public class StartStopLoggingRequestItem
    {
        #region Properties - Getters and Setters
        public string Action { get; set; }
        
        public string LoggingEntries { get; set; }
        
        public string EventViewer { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            Action = "";
            LoggingEntries = "";
            EventViewer = "";
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
                    Key = "LoggingEntries";
                    if (string.IsNullOrEmpty(LoggingEntries)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EventViewer";
                    if (string.IsNullOrEmpty(EventViewer)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(StartStopLoggingRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.Action = Source.Action;
                this.LoggingEntries = Source.LoggingEntries;
                this.EventViewer = Source.EventViewer;
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
            
                    StartStopLoggingRequestItem TempItem = new StartStopLoggingRequestItem();
                    //Result = CommonMethods.LoadStartStopLoggingRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadStartStopLoggingRequestItem(ActionBase, ref TempItem);
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
