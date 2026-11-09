/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 25-03-2020
 * Time: 5:33 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Debts\Actions\Templates\Debts_UndoRedoNonGridRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Debts
{
    public class Debts_UndoRedoNonGridRequestItem
    {
        #region Properties - Getters and Setters
        private string FMasterExclusionIdValue = "";
        public bool MasterExclusionIdIsValid = false;
        public int MasterExclusionId { get; set; }
        public string MasterExclusionIdValue
        {
            get { return FMasterExclusionIdValue; }
            set
            {
                FMasterExclusionIdValue = value;
                int Dummy = 0;
                MasterExclusionIdIsValid = int.TryParse(FMasterExclusionIdValue, out Dummy);
                MasterExclusionId = (MasterExclusionIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOfficeIdValue = "";
        public bool OfficeIdIsValid = false;
        public int OfficeId { get; set; }
        public string OfficeIdValue
        {
            get { return FOfficeIdValue; }
            set
            {
                FOfficeIdValue = value;
                int Dummy = 0;
                OfficeIdIsValid = int.TryParse(FOfficeIdValue, out Dummy);
                OfficeId = (OfficeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FReportsIdValue = "";
        public bool ReportsIdIsValid = false;
        public int ReportsId { get; set; }
        public string ReportsIdValue
        {
            get { return FReportsIdValue; }
            set
            {
                FReportsIdValue = value;
                int Dummy = 0;
                ReportsIdIsValid = int.TryParse(FReportsIdValue, out Dummy);
                ReportsId = (ReportsIdIsValid) ? Dummy : 0;
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
        
        private string FPastEventsIdValue = "";
        public bool PastEventsIdIsValid = false;
        public int PastEventsId { get; set; }
        public string PastEventsIdValue
        {
            get { return FPastEventsIdValue; }
            set
            {
                FPastEventsIdValue = value;
                int Dummy = 0;
                PastEventsIdIsValid = int.TryParse(FPastEventsIdValue, out Dummy);
                PastEventsId = (PastEventsIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FBankTypesIdValue = "";
        public bool BankTypesIdIsValid = false;
        public int BankTypesId { get; set; }
        public string BankTypesIdValue
        {
            get { return FBankTypesIdValue; }
            set
            {
                FBankTypesIdValue = value;
                int Dummy = 0;
                BankTypesIdIsValid = int.TryParse(FBankTypesIdValue, out Dummy);
                BankTypesId = (BankTypesIdIsValid) ? Dummy : 0;
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
        
        private string FLedgerIdValue = "";
        public bool LedgerIdIsValid = false;
        public int LedgerId { get; set; }
        public string LedgerIdValue
        {
            get { return FLedgerIdValue; }
            set
            {
                FLedgerIdValue = value;
                int Dummy = 0;
                LedgerIdIsValid = int.TryParse(FLedgerIdValue, out Dummy);
                LedgerId = (LedgerIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FTransactionTypeIdValue = "";
        public bool TransactionTypeIdIsValid = false;
        public int TransactionTypeId { get; set; }
        public string TransactionTypeIdValue
        {
            get { return FTransactionTypeIdValue; }
            set
            {
                FTransactionTypeIdValue = value;
                int Dummy = 0;
                TransactionTypeIdIsValid = int.TryParse(FTransactionTypeIdValue, out Dummy);
                TransactionTypeId = (TransactionTypeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FTransactionsIdValue = "";
        public bool TransactionsIdIsValid = false;
        public int TransactionsId { get; set; }
        public string TransactionsIdValue
        {
            get { return FTransactionsIdValue; }
            set
            {
                FTransactionsIdValue = value;
                int Dummy = 0;
                TransactionsIdIsValid = int.TryParse(FTransactionsIdValue, out Dummy);
                TransactionsId = (TransactionsIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FPrintGroupIdValue = "";
        public bool PrintGroupIdIsValid = false;
        public int PrintGroupId { get; set; }
        public string PrintGroupIdValue
        {
            get { return FPrintGroupIdValue; }
            set
            {
                FPrintGroupIdValue = value;
                int Dummy = 0;
                PrintGroupIdIsValid = int.TryParse(FPrintGroupIdValue, out Dummy);
                PrintGroupId = (PrintGroupIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FChargeOptionsIdValue = "";
        public bool ChargeOptionsIdIsValid = false;
        public int ChargeOptionsId { get; set; }
        public string ChargeOptionsIdValue
        {
            get { return FChargeOptionsIdValue; }
            set
            {
                FChargeOptionsIdValue = value;
                int Dummy = 0;
                ChargeOptionsIdIsValid = int.TryParse(FChargeOptionsIdValue, out Dummy);
                ChargeOptionsId = (ChargeOptionsIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FRangeFromValue = "";
        public bool RangeFromIsValid = false;
        public int RangeFrom { get; set; }
        public string RangeFromValue
        {
            get { return FRangeFromValue; }
            set
            {
                FRangeFromValue = value;
                int Dummy = 0;
                RangeFromIsValid = int.TryParse(FRangeFromValue, out Dummy);
                RangeFrom = (RangeFromIsValid) ? Dummy : 0;
            }
        }
        
        private string FRangeToValue = "";
        public bool RangeToIsValid = false;
        public int RangeTo { get; set; }
        public string RangeToValue
        {
            get { return FRangeToValue; }
            set
            {
                FRangeToValue = value;
                int Dummy = 0;
                RangeToIsValid = int.TryParse(FRangeToValue, out Dummy);
                RangeTo = (RangeToIsValid) ? Dummy : 0;
            }
        }
        
        private string FSourceValue = "";
        public bool SourceIsValid = false;
        public int Source { get; set; }
        public string SourceValue
        {
            get { return FSourceValue; }
            set
            {
                FSourceValue = value;
                int Dummy = 0;
                SourceIsValid = int.TryParse(FSourceValue, out Dummy);
                Source = (SourceIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            MasterExclusionIdValue = "";
            OfficeIdValue = "";
            ReportsIdValue = "";
            ManagerIdValue = "";
            PastEventsIdValue = "";
            BankTypesIdValue = "";
            LedgerTypeIdValue = "";
            LedgerIdValue = "";
            TransactionTypeIdValue = "";
            TransactionsIdValue = "";
            PrintGroupIdValue = "";
            ChargeOptionsIdValue = "";
            RangeFromValue = "";
            RangeToValue = "";
            SourceValue = "";
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
                    Key = "MasterExclusionId";
                    if (!MasterExclusionIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OfficeId";
                    if (!OfficeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReportsId";
                    if (!ReportsIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ManagerId";
                    if (!ManagerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PastEventsId";
                    if (!PastEventsIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankTypesId";
                    if (!BankTypesIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LedgerTypeId";
                    if (!LedgerTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LedgerId";
                    if (!LedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransactionTypeId";
                    if (!TransactionTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransactionsId";
                    if (!TransactionsIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PrintGroupId";
                    if (!PrintGroupIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargeOptionsId";
                    if (!ChargeOptionsIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RangeFrom";
                    if (!RangeFromIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RangeTo";
                    if (!RangeToIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Source";
                    if (!SourceIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(Debts_UndoRedoNonGridRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.MasterExclusionIdValue = Source.MasterExclusionIdValue;
                this.OfficeIdValue = Source.OfficeIdValue;
                this.ReportsIdValue = Source.ReportsIdValue;
                this.ManagerIdValue = Source.ManagerIdValue;
                this.PastEventsIdValue = Source.PastEventsIdValue;
                this.BankTypesIdValue = Source.BankTypesIdValue;
                this.LedgerTypeIdValue = Source.LedgerTypeIdValue;
                this.LedgerIdValue = Source.LedgerIdValue;
                this.TransactionTypeIdValue = Source.TransactionTypeIdValue;
                this.TransactionsIdValue = Source.TransactionsIdValue;
                this.PrintGroupIdValue = Source.PrintGroupIdValue;
                this.ChargeOptionsIdValue = Source.ChargeOptionsIdValue;
                this.RangeFromValue = Source.RangeFromValue;
                this.RangeToValue = Source.RangeToValue;
                this.SourceValue = Source.SourceValue;
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
            
                    Debts_UndoRedoNonGridRequestItem TempItem = new Debts_UndoRedoNonGridRequestItem();
                    //Result = CommonMethods.LoadDebts_UndoRedoNonGridRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadDebts_UndoRedoNonGridRequestItem(ActionBase, ref TempItem);
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
