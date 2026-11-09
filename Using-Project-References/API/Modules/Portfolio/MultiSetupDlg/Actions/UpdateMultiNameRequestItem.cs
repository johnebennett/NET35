/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 23-12-2020
 * Time: 9:28 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/MultiSetupDlg/Actions/Templates/UpdateMultiNameRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.MultiSetupDlg
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
        
        private string FMultiOCListIdValue = "";
        public bool MultiOCListIdIsValid = false;
        public int MultiOCListId { get; set; }
        public string MultiOCListIdValue
        {
            get { return FMultiOCListIdValue; }
            set
            {
                FMultiOCListIdValue = value;
                int Dummy = 0;
                MultiOCListIdIsValid = int.TryParse(FMultiOCListIdValue, out Dummy);
                MultiOCListId = (MultiOCListIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FLotOwnerListIdValue = "";
        public bool LotOwnerListIdIsValid = false;
        public int LotOwnerListId { get; set; }
        public string LotOwnerListIdValue
        {
            get { return FLotOwnerListIdValue; }
            set
            {
                FLotOwnerListIdValue = value;
                int Dummy = 0;
                LotOwnerListIdIsValid = int.TryParse(FLotOwnerListIdValue, out Dummy);
                LotOwnerListId = (LotOwnerListIdIsValid) ? Dummy : 0;
            }
        }
        
        public string StrataName1 { get; set; }
        
        public string StrataName2 { get; set; }
        
        public string StrataName3 { get; set; }
        
        public string StrataName4 { get; set; }
        
        public string StrataName5 { get; set; }
        
        public string StrataName6 { get; set; }
        
        public string StrataName7 { get; set; }
        
        public string StrataName8 { get; set; }
        
        public string StrataName9 { get; set; }
        
        public string StrataName10 { get; set; }
        
        public string StrataName11 { get; set; }
        
        public string StrataName12 { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            MultiOCListIdValue = "";
            LotOwnerListIdValue = "";
            StrataName1 = "";
            StrataName2 = "";
            StrataName3 = "";
            StrataName4 = "";
            StrataName5 = "";
            StrataName6 = "";
            StrataName7 = "";
            StrataName8 = "";
            StrataName9 = "";
            StrataName10 = "";
            StrataName11 = "";
            StrataName12 = "";
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
                    Key = "MultiOCListId";
                    if (!MultiOCListIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LotOwnerListId";
                    if (!LotOwnerListIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName1";
                    if (string.IsNullOrEmpty(StrataName1)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName2";
                    if (string.IsNullOrEmpty(StrataName2)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName3";
                    if (string.IsNullOrEmpty(StrataName3)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName4";
                    if (string.IsNullOrEmpty(StrataName4)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName5";
                    if (string.IsNullOrEmpty(StrataName5)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName6";
                    if (string.IsNullOrEmpty(StrataName6)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName7";
                    if (string.IsNullOrEmpty(StrataName7)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName8";
                    if (string.IsNullOrEmpty(StrataName8)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName9";
                    if (string.IsNullOrEmpty(StrataName9)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName10";
                    if (string.IsNullOrEmpty(StrataName10)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName11";
                    if (string.IsNullOrEmpty(StrataName11)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StrataName12";
                    if (string.IsNullOrEmpty(StrataName12)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.MultiOCListIdValue = Source.MultiOCListIdValue;
                this.LotOwnerListIdValue = Source.LotOwnerListIdValue;
                this.StrataName1 = Source.StrataName1;
                this.StrataName2 = Source.StrataName2;
                this.StrataName3 = Source.StrataName3;
                this.StrataName4 = Source.StrataName4;
                this.StrataName5 = Source.StrataName5;
                this.StrataName6 = Source.StrataName6;
                this.StrataName7 = Source.StrataName7;
                this.StrataName8 = Source.StrataName8;
                this.StrataName9 = Source.StrataName9;
                this.StrataName10 = Source.StrataName10;
                this.StrataName11 = Source.StrataName11;
                this.StrataName12 = Source.StrataName12;
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
