/*
 * TheNexusAvenger
 *
 * Handles commits to the repository.
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
        public override HttpResponse GetCompleteResponseData(HttpRequest request)
        {
            // Get the arguments.
            var gitCommitData = JsonConvert.DeserializeObject<GitCommitBody>(Encoding.UTF8.GetString(request.GetBody()));
            
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.Commit(gitCommitData.files,gitCommitData.message);
            return HttpResponse.CreateSuccessResponse(response.ToJson());
        }
    }
}