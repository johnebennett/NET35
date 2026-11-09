/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 01-09-2020
 * Time: 5:03 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/PrepareLetter/Actions/Templates/SetupPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.PrepareLetter
{
    public class SetupPageEntriesRequestItem
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
        
        private string FCategoryIdValue = "";
        public bool CategoryIdIsValid = false;
        public int CategoryId { get; set; }
        public string CategoryIdValue
        {
            get { return FCategoryIdValue; }
            set
            {
                FCategoryIdValue = value;
                int Dummy = 0;
                CategoryIdIsValid = int.TryParse(FCategoryIdValue, out Dummy);
                CategoryId = (CategoryIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FDocumentIdValue = "";
        public bool DocumentIdIsValid = false;
        public int DocumentId { get; set; }
        public string DocumentIdValue
        {
            get { return FDocumentIdValue; }
            set
            {
                FDocumentIdValue = value;
                int Dummy = 0;
                DocumentIdIsValid = int.TryParse(FDocumentIdValue, out Dummy);
                DocumentId = (DocumentIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FClientsIdValue = "";
        public bool ClientsIdIsValid = false;
        public int ClientsId { get; set; }
        public string ClientsIdValue
        {
            get { return FClientsIdValue; }
            set
            {
                FClientsIdValue = value;
                int Dummy = 0;
                ClientsIdIsValid = int.TryParse(FClientsIdValue, out Dummy);
                ClientsId = (ClientsIdIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            CategoryIdValue = "";
            DocumentIdValue = "";
            ClientsIdValue = "";
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
                    Key = "CategoryId";
                    if (!CategoryIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DocumentId";
                    if (!DocumentIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ClientsId";
                    if (!ClientsIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.CategoryIdValue = Source.CategoryIdValue;
                this.DocumentIdValue = Source.DocumentIdValue;
                this.ClientsIdValue = Source.ClientsIdValue;
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
