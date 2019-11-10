/*
 * TheNexusAvenger
 * 
 * Gets the local branches in a repository.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing a local branches action.
     */
    public class LocalBranches : RepositoryAction
    {
        private string Command;

        /*
         * Creates a local branches action object.
         */
        public LocalBranches(Repository repository) : base(repository)
        {
            this.Command = "branch";
        }

        /*
         * Returns the command to run.
         */
        public override string GetCommand()
        {
            return this.Command;
        }

        /*
         * Performs the action and returns the response.
         */
        public override GitResponse CompleteAction(ExecutableOutput executableOutput)
        {
            // Create the response.
            var response = new GitResponse();
            
            // Add the branches.
            foreach (var line in executableOutput)
            {
                if (line != "")
                {
                    response.AddResponse(line);
                }
            }

            // Return the response.
            return response;
        }
    }
}