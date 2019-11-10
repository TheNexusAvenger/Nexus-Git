/*
 * TheNexusAvenger
 *
 * Handles checkout for local branches.
 */

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
            var branch = request.GetBody();
            
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.LocalCheckout(branch);
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}