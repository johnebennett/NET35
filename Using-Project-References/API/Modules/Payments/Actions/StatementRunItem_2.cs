/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 13-07-2019
 * Time: 5:49 PM

 * SourceFile: D:\Source\EAOffice\HelperApps\MakeItemClasses\bin\Debug\Files\StatementRunItem.json
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
        
        public string RunDate { get; set; }
        
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
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            IdValue = "";
            RunIdValue = "";
            RunType = "";
            DirectEntryDiskCreated = "";
            MultiDirectRun = "";
            BPA = "";
            PaymentEventIdValue = "";
            StatementPreview = "";
            AccountTypeValue = "";
            RunDate = "";
            AmountValue = "";
            EntriesValue = "";
            PaymentFileStatus = "";
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
                    if (string.IsNullOrEmpty(IdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
                    else if (Id < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunId";
                    if (string.IsNullOrEmpty(RunIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
                    else if (RunId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunType";
                    if (string.IsNullOrEmpty(RunType)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    if (string.IsNullOrEmpty(PaymentEventIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
                    else if (PaymentEventId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StatementPreview";
                    if (string.IsNullOrEmpty(StatementPreview)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AccountType";
                    if (string.IsNullOrEmpty(AccountTypeValue)) Result = string.Format(BaseFormat, Key, NotProvided);
                    else if (AccountType < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunDate";
                    if (string.IsNullOrEmpty(RunDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount";
                    if (!AmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Entries";
                    if (string.IsNullOrEmpty(EntriesValue)) Result = string.Format(BaseFormat, Key, NotProvided);
                    else if (Entries < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PaymentFileStatus";
                    if (string.IsNullOrEmpty(PaymentFileStatus)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
            }
            catch (Exception Ex)
            {
                Result =
                    Ex.InnerException + Environment.NewLine +
                    Ex.Message + Environment.NewLine +
                    Ex.StackTrace;
                }
            
            return Result;
        }
        #endregion
        
        #region Load for CompanyId and CloseAccountRunId
        public string Load(string ConnectionString, Int64 CompanyId, Int64 CloseAccountRunId)
            {
                string Result = "";
                string Method = "Load";
                string ClassName = this.GetType().ToString();
            
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
            
                ActionBase actionBase = new ActionBase();
                actionBase.DebugMode = true;
                actionBase.LogOnInsideMethod(Method);
            
                DataTable dt = null;
                try
                {
                    dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
                    if (dt == null)
                    {
                        Result = actionBase.LogOnProcessingError(Method, "No tables in the dataset to process", "");
                        return Result;
                    }
            
                    Clear();
            
                    if (dt.Rows.Count > 0)
                    {
                        DataRow Row = dt.Rows[0];
                    }
                }
                catch (Exception Ex)
                {
                    Result = actionBase.LogOnException(ClassName + "." + Method, Ex, "");
                    return Result;
                }
                finally
                {
                    actionBase.LogOnLeavingMethod(Method);
                }
            
                return Result;
        }
        #endregion
        
    }
}
