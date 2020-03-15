/*
 * TheNexusAvenger
 * 
 * Tests the Modules/List request with Rojo 0.4.
 */

using System.Collections.Generic;
using Newtonsoft.Json;
using NexusGit.Git.RepositoryActions.Modules;
using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04.Modules
{
    /*
     * Tests the modules list without any modules as a standalone request.
     */
    public class TestListModulesNoModules : BaseFunctionalTest
    {
        /*
         * Sets up the test.
         */
        public override void Setup()
        {
            // Write the files.
            this.Workspace.WriteFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}");
           
            
            // Initialize the repository.
            this.Workspace.RunCommand("git","init");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            // Assert the modules are correct.
            var response = this.SendGETRequest("/modules/list");
            Assert.AreEqual(response,"[\"[]\"]");
        }
    }
    
    /*
     * Tests the modules list with a module as a standalone request.
     */
    public class TestListModulesOneModules : BaseFunctionalTest
    {
        /*
         * Sets up the test.
         */
        public override void Setup()
        {
            // Write the files.
            this.Workspace.WriteFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}");
           
            
            // Initialize the repository.
            this.Workspace.RunCommand("git","init");
            this.Workspace.RunCommand("git","submodule add https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git modules/module-name");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            // Assert the modules are correct.
            var response = this.SendGETRequest("/modules/list");
            var responseString = JsonConvert.DeserializeObject<List<string>>(response)[0];
            var modules = JsonConvert.DeserializeObject<List<SubmoduleData>>(responseString);
            Assert.AreEqual(modules.Count,1);
            Assert.AreEqual(modules[0].Name,"modules/module-name");
            Assert.AreEqual(modules[0].Url,"https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git");
            Assert.AreEqual(modules[0].Initialized,true);
        }
    }
}