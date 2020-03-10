/*
 * TheNexusAvenger
 * 
 * Tests the ListCommits request with Rojo 0.4.
 */

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests listing commits as a standalone request.
     */
    public class TestListCommits : BaseFunctionalTest
    {
        /*
         * Sets up the test.
         */
        public override void Setup()
        {
            // Write the files.
            this.Workspace.WriteFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}");
            this.Workspace.WriteFile("file1.txt","Test file 1");
            this.Workspace.WriteFile("file2.txt","Test file 2");
            this.Workspace.WriteFile("file3.txt","Test file 3");
            
            // Initialize the repository.
            this.Workspace.RunCommand("git","init");
            this.Workspace.RunCommand("git","remote add origin https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git");
            this.Workspace.RunCommand("git","fetch");
            this.Workspace.RunCommand("git","config user.email \"john@doe.com\"");
            this.Workspace.RunCommand("git","config user.name \"John Doe\"");
            this.Workspace.RunCommand("git","add src/testscript1.server.lua");
            this.Workspace.RunCommand("git","add src/testscript2.client.lua");
            this.Workspace.RunCommand("git","commit -m \"Test commit 1\"");
            this.Workspace.RunCommand("git","add test/testscript3.lua");
            this.Workspace.RunCommand("git","commit -m \"Test commit 2\nNew line\"");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            var response = JsonConvert.DeserializeObject<List<Dictionary<string,string>>>(this.SendGETRequest("/listcommits?remote=origin&branch=master"));
            Assert.AreEqual(response[0]["id"].Length,40);
            Assert.AreEqual(response[0]["message"],"Test commit 2\nNew line");
            Assert.AreEqual(response[1]["id"].Length,40);
            Assert.AreEqual(response[1]["message"],"Test commit 1");
        }
    }
}