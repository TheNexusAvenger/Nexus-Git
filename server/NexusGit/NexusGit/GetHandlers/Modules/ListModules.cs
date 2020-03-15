/*
 * TheNexusAvenger
 *
 * Handles getting the modules from the current project.
 */

using Nexus.Http.Server.Http.Request;
using Nexus.Http.Server.Http.Response;
using Nexus.Http.Server.SplitHttp.Request;
using NexusGit.Git;

namespace NexusGit.NexusGit.GetHandlers.Modules
{
    public class ListModules : SplitClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        public override HttpResponse GetCompleteResponseData(HttpRequest request)
        {
            // Create the repository.
            var repository = new Repository();
            
            // Return the response.
            var response = repository.ListSubmodules();
            return HttpResponse.CreateSuccessResponse(response.ToJson());
        }
    }
}