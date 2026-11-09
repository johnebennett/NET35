/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 26-08-2020
 * Time: 4:00 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/NewTransactionEventDlg/Actions/Templates/CreateLedgerRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.NewTransactionEventDlg
{
    public class CreateLedgerRequestItem
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
        
        public string ABN { get; set; }
        
        public string GSTRegistered { get; set; }
        
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
            GSTRegistered = "";
            CommissionRateValue = "";
            StmtFeeValue = "";
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
                    Key = "GSTRegistered";
                    if (string.IsNullOrEmpty(GSTRegistered)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CommissionRate";
                    if (!CommissionRateIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StmtFee";
                    if (!StmtFeeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(CreateLedgerRequestItem Source)
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
                this.ABN = Source.ABN;
                this.GSTRegistered = Source.GSTRegistered;
                this.CommissionRateValue = Source.CommissionRateValue;
                this.StmtFeeValue = Source.StmtFeeValue;
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
            
                    CreateLedgerRequestItem TempItem = new CreateLedgerRequestItem();
                    //Result = CommonMethods.LoadCreateLedgerRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadCreateLedgerRequestItem(ActionBase, ref TempItem);
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
