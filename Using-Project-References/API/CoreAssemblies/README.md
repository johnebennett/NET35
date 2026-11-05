# Purpose of this document
This folder is the current source for the assemblies that are used in the majority of Modules and console applications used by the API

This set of assemblies forms the CoreAssemblies of GOAT

The source comes from the EAOfficeAPI repository on GitHub.

The purpose here is to get the source for these assemblies to build from the current folder structure:
```
+---ActionBase
+---CommonSourceFiles
+---CommonUtilities
+---ProgressLogging
+---RequestProcessor
+---UsefulDBMethods
```
whereas the existing repository folder structure is
```
+---CommonSourceFiles
+---Projects
|   +---ActionBase
|   +---CommonUtilities
|   +---ProgressLogging
+---RequestProcessor
+---Modules
|   +---UsefulDBMethods
```
The first thing to consider is the files in the CommonSourceFiles folder

A number of the files in this folder are referenced in several other projects and they probably should not be.

The files in the CommonSourceFiles folder are:
- ActionBase.cs
- APISettingsConfig.cs
- CommonDBMethods.cs
- JsonResponseContent.cs
- ProgressLogging.cs
- RequestCommon.cs
- SqliteLogging.cs

## Project ActionBase

References:
===========
- CommonUtilities.dll
- ProgressLogging.dll

Includes:
=========
```
\CommonSourceFiles\CommonDBMethods.cs
\CommonSourceFiles\RequestCommon.cs
```
## Project CommonUtilities

References:
===========
- ActionBase.dll
- UsefulDBMethods.dll

Includes:
=========
**NONE**

## Project ProgressLogging

References:
===========
- ActionBase.dll
- CommonUtilities.dll
- UsefulDBMethods.dll

## Project RequestProcessor

References:
===========
- CommonUtilities.dll

## Project UsefulDBMethods

References:
===========
- CommonUtilities.dll

The only project that uses any of the files in the CommonSourceFiles folder is **ActionBase**
Investigate if the 2 files can be added to the project and just eliminate the CommonSourceFiles folder altogether
