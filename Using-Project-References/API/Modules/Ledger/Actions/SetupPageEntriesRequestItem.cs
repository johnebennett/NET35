/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 10-12-2019
 * Time: 4:06 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Ledger\Actions\Templates\SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Ledger
{
    public class SetupPageEntriesRequestItem
    {
        #region Properties - Getters and Setters
        private string FBranchIdValue = "";
        public bool BranchIdIsValid = false;
        public int BranchId { get; set; }
        public string BranchIdValue
        {
            get { return FBranchIdValue; }
            set
            {
                FBranchIdValue = value;
                int Dummy = 0;
                BranchIdIsValid = int.TryParse(FBranchIdValue, out Dummy);
                BranchId = (BranchIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FManagerIdValue = "";
        public bool ManagerIdIsValid = false;
        public int ManagerId { get; set; }
        public string ManagerIdValue
        {
            get { return FManagerIdValue; }
            set
            {
                FManagerIdValue = value;
                int Dummy = 0;
                ManagerIdIsValid = int.TryParse(FManagerIdValue, out Dummy);
                ManagerId = (ManagerIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FLedgerIdValue = "";
        public bool LedgerIdIsValid = false;
        public int LedgerId { get; set; }
        public string LedgerIdValue
        {
            get { return FLedgerIdValue; }
            set
            {
                FLedgerIdValue = value;
                int Dummy = 0;
                LedgerIdIsValid = int.TryParse(FLedgerIdValue, out Dummy);
                LedgerId = (LedgerIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FEventIdValue = "";
        public bool EventIdIsValid = false;
        public int EventId { get; set; }
        public string EventIdValue
        {
            get { return FEventIdValue; }
            set
            {
                FEventIdValue = value;
                int Dummy = 0;
                EventIdIsValid = int.TryParse(FEventIdValue, out Dummy);
                EventId = (EventIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FReportIdValue = "";
        public bool ReportIdIsValid = false;
        public int ReportId { get; set; }
        public string ReportIdValue
        {
            get { return FReportIdValue; }
            set
            {
                FReportIdValue = value;
                int Dummy = 0;
                ReportIdIsValid = int.TryParse(FReportIdValue, out Dummy);
                ReportId = (ReportIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FActiveStatusValue = "";
        public bool ActiveStatusIsValid = false;
        public int ActiveStatus { get; set; }
        public string ActiveStatusValue
        {
            get { return FActiveStatusValue; }
            set
            {
                FActiveStatusValue = value;
                int Dummy = 0;
                ActiveStatusIsValid = int.TryParse(FActiveStatusValue, out Dummy);
                ActiveStatus = (ActiveStatusIsValid) ? Dummy : 0;
            }
        }
        
        private string FRunNumberValue = "";
        public bool RunNumberIsValid = false;
        public int RunNumber { get; set; }
        public string RunNumberValue
        {
            get { return FRunNumberValue; }
            set
            {
                FRunNumberValue = value;
                int Dummy = 0;
                RunNumberIsValid = int.TryParse(FRunNumberValue, out Dummy);
                RunNumber = (RunNumberIsValid) ? Dummy : 0;
            }
        }
        
        private string FScreenOrBulkValue = "";
        public bool ScreenOrBulkIsValid = false;
        public int ScreenOrBulk { get; set; }
        public string ScreenOrBulkValue
        {
            get { return FScreenOrBulkValue; }
            set
            {
                FScreenOrBulkValue = value;
                int Dummy = 0;
                ScreenOrBulkIsValid = int.TryParse(FScreenOrBulkValue, out Dummy);
                ScreenOrBulk = (ScreenOrBulkIsValid) ? Dummy : 0;
            }
        }
        
        private string FGSTRegisteredValue = "";
        public bool GSTRegisteredIsValid = false;
        public int GSTRegistered { get; set; }
        public string GSTRegisteredValue
        {
            get { return FGSTRegisteredValue; }
            set
            {
                FGSTRegisteredValue = value;
                int Dummy = 0;
                GSTRegisteredIsValid = int.TryParse(FGSTRegisteredValue, out Dummy);
                GSTRegistered = (GSTRegisteredIsValid) ? Dummy : 0;
            }
        }
        
        public string FindText { get; set; }
        
        public string DateFrom { get; set; }
        
        public string DateTo { get; set; }
        
        public string DateShow { get; set; }
        
        public string OnlyShowPDFEntreis { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            BranchIdValue = "";
            ManagerIdValue = "";
            LedgerIdValue = "";
            EventIdValue = "";
            ReportIdValue = "";
            ActiveStatusValue = "";
            RunNumberValue = "";
            ScreenOrBulkValue = "";
            GSTRegisteredValue = "";
            DateFrom = "";
            DateTo = "";
            DateShow = "";
            OnlyShowPDFEntreis = "";
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
                    Key = "BranchId";
                    if (!BranchIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ManagerId";
                    if (!ManagerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LedgerId";
                    if (!LedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EventId";
                    if (!EventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReportId";
                    if (!ReportIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ActiveStatus";
                    if (!ActiveStatusIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunNumber";
                    if (!RunNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ScreenOrBulk";
                    if (!ScreenOrBulkIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GSTRegistered";
                    if (!GSTRegisteredIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    Key = "DateShow";
                    if (string.IsNullOrEmpty(DateShow)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OnlyShowPDFEntreis";
                    if (string.IsNullOrEmpty(OnlyShowPDFEntreis)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SetupPageEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.BranchIdValue = Source.BranchIdValue;
                this.ManagerIdValue = Source.ManagerIdValue;
                this.LedgerIdValue = Source.LedgerIdValue;
                this.EventIdValue = Source.EventIdValue;
                this.ReportIdValue = Source.ReportIdValue;
                this.ActiveStatusValue = Source.ActiveStatusValue;
                this.RunNumberValue = Source.RunNumberValue;
                this.ScreenOrBulkValue = Source.ScreenOrBulkValue;
                this.GSTRegisteredValue = Source.GSTRegisteredValue;
                this.FindText = Source.FindText;
                this.DateFrom = Source.DateFrom;
                this.DateTo = Source.DateTo;
                this.DateShow = Source.DateShow;
                this.OnlyShowPDFEntreis = Source.OnlyShowPDFEntreis;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
        //public string Load(ActionBase AActionBase, JObject DataObject)
        public string Load(ActionBase AActionBase)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                //string Method = ClassName + ".LoadFromDataObject";
                string Method = ClassName + ".LoadFromActionBase";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    SetupPageEntriesRequestItem TempItem = new SetupPageEntriesRequestItem();
                    //Result = CommonMethods.LoadSetupPageEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSetupPageEntriesRequestItem(AActionBase, ref TempItem);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = AActionBase.LogOnProcessingError(Method, Result, "");
                    }
                    else
                    {
                        Result = Update(TempItem);
                    }
                }
                catch (Exception Ex)
                {
                    Result = AActionBase.LogOnException(Method, Ex, "");
                    return Result;
                }
                finally
                {
                    AActionBase.LogOnLeavingMethod(Method);
                }
            
                return Result;
        }
        #endregion
        
    }
}
