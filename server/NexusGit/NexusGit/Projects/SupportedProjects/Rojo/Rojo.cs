/*
 * TheNexusAvenger
 *
 * Base class for implementations of Rojo.
 */

using System.Collections.Generic;
using System.IO;
using NexusGit.FileIO;
using NexusGit.RobloxInstance;

namespace NexusGit.NexusGit.Projects.SupportedProjects.Rojo
{
    /*
     * Class representing a file or directory. If it is a
     * directory, it will have no contents. This is used instead
     * of just reading files for testing purposes.
     */
    public class RojoFile
    {
        public List<RojoFile> SubFiles;
        public string Name;
        public string Contents;
        
        /*
         * Creates a Rojo file.
         */
        public RojoFile(string name) {
            this.Name = name;
            this.SubFiles = new List<RojoFile>();
        }
        
        /*
         * Returns the file for a given name.
         */
        public RojoFile GetFile(string name) {
            name = name.ToLower();
            
            // Return the file if it exists.
            foreach (var file in this.SubFiles) {
                if (file.Name.ToLower() == name) {
                    return file;
                }
            }
            
            // Return null (not found).
            return null;
        }
        
        /*
         * Returns if a file exists.
         */
        public bool FileExists(string name) {
            return this.GetFile(name) != null;
        }
        
        /*
         * Removes a file.
         */
        public RojoFile RemoveFile(string name) {
            // Get the file.
            var file = this.GetFile(name);
            
            // Remove and return the file.
            if (file != null) {
                this.SubFiles.Remove(file);
            }
            return file;
        }
        
        /*
         * Creates a Rojo file from a file.
         */
        public static RojoFile FromFile(string location)
        {
            // Create the base file.
            var file = new RojoFile(Path.GetFileName(location));
            
            // Handle the file being a file or a directory.
            if (Directory.Exists(location))
            {
                // Add the children.
                foreach (var subFile in Directory.GetFiles(location))
                {
                    file.SubFiles.Add(FromFile(subFile));
                }
            } else if (File.Exists(location))
            {
                // Add the contents.
                file.Contents = File.ReadAllText(location);
            }
            
            // Return the file.
            return file;
        }
    }
    
    /*
     * Class representing a Roblox instance.
     */
    public class RojoInstance
    {
        public string Name; 
        public string ClassName;
        public List<RojoInstance> Children;
        public Dictionary<string,Property<object>> Properties;
        
        /*
         * Creates a Rojo instance.
         */
        public RojoInstance()
        {
            this.Children = new List<RojoInstance>();
            this.Properties = new Dictionary<string,Property<object>>();
        }

        /*
         * Converts an Instance to a RojoInstance.
         */
        public static RojoInstance ConvertInstance(Instance instance)
        {
            // Create the new instance.
            var newInstance = new RojoInstance();
               
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
         * Converts the Rojo Instance to a Instance.
         */
        public Instance ToRobloxInstance(TemporaryIdIncrementer reader)
        {
            // Create the instance.
            var newInstance = new Instance(reader.GetNextId());
            
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
     * Class for storing the current temporary id.
     */
    public class TemporaryIdIncrementer
    {
        private int CurrentTemporaryId;
        
        /*
         * Creates a Temporary Id Incrementer.
         */
        public TemporaryIdIncrementer()
        {
            this.CurrentTemporaryId = -1;
        }
        
        /*
         * Returns the next temporary id.
         */
        public int GetNextId()
        {
            this.CurrentTemporaryId += 1;
            return this.CurrentTemporaryId;
        }
    }
    /*
     * Abstract class for a Rojo project.
     */
    public abstract class Rojo : IProject
    {
        /*
         * Returns if a directory is valid for the project. It should not determine if a parent
         * directory is valid because parent directories are also checked.
         */
        public bool IsDirectoryValid(string directory)
        {
            return File.Exists(directory + this.GetRequiredFile());
        }
        
        /*
         * Returns a Roblox instance for a given file or directory.
         */
        public RojoInstance GetFromFile(string file)
        {
            // Create the Rojo file structure.
            var rojoFile = RojoFile.FromFile(file);
            
            // Return the instances.
            return this.GetFromFile(rojoFile);
        }
        
        /*
         * Returns the partitions of the project.
         */
        public Partitions ReadProjectStructure()
        {
            // Create the new partitions.
            var partitions = new Partitions();
            
            // Read the folders.
            var incrementer = new TemporaryIdIncrementer();
            var workingDirectory = FileFinder.GetParentDirectoryOfFile(this.GetRequiredFile());
            var partitionData = this.GetPartitions();
            foreach (var partitionLocation in partitionData.Keys)
            {
                // Get the target instance and new name.
                var targetInstance = partitionData[partitionLocation];
                var splitTargetName = targetInstance.Split('.');
                var instanceName = splitTargetName[splitTargetName.Length - 1];
                
                // Get the instance and add it to the partitions if it exists.
                var instance = this.GetFromFile(workingDirectory + partitionLocation);
                if (instance != null) {
                    instance.Name = instanceName;
                    partitions.AddInstance(partitionLocation,instance.ToRobloxInstance(incrementer));
                }
            }
            
            // Return the partitions.
            return partitions;
        }
        
        /*
         * Returns a Roblox instance for a given file or directory.
         */
        public abstract RojoInstance GetFromFile(RojoFile file);
        
        /*
         * Returns a Roblox instance for a given file or directory.
         */
        public abstract RojoFile GetFile(RojoInstance instance);
        
        /*
         * Returns the file required for the Rojo project.
         */
        public abstract string GetRequiredFile();
        
        /*
         * Returns the name of the project.
         */
        public abstract string GetName();
        
        /*
         * Returns the port to serve for the server.
         */
        public abstract int GetPort();
        
        /*
         * Returns the partitions to use.
         */
        public abstract Dictionary<string,string> GetPartitions();
        
        /*
         * Writes the partitions to the file system.
         */
        public abstract void WriteProjectStructure(Partitions partitions);
    }
}