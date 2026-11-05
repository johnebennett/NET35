using System;
using System.Collections.Generic;
using System.Text;
using CommonUtilities;
namespace UsefulDBMethods
{
 public static class EventViewerLogging
 {
  public static string WriteRecord(string ConnectionString, string Level, string Source, int EventId, string Message)
  {
   string Result = "";
int NewRecordId = 
					UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
						ConnectionString, "EVENT_VIEWER_ID_GEN", 0, 0, out Result);
			if (!string.IsNullOrEmpty(Result))
			{
				Result =
					"Error retrieving the next Generator Id for the LogEventViewerRecord" + Environment.NewLine +
					Result;
			}
			else if (NewRecordId <= 0)
			{
				Result = 
					"Error generating the next record Id in LogEventViewerRecord" +
					Environment.NewLine + Result;
			}
			
			string QueryText = "";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			if (string.IsNullOrEmpty(Result))
			{
				QueryText = 
					" INSERT INTO EVENT_VIEWER (Id, Level, DateAdded, TimeAdded, Source, EventId, Content)" +
					" VALUES (@Id, @Level, @DateAdded, @TimeAdded, @Source, @EventId, @Content)";
				
				DateTime CurrentDateTime = DateTime.Now;
				string CurrentDate = CurrentDateTime.ToString("yyyyMMdd");
				string CurrentTime = CurrentDateTime.ToString("HHmmss");
				Params.Clear();
				Params.Add(new FbParameterItem("Id", NewRecordId));
	
				Params.Add(new FbParameterItem("Level", Level));
				Params.Add(new FbParameterItem("DateAdded", CurrentDate));
				Params.Add(new FbParameterItem("TimeAdded", CurrentTime));
				Params.Add(new FbParameterItem("Source", Source));
				Params.Add(new FbParameterItem("EventId", EventId));
	
				byte[] bytes = Encoding.ASCII.GetBytes(Message);
				Params.Add(new FbParameterItem("Content", bytes));
				//Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "ProgressLogging", EventId);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			}
			
			return Result;
  }
 }
}
