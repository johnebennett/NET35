/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 30-01-2020
 * Time: 9:37 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Invoices\Actions\Templates\InvoiceReverseProcessRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Invoices
{
    public class InvoiceReverseProcessRequestItem
    {
        #region Properties - Getters and Setters
        private string FProcessIdValue = "";
        public bool ProcessIdIsValid = false;
        public int ProcessId { get; set; }
        public string ProcessIdValue
        {
            get { return FProcessIdValue; }
            set
            {
                FProcessIdValue = value;
                int Dummy = 0;
                ProcessIdIsValid = int.TryParse(FProcessIdValue, out Dummy);
                ProcessId = (ProcessIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FPassedIdValue = "";
        public bool PassedIdIsValid = false;
        public int PassedId { get; set; }
        public string PassedIdValue
        {
            get { return FPassedIdValue; }
            set
            {
                FPassedIdValue = value;
                int Dummy = 0;
                PassedIdIsValid = int.TryParse(FPassedIdValue, out Dummy);
                PassedId = (PassedIdIsValid) ? Dummy : 0;
            }
        }
        
        public string ReverseDate { get; set; }
        
        public string ReverseInvoice { get; set; }
        
        public string ReverseDebt { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            ProcessIdValue = "";
            PassedIdValue = "";
            ReverseDate = "";
            ReverseInvoice = "";
            ReverseDebt = "";
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
                    Key = "ProcessId";
                    if (!ProcessIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PassedId";
                    if (!PassedIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReverseDate";
                    if (string.IsNullOrEmpty(ReverseDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReverseInvoice";
                    if (string.IsNullOrEmpty(ReverseInvoice)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReverseDebt";
                    if (string.IsNullOrEmpty(ReverseDebt)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(InvoiceReverseProcessRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.ProcessIdValue = Source.ProcessIdValue;
                this.PassedIdValue = Source.PassedIdValue;
                this.ReverseDate = Source.ReverseDate;
                this.ReverseInvoice = Source.ReverseInvoice;
                this.ReverseDebt = Source.ReverseDebt;
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
            
                    InvoiceReverseProcessRequestItem TempItem = new InvoiceReverseProcessRequestItem();
                    //Result = CommonMethods.LoadInvoiceReverseProcessRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadInvoiceReverseProcessRequestItem(ActionBase, ref TempItem);
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
