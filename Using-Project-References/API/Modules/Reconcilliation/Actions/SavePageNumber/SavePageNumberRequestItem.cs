/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 23-10-2019
 * Time: 3:25 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Reconcilliation\Actions\SavePageNumber\Templates\SavePageNumberRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation.SavePageNumber
{
    public class SavePageNumberRequestItem
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
        public string Update(SavePageNumberRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.BankIdValue = Source.BankIdValue;
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
            
                    SavePageNumberRequestItem TempItem = new SavePageNumberRequestItem();
                    //Result = CommonMethods.LoadSavePageNumberRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSavePageNumberRequestItem(AActionBase, ref TempItem);
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
