"""
TheNexusAvenger

Builds and "publishes" (creates platform-specific executables)
Nexus Git for the supported platforms.
"""

import os
import shutil
import subprocess
import sys



DOTNET_CORE_VERSION = "netcoreapp3.1"
PLATFORMS = [
    "win-x64",
    "osx-x64",
    "linux-x64",
]



"""
Returns the path to the executable for the platform.
"""
def getExecutablePath(projectDirectory,platform):
    # Create the base path.
    publishDirectory = projectDirectory + "bin\\Release\\" + DOTNET_CORE_VERSION + "\\" + platform + "\\publish\\"

    # Return the first non-pdb file.
    for fileName in os.listdir(publishDirectory):
        if fileName[-4:] != ".pdb":
            return publishDirectory + fileName

"""
Returns the new file name for a platform.
"""
def getNewFileName(fileLocation,platform):
    # Get the file name.
    splitLocation = fileLocation.split("\\")
    fileName = splitLocation[len(splitLocation) - 1]

    # Return the name.
    splitName = os.path.splitext(fileName)
    return splitName[0] + "-" + platform + splitName[1]

"""
Creates a release for a platform version.
"""
def publishVersion(platform,compiler,solutionFile,projectDirectory,destinationDirectory):
    # Create the destination directory if needed.
    if not os.path.exists(destinationDirectory):
        os.mkdir(destinationDirectory)

    # Build the file.
    print("Building .NET Core for " + platform)
    subprocess.call([compiler,"publish","-r",platform,"-c","Release",solutionFile])

    # Copy the compiled file to central directory.
    executableLocation = getExecutablePath(projectDirectory,platform)
    destinationFile = destinationDirectory + getNewFileName(executableLocation,platform)
    shutil.copyfile(executableLocation,destinationFile)


if __name__ == '__main__':
    # Get the parameters.
    if len(sys.argv) < 2:
        print("Usage: Publisher.py Compiler (TargetPlatform)")
        exit(0)

    # Get the directories.
    compiler = sys.argv[1]
    workspaceDirectory = os.path.realpath(__file__ + "\\..")
    solutionFile = workspaceDirectory + "\\NexusGit.sln"
    projectDirectory = workspaceDirectory + "\\NexusGit\\"
    destinationDirectory = workspaceDirectory + "\\bin\\"

    # Build the platforms.
    if len(sys.argv) >= 3:
        publishVersion(sys.argv[2],compiler,solutionFile,projectDirectory,destinationDirectory)
    else:
        print("No custom platform defined. Compiling for: " + str(PLATFORMS))
        for platformData in PLATFORMS:
            publishVersion(platformData,compiler,solutionFile,projectDirectory,destinationDirectory)
