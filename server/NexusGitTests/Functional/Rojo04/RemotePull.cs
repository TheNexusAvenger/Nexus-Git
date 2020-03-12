/*
 * TheNexusAvenger
 *
 * Tests the RemotePull request with Rojo 0.4.
 */

using NUnit.Framework;

namespace NexusGitTests.Functional.Rojo04
{
    /*
     * Tests remote pull as a standalone request.
     */
    public class TestRemotePull : BaseFunctionalTest
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
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            this.SendGETRequest("/remotepull?remote=origin&branch=dummy-branch-2");
            Assert.AreEqual(this.Workspace.ReadFile("DummyFile1"),"Contents 1\r\n");
            Assert.AreEqual(this.Workspace.ReadFile("DummyFile2"),"Contents 2\r\n");
            Assert.AreEqual(this.Workspace.ReadFile("DummyFile6"),"Contents 6\r\n");
        }
    }
    
    /*
     * Tests remote pull with no remote defined as a standalone request.
     */
    public class TestRemotePullNoRemoteDefined : BaseFunctionalTest
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
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            this.SendGETRequest("/remotepull?branch=dummy-branch-2");
            Assert.AreEqual(this.Workspace.ReadFile("DummyFile1"),"Contents 1\r\n");
            Assert.AreEqual(this.Workspace.ReadFile("DummyFile2"),"Contents 2\r\n");
            Assert.AreEqual(this.Workspace.ReadFile("DummyFile6"),"Contents 6\r\n");
        }
    }
    
    /*
     * Tests remote pull with not branch defined as a standalone request.
     */
    public class TestRemotePullNoBranchDefined : BaseFunctionalTest
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
        }
        
        /*
         * Runs the test.
         */
        public override void Run()
        {
            this.SendGETRequest("/remotepull?remote=origin");
            Assert.AreEqual(this.Workspace.ReadFile("DummyFile1"),"Contents 1\r\n");
            Assert.AreEqual(this.Workspace.ReadFile("DummyFile2"),"Contents 2\r\n");
            Assert.AreEqual(this.Workspace.ReadFile("DummyFile3"),"Contents 3\r\n");
        }
    }
}