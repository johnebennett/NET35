There seems to be no .NET 3.5 - < .NET 4.8 specific repository that contains all of the source for the code used on the servers

I have taken a copy of the assemblies and executables from the EAOfficeService folder on the server and had Codex decompile them
and then to find the exact / best / closest project(s) on my development machine that match these EAOfficeService files.

This project is to recreate the assemblies / API / Web Server / Modules and Helper apps using the assembly references.
There will be another branch of these sources that use project references - see the Using-Project-References branch at this root.

Taking the assemblies and executables from the server and decompiling them using Codex a number of analysis files have been generated.
These Markdown files have been added here to indicate the process of determining the best candidate(s) of existing source to the objects
taken from the Web Server.
These Codex generated files form the basis only of the projects. As the projects are compared at a compiled (binary) level they are also 
inspected visually to make sure they cover the complete content of the objects from the web server.
Using tools like ilSpy and dnSpy as well as my own generated application
- GOAT\AssemblyComparer and
- GOAT\AssemblyExtractor
any gaps in the source should be able to be filled.
  
