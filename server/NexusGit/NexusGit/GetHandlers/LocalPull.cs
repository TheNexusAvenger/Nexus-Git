/*
 * TheNexusAvenger
 *
 * Handles pulling the project structure from the file system to Roblox Studio.
 */

using Nexus.Http.Server.Http.Request;
using Nexus.Http.Server.Http.Response;
using Nexus.Http.Server.SplitHttp.Request;
using NexusGit.NexusGit.Projects;

namespace NexusGit.NexusGit.GetHandlers
{
    /*
     * Class representing a local pull HTTP request handler.
     */
    public class LocalPull: SplitClientRequestHandler
    {
        private IProject Project;
        
        /*
         * Creates a local pull request object.
         */
        public LocalPull(IProject project)
        {
            this.Project = project;
        }
        
        /*
         * Returns a response for a given request.
         */
        public override HttpResponse GetCompleteResponseData(HttpRequest request)
        {
            // Get the partitions.
            var partitions = this.Project.ReadProjectStructure();
            
            // Serialize the partitions.
            var partitionData = partitions.Serialize();

            // Return a response.
            return HttpResponse.CreateSuccessResponse(partitionData);
        }
    }
}