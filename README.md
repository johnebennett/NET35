# NET35
This is the current source that makes up the .NET 3.5 version of GOAT, its web server, API, web application including Modules and the Helper Apps that form the services launched from the API and emailing

There will be 2 main sections to this repository.
The first is rebuilding the source as it currently exists on the web server which includes the use of the assemblies
- ActionBase
- UsefulDBMethods
- CommonUtilties
- ProgressLogging
- EAOfficeAPI and
- RequestProcessor

Due to circular references in these assemblies the second section cannot be implemented.
The design should be to remove the circular references by redesigning the assemblies to match, as close as possible
the CoreAssemblies used in the .NET 10 repository.
At this point the project references to these projects can be swapped with the assembly references in all the source going forward.

With this in mind there will be 2 main folders under this repository
One Using-Assembly-References for the existing (first) branch and one Using-Project-References
