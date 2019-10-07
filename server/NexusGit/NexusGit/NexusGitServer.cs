/*
 * TheNexusAvenger
 *
 * Runs the Nexus Git server.
 */

using NexusGit.Http;
using NexusGit.NexusGit.GetHandlers;
using NexusGit.NexusGit.PostHandlers;
using NexusGit.NexusGit.Projects;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit
{
    /*
     * Class representing a server.
     */
    public class NexusGitServer
    {
        private HttpServer Server;
        private int Port;
        
        /*
         * Creates the server.
         */
        private NexusGitServer(int port,IProject project)
        {
            // Create the request handlers.
            SplitRequestHandler requestHandler = new SplitRequestHandler();
            requestHandler.RegisterHandler("GET","GitStatus",new GitStatus());
            requestHandler.RegisterHandler("GET","RemotePull",new RemotePull());
            requestHandler.RegisterHandler("GET","ProjectPartitions",new ProjectPartitions(project));
            requestHandler.RegisterHandler("GET","LocalPull",new LocalPull(project));
            requestHandler.RegisterHandler("GET","Version",new VersionInfo(project));
            requestHandler.RegisterHandler("GET","LocalBranches",new LocalBranches());
            requestHandler.RegisterHandler("GET","RemoteBranches",new RemoteBranches());
            requestHandler.RegisterHandler("POST","GitAdd",new GitAdd());
            requestHandler.RegisterHandler("POST","GitCommit",new GitCommit());
            requestHandler.RegisterHandler("POST","RemotePush",new RemotePush());
            requestHandler.RegisterHandler("POST","LocalPush",new LocalPush(project));
            
            // Create the server.
            this.Port = port;
            this.Server = new HttpServer(port,requestHandler);
        }
        
        /*
         * Creates the server after detecting the current project.
         */
        public static NexusGitServer GetServer()
        {
            // Create a project manager.
            ProjectManager projectManager = new ProjectManager();
            
            // Get the project to use.
            IProject project = projectManager.GetProject();
            
            // Return null if the project does not exist.
            if (project == null)
            {
                return null;
            }
            
            // Return a server object.
            return new NexusGitServer(project.GetPort(),project);
        }
        
        /*
         * Returns the port number.
         */
        public int GetPort()
        {
            return this.Port;
        }
        
        /*
         * Starts the server.
         */
        public void Start()
        {
            this.Server.Start();
        }
        
        /*
         * Stops the server.
         */
        public void Stop()
        {
            this.Server.Stop();
        }
    }
}