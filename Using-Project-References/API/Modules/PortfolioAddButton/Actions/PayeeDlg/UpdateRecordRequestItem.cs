/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 24-06-2020
 * Time: 8:23 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\PortfolioAddButton\Actions\Templates\PayeeDlg\UpdateRecordRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PortfolioAddButton.PayeeDlg
{
    public class UpdateRecordRequestItem
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
        
        private string FOption1IdValue = "";
        public bool Option1IdIsValid = false;
        public int Option1Id { get; set; }
        public string Option1IdValue
        {
            get { return FOption1IdValue; }
            set
            {
                FOption1IdValue = value;
                int Dummy = 0;
                Option1IdIsValid = int.TryParse(FOption1IdValue, out Dummy);
                Option1Id = (Option1IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOption2IdValue = "";
        public bool Option2IdIsValid = false;
        public int Option2Id { get; set; }
        public string Option2IdValue
        {
            get { return FOption2IdValue; }
            set
            {
                FOption2IdValue = value;
                int Dummy = 0;
                Option2IdIsValid = int.TryParse(FOption2IdValue, out Dummy);
                Option2Id = (Option2IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FGridIdValue = "";
        public bool GridIdIsValid = false;
        public int GridId { get; set; }
        public string GridIdValue
        {
            get { return FGridIdValue; }
            set
            {
                FGridIdValue = value;
                int Dummy = 0;
                GridIdIsValid = int.TryParse(FGridIdValue, out Dummy);
                GridId = (GridIdIsValid) ? Dummy : 0;
            }
        }
        
        public string TempId { get; set; }
        
        private string FPayeeIdValue = "";
        public bool PayeeIdIsValid = false;
        public int PayeeId { get; set; }
        public string PayeeIdValue
        {
            get { return FPayeeIdValue; }
            set
            {
                FPayeeIdValue = value;
                int Dummy = 0;
                PayeeIdIsValid = int.TryParse(FPayeeIdValue, out Dummy);
                PayeeId = (PayeeIdIsValid) ? Dummy : 0;
            }
        }
        
        public string StmtName { get; set; }
        
        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        
        public string Address3 { get; set; }
        
        public string Address4 { get; set; }
        
        public string DirectEntry { get; set; }
        
        public string BSB { get; set; }
        
        public string AccountNumber { get; set; }
        
        public string AccountDetail { get; set; }
        
        public string RemoveWhenPosted { get; set; }
        
        private string FPercentageValue = "";
        public bool PercentageIsValid = false;
        public double Percentage { get; set; }
        public string PercentageValue
        {
            get { return FPercentageValue; }
            set
            {
                FPercentageValue = value;
                double Dummy = 0.0;
                PercentageIsValid = Double.TryParse(FPercentageValue, out Dummy);
                Percentage = (PercentageIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            Option1IdValue = "";
            Option2IdValue = "";
            GridIdValue = "";
            TempId = "";
            PayeeIdValue = "";
            StmtName = "";
            DirectEntry = "";
            RemoveWhenPosted = "";
            PercentageValue = "";
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
                    Key = "Option1Id";
                    if (!Option1IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Option2Id";
                    if (!Option2IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GridId";
                    if (!GridIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TempId";
                    if (string.IsNullOrEmpty(TempId)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PayeeId";
                    if (!PayeeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "StmtName";
                    if (string.IsNullOrEmpty(StmtName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DirectEntry";
                    if (string.IsNullOrEmpty(DirectEntry)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RemoveWhenPosted";
                    if (string.IsNullOrEmpty(RemoveWhenPosted)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Percentage";
                    if (!PercentageIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UpdateRecordRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.Option1IdValue = Source.Option1IdValue;
                this.Option2IdValue = Source.Option2IdValue;
                this.GridIdValue = Source.GridIdValue;
                this.TempId = Source.TempId;
                this.PayeeIdValue = Source.PayeeIdValue;
                this.StmtName = Source.StmtName;
                this.Address1 = Source.Address1;
                this.Address2 = Source.Address2;
                this.Address3 = Source.Address3;
                this.Address4 = Source.Address4;
                this.DirectEntry = Source.DirectEntry;
                this.BSB = Source.BSB;
                this.AccountNumber = Source.AccountNumber;
                this.AccountDetail = Source.AccountDetail;
                this.RemoveWhenPosted = Source.RemoveWhenPosted;
                this.PercentageValue = Source.PercentageValue;
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
            
                    UpdateRecordRequestItem TempItem = new UpdateRecordRequestItem();
                    //Result = CommonMethods.LoadUpdateRecordRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadUpdateRecordRequestItem(ActionBase, ref TempItem);
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
