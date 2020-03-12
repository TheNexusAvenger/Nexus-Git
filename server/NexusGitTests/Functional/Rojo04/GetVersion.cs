/*
 * TheNexusAvenger
 * 
 * Tests the GetVersion request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests the version as a split request.
     */
    public class TestGetVersionSplit : BaseFunctionalTest
    {
        /*
         * Sets up the test.
         */
        public override void Setup()
        {
            this.Workspace.WriteFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            var response = this.SendGETRequest("/getversion?packet=0&maxPackets=1");
            Assert.AreEqual(response,"{\"status\":\"success\",\"id\":0,\"currentPacket\":0,\"maxPackets\":1,\"packet\":\"{\\r\\n  \\\"version\\\": \\\"0.2.0 Alpha\\\",\\r\\n  \\\"project\\\": \\\"Rojo 0.4.X\\\"\\r\\n}\"}");
        }
    }
    
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
            this.Workspace.WriteFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            // Assert the server information is correct.
            Assert.AreEqual(this.server.GetPort(),20001);
            Assert.AreEqual(this.server.GetProject().GetName(),"Rojo 0.4.X");
            
            // Assert the response for the version is correct.
            var response = this.SendGETRequest("/getversion");
            Assert.AreEqual(response,"{\r\n  \"version\": \"0.2.0 Alpha\",\r\n  \"project\": \"Rojo 0.4.X\"\r\n}");
        }
    }
}