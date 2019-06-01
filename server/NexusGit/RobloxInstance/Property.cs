/*
 * TheNexusAvenger
 * 
 * Stores the value of a property.
 */

using Newtonsoft.Json;

namespace NexusGit.RobloxInstance
{
    /*
     * Class representing a property builder.
     */
    public class PropertyBuilder<T>
    {
        /*
         * Returns an ISerializable object from a string.
         */
        public Property<T> Deserialize(string data)
        {
            return JsonConvert.DeserializeObject<Property<T>>(data);
        }
    }

    /*
     * Class representing a property.
     */
    public class Property<T>
    {
        public string Type;
        public T Value;

        /*
         * Creates a Roblox Instance object.
         */
        public Property(string type,T value)
        {
            this.Type = type;
            this.Value = value;
        }

        /*
         * Returns the ISerializable as a string.
         */
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}