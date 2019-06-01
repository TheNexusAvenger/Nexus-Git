/*
 * TheNexusAvenger
 * 
 * Test the NexusGit.SplitRequestHttp.TemporaryStorage class.
 */

using NexusGit.SplitRequestHttp;
using NUnit.Framework;

namespace NexusGitTests.SplitRequestHttp
{
    [TestFixture]
    public class TemporaryStorageTests
    {
        [Test]
        public void StoreTest()
        {
            // Create the component under testing.
            TemporaryStorage<string> CuT = new TemporaryStorage<string>();

            // Assert adding strings.
            Assert.AreEqual(CuT.Store("Test1"),0);
            Assert.AreEqual(CuT.Store("Test2"),1);
            Assert.AreEqual(CuT.Store("Test3"),2);
        }

        [Test]
        public void GetTest()
        {
            // Create the component under testing.
            TemporaryStorage<string> CuT = new TemporaryStorage<string>();

            // Assert adding strings.
            Assert.AreEqual(CuT.Store("Test1"),0);
            Assert.AreEqual(CuT.Store("Test2"),1);
            Assert.AreEqual(CuT.Store("Test3"),2);

            // Assert the strings can be fetched correctly.
            Assert.AreEqual(CuT.Get(0),"Test1");
            Assert.AreEqual(CuT.Get(1),"Test2");
            Assert.AreEqual(CuT.Get(2),"Test3");
        }

        [Test]
        public void RemoveTest()
        {
            // Create the component under testing.
            TemporaryStorage<string> CuT = new TemporaryStorage<string>();

            // Assert adding strings.
            Assert.AreEqual(CuT.Store("Test1"),0);
            Assert.AreEqual(CuT.Store("Test2"),1);
            Assert.AreEqual(CuT.Store("Test3"),2);

            // Assert the strings can be fetched correctly.
            Assert.AreEqual(CuT.Get(0),"Test1");
            Assert.AreEqual(CuT.Get(1),"Test2");
            Assert.AreEqual(CuT.Get(2),"Test3");

            // Remove 2 strings and assert they were removed.
            CuT.Remove(0);
            CuT.Remove(2);
            Assert.AreEqual(CuT.Get(0),null);
            Assert.AreEqual(CuT.Get(1),"Test2");
            Assert.AreEqual(CuT.Get(2),null);

            // Assert adding new strings.
            Assert.AreEqual(CuT.Store("Test4"),0);
            Assert.AreEqual(CuT.Store("Test5"),2);
            Assert.AreEqual(CuT.Store("Test6"),3);

            // Assert the strings can be fetched correctly.
            Assert.AreEqual(CuT.Get(0),"Test4");
            Assert.AreEqual(CuT.Get(1),"Test2");
            Assert.AreEqual(CuT.Get(2),"Test5");
            Assert.AreEqual(CuT.Get(3),"Test6");
        }
    }
}