/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 16-01-2020
 * Time: 9:18 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Invoices\Actions\Templates\InvoicesReverseTransferRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Invoices
{
    public class InvoicesReverseTransferRequestItem
    {
        #region Properties - Getters and Setters
        private string FBankIdValue = "";
        public bool BankIdIsValid = false;
        public int BankId { get; set; }
        public string BankIdValue
        {
            get { return FBankIdValue; }
            set
            {
                FBankIdValue = value;
                int Dummy = 0;
                BankIdIsValid = int.TryParse(FBankIdValue, out Dummy);
                BankId = (BankIdIsValid) ? Dummy : 0;
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
        
        private string FDisplayTypeValue = "";
        public bool DisplayTypeIsValid = false;
        public int DisplayType { get; set; }
        public string DisplayTypeValue
        {
            get { return FDisplayTypeValue; }
            set
            {
                FDisplayTypeValue = value;
                int Dummy = 0;
                DisplayTypeIsValid = int.TryParse(FDisplayTypeValue, out Dummy);
                DisplayType = (DisplayTypeIsValid) ? Dummy : 0;
            }
        }
        
        private string FExpenseIdValue = "";
        public bool ExpenseIdIsValid = false;
        public int ExpenseId { get; set; }
        public string ExpenseIdValue
        {
            get { return FExpenseIdValue; }
            set
            {
                FExpenseIdValue = value;
                int Dummy = 0;
                ExpenseIdIsValid = int.TryParse(FExpenseIdValue, out Dummy);
                ExpenseId = (ExpenseIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FAssignedToIdValue = "";
        public bool AssignedToIdIsValid = false;
        public int AssignedToId { get; set; }
        public string AssignedToIdValue
        {
            get { return FAssignedToIdValue; }
            set
            {
                FAssignedToIdValue = value;
                int Dummy = 0;
                AssignedToIdIsValid = int.TryParse(FAssignedToIdValue, out Dummy);
                AssignedToId = (AssignedToIdIsValid) ? Dummy : 0;
            }
        }
        
        public string DateFrom { get; set; }
        
        public string DateTo { get; set; }
        
        private string FCreditorIdValue = "";
        public bool CreditorIdIsValid = false;
        public int CreditorId { get; set; }
        public string CreditorIdValue
        {
            get { return FCreditorIdValue; }
            set
            {
                FCreditorIdValue = value;
                int Dummy = 0;
                CreditorIdIsValid = int.TryParse(FCreditorIdValue, out Dummy);
                CreditorId = (CreditorIdIsValid) ? Dummy : 0;
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
        
        private string FLedgerTypeValue = "";
        public bool LedgerTypeIsValid = false;
        public int LedgerType { get; set; }
        public string LedgerTypeValue
        {
            get { return FLedgerTypeValue; }
            set
            {
                FLedgerTypeValue = value;
                int Dummy = 0;
                LedgerTypeIsValid = int.TryParse(FLedgerTypeValue, out Dummy);
                LedgerType = (LedgerTypeIsValid) ? Dummy : 0;
            }
        }
        
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
        
        private string FPassedIdValue = "";
        public bool PassedIdIsValid = false;
        public int PassedId { get; set; }
        public string PassedIdValue
        {
            get { return FPassedIdValue; }
            set
            {
                FPassedIdValue = value;
                int Dummy = 0;
                PassedIdIsValid = int.TryParse(FPassedIdValue, out Dummy);
                PassedId = (PassedIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            BankIdValue = "";
            BranchIdValue = "";
            ManagerIdValue = "";
            DisplayTypeValue = "";
            ExpenseIdValue = "";
            AssignedToIdValue = "";
            DateFrom = "";
            DateTo = "";
            CreditorIdValue = "";
            AccountIdValue = "";
            LedgerTypeValue = "";
            TransactionIdValue = "";
            PassedIdValue = "";
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
                    Key = "BankId";
                    if (!BankIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    Key = "DisplayType";
                    if (!DisplayTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ExpenseId";
                    if (!ExpenseIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AssignedToId";
                    if (!AssignedToIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CreditorId";
                    if (!CreditorIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AccountId";
                    if (!AccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LedgerType";
                    if (!LedgerTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransactionId";
                    if (!TransactionIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PassedId";
                    if (!PassedIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(InvoicesReverseTransferRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.BankIdValue = Source.BankIdValue;
                this.BranchIdValue = Source.BranchIdValue;
                this.ManagerIdValue = Source.ManagerIdValue;
                this.DisplayTypeValue = Source.DisplayTypeValue;
                this.ExpenseIdValue = Source.ExpenseIdValue;
                this.AssignedToIdValue = Source.AssignedToIdValue;
                this.DateFrom = Source.DateFrom;
                this.DateTo = Source.DateTo;
                this.CreditorIdValue = Source.CreditorIdValue;
                this.AccountIdValue = Source.AccountIdValue;
                this.LedgerTypeValue = Source.LedgerTypeValue;
                this.TransactionIdValue = Source.TransactionIdValue;
                this.PassedIdValue = Source.PassedIdValue;
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
            
                    InvoicesReverseTransferRequestItem TempItem = new InvoicesReverseTransferRequestItem();
                    //Result = CommonMethods.LoadInvoicesReverseTransferRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadInvoicesReverseTransferRequestItem(ActionBase, ref TempItem);
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
