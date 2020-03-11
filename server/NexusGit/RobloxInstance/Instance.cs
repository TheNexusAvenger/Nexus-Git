/*
 * TheNexusAvenger
 * 
 * Stores information about Roblox instances.
 */

using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NexusGit.RobloxInstance
{
    /*
     * Class representing a Roblox instance.
     */
    public class Instance
    {
        public string Type = "Instance";
        public int TemporaryId;
        public Dictionary<string,Property<Object>> Properties;
        public List<Instance> Children;

        /*
         * Creates a Roblox Instance object.
         */
        public Instance(int temporaryId)
        {
            this.TemporaryId = temporaryId;
            this.Properties = new Dictionary<string,Property<Object>>();
            this.Children = new List<Instance>();
        }

        /*
         * Adds a child object.
         */
        public void AddChild(Instance instance)
        {
            this.Children.Add(instance);
        }

        /*
         * Returns the children objects.
         */
        public List<Instance> GetChildren()
        {
            return this.Children;
        }

        /*
         * Sets a property of the instance.
         */
        public void SetProperty(string name,Property<Object> property)
        {
            this.Properties.Add(name,property);
        }

        /*
         * Returns the value of a property.
         */
        public Property<Object> GetProperty(string name)
        {
            return this.Properties[name];
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
