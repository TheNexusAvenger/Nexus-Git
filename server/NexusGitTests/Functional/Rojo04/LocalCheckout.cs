/*
 * TheNexusAvenger
 *
 * Tests the LocalCheckout request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests local checkout as a standalone request.
     */
    public class TestLocalCheckout : BaseFunctionalTest
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
            this.Workspace.RunCommand("git","branch second_branch");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            // Assert the branches are correct.
            this.SendPOSTRequest("/localcheckout","second_branch");
            var response = this.SendGETRequest("/listlocalbranches");
            Assert.AreEqual(response, "[\"master\",\"* second_branch\"]");

            // Write files to the branch.
            this.Workspace.WriteFile("file1.txt","Test file 4");
            this.Workspace.WriteFile("file2.txt","Test file 5");
            this.Workspace.WriteFile("file4.txt","Test file 6");
            this.Workspace.RunCommand("git","add .");
            this.Workspace.RunCommand("git","commit -m \"Second commit\"");

            // Change branches back and assert the files are back.
            this.SendPOSTRequest("/localcheckout","master");
            response = this.SendGETRequest("/listlocalbranches");
            Assert.AreEqual(response,"[\"* master\",\"second_branch\"]");
            Assert.AreEqual(this.Workspace.ReadFile("file1.txt"),"Test file 1");
            Assert.AreEqual(this.Workspace.ReadFile("file2.txt"),"Test file 2");
            Assert.AreEqual(this.Workspace.ReadFile("file3.txt"),"Test file 3");
            Assert.IsFalse(this.Workspace.FileExists("file4.txt"));
            this.Workspace.RunCommand("git","add .");
            this.Workspace.RunCommand("git","commit -m \"Third commit\"");

            // Change the branch to the second and assert the files are correct.
            this.SendPOSTRequest("/localcheckout","second_branch");
            response = this.SendGETRequest("/listlocalbranches");
            Assert.AreEqual(response,"[\"master\",\"* second_branch\"]");
            Assert.AreEqual(this.Workspace.ReadFile("file1.txt"),"Test file 4");
            Assert.AreEqual(this.Workspace.ReadFile("file2.txt"),"Test file 5");
            Assert.AreEqual(this.Workspace.ReadFile("file3.txt"),"Test file 3");
            Assert.AreEqual(this.Workspace.ReadFile("file4.txt"),"Test file 6");
        }
    }
}