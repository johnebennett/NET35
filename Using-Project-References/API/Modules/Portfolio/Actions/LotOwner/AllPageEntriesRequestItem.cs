/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 17-07-2020
 * Time: 5:56 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\Portfolio\Actions\Templates\LotOwner\AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.Portfolio.LotOwner
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
        
        public string Chk1 { get; set; }
        
        public string Chk2 { get; set; }
        
        public string Chk3 { get; set; }
        
        public string Chk4 { get; set; }
        
        public string Chk5 { get; set; }
        
        public string Chk6 { get; set; }
        
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
            Chk1 = "";
            Chk2 = "";
            Chk3 = "";
            Chk4 = "";
            Chk5 = "";
            Chk6 = "";
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
                this.Chk1 = Source.Chk1;
                this.Chk2 = Source.Chk2;
                this.Chk3 = Source.Chk3;
                this.Chk4 = Source.Chk4;
                this.Chk5 = Source.Chk5;
                this.Chk6 = Source.Chk6;
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
