using System;
using System.IO;
using System.Data;
/*
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
*/
namespace Base64EncoderDecoder
{
    public class Decoder
    {
        public Decoder(ConfigItems ConfigItems)
        {
        	this.m_ConfigItems = ConfigItems;
        }
        
        public ConfigItems m_ConfigItems;

        public void ExecuteF2F(string Src, string Dest)
        {
            CommonMethods.WriteToOutput("", "Inside Decoder.Execute");
            Console.WriteLine("Inside Decoder.ExecuteF2F");
            string base64 = File.ReadAllText(Src);
            byte[] bytes = Convert.FromBase64String(base64);
            File.WriteAllBytes(Dest, bytes);
            return;
        }

        public void ExecuteD2F(CommandLineParams Params)
        {
            CommonMethods.WriteToOutput("", "Inside Decoder.ExecuteD2F");
            string QueryText =
                "SELECT " + Params.SrcFieldName + " FROM " + Params.SrcTableName + " " + Params.SrcWhereClause;

            DataTable tbl = null;
            object Obj = null;
            string Result = "";
            byte[] data = new byte[] { };
            tbl =
                UsefulDBMethods.Helpers.GetFirstDataTableEx(
                    Params.ConnectionString, QueryText, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
            if (string.IsNullOrEmpty(Result))
            {
                foreach (DataRow Row in tbl.Rows)
                {
                    Obj = Row[Params.SrcFieldName];
                    if (Obj != DBNull.Value) data = (byte[])Obj;
                }
            }
            //File.WriteAllBytes(Params.DestFile + ".txt", data);
            string Base64Str = System.Text.Encoding.Default.GetString(data);
            data = Convert.FromBase64String(Base64Str);
            File.WriteAllBytes(Params.DestFile, data);
            return;
        }
    }
}
