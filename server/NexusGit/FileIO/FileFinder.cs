/*
 * TheNexusAvenger
 * 
 * Helper methods for finding files and directories.
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NexusGit.FileIO
{
    /*
     * Class that contains methods for finding files and directories.
     */
    public static class FileFinder
    {
        /*
         * Returns the name of the most upper level directory.
         */
        public static string GetUpperDirectoryName(string directory)
        {
            // Get the current directory and split the parts.
            List<string> directoryParts = directory.Split('\\').ToList();
            if (directoryParts.Count == 1)
            {
                directoryParts = directory.Split('/').ToList();
            }
            
            // Return the top directory.
            return directoryParts[directoryParts.Count - 1];
        }
        
        /*
         * Returns the parent directory. Returns null if it is already the root.
         */
        public static string MoveDirectoryUp(string directory)
        {
            // Get the current directory and split the parts.
            List<string> directoryParts = directory.Split('\\').ToList();
            if (directoryParts.Count == 1)
            {
                directoryParts = directory.Split('/').ToList();
            }
            
            // Remove the last item if it is empty.
            if (directoryParts[directoryParts.Count - 1] == "")
            {
                directoryParts.RemoveAt(directoryParts.Count - 1);
            }
            
            // If there aren't enough parts, return null.
            if (directoryParts.Count <= 1)
            {
                return null;
            }
            
            // Create the parent directory.
            string newDirectory = "";
            for (int i = 0; i < directoryParts.Count - 1; i++)
            {
                newDirectory += directoryParts[i] + "/";
            }
            
            // Return the directory.
            return newDirectory;
        }
        
        /*
         * Returns the path for the parent directory containing a file or directory.
         * Returns null if the file doesn't exist.
         */
        private static string GetParentDirectory(string fileName,bool isDirectory)
        {
            // Get the current directory and split the parts.
            string currentDirectory = Directory.GetCurrentDirectory();
            List<string> directoryParts = currentDirectory.Split('\\').ToList();
            if (directoryParts.Count == 1)
            {
                directoryParts = currentDirectory.Split('/').ToList();
            }

            // Iterate through the directories to find the file.
            while (directoryParts.Count > 0)
            {
                // Concat the current directory.
                string newDirectory = "";
                foreach (string directory in directoryParts)
                {
                    newDirectory += directory + "/";
                }

                // Return the location if the file exists.
                string requiredFile = newDirectory + fileName;
                if ((isDirectory && Directory.Exists(requiredFile)) || (!isDirectory && File.Exists(requiredFile)))
                {
                    return newDirectory;
                }

                // Remove the last entry.
                directoryParts.RemoveAt(directoryParts.Count - 1);
            }

            // Return null (file not found).
            return null;
        }

        /*
         * Returns the path for the parent directory containing a file. Returns
         * null if the file doesn't exist.
         */
        public static string GetParentDirectoryOfFile(string fileName)
        {
            return GetParentDirectory(fileName,false);
        }

        /*
         * Returns the path for the parent directory containing a directory. Returns
         * null if the file doesn't exist.
         */
        public static string GetParentDirectoryOfDirectory(string fileName)
        {
            return GetParentDirectory(fileName,true);
        }
    }
}