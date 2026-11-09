/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 28-07-2020
 * Time: 7:14 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Independent\AddEditBankAccountDlg\Actions\Templates\AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Independent.AddEditBankAccountDlg
{
    public class AllPageEntriesRequestItem
    {
        #region Properties - Getters and Setters
        private string FAddEditModeValue = "";
        public bool AddEditModeIsValid = false;
        public int AddEditMode { get; set; }
        public string AddEditModeValue
        {
            get { return FAddEditModeValue; }
            set
            {
                FAddEditModeValue = value;
                int Dummy = 0;
                AddEditModeIsValid = int.TryParse(FAddEditModeValue, out Dummy);
                AddEditMode = (AddEditModeIsValid) ? Dummy : 0;
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
        
        public string BankName { get; set; }
        
        public string BranchName { get; set; }
        
        public string BSB { get; set; }
        
        public string AccountNumber { get; set; }
        
        public string Description { get; set; }
        
        public string ShortBankName { get; set; }
        
        public string ForResidentialLedgers { get; set; }
        
        public string ForCommercialLedgers { get; set; }
        
        public string ForSalesLedgers { get; set; }
        
        public string ForSecurityDepositLedgers { get; set; }
        
        public string ForStrataLedgers { get; set; }
        
        public string ForManagementLedgers { get; set; }
        
        public string CompanyName { get; set; }
        
        public string CompanyAddress { get; set; }
        
        public string EffectiveOfficer { get; set; }
        
        public string ExcludeEOM { get; set; }
        
        public string NSWUniqueTrustId { get; set; }
        
        public string DirectEntryUserNumber { get; set; }
        
        public string Inactive { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            AddEditModeValue = "";
            BankNameIdValue = "";
            BankName = "";
            BranchName = "";
            BSB = "";
            AccountNumber = "";
            Description = "";
            ShortBankName = "";
            ForResidentialLedgers = "";
            ForCommercialLedgers = "";
            ForSalesLedgers = "";
            ForSecurityDepositLedgers = "";
            ForStrataLedgers = "";
            ForManagementLedgers = "";
            CompanyName = "";
            CompanyAddress = "";
            EffectiveOfficer = "";
            ExcludeEOM = "";
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
                    Key = "AddEditMode";
                    if (!AddEditModeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankNameId";
                    if (!BankNameIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankName";
                    if (string.IsNullOrEmpty(BankName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BranchName";
                    if (string.IsNullOrEmpty(BranchName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BSB";
                    if (string.IsNullOrEmpty(BSB)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AccountNumber";
                    if (string.IsNullOrEmpty(AccountNumber)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Description";
                    if (string.IsNullOrEmpty(Description)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShortBankName";
                    if (string.IsNullOrEmpty(ShortBankName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ForResidentialLedgers";
                    if (string.IsNullOrEmpty(ForResidentialLedgers)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ForCommercialLedgers";
                    if (string.IsNullOrEmpty(ForCommercialLedgers)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ForSalesLedgers";
                    if (string.IsNullOrEmpty(ForSalesLedgers)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ForSecurityDepositLedgers";
                    if (string.IsNullOrEmpty(ForSecurityDepositLedgers)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ForStrataLedgers";
                    if (string.IsNullOrEmpty(ForStrataLedgers)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ForManagementLedgers";
                    if (string.IsNullOrEmpty(ForManagementLedgers)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CompanyName";
                    if (string.IsNullOrEmpty(CompanyName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CompanyAddress";
                    if (string.IsNullOrEmpty(CompanyAddress)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EffectiveOfficer";
                    if (string.IsNullOrEmpty(EffectiveOfficer)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ExcludeEOM";
                    if (string.IsNullOrEmpty(ExcludeEOM)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(AllPageEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.AddEditModeValue = Source.AddEditModeValue;
                this.BankNameIdValue = Source.BankNameIdValue;
                this.BankName = Source.BankName;
                this.BranchName = Source.BranchName;
                this.BSB = Source.BSB;
                this.AccountNumber = Source.AccountNumber;
                this.Description = Source.Description;
                this.ShortBankName = Source.ShortBankName;
                this.ForResidentialLedgers = Source.ForResidentialLedgers;
                this.ForCommercialLedgers = Source.ForCommercialLedgers;
                this.ForSalesLedgers = Source.ForSalesLedgers;
                this.ForSecurityDepositLedgers = Source.ForSecurityDepositLedgers;
                this.ForStrataLedgers = Source.ForStrataLedgers;
                this.ForManagementLedgers = Source.ForManagementLedgers;
                this.CompanyName = Source.CompanyName;
                this.CompanyAddress = Source.CompanyAddress;
                this.EffectiveOfficer = Source.EffectiveOfficer;
                this.ExcludeEOM = Source.ExcludeEOM;
                this.NSWUniqueTrustId = Source.NSWUniqueTrustId;
                this.DirectEntryUserNumber = Source.DirectEntryUserNumber;
                this.Inactive = Source.Inactive;
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
            
                    AllPageEntriesRequestItem TempItem = new AllPageEntriesRequestItem();
                    //Result = CommonMethods.LoadAllPageEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadAllPageEntriesRequestItem(ActionBase, ref TempItem);
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
