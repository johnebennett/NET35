/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 10-08-2021
 * Time: 7:08 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/Actions/Templates/KeyNumber/SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.KeyNumber
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
        
        private string FKeyNumberValue = "";
        public bool KeyNumberIsValid = false;
        public int KeyNumber { get; set; }
        public string KeyNumberValue
        {
            get { return FKeyNumberValue; }
            set
            {
                FKeyNumberValue = value;
                int Dummy = 0;
                KeyNumberIsValid = int.TryParse(FKeyNumberValue, out Dummy);
                KeyNumber = (KeyNumberIsValid) ? Dummy : 0;
            }
        }
        
        private string FKeyNumber2Value = "";
        public bool KeyNumber2IsValid = false;
        public int KeyNumber2 { get; set; }
        public string KeyNumber2Value
        {
            get { return FKeyNumber2Value; }
            set
            {
                FKeyNumber2Value = value;
                int Dummy = 0;
                KeyNumber2IsValid = int.TryParse(FKeyNumber2Value, out Dummy);
                KeyNumber2 = (KeyNumber2IsValid) ? Dummy : 0;
            }
        }
        
        private string FKeyNumber3Value = "";
        public bool KeyNumber3IsValid = false;
        public int KeyNumber3 { get; set; }
        public string KeyNumber3Value
        {
            get { return FKeyNumber3Value; }
            set
            {
                FKeyNumber3Value = value;
                int Dummy = 0;
                KeyNumber3IsValid = int.TryParse(FKeyNumber3Value, out Dummy);
                KeyNumber3 = (KeyNumber3IsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            KeyNumberValue = "";
            KeyNumber2Value = "";
            KeyNumber3Value = "";
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
                    Key = "KeyNumber";
                    if (!KeyNumberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "KeyNumber2";
                    if (!KeyNumber2IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "KeyNumber3";
                    if (!KeyNumber3IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.KeyNumberValue = Source.KeyNumberValue;
                this.KeyNumber2Value = Source.KeyNumber2Value;
                this.KeyNumber3Value = Source.KeyNumber3Value;
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
