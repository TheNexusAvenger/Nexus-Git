/*
 * TheNexusAvenger
 *
 * Tests Rojo 0.5 support.
 */

using System.Collections.Generic;
using Newtonsoft.Json;
using NexusGit.NexusGit.Projects.SupportedProjects.Rojo;
using NexusGit.RobloxInstance;
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
                                        {"$path", "module/NexusIns"},
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
                            new RojoFile("NexusIns")
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
        
        /*
         * Tests the PopulateRojoFiles method of Rojo05TreeObject.
         */
        [Test]
        public void TestPopulateRojoFiles()
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
                                        {"$path", "module/NexusIns"},
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
            
            // Create the Rojo instances.
            var instances = new RojoInstance()
            {
                Name = "game",
                ClassName = "DataModel",
                Children = new List<RojoInstance>()
                {
                    new RojoInstance()
                    {
                        Name = "Lighting",
                        ClassName = "Lighting",
                    },
                    new RojoInstance()
                    {
                        Name = "ReplicatedStorage",
                        ClassName = "ReplicatedStorage",
                        Children = new List<RojoInstance>()
                        {
                            new RojoInstance()
                            {
                                Name = "NexusButton",
                                ClassName = "ModuleScript",
                                Properties = new Dictionary<string,Property<object>>()
                                {
                                    {"Source",new Property<object>("string","print(\"Test source 3\")")},
                                },
                                Children = new List<RojoInstance>()
                                {
                                    new RojoInstance()
                                    {
                                        Name = "CustomFrame",
                                        ClassName = "ModuleScript",
                                        Properties = new Dictionary<string,Property<object>>()
                                        {
                                            {"Source",new Property<object>("string","print(\"Test source 4\")")},
                                        },
                                    },
                                    new RojoInstance()
                                    {
                                        Name = "NexusInstance",
                                        ClassName = "Folder",
                                        Children = new List<RojoInstance>()
                                        {
                                            new RojoInstance()
                                            {
                                                Name = "NexusObject",
                                                ClassName = "ModuleScript",
                                                Properties = new Dictionary<string,Property<object>>()
                                                {
                                                    {"Source",new Property<object>("string","print(\"Test source 1\")")}
                                                },
                                            },
                                            new RojoInstance()
                                            {
                                                Name = "NexusInstance",
                                                ClassName = "LocalScript",
                                                Properties = new Dictionary<string,Property<object>>()
                                                {
                                                    {"Source",new Property<object>("string","print(\"Test source 2\")")},
                                                    {"Disabled",new Property<object>("bool",true)}
                                                },
                                            },
                                        }
                                    },
                                }
                            },
                            new RojoInstance()
                            {
                                Name = "NexusButtonTests",
                                ClassName = "Script",
                                Properties = new Dictionary<string,Property<object>>()
                                {
                                    {"Source",new Property<object>("string","print(\"Test source 5\")")},
                                    {"Disabled",new Property<object>("bool",true)}
                                },
                                Children = new List<RojoInstance>()
                                {
                                    new RojoInstance()
                                    {
                                        Name = "CustomFrameTests",
                                        ClassName = "ModuleScript",
                                        Properties = new Dictionary<string,Property<object>>()
                                        {
                                            {"Source",new Property<object>("string","print(\"Test source 6\")")},
                                        },
                                    },
                                }
                            },
                        }
                    },
                }
            };
            
            // Populate a file.
            var treeObject = Rojo05TreeObject.CreateFromStructure(tree,"game");
            var project = new Rojo05();
            var CuT = new RojoFile("Root");
            treeObject.Children[0].PopulateRojoFiles(CuT,instances.Children[0],project);
            treeObject.Children[1].PopulateRojoFiles(CuT,instances.Children[1],project);

            // Assert the children counts are correct.
            Assert.AreEqual(CuT.SubFiles.Count,3);
            Assert.AreEqual(CuT.GetFile("module").SubFiles.Count,1);
            Assert.AreEqual(CuT.GetFile("module/NexusIns").SubFiles.Count,3);
            Assert.AreEqual(CuT.GetFile("module/NexusIns/NexusObject.lua").SubFiles.Count,0);
            Assert.AreEqual(CuT.GetFile("module/NexusIns/NexusInstance.client.lua").SubFiles.Count,0);
            Assert.AreEqual(CuT.GetFile("module/NexusIns/NexusInstance.meta.json").SubFiles.Count,0);
            Assert.AreEqual(CuT.GetFile("src").SubFiles.Count,2);
            Assert.AreEqual(CuT.GetFile("src/init.lua").SubFiles.Count, 0);
            Assert.AreEqual(CuT.GetFile("src/CustomFrame.lua").SubFiles.Count,0);
            Assert.AreEqual(CuT.GetFile("test").SubFiles.Count,3);
            Assert.AreEqual(CuT.GetFile("test/init.server.lua").SubFiles.Count, 0);
            Assert.AreEqual(CuT.GetFile("test/init.meta.json").SubFiles.Count,0);
            Assert.AreEqual(CuT.GetFile("test/CustomFrameTests.lua").SubFiles.Count,0);

            // Assert the contents are correct.
            Assert.AreEqual(CuT.GetFile("module").Contents,null);
            Assert.AreEqual(CuT.GetFile("module/NexusIns").Contents,null);
            Assert.AreEqual(CuT.GetFile("module/NexusIns/NexusObject.lua").Contents,"print(\"Test source 1\")");
            Assert.AreEqual(CuT.GetFile("module/NexusIns/NexusInstance.client.lua").Contents,"print(\"Test source 2\")");
            Assert.AreEqual(CuT.GetFile("module/NexusIns/NexusInstance.meta.json").Contents,"{\r\n  \"properties\": {\r\n    \"Disabled\": true\r\n  }\r\n}");
            Assert.AreEqual(CuT.GetFile("src").Contents,null);
            Assert.AreEqual(CuT.GetFile("src/init.lua").Contents,"print(\"Test source 3\")");
            Assert.AreEqual(CuT.GetFile("src/CustomFrame.lua").Contents,"print(\"Test source 4\")");
            Assert.AreEqual(CuT.GetFile("test").Contents,null);
            Assert.AreEqual(CuT.GetFile("test/init.server.lua").Contents,"print(\"Test source 5\")");
            Assert.AreEqual(CuT.GetFile("test/CustomFrameTests.lua").Contents,"print(\"Test source 6\")");
            Assert.AreEqual(CuT.GetFile("test/init.meta.json").Contents,"{\r\n  \"properties\": {\r\n    \"Disabled\": true\r\n  }\r\n}");
        }
        
        /*
         * Tests the PopulateRojoFiles method of Rojo05TreeObject with custom containers.
         */
        [Test]
        public void TestPopulateRojoFilesCustomContainers()
        {
            // Create a project structure tree.
            var tree = new Dictionary<string, object>()
            {
                {
                    "ReplicatedStorage", new Dictionary<string, object>()
                    {
                        {"$className", "ReplicatedStorage"},
                        {
                            "NexusButton", new Dictionary<string, object>()
                            {
                                {"$path", "src"},
                            }
                        },
                    }
                },
            };
            tree = JsonConvert.DeserializeObject<Dictionary<string,object>>(JsonConvert.SerializeObject(tree));
            
            // Create the Rojo instances.
            var instances = new RojoInstance()
            {
                Name = "game",
                ClassName = "DataModel",
                Children = new List<RojoInstance>()
                {
                    new RojoInstance()
                    {
                        Name = "ReplicatedStorage",
                        ClassName = "ReplicatedStorage",
                        Children = new List<RojoInstance>()
                        {
                            new RojoInstance()
                            {
                                Name = "NexusButton",
                                ClassName = "Tool",
                                Properties = new Dictionary<string,Property<object>>()
                                {
                                    {"ToolTip",new Property<object>("string","Test tooltip 1")},
                                },
                                Children = new List<RojoInstance>()
                                {
                                    new RojoInstance()
                                    {
                                        Name = "ToolRunner",
                                        ClassName = "Script",
                                        Properties = new Dictionary<string,Property<object>>()
                                        {
                                            {"Source",new Property<object>("string","print(\"Test source 1\")")},
                                        },
                                    },
                                    new RojoInstance()
                                    {
                                        Name = "SubTool",
                                        ClassName = "Tool",
                                        Properties = new Dictionary<string,Property<object>>()
                                        {
                                            {"ToolTip",new Property<object>("string","Test tooltip 1")},
                                        },
                                        Children = new List<RojoInstance>()
                                        {
                                            new RojoInstance()
                                            {
                                                Name = "Handle",
                                                ClassName = "Part",
                                            },
                                        }
                                    },
                                }
                            },
                        }
                    },
                }
            };
            
            // Populate a file.
            var treeObject = Rojo05TreeObject.CreateFromStructure(tree,"game");
            var project = new Rojo05();
            var CuT = new RojoFile("Root");
            treeObject.Children[0].PopulateRojoFiles(CuT,instances.Children[0],project);

            // Assert the children counts are correct.
            Assert.AreEqual(CuT.SubFiles.Count,1);
            Assert.AreEqual(CuT.GetFile("src").SubFiles.Count,3);
            Assert.AreEqual(CuT.GetFile("src/ToolRunner.server.lua").SubFiles.Count, 0);
            Assert.AreEqual(CuT.GetFile("src/init.meta.json").SubFiles.Count,0);
            Assert.AreEqual(CuT.GetFile("src/SubTool.model.json").SubFiles.Count,0);
            
            // Assert the contents are correct.
            Assert.AreEqual(CuT.GetFile("src").Contents,null);
            Assert.AreEqual(CuT.GetFile("src/ToolRunner.server.lua").Contents,"print(\"Test source 1\")");
            Assert.AreEqual(CuT.GetFile("src/init.meta.json").Contents,"{\r\n  \"className\": \"Tool\",\r\n  \"properties\": {\r\n    \"ToolTip\": \"Test tooltip 1\"\r\n  }\r\n}");
            Assert.AreEqual(CuT.GetFile("src/SubTool.model.json").Contents,"{\r\n  \"Name\": \"SubTool\",\r\n  \"ClassName\": \"Tool\",\r\n  \"Children\": [\r\n    {\r\n      \"Name\": \"Handle\",\r\n      \"ClassName\": \"Part\",\r\n      \"Children\": [],\r\n      \"Properties\": {}\r\n    }\r\n  ],\r\n  \"Properties\": {\r\n    \"ToolTip\": {\r\n      \"Type\": \"string\",\r\n      \"Value\": \"Test tooltip 1\"\r\n    }\r\n  }\r\n}");
        }
    }
}