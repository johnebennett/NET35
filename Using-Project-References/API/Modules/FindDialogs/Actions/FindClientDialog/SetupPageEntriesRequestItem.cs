/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 23-06-2020
 * Time: 8:15 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\FindDialogs\Actions\Templates\FindClientDialog\SetupFindDialogRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.FindDialogs.FindClientDialog
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
        
        public string TableToSearch { get; set; }
        
        public string FindText { get; set; }
        
        public string IncludeInactives { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            TypeIdValue = "";
            TableToSearch = "";
            IncludeInactives = "";
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
                    Key = "TableToSearch";
                    if (string.IsNullOrEmpty(TableToSearch)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "IncludeInactives";
                    if (string.IsNullOrEmpty(IncludeInactives)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.TableToSearch = Source.TableToSearch;
                this.FindText = Source.FindText;
                this.IncludeInactives = Source.IncludeInactives;
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
