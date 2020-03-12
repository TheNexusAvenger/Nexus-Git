/*
 * TheNexusAvenger
 * 
 * Tests the GetProjectPartitions request with Rojo 0.5.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo05
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
            this.Workspace.WriteFile("default.project.json","{\"name\": \"Repository\",\"servePort\": 20001,\"tree\": {\"$className\": \"DataModel\",\"ReplicatedStorage\": {\"$className\": \"ReplicatedStorage\",\"NexusGit\": {\"$path\": \"src\"},\"NexusGitTests\": {\"$path\": \"test\"}},\"Lighting\": {\"$className\": \"Lighting\"}}}");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            var response = this.SendGETRequest("/getprojectpartitions");
            Assert.AreEqual(response,"{\"ReplicatedStorage\":true,\"Lighting\":false}");
        }
    }
}