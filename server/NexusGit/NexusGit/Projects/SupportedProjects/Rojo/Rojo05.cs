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
     * Class representing a meta file without a class name.
     */
    public class Rojo05MetaFileNoClassName
    {
        public Dictionary<string,object> properties = new Dictionary<string,object>(); 
    }
    
    /*
     * Class representing a meta file.
     */
    public class Rojo05MetaFile : Rojo05MetaFileNoClassName
    {
        public string className;
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
                    var properties = JsonConvert.DeserializeObject<Dictionary<string,object>>(structure[key].ToString());
                    
                    // Add the properties.
                    foreach (var propertyName in properties.Keys)
                    {
                        treeObject.Properties.Add(propertyName,new Property<object>("",properties[propertyName]));
                    }
                } else
                {
                    treeObject.Children.Add(CreateFromStructure(JsonConvert.DeserializeObject<Dictionary<string,object>>(structure[key].ToString()),key));
                }
            }
            
            // Return the object.
            return treeObject;
        }
        
        /*
         * Returns if a path is needed in the object or a subobject.
         */
        public bool HasPathReference()
        {
            // Return true if the path is defined.
            if (this.Path != null)
            {
                return true;
            }
            
            // Return if a child has a path.
            foreach (var childObject in this.Children)
            {
                if (childObject.HasPathReference())
                {
                    return true;
                }
            }
            
            // Return false (path not used).
            return false;
        }
        
        /*
         * Returns a Rojo instance
         */
        public RojoInstance GetRojoInstance(RojoFile projectFiles,Rojo05 project)
        {
            RojoInstance newInstance = null;
            
            // Return a file-based object if the path used.
            if (this.Path != null)
            {
                // Get the file.
                var file = projectFiles.GetFile(this.Path);
                if (file == null)
                {
                    return null;
                }
                
                // Create the instance.
                newInstance = project.GetFromFile(file);
                newInstance.Name = this.Name;
            } else {
                newInstance = new RojoInstance();
            }
            
            // Set the class name.
            if (this.ClassName != null)
            {
                newInstance.ClassName = this.ClassName;
            }
            
            // Add the properties.
            foreach (var propertyName in this.Properties.Keys)
            {
                newInstance.Properties.Add(propertyName,this.Properties[propertyName]);
            }
            
            // Add the children.
            foreach (var child in this.Children)
            {
                var childInstance = child.GetRojoInstance(projectFiles,project);
                if (childInstance != null)
                {
                    childInstance.Name = child.Name;
                    newInstance.Children.Add(childInstance);
                }
            }
            
            // Return the instance.
            return newInstance;
        }
        
        /*
         * Populates the files given the tree node.
         */
        public void PopulateRojoFiles(RojoFile rootDirectory,RojoInstance instance,Rojo05 project)
        {
            // Populate the files.
            if (this.Path != null)
            {
                // Create the directories to the path.
                var parentDirectory = rootDirectory;
                var pathEnding = this.Path.Trim();
                if (this.Path.Contains("\\") || this.Path.Contains("/"))
                {
                    var parentPath = FileFinder.MoveDirectoryUp(this.Path);
                    parentDirectory = rootDirectory.CreateEmptyFilesToPath(parentPath);
                    
                    // Split the path and set the ending.
                    var splitPath = this.Path.Replace("\\","/").Split("/").ToList();
                    if (splitPath.Last().Trim() == "")
                    {
                        pathEnding = splitPath[splitPath.Count - 2].Trim();
                    }
                    else
                    {
                        pathEnding = splitPath.Last().Trim();
                    }
                }
                
                // Remove already defined files.
                var newRojoFile = project.GetFile(instance);
                foreach (var subFile in new List<RojoFile>(newRojoFile.SubFiles))
                {
                    // Get the tree object.
                    Rojo05TreeObject treeObject = null;
                    foreach (var subTreeObject in this.Children)
                    {
                        if (subFile.Name.Contains(subTreeObject.Name))
                        {
                            treeObject = subTreeObject;
                            break;
                        }
                    }
                    
                    // Remove the file if the tree object exists.
                    if (treeObject != null)
                    {
                        newRojoFile.RemoveFile(subFile.Name);
                    }
                }
                
                // Add the file.
                if (newRojoFile.SubFiles.Count != 0 || newRojoFile.Contents != null)
                {
                    newRojoFile.Name = pathEnding;
                    parentDirectory.AddFile(newRojoFile);
                }
            }
            
            // Populate the children.
            foreach (var subTreeObject in this.Children)
            {
                // Get the child.
                RojoInstance child = null;
                foreach (var newChild in instance.Children)
                {
                    if (newChild.Name == subTreeObject.Name)
                    {
                        child = newChild;
                        break;
                    }
                }
                
                // Populate the child if it exists.
                if (child != null)
                {
                    subTreeObject.PopulateRojoFiles(rootDirectory,child,project);
                }
            }
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
                if (file.FileExists( "init.meta.json"))
                {
                    var metaFile = file.RemoveFile("init.meta.json");
                    var metaData = JsonConvert.DeserializeObject<Dictionary<string,object>>(metaFile.Contents);
                    
                    // Set the class name.
                    if (metaData.Keys.Contains("className"))
                    {
                        newInstance.ClassName = (string) metaData["className"];
                    }
                    
                    // Add the properties.
                    if (metaData.Keys.Contains("properties"))
                    {
                        var properties = JsonConvert.DeserializeObject<Dictionary<string,object>>(metaData["properties"].ToString());
                        foreach (var propertyName in properties.Keys)
                        {
                            newInstance.Properties.Add(propertyName,new Property<object>("",properties[propertyName]));
                        }
                    }
                }
                
                // Add the child objects.
                foreach (var subFile in new List<RojoFile>(file.SubFiles))
                {
                    if (file.SubFiles.Contains(subFile)) {
                        var subInstance = this.GetFromFile(subFile);
                        if (subInstance != null)
                        {
                            newInstance.Children.Add(subInstance);
                        }
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
                if (className.Contains("Script"))
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
                        var properties = JsonConvert.DeserializeObject<Dictionary<string,object>>(metaData["properties"].ToString());
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
         * Creates a meta file for an instance.
         */
        public RojoFile GetMetaFile(RojoInstance instance)
        {
            // Clone the dictionary of properties.
            var properties = new Dictionary<string,Property<object>>(instance.Properties);
            if (properties.ContainsKey("Source"))
            {
                properties.Remove("Source");
            }
            if (properties.ContainsKey("LinkedSource") && ((string) properties["LinkedSource"].Value) == "")
            {
                properties.Remove("LinkedSource");
            }

            // Create a file if properties exist.
            if (properties.Count > 0)
            {
                // Get the properties.
                var metaProperties = new  Rojo05MetaFileNoClassName();
                foreach (var propertyName in properties.Keys)
                {
                    var property = instance.Properties[propertyName];
                    metaProperties.properties[propertyName] = property.Value;
                }
                
                // Create and return the file.
                var metaContents = JsonConvert.SerializeObject(metaProperties,Formatting.Indented);
                var metaFile = new RojoFile(instance.Name + ".meta.json");
                metaFile.Contents = metaContents;
                return metaFile;
            }
            
            // Return null (no file).
            return null;
        }
        
        /*
         * Creates a meta file for an instance with the class name.
         */
        public RojoFile GetMetaFileWithClassName(RojoInstance instance)
        {
            // Create a file if properties exist.
            if (instance.Properties.Count > 0)
            {
                // Get the properties.
                var metaProperties = new Rojo05MetaFile();
                metaProperties.className = instance.ClassName;
                foreach (var propertyName in instance.Properties.Keys)
                {
                    var property = instance.Properties[propertyName];
                    metaProperties.properties[propertyName] = property.Value;
                }
                
                // Create and return the file.
                var metaContents = JsonConvert.SerializeObject(metaProperties,Formatting.Indented);
                var metaFile = new RojoFile(instance.Name + ".meta.json");
                metaFile.Contents = metaContents;
                return metaFile;
            }
            
            // Return null (no file).
            return null;
        }
        
        /*
         * Returns a Roblox instance for a given file or directory.
         */
        public override RojoFile GetFile(RojoInstance instance)
        {
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
                    newDirectory.AddFile(newFile);
                    
                    // Add the meta file.
                    if (instance.Properties.Count > 1)
                    {
                        var metaFile = this.GetMetaFile(instance);
                        if (metaFile != null)
                        {
                            metaFile.Name = "init.meta.json";
                            newDirectory.AddFile(metaFile);
                        }
                    }
                    
                    // Add the child instances.
                    foreach (var subInstance in instance.Children) {
                        var subFile = this.GetFile(subInstance);
                        newDirectory.AddFile(subFile);
                        
                        // Create a meta file.
                        if (subFile.Contents != "" && subInstance.ClassName.Contains("Script") && subInstance.Properties.Count > 1)
                        {
                            var metaFile = this.GetMetaFile(subInstance);
                            if (metaFile != null)
                            {
                                newDirectory.AddFile(metaFile);
                            }
                        }
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
                foreach (var subInstance in instance.Children)
                {
                    var subFile = this.GetFile(subInstance);
                    newDirectory.AddFile(subFile);
                    
                    // Create a meta file.
                    if (subFile.Contents != "" && subInstance.ClassName.Contains("Script") && subInstance.Properties.Count > 1)
                    {
                        var metaFile = this.GetMetaFile(subInstance);
                        if (metaFile != null)
                        {
                            newDirectory.AddFile(metaFile);
                        }
                    }
                }
                    
                // Return the directory.
                return newDirectory;
            } else if (instance.ChildOfClassExists("Script") || instance.ChildOfClassExists("LocalScript") || instance.ChildOfClassExists("ModuleScript"))
            {
                // Create a directory.
                var newDirectory = new RojoFile(instance.Name);
                var metaFile = this.GetMetaFileWithClassName(instance);
                metaFile.Name = "init.meta.json";
                newDirectory.AddFile(metaFile);
                
                // Add the child instances.
                foreach (var subInstance in instance.Children) {
                    var subFile = this.GetFile(subInstance);
                    newDirectory.AddFile(subFile);
                        
                    // Create a meta file.
                    if (subFile.Contents != "" && subInstance.ClassName.Contains("Script") && subInstance.Properties.Count > 1)
                    {
                        var subMetaFile = this.GetMetaFile(subInstance);
                        if (subMetaFile != null)
                        {
                            newDirectory.AddFile(subMetaFile);
                        }
                    }
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
         * Returns the partitions of the project.
         */
        public override Partitions ReadProjectStructure()
        {
            // Get the project structure and return null if it doesn't exist.
            var structure = this.GetStructure();
            if (structure == null) {
                return null;
            }

            // Create the new partitions.
            var partitions = new Partitions();
            
            // Add the partitions.
            var incrementer = new TemporaryIdIncrementer();
            var treeStructure = Rojo05TreeObject.CreateFromStructure(structure.tree,"game");
            var parentDirectory = FileFinder.GetParentDirectoryOfFile(this.GetRequiredFile());
            var projectFiles = RojoFile.FromFile(parentDirectory);
            foreach (var treeItem in treeStructure.Children) {
                // Get the instance and add it to the partitions if it exists.
                var instance = treeItem.GetRojoInstance(projectFiles,this);
                if (instance != null) {
                    instance.Name = treeItem.Name;
                    partitions.AddInstance(treeItem.Name,instance.ToRobloxInstance(incrementer));
                }
            }
            
            // Return the partitions.
            return partitions;
        }

        /*
         * Writes the partitions to the file system.
         */
        public override void WriteProjectStructure(Partitions partitions) {
            // Get the project structure and return if it doesn't exist.
            var structure = this.GetStructure();
            if (structure == null)
            {
                return;
            }
            
            // Create a root directory and add the partitions.
            var rootDirectory = new RojoFile("root");
            var treeStructure = Rojo05TreeObject.CreateFromStructure(structure.tree,"game");
            foreach (var treeItem in treeStructure.Children) {
                var instance = partitions.GetInstance(treeItem.Name);
                if (instance != null) {
                    var rojoInstance = RojoInstance.ConvertInstance(instance);
                    treeItem.PopulateRojoFiles(rootDirectory,rojoInstance,this);
                }
            }
            
            // Write the subfiles.
            var workingDirectory = FileFinder.GetParentDirectoryOfFile(this.GetRequiredFile());
            foreach (var file in rootDirectory.SubFiles)
            {
                var location = Path.Combine(workingDirectory, file.Name);
                file.WriteFile(location);
            }
        }
        
        /*
         * Returns the partitions to use.
         */
        public override Dictionary<string,string> GetPartitions() {
            // Get the project structure and return if it doesn't exist.
            var structure = this.GetStructure();
            if (structure == null)
            {
                return null;
            }
            
            // Get the partitions.
            var partitions = new Dictionary<string,string>();
            var treeStructure = Rojo05TreeObject.CreateFromStructure(structure.tree,"game");
            foreach (var treeObject in treeStructure.Children)
            {
                //if (treeObject.HasPathReference()) // TODO: Requires rework of GetPartitions. Already planned for V.0.2.0.
                //{
                    partitions.Add(treeObject.Name,treeObject.Name);
                //}
            }
            
            // Return the partitions.
            return partitions;
        }
    }
}