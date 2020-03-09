/*
 * TheNexusAvenger
 *
 * Handles getting the local branches.
 */

using Nexus.Http.Server.Http.Request;
using Nexus.Http.Server.Http.Response;
using Nexus.Http.Server.SplitHttp.Request;
using NexusGit.Git;

namespace NexusGit.NexusGit.GetHandlers
{
    /*
     * Class representing a local branches HTTP request handler.
     */
    public class LocalBranches : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override HttpResponse GetCompleteResponseData(HttpRequest request)
        {
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.LocalBranches();
            return HttpResponse.CreateSuccessResponse(response.ToJson());
        }
    }
}