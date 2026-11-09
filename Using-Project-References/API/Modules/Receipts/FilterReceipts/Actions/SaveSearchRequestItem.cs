/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 08-09-2020
 * Time: 10:41 AM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Receipts/FilterReceipts/Actions/Templates/SaveSearchRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Receipts.FilterReceipts
{
    public class SaveSearchRequestItem
    {
        #region Properties - Getters and Setters
        private string FMasterEventIdValue = "";
        public bool MasterEventIdIsValid = false;
        public int MasterEventId { get; set; }
        public string MasterEventIdValue
        {
            get { return FMasterEventIdValue; }
            set
            {
                FMasterEventIdValue = value;
                int Dummy = 0;
                MasterEventIdIsValid = int.TryParse(FMasterEventIdValue, out Dummy);
                MasterEventId = (MasterEventIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FBranchIdValue = "";
        public bool BranchIdIsValid = false;
        public int BranchId { get; set; }
        public string BranchIdValue
        {
            get { return FBranchIdValue; }
            set
            {
                FBranchIdValue = value;
                int Dummy = 0;
                BranchIdIsValid = int.TryParse(FBranchIdValue, out Dummy);
                BranchId = (BranchIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FManagerIdValue = "";
        public bool ManagerIdIsValid = false;
        public int ManagerId { get; set; }
        public string ManagerIdValue
        {
            get { return FManagerIdValue; }
            set
            {
                FManagerIdValue = value;
                int Dummy = 0;
                ManagerIdIsValid = int.TryParse(FManagerIdValue, out Dummy);
                ManagerId = (ManagerIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FBankAccountIdValue = "";
        public bool BankAccountIdIsValid = false;
        public int BankAccountId { get; set; }
        public string BankAccountIdValue
        {
            get { return FBankAccountIdValue; }
            set
            {
                FBankAccountIdValue = value;
                int Dummy = 0;
                BankAccountIdIsValid = int.TryParse(FBankAccountIdValue, out Dummy);
                BankAccountId = (BankAccountIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FRevenueIdValue = "";
        public bool RevenueIdIsValid = false;
        public int RevenueId { get; set; }
        public string RevenueIdValue
        {
            get { return FRevenueIdValue; }
            set
            {
                FRevenueIdValue = value;
                int Dummy = 0;
                RevenueIdIsValid = int.TryParse(FRevenueIdValue, out Dummy);
                RevenueId = (RevenueIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FLedgerTypeIdValue = "";
        public bool LedgerTypeIdIsValid = false;
        public int LedgerTypeId { get; set; }
        public string LedgerTypeIdValue
        {
            get { return FLedgerTypeIdValue; }
            set
            {
                FLedgerTypeIdValue = value;
                int Dummy = 0;
                LedgerTypeIdIsValid = int.TryParse(FLedgerTypeIdValue, out Dummy);
                LedgerTypeId = (LedgerTypeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FAddedByIdValue = "";
        public bool AddedByIdIsValid = false;
        public int AddedById { get; set; }
        public string AddedByIdValue
        {
            get { return FAddedByIdValue; }
            set
            {
                FAddedByIdValue = value;
                int Dummy = 0;
                AddedByIdIsValid = int.TryParse(FAddedByIdValue, out Dummy);
                AddedById = (AddedByIdIsValid) ? Dummy : 0;
            }
        }
        
        public string ReceiptDateFrom { get; set; }
        
        public string ReceiptDateTo { get; set; }
        
        private string FReceiptNumberFromValue = "";
        public bool ReceiptNumberFromIsValid = false;
        public int ReceiptNumberFrom { get; set; }
        public string ReceiptNumberFromValue
        {
            get { return FReceiptNumberFromValue; }
            set
            {
                FReceiptNumberFromValue = value;
                int Dummy = 0;
                ReceiptNumberFromIsValid = int.TryParse(FReceiptNumberFromValue, out Dummy);
                ReceiptNumberFrom = (ReceiptNumberFromIsValid) ? Dummy : 0;
            }
        }
        
        private string FReceiptNumberToValue = "";
        public bool ReceiptNumberToIsValid = false;
        public int ReceiptNumberTo { get; set; }
        public string ReceiptNumberToValue
        {
            get { return FReceiptNumberToValue; }
            set
            {
                FReceiptNumberToValue = value;
                int Dummy = 0;
                ReceiptNumberToIsValid = int.TryParse(FReceiptNumberToValue, out Dummy);
                ReceiptNumberTo = (ReceiptNumberToIsValid) ? Dummy : 0;
            }
        }
        
        public string EnteredDateFrom { get; set; }
        
        public string EnteredDateTo { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            MasterEventIdValue = "";
            BranchIdValue = "";
            ManagerIdValue = "";
            BankAccountIdValue = "";
            RevenueIdValue = "";
            LedgerTypeIdValue = "";
            AddedByIdValue = "";
            ReceiptDateFrom = "";
            ReceiptDateTo = "";
            ReceiptNumberFromValue = "";
            ReceiptNumberToValue = "";
            EnteredDateFrom = "";
            EnteredDateTo = "";
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
                    Key = "MasterEventId";
                    if (!MasterEventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BranchId";
                    if (!BranchIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ManagerId";
                    if (!ManagerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankAccountId";
                    if (!BankAccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RevenueId";
                    if (!RevenueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LedgerTypeId";
                    if (!LedgerTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AddedById";
                    if (!AddedByIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptDateFrom";
                    if (string.IsNullOrEmpty(ReceiptDateFrom)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptDateTo";
                    if (string.IsNullOrEmpty(ReceiptDateTo)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptNumberFrom";
                    if (!ReceiptNumberFromIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptNumberTo";
                    if (!ReceiptNumberToIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EnteredDateFrom";
                    if (string.IsNullOrEmpty(EnteredDateFrom)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EnteredDateTo";
                    if (string.IsNullOrEmpty(EnteredDateTo)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SaveSearchRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.MasterEventIdValue = Source.MasterEventIdValue;
                this.BranchIdValue = Source.BranchIdValue;
                this.ManagerIdValue = Source.ManagerIdValue;
                this.BankAccountIdValue = Source.BankAccountIdValue;
                this.RevenueIdValue = Source.RevenueIdValue;
                this.LedgerTypeIdValue = Source.LedgerTypeIdValue;
                this.AddedByIdValue = Source.AddedByIdValue;
                this.ReceiptDateFrom = Source.ReceiptDateFrom;
                this.ReceiptDateTo = Source.ReceiptDateTo;
                this.ReceiptNumberFromValue = Source.ReceiptNumberFromValue;
                this.ReceiptNumberToValue = Source.ReceiptNumberToValue;
                this.EnteredDateFrom = Source.EnteredDateFrom;
                this.EnteredDateTo = Source.EnteredDateTo;
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
            
                    SaveSearchRequestItem TempItem = new SaveSearchRequestItem();
                    //Result = CommonMethods.LoadSaveSearchRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSaveSearchRequestItem(ActionBase, ref TempItem);
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
