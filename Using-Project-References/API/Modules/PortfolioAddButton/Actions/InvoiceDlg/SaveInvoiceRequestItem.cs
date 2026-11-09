/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 16-08-2021
 * Time: 5:23 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/PortfolioAddButton/Actions/Templates/InvoiceDlg/SaveInvoiceRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PortfolioAddButton.InvoiceDlg
{
    public class SaveInvoiceRequestItem
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
        
        public string InvDate { get; set; }
        
        public string InvPayDate { get; set; }
        
        public string InvNumber { get; set; }
        
        public string InvBPay { get; set; }
        
        public string InvDetail { get; set; }
        
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
        
        private string FInvGSTValue = "";
        public bool InvGSTIsValid = false;
        public double InvGST { get; set; }
        public string InvGSTValue
        {
            get { return FInvGSTValue; }
            set
            {
                FInvGSTValue = value;
                double Dummy = 0.0;
                InvGSTIsValid = Double.TryParse(FInvGSTValue, out Dummy);
                InvGST = (InvGSTIsValid) ? Dummy : 0;
            }
        }
        
        public string HoldForTenant { get; set; }
        
        public string HoldForManager { get; set; }
        
        private string FPriorityValue = "";
        public bool PriorityIsValid = false;
        public int Priority { get; set; }
        public string PriorityValue
        {
            get { return FPriorityValue; }
            set
            {
                FPriorityValue = value;
                int Dummy = 0;
                PriorityIsValid = int.TryParse(FPriorityValue, out Dummy);
                Priority = (PriorityIsValid) ? Dummy : 0;
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
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            Option1IdValue = "";
            Option2IdValue = "";
            GridIdValue = "";
            InvDate = "";
            InvPayDate = "";
            InvDetail = "";
            InvAmountValue = "";
            InvGSTValue = "";
            HoldForTenant = "";
            HoldForManager = "";
            PriorityValue = "";
            ExpenseIdValue = "";
            RevenueIdValue = "";
            SubLedgerIdValue = "";
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
                    Key = "InvDate";
                    if (string.IsNullOrEmpty(InvDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvPayDate";
                    if (string.IsNullOrEmpty(InvPayDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvDetail";
                    if (string.IsNullOrEmpty(InvDetail)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvAmount";
                    if (!InvAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvGST";
                    if (!InvGSTIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "HoldForTenant";
                    if (string.IsNullOrEmpty(HoldForTenant)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "HoldForManager";
                    if (string.IsNullOrEmpty(HoldForManager)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Priority";
                    if (!PriorityIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    Key = "SubLedgerId";
                    if (!SubLedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SaveInvoiceRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.Option1IdValue = Source.Option1IdValue;
                this.Option2IdValue = Source.Option2IdValue;
                this.GridIdValue = Source.GridIdValue;
                this.InvDate = Source.InvDate;
                this.InvPayDate = Source.InvPayDate;
                this.InvNumber = Source.InvNumber;
                this.InvBPay = Source.InvBPay;
                this.InvDetail = Source.InvDetail;
                this.InvAmountValue = Source.InvAmountValue;
                this.InvGSTValue = Source.InvGSTValue;
                this.HoldForTenant = Source.HoldForTenant;
                this.HoldForManager = Source.HoldForManager;
                this.PriorityValue = Source.PriorityValue;
                this.ExpenseIdValue = Source.ExpenseIdValue;
                this.RevenueIdValue = Source.RevenueIdValue;
                this.SubLedgerIdValue = Source.SubLedgerIdValue;
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
            
                    SaveInvoiceRequestItem TempItem = new SaveInvoiceRequestItem();
                    //Result = CommonMethods.LoadSaveInvoiceRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSaveInvoiceRequestItem(ActionBase, ref TempItem);
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
