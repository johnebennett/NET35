/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 21-10-2019
 * Time: 7:47 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Reconcilliation\Actions\GatherReconcileGridEntries\Templates\GatherReconcileGridEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation.GatherReconcileGridEntries
{
    public class GatherReconcileGridEntriesRequestItem
    {
        #region Properties - Getters and Setters
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
        
        private string FBankTypeValue = "";
        public bool BankTypeIsValid = false;
        public int BankType { get; set; }
        public string BankTypeValue
        {
            get { return FBankTypeValue; }
            set
            {
                FBankTypeValue = value;
                int Dummy = 0;
                BankTypeIsValid = int.TryParse(FBankTypeValue, out Dummy);
                BankType = (BankTypeIsValid) ? Dummy : 0;
            }
        }
        
        private string FPageNoValue = "";
        public bool PageNoIsValid = false;
        public int PageNo { get; set; }
        public string PageNoValue
        {
            get { return FPageNoValue; }
            set
            {
                FPageNoValue = value;
                int Dummy = 0;
                PageNoIsValid = int.TryParse(FPageNoValue, out Dummy);
                PageNo = (PageNoIsValid) ? Dummy : 0;
            }
        }
        
        private string FPresentedPageNoValue = "";
        public bool PresentedPageNoIsValid = false;
        public int PresentedPageNo { get; set; }
        public string PresentedPageNoValue
        {
            get { return FPresentedPageNoValue; }
            set
            {
                FPresentedPageNoValue = value;
                int Dummy = 0;
                PresentedPageNoIsValid = int.TryParse(FPresentedPageNoValue, out Dummy);
                PresentedPageNo = (PresentedPageNoIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            BankIdValue = "";
            BankTypeValue = "";
            PageNoValue = "";
            PresentedPageNoValue = "";
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
                    Key = "BankId";
                    if (!BankIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BankType";
                    if (!BankTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PageNo";
                    if (!PageNoIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PresentedPageNo";
                    if (!PresentedPageNoIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(GatherReconcileGridEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.BankIdValue = Source.BankIdValue;
                this.BankTypeValue = Source.BankTypeValue;
                this.PageNoValue = Source.PageNoValue;
                this.PresentedPageNoValue = Source.PresentedPageNoValue;
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
            
                    GatherReconcileGridEntriesRequestItem TempItem = new GatherReconcileGridEntriesRequestItem();
                    //Result = CommonMethods.LoadGatherReconcileGridEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadGatherReconcileGridEntriesRequestItem(AActionBase, ref TempItem);
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
