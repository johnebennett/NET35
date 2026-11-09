/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 21-12-2020
 * Time: 3:13 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/SplitsDlg/Actions/Templates/UpdateMultiNameRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.SplitsDlg
{
    public class UpdateMultiNameRequestItem
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
        
        public string MultiName1 { get; set; }
        
        public string MultiName2 { get; set; }
        
        public string MultiName3 { get; set; }
        
        public string MultiName4 { get; set; }
        
        public string MultiName5 { get; set; }
        
        public string MultiName6 { get; set; }
        
        public string MultiName7 { get; set; }
        
        public string MultiName8 { get; set; }
        
        public string MultiName9 { get; set; }
        
        public string MultiName10 { get; set; }
        
        public string MultiName11 { get; set; }
        
        public string MultiName12 { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            MultiStrataNumberValue = "";
            IncludedListValue = "";
            MultiName1 = "";
            MultiName2 = "";
            MultiName3 = "";
            MultiName4 = "";
            MultiName5 = "";
            MultiName6 = "";
            MultiName7 = "";
            MultiName8 = "";
            MultiName9 = "";
            MultiName10 = "";
            MultiName11 = "";
            MultiName12 = "";
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
                    Key = "MultiName1";
                    if (string.IsNullOrEmpty(MultiName1)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName2";
                    if (string.IsNullOrEmpty(MultiName2)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName3";
                    if (string.IsNullOrEmpty(MultiName3)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName4";
                    if (string.IsNullOrEmpty(MultiName4)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName5";
                    if (string.IsNullOrEmpty(MultiName5)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName6";
                    if (string.IsNullOrEmpty(MultiName6)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName7";
                    if (string.IsNullOrEmpty(MultiName7)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName8";
                    if (string.IsNullOrEmpty(MultiName8)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName9";
                    if (string.IsNullOrEmpty(MultiName9)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName10";
                    if (string.IsNullOrEmpty(MultiName10)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName11";
                    if (string.IsNullOrEmpty(MultiName11)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MultiName12";
                    if (string.IsNullOrEmpty(MultiName12)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UpdateMultiNameRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.MultiStrataNumberValue = Source.MultiStrataNumberValue;
                this.IncludedListValue = Source.IncludedListValue;
                this.MultiName1 = Source.MultiName1;
                this.MultiName2 = Source.MultiName2;
                this.MultiName3 = Source.MultiName3;
                this.MultiName4 = Source.MultiName4;
                this.MultiName5 = Source.MultiName5;
                this.MultiName6 = Source.MultiName6;
                this.MultiName7 = Source.MultiName7;
                this.MultiName8 = Source.MultiName8;
                this.MultiName9 = Source.MultiName9;
                this.MultiName10 = Source.MultiName10;
                this.MultiName11 = Source.MultiName11;
                this.MultiName12 = Source.MultiName12;
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
            
                    UpdateMultiNameRequestItem TempItem = new UpdateMultiNameRequestItem();
                    //Result = CommonMethods.LoadUpdateMultiNameRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadUpdateMultiNameRequestItem(ActionBase, ref TempItem);
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
