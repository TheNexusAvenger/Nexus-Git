"""
TheNexusAvenger

Builds and "publishes" (creates platform-specific executables)
Nexus Git for the supported platforms.
"""

import os
import shutil
import subprocess
import sys



DOTNET_CORE_VERSION = "netcoreapp3.0"
PLATFORMS = [
    ["win-x64","NexusGit.exe","NexusGit-win-x64.exe"],
    ["osx-x64","NexusGit","NexusGit-osx-x64"],
    ["linux-x64","NexusGit","NexusGit-linux-x64"],
]



"""
Creates a release for a platform version.
"""
def publishVersion(platform,compiledFileName,compiler,solutionFile,projectDirectory,destinationFile):
    # Create the destination directory if needed.
    if not os.path.exists(destinationDirectory):
        os.mkdir(destinationDirectory)

    # Build the file.
    print("Building .NET Core for " + platform)
    subprocess.call([compiler,"publish","-r",platform,"-c","Release",solutionFile])

    # Copy the compiled file to central directory.
    executableLocation = projectDirectory + "bin\\Release\\" + DOTNET_CORE_VERSION + "\\" + platform + "\\publish\\" + compiledFileName
    shutil.copyfile(executableLocation,destinationFile)



if __name__ == '__main__':
    # Get the parameters.
    if len(sys.argv) < 2:
        print("Usage: Publisher compiler")
        exit(0)

    # Get the directories.
    compiler = sys.argv[1]
    workspaceDirectory = os.path.realpath(__file__ + "\\..")
    solutionFile = workspaceDirectory + "\\NexusGit.sln"
    projectDirectory = workspaceDirectory + "\\NexusGit\\"
    destinationDirectory = workspaceDirectory + "\\bin\\"

    # Build the platforms.
    for platformData in PLATFORMS:
        publishVersion(platformData[0],platformData[1],compiler,solutionFile,projectDirectory,destinationDirectory + platformData[2])