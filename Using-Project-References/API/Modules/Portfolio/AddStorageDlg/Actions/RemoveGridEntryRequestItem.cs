/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 10-12-2020
 * Time: 8:23 PM

 * SourceFile: /home/john/TestSource/EAOffice/EAOfficeAPI/Modules/Portfolio/AddStorageDlg/Actions/Templates/RemoveGridEntryRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.AddStorageDlg
{
    public class RemoveGridEntryRequestItem
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
        
        private string FStorageTypeValue = "";
        public bool StorageTypeIsValid = false;
        public int StorageType { get; set; }
        public string StorageTypeValue
        {
            get { return FStorageTypeValue; }
            set
            {
                FStorageTypeValue = value;
                int Dummy = 0;
                StorageTypeIsValid = int.TryParse(FStorageTypeValue, out Dummy);
                StorageType = (StorageTypeIsValid) ? Dummy : 0;
            }
        }
        
        public string FileName { get; set; }
        
        private string FOption1Value = "";
        public bool Option1IsValid = false;
        public int Option1 { get; set; }
        public string Option1Value
        {
            get { return FOption1Value; }
            set
            {
                FOption1Value = value;
                int Dummy = 0;
                Option1IsValid = int.TryParse(FOption1Value, out Dummy);
                Option1 = (Option1IsValid) ? Dummy : 0;
            }
        }
        
        private string FGridIdValue = "";
        public bool GridIdIsValid = false;
        public int GridId { get; set; }
        public string GridIdValue
        {
            get { return FGridIdValue; }
            set
            {
                FGridIdValue = value;
                int Dummy = 0;
                GridIdIsValid = int.TryParse(FGridIdValue, out Dummy);
                GridId = (GridIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            StorageTypeValue = "";
            Option1Value = "";
            GridIdValue = "";
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
                    Key = "StorageType";
                    if (!StorageTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Option1";
                    if (!Option1IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GridId";
                    if (!GridIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(RemoveGridEntryRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.StorageTypeValue = Source.StorageTypeValue;
                this.FileName = Source.FileName;
                this.Option1Value = Source.Option1Value;
                this.GridIdValue = Source.GridIdValue;
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
            
                    RemoveGridEntryRequestItem TempItem = new RemoveGridEntryRequestItem();
                    //Result = CommonMethods.LoadRemoveGridEntryRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadRemoveGridEntryRequestItem(ActionBase, ref TempItem);
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
