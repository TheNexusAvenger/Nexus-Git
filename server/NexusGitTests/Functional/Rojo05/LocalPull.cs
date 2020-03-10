/*
 * TheNexusAvenger
 *
 * Tests the LocalPull request with Rojo 0.5.
 */

using Newtonsoft.Json;
using NexusGit.RobloxInstance;
using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo05
{
    /*
     * Tests local pull as a standalone request.
     */
    public class TestLocalPull : BaseFunctionalTest
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
            this.Workspace.WriteFile("src/testscript1.server.lua","print(\"Hello world 1!\")");
            this.Workspace.WriteFile("src/testscript2.client.lua", "print(\"Hello world 2!\")");
            this.Workspace.WriteFile("test/testscript3.lua", "print(\"Hello world 3!\")");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            var response = JsonConvert.DeserializeObject<Partitions>(this.SendGETRequest("/localpull"));

            Assert.AreEqual(response.Type,"Partitions");
            Assert.AreEqual(response.Instances.Count,2);
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Properties["Name"].Value,"NexusGit");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children.Count,2);
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGitTest"].Children.Count,1);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["ClassName"].Value,"Script");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["Name"].Value,"testscript1");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["Source"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["Source"].Value,"print(\"Hello world 1!\")");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children.Count,0);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["ClassName"].Value,"LocalScript");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["Name"].Value,"testscript2");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["Source"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["Source"].Value,"print(\"Hello world 2!\")");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Children.Count,0);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGitTest"].Children[0].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGitTest"].Children[0].Properties["ClassName"].Value,"ModuleScript");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGitTest"].Children[0].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGitTest"].Children[0].Properties["Name"].Value,"testscript3");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGitTest"].Children[0].Properties["Source"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGitTest"].Children[0].Properties["Source"].Value,"print(\"Hello world 3!\")");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGitTest"].Children[0].Children.Count,0);
        }
    }
    
    /*
     * Tests local pull with directories as a standalone request.
     */
    public class TestLocalPullWithDirectories : BaseFunctionalTest
    {
        /*
         * Sets up the test.
         */
        public override void Setup()
        {
            // Write the files.
            this.Workspace.WriteFile("default.project.json","{\"name\": \"Repository\",\"servePort\": 20001,\"tree\": {\"$className\": \"DataModel\",\"ReplicatedStorage\": {\"$className\": \"ReplicatedStorage\",\"NexusGit\": {\"$path\": \"src\"},\"NexusGitTests\": {\"$path\": \"test\"}}}}");
            this.Workspace.CreateDirectory("src");
            this.Workspace.CreateDirectory("src/directory1");
            this.Workspace.CreateDirectory("src/directory2");
            this.Workspace.WriteFile("src/directory1/testscript1.server.lua","print(\"Hello world 1!\")");
            this.Workspace.WriteFile("src/directory1/testscript2.client.lua", "print(\"Hello world 2!\")");
            this.Workspace.WriteFile("src/directory2/testscript3.lua", "print(\"Hello world 3!\")");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            var response = JsonConvert.DeserializeObject<Partitions>(this.SendGETRequest("/localpull"));

            Assert.AreEqual(response.Type,"Partitions");
            Assert.AreEqual(response.Instances.Count,1);
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Properties["Name"].Value,"NexusGit");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children,2);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["ClassName"].Value,"Folder");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["Name"].Value,"directory1");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children,1);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Properties["ClassName"].Value,"Script");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Properties["Name"].Value,"testscript1");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Properties["Source"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Properties["Source"].Value,"print(\"Hello world 1!\")");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children,0);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["ClassName"].Value,"LocalScript");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["Name"].Value,"directory2");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["Source"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Properties["Source"].Value,"print(\"Hello world 2!\")");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Children,1);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Children[0].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Children[0].Properties["ClassName"].Value,"ModuleScript");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Children[0].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Children[0].Properties["Name"].Value,"testscript3");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Children[0].Properties["Source"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Children[0].Properties["Source"].Value,"print(\"Hello world 3!\")");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[1].Children[0].Children,0);
        }
    }
    
    /*
     * Tests local pull with a model file as a standalone request.
     */
    public class TestLocalPullWithModelFile : BaseFunctionalTest
    {
        /*
         * Sets up the test.
         */
        public override void Setup()
        {
            // Write the files.
            this.Workspace.WriteFile("default.project.json","{\"name\": \"Repository\",\"servePort\": 20001,\"tree\": {\"$className\": \"DataModel\",\"ReplicatedStorage\": {\"$className\": \"ReplicatedStorage\",\"NexusGit\": {\"$path\": \"src\"},\"NexusGitTests\": {\"$path\": \"test\"}}}}");
            this.Workspace.CreateDirectory("src");
            this.Workspace.CreateDirectory("src/directory");
            this.Workspace.WriteFile("src/directory/init.server.lua","print(\"Hello world!\")");
            this.Workspace.WriteFile("src/directory/hello.model.json","{\"Name\": \"hello\",\"ClassName\": \"Model\",\"Children\": [{\"Name\": \"Some Part\",\"ClassName\": \"Part\"},{\"Name\": \"Some StringValue\",\"ClassName\": \"StringValue\",\"Properties\": {\"Value\": {\"Type\": \"String\",\"Value\": \"Hello, world!\"}}}]}");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            var response = JsonConvert.DeserializeObject<Partitions>(this.SendGETRequest("/localpull"));

            Assert.AreEqual(response.Type,"Partitions");
            Assert.AreEqual(response.Instances,1);
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Properties["Name"].Value,"NexusGit");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children,1);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["ClassName"].Value,"Script");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["Name"].Value,"directory");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["Source"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Properties["Source"].Value,"print(\"Hello world!\")");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children,1);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Properties["ClassName"].Value,"Model");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Properties["Name"].Value,"hello");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children,2);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[0].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[0].Properties["ClassName"].Value,"Part");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[0].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[0].Properties["Name"].Value,"Some Part");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[0].Children,0);

            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[1].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[1].Properties["ClassName"].Value,"StringValue");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[1].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[1].Properties["Name"].Value,"Some StringValue");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[1].Properties["Value"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[1].Properties["Value"].Value,"Hello, world!");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children[0].Children[0].Children[1].Children,0);
        }
    }
    
    /*
     * Tests local pull with a text file as a standalone request.
     */
    public class TestLocalPullWithTextFile : BaseFunctionalTest
    {
        /*
         * Sets up the test.
         */
        public override void Setup()
        {
            // Write the files.
            this.Workspace.WriteFile("default.project.json","{\"name\": \"Repository\",\"servePort\": 20001,\"tree\": {\"$className\": \"DataModel\",\"ReplicatedStorage\": {\"$className\": \"ReplicatedStorage\",\"NexusGit\": {\"$path\": \"src\"},\"NexusGitTests\": {\"$path\": \"test\"}}}}");
            this.Workspace.CreateDirectory("src");
            this.Workspace.WriteFile("src/TestValue.txt","Test value");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            var response = JsonConvert.DeserializeObject<Partitions>(this.SendGETRequest("/localpull"));

            Assert.AreEqual(response.Type,"Partitions");
            Assert.AreEqual(response.Instances,1);
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Properties["Name"].Value,"NexusGit");
            Assert.AreEqual(response.Instances["ReplicatedStorage.NexusGit"].Children,1);
            
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Properties["ClassName"].Value,"Folder");
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Properties["Name"].Value,"NexusGit");
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Children.Count,1);

            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Children[0].Properties["ClassName"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Children[0].Properties["ClassName"].Value, "StringValue");
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Children[0].Properties["Name"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Children[0].Properties["Name"].Value,"TestValue");
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Children[0].Properties["Value"].Type,"String");
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Children[0].Properties["Value"].Value,"Test value");
            Assert.AreEqual(response.Instances["ReplicatedStorage"].Children[0].Children[0].Children.Count,0);
        }
    }
}