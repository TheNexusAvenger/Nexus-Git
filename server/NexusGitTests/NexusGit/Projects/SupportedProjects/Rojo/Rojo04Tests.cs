/*
 * TheNexusAvenger
 *
 * Tests Rojo 0.4 support.
 */

using NexusGit.NexusGit.Projects.SupportedProjects.Rojo;
using NUnit.Framework;

namespace NexusGitTests.NexusGit.Projects.SupportedProjects.Rojo {
    [TestFixture]
    public class Rojo04Tests {
        public const string TEST_MODEL_JSON = "{\n" +
                                              "    \"Name\": \"TestModel\",\n" +
                                              "    \"ClassName\": \"Model\",\n" +
                                              "    \"Properties\": {},\n" +
                                              "    \"Children\": [\n" +
                                              "        {\n" +
                                              "            \"Name\": \"TestPart\",\n" +
                                              "            \"ClassName\": \"Part\",\n" +
                                              "            \"Children\": [],\n" +
                                              "            \"Properties\": {\n" +
                                              "                \"Anchored\": {\n" +
                                              "                    \"Type\": \"bool\",\n" +
                                              "                    \"Value\": false\n" +
                                              "                }\n" +
                                              "            }\n" +
                                              "        }\n" +
                                              "    ]\n" +
                                              "}";

                                                  
        /*
         * Tests the GetFromFile method with different file types.
         */
        [Test]
        public void TestGetFromFileCombined() {
            // Create several files.
            var rootFile = new RojoFile("TestObject");
            var subFile1 = new RojoFile("init.server.lua");
            subFile1.Contents = "print(\"Hello world 1!\")";
            rootFile.SubFiles.Add(subFile1);
            var subFile2 = new RojoFile("client.lua");
            subFile2.Contents = "print(\"Hello world 2!\")";
            rootFile.SubFiles.Add(subFile2);
            var subFile3 = new RojoFile("module.client.lua");
            rootFile.SubFiles.Add(subFile3);
            var subFile4 = new RojoFile("Script.client.LUA");
            subFile4.Contents = "print(\"Hello world 3!\")";
            subFile3.SubFiles.Add(subFile4);
            var subFile5 = new RojoFile("testmodel.model.json");
            subFile5.Contents = TEST_MODEL_JSON;
            subFile3.SubFiles.Add(subFile5);

            // Create the component under testing.
            var CuT = new Rojo04();

            // Assert that the returned files are correct.
            var instance = CuT.GetFromFile(rootFile);
            Assert.AreEqual(instance.Name, "TestObject");
            Assert.AreEqual(instance.ClassName, "Script");
            Assert.AreEqual(instance.Properties.Count, 1);
            Assert.AreEqual(instance.Properties["Source"].Type, "String");
            Assert.AreEqual(instance.Properties["Source"].Value, "print(\"Hello world 1!\")");
            Assert.AreEqual(instance.Children.Count, 2);
            Assert.AreEqual(instance.Children[0].Name, "client");
            Assert.AreEqual(instance.Children[0].ClassName, "ModuleScript");
            Assert.AreEqual(instance.Children[0].Properties.Count, 1);
            Assert.AreEqual(instance.Children[0].Properties["Source"].Type, "String");
            Assert.AreEqual(instance.Children[0].Properties["Source"].Value, "print(\"Hello world 2!\")");
            Assert.AreEqual(instance.Children[0].Children.Count, 0);
            Assert.AreEqual(instance.Children[1].Name, "module.client.lua");
            Assert.AreEqual(instance.Children[1].ClassName, "Folder");
            Assert.AreEqual(instance.Children[1].Properties.Count, 0);
            Assert.AreEqual(instance.Children[1].Children.Count, 2);
            Assert.AreEqual(instance.Children[1].Children[0].Name, "Script");
            Assert.AreEqual(instance.Children[1].Children[0].ClassName, "LocalScript");
            Assert.AreEqual(instance.Children[1].Children[0].Properties.Count, 1);
            Assert.AreEqual(instance.Children[1].Children[0].Properties["Source"].Type, "String");
            Assert.AreEqual(instance.Children[1].Children[0].Properties["Source"].Value, "print(\"Hello world 3!\")");
            Assert.AreEqual(instance.Children[1].Children[0].Children.Count, 0);
            Assert.AreEqual(instance.Children[1].Children[1].Name, "TestModel");
            Assert.AreEqual(instance.Children[1].Children[1].ClassName, "Model");
            Assert.AreEqual(instance.Children[1].Children[1].Properties.Count, 0);
            Assert.AreEqual(instance.Children[1].Children[1].Children.Count, 1);
            Assert.AreEqual(instance.Children[1].Children[1].Children[0].Name, "TestPart");
            Assert.AreEqual(instance.Children[1].Children[1].Children[0].ClassName, "Part");
            Assert.AreEqual(instance.Children[1].Children[1].Children[0].Properties.Count, 1);
            Assert.AreEqual(instance.Children[1].Children[1].Children[0].Properties["Anchored"].Type, "bool");
            Assert.AreEqual(instance.Children[1].Children[1].Children[0].Properties["Anchored"].Value, false);
            Assert.AreEqual(instance.Children[1].Children[1].Children[0].Children.Count, 0);
        }
    }
}