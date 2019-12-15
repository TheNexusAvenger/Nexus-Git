/*
 * TheNexusAvenger
 * 
 * Tests the Rojo class.
 */

using NexusGit.NexusGit.Projects.SupportedProjects.Rojo;
using NUnit.Framework;

namespace NexusGitTests.NexusGit.Projects.SupportedProjects.Rojo
{
    [TestFixture]
    public class RojoTests
    {
        /*
         * Tests the GetFile method of RojoFile.
         */
        [Test]
        public void TestGetFile()
        {
            // Create several components under testing.
            var CuT1 = new RojoFile("File1");
            var CuT2 = new RojoFile("File2");
            var CuT3 = new RojoFile("file3");
            var CuT4 = new RojoFile("file4");
            CuT1.AddFile(CuT2);
            CuT1.AddFile(CuT3);
            CuT3.AddFile(CuT4);
            
            // Assert the files are correct.
            Assert.AreEqual(CuT1.GetFile("file1"),null);
            Assert.AreEqual(CuT1.GetFile("File1"),null);
            Assert.AreEqual(CuT1.GetFile("file5"),null);
            Assert.AreEqual(CuT1.GetFile("file2"),CuT2);
            Assert.AreEqual(CuT1.GetFile("File2"),CuT2);
            Assert.AreEqual(CuT1.GetFile("file3"),CuT3);
            Assert.AreEqual(CuT1.GetFile("File3"),CuT3);
            Assert.AreEqual(CuT1.GetFile("file4"),null);
            Assert.AreEqual(CuT1.GetFile("File4"),null);
            Assert.AreEqual(CuT3.GetFile("file4"),CuT4);
            Assert.AreEqual(CuT3.GetFile("File4"),CuT4);
            
            // Assert that paths are correct.
            Assert.AreEqual(CuT1.GetFile("file1/file2"),null);
            Assert.AreEqual(CuT1.GetFile("file2/file4"),null);
            Assert.AreEqual(CuT1.GetFile("file3/file4"),CuT4);
            Assert.AreEqual(CuT1.GetFile("file3/File4"),CuT4);
            Assert.AreEqual(CuT1.GetFile("File3/File4"),CuT4);
            Assert.AreEqual(CuT1.GetFile("File3/File4"),CuT4);
        }
        
        /*
         * Tests the FileExists method of RojoFile.
         */
        [Test]
        public void TestFileExists()
        {
            // Create several components under testing.
            var CuT1 = new RojoFile("File1");
            var CuT2 = new RojoFile("File2");
            var CuT3 = new RojoFile("file3");
            var CuT4 = new RojoFile("file4");
            CuT1.AddFile(CuT2);
            CuT1.AddFile(CuT3);
            CuT3.AddFile(CuT4);
            
            // Assert the files are correct.
            Assert.IsFalse(CuT1.FileExists("file1"));
            Assert.IsFalse(CuT1.FileExists("File1"));
            Assert.IsFalse(CuT1.FileExists("file5"));
            Assert.IsTrue(CuT1.FileExists("file2"));
            Assert.IsTrue(CuT1.FileExists("File2"));
            Assert.IsTrue(CuT1.FileExists("file3"));
            Assert.IsTrue(CuT1.FileExists("File3"));
            Assert.IsFalse(CuT1.FileExists("file4"));
            Assert.IsFalse(CuT1.FileExists("File4"));
            Assert.IsTrue(CuT3.FileExists("file4"));
            Assert.IsTrue(CuT3.FileExists("File4"));
            
            // Assert that paths are correct.
            Assert.IsFalse(CuT1.FileExists("file1/file2"));
            Assert.IsFalse(CuT1.FileExists("file2/file4"));
            Assert.IsTrue(CuT1.FileExists("file3/file4"));
            Assert.IsTrue(CuT1.FileExists("file3/File4"));
            Assert.IsTrue(CuT1.FileExists("File3/File4"));
            Assert.IsTrue(CuT1.FileExists("File3/File4"));
        }
        
        /*
         * Tests the AddFile method of RojoFile.
         */
        [Test]
        public void TestAddFile()
        {
            // Create several components under testing.
            var CuT1 = new RojoFile("File1");
            var CuT2 = new RojoFile("File2");
            var CuT3A = new RojoFile("file3");
            var CuT3B = new RojoFile("file3");
            
            // Add the files and assert the parents and contains are correct.
            CuT1.AddFile(CuT2);
            CuT1.AddFile(CuT3A);
            Assert.AreEqual(CuT1.SubFiles.Count,2);
            Assert.AreEqual(CuT1.Parent, null);
            Assert.AreEqual(CuT2.Parent, CuT1);
            Assert.AreEqual(CuT3A.Parent, CuT1);
            Assert.AreEqual(CuT3B.Parent, null);
            
            // Add a file with the same name and assert it replaces it.
            CuT1.AddFile(CuT3B);
            Assert.AreEqual(CuT1.SubFiles.Count,2);
            Assert.AreEqual(CuT1.Parent, null);
            Assert.AreEqual(CuT2.Parent, CuT1);
            Assert.AreEqual(CuT3A.Parent, null);
            Assert.AreEqual(CuT3B.Parent, CuT1);
        }
        
        /*
         * Tests the RemoveFile method of RojoFile.
         */
        [Test]
        public void TestRemoveFile()
        {
            // Create several components under testing.
            var CuT1 = new RojoFile("File1");
            var CuT2 = new RojoFile("File2");
            var CuT3 = new RojoFile("file3");
            CuT1.AddFile(CuT2);
            CuT1.AddFile(CuT3);
            
            // Remove a non-existent file and assert the files still exist.
            Assert.AreEqual(CuT1.RemoveFile("file4"),null);
            Assert.IsTrue(CuT1.FileExists("file2"));
            Assert.IsTrue(CuT1.FileExists("File3"));
            
            // Remove a file and assert the correct file is removed.
            Assert.AreEqual(CuT1.RemoveFile("FILE3"),CuT3);
            Assert.IsTrue(CuT1.FileExists("file2"));
            Assert.IsFalse(CuT1.FileExists("File3"));
        }
        
        /*
         * Tests the CorrectParents method of RojoFile.
         */
        [Test]
        public void CorrectParents()
        {
            // Create several components under testing.
            var CuT1 = new RojoFile("File1");
            var CuT2 = new RojoFile("File2");
            var CuT3 = new RojoFile("file3");
            var CuT4 = new RojoFile("file4");
            CuT1.Parent = CuT2;
            CuT1.SubFiles.Add(CuT2);
            CuT1.SubFiles.Add(CuT3);
            CuT3.SubFiles.Add(CuT4);
            
            // Correct the parents and assert they are correct.
            CuT1.CorrectParents();
            Assert.AreEqual(CuT1.Parent,null);
            Assert.AreEqual(CuT2.Parent,CuT1);
            Assert.AreEqual(CuT3.Parent,CuT1);
            Assert.AreEqual(CuT4.Parent,CuT3);
        }
    }
}