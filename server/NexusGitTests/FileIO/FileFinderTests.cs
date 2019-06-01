/*
 * TheNexusAvenger
 *
 * Test the NexusGit.FileIO.FileFinder class.
 */

using NexusGit.FileIO;
using NUnit.Framework;

namespace NexusGitTests.FileIO
{
    [TestFixture]
    public class FileFinderTests
    {
        /*
         * Tests the MoveDirectoryUp method.
         */
        [Test]
        public void MoveDirectoryUpTest()
        {
            // Run the tests for a Microsoft based file system.
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:/Test Directory1/Test Directory 2"),"C:/Test Directory1/");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:/Test Directory1/Test Directory 2/"),"C:/Test Directory1/");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:/"),null);
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:/"),null);
            
            // Run the tests for a Unix based file system.
            Assert.AreEqual(FileFinder.MoveDirectoryUp("/Test Directory1/Test Directory 2"),"/Test Directory1/");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("/Test Directory1/Test Directory 2/"),"/Test Directory1/");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("/Test Directory1/"),"/");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("/"),null);
        }
    }
}