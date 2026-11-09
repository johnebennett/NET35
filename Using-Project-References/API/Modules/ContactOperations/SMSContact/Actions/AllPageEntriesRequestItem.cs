/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 24-08-2020
 * Time: 8:43 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/ContactOperations/SMSContact/Actions/Templates/AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.ContactOperations.SMSContact
{
    public class AllPageEntriesRequestItem
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
        
        private string FTaskIdValue = "";
        public bool TaskIdIsValid = false;
        public int TaskId { get; set; }
        public string TaskIdValue
        {
            get { return FTaskIdValue; }
            set
            {
                FTaskIdValue = value;
                int Dummy = 0;
                TaskIdIsValid = int.TryParse(FTaskIdValue, out Dummy);
                TaskId = (TaskIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FTransTypeIdValue = "";
        public bool TransTypeIdIsValid = false;
        public int TransTypeId { get; set; }
        public string TransTypeIdValue
        {
            get { return FTransTypeIdValue; }
            set
            {
                FTransTypeIdValue = value;
                int Dummy = 0;
                TransTypeIdIsValid = int.TryParse(FTransTypeIdValue, out Dummy);
                TransTypeId = (TransTypeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FFilterEventIdValue = "";
        public bool FilterEventIdIsValid = false;
        public int FilterEventId { get; set; }
        public string FilterEventIdValue
        {
            get { return FFilterEventIdValue; }
            set
            {
                FFilterEventIdValue = value;
                int Dummy = 0;
                FilterEventIdIsValid = int.TryParse(FFilterEventIdValue, out Dummy);
                FilterEventId = (FilterEventIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FTaskTypeIdValue = "";
        public bool TaskTypeIdIsValid = false;
        public int TaskTypeId { get; set; }
        public string TaskTypeIdValue
        {
            get { return FTaskTypeIdValue; }
            set
            {
                FTaskTypeIdValue = value;
                int Dummy = 0;
                TaskTypeIdIsValid = int.TryParse(FTaskTypeIdValue, out Dummy);
                TaskTypeId = (TaskTypeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FWhoTo1Value = "";
        public bool WhoTo1IsValid = false;
        public int WhoTo1 { get; set; }
        public string WhoTo1Value
        {
            get { return FWhoTo1Value; }
            set
            {
                FWhoTo1Value = value;
                int Dummy = 0;
                WhoTo1IsValid = int.TryParse(FWhoTo1Value, out Dummy);
                WhoTo1 = (WhoTo1IsValid) ? Dummy : 0;
            }
        }
        
        private string FWhoTo2Value = "";
        public bool WhoTo2IsValid = false;
        public int WhoTo2 { get; set; }
        public string WhoTo2Value
        {
            get { return FWhoTo2Value; }
            set
            {
                FWhoTo2Value = value;
                int Dummy = 0;
                WhoTo2IsValid = int.TryParse(FWhoTo2Value, out Dummy);
                WhoTo2 = (WhoTo2IsValid) ? Dummy : 0;
            }
        }
        
        private string FTemplateIdValue = "";
        public bool TemplateIdIsValid = false;
        public int TemplateId { get; set; }
        public string TemplateIdValue
        {
            get { return FTemplateIdValue; }
            set
            {
                FTemplateIdValue = value;
                int Dummy = 0;
                TemplateIdIsValid = int.TryParse(FTemplateIdValue, out Dummy);
                TemplateId = (TemplateIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FTaskStatusIdValue = "";
        public bool TaskStatusIdIsValid = false;
        public int TaskStatusId { get; set; }
        public string TaskStatusIdValue
        {
            get { return FTaskStatusIdValue; }
            set
            {
                FTaskStatusIdValue = value;
                int Dummy = 0;
                TaskStatusIdIsValid = int.TryParse(FTaskStatusIdValue, out Dummy);
                TaskStatusId = (TaskStatusIdIsValid) ? Dummy : 0;
            }
        }
        
        public string SMSBody { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            MasterEventIdValue = "";
            PortfolioIdValue = "";
            TaskIdValue = "";
            TransTypeIdValue = "";
            FilterEventIdValue = "";
            TaskTypeIdValue = "";
            WhoTo1Value = "";
            WhoTo2Value = "";
            TemplateIdValue = "";
            TaskStatusIdValue = "";
            SMSBody = "";
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
                    Key = "PortfolioId";
                    if (!PortfolioIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TaskId";
                    if (!TaskIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransTypeId";
                    if (!TransTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FilterEventId";
                    if (!FilterEventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TaskTypeId";
                    if (!TaskTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "WhoTo1";
                    if (!WhoTo1IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "WhoTo2";
                    if (!WhoTo2IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TemplateId";
                    if (!TemplateIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TaskStatusId";
                    if (!TaskStatusIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SMSBody";
                    if (string.IsNullOrEmpty(SMSBody)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(AllPageEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.MasterEventIdValue = Source.MasterEventIdValue;
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.TaskIdValue = Source.TaskIdValue;
                this.TransTypeIdValue = Source.TransTypeIdValue;
                this.FilterEventIdValue = Source.FilterEventIdValue;
                this.TaskTypeIdValue = Source.TaskTypeIdValue;
                this.WhoTo1Value = Source.WhoTo1Value;
                this.WhoTo2Value = Source.WhoTo2Value;
                this.TemplateIdValue = Source.TemplateIdValue;
                this.TaskStatusIdValue = Source.TaskStatusIdValue;
                this.SMSBody = Source.SMSBody;
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
            
                    AllPageEntriesRequestItem TempItem = new AllPageEntriesRequestItem();
                    //Result = CommonMethods.LoadAllPageEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadAllPageEntriesRequestItem(ActionBase, ref TempItem);
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
