/*
 * TheNexusAvenger
 *
 * Handles getting the project structure partitions from the current project.
 */

using NexusGit.Git;
using Nexus.Http.Server.Http.Request;
using Nexus.Http.Server.Http.Response;
using Nexus.Http.Server.SplitHttp.Request;

namespace NexusGit.NexusGit.GetHandlers
{
    /*
     * Class representing a Git status HTTP request handler.
     */
    public class GitStatus : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override HttpResponse GetCompleteResponseData(HttpRequest request)
        {
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.Status();
            return HttpResponse.CreateSuccessResponse(response.ToJson());
        }
    }
}