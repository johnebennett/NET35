/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 03-02-2021
 * Time: 10:24 AM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/PortfolioAddButton/Actions/Templates/SubLedgerDlg/SaveCmdRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PortfolioAddButton.SubLedgerDlg
{
    public class SaveCmdRequestItem
    {
        #region Properties - Getters and Setters
        private string FPortfolioIdValue = "";
        public bool PortfolioIdIsValid = false;
        public int PortfolioId { get; set; }
        public string PortfolioIdValue
        {
            get { return FPortfolioIdValue; }
            set
            {
                FPortfolioIdValue = value;
                int Dummy = 0;
                PortfolioIdIsValid = int.TryParse(FPortfolioIdValue, out Dummy);
                PortfolioId = (PortfolioIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOption1IdValue = "";
        public bool Option1IdIsValid = false;
        public int Option1Id { get; set; }
        public string Option1IdValue
        {
            get { return FOption1IdValue; }
            set
            {
                FOption1IdValue = value;
                int Dummy = 0;
                Option1IdIsValid = int.TryParse(FOption1IdValue, out Dummy);
                Option1Id = (Option1IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOption2IdValue = "";
        public bool Option2IdIsValid = false;
        public int Option2Id { get; set; }
        public string Option2IdValue
        {
            get { return FOption2IdValue; }
            set
            {
                FOption2IdValue = value;
                int Dummy = 0;
                Option2IdIsValid = int.TryParse(FOption2IdValue, out Dummy);
                Option2Id = (Option2IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FGridIdValue = "";
        public bool GridIdIsValid = false;
        public int GridId { get; set; }
        public string GridIdValue
        {
            get { return FGridIdValue; }
            set
            {
                FGridIdValue = value;
                int Dummy = 0;
                GridIdIsValid = int.TryParse(FGridIdValue, out Dummy);
                GridId = (GridIdIsValid) ? Dummy : 0;
            }
        }
        
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
        
        public string SubLedgerName { get; set; }
        
        private string FRunNumberValue = "";
        public bool RunNumberIsValid = false;
        public int RunNumber { get; set; }
        public string RunNumberValue
        {
            get { return FRunNumberValue; }
            set
            {
                FRunNumberValue = value;
                int Dummy = 0;
                RunNumberIsValid = int.TryParse(FRunNumberValue, out Dummy);
                RunNumber = (RunNumberIsValid) ? Dummy : 0;
            }
        }
        
        public string ChargeStmtFee { get; set; }
        
        private string FStmtFeeValue = "";
        public bool StmtFeeIsValid = false;
        public double StmtFee { get; set; }
        public string StmtFeeValue
        {
            get { return FStmtFeeValue; }
            set
            {
                FStmtFeeValue = value;
                double Dummy = 0.0;
                StmtFeeIsValid = Double.TryParse(FStmtFeeValue, out Dummy);
                StmtFee = (StmtFeeIsValid) ? Dummy : 0;
            }
        }
        
        private string FHoldOverAmountValue = "";
        public bool HoldOverAmountIsValid = false;
        public double HoldOverAmount { get; set; }
        public string HoldOverAmountValue
        {
            get { return FHoldOverAmountValue; }
            set
            {
                FHoldOverAmountValue = value;
                double Dummy = 0.0;
                HoldOverAmountIsValid = Double.TryParse(FHoldOverAmountValue, out Dummy);
                HoldOverAmount = (HoldOverAmountIsValid) ? Dummy : 0;
            }
        }
        
        public string HoldOverReason { get; set; }
        
        public string StatementName { get; set; }
        
        public string StatementAddress1 { get; set; }
        
        public string StatementAddress2 { get; set; }
        
        public string StatementAddress3 { get; set; }
        
        public string StatementAddress4 { get; set; }
        
        public string BSB { get; set; }
        
        public string AccountNumber { get; set; }
        
        public string ABN { get; set; }
        
        public string CanChangeStmt { get; set; }
        
        public string CanChangeBSB { get; set; }
        
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
        
        private string FToStmtFeeIdValue = "";
        public bool ToStmtFeeIdIsValid = false;
        public int ToStmtFeeId { get; set; }
        public string ToStmtFeeIdValue
        {
            get { return FToStmtFeeIdValue; }
            set
            {
                FToStmtFeeIdValue = value;
                int Dummy = 0;
                ToStmtFeeIdIsValid = int.TryParse(FToStmtFeeIdValue, out Dummy);
                ToStmtFeeId = (ToStmtFeeIdIsValid) ? Dummy : 0;
            }
        }
        
        public string PrepareStatement { get; set; }
        
        public string AccountOwner { get; set; }
        
        public string PropertyDisplay { get; set; }
        
        public string ExcludeFromEOYRun { get; set; }
        
        public string DoNotInvoiceEOYRun { get; set; }
        
        public string UseBroadStatement { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            Option1IdValue = "";
            Option2IdValue = "";
            GridIdValue = "";
            BankIdValue = "";
            SubLedgerName = "";
            RunNumberValue = "";
            ChargeStmtFee = "";
            StmtFeeValue = "";
            HoldOverAmountValue = "";
            CanChangeStmt = "";
            CanChangeBSB = "";
            ExpenseIdValue = "";
            RevenueIdValue = "";
            ToStmtFeeIdValue = "";
            PrepareStatement = "";
            AccountOwner = "";
            ExcludeFromEOYRun = "";
            DoNotInvoiceEOYRun = "";
            UseBroadStatement = "";
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
                    Key = "PortfolioId";
                    if (!PortfolioIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Option1Id";
                    if (!Option1IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Option2Id";
                    if (!Option2IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GridId";
                    if (!GridIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankId";
                    if (!BankIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SubLedgerName";
                    if (string.IsNullOrEmpty(SubLedgerName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunNumber";
                    if (!RunNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargeStmtFee";
                    if (string.IsNullOrEmpty(ChargeStmtFee)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StmtFee";
                    if (!StmtFeeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "HoldOverAmount";
                    if (!HoldOverAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CanChangeStmt";
                    if (string.IsNullOrEmpty(CanChangeStmt)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CanChangeBSB";
                    if (string.IsNullOrEmpty(CanChangeBSB)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    Key = "ToStmtFeeId";
                    if (!ToStmtFeeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PrepareStatement";
                    if (string.IsNullOrEmpty(PrepareStatement)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AccountOwner";
                    if (string.IsNullOrEmpty(AccountOwner)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ExcludeFromEOYRun";
                    if (string.IsNullOrEmpty(ExcludeFromEOYRun)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DoNotInvoiceEOYRun";
                    if (string.IsNullOrEmpty(DoNotInvoiceEOYRun)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "UseBroadStatement";
                    if (string.IsNullOrEmpty(UseBroadStatement)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SaveCmdRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.Option1IdValue = Source.Option1IdValue;
                this.Option2IdValue = Source.Option2IdValue;
                this.GridIdValue = Source.GridIdValue;
                this.BankIdValue = Source.BankIdValue;
                this.SubLedgerName = Source.SubLedgerName;
                this.RunNumberValue = Source.RunNumberValue;
                this.ChargeStmtFee = Source.ChargeStmtFee;
                this.StmtFeeValue = Source.StmtFeeValue;
                this.HoldOverAmountValue = Source.HoldOverAmountValue;
                this.HoldOverReason = Source.HoldOverReason;
                this.StatementName = Source.StatementName;
                this.StatementAddress1 = Source.StatementAddress1;
                this.StatementAddress2 = Source.StatementAddress2;
                this.StatementAddress3 = Source.StatementAddress3;
                this.StatementAddress4 = Source.StatementAddress4;
                this.BSB = Source.BSB;
                this.AccountNumber = Source.AccountNumber;
                this.ABN = Source.ABN;
                this.CanChangeStmt = Source.CanChangeStmt;
                this.CanChangeBSB = Source.CanChangeBSB;
                this.ExpenseIdValue = Source.ExpenseIdValue;
                this.RevenueIdValue = Source.RevenueIdValue;
                this.ToStmtFeeIdValue = Source.ToStmtFeeIdValue;
                this.PrepareStatement = Source.PrepareStatement;
                this.AccountOwner = Source.AccountOwner;
                this.PropertyDisplay = Source.PropertyDisplay;
                this.ExcludeFromEOYRun = Source.ExcludeFromEOYRun;
                this.DoNotInvoiceEOYRun = Source.DoNotInvoiceEOYRun;
                this.UseBroadStatement = Source.UseBroadStatement;
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
            
                    SaveCmdRequestItem TempItem = new SaveCmdRequestItem();
                    //Result = CommonMethods.LoadSaveCmdRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSaveCmdRequestItem(ActionBase, ref TempItem);
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
