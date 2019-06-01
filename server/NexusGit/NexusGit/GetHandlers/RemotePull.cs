/*
 * TheNexusAvenger
 *
 * Handles pulling from the remote repository.
 */

using NexusGit.Git;
using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit.GetHandlers
{
    /*
     * Class representing a remote pull HTTP request handler.
     */
    public class RemotePull : SplitClientRequestHandler
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
            
            // Create the repository.
            Repository repository = new Repository();
            
            // Return the response.
            GitResponse response = repository.RemotePull(remote,branch);
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}