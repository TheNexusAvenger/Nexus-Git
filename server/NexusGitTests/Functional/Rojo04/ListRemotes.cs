/*
 * TheNexusAvenger
 *
 * Tests the ListRemotes request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests listing remotes as a standalone request.
     */
    public class TestListRemote : BaseFunctionalTest
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
            this.Workspace.RunCommand("git","remote add origin1 https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git");
            this.Workspace.RunCommand("git","remote add origin2 https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            var response = this.SendGETRequest("/listremotes");
            Assert.AreEqual(response,"[\"origin1\",\"origin2\"]");
        }
    }
}