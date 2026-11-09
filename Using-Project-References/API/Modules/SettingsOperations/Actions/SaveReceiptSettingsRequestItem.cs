/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 04-01-2021
 * Time: 6:43 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/Actions/Templates/SaveReceiptSettingsRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations
{
    public class SaveReceiptSettingsRequestItem
    {
        #region Properties - Getters and Setters
        public string PrintCashChequeRec { get; set; }
        
        public string PrintDirectRec { get; set; }
        
        public string AutoInsertAgentNumbersForResidential { get; set; }
        
        public string AutoInsertAgentNumbersForCommercial { get; set; }
        
        public string AutoInsertAgentNumbersforStrataLotOwners { get; set; }
        
        public string AutoAgentNumberAddCheckDigit { get; set; }
        
        private string FAutoAgentNumberMinimumLengthValue = "";
        public bool AutoAgentNumberMinimumLengthIsValid = false;
        public int AutoAgentNumberMinimumLength { get; set; }
        public string AutoAgentNumberMinimumLengthValue
        {
            get { return FAutoAgentNumberMinimumLengthValue; }
            set
            {
                FAutoAgentNumberMinimumLengthValue = value;
                int Dummy = 0;
                AutoAgentNumberMinimumLengthIsValid = int.TryParse(FAutoAgentNumberMinimumLengthValue, out Dummy);
                AutoAgentNumberMinimumLength = (AutoAgentNumberMinimumLengthIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PrintCashChequeRec = "";
            PrintDirectRec = "";
            AutoInsertAgentNumbersForResidential = "";
            AutoInsertAgentNumbersForCommercial = "";
            AutoInsertAgentNumbersforStrataLotOwners = "";
            AutoAgentNumberAddCheckDigit = "";
            AutoAgentNumberMinimumLengthValue = "";
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
                    Key = "PrintCashChequeRec";
                    if (string.IsNullOrEmpty(PrintCashChequeRec)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PrintDirectRec";
                    if (string.IsNullOrEmpty(PrintDirectRec)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AutoInsertAgentNumbersForResidential";
                    if (string.IsNullOrEmpty(AutoInsertAgentNumbersForResidential)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AutoInsertAgentNumbersForCommercial";
                    if (string.IsNullOrEmpty(AutoInsertAgentNumbersForCommercial)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AutoInsertAgentNumbersforStrataLotOwners";
                    if (string.IsNullOrEmpty(AutoInsertAgentNumbersforStrataLotOwners)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AutoAgentNumberAddCheckDigit";
                    if (string.IsNullOrEmpty(AutoAgentNumberAddCheckDigit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AutoAgentNumberMinimumLength";
                    if (!AutoAgentNumberMinimumLengthIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SaveReceiptSettingsRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PrintCashChequeRec = Source.PrintCashChequeRec;
                this.PrintDirectRec = Source.PrintDirectRec;
                this.AutoInsertAgentNumbersForResidential = Source.AutoInsertAgentNumbersForResidential;
                this.AutoInsertAgentNumbersForCommercial = Source.AutoInsertAgentNumbersForCommercial;
                this.AutoInsertAgentNumbersforStrataLotOwners = Source.AutoInsertAgentNumbersforStrataLotOwners;
                this.AutoAgentNumberAddCheckDigit = Source.AutoAgentNumberAddCheckDigit;
                this.AutoAgentNumberMinimumLengthValue = Source.AutoAgentNumberMinimumLengthValue;
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
            
                    SaveReceiptSettingsRequestItem TempItem = new SaveReceiptSettingsRequestItem();
                    //Result = CommonMethods.LoadSaveReceiptSettingsRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSaveReceiptSettingsRequestItem(ActionBase, ref TempItem);
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
