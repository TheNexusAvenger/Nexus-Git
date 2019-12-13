/*
 * TheNexusAvenger
 *
 * Provides support for Version 0.5 of Rojo.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NexusGit.FileIO;
using NexusGit.RobloxInstance;

namespace NexusGit.NexusGit.Projects.SupportedProjects.Rojo
{
    /*
     * Class representing a Rojo 0.5 structure.
     * Used for JSON serialization.
     */
    public class Rojo05Structure
    {
        public string name;
        public int servePort = 34872;
        public Dictionary<string,object> tree;
    }
    
    /*
     * Class for a tree object.
     */
    public class Rojo05TreeObject
    {
        public string Name;
        public string ClassName;
        public string Path;
        public List<Rojo05TreeObject> Children;
        public Dictionary<string,Property<object>> Properties;
        
        /*
         * Creates a tree object.
         */
        public Rojo05TreeObject()
        {
            this.Children = new List<Rojo05TreeObject>();
            this.Properties = new Dictionary<string,Property<object>>();
        }
        
        /*
         * Creates a tree object from a dictionary.
         */
        public static Rojo05TreeObject CreateFromStructure(Dictionary<string,object> structure,string name)
        {
            // Create the object.
            var treeObject = new Rojo05TreeObject();
            treeObject.Name = name;
            
            // Read the dictionary.
            foreach (var key in structure.Keys)
            {
                // Read a property or initialize a child.
                if (key.ToLower() == "$classname")
                {
                    treeObject.ClassName = (string) structure[key];
                } else if (key.ToLower() == "$path")
                {
                    treeObject.Path = (string) structure[key];
                } else if (key.ToLower() == "$properties")
                {
                    var properties = (Dictionary<string, object>) structure[key];
                    
                    // Add the properties.
                    foreach (var propertyName in properties.Keys)
                    {
                        treeObject.Properties.Add(propertyName,new Property<object>("",properties[propertyName]));
                    }
                } else
                {
                    treeObject.Children.Add(CreateFromStructure((Dictionary<string,object>) structure[key],key));
                }
            }
            
            // Return the object.
            return treeObject;
        }
        
        /*
         * Returns a Rojo instance
         */
        public RojoInstance GetRojoInstance(RojoFile projectFiles,Rojo05 project)
        {
            return null;
        }
    }
    
    /*
     * Class for writing Rojo projects.
     */
    public class Rojo05 : Rojo
    {
        /*
         * Returns the project structure.
         */
        public Rojo05Structure GetStructure()
        {
            // Get the JSON file parent directory.
            var parentDirectory = FileFinder.GetParentDirectoryOfFile(this.GetRequiredFile());
            
            // Display a console error and return null if the parent directory is null.
            if (parentDirectory == null)
            {
                Console.WriteLine("The Rojo 0.5 project file (default.project.json) does not exist in the working or parent directory.");
                Console.WriteLine("If you are changing projects, please restart the server since a new port may be needed.");
                return null;
            }

            // Read and parse the JSON file.
            var jsonSource = File.ReadAllText(parentDirectory + this.GetRequiredFile());
            return JsonConvert.DeserializeObject<Rojo05Structure>(jsonSource);
        }
        
        /*
         * Returns the file required for the Rojo project.
         */
        public override string GetRequiredFile()
        {
            return "default.project.json";
        }
        
        /*
         * Returns the name of the project.
         */
        public override string GetName()
        {
            return "Rojo 0.5.X";
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
                } else if (file.FileExists("init.txt")) {
                    className = "StringValue";
                    newInstance.Properties.Add("Value",new Property<object>("String",file.Contents));
                    file.RemoveFile("init.txt");
                } else if (file.Name.ToLower().EndsWith(".cvs")) {
                    Console.WriteLine("CVS file parsing is currently not supported. Expect it to be supported later.");
                } else if (file.Name.ToLower().EndsWith(".rbxmx")) {
                    Console.WriteLine("RBXMX file parsing is currently not supported. Expect it to be supported later.");
                } else if (file.Name.ToLower().EndsWith(".rbxm")) {
                    Console.WriteLine("RBXM file parsing is currently not supported. Support is not planned. To use model syncing, use .rbxmx files instead.");
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
                
                // Populate the meta information.
                if (file.Parent != null && file.Parent.FileExists( "init.meta.json"))
                {
                    var metaFile = file.Parent.RemoveFile("init.meta.json");
                    var metaData = JsonConvert.DeserializeObject<Dictionary<string,object>>(metaFile.Contents);
                    
                    // Set the class name.
                    if (metaData.Keys.Contains("className"))
                    {
                        newInstance.ClassName = (string) metaData["className"];
                    }
                    
                    // Add the properties.
                    if (metaData.Keys.Contains("properties"))
                    {
                        var properties = (Dictionary<string,object>) metaData["properties"];
                        foreach (var propertyName in properties.Keys)
                        {
                            newInstance.Properties.Add(propertyName,new Property<object>("",properties[propertyName]));
                        }
                    }
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
                } else if (file.Name.ToLower().EndsWith(".txt")) {
                    className = "StringValue";
                    name = file.Name.Remove(file.Name.Length - 4);
                    newInstance.Properties.Add("Value",new Property<object>("String",file.Contents));
                } else if (file.Name.ToLower().EndsWith(".cvs")) {
                    Console.WriteLine("CVS file parsing is currently not supported. Expect it to be supported later.");
                } else if (file.Name.ToLower().EndsWith(".rbxmx")) {
                    Console.WriteLine("RBXMX file parsing is currently not supported. Expect it to be supported later.");
                } else if (file.Name.ToLower().EndsWith(".rbxm")) {
                    Console.WriteLine("RBXM file parsing is currently not supported. Support is not planned. To use model syncing, use .rbxmx files instead.");
                }
                
                // Return null if a class name doesn't exist.
                if (className == null) {
                    return null;
                }
                
                // Populate the properties.
                newInstance.ClassName = className;
                newInstance.Name = name;
                if (name.Contains("Script"))
                {
                    newInstance.Properties.Add("Source", new Property<object>("String", file.Contents));
                }
                
                // Populate the meta information.
                if (file.Parent != null && file.Parent.FileExists(name + ".meta.json"))
                {
                    var metaFile = file.Parent.RemoveFile(name + ".meta.json");
                    var metaData = JsonConvert.DeserializeObject<Dictionary<string,object>>(metaFile.Contents);
                    
                    // Add the properties.
                    if (metaData.Keys.Contains("properties"))
                    {
                        var properties = (Dictionary<string,object>) metaData["properties"];
                        foreach (var propertyName in properties.Keys)
                        {
                            newInstance.Properties.Add(propertyName,new Property<object>("",properties[propertyName]));
                        }
                    }
                }
            }
            
            // Return the instance.
            return newInstance;
        }
        
        /*
         * Returns a Roblox instance for a given file or directory.
         */
        public override RojoFile GetFile(RojoInstance instance)
        {
            return null;
        }
        
        /*
         * Returns the partitions of the project.
         */
        public override Partitions ReadProjectStructure()
        {
            // Get the project structure and return null if it doesn't exist.
            var structure = this.GetStructure();
            if (structure == null) {
                return null;
            }

            return null;
        }

        /*
         * Writes the partitions to the file system.
         */
        public override void WriteProjectStructure(Partitions partitions) {
            
        }
        
        /*
         * Returns the partitions to use.
         */
        public override Dictionary<string,string> GetPartitions() {
            // Get the project structure and return null if it doesn't exist.
            var structure = this.GetStructure();
            if (structure == null) {
                return null;
            }

            // Get the partitions.
            var partitions = new Dictionary<string,string>();
            foreach (var treeItemName in structure.tree.Keys) {
                if (!treeItemName.StartsWith("$"))
                {
                    partitions.Add(treeItemName,treeItemName);
                }
            }

            // Return the partitions.
            return partitions;
        }
    }
}