/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 23-08-2022
 * Time: 6:29 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/Actions/Templates/Insurance/AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.Insurance
{
    public class AllPageEntriesRequestItem
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
        
        public string Header4Value { get; set; }
        
        private string FBranchIdValue = "";
        public bool BranchIdIsValid = false;
        public int BranchId { get; set; }
        public string BranchIdValue
        {
            get { return FBranchIdValue; }
            set
            {
                FBranchIdValue = value;
                int Dummy = 0;
                BranchIdIsValid = int.TryParse(FBranchIdValue, out Dummy);
                BranchId = (BranchIdIsValid) ? Dummy : 0;
            }
        }
        
        public string Property { get; set; }
        
        public string Header7Value { get; set; }
        
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
        
        public string Header3Value { get; set; }
        
        public string Header8Value { get; set; }
        
        public string NoteText { get; set; }
        
        public string Date1 { get; set; }
        
        public string Date2 { get; set; }
        
        public string Date3 { get; set; }
        
        public string Date4 { get; set; }
        
        private string FAmount1Value = "";
        public bool Amount1IsValid = false;
        public double Amount1 { get; set; }
        public string Amount1Value
        {
            get { return FAmount1Value; }
            set
            {
                FAmount1Value = value;
                double Dummy = 0.0;
                Amount1IsValid = Double.TryParse(FAmount1Value, out Dummy);
                Amount1 = (Amount1IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount2Value = "";
        public bool Amount2IsValid = false;
        public double Amount2 { get; set; }
        public string Amount2Value
        {
            get { return FAmount2Value; }
            set
            {
                FAmount2Value = value;
                double Dummy = 0.0;
                Amount2IsValid = Double.TryParse(FAmount2Value, out Dummy);
                Amount2 = (Amount2IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount3Value = "";
        public bool Amount3IsValid = false;
        public double Amount3 { get; set; }
        public string Amount3Value
        {
            get { return FAmount3Value; }
            set
            {
                FAmount3Value = value;
                double Dummy = 0.0;
                Amount3IsValid = Double.TryParse(FAmount3Value, out Dummy);
                Amount3 = (Amount3IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount4Value = "";
        public bool Amount4IsValid = false;
        public double Amount4 { get; set; }
        public string Amount4Value
        {
            get { return FAmount4Value; }
            set
            {
                FAmount4Value = value;
                double Dummy = 0.0;
                Amount4IsValid = Double.TryParse(FAmount4Value, out Dummy);
                Amount4 = (Amount4IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount5Value = "";
        public bool Amount5IsValid = false;
        public double Amount5 { get; set; }
        public string Amount5Value
        {
            get { return FAmount5Value; }
            set
            {
                FAmount5Value = value;
                double Dummy = 0.0;
                Amount5IsValid = Double.TryParse(FAmount5Value, out Dummy);
                Amount5 = (Amount5IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount6Value = "";
        public bool Amount6IsValid = false;
        public double Amount6 { get; set; }
        public string Amount6Value
        {
            get { return FAmount6Value; }
            set
            {
                FAmount6Value = value;
                double Dummy = 0.0;
                Amount6IsValid = Double.TryParse(FAmount6Value, out Dummy);
                Amount6 = (Amount6IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount7Value = "";
        public bool Amount7IsValid = false;
        public double Amount7 { get; set; }
        public string Amount7Value
        {
            get { return FAmount7Value; }
            set
            {
                FAmount7Value = value;
                double Dummy = 0.0;
                Amount7IsValid = Double.TryParse(FAmount7Value, out Dummy);
                Amount7 = (Amount7IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount8Value = "";
        public bool Amount8IsValid = false;
        public double Amount8 { get; set; }
        public string Amount8Value
        {
            get { return FAmount8Value; }
            set
            {
                FAmount8Value = value;
                double Dummy = 0.0;
                Amount8IsValid = Double.TryParse(FAmount8Value, out Dummy);
                Amount8 = (Amount8IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount9Value = "";
        public bool Amount9IsValid = false;
        public double Amount9 { get; set; }
        public string Amount9Value
        {
            get { return FAmount9Value; }
            set
            {
                FAmount9Value = value;
                double Dummy = 0.0;
                Amount9IsValid = Double.TryParse(FAmount9Value, out Dummy);
                Amount9 = (Amount9IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount10Value = "";
        public bool Amount10IsValid = false;
        public double Amount10 { get; set; }
        public string Amount10Value
        {
            get { return FAmount10Value; }
            set
            {
                FAmount10Value = value;
                double Dummy = 0.0;
                Amount10IsValid = Double.TryParse(FAmount10Value, out Dummy);
                Amount10 = (Amount10IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount11Value = "";
        public bool Amount11IsValid = false;
        public double Amount11 { get; set; }
        public string Amount11Value
        {
            get { return FAmount11Value; }
            set
            {
                FAmount11Value = value;
                double Dummy = 0.0;
                Amount11IsValid = Double.TryParse(FAmount11Value, out Dummy);
                Amount11 = (Amount11IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount12Value = "";
        public bool Amount12IsValid = false;
        public double Amount12 { get; set; }
        public string Amount12Value
        {
            get { return FAmount12Value; }
            set
            {
                FAmount12Value = value;
                double Dummy = 0.0;
                Amount12IsValid = Double.TryParse(FAmount12Value, out Dummy);
                Amount12 = (Amount12IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount13Value = "";
        public bool Amount13IsValid = false;
        public double Amount13 { get; set; }
        public string Amount13Value
        {
            get { return FAmount13Value; }
            set
            {
                FAmount13Value = value;
                double Dummy = 0.0;
                Amount13IsValid = Double.TryParse(FAmount13Value, out Dummy);
                Amount13 = (Amount13IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount14Value = "";
        public bool Amount14IsValid = false;
        public double Amount14 { get; set; }
        public string Amount14Value
        {
            get { return FAmount14Value; }
            set
            {
                FAmount14Value = value;
                double Dummy = 0.0;
                Amount14IsValid = Double.TryParse(FAmount14Value, out Dummy);
                Amount14 = (Amount14IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount15Value = "";
        public bool Amount15IsValid = false;
        public double Amount15 { get; set; }
        public string Amount15Value
        {
            get { return FAmount15Value; }
            set
            {
                FAmount15Value = value;
                double Dummy = 0.0;
                Amount15IsValid = Double.TryParse(FAmount15Value, out Dummy);
                Amount15 = (Amount15IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount16Value = "";
        public bool Amount16IsValid = false;
        public double Amount16 { get; set; }
        public string Amount16Value
        {
            get { return FAmount16Value; }
            set
            {
                FAmount16Value = value;
                double Dummy = 0.0;
                Amount16IsValid = Double.TryParse(FAmount16Value, out Dummy);
                Amount16 = (Amount16IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount17Value = "";
        public bool Amount17IsValid = false;
        public double Amount17 { get; set; }
        public string Amount17Value
        {
            get { return FAmount17Value; }
            set
            {
                FAmount17Value = value;
                double Dummy = 0.0;
                Amount17IsValid = Double.TryParse(FAmount17Value, out Dummy);
                Amount17 = (Amount17IsValid) ? Dummy : 0;
            }
        }
        
        private string FAmount18Value = "";
        public bool Amount18IsValid = false;
        public double Amount18 { get; set; }
        public string Amount18Value
        {
            get { return FAmount18Value; }
            set
            {
                FAmount18Value = value;
                double Dummy = 0.0;
                Amount18IsValid = Double.TryParse(FAmount18Value, out Dummy);
                Amount18 = (Amount18IsValid) ? Dummy : 0;
            }
        }
        
        public string Text1 { get; set; }
        
        public string Text2 { get; set; }
        
        public string Text3 { get; set; }
        
        public string Text4 { get; set; }
        
        private string FCombo1IdValue = "";
        public bool Combo1IdIsValid = false;
        public int Combo1Id { get; set; }
        public string Combo1IdValue
        {
            get { return FCombo1IdValue; }
            set
            {
                FCombo1IdValue = value;
                int Dummy = 0;
                Combo1IdIsValid = int.TryParse(FCombo1IdValue, out Dummy);
                Combo1Id = (Combo1IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FCombo2IdValue = "";
        public bool Combo2IdIsValid = false;
        public int Combo2Id { get; set; }
        public string Combo2IdValue
        {
            get { return FCombo2IdValue; }
            set
            {
                FCombo2IdValue = value;
                int Dummy = 0;
                Combo2IdIsValid = int.TryParse(FCombo2IdValue, out Dummy);
                Combo2Id = (Combo2IdIsValid) ? Dummy : 0;
            }
        }
        
        public string Chk1 { get; set; }
        
        public string Chk2 { get; set; }
        
        public string Chk3 { get; set; }
        
        public string Chk4 { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            NotesIdValue = "";
            Option1IdValue = "";
            Option2IdValue = "";
            TypeIdValue = "";
            BranchIdValue = "";
            Property = "";
            ManagerIdValue = "";
            Header3Value = "";
            Amount1Value = "";
            Amount2Value = "";
            Amount3Value = "";
            Amount4Value = "";
            Amount5Value = "";
            Amount6Value = "";
            Amount7Value = "";
            Amount8Value = "";
            Amount9Value = "";
            Amount10Value = "";
            Amount11Value = "";
            Amount12Value = "";
            Amount13Value = "";
            Amount14Value = "";
            Amount15Value = "";
            Amount16Value = "";
            Amount17Value = "";
            Amount18Value = "";
            Combo1IdValue = "";
            Combo2IdValue = "";
            Chk1 = "";
            Chk2 = "";
            Chk3 = "";
            Chk4 = "";
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
                    Key = "NotesId";
                    if (!NotesIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    Key = "TypeId";
                    if (!TypeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BranchId";
                    if (!BranchIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Property";
                    if (string.IsNullOrEmpty(Property)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ManagerId";
                    if (!ManagerIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Header3Value";
                    if (string.IsNullOrEmpty(Header3Value)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount1";
                    if (!Amount1IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount2";
                    if (!Amount2IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount3";
                    if (!Amount3IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount4";
                    if (!Amount4IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount5";
                    if (!Amount5IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount6";
                    if (!Amount6IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount7";
                    if (!Amount7IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount8";
                    if (!Amount8IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount9";
                    if (!Amount9IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount10";
                    if (!Amount10IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount11";
                    if (!Amount11IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount12";
                    if (!Amount12IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount13";
                    if (!Amount13IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount14";
                    if (!Amount14IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount15";
                    if (!Amount15IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount16";
                    if (!Amount16IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount17";
                    if (!Amount17IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Amount18";
                    if (!Amount18IsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Combo1Id";
                    if (!Combo1IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Combo2Id";
                    if (!Combo2IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Chk1";
                    if (string.IsNullOrEmpty(Chk1)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Chk2";
                    if (string.IsNullOrEmpty(Chk2)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Chk3";
                    if (string.IsNullOrEmpty(Chk3)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Chk4";
                    if (string.IsNullOrEmpty(Chk4)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.NotesIdValue = Source.NotesIdValue;
                this.Option1IdValue = Source.Option1IdValue;
                this.Option2IdValue = Source.Option2IdValue;
                this.TypeIdValue = Source.TypeIdValue;
                this.Header4Value = Source.Header4Value;
                this.BranchIdValue = Source.BranchIdValue;
                this.Property = Source.Property;
                this.Header7Value = Source.Header7Value;
                this.ManagerIdValue = Source.ManagerIdValue;
                this.Header3Value = Source.Header3Value;
                this.Header8Value = Source.Header8Value;
                this.NoteText = Source.NoteText;
                this.Date1 = Source.Date1;
                this.Date2 = Source.Date2;
                this.Date3 = Source.Date3;
                this.Date4 = Source.Date4;
                this.Amount1Value = Source.Amount1Value;
                this.Amount2Value = Source.Amount2Value;
                this.Amount3Value = Source.Amount3Value;
                this.Amount4Value = Source.Amount4Value;
                this.Amount5Value = Source.Amount5Value;
                this.Amount6Value = Source.Amount6Value;
                this.Amount7Value = Source.Amount7Value;
                this.Amount8Value = Source.Amount8Value;
                this.Amount9Value = Source.Amount9Value;
                this.Amount10Value = Source.Amount10Value;
                this.Amount11Value = Source.Amount11Value;
                this.Amount12Value = Source.Amount12Value;
                this.Amount13Value = Source.Amount13Value;
                this.Amount14Value = Source.Amount14Value;
                this.Amount15Value = Source.Amount15Value;
                this.Amount16Value = Source.Amount16Value;
                this.Amount17Value = Source.Amount17Value;
                this.Amount18Value = Source.Amount18Value;
                this.Text1 = Source.Text1;
                this.Text2 = Source.Text2;
                this.Text3 = Source.Text3;
                this.Text4 = Source.Text4;
                this.Combo1IdValue = Source.Combo1IdValue;
                this.Combo2IdValue = Source.Combo2IdValue;
                this.Chk1 = Source.Chk1;
                this.Chk2 = Source.Chk2;
                this.Chk3 = Source.Chk3;
                this.Chk4 = Source.Chk4;
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
