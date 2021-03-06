/*
 * TheNexusAvenger
 *
 * Base class for implementations of Rojo.
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using NexusGit.FileIO;
using NexusGit.Git;
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
        public static readonly List<string> FILE_ENDINGS = new List<string>()
        {
            ".lua",
            ".model.json",
            ".meta.json",
            ".rbxm",
            ".rbxmx",
            ".txt",
            ".cvs"
        };
        
        public List<RojoFile> SubFiles;
        public string Name;
        public string Contents;
        public RojoFile Parent;

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
            
            // Return a subfile if a slash exists.
            name = name.Replace("\\","/");
            if (name.Contains("/"))
            {
                // Split the string.
                var splitElements = name.Split("/",2);
                
                // Get the file and return null if it doesn't exist.
                var subFile = this.GetFile(splitElements[0]);
                if (subFile == null)
                {
                    return null;
                }
                
                // Return the sub-file's result.
                return subFile.GetFile(splitElements[1]);
            }
            
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
            if (file != null)
            {
                file.Parent = null;
                this.SubFiles.Remove(file);
            }
            return file;
        }
        
        /*
         * Adds a file. If a file exists with the same
         * name, the old file is removed.
         */
        public void AddFile(RojoFile file)
        {
            this.RemoveFile(file.Name);
            this.SubFiles.Add(file);
            file.Parent = this;
        }
        
        /*
         * Creates a Rojo file from a file.
         */
        public static RojoFile FromFile(string location)
        {
            location = location.Replace("\\", "/");
            
            // Return null if the file doesn't exist.
            if (!File.Exists(location) && !Directory.Exists(location))
            {
                return null;
            }
            
            // Return null if the file ending is invalid.
            if (File.Exists(location))
            {
                var endingFound = false;
                foreach (var ending in FILE_ENDINGS)
                {
                    if (location.ToLower().EndsWith(ending))
                    {
                        endingFound = true;
                        break;
                    }
                }

                if (!endingFound)
                {
                    return null;
                }
            }
            
            // Create the base file.
            var file = new RojoFile(Path.GetFileName(location));
            
            // Handle the file being a file or a directory.
            if (Directory.Exists(location))
            {
                // Add the children.
                foreach (var subFile in Directory.GetDirectories(location))
                {
                    var subFileObject = FromFile(subFile);
                    if (subFileObject != null)
                    {
                        file.SubFiles.Add(subFileObject);
                        subFileObject.Parent = file;
                    }
                }
                foreach (var subFile in Directory.GetFiles(location))
                {
                    var subFileObject = FromFile(subFile);
                    if (subFileObject != null)
                    {
                        file.SubFiles.Add(subFileObject);
                        subFileObject.Parent = file;
                    }
                }
            } else
            {
                // Add the contents.
                file.Contents = File.ReadAllText(location);
            }
            
            // Return the file.
            return file;
        }
        
        /*
         * Writes a Rojo file.
         */
        public void WriteFile(string location,List<string> extensionsToRemove,Submodules submodules) {
            // Return if the file is in a submodule.
            if (submodules != null && submodules.IsInSubmodule(location))
            {
                return;
            }
            
            if (this.Contents == null) {
                // Create the directory if it doesn't exist.
                if (!Directory.Exists(location)) {
                    Directory.CreateDirectory(location);
                }
                
                // Write the children.
                foreach (var child in this.SubFiles) {
                    child.WriteFile(Path.Combine(location, child.Name), extensionsToRemove,submodules);
                }
                
                // Clear the old files.
                var filesList =  Directory.GetFiles(location).Union(Directory.GetDirectories(location)).ToArray();
                foreach (var subFilePath in filesList) {
                    var subFileName = Path.GetFileName(subFilePath);
                    if (File.Exists(subFilePath) && !this.FileExists(subFileName)) {
                        foreach (var extension in extensionsToRemove) {
                            if (subFilePath.ToLower().EndsWith(extension.ToLower())) {
                                File.Delete(subFilePath);
                                break;
                            }
                        }
                    } else if (Directory.Exists(subFilePath) && Directory.GetDirectories(subFilePath).Length == 0 && Directory.GetFiles(subFilePath).Length == 0) {
                        Directory.Delete(subFilePath);
                    }
                }
                
                // Clear the directory if it is empty.
                if (Directory.GetFiles(location).Length == 0 && Directory.GetDirectories(location).Length == 0) {
                    Directory.Delete(location);
                }
            } else {
                File.WriteAllText(location,this.Contents);
            }
        }

        /*
         * Writes a Rojo file.
         */
        public void WriteFile(string location) {
            // Get the submodules.
            var submodulesDirectory = FileFinder.GetParentDirectoryOfFile(".gitmodules");
            Submodules submodules = null;
            if (submodulesDirectory != null)
            {
                submodules = Submodules.FromDirectory(submodulesDirectory);
            }
            
            // Write the files.
            this.WriteFile(location,FILE_ENDINGS,submodules);
        }
        
        /*
         * Corrects the parents of the child files.
         * Intended to be used for unit tests.
         */
        public void CorrectParents()
        {
            // Unset the parent if the parent doesn't have the file.
            if (this.Parent != null && !this.Parent.SubFiles.Contains(this))
            {
                this.Parent = null;
            }
            
            // Correct the sub-files.
            foreach (var subFile in this.SubFiles)
            {
                subFile.Parent = this;
                subFile.CorrectParents();
            }
        }
        
        /*
         * Creates empty files for a given path.
         */
        public RojoFile CreateEmptyFilesToPath(string path)
        {
            // If the path is empty, return itself.
            if (path == "")
            {
                return this;
            }
            
            // Split the path.
            var splitSections = path.Replace("\\","/").Split("/",2).ToList();
            if (splitSections[0] == "")
            {
                splitSections.RemoveAt(0);
            }
            
            // Create and return the new file.
            var newFile = this.GetFile(splitSections[0]);
            if (newFile == null)
            {
                newFile = new RojoFile(splitSections[0]);
                this.AddFile(newFile);
            }
            if (splitSections.Count == 2)
            {
                return newFile.CreateEmptyFilesToPath(splitSections[1]);
            }
            return newFile;
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
        
        /*
         * Returns an instance of a class name exists in the tree.
         */
        public bool ChildOfClassExists(string className)
        {
            // Return true if a child of the class name exists.
            foreach (var child in this.Children)
            {
                if (child.ClassName == className || child.ChildOfClassExists(className))
                {
                    return true;
                }
            }
            
            // Return false (not found).
            return false;
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
            if (rojoFile != null)
            {
                return this.GetFromFile(rojoFile);
            }
            
            // Return null (file doesn't exist).
            return null;
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
         * Returns the partitions of the project.
         */
        public abstract Partitions ReadProjectStructure();

        /*
         * Writes the partitions to the file system.
         */
        public abstract void WriteProjectStructure(Partitions partitions);
        
        /*
         * Returns the partitions to use.
         */
        public abstract Dictionary<string,bool> GetPartitions();
    }
}