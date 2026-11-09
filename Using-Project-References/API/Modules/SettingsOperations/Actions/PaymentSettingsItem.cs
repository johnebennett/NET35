/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 14-10-2019
 * Time: 6:46 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\SettingsOperations\Actions\Templates\PaymentSettingsItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.SettingsOperations
{
    public class PaymentSettingsItem
    {
        #region Properties - Getters and Setters
        public string Section { get; set; }
        
        private string FClientIdValue = "";
        public bool ClientIdIsValid = false;
        public int ClientId { get; set; }
        public string ClientIdValue
        {
            get { return FClientIdValue; }
            set
            {
                FClientIdValue = value;
                int Dummy = 0;
                ClientIdIsValid = int.TryParse(FClientIdValue, out Dummy);
                ClientId = (ClientIdIsValid) ? Dummy : 0;
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
        
        private string FLedgerTypeIdValue = "";
        public bool LedgerTypeIdIsValid = false;
        public int LedgerTypeId { get; set; }
        public string LedgerTypeIdValue
        {
            get { return FLedgerTypeIdValue; }
            set
            {
                FLedgerTypeIdValue = value;
                int Dummy = 0;
                LedgerTypeIdIsValid = int.TryParse(FLedgerTypeIdValue, out Dummy);
                LedgerTypeId = (LedgerTypeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FPaymentTypeIdValue = "";
        public bool PaymentTypeIdIsValid = false;
        public int PaymentTypeId { get; set; }
        public string PaymentTypeIdValue
        {
            get { return FPaymentTypeIdValue; }
            set
            {
                FPaymentTypeIdValue = value;
                int Dummy = 0;
                PaymentTypeIdIsValid = int.TryParse(FPaymentTypeIdValue, out Dummy);
                PaymentTypeId = (PaymentTypeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FDateRangeIdValue = "";
        public bool DateRangeIdIsValid = false;
        public int DateRangeId { get; set; }
        public string DateRangeIdValue
        {
            get { return FDateRangeIdValue; }
            set
            {
                FDateRangeIdValue = value;
                int Dummy = 0;
                DateRangeIdIsValid = int.TryParse(FDateRangeIdValue, out Dummy);
                DateRangeId = (DateRangeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FStatementOrderByIdValue = "";
        public bool StatementOrderByIdIsValid = false;
        public int StatementOrderById { get; set; }
        public string StatementOrderByIdValue
        {
            get { return FStatementOrderByIdValue; }
            set
            {
                FStatementOrderByIdValue = value;
                int Dummy = 0;
                StatementOrderByIdIsValid = int.TryParse(FStatementOrderByIdValue, out Dummy);
                StatementOrderById = (StatementOrderByIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            Section = "";
            ClientIdValue = "";
            BankNameIdValue = "";
            LedgerTypeIdValue = "";
            PaymentTypeIdValue = "";
            DateRangeIdValue = "";
            StatementOrderByIdValue = "";
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
                    Key = "Section";
                    if (string.IsNullOrEmpty(Section)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ClientId";
                    if (!ClientIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankNameId";
                    if (!BankNameIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LedgerTypeId";
                    if (!LedgerTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PaymentTypeId";
                    if (!PaymentTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DateRangeId";
                    if (!DateRangeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StatementOrderById";
                    if (!StatementOrderByIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(PaymentSettingsItem Source)
        {
            string Result = "";
            
            try
            {
                this.Section = Source.Section;
                this.ClientIdValue = Source.ClientIdValue;
                this.BankNameIdValue = Source.BankNameIdValue;
                this.LedgerTypeIdValue = Source.LedgerTypeIdValue;
                this.PaymentTypeIdValue = Source.PaymentTypeIdValue;
                this.DateRangeIdValue = Source.DateRangeIdValue;
                this.StatementOrderByIdValue = Source.StatementOrderByIdValue;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
        public string Load(ActionBase AActionBase, JObject DataObject)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".LoadFromDataObject";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    PaymentSettingsItem TempItem = new PaymentSettingsItem();
                    Result = CommonMethods.LoadPaymentSettingsItem(DataObject, ref TempItem);
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
        
        #region Load for CompanyId and ClientId
        public string Load(ActionBase AActionBase, string ConnectionString, Int64 CompanyId, string Section)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".LoadFromDB";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    PaymentSettingsItem TempItem = new PaymentSettingsItem();
                    Result =
                        CommonMethods.LoadPaymentSettingsItem(ConnectionString, CompanyId, Section, ref TempItem);
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
        
        #region Save
        public string Save(ActionBase AActionBase, string ConnectionString, Int64 CompanyId)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".Save";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Result = CommonMethods.SavePaymentSettingsItem(ConnectionString, CompanyId, this);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = AActionBase.LogOnProcessingError(Method, Result, "");
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
