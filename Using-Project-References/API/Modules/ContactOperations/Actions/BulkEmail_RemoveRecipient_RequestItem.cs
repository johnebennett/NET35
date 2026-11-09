/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 13-12-2019
 * Time: 11:41 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\ContactOperations\Actions\Templates\BulkEmail_RemoveRecipient_RequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations
{
    public class BulkEmail_RemoveRecipient_RequestItem
    {
        #region Properties - Getters and Setters
        private string FRecordIdValue = "";
        public bool RecordIdIsValid = false;
        public int RecordId { get; set; }
        public string RecordIdValue
        {
            get { return FRecordIdValue; }
            set
            {
                FRecordIdValue = value;
                int Dummy = 0;
                RecordIdIsValid = int.TryParse(FRecordIdValue, out Dummy);
                RecordId = (RecordIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FRunIdValue = "";
        public bool RunIdIsValid = false;
        public int RunId { get; set; }
        public string RunIdValue
        {
            get { return FRunIdValue; }
            set
            {
                FRunIdValue = value;
                int Dummy = 0;
                RunIdIsValid = int.TryParse(FRunIdValue, out Dummy);
                RunId = (RunIdIsValid) ? Dummy : 0;
            }
        }
        
        public string Source { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            RecordIdValue = "";
            RunIdValue = "";
            Source = "";
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
                    Key = "RecordId";
                    if (!RecordIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RunId";
                    if (!RunIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Source";
                    if (string.IsNullOrEmpty(Source)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(BulkEmail_RemoveRecipient_RequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.RecordIdValue = Source.RecordIdValue;
                this.RunIdValue = Source.RunIdValue;
                this.Source = Source.Source;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
        //public string Load(ActionBase AActionBase, JObject DataObject)
        public string Load(ActionBase AActionBase)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                //string Method = ClassName + ".LoadFromDataObject";
                string Method = ClassName + ".LoadFromActionBase";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    BulkEmail_RemoveRecipient_RequestItem TempItem = new BulkEmail_RemoveRecipient_RequestItem();
                    //Result = CommonMethods.LoadBulkEmail_RemoveRecipient_RequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadBulkEmail_RemoveRecipient_RequestItem(AActionBase, ref TempItem);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = AActionBase.LogOnProcessingError(Method, Result, "");
                    }
                    else
                    {
                        Result = Update(TempItem);
                    }
                }
                catch (Exception Ex)
                {
                    Result = AActionBase.LogOnException(Method, Ex, "");
                    return Result;
                }
                finally
                {
                    AActionBase.LogOnLeavingMethod(Method);
                }
            
                return Result;
        }
        #endregion
        
    }
}
