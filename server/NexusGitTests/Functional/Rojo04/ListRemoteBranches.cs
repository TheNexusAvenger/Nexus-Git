/*
 * TheNexusAvenger
 *
 * Tests the ListRemoteBranches request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests listing remote branches as a standalone request.
     */
    public class TestListRemoteBranches : BaseFunctionalTest
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
            var response = this.SendGETRequest("/listremotebranches");
            Assert.AreEqual(response,"[\"origin/dummy-branch-1\",\"origin/dummy-branch-2\",\"origin/master\"]");
        }
    }
    
    /*
     * Tests listing remote branches with tracking as a standalone request.
     */
    public class TestListRemoteBranchesWithTracking : BaseFunctionalTest
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
            this.Workspace.RunCommand("git", "branch -u origin/master");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            var response = this.SendGETRequest("/listremotebranches");
            Assert.AreEqual(response,"[\"origin/dummy-branch-1\",\"origin/dummy-branch-2\",\"* origin/master\"]");
        }
    }
}