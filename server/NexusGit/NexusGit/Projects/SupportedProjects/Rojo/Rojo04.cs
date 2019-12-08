/*
 * TheNexusAvenger
 *
 * Provides support for Version 0.4 and lower of Rojo.
 */


using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NexusGit.FileIO;
using NexusGit.RobloxInstance;

namespace NexusGit.NexusGit.Projects.SupportedProjects.Rojo
{
    /*
     * Class representing a Rojo 0.4 structure.
     * Used for JSON serialization.
     */
    public class Rojo04Structure
    {
        public string name;
        public int servePort;
        public Dictionary<string,Dictionary<string,string>> partitions;
    }
    
    /*
     * Class for writing Rojo projects.
     */
    public class Rojo04 : Rojo
    {
        /*
         * Returns the project structure.
         */
        public Rojo04Structure GetStructure()
        {
            // Get the JSON file parent directory.
            var parentDirectory = FileFinder.GetParentDirectoryOfFile(this.GetRequiredFile());
            
            // Display a console error and return null if the parent directory is null.
            if (parentDirectory == null)
            {
                Console.WriteLine("The Rojo 0.4 project file (rojo.json) does not exist in the working or parent directory.");
                Console.WriteLine("If you are changing projects, please restart the server since a new port may be needed.");
                return null;
            }

            // Read and parse the JSON file.
            var jsonSource = File.ReadAllText(parentDirectory + this.GetRequiredFile());
            return JsonConvert.DeserializeObject<Rojo04Structure>(jsonSource);
        }
        
        /*
         * Returns the file required for the Rojo project.
         */
        public override string GetRequiredFile()
        {
            return "rojo.json";
        }
        
        /*
         * Returns the name of the project.
         */
        public override string GetName()
        {
            return "Rojo 0.4.X";
        }
        
        /*
         * Returns the port to serve for the server.
         */
        public override int GetPort()
        {
            // Get the project structure.
            var structure = GetStructure();
            
            // Return the port. 
            return structure.servePort;
        }
        
        /*
         * Returns a Roblox instance for a given file or directory.
         */
        public override RojoInstance GetFromFile(RojoFile file)
        {
            // Create the base instance.
            var newInstance = new RojoInstance();
            
            // Parse the file as a directory or as a file based on if it has contents.
            if (file.Contents == null) {
                // Get the contents and class name.
                var className = "Folder";
                RojoFile initFile = null;
                if (file.FileExists("init.lua")) {
                    className = "ModuleScript";
                    initFile = file.RemoveFile("init.lua");
                } else if (file.FileExists("init.server.lua")) {
                    className = "Script";
                    initFile = file.RemoveFile("init.server.lua");
                } else if (file.FileExists("init.client.lua")) {
                    className = "LocalScript";
                    initFile = file.RemoveFile("init.client.lua");
                } else if (file.FileExists("init.model.json")) {
                    className = null;
                    initFile = file.RemoveFile("init.model.json");
                }
                
                // Populate the name and class name or replace the instance with a file.
                if (className != null) {
                    newInstance.ClassName = className;
                    newInstance.Name = file.Name;

                    // Add the source.
                    if (initFile != null) {
                        newInstance.Properties.Add("Source",new Property<object>("String",initFile.Contents));
                    }
                } else {
                    newInstance = this.GetFromFile(initFile);
                }
                
                // Add the child objects.
                foreach (var subFile in file.SubFiles) {
                    var subInstance = this.GetFromFile(subFile);
                    if (subInstance != null) {
                        newInstance.Children.Add(subInstance);
                    }
                }
            } else {
                // Get the contents and class name.
                string className = null;
                string name = null;
                if (file.Name.ToLower().EndsWith(".server.lua")) {
                    className = "Script";
                    name = file.Name.Remove(file.Name.Length - 11);
                } else if (file.Name.ToLower().EndsWith(".client.lua")) {
                    className = "LocalScript";
                    name = file.Name.Remove(file.Name.Length - 11);
                } else if (file.Name.ToLower().EndsWith(".lua")) {
                    className = "ModuleScript";
                    name = file.Name.Remove(file.Name.Length - 4);
                } else if (file.Name.ToLower().EndsWith(".model.json")) {
                    return JsonConvert.DeserializeObject<RojoInstance>(file.Contents);
                }
                
                // Return null if a class name doesn't exist.
                if (className == null) {
                    return null;
                }
                
                // Populate the properties.
                newInstance.ClassName = className;
                newInstance.Name = name;
                newInstance.Properties.Add("Source",new Property<object>("String",file.Contents));
            }
            
            // Return the instance.
            return newInstance;
        }
        
        /*
         * Returns a Roblox instance for a given file or directory.
         */
        public override RojoFile GetFile(RojoInstance instance) {
            // Return a file for a script.
            if (instance.ClassName == "Script" || instance.ClassName == "LocalScript" || instance.ClassName == "ModuleScript") {
                if (instance.Children.Count != 0) {
                    // Create the directory.
                    var newDirectory = new RojoFile(instance.Name);
                    
                    // Create the child file.
                    RojoFile newFile = null;
                    if (instance.ClassName == "Script") {
                        newFile = new RojoFile("init.server.lua");
                    } else if (instance.ClassName == "LocalScript") {
                        newFile = new RojoFile("init.client.lua");
                    } else {
                        newFile = new RojoFile("init.lua");
                    }
                    
                    // Add the child file.
                    newFile.Contents = (string) instance.Properties["Source"].Value;
                    newDirectory.SubFiles.Add(newFile);
                    
                    // Add the child instances.
                    foreach (var subInstance in instance.Children) {
                        newDirectory.SubFiles.Add(this.GetFile(subInstance));
                    }
                    
                    // Return the directory.
                    return newDirectory;
                } else {
                    // Create the file.
                    RojoFile newFile = null;
                    if (instance.ClassName == "Script") {
                        newFile = new RojoFile(instance.Name + ".server.lua");
                    } else if (instance.ClassName == "LocalScript") {
                        newFile = new RojoFile(instance.Name + ".client.lua");
                    } else {
                        newFile = new RojoFile(instance.Name + ".lua");
                    }
                    
                    // Return the file with contents.
                    newFile.Contents = (string) instance.Properties["Source"].Value;
                    return newFile;
                }
            } else if (instance.ClassName == "Folder") {
                // Create the directory.
                var newDirectory = new RojoFile(instance.Name);
                    
                // Add the child instances.
                foreach (var subInstance in instance.Children) {
                    newDirectory.SubFiles.Add(this.GetFile(subInstance));
                }
                    
                // Return the directory.
                return newDirectory;
            }
            
            // Serialize and store the instance.
            var instanceData = JsonConvert.SerializeObject(instance,Formatting.Indented);
            var file = new RojoFile(instance.Name + ".model.json");
            file.Contents = instanceData;
            return file;
        }
        
        /*
         * Returns the partitions to use.
         */
        public override Dictionary<string, string> GetPartitions() {
            // Get the project structure and return null if it doesn't exist.
            var structure = this.GetStructure();
            if (structure == null) {
                return null;
            }

            // Get the partitions.
            var partitions = new Dictionary<string, string>();
            foreach (var partitionData in structure.partitions.Values) {
                partitions.Add(partitionData["path"], partitionData["target"]);
            }

            // Return the partitions.
            return partitions;
        }
    }
}