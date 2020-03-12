/*
 * TheNexusAvenger
 * 
 * Tests the GetVersion request with Rojo 0.5.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo05
{
    /*
     * Tests the version as a standalone request.
     */
    public class TestGetVersionStandalone : BaseFunctionalTest
    {
        /*
         * Sets up the test.
         */
        public override void Setup()
        {
            this.Workspace.WriteFile("default.project.json","{\"name\": \"Repository\",\"servePort\": 20001,\"tree\": {\"$className\": \"DataModel\",\"ReplicatedStorage\": {\"$className\": \"ReplicatedStorage\",\"NexusGit\": {\"$path\": \"src\"},\"NexusGitTests\": {\"$path\": \"test\"}}}}");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            // Assert the server information is correct.
            Assert.AreEqual(this.server.GetPort(),20001);
            Assert.AreEqual(this.server.GetProject().GetName(),"Rojo 0.5.X");
            
            // Assert the response for the version is correct.
            var response = this.SendGETRequest("/getversion");
            Assert.AreEqual(response,"{\r\n  \"version\": \"0.2.0 Alpha\",\r\n  \"project\": \"Rojo 0.5.X\"\r\n}");
        }
    }
}