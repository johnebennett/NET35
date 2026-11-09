/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 12-09-2022
 * Time: 7:13 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Receipts/Actions/Templates/CreateDirectDebitListRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Receipts.CreateOptions
{
    public class CreateDirectDebitListRequestItem
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
        
        private string FBankNameIdValue = "";
        public bool BankNameIdIsValid = false;
        public int BankNameId { get; set; }
        public string BankNameIdValue
        {
            get { return FBankNameIdValue; }
            set
            {
                FBankNameIdValue = value;
                int Dummy = 0;
                BankNameIdIsValid = int.TryParse(FBankNameIdValue, out Dummy);
                BankNameId = (BankNameIdIsValid) ? Dummy : 0;
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
        
        private string FTransactionTypeValue = "";
        public bool TransactionTypeIsValid = false;
        public int TransactionType { get; set; }
        public string TransactionTypeValue
        {
            get { return FTransactionTypeValue; }
            set
            {
                FTransactionTypeValue = value;
                int Dummy = 0;
                TransactionTypeIsValid = int.TryParse(FTransactionTypeValue, out Dummy);
                TransactionType = (TransactionTypeIsValid) ? Dummy : 0;
            }
        }
        
        private string FDebitTypeIdValue = "";
        public bool DebitTypeIdIsValid = false;
        public int DebitTypeId { get; set; }
        public string DebitTypeIdValue
        {
            get { return FDebitTypeIdValue; }
            set
            {
                FDebitTypeIdValue = value;
                int Dummy = 0;
                DebitTypeIdIsValid = int.TryParse(FDebitTypeIdValue, out Dummy);
                DebitTypeId = (DebitTypeIdIsValid) ? Dummy : 0;
            }
        }
        
        public string MultiBankFile { get; set; }
        
        public string DebitRunCode1 { get; set; }
        
        public string DebitRunCode2 { get; set; }
        
        public string DebitRunCode3 { get; set; }
        
        private string FClearanceDaysValue = "";
        public bool ClearanceDaysIsValid = false;
        public int ClearanceDays { get; set; }
        public string ClearanceDaysValue
        {
            get { return FClearanceDaysValue; }
            set
            {
                FClearanceDaysValue = value;
                int Dummy = 0;
                ClearanceDaysIsValid = int.TryParse(FClearanceDaysValue, out Dummy);
                ClearanceDays = (ClearanceDaysIsValid) ? Dummy : 0;
            }
        }
        
        public string DueFrom { get; set; }
        
        public string DueTo { get; set; }
        
        public string ExcludeVacating { get; set; }
        
        public string VacatingFrom { get; set; }
        
        public string VacatingTo { get; set; }
        
        private string FDirectDebitIdValue = "";
        public bool DirectDebitIdIsValid = false;
        public int DirectDebitId { get; set; }
        public string DirectDebitIdValue
        {
            get { return FDirectDebitIdValue; }
            set
            {
                FDirectDebitIdValue = value;
                int Dummy = 0;
                DirectDebitIdIsValid = int.TryParse(FDirectDebitIdValue, out Dummy);
                DirectDebitId = (DirectDebitIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FNextReceiptNumberValue = "";
        public bool NextReceiptNumberIsValid = false;
        public int NextReceiptNumber { get; set; }
        public string NextReceiptNumberValue
        {
            get { return FNextReceiptNumberValue; }
            set
            {
                FNextReceiptNumberValue = value;
                int Dummy = 0;
                NextReceiptNumberIsValid = int.TryParse(FNextReceiptNumberValue, out Dummy);
                NextReceiptNumber = (NextReceiptNumberIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            MasterEventIdValue = "";
            BankNameIdValue = "";
            BranchIdValue = "";
            ManagerIdValue = "";
            TransactionTypeValue = "";
            DebitTypeIdValue = "";
            MultiBankFile = "";
            ClearanceDaysValue = "";
            DueFrom = "";
            DueTo = "";
            ExcludeVacating = "";
            VacatingFrom = "";
            VacatingTo = "";
            DirectDebitIdValue = "";
            NextReceiptNumberValue = "";
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
                    Key = "BankNameId";
                    if (!BankNameIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    Key = "TransactionType";
                    if (!TransactionTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DebitTypeId";
                    if (!DebitTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiBankFile";
                    if (string.IsNullOrEmpty(MultiBankFile)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ClearanceDays";
                    if (!ClearanceDaysIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DueFrom";
                    if (string.IsNullOrEmpty(DueFrom)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DueTo";
                    if (string.IsNullOrEmpty(DueTo)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ExcludeVacating";
                    if (string.IsNullOrEmpty(ExcludeVacating)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "VacatingFrom";
                    if (string.IsNullOrEmpty(VacatingFrom)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "VacatingTo";
                    if (string.IsNullOrEmpty(VacatingTo)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DirectDebitId";
                    if (!DirectDebitIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "NextReceiptNumber";
                    if (!NextReceiptNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(CreateDirectDebitListRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.MasterEventIdValue = Source.MasterEventIdValue;
                this.BankNameIdValue = Source.BankNameIdValue;
                this.BranchIdValue = Source.BranchIdValue;
                this.ManagerIdValue = Source.ManagerIdValue;
                this.TransactionTypeValue = Source.TransactionTypeValue;
                this.DebitTypeIdValue = Source.DebitTypeIdValue;
                this.MultiBankFile = Source.MultiBankFile;
                this.DebitRunCode1 = Source.DebitRunCode1;
                this.DebitRunCode2 = Source.DebitRunCode2;
                this.DebitRunCode3 = Source.DebitRunCode3;
                this.ClearanceDaysValue = Source.ClearanceDaysValue;
                this.DueFrom = Source.DueFrom;
                this.DueTo = Source.DueTo;
                this.ExcludeVacating = Source.ExcludeVacating;
                this.VacatingFrom = Source.VacatingFrom;
                this.VacatingTo = Source.VacatingTo;
                this.DirectDebitIdValue = Source.DirectDebitIdValue;
                this.NextReceiptNumberValue = Source.NextReceiptNumberValue;
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
            
                    CreateDirectDebitListRequestItem TempItem = new CreateDirectDebitListRequestItem();
                    //Result = CommonMethods.LoadCreateDirectDebitListRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadCreateDirectDebitListRequestItem(ActionBase, ref TempItem);
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
