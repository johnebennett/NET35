/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 24-04-2020
 * Time: 7:16 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Receipts\Actions\Templates\ImportBankingFileRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Receipts.CreateOptions
{
    public class ImportBankingFileRequestItem
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
        
        private string FBankImportTypeIdValue = "";
        public bool BankImportTypeIdIsValid = false;
        public int BankImportTypeId { get; set; }
        public string BankImportTypeIdValue
        {
            get { return FBankImportTypeIdValue; }
            set
            {
                FBankImportTypeIdValue = value;
                int Dummy = 0;
                BankImportTypeIdIsValid = int.TryParse(FBankImportTypeIdValue, out Dummy);
                BankImportTypeId = (BankImportTypeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FBankNameIdValue = "";
        public bool BankNameIdIsValid = false;
        public int BankNameId { get; set; }
        public string BankNameIdValue
        {
            get { return FBankNameIdValue; }
            set
            {
                FBankNameIdValue = value;
                int Dummy = 0;
                BankNameIdIsValid = int.TryParse(FBankNameIdValue, out Dummy);
                BankNameId = (BankNameIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FReceiptSetIdValue = "";
        public bool ReceiptSetIdIsValid = false;
        public int ReceiptSetId { get; set; }
        public string ReceiptSetIdValue
        {
            get { return FReceiptSetIdValue; }
            set
            {
                FReceiptSetIdValue = value;
                int Dummy = 0;
                ReceiptSetIdIsValid = int.TryParse(FReceiptSetIdValue, out Dummy);
                ReceiptSetId = (ReceiptSetIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FStartingReceiptNumberValue = "";
        public bool StartingReceiptNumberIsValid = false;
        public int StartingReceiptNumber { get; set; }
        public string StartingReceiptNumberValue
        {
            get { return FStartingReceiptNumberValue; }
            set
            {
                FStartingReceiptNumberValue = value;
                int Dummy = 0;
                StartingReceiptNumberIsValid = int.TryParse(FStartingReceiptNumberValue, out Dummy);
                StartingReceiptNumber = (StartingReceiptNumberIsValid) ? Dummy : 0;
            }
        }
        
        private string FPageNumberValue = "";
        public bool PageNumberIsValid = false;
        public int PageNumber { get; set; }
        public string PageNumberValue
        {
            get { return FPageNumberValue; }
            set
            {
                FPageNumberValue = value;
                int Dummy = 0;
                PageNumberIsValid = int.TryParse(FPageNumberValue, out Dummy);
                PageNumber = (PageNumberIsValid) ? Dummy : 0;
            }
        }
        
        public string BankStmtDate { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            MasterEventIdValue = "";
            BankImportTypeIdValue = "";
            BankNameIdValue = "";
            ReceiptSetIdValue = "";
            StartingReceiptNumberValue = "";
            PageNumberValue = "";
            BankStmtDate = "";
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
                    Key = "BankImportTypeId";
                    if (!BankImportTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankNameId";
                    if (!BankNameIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptSetId";
                    if (!ReceiptSetIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StartingReceiptNumber";
                    if (!StartingReceiptNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PageNumber";
                    if (!PageNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankStmtDate";
                    if (string.IsNullOrEmpty(BankStmtDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(ImportBankingFileRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.MasterEventIdValue = Source.MasterEventIdValue;
                this.BankImportTypeIdValue = Source.BankImportTypeIdValue;
                this.BankNameIdValue = Source.BankNameIdValue;
                this.ReceiptSetIdValue = Source.ReceiptSetIdValue;
                this.StartingReceiptNumberValue = Source.StartingReceiptNumberValue;
                this.PageNumberValue = Source.PageNumberValue;
                this.BankStmtDate = Source.BankStmtDate;
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
            
                    ImportBankingFileRequestItem TempItem = new ImportBankingFileRequestItem();
                    //Result = CommonMethods.LoadImportBankingFileRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadImportBankingFileRequestItem(ActionBase, ref TempItem);
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
