/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 17-02-2021
 * Time: 10:14 AM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/Actions/Templates/SaveChartOfAccountEditRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations
{
    public class SaveChartOfAccountEditRequestItem
    {
        #region Properties - Getters and Setters
        private string FRecordIdValue = "";
        public bool RecordIdIsValid = false;
        public int RecordId { get; set; }
        public string RecordIdValue
        {
            get { return FRecordIdValue; }
            set
            {
                FRecordIdValue = value;
                int Dummy = 0;
                RecordIdIsValid = int.TryParse(FRecordIdValue, out Dummy);
                RecordId = (RecordIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FDlgModeValue = "";
        public bool DlgModeIsValid = false;
        public int DlgMode { get; set; }
        public string DlgModeValue
        {
            get { return FDlgModeValue; }
            set
            {
                FDlgModeValue = value;
                int Dummy = 0;
                DlgModeIsValid = int.TryParse(FDlgModeValue, out Dummy);
                DlgMode = (DlgModeIsValid) ? Dummy : 0;
            }
        }
        
        public string Description { get; set; }
        
        public string Account { get; set; }
        
        private string FNumberOrderValue = "";
        public bool NumberOrderIsValid = false;
        public int NumberOrder { get; set; }
        public string NumberOrderValue
        {
            get { return FNumberOrderValue; }
            set
            {
                FNumberOrderValue = value;
                int Dummy = 0;
                NumberOrderIsValid = int.TryParse(FNumberOrderValue, out Dummy);
                NumberOrder = (NumberOrderIsValid) ? Dummy : 0;
            }
        }
        
        private string FOrder2Value = "";
        public bool Order2IsValid = false;
        public int Order2 { get; set; }
        public string Order2Value
        {
            get { return FOrder2Value; }
            set
            {
                FOrder2Value = value;
                int Dummy = 0;
                Order2IsValid = int.TryParse(FOrder2Value, out Dummy);
                Order2 = (Order2IsValid) ? Dummy : 0;
            }
        }
        
        private string FOrder3Value = "";
        public bool Order3IsValid = false;
        public int Order3 { get; set; }
        public string Order3Value
        {
            get { return FOrder3Value; }
            set
            {
                FOrder3Value = value;
                int Dummy = 0;
                Order3IsValid = int.TryParse(FOrder3Value, out Dummy);
                Order3 = (Order3IsValid) ? Dummy : 0;
            }
        }
        
        private string FOrder2IdValue = "";
        public bool Order2IdIsValid = false;
        public int Order2Id { get; set; }
        public string Order2IdValue
        {
            get { return FOrder2IdValue; }
            set
            {
                FOrder2IdValue = value;
                int Dummy = 0;
                Order2IdIsValid = int.TryParse(FOrder2IdValue, out Dummy);
                Order2Id = (Order2IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOrder3IdValue = "";
        public bool Order3IdIsValid = false;
        public int Order3Id { get; set; }
        public string Order3IdValue
        {
            get { return FOrder3IdValue; }
            set
            {
                FOrder3IdValue = value;
                int Dummy = 0;
                Order3IdIsValid = int.TryParse(FOrder3IdValue, out Dummy);
                Order3Id = (Order3IdIsValid) ? Dummy : 0;
            }
        }
        
        public string Hide { get; set; }
        
        public string Advertising { get; set; }
        
        public string CommissionOnRental { get; set; }
        
        public string TenancySchedule { get; set; }
        
        public string Outgoings { get; set; }
        
        public string NotOnBAS { get; set; }
        
        public string SecurityDeposit { get; set; }
        
        public string CommissionOnSale { get; set; }
        
        public string Insurance { get; set; }
        
        public string PenaltyInterest { get; set; }
        
        public string DoNotChargeInterest { get; set; }
        
        public string GroupOnStatement { get; set; }
        
        public string InvoiceHandling { get; set; }
        
        public string Maintenance { get; set; }
        
        public string Compliance { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            RecordIdValue = "";
            DlgModeValue = "";
            Description = "";
            Account = "";
            NumberOrderValue = "";
            Order2Value = "";
            Order3Value = "";
            Order2IdValue = "";
            Order3IdValue = "";
            InvoiceHandling = "";
            Maintenance = "";
            Compliance = "";
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
                    Key = "RecordId";
                    if (!RecordIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DlgMode";
                    if (!DlgModeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Description";
                    if (string.IsNullOrEmpty(Description)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Account";
                    if (string.IsNullOrEmpty(Account)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "NumberOrder";
                    if (!NumberOrderIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Order2";
                    if (!Order2IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Order3";
                    if (!Order3IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Order2Id";
                    if (!Order2IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Order3Id";
                    if (!Order3IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InvoiceHandling";
                    if (string.IsNullOrEmpty(InvoiceHandling)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Maintenance";
                    if (string.IsNullOrEmpty(Maintenance)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Compliance";
                    if (string.IsNullOrEmpty(Compliance)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SaveChartOfAccountEditRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.RecordIdValue = Source.RecordIdValue;
                this.DlgModeValue = Source.DlgModeValue;
                this.Description = Source.Description;
                this.Account = Source.Account;
                this.NumberOrderValue = Source.NumberOrderValue;
                this.Order2Value = Source.Order2Value;
                this.Order3Value = Source.Order3Value;
                this.Order2IdValue = Source.Order2IdValue;
                this.Order3IdValue = Source.Order3IdValue;
                this.Hide = Source.Hide;
                this.Advertising = Source.Advertising;
                this.CommissionOnRental = Source.CommissionOnRental;
                this.TenancySchedule = Source.TenancySchedule;
                this.Outgoings = Source.Outgoings;
                this.NotOnBAS = Source.NotOnBAS;
                this.SecurityDeposit = Source.SecurityDeposit;
                this.CommissionOnSale = Source.CommissionOnSale;
                this.Insurance = Source.Insurance;
                this.PenaltyInterest = Source.PenaltyInterest;
                this.DoNotChargeInterest = Source.DoNotChargeInterest;
                this.GroupOnStatement = Source.GroupOnStatement;
                this.InvoiceHandling = Source.InvoiceHandling;
                this.Maintenance = Source.Maintenance;
                this.Compliance = Source.Compliance;
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
            
                    SaveChartOfAccountEditRequestItem TempItem = new SaveChartOfAccountEditRequestItem();
                    //Result = CommonMethods.LoadSaveChartOfAccountEditRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSaveChartOfAccountEditRequestItem(ActionBase, ref TempItem);
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
