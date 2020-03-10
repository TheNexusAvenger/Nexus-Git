/*
 * TheNexusAvenger
 *
 * Workspace for functional tests.
 */

using System;
using System.Diagnostics;
using System.IO;

namespace NexusGitTests.Functional
{
    public class FunctionalTestWorkspace
    {
        private string workspaceDirectory;
        
        /*
         * Creates a workspace.
         */
        public FunctionalTestWorkspace(string workspaceDirectory)
        {
            // Create a temporary directory.
            this.workspaceDirectory = workspaceDirectory;
            Directory.CreateDirectory(this.workspaceDirectory);
        }
        
        /*
         * Returns if a file exists.
         */
        public bool FileExists(string fileLocation)
        {
            fileLocation = Path.Combine(this.workspaceDirectory, fileLocation);
            return File.Exists(fileLocation);
        }

        /*
         * Returns if a directory exists.
         */
        public bool DirectoryExists(string fileLocation)
        {
            fileLocation = Path.Combine(this.workspaceDirectory, fileLocation);
            return Directory.Exists(fileLocation);
        }
        
        /*
         * Writes a file.
         */
        public void WriteFile(string fileLocation,string contents)
        {
            fileLocation = Path.Combine(this.workspaceDirectory, fileLocation);
            File.WriteAllText(fileLocation,contents);
        }
        
        /*
         * Deletes a file.
         */
        public void DeleteFile(string fileLocation)
        {
            fileLocation = Path.Combine(this.workspaceDirectory, fileLocation);
            File.Delete(fileLocation);
        }
        
        /*
         * Renames a file.
         */
        public void RenameFile(string initialFileLocation,string newFileLocation)
        {
            initialFileLocation = Path.Combine(this.workspaceDirectory,initialFileLocation);
            newFileLocation = Path.Combine(this.workspaceDirectory,newFileLocation);
            File.Move(initialFileLocation,newFileLocation);
        }
        
        /*
         * Reads a file.
         */
        public string ReadFile(string fileLocation)
        {
            fileLocation = Path.Combine(this.workspaceDirectory,fileLocation);
            return File.ReadAllText(fileLocation);
        }
        
        /*
         * Creates a directory.
         */
        public void CreateDirectory(string fileLocation)
        {
            fileLocation = Path.Combine(this.workspaceDirectory, fileLocation);
            Directory.CreateDirectory(fileLocation);
        }
        
        /*
         * Runs a command in the workspace and waits for it complete.
         */
        public void RunCommand(string command,string arguments)
        {
            var process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = arguments;
            process.Start();
            process.WaitForExit();
        }
    }
}