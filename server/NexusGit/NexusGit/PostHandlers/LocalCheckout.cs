/*
 * TheNexusAvenger
 *
 * Handles checkout for local branches.
 */

using System.Text;
using Nexus.Http.Server.Http.Request;
using Nexus.Http.Server.Http.Response;
using Nexus.Http.Server.SplitHttp.Request;
using NexusGit.Git;

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
        public override HttpResponse GetCompleteResponseData(HttpRequest request)
        {
            // Get the body.
            var branch = Encoding.UTF8.GetString(request.GetBody());
            
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.LocalCheckout(branch);
            return HttpResponse.CreateSuccessResponse(response.ToJson());
        }
    }
}