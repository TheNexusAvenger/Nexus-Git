/*
 * TheNexusAvenger
 *
 * Handles checkout for local branches.
 */

using Newtonsoft.Json;
using NexusGit.Git;
using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit.PostHandlers
{
    /*
     * Class representing an add HTTP request handler.
     */
    public class LocalCheckout : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Get the body.
            string branch = request.GetBody();
            
            // Create the repository.
            Repository repository = new Repository();
            
            // Return the response.
            GitResponse response = repository.LocalCheckout(branch);
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}