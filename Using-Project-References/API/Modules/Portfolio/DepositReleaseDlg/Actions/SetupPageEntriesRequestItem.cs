/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 24-07-2020
 * Time: 7:57 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\DepositReleaseDlg\Actions\Templates\SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.DepositReleaseDlg
{
    public class SetupPageEntriesRequestItem
    {
        #region Properties - Getters and Setters
        private string FPortfolioIdValue = "";
        public bool PortfolioIdIsValid = false;
        public int PortfolioId { get; set; }
        public string PortfolioIdValue
        {
            get { return FPortfolioIdValue; }
            set
            {
                FPortfolioIdValue = value;
                int Dummy = 0;
                PortfolioIdIsValid = int.TryParse(FPortfolioIdValue, out Dummy);
                PortfolioId = (PortfolioIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FStatementRunIdValue = "";
        public bool StatementRunIdIsValid = false;
        public int StatementRunId { get; set; }
        public string StatementRunIdValue
        {
            get { return FStatementRunIdValue; }
            set
            {
                FStatementRunIdValue = value;
                int Dummy = 0;
                StatementRunIdIsValid = int.TryParse(FStatementRunIdValue, out Dummy);
                StatementRunId = (StatementRunIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FEventRunIdValue = "";
        public bool EventRunIdIsValid = false;
        public int EventRunId { get; set; }
        public string EventRunIdValue
        {
            get { return FEventRunIdValue; }
            set
            {
                FEventRunIdValue = value;
                int Dummy = 0;
                EventRunIdIsValid = int.TryParse(FEventRunIdValue, out Dummy);
                EventRunId = (EventRunIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FDepositReleaseTypeValue = "";
        public bool DepositReleaseTypeIsValid = false;
        public int DepositReleaseType { get; set; }
        public string DepositReleaseTypeValue
        {
            get { return FDepositReleaseTypeValue; }
            set
            {
                FDepositReleaseTypeValue = value;
                int Dummy = 0;
                DepositReleaseTypeIsValid = int.TryParse(FDepositReleaseTypeValue, out Dummy);
                DepositReleaseType = (DepositReleaseTypeIsValid) ? Dummy : 0;
            }
        }
        
        public string StmtDate { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            StatementRunIdValue = "";
            EventRunIdValue = "";
            DepositReleaseTypeValue = "";
            StmtDate = "";
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
                    Key = "PortfolioId";
                    if (!PortfolioIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StatementRunId";
                    if (!StatementRunIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EventRunId";
                    if (!EventRunIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DepositReleaseType";
                    if (!DepositReleaseTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StmtDate";
                    if (string.IsNullOrEmpty(StmtDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SetupPageEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.StatementRunIdValue = Source.StatementRunIdValue;
                this.EventRunIdValue = Source.EventRunIdValue;
                this.DepositReleaseTypeValue = Source.DepositReleaseTypeValue;
                this.StmtDate = Source.StmtDate;
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
            
                    SetupPageEntriesRequestItem TempItem = new SetupPageEntriesRequestItem();
                    //Result = CommonMethods.LoadSetupPageEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSetupPageEntriesRequestItem(ActionBase, ref TempItem);
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
