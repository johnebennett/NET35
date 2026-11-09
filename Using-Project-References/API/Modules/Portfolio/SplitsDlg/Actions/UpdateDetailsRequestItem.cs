/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 21-12-2020
 * Time: 5:23 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/SplitsDlg/Actions/Templates/UpdateDetailsRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.SplitsDlg
{
    public class UpdateDetailsRequestItem
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
        
        private string FMultiStrataNumberValue = "";
        public bool MultiStrataNumberIsValid = false;
        public int MultiStrataNumber { get; set; }
        public string MultiStrataNumberValue
        {
            get { return FMultiStrataNumberValue; }
            set
            {
                FMultiStrataNumberValue = value;
                int Dummy = 0;
                MultiStrataNumberIsValid = int.TryParse(FMultiStrataNumberValue, out Dummy);
                MultiStrataNumber = (MultiStrataNumberIsValid) ? Dummy : 0;
            }
        }
        
        private string FIncludedListValue = "";
        public bool IncludedListIsValid = false;
        public int IncludedList { get; set; }
        public string IncludedListValue
        {
            get { return FIncludedListValue; }
            set
            {
                FIncludedListValue = value;
                int Dummy = 0;
                IncludedListIsValid = int.TryParse(FIncludedListValue, out Dummy);
                IncludedList = (IncludedListIsValid) ? Dummy : 0;
            }
        }
        
        private string FPercentageValue = "";
        public bool PercentageIsValid = false;
        public double Percentage { get; set; }
        public string PercentageValue
        {
            get { return FPercentageValue; }
            set
            {
                FPercentageValue = value;
                double Dummy = 0.0;
                PercentageIsValid = Double.TryParse(FPercentageValue, out Dummy);
                Percentage = (PercentageIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            MultiStrataNumberValue = "";
            IncludedListValue = "";
            PercentageValue = "";
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
                    Key = "MultiStrataNumber";
                    if (!MultiStrataNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "IncludedList";
                    if (!IncludedListIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Percentage";
                    if (!PercentageIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UpdateDetailsRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.MultiStrataNumberValue = Source.MultiStrataNumberValue;
                this.IncludedListValue = Source.IncludedListValue;
                this.PercentageValue = Source.PercentageValue;
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
            
                    UpdateDetailsRequestItem TempItem = new UpdateDetailsRequestItem();
                    //Result = CommonMethods.LoadUpdateDetailsRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadUpdateDetailsRequestItem(ActionBase, ref TempItem);
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
