/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 08-10-2020
 * Time: 5:20 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/SignatureDlg/Actions/Templates/SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.SettingsOperations.SignatureDlg
{
    public class SetupPageEntriesRequestItem
    {
        #region Properties - Getters and Setters
        private string FClientIdValue = "";
        public bool ClientIdIsValid = false;
        public int ClientId { get; set; }
        public string ClientIdValue
        {
            get { return FClientIdValue; }
            set
            {
                FClientIdValue = value;
                int Dummy = 0;
                ClientIdIsValid = int.TryParse(FClientIdValue, out Dummy);
                ClientId = (ClientIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FSignatureSourceValue = "";
        public bool SignatureSourceIsValid = false;
        public int SignatureSource { get; set; }
        public string SignatureSourceValue
        {
            get { return FSignatureSourceValue; }
            set
            {
                FSignatureSourceValue = value;
                int Dummy = 0;
                SignatureSourceIsValid = int.TryParse(FSignatureSourceValue, out Dummy);
                SignatureSource = (SignatureSourceIsValid) ? Dummy : 0;
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
        
        public string FileName { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            ClientIdValue = "";
            SignatureSourceValue = "";
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
                    Key = "ClientId";
                    if (!ClientIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SignatureSource";
                    if (!SignatureSourceIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SetupPageEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.ClientIdValue = Source.ClientIdValue;
                this.SignatureSourceValue = Source.SignatureSourceValue;
                this.Source = Source.Source;
                this.FileSize = Source.FileSize;
                this.FileName = Source.FileName;
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
            
                    SetupPageEntriesRequestItem TempItem = new SetupPageEntriesRequestItem();
                    //Result = CommonMethods.LoadSetupPageEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSetupPageEntriesRequestItem(ActionBase, ref TempItem);
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
