/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 08-05-2020
 * Time: 6:05 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Receipts\Actions\Templates\AddCashChqReceipt\SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Receipts.AddCashChqReceipt
{
    public class SetupPageEntriesRequestItem
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
        
        private string FReceiptTypeValue = "";
        public bool ReceiptTypeIsValid = false;
        public int ReceiptType { get; set; }
        public string ReceiptTypeValue
        {
            get { return FReceiptTypeValue; }
            set
            {
                FReceiptTypeValue = value;
                int Dummy = 0;
                ReceiptTypeIsValid = int.TryParse(FReceiptTypeValue, out Dummy);
                ReceiptType = (ReceiptTypeIsValid) ? Dummy : 0;
            }
        }
        
        private string FBankingIdValue = "";
        public bool BankingIdIsValid = false;
        public int BankingId { get; set; }
        public string BankingIdValue
        {
            get { return FBankingIdValue; }
            set
            {
                FBankingIdValue = value;
                int Dummy = 0;
                BankingIdIsValid = int.TryParse(FBankingIdValue, out Dummy);
                BankingId = (BankingIdIsValid) ? Dummy : 0;
            }
        }
        
        public string FindText { get; set; }
        
        private string FFindIdValue = "";
        public bool FindIdIsValid = false;
        public int FindId { get; set; }
        public string FindIdValue
        {
            get { return FFindIdValue; }
            set
            {
                FFindIdValue = value;
                int Dummy = 0;
                FindIdIsValid = int.TryParse(FFindIdValue, out Dummy);
                FindId = (FindIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FSubLedgerIdValue = "";
        public bool SubLedgerIdIsValid = false;
        public int SubLedgerId { get; set; }
        public string SubLedgerIdValue
        {
            get { return FSubLedgerIdValue; }
            set
            {
                FSubLedgerIdValue = value;
                int Dummy = 0;
                SubLedgerIdIsValid = int.TryParse(FSubLedgerIdValue, out Dummy);
                SubLedgerId = (SubLedgerIdIsValid) ? Dummy : 0;
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
        
        public string GST { get; set; }
        
        private string FReceiptAmountValue = "";
        public bool ReceiptAmountIsValid = false;
        public double ReceiptAmount { get; set; }
        public string ReceiptAmountValue
        {
            get { return FReceiptAmountValue; }
            set
            {
                FReceiptAmountValue = value;
                double Dummy = 0.0;
                ReceiptAmountIsValid = Double.TryParse(FReceiptAmountValue, out Dummy);
                ReceiptAmount = (ReceiptAmountIsValid) ? Dummy : 0;
            }
        }
        
        private string FReceiptGSTAmountValue = "";
        public bool ReceiptGSTAmountIsValid = false;
        public double ReceiptGSTAmount { get; set; }
        public string ReceiptGSTAmountValue
        {
            get { return FReceiptGSTAmountValue; }
            set
            {
                FReceiptGSTAmountValue = value;
                double Dummy = 0.0;
                ReceiptGSTAmountIsValid = Double.TryParse(FReceiptGSTAmountValue, out Dummy);
                ReceiptGSTAmount = (ReceiptGSTAmountIsValid) ? Dummy : 0;
            }
        }
        
        private string FReceiptNoValue = "";
        public bool ReceiptNoIsValid = false;
        public int ReceiptNo { get; set; }
        public string ReceiptNoValue
        {
            get { return FReceiptNoValue; }
            set
            {
                FReceiptNoValue = value;
                int Dummy = 0;
                ReceiptNoIsValid = int.TryParse(FReceiptNoValue, out Dummy);
                ReceiptNo = (ReceiptNoIsValid) ? Dummy : 0;
            }
        }
        
        public string ReceiptDate { get; set; }
        
        public string ReceiptClearance { get; set; }
        
        public string Detail { get; set; }
        
        public string Payee { get; set; }
        
        public string TakenBy { get; set; }
        
        public string ClientName { get; set; }
        
        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        
        public string Address3 { get; set; }
        
        public string Address4 { get; set; }
        
        private string FCashAmountValue = "";
        public bool CashAmountIsValid = false;
        public double CashAmount { get; set; }
        public string CashAmountValue
        {
            get { return FCashAmountValue; }
            set
            {
                FCashAmountValue = value;
                double Dummy = 0.0;
                CashAmountIsValid = Double.TryParse(FCashAmountValue, out Dummy);
                CashAmount = (CashAmountIsValid) ? Dummy : 0;
            }
        }
        
        private string FChqAmount1Value = "";
        public bool ChqAmount1IsValid = false;
        public double ChqAmount1 { get; set; }
        public string ChqAmount1Value
        {
            get { return FChqAmount1Value; }
            set
            {
                FChqAmount1Value = value;
                double Dummy = 0.0;
                ChqAmount1IsValid = Double.TryParse(FChqAmount1Value, out Dummy);
                ChqAmount1 = (ChqAmount1IsValid) ? Dummy : 0;
            }
        }
        
        public string ChqBank1 { get; set; }
        
        public string ChqBranch1 { get; set; }
        
        public string ChqDrawer1 { get; set; }
        
        private string FChqNumber1Value = "";
        public bool ChqNumber1IsValid = false;
        public int ChqNumber1 { get; set; }
        public string ChqNumber1Value
        {
            get { return FChqNumber1Value; }
            set
            {
                FChqNumber1Value = value;
                int Dummy = 0;
                ChqNumber1IsValid = int.TryParse(FChqNumber1Value, out Dummy);
                ChqNumber1 = (ChqNumber1IsValid) ? Dummy : 0;
            }
        }
        
        private string FChqAmount2Value = "";
        public bool ChqAmount2IsValid = false;
        public double ChqAmount2 { get; set; }
        public string ChqAmount2Value
        {
            get { return FChqAmount2Value; }
            set
            {
                FChqAmount2Value = value;
                double Dummy = 0.0;
                ChqAmount2IsValid = Double.TryParse(FChqAmount2Value, out Dummy);
                ChqAmount2 = (ChqAmount2IsValid) ? Dummy : 0;
            }
        }
        
        public string ChqBank2 { get; set; }
        
        public string ChqBranch2 { get; set; }
        
        public string ChqDrawer2 { get; set; }
        
        private string FChqNumber2Value = "";
        public bool ChqNumber2IsValid = false;
        public int ChqNumber2 { get; set; }
        public string ChqNumber2Value
        {
            get { return FChqNumber2Value; }
            set
            {
                FChqNumber2Value = value;
                int Dummy = 0;
                ChqNumber2IsValid = int.TryParse(FChqNumber2Value, out Dummy);
                ChqNumber2 = (ChqNumber2IsValid) ? Dummy : 0;
            }
        }
        
        private string FChqAmount3Value = "";
        public bool ChqAmount3IsValid = false;
        public double ChqAmount3 { get; set; }
        public string ChqAmount3Value
        {
            get { return FChqAmount3Value; }
            set
            {
                FChqAmount3Value = value;
                double Dummy = 0.0;
                ChqAmount3IsValid = Double.TryParse(FChqAmount3Value, out Dummy);
                ChqAmount3 = (ChqAmount3IsValid) ? Dummy : 0;
            }
        }
        
        public string ChqBank3 { get; set; }
        
        public string ChqBranch3 { get; set; }
        
        public string ChqDrawer3 { get; set; }
        
        private string FChqNumber3Value = "";
        public bool ChqNumber3IsValid = false;
        public int ChqNumber3 { get; set; }
        public string ChqNumber3Value
        {
            get { return FChqNumber3Value; }
            set
            {
                FChqNumber3Value = value;
                int Dummy = 0;
                ChqNumber3IsValid = int.TryParse(FChqNumber3Value, out Dummy);
                ChqNumber3 = (ChqNumber3IsValid) ? Dummy : 0;
            }
        }
        
        private string FChqAmount4Value = "";
        public bool ChqAmount4IsValid = false;
        public double ChqAmount4 { get; set; }
        public string ChqAmount4Value
        {
            get { return FChqAmount4Value; }
            set
            {
                FChqAmount4Value = value;
                double Dummy = 0.0;
                ChqAmount4IsValid = Double.TryParse(FChqAmount4Value, out Dummy);
                ChqAmount4 = (ChqAmount4IsValid) ? Dummy : 0;
            }
        }
        
        public string ChqBank4 { get; set; }
        
        public string ChqBranch4 { get; set; }
        
        public string ChqDrawer4 { get; set; }
        
        private string FChqNumber4Value = "";
        public bool ChqNumber4IsValid = false;
        public int ChqNumber4 { get; set; }
        public string ChqNumber4Value
        {
            get { return FChqNumber4Value; }
            set
            {
                FChqNumber4Value = value;
                int Dummy = 0;
                ChqNumber4IsValid = int.TryParse(FChqNumber4Value, out Dummy);
                ChqNumber4 = (ChqNumber4IsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            MasterEventIdValue = "";
            ReceiptTypeValue = "";
            BankingIdValue = "";
            FindIdValue = "";
            SubLedgerIdValue = "";
            BankIdValue = "";
            RevenueIdValue = "";
            GST = "";
            ReceiptAmountValue = "";
            ReceiptGSTAmountValue = "";
            ReceiptNoValue = "";
            ReceiptDate = "";
            ReceiptClearance = "";
            CashAmountValue = "";
            ChqAmount1Value = "";
            ChqNumber1Value = "";
            ChqAmount2Value = "";
            ChqNumber2Value = "";
            ChqAmount3Value = "";
            ChqNumber3Value = "";
            ChqAmount4Value = "";
            ChqNumber4Value = "";
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
                    Key = "ReceiptType";
                    if (!ReceiptTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankingId";
                    if (!BankingIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FindId";
                    if (!FindIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SubLedgerId";
                    if (!SubLedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankId";
                    if (!BankIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RevenueId";
                    if (!RevenueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GST";
                    if (string.IsNullOrEmpty(GST)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptAmount";
                    if (!ReceiptAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptGSTAmount";
                    if (!ReceiptGSTAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptNo";
                    if (!ReceiptNoIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptDate";
                    if (string.IsNullOrEmpty(ReceiptDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptClearance";
                    if (string.IsNullOrEmpty(ReceiptClearance)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CashAmount";
                    if (!CashAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChqAmount1";
                    if (!ChqAmount1IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChqNumber1";
                    if (!ChqNumber1IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChqAmount2";
                    if (!ChqAmount2IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChqNumber2";
                    if (!ChqNumber2IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChqAmount3";
                    if (!ChqAmount3IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChqNumber3";
                    if (!ChqNumber3IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChqAmount4";
                    if (!ChqAmount4IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChqNumber4";
                    if (!ChqNumber4IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.MasterEventIdValue = Source.MasterEventIdValue;
                this.ReceiptTypeValue = Source.ReceiptTypeValue;
                this.BankingIdValue = Source.BankingIdValue;
                this.FindText = Source.FindText;
                this.FindIdValue = Source.FindIdValue;
                this.SubLedgerIdValue = Source.SubLedgerIdValue;
                this.BankIdValue = Source.BankIdValue;
                this.RevenueIdValue = Source.RevenueIdValue;
                this.GST = Source.GST;
                this.ReceiptAmountValue = Source.ReceiptAmountValue;
                this.ReceiptGSTAmountValue = Source.ReceiptGSTAmountValue;
                this.ReceiptNoValue = Source.ReceiptNoValue;
                this.ReceiptDate = Source.ReceiptDate;
                this.ReceiptClearance = Source.ReceiptClearance;
                this.Detail = Source.Detail;
                this.Payee = Source.Payee;
                this.TakenBy = Source.TakenBy;
                this.ClientName = Source.ClientName;
                this.Address1 = Source.Address1;
                this.Address2 = Source.Address2;
                this.Address3 = Source.Address3;
                this.Address4 = Source.Address4;
                this.CashAmountValue = Source.CashAmountValue;
                this.ChqAmount1Value = Source.ChqAmount1Value;
                this.ChqBank1 = Source.ChqBank1;
                this.ChqBranch1 = Source.ChqBranch1;
                this.ChqDrawer1 = Source.ChqDrawer1;
                this.ChqNumber1Value = Source.ChqNumber1Value;
                this.ChqAmount2Value = Source.ChqAmount2Value;
                this.ChqBank2 = Source.ChqBank2;
                this.ChqBranch2 = Source.ChqBranch2;
                this.ChqDrawer2 = Source.ChqDrawer2;
                this.ChqNumber2Value = Source.ChqNumber2Value;
                this.ChqAmount3Value = Source.ChqAmount3Value;
                this.ChqBank3 = Source.ChqBank3;
                this.ChqBranch3 = Source.ChqBranch3;
                this.ChqDrawer3 = Source.ChqDrawer3;
                this.ChqNumber3Value = Source.ChqNumber3Value;
                this.ChqAmount4Value = Source.ChqAmount4Value;
                this.ChqBank4 = Source.ChqBank4;
                this.ChqBranch4 = Source.ChqBranch4;
                this.ChqDrawer4 = Source.ChqDrawer4;
                this.ChqNumber4Value = Source.ChqNumber4Value;
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
