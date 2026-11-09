/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 29-04-2020
 * Time: 11:25 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Receipts\Actions\Templates\EventProcessOptions_OnChangeRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Receipts.EventProcessOptions
{
    public class EventProcessOptions_OnChangeRequestItem
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
        
        private string FItemIdValue = "";
        public bool ItemIdIsValid = false;
        public int ItemId { get; set; }
        public string ItemIdValue
        {
            get { return FItemIdValue; }
            set
            {
                FItemIdValue = value;
                int Dummy = 0;
                ItemIdIsValid = int.TryParse(FItemIdValue, out Dummy);
                ItemId = (ItemIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            MasterEventIdValue = "";
            PastEventIdValue = "";
            ItemIdValue = "";
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
                    Key = "PastEventId";
                    if (!PastEventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ItemId";
                    if (!ItemIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(EventProcessOptions_OnChangeRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.MasterEventIdValue = Source.MasterEventIdValue;
                this.PastEventIdValue = Source.PastEventIdValue;
                this.ItemIdValue = Source.ItemIdValue;
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
            
                    EventProcessOptions_OnChangeRequestItem TempItem = new EventProcessOptions_OnChangeRequestItem();
                    //Result = CommonMethods.LoadEventProcessOptions_OnChangeRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadEventProcessOptions_OnChangeRequestItem(ActionBase, ref TempItem);
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
