/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 23-06-2023
 * Time: 11:25 AM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/Actions/Templates/Stocklist/DisplayGridReportRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.Stocklist
{
    public class DisplayGridReportRequestItem
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
        
        public string Date5 { get; set; }
        
        public string Text1 { get; set; }
        
        public string Text2 { get; set; }
        
        public string Text3 { get; set; }
        
        public string Text4 { get; set; }
        
        public string Text5 { get; set; }
        
        public string Text6 { get; set; }
        
        public string Text7 { get; set; }
        
        public string Text8 { get; set; }
        
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
        
        public string Chk1 { get; set; }
        
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
        
        private string FUniqueIdValue = "";
        public bool UniqueIdIsValid = false;
        public int UniqueId { get; set; }
        public string UniqueIdValue
        {
            get { return FUniqueIdValue; }
            set
            {
                FUniqueIdValue = value;
                int Dummy = 0;
                UniqueIdIsValid = int.TryParse(FUniqueIdValue, out Dummy);
                UniqueId = (UniqueIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FButtonIdValue = "";
        public bool ButtonIdIsValid = false;
        public int ButtonId { get; set; }
        public string ButtonIdValue
        {
            get { return FButtonIdValue; }
            set
            {
                FButtonIdValue = value;
                int Dummy = 0;
                ButtonIdIsValid = int.TryParse(FButtonIdValue, out Dummy);
                ButtonId = (ButtonIdIsValid) ? Dummy : 0;
            }
        }
        
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
            Chk1 = "";
            Combo2IdValue = "";
            UniqueIdValue = "";
            ButtonIdValue = "";
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
                    Key = "Chk1";
                    if (string.IsNullOrEmpty(Chk1)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Combo2Id";
                    if (!Combo2IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "UniqueId";
                    if (!UniqueIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ButtonId";
                    if (!ButtonIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(DisplayGridReportRequestItem Source)
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
                this.Date5 = Source.Date5;
                this.Text1 = Source.Text1;
                this.Text2 = Source.Text2;
                this.Text3 = Source.Text3;
                this.Text4 = Source.Text4;
                this.Text5 = Source.Text5;
                this.Text6 = Source.Text6;
                this.Text7 = Source.Text7;
                this.Text8 = Source.Text8;
                this.Amount1Value = Source.Amount1Value;
                this.Amount2Value = Source.Amount2Value;
                this.Amount3Value = Source.Amount3Value;
                this.Amount4Value = Source.Amount4Value;
                this.Amount5Value = Source.Amount5Value;
                this.Amount6Value = Source.Amount6Value;
                this.Chk1 = Source.Chk1;
                this.Combo2IdValue = Source.Combo2IdValue;
                this.UniqueIdValue = Source.UniqueIdValue;
                this.ButtonIdValue = Source.ButtonIdValue;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
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
            
                    DisplayGridReportRequestItem TempItem = new DisplayGridReportRequestItem();
                    //Result = CommonMethods.LoadDisplayGridReportRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadDisplayGridReportRequestItem(ActionBase, ref TempItem);
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
