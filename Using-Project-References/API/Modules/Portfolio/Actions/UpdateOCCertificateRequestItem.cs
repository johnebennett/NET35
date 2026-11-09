/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 10-02-2020
 * Time: 4:05 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\Actions\Templates\UpdateOCCertificateRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio
{
    public class UpdateOCCertificateRequestItem
    {
        #region Properties - Getters and Setters
        private string FRecordIdValue = "";
        public bool RecordIdIsValid = false;
        public int RecordId { get; set; }
        public string RecordIdValue
        {
            get { return FRecordIdValue; }
            set
            {
                FRecordIdValue = value;
                int Dummy = 0;
                RecordIdIsValid = int.TryParse(FRecordIdValue, out Dummy);
                RecordId = (RecordIdIsValid) ? Dummy : 0;
            }
        }
        
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
        
        public string Address { get; set; }
        
        public string InPresenceOf { get; set; }
        
        public string LotNo { get; set; }
        
        public string PlanNo { get; set; }
        
        public string PostalAddress { get; set; }
        
        public string Applicant { get; set; }
        
        public string DeliveryAddress { get; set; }
        
        public string OCDate { get; set; }
        
        public string IssuedDate { get; set; }
        
        public string EntityName { get; set; }
        
        private string FCategoryOnlyIdValue = "";
        public bool CategoryOnlyIdIsValid = false;
        public int CategoryOnlyId { get; set; }
        public string CategoryOnlyIdValue
        {
            get { return FCategoryOnlyIdValue; }
            set
            {
                FCategoryOnlyIdValue = value;
                int Dummy = 0;
                CategoryOnlyIdIsValid = int.TryParse(FCategoryOnlyIdValue, out Dummy);
                CategoryOnlyId = (CategoryOnlyIdIsValid) ? Dummy : 0;
            }
        }
        
        public string NoteContent { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            RecordIdValue = "";
            PortfolioIdValue = "";
            OCDate = "";
            IssuedDate = "";
            CategoryOnlyIdValue = "";
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
                    Key = "RecordId";
                    if (!RecordIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PortfolioId";
                    if (!PortfolioIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OCDate";
                    if (string.IsNullOrEmpty(OCDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "IssuedDate";
                    if (string.IsNullOrEmpty(IssuedDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CategoryOnlyId";
                    if (!CategoryOnlyIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(UpdateOCCertificateRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.RecordIdValue = Source.RecordIdValue;
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.Address = Source.Address;
                this.InPresenceOf = Source.InPresenceOf;
                this.LotNo = Source.LotNo;
                this.PlanNo = Source.PlanNo;
                this.PostalAddress = Source.PostalAddress;
                this.Applicant = Source.Applicant;
                this.DeliveryAddress = Source.DeliveryAddress;
                this.OCDate = Source.OCDate;
                this.IssuedDate = Source.IssuedDate;
                this.EntityName = Source.EntityName;
                this.CategoryOnlyIdValue = Source.CategoryOnlyIdValue;
                this.NoteContent = Source.NoteContent;
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
                string Method = ClassName + ".LoadFromDataObject";
            
                ActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    UpdateOCCertificateRequestItem TempItem = new UpdateOCCertificateRequestItem();
                    Result = CommonMethods.LoadUpdateOCCertificateRequestItem(ActionBase, ref TempItem);
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
