/*
 * TheNexusAvenger
 * 
 * Tests the GetProjectPartitions request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests getting the partitions as a standalone request.
     */
    public class TestGetProjectPartitions : BaseFunctionalTest
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
            // Assert the initial status is correct.
            var response = this.SendGETRequest("/getprojectpartitions");
            Assert.AreEqual(response,"{\"ReplicatedStorage.NexusGit\":true,\"ReplicatedStorage.NexusGitTest\":true}");
        }
    }
}