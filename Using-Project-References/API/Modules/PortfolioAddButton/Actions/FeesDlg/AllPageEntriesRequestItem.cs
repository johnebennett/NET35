/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 30-06-2020
 * Time: 6:40 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\PortfolioAddButton\Actions\Templates\FeesDlg\AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PortfolioAddButton.FeesDlg
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
        
        private string FEventTypeValue = "";
        public bool EventTypeIsValid = false;
        public int EventType { get; set; }
        public string EventTypeValue
        {
            get { return FEventTypeValue; }
            set
            {
                FEventTypeValue = value;
                int Dummy = 0;
                EventTypeIsValid = int.TryParse(FEventTypeValue, out Dummy);
                EventType = (EventTypeIsValid) ? Dummy : 0;
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
        
        private string FCreditorSubLedgerValue = "";
        public bool CreditorSubLedgerIsValid = false;
        public int CreditorSubLedger { get; set; }
        public string CreditorSubLedgerValue
        {
            get { return FCreditorSubLedgerValue; }
            set
            {
                FCreditorSubLedgerValue = value;
                int Dummy = 0;
                CreditorSubLedgerIsValid = int.TryParse(FCreditorSubLedgerValue, out Dummy);
                CreditorSubLedger = (CreditorSubLedgerIsValid) ? Dummy : 0;
            }
        }
        
        private string FChartCodeValue = "";
        public bool ChartCodeIsValid = false;
        public int ChartCode { get; set; }
        public string ChartCodeValue
        {
            get { return FChartCodeValue; }
            set
            {
                FChartCodeValue = value;
                int Dummy = 0;
                ChartCodeIsValid = int.TryParse(FChartCodeValue, out Dummy);
                ChartCode = (ChartCodeIsValid) ? Dummy : 0;
            }
        }
        
        private string FExclusion1Value = "";
        public bool Exclusion1IsValid = false;
        public int Exclusion1 { get; set; }
        public string Exclusion1Value
        {
            get { return FExclusion1Value; }
            set
            {
                FExclusion1Value = value;
                int Dummy = 0;
                Exclusion1IsValid = int.TryParse(FExclusion1Value, out Dummy);
                Exclusion1 = (Exclusion1IsValid) ? Dummy : 0;
            }
        }
        
        private string FExclusion2Value = "";
        public bool Exclusion2IsValid = false;
        public int Exclusion2 { get; set; }
        public string Exclusion2Value
        {
            get { return FExclusion2Value; }
            set
            {
                FExclusion2Value = value;
                int Dummy = 0;
                Exclusion2IsValid = int.TryParse(FExclusion2Value, out Dummy);
                Exclusion2 = (Exclusion2IsValid) ? Dummy : 0;
            }
        }
        
        public string InvoiceGSTReceipts { get; set; }
        
        private string FRevenueValue = "";
        public bool RevenueIsValid = false;
        public int Revenue { get; set; }
        public string RevenueValue
        {
            get { return FRevenueValue; }
            set
            {
                FRevenueValue = value;
                int Dummy = 0;
                RevenueIsValid = int.TryParse(FRevenueValue, out Dummy);
                Revenue = (RevenueIsValid) ? Dummy : 0;
            }
        }
        
        public string Detail { get; set; }
        
        private string FPercentRateValue = "";
        public bool PercentRateIsValid = false;
        public double PercentRate { get; set; }
        public string PercentRateValue
        {
            get { return FPercentRateValue; }
            set
            {
                FPercentRateValue = value;
                double Dummy = 0.0;
                PercentRateIsValid = Double.TryParse(FPercentRateValue, out Dummy);
                PercentRate = (PercentRateIsValid) ? Dummy : 0;
            }
        }
        
        private string FFixedAmountValue = "";
        public bool FixedAmountIsValid = false;
        public double FixedAmount { get; set; }
        public string FixedAmountValue
        {
            get { return FFixedAmountValue; }
            set
            {
                FFixedAmountValue = value;
                double Dummy = 0.0;
                FixedAmountIsValid = Double.TryParse(FFixedAmountValue, out Dummy);
                FixedAmount = (FixedAmountIsValid) ? Dummy : 0;
            }
        }
        
        public string ReverseInvoiceSign { get; set; }
        
        public string RecallDate { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            Option1IdValue = "";
            Option2IdValue = "";
            GridIdValue = "";
            FindIdValue = "";
            ExpenseIdValue = "";
            EventTypeValue = "";
            SubLedgerIdValue = "";
            CreditorSubLedgerValue = "";
            ChartCodeValue = "";
            Exclusion1Value = "";
            Exclusion2Value = "";
            InvoiceGSTReceipts = "";
            RevenueValue = "";
            Detail = "";
            PercentRateValue = "";
            FixedAmountValue = "";
            ReverseInvoiceSign = "";
            RecallDate = "";
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
                    Key = "FindId";
                    if (!FindIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ExpenseId";
                    if (!ExpenseIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EventType";
                    if (!EventTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SubLedgerId";
                    if (!SubLedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CreditorSubLedger";
                    if (!CreditorSubLedgerIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChartCode";
                    if (!ChartCodeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Exclusion1";
                    if (!Exclusion1IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Exclusion2";
                    if (!Exclusion2IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvoiceGSTReceipts";
                    if (string.IsNullOrEmpty(InvoiceGSTReceipts)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Revenue";
                    if (!RevenueIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Detail";
                    if (string.IsNullOrEmpty(Detail)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PercentRate";
                    if (!PercentRateIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FixedAmount";
                    if (!FixedAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReverseInvoiceSign";
                    if (string.IsNullOrEmpty(ReverseInvoiceSign)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RecallDate";
                    if (string.IsNullOrEmpty(RecallDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.Option1IdValue = Source.Option1IdValue;
                this.Option2IdValue = Source.Option2IdValue;
                this.GridIdValue = Source.GridIdValue;
                this.FindIdValue = Source.FindIdValue;
                this.ExpenseIdValue = Source.ExpenseIdValue;
                this.EventTypeValue = Source.EventTypeValue;
                this.SubLedgerIdValue = Source.SubLedgerIdValue;
                this.CreditorSubLedgerValue = Source.CreditorSubLedgerValue;
                this.ChartCodeValue = Source.ChartCodeValue;
                this.Exclusion1Value = Source.Exclusion1Value;
                this.Exclusion2Value = Source.Exclusion2Value;
                this.InvoiceGSTReceipts = Source.InvoiceGSTReceipts;
                this.RevenueValue = Source.RevenueValue;
                this.Detail = Source.Detail;
                this.PercentRateValue = Source.PercentRateValue;
                this.FixedAmountValue = Source.FixedAmountValue;
                this.ReverseInvoiceSign = Source.ReverseInvoiceSign;
                this.RecallDate = Source.RecallDate;
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
