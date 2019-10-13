/*
 * TheNexusAvenger
 *
 * Handles getting the remotes.
 */

using NexusGit.Git;
using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit.GetHandlers
{
    /*
     * Class representing a remote branches HTTP request handler.
     */
    public class ListRemotes : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Create the repository.
            Repository repository = new Repository();
            
            // Return the response.
            GitResponse response = repository.ListRemotes();
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}