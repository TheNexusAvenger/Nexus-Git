/*
 * TheNexusAvenger
 *
 * Stores information of Git submodules.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace NexusGit.Git
{
    public class Submodule
    {
        public string Name;
        public string Path;
        public string Url;
        
        /*
         * Returns if a file is in the submodule.
         */
        public bool IsInSubmodule(string filePath)
        {
            return filePath.Replace("\\", "/").StartsWith(this.Path.Replace("\\", "/"));
        }
    }
    
    public class Submodules
    {
        public List<Submodule> Modules;
        
        /*
         * Creates a submodules parser.
         */
        public Submodules(string directory,string modulesSource)
        {
            this.Modules = new List<Submodule>();
            
            // Parse the modules.
            foreach (var line in modulesSource.Split("\n"))
            {
                var newLine = line.Trim();
                if (newLine.ToLower().StartsWith("[submodule"))
                {
                    this.Modules.Add(new Submodule());
                    var startIndex = newLine.IndexOf("\"", StringComparison.Ordinal) + 1;
                    this.Modules[^1].Name = newLine.Substring(startIndex,newLine.LastIndexOf("\"]", StringComparison.Ordinal) - startIndex);
                } else if (newLine.ToLower().StartsWith("path"))
                {
                    this.Modules[^1].Path = Path.Combine(directory,newLine.Substring(4).Trim().Substring(1).Trim());
                } else if (newLine.ToLower().StartsWith("url"))
                {
                    this.Modules[^1].Url = newLine.Substring(3).Trim().Substring(1).Trim();
                }
            }
        }
        
        /*
         * Parses a module file from a directory.
         */
        public static Submodules FromDirectory(string directory)
        {
            // Return if the modules file doesn't exist.
            var modulesLocation = Path.Combine(directory,".gitmodules");
            if (!File.Exists(modulesLocation))
            {
                return new Submodules(directory,"");
            }
            
            // Return the parsed file.
            return new Submodules(directory,File.ReadAllText(modulesLocation));
        }
        
        /*
         * Returns if a file is in a submodule.
         */
        public bool IsInSubmodule(string filePath)
        {
            // Return true if a file is in the module.
            foreach (var module in this.Modules)
            {
                if (module.IsInSubmodule(filePath))
                {
                    return true;
                }
            }
            
            // Return false (not in a submodule).
            return false;
        }
    }
}