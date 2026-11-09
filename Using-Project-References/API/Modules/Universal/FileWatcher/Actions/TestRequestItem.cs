/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 07-10-2024
 * Time: 5:31 PM

 * SourceFile: E:\Source\EAOffice\EAOfficeAPI\Modules\Universal\FileWatcher\Actions\Templates\Test\TestRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.FileWatcher
{
    public class TestRequestItem
    {
        #region Properties - Getters and Setters
        public string TestString { get; set; }
        
        public string SenderName { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            TestString = "";
            SenderName = "";
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
                    Key = "TestString";
                    if (string.IsNullOrEmpty(TestString)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "SenderName";
                    if (string.IsNullOrEmpty(SenderName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(TestRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.TestString = Source.TestString;
                this.SenderName = Source.SenderName;
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
            
                    TestRequestItem TempItem = new TestRequestItem();
                    //Result = CommonMethods.LoadTestRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadTestRequestItem(ActionBase, ref TempItem);
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
