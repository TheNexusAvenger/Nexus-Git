/*
 * TheNexusAvenger
 *
 * Tests the Submodules class.
 */

using System.IO;
using NexusGit.Git;
using NUnit.Framework;

namespace NexusGitTests.Git
{
    public class SubmodulesTests
    {
        /*
         * Tests the constructor with an empty file.
         */
        [Test]
        public void TestConstructorEmptySource()
        {
            // Create the component under testing.
            var CuT = new Submodules("test1/test2","");
            
            // Assert the modules are correct.
            Assert.AreEqual(CuT.Modules.Count,0,"Amount of modules is incorrect.");
        }
        
        /*
         * Tests the constructor with multiple modules.
         */
        [Test]
        public void TestConstructorMultipleModules()
        {
            // Create the component under testing.
            var CuT = new Submodules("/test1/test2","[submodule \"module/test3\"]\n\tpath = module/test3\n\turl = https://github.com/Account/test3.git\n[submodule \"module/test4\"]\n\tpath = module/test4\n\turl = https://github.com/Account/test4.git");
            
            // Assert the modules are correct.
            Assert.AreEqual(CuT.Modules.Count,2,"Amount of modules is incorrect.");
            Assert.AreEqual(CuT.Modules[0].Name,"module/test3","Name is incorrect.");
            Assert.AreEqual(CuT.Modules[0].Path,Path.Combine("/test1/test2","module/test3"),"Path is incorrect.");
            Assert.AreEqual(CuT.Modules[0].Url,"https://github.com/Account/test3.git","URL is incorrect.");
            Assert.AreEqual(CuT.Modules[1].Name,"module/test4","Name is incorrect.");
            Assert.AreEqual(CuT.Modules[1].Path,Path.Combine("/test1/test2","module/test4"),"Path is incorrect.");
            Assert.AreEqual(CuT.Modules[1].Url,"https://github.com/Account/test4.git","URL is incorrect.");
            
            // Return if the paths are correct.
            Assert.IsFalse(CuT.IsInSubmodule("/test1/module/test3"));
            Assert.IsFalse(CuT.IsInSubmodule("/test1/test2/module/test2"));
            Assert.IsFalse(CuT.IsInSubmodule("/test1/test2/module/test2/test"));
            Assert.IsTrue(CuT.IsInSubmodule("/test1/test2/module/test3"));
            Assert.IsTrue(CuT.IsInSubmodule("/test1/test2/module/test3/test"));
            Assert.IsTrue(CuT.IsInSubmodule("/test1/test2/module/test4"));
            Assert.IsTrue(CuT.IsInSubmodule("/test1/test2/module/test4/test"));
            Assert.IsFalse(CuT.IsInSubmodule("/test1/test2/module/test5"));
            Assert.IsFalse(CuT.IsInSubmodule("/test1/test2/module/test5/test"));
        }
    }
}