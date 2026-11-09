/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 30-12-2019
 * Time: 3:52 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Invoices\Actions\Templates\InvoicesOnChangeRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Invoices
{
    public class InvoicesOnChangeRequestItem
    {
        #region Properties - Getters and Setters
        public string OnChangeType { get; set; }
        
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
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            OnChangeType = "";
            PassedIdValue = "";
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
                    Key = "OnChangeType";
                    if (string.IsNullOrEmpty(OnChangeType)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PassedId";
                    if (!PassedIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(InvoicesOnChangeRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.OnChangeType = Source.OnChangeType;
                this.PassedIdValue = Source.PassedIdValue;
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
            
                    InvoicesOnChangeRequestItem TempItem = new InvoicesOnChangeRequestItem();
                    //Result = CommonMethods.LoadInvoicesOnChangeRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadInvoicesOnChangeRequestItem(ActionBase, ref TempItem);
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
