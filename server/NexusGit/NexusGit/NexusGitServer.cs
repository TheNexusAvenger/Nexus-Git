/*
 * TheNexusAvenger
 *
 * Runs the Nexus Git server.
 */

using Nexus.Http.Server.Http.Server;
using Nexus.Http.Server.SplitHttp.Request;
using NexusGit.NexusGit.GetHandlers;
using NexusGit.NexusGit.GetHandlers.Modules;
using NexusGit.NexusGit.PostHandlers;
using NexusGit.NexusGit.Projects;

namespace NexusGit.NexusGit
{
    /*
     * Class representing a server.
     */
    public class NexusGitServer
    {
        private HttpServer Server;
        private IProject Project;
        private int Port;
        
        /*
         * Creates the server.
         */
        private NexusGitServer(int port,IProject project)
        {
            // Create the request handlers.
            var requestHandler = new SplitRequestHandler();
            requestHandler.RegisterHandler("GET","GetGitStatus",new GitStatus());
            requestHandler.RegisterHandler("GET","RemotePull",new RemotePull());
            requestHandler.RegisterHandler("GET","GetProjectPartitions",new ProjectPartitions(project));
            requestHandler.RegisterHandler("GET","LocalPull",new LocalPull(project));
            requestHandler.RegisterHandler("GET","GetVersion",new VersionInfo(project));
            requestHandler.RegisterHandler("GET","ListLocalBranches",new LocalBranches());
            requestHandler.RegisterHandler("GET","ListRemoteBranches",new RemoteBranches());
            requestHandler.RegisterHandler("GET","ListCommits",new ListCommits());
            requestHandler.RegisterHandler("GET","ListRemotes",new ListRemotes());
            requestHandler.RegisterHandler("GET","Modules/List",new ListModules());
            requestHandler.RegisterHandler("POST","GitAdd",new GitAdd());
            requestHandler.RegisterHandler("POST","GitCommit",new GitCommit());
            requestHandler.RegisterHandler("POST","RemotePush",new RemotePush());
            requestHandler.RegisterHandler("POST","LocalPush",new LocalPush(project));
            requestHandler.RegisterHandler("POST","RemoteCheckout",new RemoteCheckout());
            requestHandler.RegisterHandler("POST","LocalCheckout",new LocalCheckout());
            
            // Create the server.
            this.Port = port;
            this.Server = new HttpServer(port,requestHandler);
            this.Project = project;
        }
        
        /*
         * Creates the server after detecting the current project.
         */
        public static NexusGitServer GetServer()
        {
            // Create a project manager.
            var projectManager = new ProjectManager();
            
            // Get the project to use.
            var project = projectManager.GetProject();
            
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
         * Returns the project used.
         */
        public IProject GetProject()
        {
            return this.Project;
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