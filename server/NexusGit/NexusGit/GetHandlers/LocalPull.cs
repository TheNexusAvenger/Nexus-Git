/*
 * TheNexusAvenger
 *
 * Handles pulling the project structure from the file system to Roblox Studio.
 */

using NexusGit.Http.Request;
using NexusGit.NexusGit.Projects;
using NexusGit.RobloxInstance;
using NexusGit.SplitRequestHttp;

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
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Get the partitions.
            var partitions = this.Project.ReadProjectStructure();
            
            // Serialize the partitions.
            var partitionData = partitions.Serialize();

            // Return a response.
            return Response.CreateSuccessResponse(partitionData);
        }
    }
}