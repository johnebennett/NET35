/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 12-09-2020
 * Time: 5:38 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Receipts/Actions/Templates/DeleteGridEntryRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Receipts
{
    public class DeleteGridEntryRequestItem
    {
        #region Properties - Getters and Setters
        private string FMasterEventIdValue = "";
        public bool MasterEventIdIsValid = false;
        public int MasterEventId { get; set; }
        public string MasterEventIdValue
        {
            get { return FMasterEventIdValue; }
            set
            {
                FMasterEventIdValue = value;
                int Dummy = 0;
                MasterEventIdIsValid = int.TryParse(FMasterEventIdValue, out Dummy);
                MasterEventId = (MasterEventIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FReceiptOptionsIdValue = "";
        public bool ReceiptOptionsIdIsValid = false;
        public int ReceiptOptionsId { get; set; }
        public string ReceiptOptionsIdValue
        {
            get { return FReceiptOptionsIdValue; }
            set
            {
                FReceiptOptionsIdValue = value;
                int Dummy = 0;
                ReceiptOptionsIdIsValid = int.TryParse(FReceiptOptionsIdValue, out Dummy);
                ReceiptOptionsId = (ReceiptOptionsIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FPastEventIdValue = "";
        public bool PastEventIdIsValid = false;
        public int PastEventId { get; set; }
        public string PastEventIdValue
        {
            get { return FPastEventIdValue; }
            set
            {
                FPastEventIdValue = value;
                int Dummy = 0;
                PastEventIdIsValid = int.TryParse(FPastEventIdValue, out Dummy);
                PastEventId = (PastEventIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FUniqueIdValue = "";
        public bool UniqueIdIsValid = false;
        public int UniqueId { get; set; }
        public string UniqueIdValue
        {
            get { return FUniqueIdValue; }
            set
            {
                FUniqueIdValue = value;
                int Dummy = 0;
                UniqueIdIsValid = int.TryParse(FUniqueIdValue, out Dummy);
                UniqueId = (UniqueIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            MasterEventIdValue = "";
            ReceiptOptionsIdValue = "";
            PastEventIdValue = "";
            UniqueIdValue = "";
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
                    Key = "MasterEventId";
                    if (!MasterEventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReceiptOptionsId";
                    if (!ReceiptOptionsIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PastEventId";
                    if (!PastEventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "UniqueId";
                    if (!UniqueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(DeleteGridEntryRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.MasterEventIdValue = Source.MasterEventIdValue;
                this.ReceiptOptionsIdValue = Source.ReceiptOptionsIdValue;
                this.PastEventIdValue = Source.PastEventIdValue;
                this.UniqueIdValue = Source.UniqueIdValue;
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
            
                    DeleteGridEntryRequestItem TempItem = new DeleteGridEntryRequestItem();
                    //Result = CommonMethods.LoadDeleteGridEntryRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadDeleteGridEntryRequestItem(ActionBase, ref TempItem);
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
