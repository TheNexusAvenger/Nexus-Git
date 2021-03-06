﻿/*
 * TheNexusAvenger
 * 
 * Test the NexusGit.RobloxInstance.Properties.Property and
 * NexusGit.RobloxInstance.Properties.PropertyBuilder classses.
 */

using NexusGit.RobloxInstance;
using NUnit.Framework;

namespace NexusGitTests.RobloxInstance
{
    [TestFixture]
    public class PropertyBuilderTests
    {
        /*
         * Tests the Deserialize method.
         */
        [Test]
        public void DeserializeTest()
        {
            // Create the component under testing.
            var CuT = new PropertyBuilder<bool>();

            // Assert it deserializes JSON correctly.
            var property = (Property<bool>) CuT.Deserialize("{\"Type\":\"Bool\",\"Value\":true}");
            Assert.AreEqual(property.Value,true);
        }
    }

    [TestFixture]
    public class PropertyTests
    {
        /*
         * Tests the Serialize method.
         */
        [Test]
        public void SerializeTest()
        {
            // Create the component under testing.
            var CuT = new Property<bool>("Bool",true);

            // Assert it is serialized correctly.
            var correctSerialization = "{\"Type\":\"Bool\",\"Value\":true}";
            Assert.AreEqual(correctSerialization,CuT.Serialize());
        }
    }
}