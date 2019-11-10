/*
 * TheNexusAvenger
 *
 * Handles getting the remote branches.
 */

using NexusGit.Git;
using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit.GetHandlers
{
    /*
     * Class representing a remote branches HTTP request handler.
     */
    public class RemoteBranches : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.RemoteBranches();
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}