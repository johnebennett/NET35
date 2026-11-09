/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 08-12-2020
 * Time: 5:40 PM

 * SourceFile: /home/john/TestSource/EAOffice/EAOfficeAPI/Modules/ToDo/AddStorageDlg/Actions/Templates/UploadFilesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.ToDo.AddStorageDlg
{
    public class UploadFilesRequestItem
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
        
        private string FJobIdValue = "";
        public bool JobIdIsValid = false;
        public int JobId { get; set; }
        public string JobIdValue
        {
            get { return FJobIdValue; }
            set
            {
                FJobIdValue = value;
                int Dummy = 0;
                JobIdIsValid = int.TryParse(FJobIdValue, out Dummy);
                JobId = (JobIdIsValid) ? Dummy : 0;
            }
        }
        
        public string FileName { get; set; }
        
        public string Destination { get; set; }
        
        public string SplitChar { get; set; }
        
        private string FUseLastValue = "";
        public bool UseLastIsValid = false;
        public int UseLast { get; set; }
        public string UseLastValue
        {
            get { return FUseLastValue; }
            set
            {
                FUseLastValue = value;
                int Dummy = 0;
                UseLastIsValid = int.TryParse(FUseLastValue, out Dummy);
                UseLast = (UseLastIsValid) ? Dummy : 0;
            }
        }
        
        public string Source { get; set; }
        
        private string FFileSizeValue = "";
        public bool FileSizeIsValid = false;
        public long FileSize { get; set; }
        public string FileSizeValue
        {
            get { return FFileSizeValue; }
            set
            {
                FFileSizeValue = value;
                long Dummy = 0;
                FileSizeIsValid = long.TryParse(FFileSizeValue, out Dummy);
                FileSize = (FileSizeIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            JobIdValue = "";
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
                    Key = "PortfolioId";
                    if (!PortfolioIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "JobId";
                    if (!JobIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UploadFilesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.JobIdValue = Source.JobIdValue;
                this.FileName = Source.FileName;
                this.Destination = Source.Destination;
                this.SplitChar = Source.SplitChar;
                this.UseLastValue = Source.UseLastValue;
                this.Source = Source.Source;
                this.FileSize = Source.FileSize;
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
            
                    UploadFilesRequestItem TempItem = new UploadFilesRequestItem();
                    //Result = CommonMethods.LoadUploadFilesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadUploadFilesRequestItem(ActionBase, ref TempItem);
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
