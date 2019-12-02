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
            return null;
        }
        
        /*
         * Returns the partitions to use.
         */
        public override Dictionary<string,string> GetPartitions()
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
        public override void WriteProjectStructure(Partitions partitions)
        {
            // Create the writer.
            var writer = new Rojo04Writer();

            // Write the structure.
            var workingDirectory = FileFinder.GetParentDirectoryOfFile(this.GetRequiredFile());
            foreach (var partitionLocation in partitions.Instances.Keys)
            {
                var instance = partitions.GetInstance(partitionLocation);
                if (instance != null)
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
                    instance.GetProperty("Name").Value = topDirectory;

                    // Write the instance.
                    writer.DeleteDirectory(fileLocation);
                    writer.WriteRobloxInstance(instance, parentLocation);
                }
            }
        }
    }
}