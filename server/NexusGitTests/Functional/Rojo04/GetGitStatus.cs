/*
 * TheNexusAvenger
 * 
 * Tests the GetGitStatus request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests the status as a standalone request.
     */
    public class TestGetGitStatus : BaseFunctionalTest
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
            // Assert the initial status is correct.
            var response = this.SendGETRequest("/getgitstatus");
            Assert.AreEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file1.txt\",\"New file: file2.txt\",\"Untracked files:\",\"file3.txt\",\"rojo.json\"]");
            
            // Modify a file and assert it was modified.
            this.Workspace.RunCommand("git","commit file1.txt -m Commit");
            this.Workspace.WriteFile("file1.txt","Test file 4");
            response = this.SendGETRequest("/getgitstatus");
            Assert.AreEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file2.txt\",\"Modified: file1.txt\",\"Untracked files:\",\"file3.txt\",\"rojo.json\"]");
        }
    }
    
    /*
     * Tests the status as a standalone request.
     */
    public class TestGetGitStatusRenamedFile : BaseFunctionalTest
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
            // Assert the initial status is correct.
            var response = this.SendGETRequest("/getgitstatus");
            Assert.AreEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file1.txt\",\"New file: file2.txt\",\"Untracked files:\",\"file3.txt\",\"rojo.json\"]");
            
            // Rename a file and assert it was modified.
            this.Workspace.RunCommand("git","commit file1.txt -m Commit");
            this.Workspace.RenameFile("file1.txt","file4.txt");
            this.Workspace.RunCommand("git","add .");
            response = this.SendGETRequest("/getgitstatus");
            Assert.AreEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file2.txt\",\"New file: file3.txt\",\"Renamed: file1.txt -> file4.txt\",\"New file: rojo.json\"]");
        }
    }
}