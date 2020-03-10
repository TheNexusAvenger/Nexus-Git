/*
 * TheNexusAvenger
 * 
 * Tests the GitCommit request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests committing as a standalone request.
     */
    public class TestGitCommit : BaseFunctionalTest
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
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            // Add the file and assert the response is correct.
            this.SendPOSTRequest("/gitadd","[\"file1.txt\",\"file3.txt\"]");
            var response = this.SendPOSTRequest("/gitcommit","{\"message\":\"Test commit\",\"files\":[\"file1.txt\",\"file3.txt\"]}");
            Assert.AreEqual(response,"[\"Commit successful.\"]");
            
            // Assert the initial status is correct.
            response = this.SendGETRequest("/getgitstatus");
            Assert.AreEqual(response,"[\"Current branch:\",\"master\",\"Untracked files:\",\"file2.txt\",\"rojo.json\"]");
        }
    }
    
    /*
     * Tests committing renamed as a standalone request.
     */
    public class TestGitCommitRenamedFiles : BaseFunctionalTest
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
            this.Workspace.RunCommand("git","add file1.txt");
            this.Workspace.RunCommand("git","add file2.txt");
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            // Modify a file and assert it was modified.
            this.Workspace.RunCommand("git","commit file1.txt -m Commit");
            this.Workspace.RenameFile("file1.txt","file4.txt");
            this.Workspace.RunCommand("git","add .");
;
            // Send a standalone request and get the response.
            var response = this.SendPOSTRequest("/gitcommit","{\"message\":\"Test commit\",\"files\":[\"file1.txt\",\"file4.txt\"]}");
            Assert.AreEqual(response,"[\"Commit successful.\"]");
            response = this.SendGETRequest("/getgitstatus");
            Assert.AreEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file2.txt\",\"New file: file3.txt\",\"New file: rojo.json\"]");
        }
    }
}