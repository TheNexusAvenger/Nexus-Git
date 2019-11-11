# Building

## Prerequisites
Before starting, you should be familiar with
[Git](https://git-scm.com/) and already have
it installed. For building both the client and
the server, you will need to clone the repository.
To your file system. This can be done with the following:
```
$ git clone https://github.com/TheNexusAvenger/Nexus-Git
```

For building the client, you will need to have
[Rojo](https://github.com/rojo-rbx/rojo) for syncing
files into Roblox Studio, or Nexus Git already installed.
For building the server, you will need
[.NET Core SDK](https://dotnet.microsoft.com/download)
for the platform you are running. Version 3.0 or higher
is required with how it is set up. Optionally,
[Python](https://www.python.org/) can be installed for
running the script that compiles platform-specific builds
of Nexus Git and copies them to a central repository.

## Building the Client
The Nexus Git Client (plugin for Roblox Studio)
is avaliable for purchase from the Roblox Plugin
Marketplace, but can also be build from the repository.

1. Open a Roblox Studio session an enable the HttpService.
Whether using Rojo or Nexus Git, both require using the
HttpService to communicate with the client.

### Using Rojo
2. Enable Rojo by starting it in the command line.
```
$ rojo serve
```

3. Sync into Roblox Studio using either a one-time
sync in or contionous syncing using the Rojo Roblox
Studio plugin.

4. In Roblox Studio, right click on the root `ModuleScript`
named `NexusGit` in `ServerStorage` and either save it as
a model file or as a local plugin. It should be saved to your
`Roblox/plugins` directory. If it doesn't exist, create the
directory. The specific path is platform and install specific,
but for a default Winodws install, it is `%localappdata%/Roblox/plugins`.

### Using Nexus Git
2. Enable Nexus Git by starting it in the command line.
```
$ nexusgit serve
```

3. Sync into Roblox Studio using either a local pull
Nexus Git Roblox Studio plugin.

4. In Roblox Studio, right click on the root `ModuleScript`
named `NexusGit` in `ServerStorage` and either save it as
a model file or as a local plugin. It should be saved to your
`Roblox/plugins` directory. If it doesn't exist, create the
directory. The specific path is platform and install specific,
but for a default Winodws install, it is `%localappdata%/Roblox/plugins`.

## Building the Server
Having .NET Core installed is required, but Python
is optional. For this, there is 2 options to build.

### With Python
In the `server` directory, there is a script that generates
and moves the platform-exclusive executables to a single directory.
The script can be run with the following:
```
$ python server/Publisher.py compiler [platform]
```

Using a normal install with the path variables correct, the supported
platforms can be generated into `server/bin` with teh following:
```
$ python server/Publisher.py dotnet
```

If you don't have `dotnet` as a path variable, you can specify the full
path. For a normal Windows install, it would look like this.
```
$ python server/Publisher.py  C:/Program\ Files/dotnet/dotnet.exe
```

In case you need a specific platform, the platform can be specified. The
platforms supported by .NET Core in [corefx/runtime.json](https://github.com/dotnet/corefx/blob/master/src/pkg/Microsoft.NETCore.Platforms/runtime.json)
in the .NET Core repository. As an example, compiling for ARM 64-bit on
Windows (not tested for by Nexus Git) can be done with the following:
```
$ python server/Publisher.py dotnet win-arm64
```

### Without Python
.NET Core's method for creating platform-specific executables is
known as "publishing". When publishing in .NET Core, it does the
following:
- Gets the packages from NuGet.
- Compiles the program.
- Packages it into an executable.
  - .NET Core 3.0 is used because packaging into a single executable
    was added.

To publish the executable, the following command is used:
```
dotnet publish -r [platform] -c Release server/NexusGit.sln
```

Using the example above for ARM 64-bit on Windows, the command
is the following:
```
dotnet publish -r win-arm64 -c Release server/NexusGit.sln
```

After running the command, the platform executable can be found
in `server/NexusGit/bin/Release/NET_CORE_VERSION/PLATFORM/publish`.
If .NET Core 3.0 or newer is used, there will be an executable and a
`.pdb` file. Only the executable is needed.