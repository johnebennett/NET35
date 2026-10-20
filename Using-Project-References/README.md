There seems to be no .NET 3.5 - < .NET 4.8 specific repository that contains all of the source for the code used on the servers

I have taken a copy of the assemblies and executables from the EAOfficeService folder on the server and had Codex decompile them
and then to find the exact / best / closest project(s) on my development machine that match these EAOfficeService files - see Using-Assembly-References at this branch root.

This project is to recreate the assemblies / API / Web Server / Modules and Helper apps using project references instead of assembly references.
This will be an advantage migrating forward as there will be a direct comparison between these projects and the .NET 10 versions.

The main obstacle to overcome for this project to commence is to remove the circular references in the core set of assemblies.
This has already been done for the .NET 10 version but the .NET 10 CoreAssemblies have additional logging that the .NET 3.5 version does not.
The new logging should **NOT** be added to this version of the assemblies as it will make the versions too different to work on in the immediate future.
Even though there will probably be an easier way to resolve the circular references they should be achieved by making the code as close as possible to that of the .NET 10 version.

The hope is that Codex can be used to perform a lot of the repetitive work while ensuring that the code remains as close to that in the Using-Assembly-References branch as possible.
