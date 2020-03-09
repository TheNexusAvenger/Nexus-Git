/*
 * TheNexusAvenger
 *
 * Handles checkout for remote branches.
 */

using System.Text;
using Newtonsoft.Json;
using Nexus.Http.Server.Http.Request;
using Nexus.Http.Server.Http.Response;
using Nexus.Http.Server.SplitHttp.Request;
using NexusGit.Git;

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
        public override HttpResponse GetCompleteResponseData(HttpRequest request)
        {
            // Get the arguments.
            var gitCommitData = JsonConvert.DeserializeObject<RemoteCheckoutBody>(Encoding.UTF8.GetString(request.GetBody()));
            
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.RemoteCheckout(gitCommitData.localBranch,gitCommitData.remote,gitCommitData.branch);
            return HttpResponse.CreateSuccessResponse(response.ToJson());
        }
    }
}