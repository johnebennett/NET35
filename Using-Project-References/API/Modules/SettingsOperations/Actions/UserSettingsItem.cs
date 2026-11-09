/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 23-12-2020
 * Time: 1:33 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/Actions/Templates/UserSettingsItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.SettingsOperations
{
    public class UserSettingsItem
    {
        #region Properties - Getters and Setters
        public string Section { get; set; }
        
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
        
        private string FOfficeIdValue = "";
        public bool OfficeIdIsValid = false;
        public int OfficeId { get; set; }
        public string OfficeIdValue
        {
            get { return FOfficeIdValue; }
            set
            {
                FOfficeIdValue = value;
                int Dummy = 0;
                OfficeIdIsValid = int.TryParse(FOfficeIdValue, out Dummy);
                OfficeId = (OfficeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FManagerIdValue = "";
        public bool ManagerIdIsValid = false;
        public int ManagerId { get; set; }
        public string ManagerIdValue
        {
            get { return FManagerIdValue; }
            set
            {
                FManagerIdValue = value;
                int Dummy = 0;
                ManagerIdIsValid = int.TryParse(FManagerIdValue, out Dummy);
                ManagerId = (ManagerIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FPortfolioTypeIdValue = "";
        public bool PortfolioTypeIdIsValid = false;
        public int PortfolioTypeId { get; set; }
        public string PortfolioTypeIdValue
        {
            get { return FPortfolioTypeIdValue; }
            set
            {
                FPortfolioTypeIdValue = value;
                int Dummy = 0;
                PortfolioTypeIdIsValid = int.TryParse(FPortfolioTypeIdValue, out Dummy);
                PortfolioTypeId = (PortfolioTypeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FTransactionOptionValue = "";
        public bool TransactionOptionIsValid = false;
        public int TransactionOption { get; set; }
        public string TransactionOptionValue
        {
            get { return FTransactionOptionValue; }
            set
            {
                FTransactionOptionValue = value;
                int Dummy = 0;
                TransactionOptionIsValid = int.TryParse(FTransactionOptionValue, out Dummy);
                TransactionOption = (TransactionOptionIsValid) ? Dummy : 0;
            }
        }
        
        public string CanEmptyNotepads { get; set; }
        
        public string DefaultTaskListToMe { get; set; }
        
        private string FToDateDaysAheadValue = "";
        public bool ToDateDaysAheadIsValid = false;
        public int ToDateDaysAhead { get; set; }
        public string ToDateDaysAheadValue
        {
            get { return FToDateDaysAheadValue; }
            set
            {
                FToDateDaysAheadValue = value;
                int Dummy = 0;
                ToDateDaysAheadIsValid = int.TryParse(FToDateDaysAheadValue, out Dummy);
                ToDateDaysAhead = (ToDateDaysAheadIsValid) ? Dummy : 0;
            }
        }
        
        public string ShowADLButton { get; set; }
        
        private string FDefaultJobTypeValue = "";
        public bool DefaultJobTypeIsValid = false;
        public int DefaultJobType { get; set; }
        public string DefaultJobTypeValue
        {
            get { return FDefaultJobTypeValue; }
            set
            {
                FDefaultJobTypeValue = value;
                int Dummy = 0;
                DefaultJobTypeIsValid = int.TryParse(FDefaultJobTypeValue, out Dummy);
                DefaultJobType = (DefaultJobTypeIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            Section = "";
            ClientIdValue = "";
            OfficeIdValue = "";
            ManagerIdValue = "";
            PortfolioTypeIdValue = "";
            TransactionOptionValue = "";
            CanEmptyNotepads = "";
            DefaultTaskListToMe = "";
            ToDateDaysAheadValue = "";
            ShowADLButton = "";
            DefaultJobTypeValue = "";
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
                    Key = "Section";
                    if (string.IsNullOrEmpty(Section)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ClientId";
                    if (!ClientIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OfficeId";
                    if (!OfficeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ManagerId";
                    if (!ManagerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PortfolioTypeId";
                    if (!PortfolioTypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransactionOption";
                    if (!TransactionOptionIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CanEmptyNotepads";
                    if (string.IsNullOrEmpty(CanEmptyNotepads)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DefaultTaskListToMe";
                    if (string.IsNullOrEmpty(DefaultTaskListToMe)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ToDateDaysAhead";
                    if (!ToDateDaysAheadIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowADLButton";
                    if (string.IsNullOrEmpty(ShowADLButton)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DefaultJobType";
                    if (!DefaultJobTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UserSettingsItem Source)
        {
            string Result = "";
            
            try
            {
                this.Section = Source.Section;
                this.ClientIdValue = Source.ClientIdValue;
                this.OfficeIdValue = Source.OfficeIdValue;
                this.ManagerIdValue = Source.ManagerIdValue;
                this.PortfolioTypeIdValue = Source.PortfolioTypeIdValue;
                this.TransactionOptionValue = Source.TransactionOptionValue;
                this.CanEmptyNotepads = Source.CanEmptyNotepads;
                this.DefaultTaskListToMe = Source.DefaultTaskListToMe;
                this.ToDateDaysAheadValue = Source.ToDateDaysAheadValue;
                this.ShowADLButton = Source.ShowADLButton;
                this.DefaultJobTypeValue = Source.DefaultJobTypeValue;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
        public string Load(ActionBase AActionBase, JObject DataObject)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".LoadFromDataObject";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    UserSettingsItem TempItem = new UserSettingsItem();
                    Result = CommonMethods.LoadUserSettingsItem(DataObject, ref TempItem);
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
        
        #region Load for CompanyId and ClientId
        public string Load(ActionBase AActionBase, string ConnectionString, Int64 CompanyId, Int64 ClientId, string Section)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".LoadFromDB";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    UserSettingsItem TempItem = new UserSettingsItem();
                    Result =
                        CommonMethods.LoadUserSettingsItem(ConnectionString, CompanyId, ClientId, Section, ref TempItem);
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
        
        #region Save
        public string Save(ActionBase AActionBase, string ConnectionString, Int64 CompanyId, Int64 ClientId)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".Save";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Result = CommonMethods.SaveUserSettingsItem(ConnectionString, CompanyId, ClientId, this);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = AActionBase.LogOnProcessingError(Method, Result, "");
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
