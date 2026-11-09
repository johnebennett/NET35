/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 22-08-2020
 * Time: 9:11 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/NewTransactionEventDlg/Actions/Templates/AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.NewTransactionEventDlg
{
    public class AllPageEntriesRequestItem
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
        
        private string FButtonNumberValue = "";
        public bool ButtonNumberIsValid = false;
        public int ButtonNumber { get; set; }
        public string ButtonNumberValue
        {
            get { return FButtonNumberValue; }
            set
            {
                FButtonNumberValue = value;
                int Dummy = 0;
                ButtonNumberIsValid = int.TryParse(FButtonNumberValue, out Dummy);
                ButtonNumber = (ButtonNumberIsValid) ? Dummy : 0;
            }
        }
        
        private string FTransTypeIdValue = "";
        public bool TransTypeIdIsValid = false;
        public int TransTypeId { get; set; }
        public string TransTypeIdValue
        {
            get { return FTransTypeIdValue; }
            set
            {
                FTransTypeIdValue = value;
                int Dummy = 0;
                TransTypeIdIsValid = int.TryParse(FTransTypeIdValue, out Dummy);
                TransTypeId = (TransTypeIdIsValid) ? Dummy : 0;
            }
        }
        
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
        
        public string FindText { get; set; }
        
        private string FNeededIdValue = "";
        public bool NeededIdIsValid = false;
        public int NeededId { get; set; }
        public string NeededIdValue
        {
            get { return FNeededIdValue; }
            set
            {
                FNeededIdValue = value;
                int Dummy = 0;
                NeededIdIsValid = int.TryParse(FNeededIdValue, out Dummy);
                NeededId = (NeededIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOptionalIdValue = "";
        public bool OptionalIdIsValid = false;
        public int OptionalId { get; set; }
        public string OptionalIdValue
        {
            get { return FOptionalIdValue; }
            set
            {
                FOptionalIdValue = value;
                int Dummy = 0;
                OptionalIdIsValid = int.TryParse(FOptionalIdValue, out Dummy);
                OptionalId = (OptionalIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FLedgerFindIdValue = "";
        public bool LedgerFindIdIsValid = false;
        public int LedgerFindId { get; set; }
        public string LedgerFindIdValue
        {
            get { return FLedgerFindIdValue; }
            set
            {
                FLedgerFindIdValue = value;
                int Dummy = 0;
                LedgerFindIdIsValid = int.TryParse(FLedgerFindIdValue, out Dummy);
                LedgerFindId = (LedgerFindIdIsValid) ? Dummy : 0;
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
        
        public string SaleDate { get; set; }
        
        private string FSalePriceValue = "";
        public bool SalePriceIsValid = false;
        public double SalePrice { get; set; }
        public string SalePriceValue
        {
            get { return FSalePriceValue; }
            set
            {
                FSalePriceValue = value;
                double Dummy = 0.0;
                SalePriceIsValid = Double.TryParse(FSalePriceValue, out Dummy);
                SalePrice = (SalePriceIsValid) ? Dummy : 0;
            }
        }
        
        private string FSaleDepositValue = "";
        public bool SaleDepositIsValid = false;
        public double SaleDeposit { get; set; }
        public string SaleDepositValue
        {
            get { return FSaleDepositValue; }
            set
            {
                FSaleDepositValue = value;
                double Dummy = 0.0;
                SaleDepositIsValid = Double.TryParse(FSaleDepositValue, out Dummy);
                SaleDeposit = (SaleDepositIsValid) ? Dummy : 0;
            }
        }
        
        private string FSaleCommissionValue = "";
        public bool SaleCommissionIsValid = false;
        public double SaleCommission { get; set; }
        public string SaleCommissionValue
        {
            get { return FSaleCommissionValue; }
            set
            {
                FSaleCommissionValue = value;
                double Dummy = 0.0;
                SaleCommissionIsValid = Double.TryParse(FSaleCommissionValue, out Dummy);
                SaleCommission = (SaleCommissionIsValid) ? Dummy : 0;
            }
        }
        
        private string FSaleAdvertisingValue = "";
        public bool SaleAdvertisingIsValid = false;
        public double SaleAdvertising { get; set; }
        public string SaleAdvertisingValue
        {
            get { return FSaleAdvertisingValue; }
            set
            {
                FSaleAdvertisingValue = value;
                double Dummy = 0.0;
                SaleAdvertisingIsValid = Double.TryParse(FSaleAdvertisingValue, out Dummy);
                SaleAdvertising = (SaleAdvertisingIsValid) ? Dummy : 0;
            }
        }
        
        private string FCommissionRateValue = "";
        public bool CommissionRateIsValid = false;
        public double CommissionRate { get; set; }
        public string CommissionRateValue
        {
            get { return FCommissionRateValue; }
            set
            {
                FCommissionRateValue = value;
                double Dummy = 0.0;
                CommissionRateIsValid = Double.TryParse(FCommissionRateValue, out Dummy);
                CommissionRate = (CommissionRateIsValid) ? Dummy : 0;
            }
        }
        
        private string FStatementFeeValue = "";
        public bool StatementFeeIsValid = false;
        public double StatementFee { get; set; }
        public string StatementFeeValue
        {
            get { return FStatementFeeValue; }
            set
            {
                FStatementFeeValue = value;
                double Dummy = 0.0;
                StatementFeeIsValid = Double.TryParse(FStatementFeeValue, out Dummy);
                StatementFee = (StatementFeeIsValid) ? Dummy : 0;
            }
        }
        
        public string GSTRegistered { get; set; }
        
        public string LedgerABN { get; set; }
        
        private string FRentGrossValue = "";
        public bool RentGrossIsValid = false;
        public double RentGross { get; set; }
        public string RentGrossValue
        {
            get { return FRentGrossValue; }
            set
            {
                FRentGrossValue = value;
                double Dummy = 0.0;
                RentGrossIsValid = Double.TryParse(FRentGrossValue, out Dummy);
                RentGross = (RentGrossIsValid) ? Dummy : 0;
            }
        }
        
        private string FRentGSTValue = "";
        public bool RentGSTIsValid = false;
        public double RentGST { get; set; }
        public string RentGSTValue
        {
            get { return FRentGSTValue; }
            set
            {
                FRentGSTValue = value;
                double Dummy = 0.0;
                RentGSTIsValid = Double.TryParse(FRentGSTValue, out Dummy);
                RentGST = (RentGSTIsValid) ? Dummy : 0;
            }
        }
        
        public string RentPaidFrom { get; set; }
        
        private string FRentCreditAmountValue = "";
        public bool RentCreditAmountIsValid = false;
        public double RentCreditAmount { get; set; }
        public string RentCreditAmountValue
        {
            get { return FRentCreditAmountValue; }
            set
            {
                FRentCreditAmountValue = value;
                double Dummy = 0.0;
                RentCreditAmountIsValid = Double.TryParse(FRentCreditAmountValue, out Dummy);
                RentCreditAmount = (RentCreditAmountIsValid) ? Dummy : 0;
            }
        }
        
        public string RentPaidToInclusive { get; set; }
        
        private string FLettingFeeValue = "";
        public bool LettingFeeIsValid = false;
        public double LettingFee { get; set; }
        public string LettingFeeValue
        {
            get { return FLettingFeeValue; }
            set
            {
                FLettingFeeValue = value;
                double Dummy = 0.0;
                LettingFeeIsValid = Double.TryParse(FLettingFeeValue, out Dummy);
                LettingFee = (LettingFeeIsValid) ? Dummy : 0;
            }
        }
        
        private string FTenancyAdvertisingValue = "";
        public bool TenancyAdvertisingIsValid = false;
        public double TenancyAdvertising { get; set; }
        public string TenancyAdvertisingValue
        {
            get { return FTenancyAdvertisingValue; }
            set
            {
                FTenancyAdvertisingValue = value;
                double Dummy = 0.0;
                TenancyAdvertisingIsValid = Double.TryParse(FTenancyAdvertisingValue, out Dummy);
                TenancyAdvertising = (TenancyAdvertisingIsValid) ? Dummy : 0;
            }
        }
        
        private string FRentFrequencyValue = "";
        public bool RentFrequencyIsValid = false;
        public int RentFrequency { get; set; }
        public string RentFrequencyValue
        {
            get { return FRentFrequencyValue; }
            set
            {
                FRentFrequencyValue = value;
                int Dummy = 0;
                RentFrequencyIsValid = int.TryParse(FRentFrequencyValue, out Dummy);
                RentFrequency = (RentFrequencyIsValid) ? Dummy : 0;
            }
        }
        
        public string ShowPreparationFee { get; set; }
        
        private string FPreparationLandlordFeeValue = "";
        public bool PreparationLandlordFeeIsValid = false;
        public double PreparationLandlordFee { get; set; }
        public string PreparationLandlordFeeValue
        {
            get { return FPreparationLandlordFeeValue; }
            set
            {
                FPreparationLandlordFeeValue = value;
                double Dummy = 0.0;
                PreparationLandlordFeeIsValid = Double.TryParse(FPreparationLandlordFeeValue, out Dummy);
                PreparationLandlordFee = (PreparationLandlordFeeIsValid) ? Dummy : 0;
            }
        }
        
        private string FPreparationTenantFeeValue = "";
        public bool PreparationTenantFeeIsValid = false;
        public double PreparationTenantFee { get; set; }
        public string PreparationTenantFeeValue
        {
            get { return FPreparationTenantFeeValue; }
            set
            {
                FPreparationTenantFeeValue = value;
                double Dummy = 0.0;
                PreparationTenantFeeIsValid = Double.TryParse(FPreparationTenantFeeValue, out Dummy);
                PreparationTenantFee = (PreparationTenantFeeIsValid) ? Dummy : 0;
            }
        }
        
        private string FBondAmountValue = "";
        public bool BondAmountIsValid = false;
        public double BondAmount { get; set; }
        public string BondAmountValue
        {
            get { return FBondAmountValue; }
            set
            {
                FBondAmountValue = value;
                double Dummy = 0.0;
                BondAmountIsValid = Double.TryParse(FBondAmountValue, out Dummy);
                BondAmount = (BondAmountIsValid) ? Dummy : 0;
            }
        }
        
        private string FNoLotsValue = "";
        public bool NoLotsIsValid = false;
        public int NoLots { get; set; }
        public string NoLotsValue
        {
            get { return FNoLotsValue; }
            set
            {
                FNoLotsValue = value;
                int Dummy = 0;
                NoLotsIsValid = int.TryParse(FNoLotsValue, out Dummy);
                NoLots = (NoLotsIsValid) ? Dummy : 0;
            }
        }
        
        private string FFirstLotNoValue = "";
        public bool FirstLotNoIsValid = false;
        public int FirstLotNo { get; set; }
        public string FirstLotNoValue
        {
            get { return FFirstLotNoValue; }
            set
            {
                FFirstLotNoValue = value;
                int Dummy = 0;
                FirstLotNoIsValid = int.TryParse(FFirstLotNoValue, out Dummy);
                FirstLotNo = (FirstLotNoIsValid) ? Dummy : 0;
            }
        }
        
        private string FFirstUnitNoValue = "";
        public bool FirstUnitNoIsValid = false;
        public int FirstUnitNo { get; set; }
        public string FirstUnitNoValue
        {
            get { return FFirstUnitNoValue; }
            set
            {
                FFirstUnitNoValue = value;
                int Dummy = 0;
                FirstUnitNoIsValid = int.TryParse(FFirstUnitNoValue, out Dummy);
                FirstUnitNo = (FirstUnitNoIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            ButtonNumberValue = "";
            TransTypeIdValue = "";
            MasterEventIdValue = "";
            FindIdValue = "";
            NeededIdValue = "";
            OptionalIdValue = "";
            LedgerFindIdValue = "";
            BankAccountIdValue = "";
            SalePriceValue = "";
            SaleDepositValue = "";
            SaleCommissionValue = "";
            SaleAdvertisingValue = "";
            CommissionRateValue = "";
            StatementFeeValue = "";
            RentGrossValue = "";
            RentGSTValue = "";
            RentCreditAmountValue = "";
            RentPaidToInclusive = "";
            LettingFeeValue = "";
            TenancyAdvertisingValue = "";
            RentFrequencyValue = "";
            ShowPreparationFee = "";
            PreparationLandlordFeeValue = "";
            PreparationTenantFeeValue = "";
            BondAmountValue = "";
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
                    Key = "ButtonNumber";
                    if (!ButtonNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransTypeId";
                    if (!TransTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MasterEventId";
                    if (!MasterEventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FindId";
                    if (!FindIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "NeededId";
                    if (!NeededIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OptionalId";
                    if (!OptionalIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LedgerFindId";
                    if (!LedgerFindIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankAccountId";
                    if (!BankAccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SalePrice";
                    if (!SalePriceIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SaleDeposit";
                    if (!SaleDepositIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SaleCommission";
                    if (!SaleCommissionIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SaleAdvertising";
                    if (!SaleAdvertisingIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CommissionRate";
                    if (!CommissionRateIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StatementFee";
                    if (!StatementFeeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RentGross";
                    if (!RentGrossIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RentGST";
                    if (!RentGSTIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RentCreditAmount";
                    if (!RentCreditAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RentPaidToInclusive";
                    if (string.IsNullOrEmpty(RentPaidToInclusive)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LettingFee";
                    if (!LettingFeeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TenancyAdvertising";
                    if (!TenancyAdvertisingIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RentFrequency";
                    if (!RentFrequencyIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowPreparationFee";
                    if (string.IsNullOrEmpty(ShowPreparationFee)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PreparationLandlordFee";
                    if (!PreparationLandlordFeeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PreparationTenantFee";
                    if (!PreparationTenantFeeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BondAmount";
                    if (!BondAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(AllPageEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.ButtonNumberValue = Source.ButtonNumberValue;
                this.TransTypeIdValue = Source.TransTypeIdValue;
                this.MasterEventIdValue = Source.MasterEventIdValue;
                this.FindIdValue = Source.FindIdValue;
                this.FindText = Source.FindText;
                this.NeededIdValue = Source.NeededIdValue;
                this.OptionalIdValue = Source.OptionalIdValue;
                this.LedgerFindIdValue = Source.LedgerFindIdValue;
                this.BankAccountIdValue = Source.BankAccountIdValue;
                this.SaleDate = Source.SaleDate;
                this.SalePriceValue = Source.SalePriceValue;
                this.SaleDepositValue = Source.SaleDepositValue;
                this.SaleCommissionValue = Source.SaleCommissionValue;
                this.SaleAdvertisingValue = Source.SaleAdvertisingValue;
                this.CommissionRateValue = Source.CommissionRateValue;
                this.StatementFeeValue = Source.StatementFeeValue;
                this.GSTRegistered = Source.GSTRegistered;
                this.LedgerABN = Source.LedgerABN;
                this.RentGrossValue = Source.RentGrossValue;
                this.RentGSTValue = Source.RentGSTValue;
                this.RentPaidFrom = Source.RentPaidFrom;
                this.RentCreditAmountValue = Source.RentCreditAmountValue;
                this.RentPaidToInclusive = Source.RentPaidToInclusive;
                this.LettingFeeValue = Source.LettingFeeValue;
                this.TenancyAdvertisingValue = Source.TenancyAdvertisingValue;
                this.RentFrequencyValue = Source.RentFrequencyValue;
                this.ShowPreparationFee = Source.ShowPreparationFee;
                this.PreparationLandlordFeeValue = Source.PreparationLandlordFeeValue;
                this.PreparationTenantFeeValue = Source.PreparationTenantFeeValue;
                this.BondAmountValue = Source.BondAmountValue;
                this.NoLotsValue = Source.NoLotsValue;
                this.FirstLotNoValue = Source.FirstLotNoValue;
                this.FirstUnitNoValue = Source.FirstUnitNoValue;
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
            
                    AllPageEntriesRequestItem TempItem = new AllPageEntriesRequestItem();
                    //Result = CommonMethods.LoadAllPageEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadAllPageEntriesRequestItem(ActionBase, ref TempItem);
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
