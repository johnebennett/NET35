/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 28-10-2019
 * Time: 4:58 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Reconcilliation\Actions\GetReconcilliationReport\Templates\GetReconcilliationReportRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation.GetReconcilliationReport
{
    public class GetReconcilliationReportRequestItem
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
        
        private string FReportIdValue = "";
        public bool ReportIdIsValid = false;
        public int ReportId { get; set; }
        public string ReportIdValue
        {
            get { return FReportIdValue; }
            set
            {
                FReportIdValue = value;
                int Dummy = 0;
                ReportIdIsValid = int.TryParse(FReportIdValue, out Dummy);
                ReportId = (ReportIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FButtonIdValue = "";
        public bool ButtonIdIsValid = false;
        public int ButtonId { get; set; }
        public string ButtonIdValue
        {
            get { return FButtonIdValue; }
            set
            {
                FButtonIdValue = value;
                int Dummy = 0;
                ButtonIdIsValid = int.TryParse(FButtonIdValue, out Dummy);
                ButtonId = (ButtonIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FComboIdValue = "";
        public bool ComboIdIsValid = false;
        public int ComboId { get; set; }
        public string ComboIdValue
        {
            get { return FComboIdValue; }
            set
            {
                FComboIdValue = value;
                int Dummy = 0;
                ComboIdIsValid = int.TryParse(FComboIdValue, out Dummy);
                ComboId = (ComboIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            BankIdValue = "";
            ReportIdValue = "";
            ButtonIdValue = "";
            ComboIdValue = "";
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
                    Key = "ReportId";
                    if (!ReportIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ButtonId";
                    if (!ButtonIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ComboId";
                    if (!ComboIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(GetReconcilliationReportRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.BankIdValue = Source.BankIdValue;
                this.ReportIdValue = Source.ReportIdValue;
                this.ButtonIdValue = Source.ButtonIdValue;
                this.ComboIdValue = Source.ComboIdValue;
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
            
                    GetReconcilliationReportRequestItem TempItem = new GetReconcilliationReportRequestItem();
                    //Result = CommonMethods.LoadGetReconcilliationReportRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadGetReconcilliationReportRequestItem(AActionBase, ref TempItem);
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
