/*
 * TheNexusAvenger
 *
 * Tests the ListLocalBranches request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests listing local branches as a standalone request.
     */
    public class TestListLocalBranches : BaseFunctionalTest
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
            this.Workspace.RunCommand("git","config user.email \"john@doe.com\"");
            this.Workspace.RunCommand("git","config user.name \"John Doe\"");
            this.Workspace.RunCommand("git","add .");
            this.Workspace.RunCommand("git","commit -m \"First commit\"");
            this.Workspace.RunCommand("git","branch branch_1");
            this.Workspace.RunCommand("git","branch branch_2");
            this.Workspace.RunCommand("git","branch branch_3");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            var response = this.SendGETRequest("/listlocalbranches");
            Assert.AreEqual(response,"[\"branch_1\",\"branch_2\",\"branch_3\",\"* master\"]");
        }
    }
}