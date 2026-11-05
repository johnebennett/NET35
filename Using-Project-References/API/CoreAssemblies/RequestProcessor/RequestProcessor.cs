/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 10/10/2018
 * Time: 11:21 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Reflection;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonUtilities;

namespace EAOfficeAPI
{
	/// <summary>
	/// This is the main unit for processing the requests that
	/// come into the API.
	/// </summary>
	public class RequestProcessor
	{		
        private static void WriteInfoToEventLog(string Message)
        {
            string lResult = CommonUtilities.WindowsEventLoggingUtilities.WriteToInfoEvent(Message, "EAOfficeAPI_INFO", 0);
            //Console.WriteLine(lResult);
        }

        public static string Execute(string Request)
        {
        	WriteInfoToEventLog("Inside RequestProcessor.Execute" + Environment.NewLine + Request);
            bool executeDefaultMethod = true;

            if (executeDefaultMethod == true)
            {
                // FXS When loading a Module via this method cannot step through the Module and debug (due to DLL being loaded dynamically and then the appropriate method invoked)
                return ExecuteDefaultMethod(Request);
            }
            else
            {
                // Alternate method to explicity call Execute method of a module. Can step into Module code
                return ExecuteRequestWithAssemblyReference(Request);
            }
        }

        public static string ExecuteDefaultMethod(string Request)
        {
            // FXS When loading a Module view this method cannot step through the Module
            string Result = "";
            string ErrorMsg = "";
            //string assembly = "";
            string plugin = "";
            string Version = "";
            string AddNamespace = "";

            var obj = JsonConvert.DeserializeObject<JObject>(Request);
            if (obj == null)
            {
                ErrorMsg = "Error deserializing the request.";
            }
            else
            {
                JObject requestObj = obj["common"] as JObject;
                if (requestObj == null)
                {
                    ErrorMsg = "Missing common element";
                }
                else
                {
                    //Object AObj = obj["common"]["assembly"];
                    Object AObj = obj["common"]["plugin"];
                    if (AObj == null)
                    {
                        ErrorMsg = "Missing assembly element.";
                    }
                    {
                        //assembly = (string)obj["common"]["assembly"];
                        plugin = (string)obj["common"]["plugin"];
                        Version = (string)obj["common"]["version"];
                    }
                }
                
                JObject DataObject = (obj["data"] as JObject);
                if (DataObject != null) 
                {
                	object AObj = obj["data"]["ns"];
                	if (AObj != null)
                		AddNamespace = (string)obj["data"]["ns"];
                }
            }

            if (ErrorMsg != "")
            {
            	//Console.WriteLine(ErrorMsg);
                return ErrorMsg;
            }

            string assemblyName = plugin;
            if (Version != "") assemblyName = assemblyName + "." + Version;
            assemblyName += ".dll";

            string AssemblyLocalPath = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            AssemblyLocalPath = Path.GetDirectoryName(AssemblyLocalPath);

            var fileInfo = new FileInfo(Path.Combine(AssemblyLocalPath, assemblyName));
            if (!fileInfo.Exists)
            {
            	//Console.WriteLine("Plugin does not exist." + " Name: " + assemblyName + " Dir: " + AssemblyLocalPath);
                return "Plugin does not exist." + " Name: " + assemblyName + " Dir: " + AssemblyLocalPath;
            }

            Assembly assembly = Assembly.LoadFrom(Path.Combine(AssemblyLocalPath, assemblyName));
            //Console.WriteLine("Plugin: " + plugin);

            // Cannot find a type directly without the namespace unless iterate through the assembly types using GetTypes
            Type type = assembly.GetType(plugin);
            if (type == null)
            {
                // Get Type from list of types in assembly
                Type[] assemblyTypes = assembly.GetTypes();
                string lassemblyTypeStr = "";
                for (int i = 0; i < assemblyTypes.Length; i++)
                {
                    lassemblyTypeStr += Environment.NewLine + assemblyTypes[i].Name;
                    //Console.WriteLine("Assembly Type: " + assemblyTypes[i].Name);

                    //if (assemblyTypes[i].Name.Equals(plugin))
                    if ((assemblyTypes[i].Name.Equals(plugin)) || (assemblyTypes[i].Name.Equals(AddNamespace)))
                    {
                        type = assemblyTypes[i];
                        break;
                    }
                }

                if (type == null)
                {
                	//Console.WriteLine("Type not present in the requested assembly");
                    return "Type not present in the requested assembly";
                }
            }

            MethodInfo methodInfo = type.GetMethod("Execute");
            if (methodInfo == null)
            {
                return "The method is not present in the requested assembly.";
            }

            ParameterInfo[] parameterInfo = methodInfo.GetParameters();
            //ParameterInfo parameterItem = null;
            /*
			 If we want to we can check the number of parameters in the parameterInfo array
			 and also that the order and types of the parameters are correct
			*/

            const int ExpectedNumberOfParameters = 1;

            if (parameterInfo.Length != ExpectedNumberOfParameters)
            {
                return "Method does not support the correct number of parameters.";
            }

            var sb = new StringBuilder();

            foreach (ParameterInfo parameterItem in parameterInfo)
            {
                switch (parameterItem.Position)
                {
                    case 0:
                        if (parameterItem.ParameterType.Name.ToLower() != "string")
                            sb.AppendLine("Parameter Type is not correct");
                        break;
                }
            }

            if (sb.Length > 0)
            {
                return sb.ToString();
            }

            var classInstance = Activator.CreateInstance(type, null);

            var parametersArray = new object[] { Request };

            Result = (string)methodInfo.Invoke(classInstance, parametersArray);

            return Result;
        }

