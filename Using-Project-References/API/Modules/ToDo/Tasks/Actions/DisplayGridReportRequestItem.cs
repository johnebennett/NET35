/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 29-08-2020
 * Time: 6:23 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/ToDo/Tasks/Actions/Templates/DisplayGridReportRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.ToDo.Tasks
{
    public class DisplayGridReportRequestItem
    {
        #region Properties - Getters and Setters
        private string FBranchIdValue = "";
        public bool BranchIdIsValid = false;
        public int BranchId { get; set; }
        public string BranchIdValue
        {
            get { return FBranchIdValue; }
            set
            {
                FBranchIdValue = value;
                int Dummy = 0;
                BranchIdIsValid = int.TryParse(FBranchIdValue, out Dummy);
                BranchId = (BranchIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FManagerIdValue = "";
        public bool ManagerIdIsValid = false;
        public int ManagerId { get; set; }
        public string ManagerIdValue
        {
            get { return FManagerIdValue; }
            set
            {
                FManagerIdValue = value;
                int Dummy = 0;
                ManagerIdIsValid = int.TryParse(FManagerIdValue, out Dummy);
                ManagerId = (ManagerIdIsValid) ? Dummy : 0;
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
        
        private string FDateTypeValue = "";
        public bool DateTypeIsValid = false;
        public int DateType { get; set; }
        public string DateTypeValue
        {
            get { return FDateTypeValue; }
            set
            {
                FDateTypeValue = value;
                int Dummy = 0;
                DateTypeIsValid = int.TryParse(FDateTypeValue, out Dummy);
                DateType = (DateTypeIsValid) ? Dummy : 0;
            }
        }
        
        public string DateFrom { get; set; }
        
        public string DateTo { get; set; }
        
        private string FOrderByValue = "";
        public bool OrderByIsValid = false;
        public int OrderBy { get; set; }
        public string OrderByValue
        {
            get { return FOrderByValue; }
            set
            {
                FOrderByValue = value;
                int Dummy = 0;
                OrderByIsValid = int.TryParse(FOrderByValue, out Dummy);
                OrderBy = (OrderByIsValid) ? Dummy : 0;
            }
        }
        
        private string FCompletedTypeValue = "";
        public bool CompletedTypeIsValid = false;
        public int CompletedType { get; set; }
        public string CompletedTypeValue
        {
            get { return FCompletedTypeValue; }
            set
            {
                FCompletedTypeValue = value;
                int Dummy = 0;
                CompletedTypeIsValid = int.TryParse(FCompletedTypeValue, out Dummy);
                CompletedType = (CompletedTypeIsValid) ? Dummy : 0;
            }
        }
        
        private string FAssignedTo1Value = "";
        public bool AssignedTo1IsValid = false;
        public int AssignedTo1 { get; set; }
        public string AssignedTo1Value
        {
            get { return FAssignedTo1Value; }
            set
            {
                FAssignedTo1Value = value;
                int Dummy = 0;
                AssignedTo1IsValid = int.TryParse(FAssignedTo1Value, out Dummy);
                AssignedTo1 = (AssignedTo1IsValid) ? Dummy : 0;
            }
        }
        
        private string FAssignedTo2Value = "";
        public bool AssignedTo2IsValid = false;
        public int AssignedTo2 { get; set; }
        public string AssignedTo2Value
        {
            get { return FAssignedTo2Value; }
            set
            {
                FAssignedTo2Value = value;
                int Dummy = 0;
                AssignedTo2IsValid = int.TryParse(FAssignedTo2Value, out Dummy);
                AssignedTo2 = (AssignedTo2IsValid) ? Dummy : 0;
            }
        }
        
        private string FTradespersonIdValue = "";
        public bool TradespersonIdIsValid = false;
        public int TradespersonId { get; set; }
        public string TradespersonIdValue
        {
            get { return FTradespersonIdValue; }
            set
            {
                FTradespersonIdValue = value;
                int Dummy = 0;
                TradespersonIdIsValid = int.TryParse(FTradespersonIdValue, out Dummy);
                TradespersonId = (TradespersonIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FAssignedTo3Value = "";
        public bool AssignedTo3IsValid = false;
        public int AssignedTo3 { get; set; }
        public string AssignedTo3Value
        {
            get { return FAssignedTo3Value; }
            set
            {
                FAssignedTo3Value = value;
                int Dummy = 0;
                AssignedTo3IsValid = int.TryParse(FAssignedTo3Value, out Dummy);
                AssignedTo3 = (AssignedTo3IsValid) ? Dummy : 0;
            }
        }
        
        private string FOptionsValue = "";
        public bool OptionsIsValid = false;
        public int Options { get; set; }
        public string OptionsValue
        {
            get { return FOptionsValue; }
            set
            {
                FOptionsValue = value;
                int Dummy = 0;
                OptionsIsValid = int.TryParse(FOptionsValue, out Dummy);
                Options = (OptionsIsValid) ? Dummy : 0;
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
            BranchIdValue = "";
            ManagerIdValue = "";
            TransTypeIdValue = "";
            TaskTypeIdValue = "";
            TaskStatusIdValue = "";
            DateTypeValue = "";
            DateFrom = "";
            DateTo = "";
            OrderByValue = "";
            CompletedTypeValue = "";
            AssignedTo1Value = "";
            AssignedTo2Value = "";
            TradespersonIdValue = "";
            UniqueIdValue = "";
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
                    Key = "BranchId";
                    if (!BranchIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ManagerId";
                    if (!ManagerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransTypeId";
                    if (!TransTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TaskTypeId";
                    if (!TaskTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TaskStatusId";
                    if (!TaskStatusIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DateType";
                    if (!DateTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DateFrom";
                    if (string.IsNullOrEmpty(DateFrom)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DateTo";
                    if (string.IsNullOrEmpty(DateTo)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OrderBy";
                    if (!OrderByIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CompletedType";
                    if (!CompletedTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AssignedTo1";
                    if (!AssignedTo1IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AssignedTo2";
                    if (!AssignedTo2IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TradespersonId";
                    if (!TradespersonIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "UniqueId";
                    if (!UniqueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(DisplayGridReportRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.BranchIdValue = Source.BranchIdValue;
                this.ManagerIdValue = Source.ManagerIdValue;
                this.TransTypeIdValue = Source.TransTypeIdValue;
                this.TaskTypeIdValue = Source.TaskTypeIdValue;
                this.TaskStatusIdValue = Source.TaskStatusIdValue;
                this.DateTypeValue = Source.DateTypeValue;
                this.DateFrom = Source.DateFrom;
                this.DateTo = Source.DateTo;
                this.OrderByValue = Source.OrderByValue;
                this.CompletedTypeValue = Source.CompletedTypeValue;
                this.AssignedTo1Value = Source.AssignedTo1Value;
                this.AssignedTo2Value = Source.AssignedTo2Value;
                this.TradespersonIdValue = Source.TradespersonIdValue;
                this.AssignedTo3Value = Source.AssignedTo3Value;
                this.OptionsValue = Source.OptionsValue;
                this.UniqueIdValue = Source.UniqueIdValue;
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
            
                    DisplayGridReportRequestItem TempItem = new DisplayGridReportRequestItem();
                    //Result = CommonMethods.LoadDisplayGridReportRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadDisplayGridReportRequestItem(ActionBase, ref TempItem);
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
