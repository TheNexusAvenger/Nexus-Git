/*
 * TheNexusAvenger
 *
 * Tests the LocalPush request with Rojo 0.5.
 */

using System.Collections.Generic;
using Newtonsoft.Json;
using NexusGit.RobloxInstance;
using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo05
{
    /*
     * Tests local push as a standalone request.
     */
    public class TestLocalPush : BaseFunctionalTest
    {
        /*
         * Sets up the test.
         */
        public override void Setup()
        {
            // Write the files.
            this.Workspace.WriteFile("default.project.json","{\"name\": \"Repository\",\"servePort\": 20001,\"tree\": {\"$className\": \"DataModel\",\"ReplicatedStorage\": {\"$className\": \"ReplicatedStorage\",\"NexusGit\": {\"$path\": \"src\"},\"NexusGitTests\": {\"$path\": \"test\"}}}}");
            this.Workspace.CreateDirectory("src");
            this.Workspace.CreateDirectory("test");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            // Create the body.
            var instances = new Partitions()
            {
                Type = "Partitions",
                Instances =
                {
                    {"ReplicatedStorage",new Instance(0)
                        {
                            Properties = new Dictionary<string, Property<object>>()
                            {
                                {"Name", new Property<object>("String","ReplicatedStorage")},
                                {"ClassName", new Property<object>("String","ReplicatedStorage")}
                            },
                            Children = new List<Instance>()
                            {
                                new Instance(1)
                                {
                                    Properties = new Dictionary<string, Property<object>>()
                                    {
                                        {"Name", new Property<object>("String","NexusGit")},
                                        {"ClassName", new Property<object>("String","Folder")}
                                    },
                                    Children = new List<Instance>()
                                    {
                                        new Instance(2)
                                        {
                                            Properties = new Dictionary<string, Property<object>>()
                                            {
                                                {"Name", new Property<object>("String","testscript1")},
                                                {"ClassName", new Property<object>("String","Script")},
                                                {"Source", new Property<object>("String","print(\"Hello world 1!\")")}
                                            },
                                        },
                                        new Instance(3)
                                        {
                                            Properties = new Dictionary<string, Property<object>>()
                                            {
                                                {"Name", new Property<object>("String","testscript2")},
                                                {"ClassName", new Property<object>("String","LocalScript")},
                                                {"Source", new Property<object>("String","print(\"Hello world 2!\")")}
                                            },
                                        }
                                    }
                                },
                                new Instance(4)
                                {
                                    Properties = new Dictionary<string, Property<object>>()
                                    {
                                        {"Name", new Property<object>("String","NexusGitTests")},
                                        {"ClassName", new Property<object>("String","Folder")}
                                    },
                                    Children = new List<Instance>()
                                    {
                                        new Instance(5)
                                        {
                                            Properties = new Dictionary<string, Property<object>>()
                                            {
                                                {"Name", new Property<object>("String","testscript3")},
                                                {"ClassName", new Property<object>("String","ModuleScript")},
                                                {"Source", new Property<object>("String","print(\"Hello world 3!\")")}
                                            },
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            
            // Send a request and assert the response is correct.
            var response = this.SendPOSTRequest("/localpush",JsonConvert.SerializeObject(instances));
            Assert.AreEqual(response,"Local push successful.","Response message is incorrect.");
            
            // Assert the files are correct.
            Assert.AreEqual(this.Workspace.ReadFile("src/testscript1.server.lua"),"print(\"Hello world 1!\")","Body is incorrect.");
            Assert.AreEqual(this.Workspace.ReadFile("src/testscript2.client.lua"),"print(\"Hello world 2!\")","Body is incorrect.");
            Assert.AreEqual(this.Workspace.ReadFile("test/testscript3.lua"),"print(\"Hello world 3!\")","Body is incorrect.");
        }
    }
    
    /*
     * Tests local push with a missing partition as a standalone request.
     */
    public class TestLocalPushMissingPartition : BaseFunctionalTest
    {
        /*
         * Sets up the test.
         */
        public override void Setup()
        {
            // Write the files.
            this.Workspace.WriteFile("default.project.json","{\"name\": \"Repository\",\"servePort\": 20001,\"tree\": {\"$className\": \"DataModel\",\"ReplicatedStorage\": {\"$className\": \"ReplicatedStorage\",\"NexusGit\": {\"$path\": \"src\"},\"NexusGitTests\": {\"$path\": \"test\"}}}}");
            this.Workspace.CreateDirectory("src");
            this.Workspace.CreateDirectory("test");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            // Create the body.
            var instances = new Partitions()
            {
                Type = "Partitions",
                Instances =
                {
                    {"ReplicatedStorage",new Instance(0)
                        {
                            Properties = new Dictionary<string, Property<object>>()
                            {
                                {"Name", new Property<object>("String","ReplicatedStorage")},
                                {"ClassName", new Property<object>("String","ReplicatedStorage")}
                            },
                            Children = new List<Instance>()
                            {
                                new Instance(1) {
                                    Properties = new Dictionary<string, Property<object>>()
                                    {
                                        {"Name", new Property<object>("String","NexusGitTests")},
                                        {"ClassName", new Property<object>("String","Folder")}
                                    },
                                    Children = new List<Instance>()
                                    {
                                        new Instance(2)
                                        {
                                            Properties = new Dictionary<string, Property<object>>()
                                            {
                                                {"Name", new Property<object>("String","testscript3")},
                                                {"ClassName", new Property<object>("String","ModuleScript")},
                                                {"Source", new Property<object>("String","print(\"Hello world 3!\")")}
                                            },
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            
            // Send a request and assert the response is correct.
            var response = this.SendPOSTRequest("/localpush",JsonConvert.SerializeObject(instances));
            Assert.AreEqual(response,"Local push successful.","Response message is incorrect.");
            
            // Assert the files are correct.
            Assert.IsFalse(this.Workspace.FileExists("src/testscript1.server.lua"));
            Assert.IsFalse(this.Workspace.FileExists("src/testscript2.client.lua"));
            Assert.AreEqual(this.Workspace.ReadFile("test/testscript3.lua"),"print(\"Hello world 3!\")","Body is incorrect.");
        }
    }
}