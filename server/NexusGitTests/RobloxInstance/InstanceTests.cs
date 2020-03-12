/*
 * TheNexusAvenger
 *
 * Tests the Instance class.
 */

using System.Collections.Generic;
using NexusGit.RobloxInstance;
using NUnit.Framework;

namespace NexusGitTests.RobloxInstance
{
    [TestFixture]
    public class InstanceTests
    {
        /*
         * Tests the constructor.
         */
        [Test]
        public void ConstructorTest()
        {
            var CuT = new Instance(4);
            Assert.AreEqual(CuT.Type,"Instance","Type is incorrect.");
            Assert.AreEqual(CuT.TemporaryId,4,"Temporary id is incorrect.");
        }
        
        /*
         * Tests the AddChild method.
         */
        [Test]
        public void AddChildTest()
        {
            // Create the components under testing.
            var CuT1 = new Instance(0);
            var CuT2 = new Instance(1);
            
            // Add the child and assert it was correct.
            CuT1.AddChild(CuT2);
            Assert.AreEqual(CuT1.GetChildren(),new List<Instance>() { CuT2 },"Child was not added.");
            Assert.AreEqual(CuT2.GetChildren(),new List<Instance>(),"Child was incorrectly added.");
            Assert.AreEqual(CuT1.Serialize(),"{\r\n  \"Type\": \"Instance\",\r\n  \"TemporaryId\": 0,\r\n  \"Properties\": {},\r\n  \"Children\": [\r\n    {\r\n      \"Type\": \"Instance\",\r\n      \"TemporaryId\": 1,\r\n      \"Properties\": {},\r\n      \"Children\": []\r\n    }\r\n  ]\r\n}","Serialization is incorrect.");
        }

        /*
         * Tests the SetProperty method.
         */
        [Test]
        public void SetPropertyTest()
        {
            // Create the component under testing and properties.
            var CuT = new Instance(0);
            var property1 = new Property<object>("Name","Value 1");
            var property2 = new Property<object>("Name","Value 2");
            
            // Set an initially unset property and assert it is correct.
            CuT.SetProperty("Property1",property1);
            Assert.AreEqual(CuT.GetProperty("Property1").Value,"Value 1","Value is incorrect.");
            Assert.IsNull(CuT.GetProperty("Property2"),"Property is defined.");
            
            // Set an previously set property and assert it is correct.
            CuT.SetProperty("Property1",property2);
            Assert.AreEqual(CuT.GetProperty("Property1").Value,"Value 2","Value is incorrect.");
            Assert.IsNull(CuT.GetProperty("Property2"),"Property is defined.");
        }
    }
}