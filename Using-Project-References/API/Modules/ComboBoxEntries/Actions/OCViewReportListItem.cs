/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 27-11-2019
 * Time: 10:59 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\ComboBoxEntries\Actions\Templates\OCViewReportListItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.ComboBoxEntries
{
    public class OCViewReportListItem
    {
        #region Properties - Getters and Setters
        private string FAccountIdValue = "";
        public bool AccountIdIsValid = false;
        public int AccountId { get; set; }
        public string AccountIdValue
        {
            get { return FAccountIdValue; }
            set
            {
                FAccountIdValue = value;
                int Dummy = 0;
                AccountIdIsValid = int.TryParse(FAccountIdValue, out Dummy);
                AccountId = (AccountIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FSubAccountIdValue = "";
        public bool SubAccountIdIsValid = false;
        public int SubAccountId { get; set; }
        public string SubAccountIdValue
        {
            get { return FSubAccountIdValue; }
            set
            {
                FSubAccountIdValue = value;
                int Dummy = 0;
                SubAccountIdIsValid = int.TryParse(FSubAccountIdValue, out Dummy);
                SubAccountId = (SubAccountIdIsValid) ? Dummy : 0;
            }
        }
        
        public string DateFrom { get; set; }
        
        public string DateTo { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            AccountIdValue = "";
            SubAccountIdValue = "";
            DateFrom = "";
            DateTo = "";
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
                    Key = "AccountId";
                    if (!AccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SubAccountId";
                    if (!SubAccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DateFrom";
                    if (string.IsNullOrEmpty(DateFrom)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DateTo";
                    if (string.IsNullOrEmpty(DateTo)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(OCViewReportListItem Source)
        {
            string Result = "";
            
            try
            {
                this.AccountIdValue = Source.AccountIdValue;
                this.SubAccountIdValue = Source.SubAccountIdValue;
                this.DateFrom = Source.DateFrom;
                this.DateTo = Source.DateTo;
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
            
                    OCViewReportListItem TempItem = new OCViewReportListItem();
                    Result = CommonMethods.LoadOCViewReportListItem(DataObject, ref TempItem);
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
