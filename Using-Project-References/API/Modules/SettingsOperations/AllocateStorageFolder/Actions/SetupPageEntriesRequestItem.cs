/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 02-09-2020
 * Time: 4:28 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/AllocateStorageFolder/Actions/Templates/SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations.AllocateStorageFolder
{
    public class SetupPageEntriesRequestItem
    {
        #region Properties - Getters and Setters
        private string FTypeIdValue = "";
        public bool TypeIdIsValid = false;
        public int TypeId { get; set; }
        public string TypeIdValue
        {
            get { return FTypeIdValue; }
            set
            {
                FTypeIdValue = value;
                int Dummy = 0;
                TypeIdIsValid = int.TryParse(FTypeIdValue, out Dummy);
                TypeId = (TypeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FModuleIdValue = "";
        public bool ModuleIdIsValid = false;
        public int ModuleId { get; set; }
        public string ModuleIdValue
        {
            get { return FModuleIdValue; }
            set
            {
                FModuleIdValue = value;
                int Dummy = 0;
                ModuleIdIsValid = int.TryParse(FModuleIdValue, out Dummy);
                ModuleId = (ModuleIdIsValid) ? Dummy : 0;
            }
        }
        
        public string StorageForProperty { get; set; }
        
        public string StorageForLedger { get; set; }
        
        public string BusTypes { get; set; }
        
        public string Hide { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            TypeIdValue = "";
            ModuleIdValue = "";
            StorageForProperty = "";
            StorageForLedger = "";
            Hide = "";
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
                    Key = "TypeId";
                    if (!TypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ModuleId";
                    if (!ModuleIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StorageForProperty";
                    if (string.IsNullOrEmpty(StorageForProperty)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StorageForLedger";
                    if (string.IsNullOrEmpty(StorageForLedger)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Hide";
                    if (string.IsNullOrEmpty(Hide)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.TypeIdValue = Source.TypeIdValue;
                this.ModuleIdValue = Source.ModuleIdValue;
                this.StorageForProperty = Source.StorageForProperty;
                this.StorageForLedger = Source.StorageForLedger;
                this.BusTypes = Source.BusTypes;
                this.Hide = Source.Hide;
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
