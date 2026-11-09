/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 24-10-2019
 * Time: 9:11 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Reconcilliation\Actions\PresentUnpresent\Templates\PresentUnpresentRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation.PresentUnpresent
{
    public class PresentUnpresentRequestItem
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
        
        private string FRecordIdValue = "";
        public bool RecordIdIsValid = false;
        public int RecordId { get; set; }
        public string RecordIdValue
        {
            get { return FRecordIdValue; }
            set
            {
                FRecordIdValue = value;
                int Dummy = 0;
                RecordIdIsValid = int.TryParse(FRecordIdValue, out Dummy);
                RecordId = (RecordIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FPageNumberValue = "";
        public bool PageNumberIsValid = false;
        public int PageNumber { get; set; }
        public string PageNumberValue
        {
            get { return FPageNumberValue; }
            set
            {
                FPageNumberValue = value;
                int Dummy = 0;
                PageNumberIsValid = int.TryParse(FPageNumberValue, out Dummy);
                PageNumber = (PageNumberIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            BankIdValue = "";
            BankTypeValue = "";
            RecordIdValue = "";
            PageNumberValue = "";
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
                    Key = "RecordId";
                    if (!RecordIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PageNumber";
                    if (!PageNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(PresentUnpresentRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.BankIdValue = Source.BankIdValue;
                this.BankTypeValue = Source.BankTypeValue;
                this.RecordIdValue = Source.RecordIdValue;
                this.PageNumberValue = Source.PageNumberValue;
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
            
                    PresentUnpresentRequestItem TempItem = new PresentUnpresentRequestItem();
                    //Result = CommonMethods.LoadPresentUnpresentRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadPresentUnpresentRequestItem(AActionBase, ref TempItem);
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
