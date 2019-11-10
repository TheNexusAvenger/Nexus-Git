/*
 * TheNexusAvenger
 *
 * Handles commits to the repository.
 */

using Newtonsoft.Json;
using NexusGit.Git;
using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit.PostHandlers
{
    /*
     * Class representing the commit HTTP body.
     */
    public class GitCommitBody
    {
        public string message;
        public FileList files;
    }
    
    /*
     * Class representing a commit HTTP request handler.
     */
    public class GitCommit : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Get the arguments.
            var gitCommitData = JsonConvert.DeserializeObject<GitCommitBody>(request.GetBody());
            
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.Commit(gitCommitData.files,gitCommitData.message);
            return Response.CreateSuccessResponse(response.ToJson());
        }
    }
}