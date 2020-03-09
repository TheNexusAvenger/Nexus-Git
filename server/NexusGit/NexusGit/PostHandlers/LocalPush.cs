/*
 * TheNexusAvenger
 *
 * Handles pushing the project structure from Roblox Studio to the file system.
 */

using System.Text;
using Nexus.Http.Server.Http.Request;
using Nexus.Http.Server.Http.Response;
using Nexus.Http.Server.SplitHttp.Request;
using NexusGit.NexusGit.Projects;
using NexusGit.RobloxInstance;

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
        public override HttpResponse GetCompleteResponseData(HttpRequest request)
        {
            // Parse the project structure.
            var builder = new PartitionsBuilder();
            var partitions = (Partitions) builder.Deserialize(Encoding.UTF8.GetString(request.GetBody()));
            
            // Write the project structure.
            this.Project.WriteProjectStructure(partitions);

            // Return a response.
            return HttpResponse.CreateSuccessResponse("Local push successful.");
        }
    }
}