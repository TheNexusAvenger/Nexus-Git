/*
 * TheNexusAvenger
 *
 * Handles pushing the project structure from Roblox Studio to the file system.
 */

using NexusGit.Http.Request;
using NexusGit.NexusGit.Projects;
using NexusGit.RobloxInstance;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit.PostHandlers
{
    /*
     * Class representing a local push HTTP request handler.
     */
    public class LocalPush : SplitClientRequestHandler
    {
        private IProject Project;
        
        /*
         * Creates a local push request.
         */
        public LocalPush(IProject project)
        {
            this.Project = project;
        }
        
        /*
         * Returns a response for a given request.
         */
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Parse the project structure.
            PartitionsBuilder builder = new PartitionsBuilder();
            Partitions partitions = (Partitions) builder.Deserialize(request.GetBody());
            
            // Write the project structure.
            this.Project.WriteProjectStructure(partitions);

            // Return a response.
            return Response.CreateSuccessResponse("Local push successful.");
        }
    }
}