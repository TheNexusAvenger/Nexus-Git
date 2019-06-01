/*
 * TheNexusAvenger
 *
 * Handles getting the project structure partitions from the current project.
 */

using NexusGit.Git;
using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;

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
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Create the repository.
            Repository repository = new Repository();
            
            // Return the response.
            GitResponse response = repository.Status();
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}