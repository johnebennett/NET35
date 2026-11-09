/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 01-12-2020
 * Time: 4:16 PM

 * SourceFile: /home/john/TestSource/EAOffice/EAOfficeAPI/Modules/SettingsOperations/Actions/Templates/SaveResidentialBondsRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations
{
    public class SaveResidentialBondsRequestItem
    {
        #region Properties - Getters and Setters
        public string BPayVic { get; set; }
        
        public string BPayNSW { get; set; }
        
        public string BPayQld { get; set; }
        
        public string BPayACT { get; set; }
        
        public string BPayNT { get; set; }
        
        public string BPayWA { get; set; }
        
        public string BPaySA { get; set; }
        
        public string BPayTas { get; set; }
        
        public string DDVic { get; set; }
        
        public string DDNSW { get; set; }
        
        public string DDQld { get; set; }
        
        public string DDACT { get; set; }
        
        public string DDNT { get; set; }
        
        public string DDWA { get; set; }
        
        public string DDSA { get; set; }
        
        public string DDTas { get; set; }
        
        private string FResInspMonthStartValue = "";
        public bool ResInspMonthStartIsValid = false;
        public int ResInspMonthStart { get; set; }
        public string ResInspMonthStartValue
        {
            get { return FResInspMonthStartValue; }
            set
            {
                FResInspMonthStartValue = value;
                int Dummy = 0;
                ResInspMonthStartIsValid = int.TryParse(FResInspMonthStartValue, out Dummy);
                ResInspMonthStart = (ResInspMonthStartIsValid) ? Dummy : 0;
            }
        }
        
        private string FInspectionAdvanceValue = "";
        public bool InspectionAdvanceIsValid = false;
        public int InspectionAdvance { get; set; }
        public string InspectionAdvanceValue
        {
            get { return FInspectionAdvanceValue; }
            set
            {
                FInspectionAdvanceValue = value;
                int Dummy = 0;
                InspectionAdvanceIsValid = int.TryParse(FInspectionAdvanceValue, out Dummy);
                InspectionAdvance = (InspectionAdvanceIsValid) ? Dummy : 0;
            }
        }
        
        private string FRentReviewInitialAdvanceValue = "";
        public bool RentReviewInitialAdvanceIsValid = false;
        public int RentReviewInitialAdvance { get; set; }
        public string RentReviewInitialAdvanceValue
        {
            get { return FRentReviewInitialAdvanceValue; }
            set
            {
                FRentReviewInitialAdvanceValue = value;
                int Dummy = 0;
                RentReviewInitialAdvanceIsValid = int.TryParse(FRentReviewInitialAdvanceValue, out Dummy);
                RentReviewInitialAdvance = (RentReviewInitialAdvanceIsValid) ? Dummy : 0;
            }
        }
        
        private string FRentReviewAdvanceValue = "";
        public bool RentReviewAdvanceIsValid = false;
        public int RentReviewAdvance { get; set; }
        public string RentReviewAdvanceValue
        {
            get { return FRentReviewAdvanceValue; }
            set
            {
                FRentReviewAdvanceValue = value;
                int Dummy = 0;
                RentReviewAdvanceIsValid = int.TryParse(FRentReviewAdvanceValue, out Dummy);
                RentReviewAdvance = (RentReviewAdvanceIsValid) ? Dummy : 0;
            }
        }
        
        public string CreateLeaseExpiry { get; set; }
        
        public string CreateResVacate { get; set; }
        
        public string LettingFeeTimes { get; set; }
        
        public string LettingFeePercentage { get; set; }
        
        private string FLettingFeeOptionValue = "";
        public bool LettingFeeOptionIsValid = false;
        public int LettingFeeOption { get; set; }
        public string LettingFeeOptionValue
        {
            get { return FLettingFeeOptionValue; }
            set
            {
                FLettingFeeOptionValue = value;
                int Dummy = 0;
                LettingFeeOptionIsValid = int.TryParse(FLettingFeeOptionValue, out Dummy);
                LettingFeeOption = (LettingFeeOptionIsValid) ? Dummy : 0;
            }
        }
        
        private string FLettingFeeAccountIdValue = "";
        public bool LettingFeeAccountIdIsValid = false;
        public int LettingFeeAccountId { get; set; }
        public string LettingFeeAccountIdValue
        {
            get { return FLettingFeeAccountIdValue; }
            set
            {
                FLettingFeeAccountIdValue = value;
                int Dummy = 0;
                LettingFeeAccountIdIsValid = int.TryParse(FLettingFeeAccountIdValue, out Dummy);
                LettingFeeAccountId = (LettingFeeAccountIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FAdvertisingAccountIdValue = "";
        public bool AdvertisingAccountIdIsValid = false;
        public int AdvertisingAccountId { get; set; }
        public string AdvertisingAccountIdValue
        {
            get { return FAdvertisingAccountIdValue; }
            set
            {
                FAdvertisingAccountIdValue = value;
                int Dummy = 0;
                AdvertisingAccountIdIsValid = int.TryParse(FAdvertisingAccountIdValue, out Dummy);
                AdvertisingAccountId = (AdvertisingAccountIdIsValid) ? Dummy : 0;
            }
        }
        
        public string EndOfYearStatement { get; set; }
        
        private string FSendEndOfYearStmtToValue = "";
        public bool SendEndOfYearStmtToIsValid = false;
        public int SendEndOfYearStmtTo { get; set; }
        public string SendEndOfYearStmtToValue
        {
            get { return FSendEndOfYearStmtToValue; }
            set
            {
                FSendEndOfYearStmtToValue = value;
                int Dummy = 0;
                SendEndOfYearStmtToIsValid = int.TryParse(FSendEndOfYearStmtToValue, out Dummy);
                SendEndOfYearStmtTo = (SendEndOfYearStmtToIsValid) ? Dummy : 0;
            }
        }
        
        public string EndOfYearStmtFee { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            BPayVic = "";
            BPayNSW = "";
            BPayQld = "";
            BPayACT = "";
            BPayNT = "";
            BPayWA = "";
            BPaySA = "";
            BPayTas = "";
            DDVic = "";
            DDNSW = "";
            DDQld = "";
            DDACT = "";
            DDNT = "";
            DDWA = "";
            DDSA = "";
            DDTas = "";
            ResInspMonthStartValue = "";
            InspectionAdvanceValue = "";
            RentReviewInitialAdvanceValue = "";
            RentReviewAdvanceValue = "";
            CreateLeaseExpiry = "";
            CreateResVacate = "";
            LettingFeeTimes = "";
            LettingFeePercentage = "";
            LettingFeeOptionValue = "";
            LettingFeeAccountIdValue = "";
            AdvertisingAccountIdValue = "";
            EndOfYearStatement = "";
            SendEndOfYearStmtToValue = "";
            EndOfYearStmtFee = "";
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
                    Key = "BPayVic";
                    if (string.IsNullOrEmpty(BPayVic)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BPayNSW";
                    if (string.IsNullOrEmpty(BPayNSW)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BPayQld";
                    if (string.IsNullOrEmpty(BPayQld)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BPayACT";
                    if (string.IsNullOrEmpty(BPayACT)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BPayNT";
                    if (string.IsNullOrEmpty(BPayNT)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BPayWA";
                    if (string.IsNullOrEmpty(BPayWA)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BPaySA";
                    if (string.IsNullOrEmpty(BPaySA)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BPayTas";
                    if (string.IsNullOrEmpty(BPayTas)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DDVic";
                    if (string.IsNullOrEmpty(DDVic)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DDNSW";
                    if (string.IsNullOrEmpty(DDNSW)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DDQld";
                    if (string.IsNullOrEmpty(DDQld)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DDACT";
                    if (string.IsNullOrEmpty(DDACT)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DDNT";
                    if (string.IsNullOrEmpty(DDNT)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DDWA";
                    if (string.IsNullOrEmpty(DDWA)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DDSA";
                    if (string.IsNullOrEmpty(DDSA)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DDTas";
                    if (string.IsNullOrEmpty(DDTas)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ResInspMonthStart";
                    if (!ResInspMonthStartIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "InspectionAdvance";
                    if (!InspectionAdvanceIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RentReviewInitialAdvance";
                    if (!RentReviewInitialAdvanceIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "RentReviewAdvance";
                    if (!RentReviewAdvanceIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CreateLeaseExpiry";
                    if (string.IsNullOrEmpty(CreateLeaseExpiry)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CreateResVacate";
                    if (string.IsNullOrEmpty(CreateResVacate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LettingFeeTimes";
                    if (string.IsNullOrEmpty(LettingFeeTimes)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LettingFeePercentage";
                    if (string.IsNullOrEmpty(LettingFeePercentage)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LettingFeeOption";
                    if (!LettingFeeOptionIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LettingFeeAccountId";
                    if (!LettingFeeAccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AdvertisingAccountId";
                    if (!AdvertisingAccountIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EndOfYearStatement";
                    if (string.IsNullOrEmpty(EndOfYearStatement)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SendEndOfYearStmtTo";
                    if (!SendEndOfYearStmtToIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "EndOfYearStmtFee";
                    if (string.IsNullOrEmpty(EndOfYearStmtFee)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SaveResidentialBondsRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.BPayVic = Source.BPayVic;
                this.BPayNSW = Source.BPayNSW;
                this.BPayQld = Source.BPayQld;
                this.BPayACT = Source.BPayACT;
                this.BPayNT = Source.BPayNT;
                this.BPayWA = Source.BPayWA;
                this.BPaySA = Source.BPaySA;
                this.BPayTas = Source.BPayTas;
                this.DDVic = Source.DDVic;
                this.DDNSW = Source.DDNSW;
                this.DDQld = Source.DDQld;
                this.DDACT = Source.DDACT;
                this.DDNT = Source.DDNT;
                this.DDWA = Source.DDWA;
                this.DDSA = Source.DDSA;
                this.DDTas = Source.DDTas;
                this.ResInspMonthStartValue = Source.ResInspMonthStartValue;
                this.InspectionAdvanceValue = Source.InspectionAdvanceValue;
                this.RentReviewInitialAdvanceValue = Source.RentReviewInitialAdvanceValue;
                this.RentReviewAdvanceValue = Source.RentReviewAdvanceValue;
                this.CreateLeaseExpiry = Source.CreateLeaseExpiry;
                this.CreateResVacate = Source.CreateResVacate;
                this.LettingFeeTimes = Source.LettingFeeTimes;
                this.LettingFeePercentage = Source.LettingFeePercentage;
                this.LettingFeeOptionValue = Source.LettingFeeOptionValue;
                this.LettingFeeAccountIdValue = Source.LettingFeeAccountIdValue;
                this.AdvertisingAccountIdValue = Source.AdvertisingAccountIdValue;
                this.EndOfYearStatement = Source.EndOfYearStatement;
                this.SendEndOfYearStmtToValue = Source.SendEndOfYearStmtToValue;
                this.EndOfYearStmtFee = Source.EndOfYearStmtFee;
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
            
                    SaveResidentialBondsRequestItem TempItem = new SaveResidentialBondsRequestItem();
                    //Result = CommonMethods.LoadSaveResidentialBondsRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSaveResidentialBondsRequestItem(ActionBase, ref TempItem);
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
