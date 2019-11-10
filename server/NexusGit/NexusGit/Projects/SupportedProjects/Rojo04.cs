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

namespace NexusGit.NexusGit.Projects.SupportedProjects
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
     * Class representing a Rojo instance.
     * Used for JSON serialization.
     */
    public class Rojo04Instance
    {
        public string Name;
        public string ClassName;
        public List<Rojo04Instance> Children;
        public Dictionary<string,Property<object>> Properties;
        
        /*
         * Creates a Rojo instance.
         */
        public Rojo04Instance()
        {
            this.Children = new List<Rojo04Instance>();
            this.Properties = new Dictionary<string,Property<object>>();
        }
        
        /*
         * Converts a RobloxInstance to a Rojo04Instance.
         */
        public static Rojo04Instance ConvertInstance(Instance instance)
        {
            // Create the new instance.
            var newInstance = new Rojo04Instance();
            
            // Set the name and class name.
            newInstance.Name = (string) instance.GetProperty("Name").Value;
            newInstance.ClassName = (string) instance.GetProperty("ClassName").Value;
            
            // Add the properties.
            foreach (var propertyName in instance.Properties.Keys)
            {
                if (propertyName != "Name" && propertyName != "ClassName" && instance.Properties[propertyName].Type != "TemporaryInstanceReference")
                {
                    newInstance.Properties.Add(propertyName,instance.Properties[propertyName]);
                }
            }
            
            // Add the children.
            foreach (var child in instance.GetChildren())
            {
                newInstance.Children.Add(ConvertInstance(child));
            }
            
            // Return the new instance.
            return newInstance;
        }
        
        /*
         * Converts the instance to a Roblox Instance.
         */
        public Instance ToRobloxInstance(Rojo04Reader reader)
        {
            // Create the instance.
            var newInstance = reader.CreateNewInstance();
            
            // Add the properties.
            newInstance.Properties.Add("Name",new Property<object>("String",this.Name));
            newInstance.Properties.Add("ClassName",new Property<object>("String",this.ClassName));
            foreach (var propertyName in this.Properties.Keys)
            {
                newInstance.Properties.Add(propertyName,this.Properties[propertyName]);
            }
            
            // Add the children.
            foreach (var child in this.Children)
            {
                newInstance.Children.Add(child.ToRobloxInstance(reader));
            }
            
            // Return the new instance.
            return newInstance;
        }
    }
    
    /*
     * Class for writing Rojo projects.
     */
    public class Rojo04Writer
    {
        /*
         * Clears the directory of Rojo files and deletes the file if it exists.
         */
        public void DeleteDirectory(string directory)
        {
            // Return if the directory doesn't exist.
            if (!Directory.Exists(directory))
            {
                return;
            }
            
            // Delete the directories.
            foreach (var subdirectory in Directory.GetDirectories(directory))
            {
                this.DeleteDirectory(subdirectory);
            }
            
            // Delete the files if they end in the correct extensions.
            foreach (var file in Directory.GetFiles(directory))
            {
                if (file.ToLower().EndsWith(".lua") || file.ToLower().EndsWith(".model.json"))
                {
                    File.Delete(file);
                }
            }
            
            // Delete the directory if it is empty.
            if (Directory.GetDirectories(directory).Length == 0 && Directory.GetFiles(directory).Length == 0)
            {
                Directory.Delete(directory);
            }
        }
        
        /*
         * Writes a Roblox Instance to the file system.
         */
        public void WriteRobloxInstance(Instance instance,string directory)
        {
            // Get the name and class name.
            var name = (string) instance.GetProperty("Name").Value;
            var className = (string) instance.GetProperty("ClassName").Value;
            
            if (className == "Folder")
            {
                // Write a directory if it is a folder.
                var newDirectory = directory + "/" + name;
                Directory.CreateDirectory(newDirectory);
                
                // Write the children.
                foreach (var subInstance in instance.GetChildren())
                {
                    this.WriteRobloxInstance(subInstance,newDirectory);
                }
            } else if (className == "Script" || className == "LocalScript" || className == "ModuleScript")
            {
                // Determine the extension for the script.
                var extension = ".lua";
                if (className == "Script")
                {
                    extension = ".server.lua";
                }
                else if (className == "LocalScript")
                {
                    extension = ".client.lua";
                }

                // Get the source.
                var source = (string) instance.GetProperty("Source").Value;

                // Write the file based on if there is any children.
                if (instance.GetChildren().Count == 0)
                {
                    File.WriteAllText(directory + "/" + name + extension, source);
                }
                else
                {
                    var newDirectory = directory + "/" + name;

                    // Write the directory and the file.
                    Directory.CreateDirectory(newDirectory);

                    // Write the children.
                    foreach (var subInstance in instance.GetChildren())
                    {
                        this.WriteRobloxInstance(subInstance,newDirectory);
                    }
                    
                    // Write the script.
                    File.WriteAllText(directory + "/" + name + "/init" + extension, source);
                }
            } else
            {
                // Convert the instance to a Rojo instance.
                var convertedInstance = Rojo04Instance.ConvertInstance(instance);
                
                // Serialize and store the instance.
                var instanceData = JsonConvert.SerializeObject(convertedInstance,Formatting.Indented);
                File.WriteAllText(directory + "/" + name + ".model.json",instanceData);
            }
        }
    }
    
    /*
     * Class for reading Rojo projects.
     */
    public class Rojo04Reader
    {
        private int currentTempId;
        
        /*
         * Creates a Rojo 0.4 reader.
         */
        public Rojo04Reader()
        {
            this.currentTempId = 0;
        }
        
        /*
         * Creates a new Roblox instance.
         */
        public Instance CreateNewInstance()
        {
            var instance = new Instance(this.currentTempId);
            this.currentTempId += 1;
            
            return instance;
        }
        
        /*
         * Reads a directory and constructs the Roblox Instance.
         */
        public Instance ReadRobloxInstance(string directory)
        {
            directory = directory.Replace("\\", "/");
            
            if (Directory.Exists(directory))
            {
                // Get the instance.
                var newInstance = this.CreateNewInstance();
                var directoryName = FileFinder.GetUpperDirectoryName(directory);
                if (File.Exists(directory + "/init.lua") || File.Exists(directory + "/init.server.lua") || File.Exists(directory + "/init.client.lua"))
                {
                    // Get the class name.
                    string className = null;
                    string source = null;
                    if (File.Exists(directory + "/init.lua"))
                    {
                        className = "ModuleScript";
                        source = File.ReadAllText(directory + "/init.lua");
                    } else if (File.Exists(directory + "/init.server.lua"))
                    {
                        className = "Script";
                        source = File.ReadAllText(directory + "/init.server.lua");
                    } else if (File.Exists(directory + "/init.client.lua"))
                    {
                        className = "LocalScript";
                        source = File.ReadAllText(directory + "/init.client.lua");
                    }
                    
                    // Create the script.
                    newInstance.Properties.Add("Name",new Property<object>("String",directoryName));
                    newInstance.Properties.Add("ClassName",new Property<object>("String",className));
                    newInstance.Properties.Add("Source",new Property<object>("String",source));
                } else
                {
                    newInstance.Properties.Add("Name",new Property<object>("String",directoryName));
                    newInstance.Properties.Add("ClassName",new Property<object>("String","Folder"));
                }
                
                // Add the children.
                foreach (var subDirectory in Directory.GetDirectories(directory))
                {
                    var childInstance = this.ReadRobloxInstance(subDirectory);
                    if (childInstance != null)
                    {
                        newInstance.AddChild(childInstance);
                    }
                }
                
                foreach (var subFile in Directory.GetFiles(directory))
                {
                    if (!subFile.EndsWith("/init.lua") && !subFile.EndsWith("/init.server.lua") && !subFile.EndsWith("/init.client.lua"))
                    {
                        var childInstance = this.ReadRobloxInstance(subFile);
                        if (childInstance != null)
                        {
                            newInstance.AddChild(childInstance);
                        }
                    }
                }
                
                // Return the new instance.
                return newInstance;
            } else if (File.Exists(directory) && !directory.EndsWith("/init.lua") && !directory.EndsWith("/init.server.lua") && !directory.EndsWith("/init.client.lua"))
            {
                // Get the class name.
                string className = null;
                
                 if (directory.ToLower().EndsWith(".server.lua"))
                {
                    className = "Script";
                } else if (directory.ToLower().EndsWith(".client.lua"))
                {
                    className = "LocalScript";
                } else if (directory.ToLower().EndsWith(".lua")) 
                { 
                    className = "ModuleScript";
                }
                 
                // Read the script if it exists or parse the model.
                if (className != null)
                {
                    // Create and return a script instance.
                    var scriptName = FileFinder.GetUpperDirectoryName(directory).Split('.')[0];
                    var source = File.ReadAllText(directory);
                    
                    var scriptInstance = this.CreateNewInstance();
                    scriptInstance.Properties.Add("Name",new Property<object>("String",scriptName));
                    scriptInstance.Properties.Add("ClassName",new Property<object>("String",className));
                    scriptInstance.Properties.Add("Source",new Property<object>("String",source));

                    return scriptInstance;
                } else if (directory.ToLower().EndsWith(".model.json"))
                {
                    // Parse and return the model.
                    var newInstance = JsonConvert.DeserializeObject<Rojo04Instance>(File.ReadAllText(directory));
                    return newInstance.ToRobloxInstance(this);
                }
            }

            // Return null (no instance).
            return null;
        }
    }

    /*
     * Class representing a Rojo 0.4 project.
     */
    public class Rojo04 : IProject
    {
        public const string REQUIRED_FILE = "rojo.json";
        
        /*
         * Returns the project structure.
         */
        public Rojo04Structure GetStructure()
        {
            // Get the JSON file parent directory.
            var parentDirectory = FileFinder.GetParentDirectoryOfFile(REQUIRED_FILE);
            
            // Display a console error and return null if the parent directory is null.
            if (parentDirectory == null)
            {
                Console.WriteLine("The Rojo 0.4 project file (rojo.json) does not exist in the working or parent directory.");
                Console.WriteLine("If you are changing projects, please restart the server since a new port may be needed.");
                return null;
            }

            // Read and parse the JSON file.
            var jsonSource = File.ReadAllText(parentDirectory + REQUIRED_FILE);
            return JsonConvert.DeserializeObject<Rojo04Structure>(jsonSource);
        }
        
        /*
         * Returns the name of the project.
         */
        public string GetName()
        {
            return "Rojo 0.4.X";
        }
        
        /*
         * Returns if a directory is valid for the project. It should not determine if a parent
         * directory is valid because parent directories are also checked.
         */
        public bool IsDirectoryValid(string directory)
        {
            return File.Exists(directory + REQUIRED_FILE);
        }
        
        /*
         * Returns the port to serve for the server.
         */
        public int GetPort()
        {
            // Get the project structure.
            var structure = GetStructure();
            
            // Return the port. 
            return structure.servePort;
        }
        
        /*
         * Returns the partitions to use.
         */
        public Dictionary<string,string> GetPartitions()
        {
            // Get the project structure and return null if it doesn't exist.
            var structure = this.GetStructure();
            if (structure == null)
            {
                return null;
            }
            
            // Get the partitions.
            var partitions = new Dictionary<string,string>();
            foreach (var partitionData in structure.partitions.Values)
            {
                partitions.Add(partitionData["path"],partitionData["target"]);
            }
            
            // Return the partitions.
            return partitions;
        }
        
        /*
         * Writes the partitions to the file system.
         */
        public void WriteProjectStructure(Partitions partitions)
        {
            // Create the writer.
            var writer = new Rojo04Writer();

            // Write the structure.
            var workingDirectory = FileFinder.GetParentDirectoryOfFile(REQUIRED_FILE);
            foreach (var partitionLocation in partitions.Instances.Keys)
            {
                // Get the location.
                var fileLocation = workingDirectory + partitionLocation;
                var topDirectory = FileFinder.GetUpperDirectoryName(fileLocation);
                var parentLocation = FileFinder.MoveDirectoryUp(fileLocation);
                if (parentLocation == null)
                {
                    parentLocation = "";
                }
                
                // Get and modify the instance.
                var instance = partitions.GetInstance(partitionLocation);
                instance.GetProperty("Name").Value = topDirectory;
                
                // Write the instance.
                writer.DeleteDirectory(fileLocation);
                writer.WriteRobloxInstance(instance,parentLocation);
            }
        }
        
        /*
         * Returns the partitions of the project.
         */
        public Partitions ReadProjectStructure()
        {
            // Create the new partitions.
            var partitions = new Partitions();
            
            // Create the reader.
            var reader = new Rojo04Reader();
            
            // Read the folders.
            var workingDirectory = FileFinder.GetParentDirectoryOfFile(REQUIRED_FILE);
            var partitionData = this.GetPartitions();
            foreach (var partitionLocation in partitionData.Keys)
            {
                // Get the target instance and new name.
                var targetInstance = partitionData[partitionLocation];
                var splitTargetName = targetInstance.Split('.');
                var instanceName = splitTargetName[splitTargetName.Length - 1];
                
                // Get the instance and add it to the partitions if it exists.
                var instance = reader.ReadRobloxInstance(workingDirectory + partitionLocation);
                if (instance != null)
                {
                    instance.Properties["Name"].Value = instanceName;
                    partitions.AddInstance(partitionLocation,instance);
                }
            }
            
            // Return the partitions.
            return partitions;
        }
    }
}