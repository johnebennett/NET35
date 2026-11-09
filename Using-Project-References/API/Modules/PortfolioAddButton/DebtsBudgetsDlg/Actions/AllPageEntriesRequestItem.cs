/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 26-07-2020
 * Time: 5:22 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\PortfolioAddButton\DebtsBudgetsDlg\Actions\Templates\AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PortfolioAddButton.DebtsBudgetsDlg
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
        
        private string FBudgetIdValue = "";
        public bool BudgetIdIsValid = false;
        public int BudgetId { get; set; }
        public string BudgetIdValue
        {
            get { return FBudgetIdValue; }
            set
            {
                FBudgetIdValue = value;
                int Dummy = 0;
                BudgetIdIsValid = int.TryParse(FBudgetIdValue, out Dummy);
                BudgetId = (BudgetIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOptionIdValue = "";
        public bool OptionIdIsValid = false;
        public int OptionId { get; set; }
        public string OptionIdValue
        {
            get { return FOptionIdValue; }
            set
            {
                FOptionIdValue = value;
                int Dummy = 0;
                OptionIdIsValid = int.TryParse(FOptionIdValue, out Dummy);
                OptionId = (OptionIdIsValid) ? Dummy : 0;
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
        
        public string BudgetName { get; set; }
        
        public string FromDate { get; set; }
        
        public string ToDate { get; set; }
        
        private string FTotalBudgetValue = "";
        public bool TotalBudgetIsValid = false;
        public double TotalBudget { get; set; }
        public string TotalBudgetValue
        {
            get { return FTotalBudgetValue; }
            set
            {
                FTotalBudgetValue = value;
                double Dummy = 0.0;
                TotalBudgetIsValid = Double.TryParse(FTotalBudgetValue, out Dummy);
                TotalBudget = (TotalBudgetIsValid) ? Dummy : 0;
            }
        }
        
        private string FFrequencyValue = "";
        public bool FrequencyIsValid = false;
        public int Frequency { get; set; }
        public string FrequencyValue
        {
            get { return FFrequencyValue; }
            set
            {
                FFrequencyValue = value;
                int Dummy = 0;
                FrequencyIsValid = int.TryParse(FFrequencyValue, out Dummy);
                Frequency = (FrequencyIsValid) ? Dummy : 0;
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
        
        private string FPastBudgetIdValue = "";
        public bool PastBudgetIdIsValid = false;
        public int PastBudgetId { get; set; }
        public string PastBudgetIdValue
        {
            get { return FPastBudgetIdValue; }
            set
            {
                FPastBudgetIdValue = value;
                int Dummy = 0;
                PastBudgetIdIsValid = int.TryParse(FPastBudgetIdValue, out Dummy);
                PastBudgetId = (PastBudgetIdIsValid) ? Dummy : 0;
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
        
        private string FUsingIdValue = "";
        public bool UsingIdIsValid = false;
        public int UsingId { get; set; }
        public string UsingIdValue
        {
            get { return FUsingIdValue; }
            set
            {
                FUsingIdValue = value;
                int Dummy = 0;
                UsingIdIsValid = int.TryParse(FUsingIdValue, out Dummy);
                UsingId = (UsingIdIsValid) ? Dummy : 0;
            }
        }
        
        public string Struck { get; set; }
        
        public string DueDate { get; set; }
        
        private string FChargingIdValue = "";
        public bool ChargingIdIsValid = false;
        public int ChargingId { get; set; }
        public string ChargingIdValue
        {
            get { return FChargingIdValue; }
            set
            {
                FChargingIdValue = value;
                int Dummy = 0;
                ChargingIdIsValid = int.TryParse(FChargingIdValue, out Dummy);
                ChargingId = (ChargingIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FCalcMethodIdValue = "";
        public bool CalcMethodIdIsValid = false;
        public int CalcMethodId { get; set; }
        public string CalcMethodIdValue
        {
            get { return FCalcMethodIdValue; }
            set
            {
                FCalcMethodIdValue = value;
                int Dummy = 0;
                CalcMethodIdIsValid = int.TryParse(FCalcMethodIdValue, out Dummy);
                CalcMethodId = (CalcMethodIdIsValid) ? Dummy : 0;
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
            BudgetIdValue = "";
            OptionIdValue = "";
            ExpenseIdValue = "";
            BudgetName = "";
            FromDate = "";
            ToDate = "";
            TotalBudgetValue = "";
            FrequencyValue = "";
            RevenueIdValue = "";
            PastBudgetIdValue = "";
            SubLedgerIdValue = "";
            UsingIdValue = "";
            Struck = "";
            DueDate = "";
            ChargingIdValue = "";
            CalcMethodIdValue = "";
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
                    Key = "BudgetId";
                    if (!BudgetIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OptionId";
                    if (!OptionIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ExpenseId";
                    if (!ExpenseIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BudgetName";
                    if (string.IsNullOrEmpty(BudgetName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FromDate";
                    if (string.IsNullOrEmpty(FromDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ToDate";
                    if (string.IsNullOrEmpty(ToDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TotalBudget";
                    if (!TotalBudgetIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Frequency";
                    if (!FrequencyIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RevenueId";
                    if (!RevenueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PastBudgetId";
                    if (!PastBudgetIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SubLedgerId";
                    if (!SubLedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "UsingId";
                    if (!UsingIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Struck";
                    if (string.IsNullOrEmpty(Struck)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DueDate";
                    if (string.IsNullOrEmpty(DueDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargingId";
                    if (!ChargingIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CalcMethodId";
                    if (!CalcMethodIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.BudgetIdValue = Source.BudgetIdValue;
                this.OptionIdValue = Source.OptionIdValue;
                this.ExpenseIdValue = Source.ExpenseIdValue;
                this.BudgetName = Source.BudgetName;
                this.FromDate = Source.FromDate;
                this.ToDate = Source.ToDate;
                this.TotalBudgetValue = Source.TotalBudgetValue;
                this.FrequencyValue = Source.FrequencyValue;
                this.RevenueIdValue = Source.RevenueIdValue;
                this.PastBudgetIdValue = Source.PastBudgetIdValue;
                this.SubLedgerIdValue = Source.SubLedgerIdValue;
                this.UsingIdValue = Source.UsingIdValue;
                this.Struck = Source.Struck;
                this.DueDate = Source.DueDate;
                this.ChargingIdValue = Source.ChargingIdValue;
                this.CalcMethodIdValue = Source.CalcMethodIdValue;
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
