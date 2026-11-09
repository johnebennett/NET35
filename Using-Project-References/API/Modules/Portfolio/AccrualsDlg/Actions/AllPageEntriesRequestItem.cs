/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 07-08-2020
 * Time: 9:33 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\AccrualsDlg\Actions\Templates\AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.AccrualsDlg
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
        
        private string FSubAccountIdValue = "";
        public bool SubAccountIdIsValid = false;
        public int SubAccountId { get; set; }
        public string SubAccountIdValue
        {
            get { return FSubAccountIdValue; }
            set
            {
                FSubAccountIdValue = value;
                int Dummy = 0;
                SubAccountIdIsValid = int.TryParse(FSubAccountIdValue, out Dummy);
                SubAccountId = (SubAccountIdIsValid) ? Dummy : 0;
            }
        }
        
        public string DateFrom { get; set; }
        
        public string DateTo { get; set; }
        
        private string FChartIdValue = "";
        public bool ChartIdIsValid = false;
        public int ChartId { get; set; }
        public string ChartIdValue
        {
            get { return FChartIdValue; }
            set
            {
                FChartIdValue = value;
                int Dummy = 0;
                ChartIdIsValid = int.TryParse(FChartIdValue, out Dummy);
                ChartId = (ChartIdIsValid) ? Dummy : 0;
            }
        }
        
        public string AddJnl { get; set; }
        
        private string FChartFromValue = "";
        public bool ChartFromIsValid = false;
        public int ChartFrom { get; set; }
        public string ChartFromValue
        {
            get { return FChartFromValue; }
            set
            {
                FChartFromValue = value;
                int Dummy = 0;
                ChartFromIsValid = int.TryParse(FChartFromValue, out Dummy);
                ChartFrom = (ChartFromIsValid) ? Dummy : 0;
            }
        }
        
        private string FChartToValue = "";
        public bool ChartToIsValid = false;
        public int ChartTo { get; set; }
        public string ChartToValue
        {
            get { return FChartToValue; }
            set
            {
                FChartToValue = value;
                int Dummy = 0;
                ChartToIsValid = int.TryParse(FChartToValue, out Dummy);
                ChartTo = (ChartToIsValid) ? Dummy : 0;
            }
        }
        
        public string JournalDesc { get; set; }
        
        private string FJournalAmountValue = "";
        public bool JournalAmountIsValid = false;
        public double JournalAmount { get; set; }
        public string JournalAmountValue
        {
            get { return FJournalAmountValue; }
            set
            {
                FJournalAmountValue = value;
                double Dummy = 0.0;
                JournalAmountIsValid = Double.TryParse(FJournalAmountValue, out Dummy);
                JournalAmount = (JournalAmountIsValid) ? Dummy : 0;
            }
        }
        
        public string JournalDate { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            SubAccountIdValue = "";
            DateFrom = "";
            DateTo = "";
            ChartIdValue = "";
            AddJnl = "";
            ChartFromValue = "";
            ChartToValue = "";
            JournalDesc = "";
            JournalAmountValue = "";
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
                    Key = "SubAccountId";
                    if (!SubAccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DateFrom";
                    if (string.IsNullOrEmpty(DateFrom)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DateTo";
                    if (string.IsNullOrEmpty(DateTo)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChartId";
                    if (!ChartIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AddJnl";
                    if (string.IsNullOrEmpty(AddJnl)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChartFrom";
                    if (!ChartFromIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ChartTo";
                    if (!ChartToIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "JournalDesc";
                    if (string.IsNullOrEmpty(JournalDesc)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "JournalAmount";
                    if (!JournalAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.SubAccountIdValue = Source.SubAccountIdValue;
                this.DateFrom = Source.DateFrom;
                this.DateTo = Source.DateTo;
                this.ChartIdValue = Source.ChartIdValue;
                this.AddJnl = Source.AddJnl;
                this.ChartFromValue = Source.ChartFromValue;
                this.ChartToValue = Source.ChartToValue;
                this.JournalDesc = Source.JournalDesc;
                this.JournalAmountValue = Source.JournalAmountValue;
                this.JournalDate = Source.JournalDate;
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
