/*
 * TheNexusAvenger
 *
 * Handles getting the local branches.
 */

using NexusGit.Git;
using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;

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
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Create the repository.
            Repository repository = new Repository();
            
            // Return the response.
            GitResponse response = repository.LocalBranches();
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}