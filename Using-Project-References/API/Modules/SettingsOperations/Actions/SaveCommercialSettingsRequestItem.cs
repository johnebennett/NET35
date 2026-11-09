/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 04-01-2021
 * Time: 5:15 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/Actions/Templates/SaveCommercialSettingsRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations
{
    public class SaveCommercialSettingsRequestItem
    {
        #region Properties - Getters and Setters
        private string FMinTaxInvoiceAmountValue = "";
        public bool MinTaxInvoiceAmountIsValid = false;
        public int MinTaxInvoiceAmount { get; set; }
        public string MinTaxInvoiceAmountValue
        {
            get { return FMinTaxInvoiceAmountValue; }
            set
            {
                FMinTaxInvoiceAmountValue = value;
                int Dummy = 0;
                MinTaxInvoiceAmountIsValid = int.TryParse(FMinTaxInvoiceAmountValue, out Dummy);
                MinTaxInvoiceAmount = (MinTaxInvoiceAmountIsValid) ? Dummy : 0;
            }
        }
        
        private string FComInspectionAdvanceValue = "";
        public bool ComInspectionAdvanceIsValid = false;
        public int ComInspectionAdvance { get; set; }
        public string ComInspectionAdvanceValue
        {
            get { return FComInspectionAdvanceValue; }
            set
            {
                FComInspectionAdvanceValue = value;
                int Dummy = 0;
                ComInspectionAdvanceIsValid = int.TryParse(FComInspectionAdvanceValue, out Dummy);
                ComInspectionAdvance = (ComInspectionAdvanceIsValid) ? Dummy : 0;
            }
        }
        
        public string ComLeaseExpiry { get; set; }
        
        public string ComVacateDate { get; set; }
        
        public string ComRentalReview { get; set; }
        
        public string ComOptionBegin { get; set; }
        
        public string ComOptionEnd { get; set; }
        
        private string FLettingFeeAccountIdValue = "";
        public bool LettingFeeAccountIdIsValid = false;
        public int LettingFeeAccountId { get; set; }
        public string LettingFeeAccountIdValue
        {
            get { return FLettingFeeAccountIdValue; }
            set
            {
                FLettingFeeAccountIdValue = value;
                int Dummy = 0;
                LettingFeeAccountIdIsValid = int.TryParse(FLettingFeeAccountIdValue, out Dummy);
                LettingFeeAccountId = (LettingFeeAccountIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FAdvertisingAccountIdValue = "";
        public bool AdvertisingAccountIdIsValid = false;
        public int AdvertisingAccountId { get; set; }
        public string AdvertisingAccountIdValue
        {
            get { return FAdvertisingAccountIdValue; }
            set
            {
                FAdvertisingAccountIdValue = value;
                int Dummy = 0;
                AdvertisingAccountIdIsValid = int.TryParse(FAdvertisingAccountIdValue, out Dummy);
                AdvertisingAccountId = (AdvertisingAccountIdIsValid) ? Dummy : 0;
            }
        }
        
        public string EndOfYearStatement { get; set; }
        
        private string FSendEndOfYearStmtToValue = "";
        public bool SendEndOfYearStmtToIsValid = false;
        public int SendEndOfYearStmtTo { get; set; }
        public string SendEndOfYearStmtToValue
        {
            get { return FSendEndOfYearStmtToValue; }
            set
            {
                FSendEndOfYearStmtToValue = value;
                int Dummy = 0;
                SendEndOfYearStmtToIsValid = int.TryParse(FSendEndOfYearStmtToValue, out Dummy);
                SendEndOfYearStmtTo = (SendEndOfYearStmtToIsValid) ? Dummy : 0;
            }
        }
        
        public string EndOfYearStmtFee { get; set; }
        
        public string EstOutgoingsAddPercent { get; set; }
        
        private string FEstOutgoingsSourceValue = "";
        public bool EstOutgoingsSourceIsValid = false;
        public int EstOutgoingsSource { get; set; }
        public string EstOutgoingsSourceValue
        {
            get { return FEstOutgoingsSourceValue; }
            set
            {
                FEstOutgoingsSourceValue = value;
                int Dummy = 0;
                EstOutgoingsSourceIsValid = int.TryParse(FEstOutgoingsSourceValue, out Dummy);
                EstOutgoingsSource = (EstOutgoingsSourceIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            MinTaxInvoiceAmountValue = "";
            ComInspectionAdvanceValue = "";
            ComLeaseExpiry = "";
            ComVacateDate = "";
            ComRentalReview = "";
            ComOptionBegin = "";
            ComOptionEnd = "";
            LettingFeeAccountIdValue = "";
            AdvertisingAccountIdValue = "";
            EndOfYearStatement = "";
            SendEndOfYearStmtToValue = "";
            EndOfYearStmtFee = "";
            EstOutgoingsAddPercent = "";
            EstOutgoingsSourceValue = "";
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
                    Key = "MinTaxInvoiceAmount";
                    if (!MinTaxInvoiceAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ComInspectionAdvance";
                    if (!ComInspectionAdvanceIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ComLeaseExpiry";
                    if (string.IsNullOrEmpty(ComLeaseExpiry)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ComVacateDate";
                    if (string.IsNullOrEmpty(ComVacateDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ComRentalReview";
                    if (string.IsNullOrEmpty(ComRentalReview)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ComOptionBegin";
                    if (string.IsNullOrEmpty(ComOptionBegin)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ComOptionEnd";
                    if (string.IsNullOrEmpty(ComOptionEnd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LettingFeeAccountId";
                    if (!LettingFeeAccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AdvertisingAccountId";
                    if (!AdvertisingAccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EndOfYearStatement";
                    if (string.IsNullOrEmpty(EndOfYearStatement)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SendEndOfYearStmtTo";
                    if (!SendEndOfYearStmtToIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EndOfYearStmtFee";
                    if (string.IsNullOrEmpty(EndOfYearStmtFee)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EstOutgoingsAddPercent";
                    if (string.IsNullOrEmpty(EstOutgoingsAddPercent)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EstOutgoingsSource";
                    if (!EstOutgoingsSourceIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SaveCommercialSettingsRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.MinTaxInvoiceAmountValue = Source.MinTaxInvoiceAmountValue;
                this.ComInspectionAdvanceValue = Source.ComInspectionAdvanceValue;
                this.ComLeaseExpiry = Source.ComLeaseExpiry;
                this.ComVacateDate = Source.ComVacateDate;
                this.ComRentalReview = Source.ComRentalReview;
                this.ComOptionBegin = Source.ComOptionBegin;
                this.ComOptionEnd = Source.ComOptionEnd;
                this.LettingFeeAccountIdValue = Source.LettingFeeAccountIdValue;
                this.AdvertisingAccountIdValue = Source.AdvertisingAccountIdValue;
                this.EndOfYearStatement = Source.EndOfYearStatement;
                this.SendEndOfYearStmtToValue = Source.SendEndOfYearStmtToValue;
                this.EndOfYearStmtFee = Source.EndOfYearStmtFee;
                this.EstOutgoingsAddPercent = Source.EstOutgoingsAddPercent;
                this.EstOutgoingsSourceValue = Source.EstOutgoingsSourceValue;
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
            
                    SaveCommercialSettingsRequestItem TempItem = new SaveCommercialSettingsRequestItem();
                    //Result = CommonMethods.LoadSaveCommercialSettingsRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSaveCommercialSettingsRequestItem(ActionBase, ref TempItem);
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
