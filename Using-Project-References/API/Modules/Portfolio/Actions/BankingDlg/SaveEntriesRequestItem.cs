/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 14-01-2022
 * Time: 7:45 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/Actions/Templates/BankingDlg/SaveEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.BankingDlg
{
    public class SaveEntriesRequestItem
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
        
        public string TenantABN { get; set; }
        
        public string BSB1 { get; set; }
        
        public string BSB2 { get; set; }
        
        public string BSB3 { get; set; }
        
        public string BSB4 { get; set; }
        
        public string Number1 { get; set; }
        
        public string Number2 { get; set; }
        
        public string Number3 { get; set; }
        
        public string Number4 { get; set; }
        
        private string FPercentage1Value = "";
        public bool Percentage1IsValid = false;
        public double Percentage1 { get; set; }
        public string Percentage1Value
        {
            get { return FPercentage1Value; }
            set
            {
                FPercentage1Value = value;
                double Dummy = 0.0;
                Percentage1IsValid = Double.TryParse(FPercentage1Value, out Dummy);
                Percentage1 = (Percentage1IsValid) ? Dummy : 0;
            }
        }
        
        private string FPercentage2Value = "";
        public bool Percentage2IsValid = false;
        public double Percentage2 { get; set; }
        public string Percentage2Value
        {
            get { return FPercentage2Value; }
            set
            {
                FPercentage2Value = value;
                double Dummy = 0.0;
                Percentage2IsValid = Double.TryParse(FPercentage2Value, out Dummy);
                Percentage2 = (Percentage2IsValid) ? Dummy : 0;
            }
        }
        
        private string FPercentage3Value = "";
        public bool Percentage3IsValid = false;
        public double Percentage3 { get; set; }
        public string Percentage3Value
        {
            get { return FPercentage3Value; }
            set
            {
                FPercentage3Value = value;
                double Dummy = 0.0;
                Percentage3IsValid = Double.TryParse(FPercentage3Value, out Dummy);
                Percentage3 = (Percentage3IsValid) ? Dummy : 0;
            }
        }
        
        private string FPercentage4Value = "";
        public bool Percentage4IsValid = false;
        public double Percentage4 { get; set; }
        public string Percentage4Value
        {
            get { return FPercentage4Value; }
            set
            {
                FPercentage4Value = value;
                double Dummy = 0.0;
                Percentage4IsValid = Double.TryParse(FPercentage4Value, out Dummy);
                Percentage4 = (Percentage4IsValid) ? Dummy : 0;
            }
        }
        
        public string Details1 { get; set; }
        
        public string Details2 { get; set; }
        
        public string Details3 { get; set; }
        
        public string Details4 { get; set; }
        
        public string RunNumber { get; set; }
        
        public string PrintRunNumber { get; set; }
        
        private string FTenancyOrderValue = "";
        public bool TenancyOrderIsValid = false;
        public double TenancyOrder { get; set; }
        public string TenancyOrderValue
        {
            get { return FTenancyOrderValue; }
            set
            {
                FTenancyOrderValue = value;
                double Dummy = 0.0;
                TenancyOrderIsValid = Double.TryParse(FTenancyOrderValue, out Dummy);
                TenancyOrder = (TenancyOrderIsValid) ? Dummy : 0;
            }
        }
        
        private string FDefaultDebitValue = "";
        public bool DefaultDebitIsValid = false;
        public double DefaultDebit { get; set; }
        public string DefaultDebitValue
        {
            get { return FDefaultDebitValue; }
            set
            {
                FDefaultDebitValue = value;
                double Dummy = 0.0;
                DefaultDebitIsValid = Double.TryParse(FDefaultDebitValue, out Dummy);
                DefaultDebit = (DefaultDebitIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            Percentage1Value = "";
            Percentage2Value = "";
            Percentage3Value = "";
            Percentage4Value = "";
            TenancyOrderValue = "";
            DefaultDebitValue = "";
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
                    Key = "TenancyOrder";
                    if (!TenancyOrderIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DefaultDebit";
                    if (!DefaultDebitIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SaveEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.TenantABN = Source.TenantABN;
                this.BSB1 = Source.BSB1;
                this.BSB2 = Source.BSB2;
                this.BSB3 = Source.BSB3;
                this.BSB4 = Source.BSB4;
                this.Number1 = Source.Number1;
                this.Number2 = Source.Number2;
                this.Number3 = Source.Number3;
                this.Number4 = Source.Number4;
                this.Percentage1Value = Source.Percentage1Value;
                this.Percentage2Value = Source.Percentage2Value;
                this.Percentage3Value = Source.Percentage3Value;
                this.Percentage4Value = Source.Percentage4Value;
                this.Details1 = Source.Details1;
                this.Details2 = Source.Details2;
                this.Details3 = Source.Details3;
                this.Details4 = Source.Details4;
                this.RunNumber = Source.RunNumber;
                this.PrintRunNumber = Source.PrintRunNumber;
                this.TenancyOrderValue = Source.TenancyOrderValue;
                this.DefaultDebitValue = Source.DefaultDebitValue;
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
            
                    SaveEntriesRequestItem TempItem = new SaveEntriesRequestItem();
                    //Result = CommonMethods.LoadSaveEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSaveEntriesRequestItem(ActionBase, ref TempItem);
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
