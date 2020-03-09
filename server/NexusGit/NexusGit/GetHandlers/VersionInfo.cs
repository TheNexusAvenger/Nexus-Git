/*
 * TheNexusAvenger
 *
 * Returns the version of the server.
 */

using Newtonsoft.Json;
using Nexus.Http.Server.Http.Request;
using Nexus.Http.Server.Http.Response;
using Nexus.Http.Server.SplitHttp.Request;
using NexusGit.NexusGit.Projects;

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
        public override HttpResponse GetCompleteResponseData(HttpRequest request)
        {
            // Create the response.
            var info = new VersionInfoResponse();
            info.version = "0.1.0 Alpha";
            info.project = this.Project.GetName();
            
            // Return a response.
            return HttpResponse.CreateSuccessResponse(JsonConvert.SerializeObject(info,Formatting.Indented));
        }
    }
}