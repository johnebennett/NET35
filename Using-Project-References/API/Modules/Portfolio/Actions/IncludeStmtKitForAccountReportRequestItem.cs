/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 20-02-2020
 * Time: 12:01 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\Actions\Templates\IncludeStmtKitForAccountReportRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio
{
    public class IncludeStmtKitForAccountReportRequestItem
    {
        #region Properties - Getters and Setters
        private string FAccountIdValue = "";
        public bool AccountIdIsValid = false;
        public int AccountId { get; set; }
        public string AccountIdValue
        {
            get { return FAccountIdValue; }
            set
            {
                FAccountIdValue = value;
                int Dummy = 0;
                AccountIdIsValid = int.TryParse(FAccountIdValue, out Dummy);
                AccountId = (AccountIdIsValid) ? Dummy : 0;
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
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            AccountIdValue = "";
            ReportIdValue = "";
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
                    Key = "AccountId";
                    if (!AccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReportId";
                    if (!ReportIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(IncludeStmtKitForAccountReportRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.AccountIdValue = Source.AccountIdValue;
                this.ReportIdValue = Source.ReportIdValue;
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
                string Method = ClassName + ".LoadFromDataObject";
            
                ActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    IncludeStmtKitForAccountReportRequestItem TempItem = new IncludeStmtKitForAccountReportRequestItem();
                    Result = CommonMethods.LoadIncludeStmtKitForAccountReportRequestItem(ActionBase, ref TempItem);
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
