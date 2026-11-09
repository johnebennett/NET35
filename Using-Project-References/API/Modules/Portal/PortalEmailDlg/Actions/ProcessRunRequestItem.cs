/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 11-04-2022
 * Time: 6:11 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portal/PortalEmailDlg/Actions/Templates/ProcessRunRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Portal.PortalEmailDlg
{
    public class ProcessRunRequestItem
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
        
        private string FTaskValue = "";
        public bool TaskIsValid = false;
        public int Task { get; set; }
        public string TaskValue
        {
            get { return FTaskValue; }
            set
            {
                FTaskValue = value;
                int Dummy = 0;
                TaskIsValid = int.TryParse(FTaskValue, out Dummy);
                Task = (TaskIsValid) ? Dummy : 0;
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
        
        private string FReportEventIdValue = "";
        public bool ReportEventIdIsValid = false;
        public int ReportEventId { get; set; }
        public string ReportEventIdValue
        {
            get { return FReportEventIdValue; }
            set
            {
                FReportEventIdValue = value;
                int Dummy = 0;
                ReportEventIdIsValid = int.TryParse(FReportEventIdValue, out Dummy);
                ReportEventId = (ReportEventIdIsValid) ? Dummy : 0;
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
        
        private string FPrintToValue = "";
        public bool PrintToIsValid = false;
        public int PrintTo { get; set; }
        public string PrintToValue
        {
            get { return FPrintToValue; }
            set
            {
                FPrintToValue = value;
                int Dummy = 0;
                PrintToIsValid = int.TryParse(FPrintToValue, out Dummy);
                PrintTo = (PrintToIsValid) ? Dummy : 0;
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
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            RunIdValue = "";
            TaskValue = "";
            WhoTo1Value = "";
            WhoTo2Value = "";
            EmailOptionValue = "";
            TemplateIdValue = "";
            ReportEventIdValue = "";
            Subject = "";
            EmailBody = "";
            FromValue = "";
            CCValue = "";
            BCCValue = "";
            PrintToValue = "";
            SignatureIdValue = "";
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
                    Key = "Task";
                    if (!TaskIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    Key = "ReportEventId";
                    if (!ReportEventIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    Key = "PrintTo";
                    if (!PrintToIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SignatureId";
                    if (!SignatureIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(ProcessRunRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.RunIdValue = Source.RunIdValue;
                this.TaskValue = Source.TaskValue;
                this.WhoTo1Value = Source.WhoTo1Value;
                this.WhoTo2Value = Source.WhoTo2Value;
                this.EmailOptionValue = Source.EmailOptionValue;
                this.TemplateIdValue = Source.TemplateIdValue;
                this.ReportEventIdValue = Source.ReportEventIdValue;
                this.Subject = Source.Subject;
                this.EmailBody = Source.EmailBody;
                this.FromValue = Source.FromValue;
                this.CCValue = Source.CCValue;
                this.BCCValue = Source.BCCValue;
                this.PrintToValue = Source.PrintToValue;
                this.SignatureIdValue = Source.SignatureIdValue;
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
            
                    ProcessRunRequestItem TempItem = new ProcessRunRequestItem();
                    //Result = CommonMethods.LoadProcessRunRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadProcessRunRequestItem(AActionBase, ref TempItem);
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
