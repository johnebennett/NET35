/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 29-06-2020
 * Time: 3:07 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\PortfolioAddButton\Actions\Templates\RecurringInvoicesDlg\AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PortfolioAddButton.RecurringInvoicesDlg
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
        
        private string FOwnerSubLedgerIdValue = "";
        public bool OwnerSubLedgerIdIsValid = false;
        public int OwnerSubLedgerId { get; set; }
        public string OwnerSubLedgerIdValue
        {
            get { return FOwnerSubLedgerIdValue; }
            set
            {
                FOwnerSubLedgerIdValue = value;
                int Dummy = 0;
                OwnerSubLedgerIdIsValid = int.TryParse(FOwnerSubLedgerIdValue, out Dummy);
                OwnerSubLedgerId = (OwnerSubLedgerIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FCreditorSubLedgerIdValue = "";
        public bool CreditorSubLedgerIdIsValid = false;
        public int CreditorSubLedgerId { get; set; }
        public string CreditorSubLedgerIdValue
        {
            get { return FCreditorSubLedgerIdValue; }
            set
            {
                FCreditorSubLedgerIdValue = value;
                int Dummy = 0;
                CreditorSubLedgerIdIsValid = int.TryParse(FCreditorSubLedgerIdValue, out Dummy);
                CreditorSubLedgerId = (CreditorSubLedgerIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FFrequencyIdValue = "";
        public bool FrequencyIdIsValid = false;
        public int FrequencyId { get; set; }
        public string FrequencyIdValue
        {
            get { return FFrequencyIdValue; }
            set
            {
                FFrequencyIdValue = value;
                int Dummy = 0;
                FrequencyIdIsValid = int.TryParse(FFrequencyIdValue, out Dummy);
                FrequencyId = (FrequencyIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FFreqPeriodValue = "";
        public bool FreqPeriodIsValid = false;
        public int FreqPeriod { get; set; }
        public string FreqPeriodValue
        {
            get { return FFreqPeriodValue; }
            set
            {
                FFreqPeriodValue = value;
                int Dummy = 0;
                FreqPeriodIsValid = int.TryParse(FFreqPeriodValue, out Dummy);
                FreqPeriod = (FreqPeriodIsValid) ? Dummy : 0;
            }
        }
        
        private string FInvAmountValue = "";
        public bool InvAmountIsValid = false;
        public double InvAmount { get; set; }
        public string InvAmountValue
        {
            get { return FInvAmountValue; }
            set
            {
                FInvAmountValue = value;
                double Dummy = 0.0;
                InvAmountIsValid = Double.TryParse(FInvAmountValue, out Dummy);
                InvAmount = (InvAmountIsValid) ? Dummy : 0;
            }
        }
        
        private string FGSTAmountValue = "";
        public bool GSTAmountIsValid = false;
        public double GSTAmount { get; set; }
        public string GSTAmountValue
        {
            get { return FGSTAmountValue; }
            set
            {
                FGSTAmountValue = value;
                double Dummy = 0.0;
                GSTAmountIsValid = Double.TryParse(FGSTAmountValue, out Dummy);
                GSTAmount = (GSTAmountIsValid) ? Dummy : 0;
            }
        }
        
        public string Created { get; set; }
        
        public string InvDetail { get; set; }
        
        public string BPAY { get; set; }
        
        public string Starts { get; set; }
        
        public string Stops { get; set; }
        
        public string NextIncDate { get; set; }
        
        private string FNextIncAmountValue = "";
        public bool NextIncAmountIsValid = false;
        public double NextIncAmount { get; set; }
        public string NextIncAmountValue
        {
            get { return FNextIncAmountValue; }
            set
            {
                FNextIncAmountValue = value;
                double Dummy = 0.0;
                NextIncAmountIsValid = Double.TryParse(FNextIncAmountValue, out Dummy);
                NextIncAmount = (NextIncAmountIsValid) ? Dummy : 0;
            }
        }
        
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
            RevenueIdValue = "";
            OwnerSubLedgerIdValue = "";
            CreditorSubLedgerIdValue = "";
            FrequencyIdValue = "";
            FreqPeriodValue = "";
            InvAmountValue = "";
            GSTAmountValue = "";
            Created = "";
            InvDetail = "";
            Starts = "";
            Stops = "";
            NextIncDate = "";
            NextIncAmountValue = "";
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
                    Key = "RevenueId";
                    if (!RevenueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OwnerSubLedgerId";
                    if (!OwnerSubLedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CreditorSubLedgerId";
                    if (!CreditorSubLedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FrequencyId";
                    if (!FrequencyIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FreqPeriod";
                    if (!FreqPeriodIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvAmount";
                    if (!InvAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GSTAmount";
                    if (!GSTAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Created";
                    if (string.IsNullOrEmpty(Created)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvDetail";
                    if (string.IsNullOrEmpty(InvDetail)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Starts";
                    if (string.IsNullOrEmpty(Starts)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Stops";
                    if (string.IsNullOrEmpty(Stops)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "NextIncDate";
                    if (string.IsNullOrEmpty(NextIncDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "NextIncAmount";
                    if (!NextIncAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.RevenueIdValue = Source.RevenueIdValue;
                this.OwnerSubLedgerIdValue = Source.OwnerSubLedgerIdValue;
                this.CreditorSubLedgerIdValue = Source.CreditorSubLedgerIdValue;
                this.FrequencyIdValue = Source.FrequencyIdValue;
                this.FreqPeriodValue = Source.FreqPeriodValue;
                this.InvAmountValue = Source.InvAmountValue;
                this.GSTAmountValue = Source.GSTAmountValue;
                this.Created = Source.Created;
                this.InvDetail = Source.InvDetail;
                this.BPAY = Source.BPAY;
                this.Starts = Source.Starts;
                this.Stops = Source.Stops;
                this.NextIncDate = Source.NextIncDate;
                this.NextIncAmountValue = Source.NextIncAmountValue;
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
