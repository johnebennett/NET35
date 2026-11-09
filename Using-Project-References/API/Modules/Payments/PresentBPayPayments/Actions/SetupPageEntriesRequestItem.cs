/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 06-09-2020
 * Time: 4:31 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Payments/PresentBPayPayments/Actions/Templates/SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Payments.PresentBPayPayments
{
    public class SetupPageEntriesRequestItem
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
        
        private string FRunIdBankIdValue = "";
        public bool RunIdBankIdIsValid = false;
        public int RunIdBankId { get; set; }
        public string RunIdBankIdValue
        {
            get { return FRunIdBankIdValue; }
            set
            {
                FRunIdBankIdValue = value;
                int Dummy = 0;
                RunIdBankIdIsValid = int.TryParse(FRunIdBankIdValue, out Dummy);
                RunIdBankId = (RunIdBankIdIsValid) ? Dummy : 0;
            }
        }
        
        public string PresentDate { get; set; }
        
        private string FPageNoValue = "";
        public bool PageNoIsValid = false;
        public int PageNo { get; set; }
        public string PageNoValue
        {
            get { return FPageNoValue; }
            set
            {
                FPageNoValue = value;
                int Dummy = 0;
                PageNoIsValid = int.TryParse(FPageNoValue, out Dummy);
                PageNo = (PageNoIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            RunIdValue = "";
            RunIdBankIdValue = "";
            PresentDate = "";
            PageNoValue = "";
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
                    Key = "RunIdBankId";
                    if (!RunIdBankIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PresentDate";
                    if (string.IsNullOrEmpty(PresentDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PageNo";
                    if (!PageNoIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SetupPageEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.RunIdValue = Source.RunIdValue;
                this.RunIdBankIdValue = Source.RunIdBankIdValue;
                this.PresentDate = Source.PresentDate;
                this.PageNoValue = Source.PageNoValue;
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
            
                    SetupPageEntriesRequestItem TempItem = new SetupPageEntriesRequestItem();
                    //Result = CommonMethods.LoadSetupPageEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSetupPageEntriesRequestItem(ActionBase, ref TempItem);
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
