/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 12-02-2020
 * Time: 4:59 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\ComboBoxEntries\Actions\Templates\MgmtPdfReportListRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.ComboBoxEntries
{
    public class MgmtPdfReportListRequestItem
    {
        #region Properties - Getters and Setters
        private string FTransactionIdValue = "";
        public bool TransactionIdIsValid = false;
        public int TransactionId { get; set; }
        public string TransactionIdValue
        {
            get { return FTransactionIdValue; }
            set
            {
                FTransactionIdValue = value;
                int Dummy = 0;
                TransactionIdIsValid = int.TryParse(FTransactionIdValue, out Dummy);
                TransactionId = (TransactionIdIsValid) ? Dummy : 0;
            }
        }
        
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
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            TransactionIdValue = "";
            AccountIdValue = "";
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
                    Key = "TransactionId";
                    if (!TransactionIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AccountId";
                    if (!AccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(MgmtPdfReportListRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.TransactionIdValue = Source.TransactionIdValue;
                this.AccountIdValue = Source.AccountIdValue;
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
                string Method = ClassName + ".LoadFromDataObject";
            
                ActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    MgmtPdfReportListRequestItem TempItem = new MgmtPdfReportListRequestItem();
                    Result = CommonMethods.LoadMgmtPdfReportListRequestItem(ActionBase, ref TempItem);
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
