/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 07-08-2020
 * Time: 5:06 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\StrataRolloverDlg\Actions\Templates\SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.StrataRolloverDlg
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
        
        private string FSubLedgerIdValue = "";
        public bool SubLedgerIdIsValid = false;
        public int SubLedgerId { get; set; }
        public string SubLedgerIdValue
        {
            get { return FSubLedgerIdValue; }
            set
            {
                FSubLedgerIdValue = value;
                int Dummy = 0;
                SubLedgerIdIsValid = int.TryParse(FSubLedgerIdValue, out Dummy);
                SubLedgerId = (SubLedgerIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FEOYIdValue = "";
        public bool EOYIdIsValid = false;
        public int EOYId { get; set; }
        public string EOYIdValue
        {
            get { return FEOYIdValue; }
            set
            {
                FEOYIdValue = value;
                int Dummy = 0;
                EOYIdIsValid = int.TryParse(FEOYIdValue, out Dummy);
                EOYId = (EOYIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            SubLedgerIdValue = "";
            EOYIdValue = "";
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
                    Key = "SubLedgerId";
                    if (!SubLedgerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EOYId";
                    if (!EOYIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.SubLedgerIdValue = Source.SubLedgerIdValue;
                this.EOYIdValue = Source.EOYIdValue;
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