        public static string ExecuteRequestWithAssemblyReference(string Request)
        {
            string Result = "";
            string ErrorMsg = "";
            string plugin = "";
            string Version = "";

            //JObject obj = JsonConvert.DeserializeObject<JObject>(Request);
            var Source = JsonConvert.DeserializeObject<JObject>(Request);
            if (Source == null)
            {
                ErrorMsg = "Error deserializing the request.";
            }
            else
            {
                JObject requestObj = Source["common"] as JObject;
                if (requestObj == null)
                {
                    ErrorMsg = "Missing common element";
                }
                else
                {
                    Object AObj = Source["common"]["assembly"];
                    if (AObj == null)
                    {
                        ErrorMsg = "Missing assembly element.";
                    }
                    {
                        plugin = (string)Source["common"]["assembly"];
                        Version = (string)Source["common"]["version"];
                    }
                }
            }

            if (ErrorMsg != "")
            {
                return ErrorMsg;
            }

            string assemblyName = plugin;
            if (Version != "") assemblyName = assemblyName + "." + Version;
            assemblyName += ".dll";

            string AssemblyLocalPath = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            AssemblyLocalPath = Path.GetDirectoryName(AssemblyLocalPath);

            var fileInfo = new FileInfo(Path.Combine(AssemblyLocalPath, assemblyName));
            if (!fileInfo.Exists)
            {
                return "Assembly does not exist." + " Name: " + assemblyName + " Dir: " + AssemblyLocalPath;
            }

            //string lModuleResult = "";

            //string lModuleResult = StrataPort.StrataPort.Execute(Request);

            //return lModuleResult;

            Assembly assembly = Assembly.LoadFrom(Path.Combine(AssemblyLocalPath, assemblyName));

            // Cannot find a type directly without the namespace unless iterate through the assembly types using GetTypes
            Type type = assembly.GetType(assemblyName);
            if (type == null)
            {
                // Get Type from list of types in assembly
                Type[] assemblyTypes = assembly.GetTypes();
                string lassemblyTypeStr = "";
                for (int i = 0; i < assemblyTypes.Length; i++)
                {
                    lassemblyTypeStr += Environment.NewLine + assemblyTypes[i].Name;

                    if (assemblyTypes[i].Name.Equals(assemblyName))
                    {
                        type = assemblyTypes[i];
                        break;
                    }
                }

                if (type == null)
                {
                    return "Type not present in the requested assembly";
                }
            }

            MethodInfo methodInfo = type.GetMethod("Execute");
            if (methodInfo == null)
            {
                return "The method is not present in the requested assembly.";
            }

            ParameterInfo[] parameterInfo = methodInfo.GetParameters();
            //ParameterInfo parameterItem = null;
            /*
             If we want to we can check the number of parameters in the parameterInfo array
             and also that the order and types of the parameters are correct
            */

            const int ExpectedNumberOfParameters = 1;

            if (parameterInfo.Length != ExpectedNumberOfParameters)
            {
                return "Method does not support the correct number of parameters.";
            }

            var sb = new StringBuilder();

            foreach (ParameterInfo parameterItem in parameterInfo)
            {
                switch (parameterItem.Position)
                {
                    case 0:
                        if (parameterItem.ParameterType.Name.ToLower() != "string")
                            sb.AppendLine("Parameter Type is not correct");
                        break;
                }
            }

            if (sb.Length > 0)
            {
                return sb.ToString();
            }

            var classInstance = Activator.CreateInstance(type, null);

            var parametersArray = new object[] { Request };

            Result = (string)methodInfo.Invoke(classInstance, parametersArray);

            return Result;
        }
    }
}