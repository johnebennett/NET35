/*
 * Created by SharpDevelop.
 * User: John
 * Date: 26/09/2019
 * Time: 4:57 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.IO;

namespace CommonUtilities
{
	/// <summary>
	/// Description of FolderHelpers.
	/// </summary>
	public class FolderHelpers
	{
		public FolderHelpers()
		{
		}
		
		public static string GetAssemblyDirectoryName(Assembly AssemblyName)
        {
            string Result = new Uri(AssemblyName.GetName().CodeBase).LocalPath;
            Result = Path.GetDirectoryName(Result);
            string PathSep = @"\";
            if (!Result.EndsWith(PathSep)) Result += PathSep;
            return Result;
        }
    }
}
