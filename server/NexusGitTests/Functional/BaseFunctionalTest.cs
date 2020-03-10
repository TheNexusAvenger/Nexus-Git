/*
 * TheNexusAvenger
 *
 * Base functional test for Nexus Git.
 */

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NexusGit.NexusGit;
using NUnit.Framework;

namespace NexusGitTests.Functional
{
    public abstract class BaseFunctionalTest
    {
        public FunctionalTestWorkspace Workspace;
        private NexusGitServer server;
        private Task nexusGitTask;
        private int port = 20001;
        private string initialDirectory;
     
        /*
         * Waits for the server to initialize.
         */
        public void WaitForInitialization()
        {
            this.SendGETRequest("/version");
        }
     
        /*
         * Sends a GET request and returns the content.
         */
        public string SendGETRequest(string url)
        {
            var client = new HttpClient();
            return client.GetAsync(new Uri("http://localhost:" + this.port + url)).Result.Content.ReadAsStringAsync().Result;
        }
        
        /*
         * Sends a POST request and returns the content.
         */
        public string SendPOSTRequest(string url,string body)
        {
            var client = new HttpClient();
            return client.PostAsync(new Uri("http://localhost:" + this.port + url),new StringContent(body)).Result.Content.ReadAsStringAsync().Result;
        }
        
        /*
         * Performs the test.
         */
        [Test]
        public void RunTest()
        {
            // Create the temporary directory and workspace.
            var temporaryDirectory = Path.Combine(Path.GetTempPath(),Path.GetRandomFileName().Replace(".",""));
            this.Workspace = new FunctionalTestWorkspace(temporaryDirectory);
            this.initialDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(temporaryDirectory);
            
            // Set up the test.
            this.Setup();
            
            // Start Nexus Git.
            this.server = NexusGitServer.GetServer();
            this.nexusGitTask = new Task(() => { this.server.Start(); });
            this.nexusGitTask.Start();
            this.WaitForInitialization();
        }
        
        /*
         * Tears down the test.
         */
        [TearDown]
        public void RunTeardown()
        {
            // Stop Nexus Git.
            this.server.Stop();
            this.nexusGitTask.Wait();
            
            // Run the teardown.
            this.Teardown();
        }
        
        /*
         * Sets up the test.
         */
        public virtual void Setup()
        {
            
        }

        /*
         * Runs the test.
         */
        public abstract void Run();

        /*
         * Tears down the text.
         */
        public virtual void Teardown()
        {
            
        }
    }
}