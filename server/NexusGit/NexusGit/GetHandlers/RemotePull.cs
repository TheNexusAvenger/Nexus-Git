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
            
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.RemotePull(remote,branch);
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}