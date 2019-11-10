/*
 * TheNexusAvenger
 *
 * Handles checkout for remote branches.
 */

using Newtonsoft.Json;
using NexusGit.Git;
using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit.PostHandlers
{
    /*
     * Class representing the remote checkout HTTP body.
     */
    public class RemoteCheckoutBody
    {
        public string localBranch;
        public string remote;
        public string branch;
    }
    
    /*
     * Class representing a commit HTTP request handler.
     */
    public class RemoteCheckout : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Get the arguments.
            var gitCommitData = JsonConvert.DeserializeObject<RemoteCheckoutBody>(request.GetBody());
            
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.RemoteCheckout(gitCommitData.localBranch,gitCommitData.remote,gitCommitData.branch);
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}