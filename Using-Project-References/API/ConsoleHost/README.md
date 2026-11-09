This project is the self hosted WCF service application that hosts the API.

Note that there are 3 csproj files in this project
The ConsoleHost.csproj is effectively the same as the ConsoleHost-Using-WebService.csproj
- these projects use the original WebService reference to build. This version links a shared file between this project and ActionBase
- having this link to the shared file means that if either places requires a change both have to be changed accordingly

The ConsoleHost-Using-WebService2.csproj replaces the shared linked file APISettingsConfig.cs with a project reference to the core assembly ActionBase
- Note that this version, when built will contain more files than the one that uses the share file due to the other projects linked via ActionBase
- The files required by ActionBase are the same as the ones already used by the rest of the API and Helper apps so there should be no issue

## Test the host application
The 2 versions of ConsoleHost buildable by these projects should be tested to make sure they produce the same results.
Using the ConsoleConsumer create whatever .json request files are needed to perform the testing and the output should be compared.
I don't see any reason why there should be any differences but you never know until you test!