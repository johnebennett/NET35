/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 22-05-2020
 * Time: 6:14 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Invoices\Actions\Templates\AddInvoice\SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Invoices.AddInvoice
{
    public class SetupPageEntriesRequestItem
    {
        #region Properties - Getters and Setters
        private string FInvoiceMasterIdValue = "";
        public bool InvoiceMasterIdIsValid = false;
        public int InvoiceMasterId { get; set; }
        public string InvoiceMasterIdValue
        {
            get { return FInvoiceMasterIdValue; }
            set
            {
                FInvoiceMasterIdValue = value;
                int Dummy = 0;
                InvoiceMasterIdIsValid = int.TryParse(FInvoiceMasterIdValue, out Dummy);
                InvoiceMasterId = (InvoiceMasterIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FInvoicesScannedIdValue = "";
        public bool InvoicesScannedIdIsValid = false;
        public int InvoicesScannedId { get; set; }
        public string InvoicesScannedIdValue
        {
            get { return FInvoicesScannedIdValue; }
            set
            {
                FInvoicesScannedIdValue = value;
                int Dummy = 0;
                InvoicesScannedIdIsValid = int.TryParse(FInvoicesScannedIdValue, out Dummy);
                InvoicesScannedId = (InvoicesScannedIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FInvoicesSelectedIdValue = "";
        public bool InvoicesSelectedIdIsValid = false;
        public int InvoicesSelectedId { get; set; }
        public string InvoicesSelectedIdValue
        {
            get { return FInvoicesSelectedIdValue; }
            set
            {
                FInvoicesSelectedIdValue = value;
                int Dummy = 0;
                InvoicesSelectedIdIsValid = int.TryParse(FInvoicesSelectedIdValue, out Dummy);
                InvoicesSelectedId = (InvoicesSelectedIdIsValid) ? Dummy : 0;
            }
        }
        
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
        
        private string FUncompletedTaskIdValue = "";
        public bool UncompletedTaskIdIsValid = false;
        public int UncompletedTaskId { get; set; }
        public string UncompletedTaskIdValue
        {
            get { return FUncompletedTaskIdValue; }
            set
            {
                FUncompletedTaskIdValue = value;
                int Dummy = 0;
                UncompletedTaskIdIsValid = int.TryParse(FUncompletedTaskIdValue, out Dummy);
                UncompletedTaskId = (UncompletedTaskIdIsValid) ? Dummy : 0;
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
        
        public string InvoiceDate { get; set; }
        
        public string InvoiceReleaseDate { get; set; }
        
        public string InvoiceNo { get; set; }
        
        public string InvoiceBPay { get; set; }
        
        public string InvoiceDetail { get; set; }
        
        private string FInvoiceAmountValue = "";
        public bool InvoiceAmountIsValid = false;
        public double InvoiceAmount { get; set; }
        public string InvoiceAmountValue
        {
            get { return FInvoiceAmountValue; }
            set
            {
                FInvoiceAmountValue = value;
                double Dummy = 0.0;
                InvoiceAmountIsValid = Double.TryParse(FInvoiceAmountValue, out Dummy);
                InvoiceAmount = (InvoiceAmountIsValid) ? Dummy : 0;
            }
        }
        
        private string FInvoiceGSTAmountValue = "";
        public bool InvoiceGSTAmountIsValid = false;
        public double InvoiceGSTAmount { get; set; }
        public string InvoiceGSTAmountValue
        {
            get { return FInvoiceGSTAmountValue; }
            set
            {
                FInvoiceGSTAmountValue = value;
                double Dummy = 0.0;
                InvoiceGSTAmountIsValid = Double.TryParse(FInvoiceGSTAmountValue, out Dummy);
                InvoiceGSTAmount = (InvoiceGSTAmountIsValid) ? Dummy : 0;
            }
        }
        
        private string FInvoicePriorityValue = "";
        public bool InvoicePriorityIsValid = false;
        public int InvoicePriority { get; set; }
        public string InvoicePriorityValue
        {
            get { return FInvoicePriorityValue; }
            set
            {
                FInvoicePriorityValue = value;
                int Dummy = 0;
                InvoicePriorityIsValid = int.TryParse(FInvoicePriorityValue, out Dummy);
                InvoicePriority = (InvoicePriorityIsValid) ? Dummy : 0;
            }
        }
        
        private string FChargeTransactionIdValue = "";
        public bool ChargeTransactionIdIsValid = false;
        public int ChargeTransactionId { get; set; }
        public string ChargeTransactionIdValue
        {
            get { return FChargeTransactionIdValue; }
            set
            {
                FChargeTransactionIdValue = value;
                int Dummy = 0;
                ChargeTransactionIdIsValid = int.TryParse(FChargeTransactionIdValue, out Dummy);
                ChargeTransactionId = (ChargeTransactionIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FChargeGrossValue = "";
        public bool ChargeGrossIsValid = false;
        public double ChargeGross { get; set; }
        public string ChargeGrossValue
        {
            get { return FChargeGrossValue; }
            set
            {
                FChargeGrossValue = value;
                double Dummy = 0.0;
                ChargeGrossIsValid = Double.TryParse(FChargeGrossValue, out Dummy);
                ChargeGross = (ChargeGrossIsValid) ? Dummy : 0;
            }
        }
        
        private string FChargeGSTValue = "";
        public bool ChargeGSTIsValid = false;
        public double ChargeGST { get; set; }
        public string ChargeGSTValue
        {
            get { return FChargeGSTValue; }
            set
            {
                FChargeGSTValue = value;
                double Dummy = 0.0;
                ChargeGSTIsValid = Double.TryParse(FChargeGSTValue, out Dummy);
                ChargeGST = (ChargeGSTIsValid) ? Dummy : 0;
            }
        }
        
        private string FEmailTaxInvoiceIdValue = "";
        public bool EmailTaxInvoiceIdIsValid = false;
        public int EmailTaxInvoiceId { get; set; }
        public string EmailTaxInvoiceIdValue
        {
            get { return FEmailTaxInvoiceIdValue; }
            set
            {
                FEmailTaxInvoiceIdValue = value;
                int Dummy = 0;
                EmailTaxInvoiceIdIsValid = int.TryParse(FEmailTaxInvoiceIdValue, out Dummy);
                EmailTaxInvoiceId = (EmailTaxInvoiceIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FInstalmentIdValue = "";
        public bool InstalmentIdIsValid = false;
        public int InstalmentId { get; set; }
        public string InstalmentIdValue
        {
            get { return FInstalmentIdValue; }
            set
            {
                FInstalmentIdValue = value;
                int Dummy = 0;
                InstalmentIdIsValid = int.TryParse(FInstalmentIdValue, out Dummy);
                InstalmentId = (InstalmentIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FSplitInvoiceByIdValue = "";
        public bool SplitInvoiceByIdIsValid = false;
        public int SplitInvoiceById { get; set; }
        public string SplitInvoiceByIdValue
        {
            get { return FSplitInvoiceByIdValue; }
            set
            {
                FSplitInvoiceByIdValue = value;
                int Dummy = 0;
                SplitInvoiceByIdIsValid = int.TryParse(FSplitInvoiceByIdValue, out Dummy);
                SplitInvoiceById = (SplitInvoiceByIdIsValid) ? Dummy : 0;
            }
        }
        
        public string OnlyACharge { get; set; }
        
        public string HoldTillClientPays { get; set; }
        
        public string ChargeFromDate { get; set; }
        
        public string ChargeToDate { get; set; }
        
        public string ChargeDueDate { get; set; }
        
        public string ChargePrintDate { get; set; }
        
        public string ApprovalRequired { get; set; }
        
        private string FApprovalRequiredByValue = "";
        public bool ApprovalRequiredByIsValid = false;
        public int ApprovalRequiredBy { get; set; }
        public string ApprovalRequiredByValue
        {
            get { return FApprovalRequiredByValue; }
            set
            {
                FApprovalRequiredByValue = value;
                int Dummy = 0;
                ApprovalRequiredByIsValid = int.TryParse(FApprovalRequiredByValue, out Dummy);
                ApprovalRequiredBy = (ApprovalRequiredByIsValid) ? Dummy : 0;
            }
        }
        
        public string ApprovedComment { get; set; }
        
        public string ShowGSTAmount { get; set; }
        
        public string CalculateGST { get; set; }
        
        public string AlterGSTAmount { get; set; }
        
        public string ShowEmailTaxInvoice { get; set; }
        
        public string Approved { get; set; }
        
        public string ChargeBusiness { get; set; }
        
        public string SelectedCreditor { get; set; }
        
        public string SelectedProperty { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            InvoiceMasterIdValue = "";
            InvoicesScannedIdValue = "";
            InvoicesSelectedIdValue = "";
            CreditorIdValue = "";
            TransactionIdValue = "";
            SubAccountIdValue = "";
            UncompletedTaskIdValue = "";
            ExpenseIdValue = "";
            RevenueIdValue = "";
            InvoiceDate = "";
            InvoiceReleaseDate = "";
            InvoiceAmountValue = "";
            InvoiceGSTAmountValue = "";
            InvoicePriorityValue = "";
            ChargeTransactionIdValue = "";
            ChargeGrossValue = "";
            ChargeGSTValue = "";
            EmailTaxInvoiceIdValue = "";
            InstalmentIdValue = "";
            SplitInvoiceByIdValue = "";
            OnlyACharge = "";
            HoldTillClientPays = "";
            ChargeFromDate = "";
            ChargeToDate = "";
            ChargeDueDate = "";
            ChargePrintDate = "";
            ApprovalRequired = "";
            ApprovalRequiredByValue = "";
            ShowGSTAmount = "";
            CalculateGST = "";
            AlterGSTAmount = "";
            ShowEmailTaxInvoice = "";
            Approved = "";
            ChargeBusiness = "";
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
                    Key = "InvoiceMasterId";
                    if (!InvoiceMasterIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvoicesScannedId";
                    if (!InvoicesScannedIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvoicesSelectedId";
                    if (!InvoicesSelectedIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CreditorId";
                    if (!CreditorIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransactionId";
                    if (!TransactionIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SubAccountId";
                    if (!SubAccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "UncompletedTaskId";
                    if (!UncompletedTaskIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ExpenseId";
                    if (!ExpenseIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RevenueId";
                    if (!RevenueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvoiceDate";
                    if (string.IsNullOrEmpty(InvoiceDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvoiceReleaseDate";
                    if (string.IsNullOrEmpty(InvoiceReleaseDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvoiceAmount";
                    if (!InvoiceAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvoiceGSTAmount";
                    if (!InvoiceGSTAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvoicePriority";
                    if (!InvoicePriorityIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargeTransactionId";
                    if (!ChargeTransactionIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargeGross";
                    if (!ChargeGrossIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargeGST";
                    if (!ChargeGSTIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EmailTaxInvoiceId";
                    if (!EmailTaxInvoiceIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InstalmentId";
                    if (!InstalmentIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SplitInvoiceById";
                    if (!SplitInvoiceByIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OnlyACharge";
                    if (string.IsNullOrEmpty(OnlyACharge)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "HoldTillClientPays";
                    if (string.IsNullOrEmpty(HoldTillClientPays)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargeFromDate";
                    if (string.IsNullOrEmpty(ChargeFromDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargeToDate";
                    if (string.IsNullOrEmpty(ChargeToDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargeDueDate";
                    if (string.IsNullOrEmpty(ChargeDueDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargePrintDate";
                    if (string.IsNullOrEmpty(ChargePrintDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ApprovalRequired";
                    if (string.IsNullOrEmpty(ApprovalRequired)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ApprovalRequiredBy";
                    if (!ApprovalRequiredByIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowGSTAmount";
                    if (string.IsNullOrEmpty(ShowGSTAmount)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CalculateGST";
                    if (string.IsNullOrEmpty(CalculateGST)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AlterGSTAmount";
                    if (string.IsNullOrEmpty(AlterGSTAmount)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowEmailTaxInvoice";
                    if (string.IsNullOrEmpty(ShowEmailTaxInvoice)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Approved";
                    if (string.IsNullOrEmpty(Approved)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargeBusiness";
                    if (string.IsNullOrEmpty(ChargeBusiness)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.InvoiceMasterIdValue = Source.InvoiceMasterIdValue;
                this.InvoicesScannedIdValue = Source.InvoicesScannedIdValue;
                this.InvoicesSelectedIdValue = Source.InvoicesSelectedIdValue;
                this.CreditorIdValue = Source.CreditorIdValue;
                this.TransactionIdValue = Source.TransactionIdValue;
                this.SubAccountIdValue = Source.SubAccountIdValue;
                this.UncompletedTaskIdValue = Source.UncompletedTaskIdValue;
                this.ExpenseIdValue = Source.ExpenseIdValue;
                this.RevenueIdValue = Source.RevenueIdValue;
                this.InvoiceDate = Source.InvoiceDate;
                this.InvoiceReleaseDate = Source.InvoiceReleaseDate;
                this.InvoiceNo = Source.InvoiceNo;
                this.InvoiceBPay = Source.InvoiceBPay;
                this.InvoiceDetail = Source.InvoiceDetail;
                this.InvoiceAmountValue = Source.InvoiceAmountValue;
                this.InvoiceGSTAmountValue = Source.InvoiceGSTAmountValue;
                this.InvoicePriorityValue = Source.InvoicePriorityValue;
                this.ChargeTransactionIdValue = Source.ChargeTransactionIdValue;
                this.ChargeGrossValue = Source.ChargeGrossValue;
                this.ChargeGSTValue = Source.ChargeGSTValue;
                this.EmailTaxInvoiceIdValue = Source.EmailTaxInvoiceIdValue;
                this.InstalmentIdValue = Source.InstalmentIdValue;
                this.SplitInvoiceByIdValue = Source.SplitInvoiceByIdValue;
                this.OnlyACharge = Source.OnlyACharge;
                this.HoldTillClientPays = Source.HoldTillClientPays;
                this.ChargeFromDate = Source.ChargeFromDate;
                this.ChargeToDate = Source.ChargeToDate;
                this.ChargeDueDate = Source.ChargeDueDate;
                this.ChargePrintDate = Source.ChargePrintDate;
                this.ApprovalRequired = Source.ApprovalRequired;
                this.ApprovalRequiredByValue = Source.ApprovalRequiredByValue;
                this.ApprovedComment = Source.ApprovedComment;
                this.ShowGSTAmount = Source.ShowGSTAmount;
                this.CalculateGST = Source.CalculateGST;
                this.AlterGSTAmount = Source.AlterGSTAmount;
                this.ShowEmailTaxInvoice = Source.ShowEmailTaxInvoice;
                this.Approved = Source.Approved;
                this.ChargeBusiness = Source.ChargeBusiness;
                this.SelectedCreditor = Source.SelectedCreditor;
                this.SelectedProperty = Source.SelectedProperty;
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
