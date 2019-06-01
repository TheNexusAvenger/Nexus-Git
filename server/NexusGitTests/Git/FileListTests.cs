/*
 * TheNexusAvenger
 * 
 * Test the NexusGit.Git.FileList class.
 */

using NexusGit.Git;
using NUnit.Framework;

namespace NexusGitTests.Git
{
    [TestFixture]
    public class FileListTests
    {
        /*
         * Tests the ToJSON method.
         */
        [Test]
        public void GetFilesAsString()
        {
            // Create the component under testing.
            FileList CuT = new FileList();

            // Add 3 files.
            CuT.Add("Test 1.txt");
            CuT.Add("Test 2.txt");
            CuT.Add("Test \"3\".txt");

            // Assert it is serialized correctly.
            Assert.AreEqual(CuT.GetFilesAsString(),"\"Test 1.txt\" \"Test 2.txt\" \"Test \\\"3\\\".txt\" ");
        }
    }
}