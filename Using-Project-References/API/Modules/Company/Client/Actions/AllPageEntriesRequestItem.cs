/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 02-08-2020
 * Time: 9:36 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Company\Client\Actions\Templates\AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Company.Client
{
    public class AllPageEntriesRequestItem
    {
        #region Properties - Getters and Setters
        public string IsAdd { get; set; }
        
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
        
        private string FPropertyIdValue = "";
        public bool PropertyIdIsValid = false;
        public int PropertyId { get; set; }
        public string PropertyIdValue
        {
            get { return FPropertyIdValue; }
            set
            {
                FPropertyIdValue = value;
                int Dummy = 0;
                PropertyIdIsValid = int.TryParse(FPropertyIdValue, out Dummy);
                PropertyId = (PropertyIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FClientTypeValue = "";
        public bool ClientTypeIsValid = false;
        public int ClientType { get; set; }
        public string ClientTypeValue
        {
            get { return FClientTypeValue; }
            set
            {
                FClientTypeValue = value;
                int Dummy = 0;
                ClientTypeIsValid = int.TryParse(FClientTypeValue, out Dummy);
                ClientType = (ClientTypeIsValid) ? Dummy : 0;
            }
        }
        
        public string SurnameCompany { get; set; }
        
        public string Title { get; set; }
        
        public string ACN { get; set; }
        
        public string ABN { get; set; }
        
        public string Contact { get; set; }
        
        public string Name { get; set; }
        
        public string Attention { get; set; }
        
        public string Position { get; set; }
        
        public string Department { get; set; }
        
        public string TradingAs { get; set; }
        
        public string DearFormal { get; set; }
        
        public string DearInformal { get; set; }
        
        public string ClientAddress1 { get; set; }
        
        public string ClientAddress2 { get; set; }
        
        public string ClientAddress3 { get; set; }
        
        public string Suburb { get; set; }
        
        public string State { get; set; }
        
        public string PostCode { get; set; }
        
        public string Country { get; set; }
        
        public string Home { get; set; }
        
        public string Mobile1 { get; set; }
        
        public string Mobile2 { get; set; }
        
        public string Mobile3 { get; set; }
        
        public string Business1 { get; set; }
        
        public string Business2 { get; set; }
        
        public string Business3 { get; set; }
        
        public string Fax { get; set; }
        
        public string Email1 { get; set; }
        
        public string Email2 { get; set; }
        
        public string Email3 { get; set; }
        
        public string AccountsEmail1 { get; set; }
        
        public string AccountsEmail2 { get; set; }
        
        public string AccountsEmail3 { get; set; }
        
        public string AccountsEmail4 { get; set; }
        
        public string AccountsEmail5 { get; set; }
        
        public string AccountsEmail6 { get; set; }
        
        public string EMailStmt { get; set; }
        
        public string PrintStmt { get; set; }
        
        public string eMailNotice { get; set; }
        
        public string PrintNotice { get; set; }
        
        public string PostalAddress1 { get; set; }
        
        public string PostalAddress2 { get; set; }
        
        public string PostalAddress3 { get; set; }
        
        public string PostalAddress4 { get; set; }
        
        public string PostalAddress5 { get; set; }
        
        public string PostalAddress6 { get; set; }
        
        public string PostalAddress7 { get; set; }
        
        public string PostalAddress8 { get; set; }
        
        public string NoteText { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            IsAdd = "";
            ClientIdValue = "";
            PropertyIdValue = "";
            ClientTypeValue = "";
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
                    Key = "IsAdd";
                    if (string.IsNullOrEmpty(IsAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ClientId";
                    if (!ClientIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PropertyId";
                    if (!PropertyIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ClientType";
                    if (!ClientTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(AllPageEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.IsAdd = Source.IsAdd;
                this.ClientIdValue = Source.ClientIdValue;
                this.PropertyIdValue = Source.PropertyIdValue;
                this.ClientTypeValue = Source.ClientTypeValue;
                this.SurnameCompany = Source.SurnameCompany;
                this.Title = Source.Title;
                this.ACN = Source.ACN;
                this.ABN = Source.ABN;
                this.Contact = Source.Contact;
                this.Name = Source.Name;
                this.Attention = Source.Attention;
                this.Position = Source.Position;
                this.Department = Source.Department;
                this.TradingAs = Source.TradingAs;
                this.DearFormal = Source.DearFormal;
                this.DearInformal = Source.DearInformal;
                this.ClientAddress1 = Source.ClientAddress1;
                this.ClientAddress2 = Source.ClientAddress2;
                this.ClientAddress3 = Source.ClientAddress3;
                this.Suburb = Source.Suburb;
                this.State = Source.State;
                this.PostCode = Source.PostCode;
                this.Country = Source.Country;
                this.Home = Source.Home;
                this.Mobile1 = Source.Mobile1;
                this.Mobile2 = Source.Mobile2;
                this.Mobile3 = Source.Mobile3;
                this.Business1 = Source.Business1;
                this.Business2 = Source.Business2;
                this.Business3 = Source.Business3;
                this.Fax = Source.Fax;
                this.Email1 = Source.Email1;
                this.Email2 = Source.Email2;
                this.Email3 = Source.Email3;
                this.AccountsEmail1 = Source.AccountsEmail1;
                this.AccountsEmail2 = Source.AccountsEmail2;
                this.AccountsEmail3 = Source.AccountsEmail3;
                this.AccountsEmail4 = Source.AccountsEmail4;
                this.AccountsEmail5 = Source.AccountsEmail5;
                this.AccountsEmail6 = Source.AccountsEmail6;
                this.EMailStmt = Source.EMailStmt;
                this.PrintStmt = Source.PrintStmt;
                this.eMailNotice = Source.eMailNotice;
                this.PrintNotice = Source.PrintNotice;
                this.PostalAddress1 = Source.PostalAddress1;
                this.PostalAddress2 = Source.PostalAddress2;
                this.PostalAddress3 = Source.PostalAddress3;
                this.PostalAddress4 = Source.PostalAddress4;
                this.PostalAddress5 = Source.PostalAddress5;
                this.PostalAddress6 = Source.PostalAddress6;
                this.PostalAddress7 = Source.PostalAddress7;
                this.PostalAddress8 = Source.PostalAddress8;
                this.NoteText = Source.NoteText;
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
            
                    AllPageEntriesRequestItem TempItem = new AllPageEntriesRequestItem();
                    //Result = CommonMethods.LoadAllPageEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadAllPageEntriesRequestItem(ActionBase, ref TempItem);
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
