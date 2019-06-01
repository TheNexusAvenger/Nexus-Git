/*
 * TheNexusAvenger
 *
 * Handles pushing to remote repositories.
 */

using NexusGit.Git;
using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;

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
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Get the arguments.
            string remote = request.GetURL().GetParameter("remote");
            if (remote == null)
            {
                remote = "origin";
            }
            string branch = request.GetURL().GetParameter("branch");
            if (branch == null)
            {
                branch = "master";
            }
            string force = request.GetURL().GetParameter("force");
            if (force == null)
            {
                force = "false";
            }
            
            // Create the repository.
            Repository repository = new Repository();
            
            // Return the response.
            GitResponse response = repository.RemotePush(remote,branch,force.ToLower() == "true");
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}