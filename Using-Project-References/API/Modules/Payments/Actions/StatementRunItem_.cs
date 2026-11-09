/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 06-09-2020
 * Time: 12:07 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Payments/Actions/Templates/StatementRunItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.PaymentOperations
{
    public class StatementRunItem
    {
        #region Properties - Getters and Setters
        private string FIdValue = "";
        public bool IdIsValid = false;
        public int Id { get; set; }
        public string IdValue
        {
            get { return FIdValue; }
            set
            {
                FIdValue = value;
                int Dummy = 0;
                IdIsValid = int.TryParse(FIdValue, out Dummy);
                Id = (IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FRunIdValue = "";
        public bool RunIdIsValid = false;
        public int RunId { get; set; }
        public string RunIdValue
        {
            get { return FRunIdValue; }
            set
            {
                FRunIdValue = value;
                int Dummy = 0;
                RunIdIsValid = int.TryParse(FRunIdValue, out Dummy);
                RunId = (RunIdIsValid) ? Dummy : 0;
            }
        }
        
        public string RunType { get; set; }
        
        public string RunDate { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string DirectEntryDiskCreated { get; set; }
        
        public string MultiDirectRun { get; set; }
        
        public string BPA { get; set; }
        
        private string FPaymentEventIdValue = "";
        public bool PaymentEventIdIsValid = false;
        public int PaymentEventId { get; set; }
        public string PaymentEventIdValue
        {
            get { return FPaymentEventIdValue; }
            set
            {
                FPaymentEventIdValue = value;
                int Dummy = 0;
                PaymentEventIdIsValid = int.TryParse(FPaymentEventIdValue, out Dummy);
                PaymentEventId = (PaymentEventIdIsValid) ? Dummy : 0;
            }
        }
        
        public string StatementPreview { get; set; }
        
        private string FAccountTypeValue = "";
        public bool AccountTypeIsValid = false;
        public int AccountType { get; set; }
        public string AccountTypeValue
        {
            get { return FAccountTypeValue; }
            set
            {
                FAccountTypeValue = value;
                int Dummy = 0;
                AccountTypeIsValid = int.TryParse(FAccountTypeValue, out Dummy);
                AccountType = (AccountTypeIsValid) ? Dummy : 0;
            }
        }
        
        private string FAmountValue = "";
        public bool AmountIsValid = false;
        public double Amount { get; set; }
        public string AmountValue
        {
            get { return FAmountValue; }
            set
            {
                FAmountValue = value;
                double Dummy = 0.0;
                AmountIsValid = Double.TryParse(FAmountValue, out Dummy);
                Amount = (AmountIsValid) ? Dummy : 0;
            }
        }
        
        private string FEntriesValue = "";
        public bool EntriesIsValid = false;
        public int Entries { get; set; }
        public string EntriesValue
        {
            get { return FEntriesValue; }
            set
            {
                FEntriesValue = value;
                int Dummy = 0;
                EntriesIsValid = int.TryParse(FEntriesValue, out Dummy);
                Entries = (EntriesIsValid) ? Dummy : 0;
            }
        }
        
        public string PaymentFileStatus { get; set; }
        
        public string FromConversion { get; set; }
        
        public string EventRunCreated { get; set; }
        
        public string FileCreatedOn { get; set; }
        
        public string FileCreatedBy { get; set; }
        
        public string ShowPost { get; set; }
        
        public string ShowReverse { get; set; }
        
        public string ShowPresent { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            IdValue = "";
            RunIdValue = "";
            RunType = "";
            RunDate = "";
            DirectEntryDiskCreated = "";
            MultiDirectRun = "";
            BPA = "";
            PaymentEventIdValue = "";
            StatementPreview = "";
            AccountTypeValue = "";
            AmountValue = "";
            EntriesValue = "";
            FromConversion = "";
            EventRunCreated = "";
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
                    Key = "Id";
                    if (!IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunId";
                    if (!RunIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunType";
                    if (string.IsNullOrEmpty(RunType)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunDate";
                    if (string.IsNullOrEmpty(RunDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DirectEntryDiskCreated";
                    if (string.IsNullOrEmpty(DirectEntryDiskCreated)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiDirectRun";
                    if (string.IsNullOrEmpty(MultiDirectRun)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BPA";
                    if (string.IsNullOrEmpty(BPA)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PaymentEventId";
                    if (!PaymentEventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StatementPreview";
                    if (string.IsNullOrEmpty(StatementPreview)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AccountType";
                    if (!AccountTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount";
                    if (!AmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Entries";
                    if (!EntriesIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FromConversion";
                    if (string.IsNullOrEmpty(FromConversion)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EventRunCreated";
                    if (string.IsNullOrEmpty(EventRunCreated)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(StatementRunItem Source)
        {
            string Result = "";
            
            try
            {
                this.IdValue = Source.IdValue;
                this.RunIdValue = Source.RunIdValue;
                this.RunType = Source.RunType;
                this.RunDate = Source.RunDate;
                this.CreatedBy = Source.CreatedBy;
                this.DirectEntryDiskCreated = Source.DirectEntryDiskCreated;
                this.MultiDirectRun = Source.MultiDirectRun;
                this.BPA = Source.BPA;
                this.PaymentEventIdValue = Source.PaymentEventIdValue;
                this.StatementPreview = Source.StatementPreview;
                this.AccountTypeValue = Source.AccountTypeValue;
                this.AmountValue = Source.AmountValue;
                this.EntriesValue = Source.EntriesValue;
                this.PaymentFileStatus = Source.PaymentFileStatus;
                this.FromConversion = Source.FromConversion;
                this.EventRunCreated = Source.EventRunCreated;
                this.FileCreatedOn = Source.FileCreatedOn;
                this.FileCreatedBy = Source.FileCreatedBy;
                this.ShowPost = Source.ShowPost;
                this.ShowReverse = Source.ShowReverse;
                this.ShowPresent = Source.ShowPresent;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load for CompanyId and CloseAccountRunId
        public string Load(ActionBase AActionBase, string ConnectionString, Int64 CompanyId, Int64 CloseAccountRunId)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".Load";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    StatementRunItem TempItem = new StatementRunItem();
                    Result =
                        CommonMethods.LoadStatementRunItem(ConnectionString, CompanyId, CloseAccountRunId, 0, ref TempItem);
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
                    Result = AActionBase.LogOnException(ClassName + "." + Method, Ex, "");
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
