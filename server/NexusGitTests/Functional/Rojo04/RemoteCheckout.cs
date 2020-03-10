/*
 * TheNexusAvenger
 *
 * Tests the RemoteCheckout request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests remote checkout as a standalone request.
     */
    public class TestRemoteCheckout : BaseFunctionalTest
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
            this.Workspace.RunCommand("git","remote add origin https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git");
            this.Workspace.RunCommand("git","checkout -b master");
            this.Workspace.RunCommand("git","pull origin master");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            this.SendPOSTRequest("/remotecheckout","{\"localBranch\":\"test_branch\",\"remote\":\"origin\",\"branch\":\"dummy-branch-2\"}");
            Assert.IsTrue(this.Workspace.FileExists("DummyFile6"));
            Assert.IsFalse(this.Workspace.FileExists("DummyFile3"));
            var response = this.SendGETRequest("/listlocalbranches");
            Assert.AreEqual(response,"[\"master\",\"* test_branch\"]");
        }
    }
}