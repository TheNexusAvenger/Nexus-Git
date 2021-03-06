﻿/*
 * TheNexusAvenger
 * 
 * Helper methods for finding files and directories.
 */

using System;
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
         * Returns the path separator for a path.
         */
        public static char GetPathSeparatorFromPath(string path) {
            // Determine the separator.
            var separator = Path.DirectorySeparatorChar;
            if (path.Contains("\\") && !path.Contains("/"))
            {
                separator = '\\';
            } else if (!path.Contains("\\") && path.Contains("/"))
            {
                separator = '/';
            }
            
            // Return the separator.
            return separator;
        }
        
        /*
         * Returns the parent directory. Returns null if it is already the root.
         */
        public static string MoveDirectoryUp(string directory)
        {
            // Determine the separator.
            var separator = GetPathSeparatorFromPath(directory);
            
            // Get the current directory and split the parts.
            var directoryParts = directory.Replace("\\","/").Split("/").ToList();
            
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
            var newDirectory = "";
            for (var i = 0; i < directoryParts.Count - 1; i++)
            {
                newDirectory += directoryParts[i] + separator;
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
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryParts = currentDirectory.Replace("\\","/").Split("/").ToList();

            // Iterate through the directories to find the file.
            while (directoryParts.Count > 0)
            {
                // Concat the current directory.
                var newDirectory = "";
                foreach (var directory in directoryParts)
                {
                    newDirectory += directory + Path.DirectorySeparatorChar;
                }

                // Return the location if the file exists.
                var requiredFile = newDirectory + fileName;
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