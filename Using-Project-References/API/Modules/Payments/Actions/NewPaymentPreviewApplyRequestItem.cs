/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 25-07-2019
 * Time: 7:45 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Payments\Actions\Templates\NewPaymentPreviewApplyRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PaymentOperations
{
    public class NewPaymentPreviewApplyRequestItem
    {
        #region Properties - Getters and Setters
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
        
        private string FAccountTypeIdValue = "";
        public bool AccountTypeIdIsValid = false;
        public int AccountTypeId { get; set; }
        public string AccountTypeIdValue
        {
            get { return FAccountTypeIdValue; }
            set
            {
                FAccountTypeIdValue = value;
                int Dummy = 0;
                AccountTypeIdIsValid = int.TryParse(FAccountTypeIdValue, out Dummy);
                AccountTypeId = (AccountTypeIdIsValid) ? Dummy : 0;
            }
        }
        
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
        
        private string FOrderIdValue = "";
        public bool OrderIdIsValid = false;
        public int OrderId { get; set; }
        public string OrderIdValue
        {
            get { return FOrderIdValue; }
            set
            {
                FOrderIdValue = value;
                int Dummy = 0;
                OrderIdIsValid = int.TryParse(FOrderIdValue, out Dummy);
                OrderId = (OrderIdIsValid) ? Dummy : 0;
            }
        }
        
        public string MidMonth { get; set; }
        
        private string FAccountIdValue = "";
        public bool AccountIdIsValid = false;
        public int AccountId { get; set; }
        public string AccountIdValue
        {
            get { return FAccountIdValue; }
            set
            {
                FAccountIdValue = value;
                int Dummy = 0;
                AccountIdIsValid = int.TryParse(FAccountIdValue, out Dummy);
                AccountId = (AccountIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FRunNumberIdValue = "";
        public bool RunNumberIdIsValid = false;
        public int RunNumberId { get; set; }
        public string RunNumberIdValue
        {
            get { return FRunNumberIdValue; }
            set
            {
                FRunNumberIdValue = value;
                int Dummy = 0;
                RunNumberIdIsValid = int.TryParse(FRunNumberIdValue, out Dummy);
                RunNumberId = (RunNumberIdIsValid) ? Dummy : 0;
            }
        }
        
        public string AllIsSelected { get; set; }
        
        public string Preview { get; set; }
        
        public string MultiDirectRun { get; set; }
        
        private string FDirectEntryUserNumberValue = "";
        public bool DirectEntryUserNumberIsValid = false;
        public int DirectEntryUserNumber { get; set; }
        public string DirectEntryUserNumberValue
        {
            get { return FDirectEntryUserNumberValue; }
            set
            {
                FDirectEntryUserNumberValue = value;
                int Dummy = 0;
                DirectEntryUserNumberIsValid = int.TryParse(FDirectEntryUserNumberValue, out Dummy);
                DirectEntryUserNumber = (DirectEntryUserNumberIsValid) ? Dummy : 0;
            }
        }
        
        public string BPay { get; set; }
        
        public string WhoBy { get; set; }
        
        public string StatementDate { get; set; }
        
        private string FBPayBankUserIdValue = "";
        public bool BPayBankUserIdIsValid = false;
        public int BPayBankUserId { get; set; }
        public string BPayBankUserIdValue
        {
            get { return FBPayBankUserIdValue; }
            set
            {
                FBPayBankUserIdValue = value;
                int Dummy = 0;
                BPayBankUserIdIsValid = int.TryParse(FBPayBankUserIdValue, out Dummy);
                BPayBankUserId = (BPayBankUserIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FBPayMinimumValue = "";
        public bool BPayMinimumIsValid = false;
        public double BPayMinimum { get; set; }
        public string BPayMinimumValue
        {
            get { return FBPayMinimumValue; }
            set
            {
                FBPayMinimumValue = value;
                double Dummy = 0.0;
                BPayMinimumIsValid = Double.TryParse(FBPayMinimumValue, out Dummy);
                BPayMinimum = (BPayMinimumIsValid) ? Dummy : 0;
            }
        }
        
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
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            RunIdValue = "";
            BankIdValue = "";
            AccountTypeIdValue = "";
            BranchIdValue = "";
            ManagerIdValue = "";
            MidMonth = "";
            AccountIdValue = "";
            RunNumberIdValue = "";
            AllIsSelected = "";
            Preview = "";
            MultiDirectRun = "";
            DirectEntryUserNumberValue = "";
            BPayBankUserIdValue = "";
            BPayMinimumValue = "";
            PaymentEventIdValue = "";
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
                    Key = "RunId";
                    if (!RunIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankId";
                    if (!BankIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AccountTypeId";
                    if (!AccountTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
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
                    Key = "MidMonth";
                    if (string.IsNullOrEmpty(MidMonth)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AccountId";
                    if (!AccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunNumberId";
                    if (!RunNumberIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllIsSelected";
                    if (string.IsNullOrEmpty(AllIsSelected)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Preview";
                    if (string.IsNullOrEmpty(Preview)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiDirectRun";
                    if (string.IsNullOrEmpty(MultiDirectRun)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DirectEntryUserNumber";
                    if (!DirectEntryUserNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BPayBankUserId";
                    if (!BPayBankUserIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BPayMinimum";
                    if (!BPayMinimumIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PaymentEventId";
                    if (!PaymentEventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(NewPaymentPreviewApplyRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.RunIdValue = Source.RunIdValue;
                this.BankIdValue = Source.BankIdValue;
                this.AccountTypeIdValue = Source.AccountTypeIdValue;
                this.BranchIdValue = Source.BranchIdValue;
                this.ManagerIdValue = Source.ManagerIdValue;
                this.OrderIdValue = Source.OrderIdValue;
                this.MidMonth = Source.MidMonth;
                this.AccountIdValue = Source.AccountIdValue;
                this.RunNumberIdValue = Source.RunNumberIdValue;
                this.AllIsSelected = Source.AllIsSelected;
                this.Preview = Source.Preview;
                this.MultiDirectRun = Source.MultiDirectRun;
                this.DirectEntryUserNumberValue = Source.DirectEntryUserNumberValue;
                this.BPay = Source.BPay;
                this.WhoBy = Source.WhoBy;
                this.StatementDate = Source.StatementDate;
                this.BPayBankUserIdValue = Source.BPayBankUserIdValue;
                this.BPayMinimumValue = Source.BPayMinimumValue;
                this.PaymentEventIdValue = Source.PaymentEventIdValue;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load for CompanyId and CloseAccountRunId
        public string Load(ActionBase AActionBase, JObject DataObject)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".LoadFromDataObject";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    NewPaymentPreviewApplyRequestItem TempItem = new NewPaymentPreviewApplyRequestItem();
                    Result = CommonMethods.LoadNewPaymentPreviewApplyRequestItem(DataObject, ref TempItem);
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
