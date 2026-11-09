/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 06-05-2020
 * Time: 6:40 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Receipts\Actions\Templates\AddDirectReceipt\ChangeIncludeStatusRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Receipts.AddDirectReceipt
{
    public class ChangeIncludeStatusRequestItem
    {
        #region Properties - Getters and Setters
        private string FMasterEventIdValue = "";
        public bool MasterEventIdIsValid = false;
        public int MasterEventId { get; set; }
        public string MasterEventIdValue
        {
            get { return FMasterEventIdValue; }
            set
            {
                FMasterEventIdValue = value;
                int Dummy = 0;
                MasterEventIdIsValid = int.TryParse(FMasterEventIdValue, out Dummy);
                MasterEventId = (MasterEventIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FReceiptTypeValue = "";
        public bool ReceiptTypeIsValid = false;
        public int ReceiptType { get; set; }
        public string ReceiptTypeValue
        {
            get { return FReceiptTypeValue; }
            set
            {
                FReceiptTypeValue = value;
                int Dummy = 0;
                ReceiptTypeIsValid = int.TryParse(FReceiptTypeValue, out Dummy);
                ReceiptType = (ReceiptTypeIsValid) ? Dummy : 0;
            }
        }
        
        public string FindText { get; set; }
        
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
        
        private string FBankIdValue = "";
        public bool BankIdIsValid = false;
        public int BankId { get; set; }
        public string BankIdValue
        {
            get { return FBankIdValue; }
            set
            {
                FBankIdValue = value;
                int Dummy = 0;
                BankIdIsValid = int.TryParse(FBankIdValue, out Dummy);
                BankId = (BankIdIsValid) ? Dummy : 0;
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
        
        public string GST { get; set; }
        
        public string ReceiptAmount { get; set; }
        
        public string ReceiptGSTAmount { get; set; }
        
        private string FReceiptNoValue = "";
        public bool ReceiptNoIsValid = false;
        public int ReceiptNo { get; set; }
        public string ReceiptNoValue
        {
            get { return FReceiptNoValue; }
            set
            {
                FReceiptNoValue = value;
                int Dummy = 0;
                ReceiptNoIsValid = int.TryParse(FReceiptNoValue, out Dummy);
                ReceiptNo = (ReceiptNoIsValid) ? Dummy : 0;
            }
        }
        
        public string ReceiptDate { get; set; }
        
        public string ReceiptClearance { get; set; }
        
        public string Detail { get; set; }
        
        public string Payee { get; set; }
        
        public string TakenBy { get; set; }
        
        public string ClientName { get; set; }
        
        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        
        public string Address3 { get; set; }
        
        public string Address4 { get; set; }
        
        public string PaidOwnerDirect { get; set; }
        
        private string FPageNoValue = "";
        public bool PageNoIsValid = false;
        public int PageNo { get; set; }
        public string PageNoValue
        {
            get { return FPageNoValue; }
            set
            {
                FPageNoValue = value;
                int Dummy = 0;
                PageNoIsValid = int.TryParse(FPageNoValue, out Dummy);
                PageNo = (PageNoIsValid) ? Dummy : 0;
            }
        }
        
        private string FXImportIdValue = "";
        public bool XImportIdIsValid = false;
        public int XImportId { get; set; }
        public string XImportIdValue
        {
            get { return FXImportIdValue; }
            set
            {
                FXImportIdValue = value;
                int Dummy = 0;
                XImportIdIsValid = int.TryParse(FXImportIdValue, out Dummy);
                XImportId = (XImportIdIsValid) ? Dummy : 0;
            }
        }
        
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
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            MasterEventIdValue = "";
            ReceiptTypeValue = "";
            FindIdValue = "";
            SubLedgerIdValue = "";
            BankIdValue = "";
            RevenueIdValue = "";
            GST = "";
            ReceiptAmount = "";
            ReceiptGSTAmount = "";
            ReceiptNoValue = "";
            ReceiptDate = "";
            ReceiptClearance = "";
            PageNoValue = "";
            XImportIdValue = "";
            UniqueIdValue = "";
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
                    Key = "MasterEventId";
                    if (!MasterEventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptType";
                    if (!ReceiptTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FindId";
                    if (!FindIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SubLedgerId";
                    if (!SubLedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankId";
                    if (!BankIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RevenueId";
                    if (!RevenueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GST";
                    if (string.IsNullOrEmpty(GST)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptAmount";
                    if (string.IsNullOrEmpty(ReceiptAmount)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptGSTAmount";
                    if (string.IsNullOrEmpty(ReceiptGSTAmount)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptNo";
                    if (!ReceiptNoIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptDate";
                    if (string.IsNullOrEmpty(ReceiptDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptClearance";
                    if (string.IsNullOrEmpty(ReceiptClearance)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PageNo";
                    if (!PageNoIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "XImportId";
                    if (!XImportIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "UniqueId";
                    if (!UniqueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(ChangeIncludeStatusRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.MasterEventIdValue = Source.MasterEventIdValue;
                this.ReceiptTypeValue = Source.ReceiptTypeValue;
                this.FindText = Source.FindText;
                this.FindIdValue = Source.FindIdValue;
                this.SubLedgerIdValue = Source.SubLedgerIdValue;
                this.BankIdValue = Source.BankIdValue;
                this.RevenueIdValue = Source.RevenueIdValue;
                this.GST = Source.GST;
                this.ReceiptAmount = Source.ReceiptAmount;
                this.ReceiptGSTAmount = Source.ReceiptGSTAmount;
                this.ReceiptNoValue = Source.ReceiptNoValue;
                this.ReceiptDate = Source.ReceiptDate;
                this.ReceiptClearance = Source.ReceiptClearance;
                this.Detail = Source.Detail;
                this.Payee = Source.Payee;
                this.TakenBy = Source.TakenBy;
                this.ClientName = Source.ClientName;
                this.Address1 = Source.Address1;
                this.Address2 = Source.Address2;
                this.Address3 = Source.Address3;
                this.Address4 = Source.Address4;
                this.PaidOwnerDirect = Source.PaidOwnerDirect;
                this.PageNoValue = Source.PageNoValue;
                this.XImportIdValue = Source.XImportIdValue;
                this.UniqueIdValue = Source.UniqueIdValue;
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
            
                    ChangeIncludeStatusRequestItem TempItem = new ChangeIncludeStatusRequestItem();
                    //Result = CommonMethods.LoadChangeIncludeStatusRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadChangeIncludeStatusRequestItem(ActionBase, ref TempItem);
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
