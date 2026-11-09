using System;
using System.IO;
//using System.Data;
using System.Collections.Generic;
using System.Text;

//using Newtonsoft.Json.Linq;
using CommonUtilities;
//using EAOfficeAPI;
namespace Base64EncoderDecoder
{
    public class Encoder
    {
        public Encoder()
        {
        }
        
        public Encoder(ConfigItems ConfigItems)
        {
        	this.m_ConfigItems = ConfigItems;
        }
        
        public ConfigItems m_ConfigItems;
        
        public void ExecuteF2F(string Src, string Dest)
        {
            CommonMethods.WriteToOutput("", "Inside Encoder.ExecuteF2F");
            byte[] data = File.ReadAllBytes(Src);
            // ... Convert byte array to Base64 string.
            string result = Convert.ToBase64String(data);
            File.WriteAllText(Dest, result);
            return;
        }

        public void ExecuteF2D(CommandLineParams Params)
        {
            CommonMethods.WriteToOutput("", "Inside Encoder.ExecuteF2D");
            byte[] data = File.ReadAllBytes(Params.SrcFile);
            // ... Convert byte array to Base64 string.
            string result = Convert.ToBase64String(data);
            data = Encoding.ASCII.GetBytes(result);
            string QueryText =
                "UPDATE " + Params.DestTableName +
                " SET " + Params.DestFieldName + " = @BlobValue " + Params.DestWhereClause;
            List<FbParameterItem> DBParams = new List<FbParameterItem>();
            DBParams.Add(new FbParameterItem("BlobValue", data));
            //UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, DBParams, "Base64EncoderDecoder", 0);
            string StrResult = FirebirdUtilities.DBExecute(Params.ConnectionString, QueryText, DBParams, 0);
            if (!string.IsNullOrEmpty(StrResult)) CommonMethods.WriteToErrorOutput("Error: " + Environment.NewLine + StrResult);
            return;
        }
    }
}
