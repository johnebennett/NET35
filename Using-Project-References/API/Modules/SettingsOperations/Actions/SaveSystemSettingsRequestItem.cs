/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 01-03-2021
 * Time: 1:40 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/Actions/Templates/SaveSystemSettingsRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations
{
    public class SaveSystemSettingsRequestItem
    {
        #region Properties - Getters and Setters
        private string FSaleCommToLedgerValue = "";
        public bool SaleCommToLedgerIsValid = false;
        public int SaleCommToLedger { get; set; }
        public string SaleCommToLedgerValue
        {
            get { return FSaleCommToLedgerValue; }
            set
            {
                FSaleCommToLedgerValue = value;
                int Dummy = 0;
                SaleCommToLedgerIsValid = int.TryParse(FSaleCommToLedgerValue, out Dummy);
                SaleCommToLedger = (SaleCommToLedgerIsValid) ? Dummy : 0;
            }
        }
        
        private string FResLettingFeeExpenseValue = "";
        public bool ResLettingFeeExpenseIsValid = false;
        public int ResLettingFeeExpense { get; set; }
        public string ResLettingFeeExpenseValue
        {
            get { return FResLettingFeeExpenseValue; }
            set
            {
                FResLettingFeeExpenseValue = value;
                int Dummy = 0;
                ResLettingFeeExpenseIsValid = int.TryParse(FResLettingFeeExpenseValue, out Dummy);
                ResLettingFeeExpense = (ResLettingFeeExpenseIsValid) ? Dummy : 0;
            }
        }
        
        private string FResLettingFeeRevenueValue = "";
        public bool ResLettingFeeRevenueIsValid = false;
        public int ResLettingFeeRevenue { get; set; }
        public string ResLettingFeeRevenueValue
        {
            get { return FResLettingFeeRevenueValue; }
            set
            {
                FResLettingFeeRevenueValue = value;
                int Dummy = 0;
                ResLettingFeeRevenueIsValid = int.TryParse(FResLettingFeeRevenueValue, out Dummy);
                ResLettingFeeRevenue = (ResLettingFeeRevenueIsValid) ? Dummy : 0;
            }
        }
        
        private string FAdvertisingExpenseValue = "";
        public bool AdvertisingExpenseIsValid = false;
        public int AdvertisingExpense { get; set; }
        public string AdvertisingExpenseValue
        {
            get { return FAdvertisingExpenseValue; }
            set
            {
                FAdvertisingExpenseValue = value;
                int Dummy = 0;
                AdvertisingExpenseIsValid = int.TryParse(FAdvertisingExpenseValue, out Dummy);
                AdvertisingExpense = (AdvertisingExpenseIsValid) ? Dummy : 0;
            }
        }
        
        private string FAdvertisingRevenueValue = "";
        public bool AdvertisingRevenueIsValid = false;
        public int AdvertisingRevenue { get; set; }
        public string AdvertisingRevenueValue
        {
            get { return FAdvertisingRevenueValue; }
            set
            {
                FAdvertisingRevenueValue = value;
                int Dummy = 0;
                AdvertisingRevenueIsValid = int.TryParse(FAdvertisingRevenueValue, out Dummy);
                AdvertisingRevenue = (AdvertisingRevenueIsValid) ? Dummy : 0;
            }
        }
        
        private string FCommercialLettingFeeExpenseValue = "";
        public bool CommercialLettingFeeExpenseIsValid = false;
        public int CommercialLettingFeeExpense { get; set; }
        public string CommercialLettingFeeExpenseValue
        {
            get { return FCommercialLettingFeeExpenseValue; }
            set
            {
                FCommercialLettingFeeExpenseValue = value;
                int Dummy = 0;
                CommercialLettingFeeExpenseIsValid = int.TryParse(FCommercialLettingFeeExpenseValue, out Dummy);
                CommercialLettingFeeExpense = (CommercialLettingFeeExpenseIsValid) ? Dummy : 0;
            }
        }
        
        private string FCommercialLettingFeeRevenueValue = "";
        public bool CommercialLettingFeeRevenueIsValid = false;
        public int CommercialLettingFeeRevenue { get; set; }
        public string CommercialLettingFeeRevenueValue
        {
            get { return FCommercialLettingFeeRevenueValue; }
            set
            {
                FCommercialLettingFeeRevenueValue = value;
                int Dummy = 0;
                CommercialLettingFeeRevenueIsValid = int.TryParse(FCommercialLettingFeeRevenueValue, out Dummy);
                CommercialLettingFeeRevenue = (CommercialLettingFeeRevenueIsValid) ? Dummy : 0;
            }
        }
        
        public string AutoPostApproved { get; set; }
        
        public string Use2FA { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            SaleCommToLedgerValue = "";
            ResLettingFeeExpenseValue = "";
            ResLettingFeeRevenueValue = "";
            AdvertisingExpenseValue = "";
            AdvertisingRevenueValue = "";
            CommercialLettingFeeExpenseValue = "";
            CommercialLettingFeeRevenueValue = "";
            AutoPostApproved = "";
            Use2FA = "";
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
                    Key = "SaleCommToLedger";
                    if (!SaleCommToLedgerIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ResLettingFeeExpense";
                    if (!ResLettingFeeExpenseIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ResLettingFeeRevenue";
                    if (!ResLettingFeeRevenueIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AdvertisingExpense";
                    if (!AdvertisingExpenseIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AdvertisingRevenue";
                    if (!AdvertisingRevenueIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CommercialLettingFeeExpense";
                    if (!CommercialLettingFeeExpenseIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CommercialLettingFeeRevenue";
                    if (!CommercialLettingFeeRevenueIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AutoPostApproved";
                    if (string.IsNullOrEmpty(AutoPostApproved)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Use2FA";
                    if (string.IsNullOrEmpty(Use2FA)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SaveSystemSettingsRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.SaleCommToLedgerValue = Source.SaleCommToLedgerValue;
                this.ResLettingFeeExpenseValue = Source.ResLettingFeeExpenseValue;
                this.ResLettingFeeRevenueValue = Source.ResLettingFeeRevenueValue;
                this.AdvertisingExpenseValue = Source.AdvertisingExpenseValue;
                this.AdvertisingRevenueValue = Source.AdvertisingRevenueValue;
                this.CommercialLettingFeeExpenseValue = Source.CommercialLettingFeeExpenseValue;
                this.CommercialLettingFeeRevenueValue = Source.CommercialLettingFeeRevenueValue;
                this.AutoPostApproved = Source.AutoPostApproved;
                this.Use2FA = Source.Use2FA;
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
            
                    SaveSystemSettingsRequestItem TempItem = new SaveSystemSettingsRequestItem();
                    //Result = CommonMethods.LoadSaveSystemSettingsRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSaveSystemSettingsRequestItem(ActionBase, ref TempItem);
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
