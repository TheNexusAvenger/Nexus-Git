/*
 * TheNexusAvenger
 *
 * Handles getting the commits.
 */

using System.Collections.Generic;
using Newtonsoft.Json;
using NexusGit.Git;
using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit.GetHandlers
{
    /*
     * Class representing a commit.
     */
    public class Commit
    {
        public string id;
        public string message;
    }
    
    /*
     * Class representing a remote branches HTTP request handler.
     */
    public class ListCommits : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Create the repository.
            var repository = new Repository();
            
            // Get and parse response.
            var response = repository.ListCommits(request.GetURL().GetParameter("remote"),request.GetURL().GetParameter("branch"));
            var commits = new List<Commit>();
            Commit currentCommit = null;
            foreach (var line in response)
            {
                if (line.Length > 8 && line.Substring(0, 6) == "commit")
                {
                    currentCommit = new Commit();
                    currentCommit.id = line.Substring(7);
                    currentCommit.message = "";
                    commits.Add(currentCommit);
                }
                else if (currentCommit != null && line.Length > 4 && line.Substring(0, 4) != "Auth" && line.Substring(0, 4) != "Date")
                {
                    currentCommit.message = (currentCommit.message + "\n" + line).Trim();
                }
            }
            
            
            // Return the response.
            return Response.CreateSuccessResponse(JsonConvert.SerializeObject(commits,Formatting.None));
        }
    }
}