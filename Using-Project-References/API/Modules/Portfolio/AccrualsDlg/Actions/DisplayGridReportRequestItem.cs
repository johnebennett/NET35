/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 06-08-2020
 * Time: 8:17 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\AccrualsDlg\Actions\Templates\DisplayGridReportRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.AccrualsDlg
{
    public class DisplayGridReportRequestItem
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
        
        private string FUniqueIdValue = "";
        public bool UniqueIdIsValid = false;
        public int UniqueId { get; set; }
        public string UniqueIdValue
        {
            get { return FUniqueIdValue; }
            set
            {
                FUniqueIdValue = value;
                int Dummy = 0;
                UniqueIdIsValid = int.TryParse(FUniqueIdValue, out Dummy);
                UniqueId = (UniqueIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FButtonIdValue = "";
        public bool ButtonIdIsValid = false;
        public int ButtonId { get; set; }
        public string ButtonIdValue
        {
            get { return FButtonIdValue; }
            set
            {
                FButtonIdValue = value;
                int Dummy = 0;
                ButtonIdIsValid = int.TryParse(FButtonIdValue, out Dummy);
                ButtonId = (ButtonIdIsValid) ? Dummy : 0;
            }
        }
        
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
            UniqueIdValue = "";
            ButtonIdValue = "";
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
                    Key = "UniqueId";
                    if (!UniqueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ButtonId";
                    if (!ButtonIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(DisplayGridReportRequestItem Source)
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
                this.UniqueIdValue = Source.UniqueIdValue;
                this.ButtonIdValue = Source.ButtonIdValue;
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
            
                    DisplayGridReportRequestItem TempItem = new DisplayGridReportRequestItem();
                    //Result = CommonMethods.LoadDisplayGridReportRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadDisplayGridReportRequestItem(ActionBase, ref TempItem);
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
