/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 08-06-2020
 * Time: 3:36 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\Actions\Templates\ResBondsApplyChangesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio
{
    public class ResBondsApplyChangesRequestItem
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
        
        private string FCategoryOnlyIdValue = "";
        public bool CategoryOnlyIdIsValid = false;
        public int CategoryOnlyId { get; set; }
        public string CategoryOnlyIdValue
        {
            get { return FCategoryOnlyIdValue; }
            set
            {
                FCategoryOnlyIdValue = value;
                int Dummy = 0;
                CategoryOnlyIdIsValid = int.TryParse(FCategoryOnlyIdValue, out Dummy);
                CategoryOnlyId = (CategoryOnlyIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FLodgedAmountValue = "";
        public bool LodgedAmountIsValid = false;
        public double LodgedAmount { get; set; }
        public string LodgedAmountValue
        {
            get { return FLodgedAmountValue; }
            set
            {
                FLodgedAmountValue = value;
                double Dummy = 0.0;
                LodgedAmountIsValid = Double.TryParse(FLodgedAmountValue, out Dummy);
                LodgedAmount = (LodgedAmountIsValid) ? Dummy : 0;
            }
        }
        
        public string LodgementNumber { get; set; }
        
        public string LodgedOn { get; set; }
        
        public string BondNumber { get; set; }
        
        public string Guarantee { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            CategoryOnlyIdValue = "";
            LodgedAmountValue = "";
            LodgedOn = "";
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
                    Key = "CategoryOnlyId";
                    if (!CategoryOnlyIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LodgedAmount";
                    if (!LodgedAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LodgedOn";
                    if (string.IsNullOrEmpty(LodgedOn)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(ResBondsApplyChangesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.CategoryOnlyIdValue = Source.CategoryOnlyIdValue;
                this.LodgedAmountValue = Source.LodgedAmountValue;
                this.LodgementNumber = Source.LodgementNumber;
                this.LodgedOn = Source.LodgedOn;
                this.BondNumber = Source.BondNumber;
                this.Guarantee = Source.Guarantee;
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
            
                    ResBondsApplyChangesRequestItem TempItem = new ResBondsApplyChangesRequestItem();
                    Result = CommonMethods.LoadResBondsApplyChangesRequestItem(ActionBase, ref TempItem);
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
