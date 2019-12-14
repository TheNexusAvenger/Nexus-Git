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
            Console.WriteLine(JsonConvert.SerializeObject(tree));
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
        [Ignore("Test will be implemented later. The code for it has dependencies.")]
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
            
            // Create the component under testing.
            var treeObject = Rojo05TreeObject.CreateFromStructure(tree,"game");
            // var CuT = treeObject.GetRojoInstance();
            
            
        }
    }
}