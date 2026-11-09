/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 07-09-2020
 * Time: 4:24 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/Portfolio/Actions/Templates/Creditor/AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.Creditor
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
        
        public string Date5 { get; set; }
        
        public string Date6 { get; set; }
        
        public string Date7 { get; set; }
        
        public string Date8 { get; set; }
        
        public string Date9 { get; set; }
        
        public string Date10 { get; set; }
        
        public string Date11 { get; set; }
        
        public string Date12 { get; set; }
        
        public string Date13 { get; set; }
        
        public string Date14 { get; set; }
        
        public string Date15 { get; set; }
        
        public string Date16 { get; set; }
        
        public string Date17 { get; set; }
        
        public string Date18 { get; set; }
        
        public string Date19 { get; set; }
        
        public string Date20 { get; set; }
        
        public string Date21 { get; set; }
        
        public string Date22 { get; set; }
        
        public string Date23 { get; set; }
        
        public string Date24 { get; set; }
        
        public string Text1 { get; set; }
        
        public string Text2 { get; set; }
        
        public string Text3 { get; set; }
        
        public string Chk1 { get; set; }
        
        public string Chk2 { get; set; }
        
        public string Chk3 { get; set; }
        
        public string Chk4 { get; set; }
        
        public string Chk5 { get; set; }
        
        public string Chk6 { get; set; }
        
        public string Chk7 { get; set; }
        
        public string Chk8 { get; set; }
        
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
        
        private string FCombo3IdValue = "";
        public bool Combo3IdIsValid = false;
        public int Combo3Id { get; set; }
        public string Combo3IdValue
        {
            get { return FCombo3IdValue; }
            set
            {
                FCombo3IdValue = value;
                int Dummy = 0;
                Combo3IdIsValid = int.TryParse(FCombo3IdValue, out Dummy);
                Combo3Id = (Combo3IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FCombo4IdValue = "";
        public bool Combo4IdIsValid = false;
        public int Combo4Id { get; set; }
        public string Combo4IdValue
        {
            get { return FCombo4IdValue; }
            set
            {
                FCombo4IdValue = value;
                int Dummy = 0;
                Combo4IdIsValid = int.TryParse(FCombo4IdValue, out Dummy);
                Combo4Id = (Combo4IdIsValid) ? Dummy : 0;
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
            Chk1 = "";
            Chk2 = "";
            Chk3 = "";
            Chk4 = "";
            Chk5 = "";
            Chk6 = "";
            Chk7 = "";
            Chk8 = "";
            Combo1IdValue = "";
            Combo2IdValue = "";
            Combo3IdValue = "";
            Combo4IdValue = "";
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
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Chk5";
                    if (string.IsNullOrEmpty(Chk5)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Chk6";
                    if (string.IsNullOrEmpty(Chk6)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Chk7";
                    if (string.IsNullOrEmpty(Chk7)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Chk8";
                    if (string.IsNullOrEmpty(Chk8)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                    Key = "Combo3Id";
                    if (!Combo3IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Combo4Id";
                    if (!Combo4IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
                this.Date5 = Source.Date5;
                this.Date6 = Source.Date6;
                this.Date7 = Source.Date7;
                this.Date8 = Source.Date8;
                this.Date9 = Source.Date9;
                this.Date10 = Source.Date10;
                this.Date11 = Source.Date11;
                this.Date12 = Source.Date12;
                this.Date13 = Source.Date13;
                this.Date14 = Source.Date14;
                this.Date15 = Source.Date15;
                this.Date16 = Source.Date16;
                this.Date17 = Source.Date17;
                this.Date18 = Source.Date18;
                this.Date19 = Source.Date19;
                this.Date20 = Source.Date20;
                this.Date21 = Source.Date21;
                this.Date22 = Source.Date22;
                this.Date23 = Source.Date23;
                this.Date24 = Source.Date24;
                this.Text1 = Source.Text1;
                this.Text2 = Source.Text2;
                this.Text3 = Source.Text3;
                this.Chk1 = Source.Chk1;
                this.Chk2 = Source.Chk2;
                this.Chk3 = Source.Chk3;
                this.Chk4 = Source.Chk4;
                this.Chk5 = Source.Chk5;
                this.Chk6 = Source.Chk6;
                this.Chk7 = Source.Chk7;
                this.Chk8 = Source.Chk8;
                this.Combo1IdValue = Source.Combo1IdValue;
                this.Combo2IdValue = Source.Combo2IdValue;
                this.Combo3IdValue = Source.Combo3IdValue;
                this.Combo4IdValue = Source.Combo4IdValue;
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
