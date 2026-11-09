/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 07-05-2020
 * Time: 6:17 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Receipts\Actions\Templates\ReceiptScreenChargePopupRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Receipts
{
    public class ReceiptScreenChargePopupRequestItem
    {
        #region Properties - Getters and Setters
        private string FIDValue = "";
        public bool IDIsValid = false;
        public int ID { get; set; }
        public string IDValue
        {
            get { return FIDValue; }
            set
            {
                FIDValue = value;
                int Dummy = 0;
                IDIsValid = int.TryParse(FIDValue, out Dummy);
                ID = (IDIsValid) ? Dummy : 0;
            }
        }
        
        private string FPayingAmountValue = "";
        public bool PayingAmountIsValid = false;
        public double PayingAmount { get; set; }
        public string PayingAmountValue
        {
            get { return FPayingAmountValue; }
            set
            {
                FPayingAmountValue = value;
                double Dummy = 0.0;
                PayingAmountIsValid = Double.TryParse(FPayingAmountValue, out Dummy);
                PayingAmount = (PayingAmountIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            IDValue = "";
            PayingAmountValue = "";
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
                    Key = "ID";
                    if (!IDIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PayingAmount";
                    if (!PayingAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(ReceiptScreenChargePopupRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.IDValue = Source.IDValue;
                this.PayingAmountValue = Source.PayingAmountValue;
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
            
                    ReceiptScreenChargePopupRequestItem TempItem = new ReceiptScreenChargePopupRequestItem();
                    //Result = CommonMethods.LoadReceiptScreenChargePopupRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadReceiptScreenChargePopupRequestItem(AActionBase, ref TempItem);
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
