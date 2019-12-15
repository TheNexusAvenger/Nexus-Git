/*
 * TheNexusAvenger
 *
 * Tests Rojo 0.5 support.
 */

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NexusGit.NexusGit.Projects.SupportedProjects.Rojo;
using NUnit.Framework;

namespace NexusGitTests.NexusGit.Projects.SupportedProjects.Rojo
{
    [TestFixture]
    public class Rojo05Tests
    {
        /*
         * Tests the CreateFromStructure method of Rojo05TreeObject.
         */
        [Test]
        public void TestCreateFromStructure()
        {
            // Create a project structure tree.
            var tree = new Dictionary<string, object>()
            {
                {"$className", "DataModel"},
                {
                    "Lighting", new Dictionary<string, object>()
                    {
                        {"$className", "Lighting"},
                        {"$properties", new Dictionary<string, object>()
                        {
                            {"TimeOfDay", "12:00:00"},
                        }}
                    }
                },
                {
                    "ReplicatedStorage", new Dictionary<string, object>()
                    {
                        {"$className", "ReplicatedStorage"},
                        {
                            "NexusButton", new Dictionary<string, object>()
                            {
                                {"$path", "src"},
                                {
                                    "NexusInstance", new Dictionary<string, object>()
                                    {
                                        {"$path", "module/NexusInstance/src"},
                                    }
                                },
                            }
                        },
                        {
                            "NexusButtonTests", new Dictionary<string, object>()
                            {
                                {"$path", "test"},
                            }
                        },
                    }
                },
            };
            tree = JsonConvert.DeserializeObject<Dictionary<string,object>>(JsonConvert.SerializeObject(tree));

            // Create the component under testing.
            var CuT = Rojo05TreeObject.CreateFromStructure(tree,"game");
            Assert.AreEqual(CuT.Name,"game");
            Assert.AreEqual(CuT.ClassName,"DataModel");
            Assert.AreEqual(CuT.Path,null);
            Assert.AreEqual(CuT.Properties.Count,0);
            Assert.AreEqual(CuT.Children.Count,2);
            Assert.AreEqual(CuT.Children[0].Name,"Lighting");
            Assert.AreEqual(CuT.Children[0].ClassName,"Lighting");
            Assert.AreEqual(CuT.Children[0].Path,null);
            Assert.AreEqual(CuT.Children[0].Properties.Count,1);
            Assert.AreEqual(CuT.Children[0].Children.Count,0);
            Assert.AreEqual(CuT.Children[1].Name,"ReplicatedStorage");
            Assert.AreEqual(CuT.Children[1].ClassName,"ReplicatedStorage");
            Assert.AreEqual(CuT.Children[1].Path,null);
            Assert.AreEqual(CuT.Children[1].Properties.Count,0);
            Assert.AreEqual(CuT.Children[1].Children.Count,2);
            Assert.AreEqual(CuT.Children[1].Children[0].Name,"NexusButton");
            Assert.AreEqual(CuT.Children[1].Children[0].ClassName,null);
            Assert.AreEqual(CuT.Children[1].Children[0].Path,"src");
            Assert.AreEqual(CuT.Children[1].Children[0].Properties.Count,0);
            Assert.AreEqual(CuT.Children[1].Children[0].Children.Count,1);
            Assert.AreEqual(CuT.Children[1].Children[0].Children[0].Name,"NexusInstance");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[0].ClassName,null);
            Assert.AreEqual(CuT.Children[1].Children[0].Children[0].Path,"module/NexusInstance/src");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[0].Properties.Count,0);
            Assert.AreEqual(CuT.Children[1].Children[0].Children[0].Children.Count,0);
            Assert.AreEqual(CuT.Children[1].Children[1].Name,"NexusButtonTests");
            Assert.AreEqual(CuT.Children[1].Children[1].ClassName,null);
            Assert.AreEqual(CuT.Children[1].Children[1].Path,"test");
            Assert.AreEqual(CuT.Children[1].Children[1].Properties.Count,0);
            Assert.AreEqual(CuT.Children[1].Children[1].Children.Count,0);
        }
        
        /*
         * Tests the GetRojoInstance method of Rojo05TreeObject.
         */
        [Test]
        public void TestGetRojoInstance()
        {
            // Create a project structure tree.
            var tree = new Dictionary<string, object>()
            {
                {"$className", "DataModel"},
                {
                    "Lighting", new Dictionary<string, object>()
                    {
                        {"$className", "Lighting"},
                        {"$properties", new Dictionary<string, object>()
                        {
                            {"TimeOfDay", "12:00:00"},
                        }}
                    }
                },
                {
                    "ReplicatedStorage", new Dictionary<string, object>()
                    {
                        {"$className", "ReplicatedStorage"},
                        {
                            "NexusButton", new Dictionary<string, object>()
                            {
                                {"$path", "src"},
                                {
                                    "NexusInstance", new Dictionary<string, object>()
                                    {
                                        {"$path", "module/NexusInstance"},
                                    }
                                },
                            }
                        },
                        {
                            "NexusButtonTests", new Dictionary<string, object>()
                            {
                                {"$path", "test"},
                            }
                        },
                    }
                },
            };
            tree = JsonConvert.DeserializeObject<Dictionary<string,object>>(JsonConvert.SerializeObject(tree));
            
            // Create several files.
            var rootDirectory = new RojoFile("TestProject")
            {
                SubFiles =  new List<RojoFile>()
                {
                    new RojoFile("module")
                    {
                        SubFiles =  new List<RojoFile>()
                        {
                            new RojoFile("NexusInstance")
                            {
                                SubFiles =  new List<RojoFile>()
                                {
                                    new RojoFile("NexusObject.lua")
                                    {
                                        Contents = "print(\"Test source 1\")"
                                    },
                                    new RojoFile("NexusInstance.client.lua")
                                    {
                                        Contents = "print(\"Test source 2\")"
                                    },
                                    new RojoFile("NexusInstance.meta.json")
                                    {
                                        Contents = "{\"properties\":{\"Disabled\":true}}"
                                    },
                                }
                            }
                        }
                    },
                    new RojoFile("src")
                    {
                        SubFiles =  new List<RojoFile>()
                        {
                            new RojoFile("init.lua")
                            {
                                Contents = "print(\"Test source 3\")"
                            },
                            new RojoFile("CustomFrame.lua")
                            {
                                Contents = "print(\"Test source 4\")"
                            },
                        }
                    },
                    new RojoFile("test")
                    {
                        SubFiles =  new List<RojoFile>()
                        {
                            new RojoFile("init.server.lua")
                            {
                                Contents = "print(\"Test source 5\")"
                            },
                            new RojoFile("init.meta.json")
                            {
                                Contents = "{\"properties\":{\"Disabled\":true}}"
                            },
                            new RojoFile("CustomFrameTests.lua")
                            {
                                Contents = "print(\"Test source 6\")"
                            },
                        }
                    },
                }
            };
            rootDirectory.CorrectParents();

            // Create the component under testing.
            var treeObject = Rojo05TreeObject.CreateFromStructure(tree,"game");
            var project = new Rojo05();
            var CuT = treeObject.GetRojoInstance(rootDirectory,project);
            Assert.AreEqual(CuT.ClassName,"DataModel");
            Assert.AreEqual(CuT.Children.Count,2);
            Assert.AreEqual(CuT.Children[0].Name,"Lighting");
            Assert.AreEqual(CuT.Children[0].ClassName,"Lighting");
            Assert.AreEqual(CuT.Children[0].Properties["TimeOfDay"].Value,"12:00:00");
            Assert.AreEqual(CuT.Children[0].Children.Count,0);
            Assert.AreEqual(CuT.Children[1].Name,"ReplicatedStorage");
            Assert.AreEqual(CuT.Children[1].ClassName,"ReplicatedStorage");
            Assert.AreEqual(CuT.Children[1].Children.Count,2);
            Assert.AreEqual(CuT.Children[1].Children[0].Name,"NexusButton");
            Assert.AreEqual(CuT.Children[1].Children[0].ClassName,"ModuleScript");
            Assert.AreEqual(CuT.Children[1].Children[0].Properties["Source"].Value,"print(\"Test source 3\")");
            Assert.AreEqual(CuT.Children[1].Children[0].Children.Count,2);
            Assert.AreEqual(CuT.Children[1].Children[0].Children[0].Name,"CustomFrame");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[0].ClassName,"ModuleScript");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[0].Properties["Source"].Value,"print(\"Test source 4\")");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[0].Children.Count,0);
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Name,"NexusInstance");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].ClassName,"Folder");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Children.Count,2);
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Children[0].Name,"NexusObject");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Children[0].ClassName,"ModuleScript");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Children[0].Properties["Source"].Value,"print(\"Test source 1\")");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Children[0].Children.Count,0);
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Children[1].Name,"NexusInstance");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Children[1].ClassName,"LocalScript");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Children[1].Properties["Source"].Value,"print(\"Test source 2\")");
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Children[1].Properties["Disabled"].Value,true);
            Assert.AreEqual(CuT.Children[1].Children[0].Children[1].Children[1].Children.Count,0);
            Assert.AreEqual(CuT.Children[1].Children[1].Name,"NexusButtonTests");
            Assert.AreEqual(CuT.Children[1].Children[1].ClassName,"Script");
            Assert.AreEqual(CuT.Children[1].Children[1].Properties["Source"].Value,"print(\"Test source 5\")");
            Assert.AreEqual(CuT.Children[1].Children[1].Properties["Disabled"].Value,true);
            Assert.AreEqual(CuT.Children[1].Children[1].Children.Count,1);
            Assert.AreEqual(CuT.Children[1].Children[1].Children[0].Name,"CustomFrameTests");
            Assert.AreEqual(CuT.Children[1].Children[1].Children[0].ClassName,"ModuleScript");
            Assert.AreEqual(CuT.Children[1].Children[1].Children[0].Properties["Source"].Value,"print(\"Test source 6\")");
            Assert.AreEqual(CuT.Children[1].Children[1].Children[0].Children.Count,0);
        }
    }
}