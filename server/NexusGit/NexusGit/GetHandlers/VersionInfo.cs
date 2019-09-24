/*
 * TheNexusAvenger
 *
 * Returns the version of the server.
 */

using Newtonsoft.Json;
using NexusGit.Http.Request;
using NexusGit.NexusGit.Projects;
using NexusGit.RobloxInstance;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit.GetHandlers
{
    /*
     * Response the stores version information.
     */
    public class VersionInfoResponse {
        public string version;
        public string project;
    }
    
    
    /*
     * Class representing a version HTTP request handler.
     */
    public class VersionInfo : SplitClientRequestHandler
    {
        private IProject Project;
        
        /*
         * Creates a local pull request object.
         */
        public VersionInfo(IProject project)
        {
            this.Project = project;
        }
        
        /*
         * Returns a response for a given request.
         */
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Create the response.
            VersionInfoResponse info = new VersionInfoResponse();
            info.version = "0.1.0 Alpha";
            info.project = this.Project.GetName();
            
            // Return a response.
            return Response.CreateSuccessResponse(JsonConvert.SerializeObject(info,Formatting.Indented));
        }
    }
}