/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 17-03-2020
 * Time: 11:15 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\Actions\Templates\CommBondsApplyChangesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio
{
    public class CommBondsApplyChangesRequestItem
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
        
        public string Details { get; set; }
        
        public string LodgedAmount { get; set; }
        
        public string LodgedOn { get; set; }
        
        public string ExpiryDate { get; set; }
        
        public string DoesNotExpire { get; set; }
        
        private string FBondHeldValue = "";
        public bool BondHeldIsValid = false;
        public int BondHeld { get; set; }
        public string BondHeldValue
        {
            get { return FBondHeldValue; }
            set
            {
                FBondHeldValue = value;
                int Dummy = 0;
                BondHeldIsValid = int.TryParse(FBondHeldValue, out Dummy);
                BondHeld = (BondHeldIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            SubLedgerIdValue = "";
            LodgedOn = "";
            ExpiryDate = "";
            DoesNotExpire = "";
            BondHeldValue = "";
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
                    Key = "LodgedOn";
                    if (string.IsNullOrEmpty(LodgedOn)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ExpiryDate";
                    if (string.IsNullOrEmpty(ExpiryDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DoesNotExpire";
                    if (string.IsNullOrEmpty(DoesNotExpire)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BondHeld";
                    if (!BondHeldIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(CommBondsApplyChangesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.SubLedgerIdValue = Source.SubLedgerIdValue;
                this.Details = Source.Details;
                this.LodgedAmount = Source.LodgedAmount;
                this.LodgedOn = Source.LodgedOn;
                this.ExpiryDate = Source.ExpiryDate;
                this.DoesNotExpire = Source.DoesNotExpire;
                this.BondHeldValue = Source.BondHeldValue;
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
            
                    CommBondsApplyChangesRequestItem TempItem = new CommBondsApplyChangesRequestItem();
                    Result = CommonMethods.LoadCommBondsApplyChangesRequestItem(ActionBase, ref TempItem);
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
