/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 09-08-2020
 * Time: 3:02 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\ToDo\Task\Actions\Templates\UpdateNoteRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.ToDo.Tasks
{
    public class UpdateNoteRequestItem
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
        
        public string NoteText { get; set; }
        
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
            NoteText = "";
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
                    Key = "NoteText";
                    if (string.IsNullOrEmpty(NoteText)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UpdateNoteRequestItem Source)
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
                this.NoteText = Source.NoteText;
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
            
                    UpdateNoteRequestItem TempItem = new UpdateNoteRequestItem();
                    //Result = CommonMethods.LoadUpdateNoteRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadUpdateNoteRequestItem(ActionBase, ref TempItem);
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
