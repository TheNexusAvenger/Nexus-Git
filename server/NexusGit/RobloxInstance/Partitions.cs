/*
 * TheNexusAvenger
 *
 * Stores a map of Roblox instances with associated keys.
 */

using System.Collections.Generic;
using Newtonsoft.Json;

namespace NexusGit.RobloxInstance
{
    /*
     * Class representing a build for Roblox instances.
     */
    public class PartitionsBuilder
    {
        /*
         * Returns an ISerializable object from a string.
         */
        public Partitions Deserialize(string data)
        {
            return JsonConvert.DeserializeObject<Partitions>(data);
        }
    }

    /*
     * Class representing a set of partitions.
     */
    public class Partitions
    {
        public string Type = "Partitions";
        public Dictionary<string,Instance> Instances;
        
        /*
         * Creates a partitions object.
         */
        public Partitions()
        {
            this.Instances = new Dictionary<string,Instance>();
        }

        /*
         * Adds an instance.
         */
        public void AddInstance(string name,Instance instance)
        {
            this.Instances.Add(name,instance);
        }

        /*
         * Returns the instance for the game.
         */
        public Instance GetInstance(string name)
        {
            // Return the instance if it exists.
            if (this.Instances.ContainsKey(name))
            {
                return this.Instances[name];
            }

            // Return null (not found).
            return null;
        }

        /*
         * Returns the ISerializable as a string.
         */
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented);
        }
    }
}