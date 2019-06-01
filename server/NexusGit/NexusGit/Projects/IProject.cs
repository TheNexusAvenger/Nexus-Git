/*
 * TheNexusAvenger
 *
 * Interface for a file-based Roblox project.
 */

using System.Collections.Generic;
using NexusGit.RobloxInstance;

namespace NexusGit.NexusGit.Projects
{
    /*
     * Interface representing a project.
     */
    public interface IProject
    {
        /*
         * Returns the name of the project.
         */
        string GetName();
        
        /*
         * Returns if a directory is valid for the project. It should not determine if a parent
         * directory is valid because parent directories are also checked.
         */
        bool IsDirectoryValid(string directory);
        
        /*
         * Returns the port to serve for the server.
         */
        int GetPort();
        
        /*
         * Returns the partitions to use.
         */
        Dictionary<string,string> GetPartitions();
        
        /*
         * Writes the partitions to the file system.
         */
        void WriteProjectStructure(Partitions partitions);
        
        /*
         * Returns the partitions of the project.
         */
        Partitions ReadProjectStructure();
    }
}