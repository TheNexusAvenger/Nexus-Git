/*
 * TheNexusAvenger
 *
 * Test the NexusGit.FileIO.FileFinder class.
 */

using System;
using System.IO;
using System.Linq;
using NexusGit.FileIO;
using NUnit.Framework;

namespace NexusGitTests.FileIO
{
    [TestFixture]
    public class FileFinderTests
    {
        /*
         * Tests the GetPathSeparatorFromPath method.
         */
        [Test]
        public void TestGetPathSeparatorFromPath()
        {
            Assert.AreEqual(FileFinder.GetPathSeparatorFromPath("C:/Test1/Test2"), '/');
            Assert.AreEqual(FileFinder.GetPathSeparatorFromPath("C:\\Test1\\Test2"), '\\');
            Assert.AreEqual(FileFinder.GetPathSeparatorFromPath("C:/Test1\\Test2"), Path.PathSeparator);
            Assert.AreEqual(FileFinder.GetPathSeparatorFromPath(""), Path.PathSeparator);
        }
        
        /*
         * Tests the MoveDirectoryUp method.
         */
        [Test]
        public void TestMoveDirectoryUp()
        {
            // Run the tests for a Microsoft based file system.
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:/Test Directory1/Test Directory 2"),"C:/Test Directory1/");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:/Test Directory1/Test Directory 2/"),"C:/Test Directory1/");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:/"),null);
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:/"),null);
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:\\Test Directory1\\Test Directory 2"),"C:\\Test Directory1\\");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:\\Test Directory1\\Test Directory 2\\"),"C:\\Test Directory1\\");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:\\"),null);
            Assert.AreEqual(FileFinder.MoveDirectoryUp("C:\\"),null);
            
            // Run the tests for a Unix based file system.
            Assert.AreEqual(FileFinder.MoveDirectoryUp("/Test Directory1/Test Directory 2"),"/Test Directory1/");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("/Test Directory1/Test Directory 2/"),"/Test Directory1/");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("/Test Directory1/"),"/");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("/"),null);
            Assert.AreEqual(FileFinder.MoveDirectoryUp("\\Test Directory1\\Test Directory 2"),"\\Test Directory1\\");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("\\Test Directory1\\Test Directory 2\\"),"\\Test Directory1\\");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("\\Test Directory1\\"),"\\");
            Assert.AreEqual(FileFinder.MoveDirectoryUp("\\"),null);
        }
    }
}