/*
 * TheNexusAvenger
 * 
 * Tests the GitAdd request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests adding as a standalone request.
     */
    public class TestGitAdd : BaseFunctionalTest
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
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            // Add the file and assert the response is correct.
            var response = this.SendPOSTRequest("/gitadd", "[\"file1.txt\",\"file3.txt\"]");
            Assert.AreEqual(response,"[\"Add complete.\"]");
            
            // Assert the initial status is correct.
            response = this.SendGETRequest("/getgitstatus");
            Assert.AreEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file1.txt\",\"New file: file3.txt\",\"Untracked files:\",\"file2.txt\",\"rojo.json\"]");
        }
    }
}