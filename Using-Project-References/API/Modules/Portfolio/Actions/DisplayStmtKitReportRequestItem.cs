/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 19-02-2020
 * Time: 1:40 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\Actions\Templates\DisplayStmtKitReportRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio
{
    public class DisplayStmtKitReportRequestItem
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
        
        private string FNewIdValue = "";
        public bool NewIdIsValid = false;
        public int NewId { get; set; }
        public string NewIdValue
        {
            get { return FNewIdValue; }
            set
            {
                FNewIdValue = value;
                int Dummy = 0;
                NewIdIsValid = int.TryParse(FNewIdValue, out Dummy);
                NewId = (NewIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FPassedIdValue = "";
        public bool PassedIdIsValid = false;
        public int PassedId { get; set; }
        public string PassedIdValue
        {
            get { return FPassedIdValue; }
            set
            {
                FPassedIdValue = value;
                int Dummy = 0;
                PassedIdIsValid = int.TryParse(FPassedIdValue, out Dummy);
                PassedId = (PassedIdIsValid) ? Dummy : 0;
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
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            AccountIdValue = "";
            CategoryOnlyIdValue = "";
            NewIdValue = "";
            PassedIdValue = "";
            ButtonIdValue = "";
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
                    Key = "CategoryOnlyId";
                    if (!CategoryOnlyIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "NewId";
                    if (!NewIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PassedId";
                    if (!PassedIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ButtonId";
                    if (!ButtonIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(DisplayStmtKitReportRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.AccountIdValue = Source.AccountIdValue;
                this.CategoryOnlyIdValue = Source.CategoryOnlyIdValue;
                this.NewIdValue = Source.NewIdValue;
                this.PassedIdValue = Source.PassedIdValue;
                this.ButtonIdValue = Source.ButtonIdValue;
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
            
                    DisplayStmtKitReportRequestItem TempItem = new DisplayStmtKitReportRequestItem();
                    Result = CommonMethods.LoadDisplayStmtKitReportRequestItem(ActionBase, ref TempItem);
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
