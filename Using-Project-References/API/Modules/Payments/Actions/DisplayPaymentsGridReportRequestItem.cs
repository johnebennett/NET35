/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 28-10-2020
 * Time: 8:38 AM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Payments/Actions/Templates/DisplayPaymentsGridReportRequestItem.json
*/
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
using CommonDBMethods;

namespace Actions.PaymentOperations
{
    public class DisplayPaymentsGridReportRequestItem
    {
        #region Properties - Getters and Setters
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
        
        private string FPaymentTypeIdValue = "";
        public bool PaymentTypeIdIsValid = false;
        public int PaymentTypeId { get; set; }
        public string PaymentTypeIdValue
        {
            get { return FPaymentTypeIdValue; }
            set
            {
                FPaymentTypeIdValue = value;
                int Dummy = 0;
                PaymentTypeIdIsValid = int.TryParse(FPaymentTypeIdValue, out Dummy);
                PaymentTypeId = (PaymentTypeIdIsValid) ? Dummy : 0;
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
        
        private string FStatementRunIdValue = "";
        public bool StatementRunIdIsValid = false;
        public int StatementRunId { get; set; }
        public string StatementRunIdValue
        {
            get { return FStatementRunIdValue; }
            set
            {
                FStatementRunIdValue = value;
                int Dummy = 0;
                StatementRunIdIsValid = int.TryParse(FStatementRunIdValue, out Dummy);
                StatementRunId = (StatementRunIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FDateRangeIdValue = "";
        public bool DateRangeIdIsValid = false;
        public int DateRangeId { get; set; }
        public string DateRangeIdValue
        {
            get { return FDateRangeIdValue; }
            set
            {
                FDateRangeIdValue = value;
                int Dummy = 0;
                DateRangeIdIsValid = int.TryParse(FDateRangeIdValue, out Dummy);
                DateRangeId = (DateRangeIdIsValid) ? Dummy : 0;
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
        
        private string FBtnIdValue = "";
        public bool BtnIdIsValid = false;
        public int BtnId { get; set; }
        public string BtnIdValue
        {
            get { return FBtnIdValue; }
            set
            {
                FBtnIdValue = value;
                int Dummy = 0;
                BtnIdIsValid = int.TryParse(FBtnIdValue, out Dummy);
                BtnId = (BtnIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            OfficeIdValue = "";
            ManagerIdValue = "";
            LedgerTypeIdValue = "";
            LedgerIdValue = "";
            BankNameIdValue = "";
            PaymentTypeIdValue = "";
            DisplayTypeValue = "";
            StatementRunIdValue = "";
            DateRangeIdValue = "";
            PassedIdValue = "";
            BtnIdValue = "";
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
                    Key = "OfficeId";
                    if (!OfficeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ManagerId";
                    if (!ManagerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    Key = "BankNameId";
                    if (!BankNameIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PaymentTypeId";
                    if (!PaymentTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DisplayType";
                    if (!DisplayTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StatementRunId";
                    if (!StatementRunIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DateRangeId";
                    if (!DateRangeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PassedId";
                    if (!PassedIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BtnId";
                    if (!BtnIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(DisplayPaymentsGridReportRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.OfficeIdValue = Source.OfficeIdValue;
                this.ManagerIdValue = Source.ManagerIdValue;
                this.LedgerTypeIdValue = Source.LedgerTypeIdValue;
                this.LedgerIdValue = Source.LedgerIdValue;
                this.BankNameIdValue = Source.BankNameIdValue;
                this.PaymentTypeIdValue = Source.PaymentTypeIdValue;
                this.DisplayTypeValue = Source.DisplayTypeValue;
                this.StatementRunIdValue = Source.StatementRunIdValue;
                this.DateRangeIdValue = Source.DateRangeIdValue;
                this.PassedIdValue = Source.PassedIdValue;
                this.BtnIdValue = Source.BtnIdValue;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load
        public string Load(ActionBase AActionBase, JObject DataObject)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".LoadFromDataObject";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    DisplayPaymentsGridReportRequestItem TempItem = new DisplayPaymentsGridReportRequestItem();
                    Result = CommonMethods.LoadDisplayPaymentsGridReportRequestItem(DataObject, ref TempItem);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = AActionBase.LogOnProcessingError(Method, Result, "");
                    }
                    else
                    {
                        Result = Update(TempItem);
                    }
                }
                catch (Exception Ex)
                {
                    Result = AActionBase.LogOnException(Method, Ex, "");
                    return Result;
                }
                finally
                {
                    AActionBase.LogOnLeavingMethod(Method);
                }
            
                return Result;
        }
        #endregion
        
    }
}
