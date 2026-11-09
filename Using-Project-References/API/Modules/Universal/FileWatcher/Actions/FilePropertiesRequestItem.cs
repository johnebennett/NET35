/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 09-10-2024
 * Time: 10:48 PM

 * SourceFile: E:\Source\EAOffice\EAOfficeAPI\Modules\Universal\FileWatcher\Actions\Templates\FileProperties\FilePropertiesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.FileWatcher
{
    public class FilePropertiesRequestItem
    {
        #region Properties - Getters and Setters
        public string SourceFolder { get; set; }
        
        public string DestinationFolder { get; set; }
        
        public string FileName { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            SourceFolder = "";
            DestinationFolder = "";
            FileName = "";
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
                    Key = "SourceFolder";
                    if (string.IsNullOrEmpty(SourceFolder)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DestinationFolder";
                    if (string.IsNullOrEmpty(DestinationFolder)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FileName";
                    if (string.IsNullOrEmpty(FileName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(FilePropertiesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.SourceFolder = Source.SourceFolder;
                this.DestinationFolder = Source.DestinationFolder;
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
            
                    FilePropertiesRequestItem TempItem = new FilePropertiesRequestItem();
                    //Result = CommonMethods.LoadFilePropertiesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadFilePropertiesRequestItem(ActionBase, ref TempItem);
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
