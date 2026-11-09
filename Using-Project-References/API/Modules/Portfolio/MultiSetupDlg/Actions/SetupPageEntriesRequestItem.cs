/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 25-07-2020
 * Time: 12:47 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\MultiSetupDlg\Actions\Templates\SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.MultiSetupDlg
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
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            MultiOCListIdValue = "";
            LotOwnerListIdValue = "";
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
                this.MultiOCListIdValue = Source.MultiOCListIdValue;
                this.LotOwnerListIdValue = Source.LotOwnerListIdValue;
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
