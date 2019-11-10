/*
 * TheNexusAvenger
 *
 * Handles getting the project structure partitions from the current project.
 */

using System.Collections.Generic;
using Newtonsoft.Json;
using NexusGit.Http.Request;
using NexusGit.NexusGit.Projects;
using NexusGit.SplitRequestHttp;

namespace NexusGit.NexusGit.GetHandlers
{
    /*
     * Class representing a project partitions HTTP request handler.
     */
    public class ProjectPartitions : SplitClientRequestHandler
    {
        private IProject Project;
        
        /*
         * Creates a project partitions request object.
         */
        public ProjectPartitions(IProject project)
        {
            this.Project = project;
        }
        
        /*
         * Returns a response for a given request.
         */
        public override Response GetCompleteResponseData(HttpRequest request)
        {
            // Get the partitions.
            var partitions = this.Project.GetPartitions();
            
            // Return the partitions as a response.
            var partitionsJson = JsonConvert.SerializeObject(partitions);
            return Response.CreateSuccessResponse(partitionsJson);
        }
    }
}