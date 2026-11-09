/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 30-07-2020
 * Time: 9:25 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\Property\Actions\Templates\UpdateNoteRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.Property
{
    public class UpdateNoteRequestItem
    {
        #region Properties - Getters and Setters
        public string IsAdd { get; set; }
        
        private string FMgmtIdValue = "";
        public bool MgmtIdIsValid = false;
        public int MgmtId { get; set; }
        public string MgmtIdValue
        {
            get { return FMgmtIdValue; }
            set
            {
                FMgmtIdValue = value;
                int Dummy = 0;
                MgmtIdIsValid = int.TryParse(FMgmtIdValue, out Dummy);
                MgmtId = (MgmtIdIsValid) ? Dummy : 0;
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
        
        public string NoteText { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            IsAdd = "";
            MgmtIdValue = "";
            NotesIdValue = "";
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
                    Key = "IsAdd";
                    if (string.IsNullOrEmpty(IsAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MgmtId";
                    if (!MgmtIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "NotesId";
                    if (!NotesIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.IsAdd = Source.IsAdd;
                this.MgmtIdValue = Source.MgmtIdValue;
                this.NotesIdValue = Source.NotesIdValue;
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
