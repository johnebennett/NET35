/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 16-10-2019
 * Time: 12:08 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Reconcilliation\Actions\GatherGridEntries\Templates\GatherGridEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation.GatherGridEntries
{
    public class GatherGridEntriesRequestItem
    {
        #region Properties - Getters and Setters
        private string FBankAccountTypeValue = "";
        public bool BankAccountTypeIsValid = false;
        public int BankAccountType { get; set; }
        public string BankAccountTypeValue
        {
            get { return FBankAccountTypeValue; }
            set
            {
                FBankAccountTypeValue = value;
                int Dummy = 0;
                BankAccountTypeIsValid = int.TryParse(FBankAccountTypeValue, out Dummy);
                BankAccountType = (BankAccountTypeIsValid) ? Dummy : 0;
            }
        }
        
        public string UseDefaults { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            BankAccountTypeValue = "";
            UseDefaults = "";
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
                    Key = "BankAccountType";
                    if (!BankAccountTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "UseDefaults";
                    if (string.IsNullOrEmpty(UseDefaults)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(GatherGridEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.BankAccountTypeValue = Source.BankAccountTypeValue;
                this.UseDefaults = Source.UseDefaults;
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
            
                    GatherGridEntriesRequestItem TempItem = new GatherGridEntriesRequestItem();
                    //Result = CommonMethods.LoadGatherGridEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadGatherGridEntriesRequestItem(AActionBase, ref TempItem);
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
