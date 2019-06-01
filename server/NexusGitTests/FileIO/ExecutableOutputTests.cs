/*
 * TheNexusAvenger
 * 
 * Test the NexusGit.FileIO.Executable class.
 */

using NexusGit.FileIO;
using NUnit.Framework;

namespace NexusGitTests.FileIO
{
    [TestFixture]
    public class ExecutableOutputTests
    {
        /*
         * Tests the OutputContainsTest method.
         */
        [Test]
        public void OutputContainsTest()
        {
            // Create the component under testing.
            ExecutableOutput CuT = new ExecutableOutput();
            CuT.Add("Test 1");
            CuT.Add("Test 2");
            CuT.Add("Test 3");

            // Run the assertions on the output.
            Assert.IsTrue(CuT.OutputContains("Test"));
            Assert.IsTrue(CuT.OutputContains("Test 1"));
            Assert.IsTrue(CuT.OutputContains("Test 2"));
            Assert.IsTrue(CuT.OutputContains("Test 3"));
            Assert.IsFalse(CuT.OutputContains("Test 4"));
        }

        /*
         * Tests the OutputContainsAtLineTest method.
         */
        [Test]
        public void OutputContainsAtLineTest()
        {
            // Create the component under testing.
            ExecutableOutput CuT = new ExecutableOutput();
            CuT.Add("Test 1");
            CuT.Add("Test 2");
            CuT.Add("Test 3");

            // Run the assertions on the first line.
            Assert.IsTrue(CuT.OutputContainsAtLine(0,"Test"));
            Assert.IsTrue(CuT.OutputContainsAtLine(0,"Test 1"));
            Assert.IsFalse(CuT.OutputContainsAtLine(0,"Test 2"));
            Assert.IsFalse(CuT.OutputContainsAtLine(0,"Test 3"));
            Assert.IsFalse(CuT.OutputContainsAtLine(0,"Test 4"));

            // Run the assertions on the second line.
            Assert.IsTrue(CuT.OutputContainsAtLine(1,"Test"));
            Assert.IsFalse(CuT.OutputContainsAtLine(1,"Test 1"));
            Assert.IsTrue(CuT.OutputContainsAtLine(1,"Test 2"));
            Assert.IsFalse(CuT.OutputContainsAtLine(1,"Test 3"));
            Assert.IsFalse(CuT.OutputContainsAtLine(1,"Test 4"));

            // Run the assertions on the third line.
            Assert.IsTrue(CuT.OutputContainsAtLine(2,"Test"));
            Assert.IsFalse(CuT.OutputContainsAtLine(2,"Test 1"));
            Assert.IsFalse(CuT.OutputContainsAtLine(2,"Test 2"));
            Assert.IsTrue(CuT.OutputContainsAtLine(2,"Test 3"));
            Assert.IsFalse(CuT.OutputContainsAtLine(2,"Test 4"));

            // Run the assertions on a non-existent line.
            Assert.IsFalse(CuT.OutputContainsAtLine(3,"Test"));
            Assert.IsFalse(CuT.OutputContainsAtLine(3,"Test 1"));
            Assert.IsFalse(CuT.OutputContainsAtLine(3,"Test 2"));
            Assert.IsFalse(CuT.OutputContainsAtLine(3,"Test 3"));
            Assert.IsFalse(CuT.OutputContainsAtLine(3,"Test 4"));
            Assert.IsFalse(CuT.OutputContainsAtLine(-1,"Test"));
            Assert.IsFalse(CuT.OutputContainsAtLine(-1,"Test 1"));
            Assert.IsFalse(CuT.OutputContainsAtLine(-1,"Test 2"));
            Assert.IsFalse(CuT.OutputContainsAtLine(-1,"Test 3"));
            Assert.IsFalse(CuT.OutputContainsAtLine(-1,"Test 4"));
        }
    }
}