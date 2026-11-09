/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 23-08-2019
 * Time: 8:01 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\ContactOperations\Actions\EmailStatements\Templates\ProcessRunRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations
{
    public class BulkEmail_ProcessRunRequestItem
    {
        #region Properties - Getters and Setters
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
        
        private string FEventRunIdValue = "";
        public bool EventRunIdIsValid = false;
        public int EventRunId { get; set; }
        public string EventRunIdValue
        {
            get { return FEventRunIdValue; }
            set
            {
                FEventRunIdValue = value;
                int Dummy = 0;
                EventRunIdIsValid = int.TryParse(FEventRunIdValue, out Dummy);
                EventRunId = (EventRunIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FEmailOptionValue = "";
        public bool EmailOptionIsValid = false;
        public int EmailOption { get; set; }
        public string EmailOptionValue
        {
            get { return FEmailOptionValue; }
            set
            {
                FEmailOptionValue = value;
                int Dummy = 0;
                EmailOptionIsValid = int.TryParse(FEmailOptionValue, out Dummy);
                EmailOption = (EmailOptionIsValid) ? Dummy : 0;
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
        
        private string FReportIdValue = "";
        public bool ReportIdIsValid = false;
        public int ReportId { get; set; }
        public string ReportIdValue
        {
            get { return FReportIdValue; }
            set
            {
                FReportIdValue = value;
                int Dummy = 0;
                ReportIdIsValid = int.TryParse(FReportIdValue, out Dummy);
                ReportId = (ReportIdIsValid) ? Dummy : 0;
            }
        }
        
        public string Subject { get; set; }
        
        public string EmailBody { get; set; }
        
        private string FFromValue = "";
        public bool FromIsValid = false;
        public int From { get; set; }
        public string FromValue
        {
            get { return FFromValue; }
            set
            {
                FFromValue = value;
                int Dummy = 0;
                FromIsValid = int.TryParse(FFromValue, out Dummy);
                From = (FromIsValid) ? Dummy : 0;
            }
        }
        
        private string FCCValue = "";
        public bool CCIsValid = false;
        public int CC { get; set; }
        public string CCValue
        {
            get { return FCCValue; }
            set
            {
                FCCValue = value;
                int Dummy = 0;
                CCIsValid = int.TryParse(FCCValue, out Dummy);
                CC = (CCIsValid) ? Dummy : 0;
            }
        }
        
        private string FBCCValue = "";
        public bool BCCIsValid = false;
        public int BCC { get; set; }
        public string BCCValue
        {
            get { return FBCCValue; }
            set
            {
                FBCCValue = value;
                int Dummy = 0;
                BCCIsValid = int.TryParse(FBCCValue, out Dummy);
                BCC = (BCCIsValid) ? Dummy : 0;
            }
        }
        
        private string FExclusionsValue = "";
        public bool ExclusionsIsValid = false;
        public int Exclusions { get; set; }
        public string ExclusionsValue
        {
            get { return FExclusionsValue; }
            set
            {
                FExclusionsValue = value;
                int Dummy = 0;
                ExclusionsIsValid = int.TryParse(FExclusionsValue, out Dummy);
                Exclusions = (ExclusionsIsValid) ? Dummy : 0;
            }
        }
        
        private string FSignatureIdValue = "";
        public bool SignatureIdIsValid = false;
        public int SignatureId { get; set; }
        public string SignatureIdValue
        {
            get { return FSignatureIdValue; }
            set
            {
                FSignatureIdValue = value;
                int Dummy = 0;
                SignatureIdIsValid = int.TryParse(FSignatureIdValue, out Dummy);
                SignatureId = (SignatureIdIsValid) ? Dummy : 0;
            }
        }
        
        public string EventRunSource { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            RunIdValue = "";
            EventRunIdValue = "";
            EmailOptionValue = "";
            TemplateIdValue = "";
            ReportIdValue = "";
            Subject = "";
            EmailBody = "";
            FromValue = "";
            CCValue = "";
            BCCValue = "";
            ExclusionsValue = "";
            SignatureIdValue = "";
            EventRunSource = "";
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
                    Key = "RunId";
                    if (!RunIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EventRunId";
                    if (!EventRunIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EmailOption";
                    if (!EmailOptionIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TemplateId";
                    if (!TemplateIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ReportId";
                    if (!ReportIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Subject";
                    if (string.IsNullOrEmpty(Subject)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EmailBody";
                    if (string.IsNullOrEmpty(EmailBody)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "From";
                    if (!FromIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CC";
                    if (!CCIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BCC";
                    if (!BCCIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Exclusions";
                    if (!ExclusionsIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SignatureId";
                    if (!SignatureIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EventRunSource";
                    if (string.IsNullOrEmpty(EventRunSource)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(BulkEmail_ProcessRunRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.RunIdValue = Source.RunIdValue;
                this.EventRunIdValue = Source.EventRunIdValue;
                this.EmailOptionValue = Source.EmailOptionValue;
                this.TemplateIdValue = Source.TemplateIdValue;
                this.ReportIdValue = Source.ReportIdValue;
                this.Subject = Source.Subject;
                this.EmailBody = Source.EmailBody;
                this.FromValue = Source.FromValue;
                this.CCValue = Source.CCValue;
                this.BCCValue = Source.BCCValue;
                this.ExclusionsValue = Source.ExclusionsValue;
                this.SignatureIdValue = Source.SignatureIdValue;
                this.EventRunSource = Source.EventRunSource;
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
            
                    BulkEmail_ProcessRunRequestItem TempItem = new BulkEmail_ProcessRunRequestItem();
                    //Result = CommonMethods.LoadProcessRunRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadBulkEmail_ProcessRunRequestItem(AActionBase.DataObject, ref TempItem);
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
