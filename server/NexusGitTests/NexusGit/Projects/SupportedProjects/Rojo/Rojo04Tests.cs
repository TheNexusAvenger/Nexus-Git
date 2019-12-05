/*
 * TheNexusAvenger
 *
 * Tests Rojo 0.4 support.
 */

using Newtonsoft.Json;
using NexusGit.NexusGit.Projects.SupportedProjects.Rojo;
using NexusGit.RobloxInstance;
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

            // Assert that the returned instances are correct.
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
        
        /*
         * Tests the GetFile method with different file types.
         */
        [Test]
        public void TestGetFileCombined() {
            // Create the instances.
            var rootInstance = new RojoInstance();
            rootInstance.Name = "TestObject";
            rootInstance.ClassName = "Script";
            rootInstance.Properties.Add("Source",new Property<object>("String","print(\"Hello world 1!\")"));

            var subInstance1 = new RojoInstance();
            subInstance1.Name = "client";
            subInstance1.ClassName = "ModuleScript";
            subInstance1.Properties.Add("Source",new Property<object>("String","print(\"Hello world 2!\")"));
            rootInstance.Children.Add(subInstance1);

            var subInstance2 = new RojoInstance();
            subInstance2.Name = "module.client.lua";
            subInstance2.ClassName = "Folder";
            rootInstance.Children.Add(subInstance2);

            var subInstance3 = new RojoInstance();
            subInstance3.Name = "Script";
            subInstance3.ClassName = "LocalScript";
            subInstance3.Properties.Add("Source",new Property<object>("String","print(\"Hello world 3!\")"));
            subInstance2.Children.Add(subInstance3);

            var subInstance4 = new RojoInstance();
            subInstance4.Name = "TestModel";
            subInstance4.ClassName = "Model";
            subInstance2.Children.Add(subInstance4);

            var subInstance5 = new RojoInstance();
            subInstance5.Name = "TestPart";
            subInstance5.ClassName = "Part";
            subInstance5.Properties.Add("Anchored",new Property<object>("bool",false));
            subInstance4.Children.Add(subInstance5);
            
            // Create the component under testing.
            var CuT = new Rojo04();

            // Assert that the returned files are correct.
            var rootFile = CuT.GetFile(rootInstance);
            Assert.AreEqual(rootFile.Name,"TestObject");
            Assert.AreEqual(rootFile.Contents,null);
            Assert.AreEqual(rootFile.SubFiles.Count,3);
            Assert.AreEqual(rootFile.SubFiles[0].Name,"init.server.lua");
            Assert.AreEqual(rootFile.SubFiles[0].Contents,"print(\"Hello world 1!\")");
            Assert.AreEqual(rootFile.SubFiles[0].SubFiles.Count,0);
            Assert.AreEqual(rootFile.SubFiles[1].Name,"client.lua");
            Assert.AreEqual(rootFile.SubFiles[1].Contents,"print(\"Hello world 2!\")");
            Assert.AreEqual(rootFile.SubFiles[1].SubFiles.Count,0);
            Assert.AreEqual(rootFile.SubFiles[2].Name,"module.client.lua");
            Assert.AreEqual(rootFile.SubFiles[2].Contents,null);
            Assert.AreEqual(rootFile.SubFiles[2].SubFiles.Count,2);
            Assert.AreEqual(rootFile.SubFiles[2].SubFiles[0].Name,"Script.client.lua");
            Assert.AreEqual(rootFile.SubFiles[2].SubFiles[0].Contents,"print(\"Hello world 3!\")");
            Assert.AreEqual(rootFile.SubFiles[2].SubFiles[0].SubFiles.Count,0);
            Assert.AreEqual(rootFile.SubFiles[2].SubFiles[1].Name,"TestModel.model.json");
            Assert.AreEqual(rootFile.SubFiles[2].SubFiles[1].Contents,JsonConvert.SerializeObject(JsonConvert.DeserializeObject<RojoInstance>(TEST_MODEL_JSON),Formatting.Indented));
            Assert.AreEqual(rootFile.SubFiles[2].SubFiles[1].SubFiles.Count,0);
        }
    }
}