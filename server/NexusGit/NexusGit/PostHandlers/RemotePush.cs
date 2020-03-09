/*
 * TheNexusAvenger
 *
 * Handles pushing to remote repositories.
 */

using Nexus.Http.Server.Http.Request;
using Nexus.Http.Server.Http.Response;
using Nexus.Http.Server.SplitHttp.Request;
using NexusGit.Git;

namespace NexusGit.NexusGit.PostHandlers
{
    /*
     * Class representing a remote push HTTP request handler.
     */
    public class RemotePush : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override HttpResponse GetCompleteResponseData(HttpRequest request)
        {
            // Get the arguments.
            var remote = request.GetURL().GetParameter("remote");
            if (remote == null)
            {
                remote = "origin";
            }
            var branch = request.GetURL().GetParameter("branch");
            if (branch == null)
            {
                branch = "master";
            }
            var force = request.GetURL().GetParameter("force");
            if (force == null)
            {
                force = "false";
            }
            
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.RemotePush(remote,branch,force.ToLower() == "true");
            return HttpResponse.CreateSuccessResponse(response.ToJson());
        }
    }
}