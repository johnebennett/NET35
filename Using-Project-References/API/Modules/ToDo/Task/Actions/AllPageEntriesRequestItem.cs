/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 29-08-2020
 * Time: 11:41 AM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/ToDo/Task/Actions/Templates/AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.ToDo.Task
{
    public class AllPageEntriesRequestItem
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
        
        private string FTaskTypeValue = "";
        public bool TaskTypeIsValid = false;
        public int TaskType { get; set; }
        public string TaskTypeValue
        {
            get { return FTaskTypeValue; }
            set
            {
                FTaskTypeValue = value;
                int Dummy = 0;
                TaskTypeIsValid = int.TryParse(FTaskTypeValue, out Dummy);
                TaskType = (TaskTypeIsValid) ? Dummy : 0;
            }
        }
        
        private string FServiceIdValue = "";
        public bool ServiceIdIsValid = false;
        public int ServiceId { get; set; }
        public string ServiceIdValue
        {
            get { return FServiceIdValue; }
            set
            {
                FServiceIdValue = value;
                int Dummy = 0;
                ServiceIdIsValid = int.TryParse(FServiceIdValue, out Dummy);
                ServiceId = (ServiceIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FNotesIdValue = "";
        public bool NotesIdIsValid = false;
        public int NotesId { get; set; }
        public string NotesIdValue
        {
            get { return FNotesIdValue; }
            set
            {
                FNotesIdValue = value;
                int Dummy = 0;
                NotesIdIsValid = int.TryParse(FNotesIdValue, out Dummy);
                NotesId = (NotesIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOptionIdValue = "";
        public bool OptionIdIsValid = false;
        public int OptionId { get; set; }
        public string OptionIdValue
        {
            get { return FOptionIdValue; }
            set
            {
                FOptionIdValue = value;
                int Dummy = 0;
                OptionIdIsValid = int.TryParse(FOptionIdValue, out Dummy);
                OptionId = (OptionIdIsValid) ? Dummy : 0;
            }
        }
        
        public string Property { get; set; }
        
        public string Client { get; set; }
        
        public string Owner { get; set; }
        
        public string Date { get; set; }
        
        private string FAssignedToValue = "";
        public bool AssignedToIsValid = false;
        public int AssignedTo { get; set; }
        public string AssignedToValue
        {
            get { return FAssignedToValue; }
            set
            {
                FAssignedToValue = value;
                int Dummy = 0;
                AssignedToIsValid = int.TryParse(FAssignedToValue, out Dummy);
                AssignedTo = (AssignedToIsValid) ? Dummy : 0;
            }
        }
        
        public string Insurer { get; set; }
        
        public string Policy { get; set; }
        
        public string PolicyNumber { get; set; }
        
        public string RenewalDate { get; set; }
        
        private string FExcessValue = "";
        public bool ExcessIsValid = false;
        public double Excess { get; set; }
        public string ExcessValue
        {
            get { return FExcessValue; }
            set
            {
                FExcessValue = value;
                double Dummy = 0.0;
                ExcessIsValid = Double.TryParse(FExcessValue, out Dummy);
                Excess = (ExcessIsValid) ? Dummy : 0;
            }
        }
        
        private string FStatusValue = "";
        public bool StatusIsValid = false;
        public int Status { get; set; }
        public string StatusValue
        {
            get { return FStatusValue; }
            set
            {
                FStatusValue = value;
                int Dummy = 0;
                StatusIsValid = int.TryParse(FStatusValue, out Dummy);
                Status = (StatusIsValid) ? Dummy : 0;
            }
        }
        
        private string FPriorityValue = "";
        public bool PriorityIsValid = false;
        public int Priority { get; set; }
        public string PriorityValue
        {
            get { return FPriorityValue; }
            set
            {
                FPriorityValue = value;
                int Dummy = 0;
                PriorityIsValid = int.TryParse(FPriorityValue, out Dummy);
                Priority = (PriorityIsValid) ? Dummy : 0;
            }
        }
        
        public string Periodic { get; set; }
        
        private string FAdvanceValue = "";
        public bool AdvanceIsValid = false;
        public int Advance { get; set; }
        public string AdvanceValue
        {
            get { return FAdvanceValue; }
            set
            {
                FAdvanceValue = value;
                int Dummy = 0;
                AdvanceIsValid = int.TryParse(FAdvanceValue, out Dummy);
                Advance = (AdvanceIsValid) ? Dummy : 0;
            }
        }
        
        public string Subject { get; set; }
        
        public string Notes { get; set; }
        
        private string FTradespersonValue = "";
        public bool TradespersonIsValid = false;
        public int Tradesperson { get; set; }
        public string TradespersonValue
        {
            get { return FTradespersonValue; }
            set
            {
                FTradespersonValue = value;
                int Dummy = 0;
                TradespersonIsValid = int.TryParse(FTradespersonValue, out Dummy);
                Tradesperson = (TradespersonIsValid) ? Dummy : 0;
            }
        }
        
        private string FQuoting1Value = "";
        public bool Quoting1IsValid = false;
        public int Quoting1 { get; set; }
        public string Quoting1Value
        {
            get { return FQuoting1Value; }
            set
            {
                FQuoting1Value = value;
                int Dummy = 0;
                Quoting1IsValid = int.TryParse(FQuoting1Value, out Dummy);
                Quoting1 = (Quoting1IsValid) ? Dummy : 0;
            }
        }
        
        private string FQuoting2Value = "";
        public bool Quoting2IsValid = false;
        public int Quoting2 { get; set; }
        public string Quoting2Value
        {
            get { return FQuoting2Value; }
            set
            {
                FQuoting2Value = value;
                int Dummy = 0;
                Quoting2IsValid = int.TryParse(FQuoting2Value, out Dummy);
                Quoting2 = (Quoting2IsValid) ? Dummy : 0;
            }
        }
        
        private string FQuoting3Value = "";
        public bool Quoting3IsValid = false;
        public int Quoting3 { get; set; }
        public string Quoting3Value
        {
            get { return FQuoting3Value; }
            set
            {
                FQuoting3Value = value;
                int Dummy = 0;
                Quoting3IsValid = int.TryParse(FQuoting3Value, out Dummy);
                Quoting3 = (Quoting3IsValid) ? Dummy : 0;
            }
        }
        
        public string MaintenanceNotes { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            TaskIdValue = "";
            TaskTypeValue = "";
            ServiceIdValue = "";
            NotesIdValue = "";
            OptionIdValue = "";
            Property = "";
            Client = "";
            Owner = "";
            Date = "";
            AssignedToValue = "";
            ExcessValue = "";
            StatusValue = "";
            PriorityValue = "";
            Periodic = "";
            AdvanceValue = "";
            Subject = "";
            TradespersonValue = "";
            Quoting1Value = "";
            Quoting2Value = "";
            Quoting3Value = "";
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
                    Key = "TaskId";
                    if (!TaskIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TaskType";
                    if (!TaskTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ServiceId";
                    if (!ServiceIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "NotesId";
                    if (!NotesIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OptionId";
                    if (!OptionIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Property";
                    if (string.IsNullOrEmpty(Property)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Client";
                    if (string.IsNullOrEmpty(Client)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Owner";
                    if (string.IsNullOrEmpty(Owner)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Date";
                    if (string.IsNullOrEmpty(Date)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AssignedTo";
                    if (!AssignedToIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Status";
                    if (!StatusIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Priority";
                    if (!PriorityIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Periodic";
                    if (string.IsNullOrEmpty(Periodic)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Advance";
                    if (!AdvanceIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Subject";
                    if (string.IsNullOrEmpty(Subject)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Tradesperson";
                    if (!TradespersonIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Quoting1";
                    if (!Quoting1IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Quoting2";
                    if (!Quoting2IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Quoting3";
                    if (!Quoting3IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.TaskIdValue = Source.TaskIdValue;
                this.TaskTypeValue = Source.TaskTypeValue;
                this.ServiceIdValue = Source.ServiceIdValue;
                this.NotesIdValue = Source.NotesIdValue;
                this.OptionIdValue = Source.OptionIdValue;
                this.Property = Source.Property;
                this.Client = Source.Client;
                this.Owner = Source.Owner;
                this.Date = Source.Date;
                this.AssignedToValue = Source.AssignedToValue;
                this.Insurer = Source.Insurer;
                this.Policy = Source.Policy;
                this.PolicyNumber = Source.PolicyNumber;
                this.RenewalDate = Source.RenewalDate;
                this.ExcessValue = Source.ExcessValue;
                this.StatusValue = Source.StatusValue;
                this.PriorityValue = Source.PriorityValue;
                this.Periodic = Source.Periodic;
                this.AdvanceValue = Source.AdvanceValue;
                this.Subject = Source.Subject;
                this.Notes = Source.Notes;
                this.TradespersonValue = Source.TradespersonValue;
                this.Quoting1Value = Source.Quoting1Value;
                this.Quoting2Value = Source.Quoting2Value;
                this.Quoting3Value = Source.Quoting3Value;
                this.MaintenanceNotes = Source.MaintenanceNotes;
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
