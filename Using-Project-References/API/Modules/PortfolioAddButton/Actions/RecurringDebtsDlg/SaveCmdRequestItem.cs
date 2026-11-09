/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 19-02-2021
 * Time: 5:15 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/PortfolioAddButton/Actions/Templates/RecurringDebtsDlg/SaveCmdRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PortfolioAddButton.RecurringDebtsDlg
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
        
        public string FromDate { get; set; }
        
        public string Inclusive { get; set; }
        
        private string FChargeAmountValue = "";
        public bool ChargeAmountIsValid = false;
        public double ChargeAmount { get; set; }
        public string ChargeAmountValue
        {
            get { return FChargeAmountValue; }
            set
            {
                FChargeAmountValue = value;
                double Dummy = 0.0;
                ChargeAmountIsValid = Double.TryParse(FChargeAmountValue, out Dummy);
                ChargeAmount = (ChargeAmountIsValid) ? Dummy : 0;
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
        
        public string ToDate { get; set; }
        
        public string Detail { get; set; }
        
        private string FDueBeforeValue = "";
        public bool DueBeforeIsValid = false;
        public int DueBefore { get; set; }
        public string DueBeforeValue
        {
            get { return FDueBeforeValue; }
            set
            {
                FDueBeforeValue = value;
                int Dummy = 0;
                DueBeforeIsValid = int.TryParse(FDueBeforeValue, out Dummy);
                DueBefore = (DueBeforeIsValid) ? Dummy : 0;
            }
        }
        
        private string FPrintBeforeValue = "";
        public bool PrintBeforeIsValid = false;
        public int PrintBefore { get; set; }
        public string PrintBeforeValue
        {
            get { return FPrintBeforeValue; }
            set
            {
                FPrintBeforeValue = value;
                int Dummy = 0;
                PrintBeforeIsValid = int.TryParse(FPrintBeforeValue, out Dummy);
                PrintBefore = (PrintBeforeIsValid) ? Dummy : 0;
            }
        }
        
        private string FCreateBeforeValue = "";
        public bool CreateBeforeIsValid = false;
        public int CreateBefore { get; set; }
        public string CreateBeforeValue
        {
            get { return FCreateBeforeValue; }
            set
            {
                FCreateBeforeValue = value;
                int Dummy = 0;
                CreateBeforeIsValid = int.TryParse(FCreateBeforeValue, out Dummy);
                CreateBefore = (CreateBeforeIsValid) ? Dummy : 0;
            }
        }
        
        public string CreateStarts { get; set; }
        
        public string CreateStops { get; set; }
        
        private string FStartDayValue = "";
        public bool StartDayIsValid = false;
        public int StartDay { get; set; }
        public string StartDayValue
        {
            get { return FStartDayValue; }
            set
            {
                FStartDayValue = value;
                int Dummy = 0;
                StartDayIsValid = int.TryParse(FStartDayValue, out Dummy);
                StartDay = (StartDayIsValid) ? Dummy : 0;
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
            RevenueIdValue = "";
            SubLedgerIdValue = "";
            FrequencyValue = "";
            FromDate = "";
            Inclusive = "";
            ChargeAmountValue = "";
            GSTAmountValue = "";
            ToDate = "";
            Detail = "";
            DueBeforeValue = "";
            PrintBeforeValue = "";
            CreateBeforeValue = "";
            CreateStarts = "";
            CreateStops = "";
            StartDayValue = "";
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
                    Key = "RevenueId";
                    if (!RevenueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SubLedgerId";
                    if (!SubLedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Frequency";
                    if (!FrequencyIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FromDate";
                    if (string.IsNullOrEmpty(FromDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Inclusive";
                    if (string.IsNullOrEmpty(Inclusive)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChargeAmount";
                    if (!ChargeAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GSTAmount";
                    if (!GSTAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ToDate";
                    if (string.IsNullOrEmpty(ToDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Detail";
                    if (string.IsNullOrEmpty(Detail)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DueBefore";
                    if (!DueBeforeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PrintBefore";
                    if (!PrintBeforeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CreateBefore";
                    if (!CreateBeforeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CreateStarts";
                    if (string.IsNullOrEmpty(CreateStarts)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CreateStops";
                    if (string.IsNullOrEmpty(CreateStops)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StartDay";
                    if (!StartDayIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.FindIdValue = Source.FindIdValue;
                this.RevenueIdValue = Source.RevenueIdValue;
                this.SubLedgerIdValue = Source.SubLedgerIdValue;
                this.FrequencyValue = Source.FrequencyValue;
                this.FromDate = Source.FromDate;
                this.Inclusive = Source.Inclusive;
                this.ChargeAmountValue = Source.ChargeAmountValue;
                this.GSTAmountValue = Source.GSTAmountValue;
                this.ToDate = Source.ToDate;
                this.Detail = Source.Detail;
                this.DueBeforeValue = Source.DueBeforeValue;
                this.PrintBeforeValue = Source.PrintBeforeValue;
                this.CreateBeforeValue = Source.CreateBeforeValue;
                this.CreateStarts = Source.CreateStarts;
                this.CreateStops = Source.CreateStops;
                this.StartDayValue = Source.StartDayValue;
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
