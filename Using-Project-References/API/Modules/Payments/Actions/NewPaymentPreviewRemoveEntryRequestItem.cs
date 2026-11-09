/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 26-07-2019
 * Time: 7:28 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Payments\Actions\Templates\NewPaymentPreviewRemoveEntryRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PaymentOperations
{
    public class NewPaymentPreviewRemoveEntryRequestItem
    {
        #region Properties - Getters and Setters
        private string FIdValue = "";
        public bool IdIsValid = false;
        public int Id { get; set; }
        public string IdValue
        {
            get { return FIdValue; }
            set
            {
                FIdValue = value;
                int Dummy = 0;
                IdIsValid = int.TryParse(FIdValue, out Dummy);
                Id = (IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FRunIdValue = "";
        public bool RunIdIsValid = false;
        public int RunId { get; set; }
        public string RunIdValue
        {
            get { return FRunIdValue; }
            set
            {
                FRunIdValue = value;
                int Dummy = 0;
                RunIdIsValid = int.TryParse(FRunIdValue, out Dummy);
                RunId = (RunIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            IdValue = "";
            RunIdValue = "";
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
                    Key = "Id";
                    if (!IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunId";
                    if (!RunIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(NewPaymentPreviewRemoveEntryRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.IdValue = Source.IdValue;
                this.RunIdValue = Source.RunIdValue;
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
            
                    NewPaymentPreviewRemoveEntryRequestItem TempItem = new NewPaymentPreviewRemoveEntryRequestItem();
                    Result = CommonMethods.LoadNewPaymentPreviewRemoveEntryRequestItem(DataObject, ref TempItem);
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
