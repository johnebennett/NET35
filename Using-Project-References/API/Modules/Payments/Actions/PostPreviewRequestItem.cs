/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 04-11-2020
 * Time: 2:25 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Payments/Actions/Templates/PostPreviewRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PaymentOperations
{
    public class PostPreviewRequestItem
    {
        #region Properties - Getters and Setters
        private string FRunIdValue = "";
        public bool RunIdIsValid = false;
        public int RunId { get; set; }
        public string RunIdValue
        {
            get { return FRunIdValue; }
            set
            {
                FRunIdValue = value;
                int Dummy = 0;
                RunIdIsValid = int.TryParse(FRunIdValue, out Dummy);
                RunId = (RunIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FChqNumberValue = "";
        public bool ChqNumberIsValid = false;
        public int ChqNumber { get; set; }
        public string ChqNumberValue
        {
            get { return FChqNumberValue; }
            set
            {
                FChqNumberValue = value;
                int Dummy = 0;
                ChqNumberIsValid = int.TryParse(FChqNumberValue, out Dummy);
                ChqNumber = (ChqNumberIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            RunIdValue = "";
            ChqNumberValue = "";
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
                    Key = "RunId";
                    if (!RunIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChqNumber";
                    if (!ChqNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(PostPreviewRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.RunIdValue = Source.RunIdValue;
                this.ChqNumberValue = Source.ChqNumberValue;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
        public string Load(ActionBase AActionBase, JObject DataObject)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".LoadFromDataObject";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    PostPreviewRequestItem TempItem = new PostPreviewRequestItem();
                    Result = CommonMethods.LoadPostPreviewRequestItem(DataObject, ref TempItem);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = AActionBase.LogOnProcessingError(Method, Result, "");
                    }
                    else
                    {
                        Result = Update(TempItem);
                    }
                }
                catch (Exception Ex)
                {
                    Result = AActionBase.LogOnException(Method, Ex, "");
                    return Result;
                }
                finally
                {
                    AActionBase.LogOnLeavingMethod(Method);
                }
            
                return Result;
        }
        #endregion
        
    }
}
