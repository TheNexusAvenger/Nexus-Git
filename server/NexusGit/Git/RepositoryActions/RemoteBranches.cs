/*
 * TheNexusAvenger
 * 
 * Gets the remote branches in a repository.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing a remote branches action.
     */
    public class RemoteBranches : RepositoryAction
    {
        private string Command;

        /*
         * Creates a local branches action object.
         */
        public RemoteBranches(Repository repository) : base(repository)
        {
            // Fetch all the remotes.
            foreach (string line in this.GetRepository().ExecuteCommand("fetch"))
            {
                if (line != "")
                {
                    this.GetRepository().ExecuteCommand("fetch " + line);
                }
            }
            
            // Set up the command.
            this.Command = "branch -r";
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
            GitResponse response = new GitResponse();
            
            // Add the branches.
            foreach (string line in executableOutput)
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