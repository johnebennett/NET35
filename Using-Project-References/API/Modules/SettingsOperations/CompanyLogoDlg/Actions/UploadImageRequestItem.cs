/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 20-09-2020
 * Time: 10:42 AM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/CompanyLogoDlg/Actions/Templates/UploadImageRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.SettingsOperations.CompanyLogoDlg
{
    public class UploadImageRequestItem
    {
        #region Properties - Getters and Setters
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
            Destination = "";
            SplitChar = "";
            UseLastValue = "";
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
                    Key = "Destination";
                    if (string.IsNullOrEmpty(Destination)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SplitChar";
                    if (string.IsNullOrEmpty(SplitChar)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "UseLast";
                    if (!UseLastIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UploadImageRequestItem Source)
        {
            string Result = "";
            
            try
            {
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
            
                    UploadImageRequestItem TempItem = new UploadImageRequestItem();
                    //Result = CommonMethods.LoadUploadImageRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadUploadImageRequestItem(ActionBase, ref TempItem);
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
