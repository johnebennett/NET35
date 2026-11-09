/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 03-10-2020
 * Time: 6:03 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/ChangePasswordDlg/Actions/Templates/ChangePasswordRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.SettingsOperations.ChangePasswordDlg
{
    public class ChangePasswordRequestItem
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
        
        public string OldPassword { get; set; }
        
        public string NewPassword { get; set; }
        
        public string ConfirmPassword { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            ClientIdValue = "";
            OldPassword = "";
            NewPassword = "";
            ConfirmPassword = "";
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
                    Key = "OldPassword";
                    if (string.IsNullOrEmpty(OldPassword)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "NewPassword";
                    if (string.IsNullOrEmpty(NewPassword)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ConfirmPassword";
                    if (string.IsNullOrEmpty(ConfirmPassword)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(ChangePasswordRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.ClientIdValue = Source.ClientIdValue;
                this.OldPassword = Source.OldPassword;
                this.NewPassword = Source.NewPassword;
                this.ConfirmPassword = Source.ConfirmPassword;
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
            
                    ChangePasswordRequestItem TempItem = new ChangePasswordRequestItem();
                    //Result = CommonMethods.LoadChangePasswordRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadChangePasswordRequestItem(ActionBase, ref TempItem);
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
