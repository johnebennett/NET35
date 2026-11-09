/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 24-10-2023
 * Time: 8:31 PM

 * SourceFile: E:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\Property\Actions\Templates\AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.Property
{
    public class AllPageEntriesRequestItem
    {
        #region Properties - Getters and Setters
        public string IsAdd { get; set; }
        
        private string FMgmtIdValue = "";
        public bool MgmtIdIsValid = false;
        public int MgmtId { get; set; }
        public string MgmtIdValue
        {
            get { return FMgmtIdValue; }
            set
            {
                FMgmtIdValue = value;
                int Dummy = 0;
                MgmtIdIsValid = int.TryParse(FMgmtIdValue, out Dummy);
                MgmtId = (MgmtIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FNotesIdValue = "";
        public bool NotesIdIsValid = false;
        public int NotesId { get; set; }
        public string NotesIdValue
        {
            get { return FNotesIdValue; }
            set
            {
                FNotesIdValue = value;
                int Dummy = 0;
                NotesIdIsValid = int.TryParse(FNotesIdValue, out Dummy);
                NotesId = (NotesIdIsValid) ? Dummy : 0;
            }
        }
        
        public string BldgName { get; set; }
        
        public string SmokeAlarm { get; set; }
        
        public string SmokeAlarmDate { get; set; }
        
        private string FBedroomsValue = "";
        public bool BedroomsIsValid = false;
        public int Bedrooms { get; set; }
        public string BedroomsValue
        {
            get { return FBedroomsValue; }
            set
            {
                FBedroomsValue = value;
                int Dummy = 0;
                BedroomsIsValid = int.TryParse(FBedroomsValue, out Dummy);
                Bedrooms = (BedroomsIsValid) ? Dummy : 0;
            }
        }
        
        public string LevelNo { get; set; }
        
        public string LevelExtra { get; set; }
        
        public string Heater { get; set; }
        
        public string HeaterDate { get; set; }
        
        private string FBathroomsValue = "";
        public bool BathroomsIsValid = false;
        public int Bathrooms { get; set; }
        public string BathroomsValue
        {
            get { return FBathroomsValue; }
            set
            {
                FBathroomsValue = value;
                int Dummy = 0;
                BathroomsIsValid = int.TryParse(FBathroomsValue, out Dummy);
                Bathrooms = (BathroomsIsValid) ? Dummy : 0;
            }
        }
        
        public string UnitNo { get; set; }
        
        public string UnitExtra { get; set; }
        
        public string AirCon { get; set; }
        
        public string AirconServiceDate { get; set; }
        
        public string StreetNo { get; set; }
        
        public string StreetNoExtra { get; set; }
        
        public string PoolCert { get; set; }
        
        public string PoolCertDate { get; set; }
        
        private string FCarSpacesValue = "";
        public bool CarSpacesIsValid = false;
        public int CarSpaces { get; set; }
        public string CarSpacesValue
        {
            get { return FCarSpacesValue; }
            set
            {
                FCarSpacesValue = value;
                int Dummy = 0;
                CarSpacesIsValid = int.TryParse(FCarSpacesValue, out Dummy);
                CarSpaces = (CarSpacesIsValid) ? Dummy : 0;
            }
        }
        
        public string StreetPOBox { get; set; }
        
        public string FireService { get; set; }
        
        public string FireServiceDate { get; set; }
        
        private string FGarageValue = "";
        public bool GarageIsValid = false;
        public int Garage { get; set; }
        public string GarageValue
        {
            get { return FGarageValue; }
            set
            {
                FGarageValue = value;
                int Dummy = 0;
                GarageIsValid = int.TryParse(FGarageValue, out Dummy);
                Garage = (GarageIsValid) ? Dummy : 0;
            }
        }
        
        public string Suburb { get; set; }
        
        public string Gas { get; set; }
        
        public string GasDate { get; set; }
        
        private string FCarPortValue = "";
        public bool CarPortIsValid = false;
        public int CarPort { get; set; }
        public string CarPortValue
        {
            get { return FCarPortValue; }
            set
            {
                FCarPortValue = value;
                int Dummy = 0;
                CarPortIsValid = int.TryParse(FCarPortValue, out Dummy);
                CarPort = (CarPortIsValid) ? Dummy : 0;
            }
        }
        
        public string State { get; set; }
        
        public string Electrical { get; set; }
        
        public string ElectricalDate { get; set; }
        
        public string PostCode { get; set; }
        
        public string Flue { get; set; }
        
        public string FlueDate { get; set; }
        
        private string FBldgAreaValue = "";
        public bool BldgAreaIsValid = false;
        public double BldgArea { get; set; }
        public string BldgAreaValue
        {
            get { return FBldgAreaValue; }
            set
            {
                FBldgAreaValue = value;
                double Dummy = 0.0;
                BldgAreaIsValid = Double.TryParse(FBldgAreaValue, out Dummy);
                BldgArea = (BldgAreaIsValid) ? Dummy : 0;
            }
        }
        
        public string Grease { get; set; }
        
        public string GreaseDate { get; set; }
        
        private string FLandAreaValue = "";
        public bool LandAreaIsValid = false;
        public double LandArea { get; set; }
        public string LandAreaValue
        {
            get { return FLandAreaValue; }
            set
            {
                FLandAreaValue = value;
                double Dummy = 0.0;
                LandAreaIsValid = Double.TryParse(FLandAreaValue, out Dummy);
                LandArea = (LandAreaIsValid) ? Dummy : 0;
            }
        }
        
        public string AuthorityDate { get; set; }
        
        public string HotPlate { get; set; }
        
        private string FPropertyTypeValue = "";
        public bool PropertyTypeIsValid = false;
        public int PropertyType { get; set; }
        public string PropertyTypeValue
        {
            get { return FPropertyTypeValue; }
            set
            {
                FPropertyTypeValue = value;
                int Dummy = 0;
                PropertyTypeIsValid = int.TryParse(FPropertyTypeValue, out Dummy);
                PropertyType = (PropertyTypeIsValid) ? Dummy : 0;
            }
        }
        
        public string NoteText { get; set; }
        
        public string OwnerToAttendInspection { get; set; }
        
        public string Council { get; set; }
        
        public string SearchKey { get; set; }
        
        public string SmokeComment { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            IsAdd = "";
            MgmtIdValue = "";
            NotesIdValue = "";
            SmokeAlarm = "";
            SmokeAlarmDate = "";
            BedroomsValue = "";
            Heater = "";
            HeaterDate = "";
            BathroomsValue = "";
            AirCon = "";
            AirconServiceDate = "";
            PoolCert = "";
            PoolCertDate = "";
            CarSpacesValue = "";
            FireService = "";
            FireServiceDate = "";
            GarageValue = "";
            Gas = "";
            GasDate = "";
            CarPortValue = "";
            Electrical = "";
            ElectricalDate = "";
            Flue = "";
            FlueDate = "";
            BldgAreaValue = "";
            Grease = "";
            GreaseDate = "";
            LandAreaValue = "";
            AuthorityDate = "";
            HotPlate = "";
            PropertyTypeValue = "";
            OwnerToAttendInspection = "";
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
                    Key = "MgmtId";
                    if (!MgmtIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "NotesId";
                    if (!NotesIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SmokeAlarm";
                    if (string.IsNullOrEmpty(SmokeAlarm)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SmokeAlarmDate";
                    if (string.IsNullOrEmpty(SmokeAlarmDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Bedrooms";
                    if (!BedroomsIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Heater";
                    if (string.IsNullOrEmpty(Heater)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "HeaterDate";
                    if (string.IsNullOrEmpty(HeaterDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Bathrooms";
                    if (!BathroomsIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AirCon";
                    if (string.IsNullOrEmpty(AirCon)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AirconServiceDate";
                    if (string.IsNullOrEmpty(AirconServiceDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PoolCert";
                    if (string.IsNullOrEmpty(PoolCert)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PoolCertDate";
                    if (string.IsNullOrEmpty(PoolCertDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CarSpaces";
                    if (!CarSpacesIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FireService";
                    if (string.IsNullOrEmpty(FireService)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FireServiceDate";
                    if (string.IsNullOrEmpty(FireServiceDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Garage";
                    if (!GarageIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Gas";
                    if (string.IsNullOrEmpty(Gas)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GasDate";
                    if (string.IsNullOrEmpty(GasDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CarPort";
                    if (!CarPortIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Electrical";
                    if (string.IsNullOrEmpty(Electrical)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ElectricalDate";
                    if (string.IsNullOrEmpty(ElectricalDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Flue";
                    if (string.IsNullOrEmpty(Flue)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FlueDate";
                    if (string.IsNullOrEmpty(FlueDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BldgArea";
                    if (!BldgAreaIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Grease";
                    if (string.IsNullOrEmpty(Grease)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GreaseDate";
                    if (string.IsNullOrEmpty(GreaseDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "LandArea";
                    if (!LandAreaIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AuthorityDate";
                    if (string.IsNullOrEmpty(AuthorityDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "HotPlate";
                    if (string.IsNullOrEmpty(HotPlate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PropertyType";
                    if (!PropertyTypeIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OwnerToAttendInspection";
                    if (string.IsNullOrEmpty(OwnerToAttendInspection)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.MgmtIdValue = Source.MgmtIdValue;
                this.NotesIdValue = Source.NotesIdValue;
                this.BldgName = Source.BldgName;
                this.SmokeAlarm = Source.SmokeAlarm;
                this.SmokeAlarmDate = Source.SmokeAlarmDate;
                this.BedroomsValue = Source.BedroomsValue;
                this.LevelNo = Source.LevelNo;
                this.LevelExtra = Source.LevelExtra;
                this.Heater = Source.Heater;
                this.HeaterDate = Source.HeaterDate;
                this.BathroomsValue = Source.BathroomsValue;
                this.UnitNo = Source.UnitNo;
                this.UnitExtra = Source.UnitExtra;
                this.AirCon = Source.AirCon;
                this.AirconServiceDate = Source.AirconServiceDate;
                this.StreetNo = Source.StreetNo;
                this.StreetNoExtra = Source.StreetNoExtra;
                this.PoolCert = Source.PoolCert;
                this.PoolCertDate = Source.PoolCertDate;
                this.CarSpacesValue = Source.CarSpacesValue;
                this.StreetPOBox = Source.StreetPOBox;
                this.FireService = Source.FireService;
                this.FireServiceDate = Source.FireServiceDate;
                this.GarageValue = Source.GarageValue;
                this.Suburb = Source.Suburb;
                this.Gas = Source.Gas;
                this.GasDate = Source.GasDate;
                this.CarPortValue = Source.CarPortValue;
                this.State = Source.State;
                this.Electrical = Source.Electrical;
                this.ElectricalDate = Source.ElectricalDate;
                this.PostCode = Source.PostCode;
                this.Flue = Source.Flue;
                this.FlueDate = Source.FlueDate;
                this.BldgAreaValue = Source.BldgAreaValue;
                this.Grease = Source.Grease;
                this.GreaseDate = Source.GreaseDate;
                this.LandAreaValue = Source.LandAreaValue;
                this.AuthorityDate = Source.AuthorityDate;
                this.HotPlate = Source.HotPlate;
                this.PropertyTypeValue = Source.PropertyTypeValue;
                this.NoteText = Source.NoteText;
                this.OwnerToAttendInspection = Source.OwnerToAttendInspection;
                this.Council = Source.Council;
                this.SearchKey = Source.SearchKey;
                this.SmokeComment = Source.SmokeComment;
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
