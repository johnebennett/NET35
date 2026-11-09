/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 28-12-2019
 * Time: 11:33 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\LookupLists\Actions\Templates\TransactionLookupRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.LookupLists
{
    public class TransactionLookupRequestItem
    {
        #region Properties - Getters and Setters
        private string FTBAValue = "";
        public bool TBAIsValid = false;
        public int TBA { get; set; }
        public string TBAValue
        {
            get { return FTBAValue; }
            set
            {
                FTBAValue = value;
                int Dummy = 0;
                TBAIsValid = int.TryParse(FTBAValue, out Dummy);
                TBA = (TBAIsValid) ? Dummy : 0;
            }
        }
        
        private string FCreditorAccountIdValue = "";
        public bool CreditorAccountIdIsValid = false;
        public int CreditorAccountId { get; set; }
        public string CreditorAccountIdValue
        {
            get { return FCreditorAccountIdValue; }
            set
            {
                FCreditorAccountIdValue = value;
                int Dummy = 0;
                CreditorAccountIdIsValid = int.TryParse(FCreditorAccountIdValue, out Dummy);
                CreditorAccountId = (CreditorAccountIdIsValid) ? Dummy : 0;
            }
        }
        
        public string SearchText { get; set; }
        
        private string FSearchTypeValue = "";
        public bool SearchTypeIsValid = false;
        public int SearchType { get; set; }
        public string SearchTypeValue
        {
            get { return FSearchTypeValue; }
            set
            {
                FSearchTypeValue = value;
                int Dummy = 0;
                SearchTypeIsValid = int.TryParse(FSearchTypeValue, out Dummy);
                SearchType = (SearchTypeIsValid) ? Dummy : 0;
            }
        }
        
        private string FSearchStreetValue = "";
        public bool SearchStreetIsValid = false;
        public int SearchStreet { get; set; }
        public string SearchStreetValue
        {
            get { return FSearchStreetValue; }
            set
            {
                FSearchStreetValue = value;
                int Dummy = 0;
                SearchStreetIsValid = int.TryParse(FSearchStreetValue, out Dummy);
                SearchStreet = (SearchStreetIsValid) ? Dummy : 0;
            }
        }
        
        private string FSearchClientValue = "";
        public bool SearchClientIsValid = false;
        public int SearchClient { get; set; }
        public string SearchClientValue
        {
            get { return FSearchClientValue; }
            set
            {
                FSearchClientValue = value;
                int Dummy = 0;
                SearchClientIsValid = int.TryParse(FSearchClientValue, out Dummy);
                SearchClient = (SearchClientIsValid) ? Dummy : 0;
            }
        }
        
        private string FSearchOwnerValue = "";
        public bool SearchOwnerIsValid = false;
        public int SearchOwner { get; set; }
        public string SearchOwnerValue
        {
            get { return FSearchOwnerValue; }
            set
            {
                FSearchOwnerValue = value;
                int Dummy = 0;
                SearchOwnerIsValid = int.TryParse(FSearchOwnerValue, out Dummy);
                SearchOwner = (SearchOwnerIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            TBAValue = "";
            CreditorAccountIdValue = "";
            SearchTypeValue = "";
            SearchStreetValue = "";
            SearchClientValue = "";
            SearchOwnerValue = "";
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
                    Key = "TBA";
                    if (!TBAIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CreditorAccountId";
                    if (!CreditorAccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SearchType";
                    if (!SearchTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SearchStreet";
                    if (!SearchStreetIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SearchClient";
                    if (!SearchClientIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SearchOwner";
                    if (!SearchOwnerIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(TransactionLookupRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.TBAValue = Source.TBAValue;
                this.CreditorAccountIdValue = Source.CreditorAccountIdValue;
                this.SearchText = Source.SearchText;
                this.SearchTypeValue = Source.SearchTypeValue;
                this.SearchStreetValue = Source.SearchStreetValue;
                this.SearchClientValue = Source.SearchClientValue;
                this.SearchOwnerValue = Source.SearchOwnerValue;
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
            
                    TransactionLookupRequestItem TempItem = new TransactionLookupRequestItem();
                    //Result = CommonMethods.LoadTransactionLookupRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadTransactionLookupRequestItem(AActionBase, ref TempItem);
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
