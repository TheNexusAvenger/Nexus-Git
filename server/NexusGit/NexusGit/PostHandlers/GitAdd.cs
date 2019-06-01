/*
 * TheNexusAvenger
 *
 * Handles adding tracked files.
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
    public class GitAdd : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Get the body.
            FileList files = JsonConvert.DeserializeObject<FileList>(request.GetBody());
            
            // Create the repository.
            Repository repository = new Repository();
            
            // Return the response.
            GitResponse response = repository.Add(files);
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}