/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 08-08-2019
 * Time: 2:04 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Payments\Actions\Templates\PaymentAttachmentListRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PaymentOperations
{
    public class PaymentAttachmentListRequestItem
    {
        #region Properties - Getters and Setters
        private string FSummaryIdValue = "";
        public bool SummaryIdIsValid = false;
        public int SummaryId { get; set; }
        public string SummaryIdValue
        {
            get { return FSummaryIdValue; }
            set
            {
                FSummaryIdValue = value;
                int Dummy = 0;
                SummaryIdIsValid = int.TryParse(FSummaryIdValue, out Dummy);
                SummaryId = (SummaryIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            SummaryIdValue = "";
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
                    Key = "SummaryId";
                    if (!SummaryIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(PaymentAttachmentListRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.SummaryIdValue = Source.SummaryIdValue;
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
            
                    PaymentAttachmentListRequestItem TempItem = new PaymentAttachmentListRequestItem();
                    Result = CommonMethods.LoadPaymentAttachmentListRequestItem(DataObject, ref TempItem);
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
