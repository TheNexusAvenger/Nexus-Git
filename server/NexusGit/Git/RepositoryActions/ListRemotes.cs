/*
 * TheNexusAvenger
 * 
 * Gets the remotes in a repository.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing a list remotes action.
     */
    public class ListRemotes : RepositoryAction
    {
        private string Command;

        /*
         * Creates a list remotes action object.
         */
        public ListRemotes(Repository repository) : base(repository)
        {
            this.Command = "remote";
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
            
            // Add the remotes.
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