/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 17-09-2020
 * Time: 7:57 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/LedgerMarkUnmark/Actions/Templates/RemoveGridEntryRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations.LedgerMarkUnmark
{
    public class RemoveGridEntryRequestItem
    {
        #region Properties - Getters and Setters
        public string CallStart { get; set; }
        
        private string FTransactionIdValue = "";
        public bool TransactionIdIsValid = false;
        public int TransactionId { get; set; }
        public string TransactionIdValue
        {
            get { return FTransactionIdValue; }
            set
            {
                FTransactionIdValue = value;
                int Dummy = 0;
                TransactionIdIsValid = int.TryParse(FTransactionIdValue, out Dummy);
                TransactionId = (TransactionIdIsValid) ? Dummy : 0;
            }
        }
        
        public string FindText { get; set; }
        
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
        
        private string FOptionsIdValue = "";
        public bool OptionsIdIsValid = false;
        public int OptionsId { get; set; }
        public string OptionsIdValue
        {
            get { return FOptionsIdValue; }
            set
            {
                FOptionsIdValue = value;
                int Dummy = 0;
                OptionsIdIsValid = int.TryParse(FOptionsIdValue, out Dummy);
                OptionsId = (OptionsIdIsValid) ? Dummy : 0;
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
            CallStart = "";
            TransactionIdValue = "";
            SubLedgerIdValue = "";
            OptionsIdValue = "";
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
                    Key = "CallStart";
                    if (string.IsNullOrEmpty(CallStart)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransactionId";
                    if (!TransactionIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SubLedgerId";
                    if (!SubLedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OptionsId";
                    if (!OptionsIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(RemoveGridEntryRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.CallStart = Source.CallStart;
                this.TransactionIdValue = Source.TransactionIdValue;
                this.FindText = Source.FindText;
                this.SubLedgerIdValue = Source.SubLedgerIdValue;
                this.OptionsIdValue = Source.OptionsIdValue;
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
            
                    RemoveGridEntryRequestItem TempItem = new RemoveGridEntryRequestItem();
                    //Result = CommonMethods.LoadRemoveGridEntryRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadRemoveGridEntryRequestItem(ActionBase, ref TempItem);
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
